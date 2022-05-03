using System;

namespace Imagin.Common.Text
{
    [Serializable]
    public class Encryption : Base
    {
        SymmetricAlgorithm algorithm = SymmetricAlgorithm.Rijndael;
        [Description("The type of algorithm involved with encrypting or decrypting something.")]
        public SymmetricAlgorithm Algorithm
        {
            get => algorithm;
            set => this.Change(ref algorithm, value);
        }

        Encoding encoding = Encoding.ASCII;
        [Description("The type of encoding involved with reading text files.")]
        public Encoding Encoding
        {
            get => encoding;
            set => this.Change(ref encoding, value);
        }

        string password = string.Empty;
        [Description("The password used to encrypt or decrypt something.")]
        public string Password
        {
            get => password;
            set => this.Change(ref password, value);
        }

        public Encryption() : base() { }

        public override string ToString() => $"{algorithm}";

        public string Decrypt(string input)
        {
            try
            {
                return Converter.DecryptText(input, password, algorithm, encoding);
            }
            catch
            {
                return input;
            }
        }

        public string Encrypt(string input)
        {
            try
            {
                return Converter.EncryptText(input, password, algorithm, encoding);
            }
            catch
            {
                return input;
            }
        }
    }
}