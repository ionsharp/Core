using Imagin.Common.Analytics;
using Imagin.Common.Serialization;
using System.Collections.Generic;

namespace Imagin.Common.Collections.Serialization
{
    public class BinaryWriter<T> : Writer<T>
    {
        public override bool Preserve { get; set; } = false;

        public BinaryWriter(string folderPath, string fileName, string fileExtension, string singleFileExtension, Limit limit) : base(folderPath, fileName, fileExtension, singleFileExtension, limit) { }

        public override Result Deserialize(string filePath, out object data) => BinarySerializer.Deserialize(filePath, out data);

        public override Result Serialize(string filePath, object data) => BinarySerializer.Serialize(filePath, new List<T>(this));
    }
}