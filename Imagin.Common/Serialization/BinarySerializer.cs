using Imagin.Common.Analytics;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Imagin.Common.Serialization
{
    public class BinarySerializer
    {
        public static Result Deserialize<T>(string filePath, out T data)
        {
            Result result = null;
            data = default;

            var stream = default(FileStream);
            try
            {
                if (File.Exists(filePath))
                {
                    stream = new FileStream(filePath, FileMode.Open);
                    data = (T)new BinaryFormatter().Deserialize(stream);
                    result = new Success();
                }
            }
            catch (Exception e)
            {
                result = new Error(e);
                Log.Write<BinarySerializer>(result);
            }
            finally
            {
                stream?.Close();
            }
            return result;
        }

        public static Result Serialize(string filePath, object data)
        {
            Result result = null;

            var type = data.GetType();
            var fileStream = default(FileStream);

            try
            {
                var directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                fileStream = new FileStream(filePath, FileMode.Create);

                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, data);
                result = new Success();
            }
            catch (Exception e)
            {
                result = new Error(e);
                Log.Write<BinarySerializer>(result);
            }
            finally
            {
                fileStream?.Close();
            }
            return result;
        }
    }
}