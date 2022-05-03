using Imagin.Common.Analytics;
using Imagin.Common.Data;
using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System;
using System.IO;
using System.Security.Cryptography;

namespace Imagin.Common.Text
{
    /// <remarks>
    /// Rijndael is strong and available on all platforms. 
    /// </remarks>
    public static class Converter
    {
        [DisplayName(nameof(Action))]
        [Serializable]
        public class Action : BaseLockable
        {
            [Serializable]
            public enum Types
            {
                Decrypt,
                Encrypt
            }

            [Flags]
            [Serializable]
            public enum Targets
            {
                [Hidden]
                None = 0,
                FileContents = 1,
                FileNames = 2,
                FolderNames = 4,
                [Hidden]
                All = FileContents | FileNames | FolderNames
            }

            Types type = Types.Encrypt;
            [Category("General")]
            [Description("Whether or not to encrypt or decrypt.")]
            [Featured]
            [Locked]
            public Types Type
            {
                get => type;
                set => this.Change(ref type, value);
            }

            Targets target = Targets.None;
            [Category("General")]
            [Description("The type of stuff to encrypt or decrypt.")]
            [Locked]
            [Style(EnumStyle.FlagSelect)]
            public Targets Target
            {
                get => target;
                set => this.Change(ref target, value);
            }

            SymmetricAlgorithm algorithm = SymmetricAlgorithm.Rijndael;
            [Category("General")]
            [Description("The type of algorithm involved with encrypting or decrypting something.")]
            [Locked]
            public SymmetricAlgorithm Algorithm
            {
                get => algorithm;
                set => this.Change(ref algorithm, value);
            }

            Encoding encoding = Encoding.ASCII;
            [Category("General")]
            [Description("The type of encoding involved with reading text files.")]
            [Locked]
            public Encoding Encoding
            {
                get => encoding;
                set => this.Change(ref encoding, value);
            }

            string password = string.Empty;
            [Category("General")]
            [Description("The password used to encrypt or decrypt something.")]
            [Locked]
            public string Password
            {
                get => password;
                set => this.Change(ref password, value);
            }

            public Action() : base() { }

            public override string ToString() => target == Targets.None ? "Copy" : $"{type} {target.ToString().SplitCamel().ToLower()}";
        }

        static byte[] DefaultSalt
        {
            get
            {
                return new byte[]
                {
                    0x49, 0x76, 0x61, 0x6e,
                    0x20, 0x4d, 0x65, 0x64,
                    0x76, 0x65, 0x64, 0x65,
                    0x76
                };
            }
        }

        static System.Security.Cryptography.SymmetricAlgorithm GetAlgorithm(SymmetricAlgorithm algorithm)
        {
            return algorithm switch
            {
                SymmetricAlgorithm.Aes => Aes.Create(),
                SymmetricAlgorithm.DES => DES.Create(),
                SymmetricAlgorithm.RC2 => RC2.Create(),
                SymmetricAlgorithm.Rijndael => Rijndael.Create(),
                SymmetricAlgorithm.TripleDES => TripleDES.Create(),
                _ => default,
            };
        }

        //...

        public static string EscapeUri(string uri, Action.Types actionType)
        {
            switch (actionType)
            {
                case Action.Types.Decrypt:
                    uri = uri.Replace('-', '/');
                    return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(uri));
                case Action.Types.Encrypt:
                    uri = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(uri));
                    return uri.Replace('/', '-');
            }
            return uri;
        }

        //...

        public static string FormatText(string text, Action action)
        {
            switch (action.Type)
            {
                case Action.Types.Decrypt:
                    text = DecryptText(EscapeUri(text, action.Type), action.Password, action.Algorithm, action.Encoding);
                    break;
                case Action.Types.Encrypt:
                    text = EscapeUri(EncryptText(text, action.Password, action.Algorithm, action.Encoding), action.Type);
                    break;
            }
            return text;
        }

        public static string FormatUri(string uriHead, string uriTail, ItemType targetType, Action action)
        {
            var result = uriHead;

            var fi = action.Target.HasFlag(Action.Targets.FileNames);
            var fo = action.Target.HasFlag(Action.Targets.FolderNames);

            var pieces = uriTail.Split(Array<char>.New('\\'), StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0, Count = pieces.Length; i < Count; i++)
            {
                var piece = pieces[i];

                if
                (
                    (
                        targetType == ItemType.File
                        &&
                        (
                            (fo && i < Count - 1)
                            ||
                            (fi && i == Count - 1)
                        )
                    )
                    ||
                    targetType == ItemType.Folder && fo
                )
                {
                    piece = FormatText(piece, action);
                }

                result += @"\{0}".F(piece);
            }

            return result;
        }

        //...

        public static byte[] Decrypt(byte[] data, byte[] key, byte[] iv, SymmetricAlgorithm algorithm = SymmetricAlgorithm.Rijndael)
        {
            // Create a MemoryStream that is going to accept the decrypted bytes 
            var Stream = new MemoryStream();

            var a = GetAlgorithm(algorithm);

            // Now set the key and the IV. 
            // We need the IV (Initialization Vector) because the algorithm
            // is operating in its default 
            // mode called CBC (Cipher Block Chaining). The IV is XORed with
            // the first block (8 byte) 
            // of the data after it is decrypted, and then each decrypted
            // block is XORed with the previous 
            // cipher block. This is done to make encryption more secure. 
            // There is also a mode called ECB which does not need an IV,
            // but it is much less secure. 
            a.Key = key;
            a.IV = iv;

            // Create a CryptoStream through which we are going to be pumping our data. 
            // CryptoStreamMode.Write means that we are going to be writing data to the stream and the output will be written in the MemoryStream we have provided. 
            var CryptoStream = new CryptoStream(Stream, a.CreateDecryptor(), CryptoStreamMode.Write);

            // Write the data and make it do the decryption 
            CryptoStream.Write(data, 0, data.Length);

            // Close the crypto stream (or do FlushFinalBlock). 
            // This will tell it that we have done our decryption and there is no more data coming in, and it is now a good time to remove the padding and finalize the decryption process. 
            CryptoStream.Close();

            // Now get the decrypted data from the MemoryStream. 
            // Some people make a mistake of using GetBuffer() here,
            // which is not the right way. 
            return Stream.ToArray();
        }

        public static byte[] Decrypt(byte[] data, string password, SymmetricAlgorithm algorithm = SymmetricAlgorithm.Rijndael)
        {
            // We need to turn the password into Key and IV. 
            // We are using salt to make it harder to guess our key
            // using a dictionary attack - 
            // trying to guess a password by enumerating all possible words. 
            var pdb = new PasswordDeriveBytes(password, DefaultSalt);

            // Now get the key/IV and do the Decryption using the 
            //function that accepts byte arrays. 
            // Using PasswordDeriveBytes object we are first getting
            // 32 bytes for the Key 
            // (the default Rijndael key length is 256bit = 32bytes)
            // and then 16 bytes for the IV. 
            // IV should always be the block size, which is by default
            // 16 bytes (128 bit) for Rijndael. 
            // If you are using DES/TripleDES/RC2 the block size is
            // 8 bytes and so should be the IV size. 

            // You can also read KeySize/BlockSize properties off the
            // algorithm to find out the sizes. 
            return Decrypt(data, pdb.GetBytes(32), pdb.GetBytes(16), algorithm);
        }

        public static byte[] Encrypt(byte[] data, byte[] key, byte[] iv, SymmetricAlgorithm algorithm = SymmetricAlgorithm.Rijndael)
        {
            // Create a MemoryStream to accept the encrypted bytes 
            var Stream = new MemoryStream();

            var a = GetAlgorithm(algorithm);

            // Now set the key and the IV. 
            // We need the IV (Initialization Vector) because the algorithm is operating in its default mode called CBC (Cipher Block Chaining).
            // The IV is XORed with the first block (8 byte) of the data before it is encrypted, and then each encrypted block is XORed with the following block of plaintext.
            // This is done to make encryption more secure. 

            // There is also a mode called ECB which does not need an IV, but it is much less secure. 
            a.Key = key;
            a.IV = iv;

            // Create a CryptoStream through which we are going to be pumping our data. 
            // CryptoStreamMode.Write means that we are going to be writing data to the stream and the output will be written in the MemoryStream we have provided. 
            var CryptoStream = new CryptoStream(Stream, a.CreateEncryptor(), CryptoStreamMode.Write);

            // Write the data and make it do the encryption 
            CryptoStream.Write(data, 0, data.Length);

            // Close the crypto stream (or do FlushFinalBlock). 
            // This will tell it that we have done our encryption and there is no more data coming in, and it is now a good time to apply the padding and finalize the encryption process. 
            CryptoStream.Close();

            // Now get the encrypted data from the MemoryStream.
            // Some people make a mistake of using GetBuffer() here, which is not the right way. 
            return Stream.ToArray();
        }

        public static byte[] Encrypt(byte[] data, string password, SymmetricAlgorithm algorithm = SymmetricAlgorithm.Rijndael)
        {
            // We need to turn the password into Key and IV. 
            // We are using salt to make it harder to guess our key
            // using a dictionary attack - 
            // trying to guess a password by enumerating all possible words. 
            var pdb = new PasswordDeriveBytes(password, DefaultSalt);

            // Now get the key/IV and do the encryption using the function
            // that accepts byte arrays. 
            // Using PasswordDeriveBytes object we are first getting
            // 32 bytes for the Key 
            // (the default Rijndael key length is 256bit = 32bytes)
            // and then 16 bytes for the IV. 
            // IV should always be the block size, which is by default
            // 16 bytes (128 bit) for Rijndael. 
            // If you are using DES/TripleDES/RC2 the block size is 8
            // bytes and so should be the IV size. 
            // You can also read KeySize/BlockSize properties off the
            // algorithm to find out the sizes. 
            return Encrypt(data, pdb.GetBytes(32), pdb.GetBytes(16), algorithm);

        }

        //...

        public static string DecryptText(string text, string password, SymmetricAlgorithm algorithm = SymmetricAlgorithm.Rijndael, Encoding encoding = Encoding.Unicode)
        {
            if (password.NullOrEmpty())
                throw new ArgumentException("Password not specified.");

            //First we need to turn the input string into a byte array; we assume that base64 encoding was used 
            var bytes = Convert.FromBase64String(text);

            //Then, we need to turn the password into Key and IV; we are using salt to make it harder to guess our key using a dictionary attack (trying to guess a password by enumerating all possible words). 
            var deriveBytes = new PasswordDeriveBytes(password, DefaultSalt);

            // Now get the key/IV and do the decryption using the function that accepts byte arrays. 
            // Using PasswordDeriveBytes object we are first getting 32 bytes for the Key 
            // (the default Rijndael key length is 256bit = 32bytes) and then 16 bytes for the IV. 
            // IV should always be the block size, which is by default 16 bytes (128 bit) for Rijndael. 
            // If you are using DES/TripleDES/RC2 the block size is 8 bytes and so should be the IV size. 
            // You can also read KeySize/BlockSize properties off the algorithm to find out the sizes. 
            var result = Decrypt(bytes, deriveBytes.GetBytes(32), deriveBytes.GetBytes(16), algorithm);

            // Now we need to turn the resulting byte array into a string; a common mistake is using an Encoding class for that.
            // It does not work because not all byte values can be represented by characters. 
            // We are going to be using Base64 encoding that is designed exactly for what we are trying to do. 
            return encoding.Convert().GetString(result);
        }

        public static string EncryptText(string text, string password, SymmetricAlgorithm algorithm = SymmetricAlgorithm.Rijndael, Encoding encoding = Encoding.Unicode)
        {
            if (password.NullOrEmpty())
                throw new ArgumentException("Password not specified.");

            //First we need to turn the input string into a byte array; we assume that base64 encoding was used 
            var bytes = encoding.Convert().GetBytes(text);

            //Then, we need to turn the password into Key and IV; we are using salt to make it harder to guess our key using a dictionary attack (trying to guess a password by enumerating all possible words). 
            var deriveBytes = new PasswordDeriveBytes(password, DefaultSalt);

            // Now get the key/IV and do the decryption using the function that accepts byte arrays. 
            // Using PasswordDeriveBytes object we are first getting 32 bytes for the Key 
            // (the default Rijndael key length is 256bit = 32bytes) and then 16 bytes for the IV. 
            // IV should always be the block size, which is by default 16 bytes (128 bit) for Rijndael. 
            // If you are using DES/TripleDES/RC2 the block size is 8 bytes and so should be the IV size. 
            // You can also read KeySize/BlockSize properties off the algorithm to find out the sizes. 
            var result = Encrypt(bytes, deriveBytes.GetBytes(32), deriveBytes.GetBytes(16), algorithm);

            // Now we need to turn the resulting byte array into a string; a common mistake is using an Encoding class for that.
            // It does not work because not all byte values can be represented by characters. 
            // We are going to be using Base64 encoding that is designed exactly for what we are trying to do. 
            return Convert.ToBase64String(result);
        }

        public static Result TryDecryptText(ref string result, string text, string password, SymmetricAlgorithm algorithm = SymmetricAlgorithm.Rijndael, Encoding encoding = Encoding.Unicode)
        {
            try
            {
                result = DecryptText(text, password, algorithm, encoding);
                return new Success();
            }
            catch (Exception e)
            {
                result = null;
                return new Error(e);
            }
        }

        public static Result TryEncryptText(ref string result, string text, string password, SymmetricAlgorithm algorithm = SymmetricAlgorithm.Rijndael, Encoding encoding = Encoding.Unicode)
        {
            try
            {
                result = EncryptText(text, password, algorithm, encoding);
                return new Success();
            }
            catch (Exception e)
            {
                result = null;
                return new Error(e);
            }
        }

        //...

        public class Callback
        {
            readonly Func<double, double, bool> Action;

            public Callback(Func<double, double, bool> action)
            {
                Action = action;
            }

            public bool Invoke(double a, double b) => Action.Invoke(a, b);
        }

        public static void DecryptFile(string source, string destination, string password, SymmetricAlgorithm algorithm = SymmetricAlgorithm.Rijndael, Encoding encoding = Encoding.Unicode, bool overwrite = false, Callback action = null)
        {
            if (password.NullOrEmpty())
                throw new ArgumentNullException("Password must contain at least one character.");

            if (overwrite && System.IO.File.Exists(destination))
                Imagin.Common.Storage.File.Long.Delete(destination);

            var size = 0d;
            if (action != null)
                size = new FileInfo(source).Length.Double();

            using (var inStream = new FileStream(source, FileMode.Open, FileAccess.Read))
            using (var outStream = new FileStream(destination, FileMode.OpenOrCreate, FileAccess.Write))
            {
                //Derive a key and an IV from the password and create an algorithm 
                var deriveBytes = new PasswordDeriveBytes(password, DefaultSalt);

                var a = GetAlgorithm(algorithm);
                a.Key = deriveBytes.GetBytes(32);
                a.IV = deriveBytes.GetBytes(16);

                //Create a CryptoStream through which to pump data; outStream will receive the decrypted bytes. 
                var cryptoStream = new CryptoStream(outStream, a.CreateDecryptor(), CryptoStreamMode.Write);

                //Initialize a buffer and process the file in chunks; this is done to avoid reading the whole file into memory, which can be huge. 
                var bufferLens = 4096;
                var buffer = new byte[bufferLens];
                int bytesRead;
                var sizeRead = 0L;

                do
                {
                    //Read a chunk of data from the file 
                    bytesRead = inStream.Read(buffer, 0, bufferLens);

                    //Decrypt it 
                    cryptoStream.Write(buffer, 0, bytesRead);

                    if (action != null)
                    {
                        sizeRead += bytesRead;
                        var cancel = action.Invoke(sizeRead.Double(), size);
                        if (cancel) break;
                    }
                }
                while (bytesRead != 0);

                //Close everything (this will also close the underlying outStream).
                cryptoStream.Close();
                inStream.Close();
            }
        }

        public static void EncryptFile(string source, string destination, string password, SymmetricAlgorithm algorithm = SymmetricAlgorithm.Rijndael, Encoding encoding = Encoding.Unicode, bool overwrite = false, Callback action = null)
        {
            if (password.NullOrEmpty())
                throw new ArgumentNullException("Password must contain at least one character.");

            if (overwrite && System.IO.File.Exists(destination))
                Imagin.Common.Storage.File.Long.Delete(destination);

            var size = 0d;
            if (action != null)
                size = new FileInfo(source).Length.Double();

            using (var inStream = new FileStream(source, FileMode.Open, FileAccess.Read))
            using (var outStream = new FileStream(destination, FileMode.OpenOrCreate, FileAccess.Write))
            {
                //Derive a key and an IV from the password and create an algorithm 
                var deriveBytes = new PasswordDeriveBytes(password, DefaultSalt);

                var a = GetAlgorithm(algorithm);
                a.Key = deriveBytes.GetBytes(32);
                a.IV = deriveBytes.GetBytes(16);

                //Create a CryptoStream through which to pump data; outStream will receive the encrypted bytes. 
                var cryptoStream = new CryptoStream(outStream, a.CreateEncryptor(), CryptoStreamMode.Write);

                //Initialize a buffer and process the file in chunks; this is done to avoid reading the whole file into memory, which can be huge. 
                var bufferLens = 4096;
                var buffer = new byte[bufferLens];

                int bytesRead;
                var sizeRead = 0L;

                do
                {
                    //Read a chunk of data from the file 
                    bytesRead = inStream.Read(buffer, 0, bufferLens);

                    //Encrypt it 
                    cryptoStream.Write(buffer, 0, bytesRead);

                    if (action != null)
                    {
                        sizeRead += bytesRead;
                        var cancel = action.Invoke(sizeRead.Double(), size);
                        if (cancel) break;
                    }
                }
                while (bytesRead != 0);

                //Close everything (this will also close the underlying outStream).
                cryptoStream.Close();
                inStream.Close();
            }
        }

        public static void CopyFile(string source, string destination, Encoding encoding = Encoding.Unicode, bool overwrite = false, Callback action = null)
        {
            if (overwrite && System.IO.File.Exists(destination))
                Storage.File.Long.Delete(destination);

            var size = 0d;
            if (action != null)
                size = new FileInfo(source).Length.Double();

            using (var inStream = new FileStream(source, FileMode.Open, FileAccess.Read))
            using (var outStream = new FileStream(destination, FileMode.OpenOrCreate, FileAccess.Write))
            {
                var bufferLens = 4096;
                var buffer = new byte[bufferLens];

                int bytesRead;
                var sizeRead = 0L;

                do
                {
                    bytesRead = inStream.Read(buffer, 0, bufferLens);
                    outStream.Write(buffer, 0, bytesRead);

                    if (action != null)
                    {
                        sizeRead += bytesRead;
                        var cancel = action.Invoke(sizeRead.Double(), size);
                        if (cancel) break;
                    }
                }
                while (bytesRead != 0);

                outStream.Close();
                inStream.Close();
            }
        }
    }
}