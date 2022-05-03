using Imagin.Common.Analytics;
using Imagin.Common.Serialization;
using System;

namespace Imagin.Common.Controls
{
    [Serializable]
    public class ExplorerWindowOptions : Base, ISerialize
    {
        enum Category
        {
            Explorer
        }

        [Hidden]
        public string FilePath { get; private set; }

        ExplorerOptions explorerOptions = new();
        [Category(Category.Explorer)]
        [DisplayName("Options")]
        public ExplorerOptions ExplorerOptions
        {
            get => explorerOptions;
            set => this.Change(ref explorerOptions, value);
        }

        double windowHeight = 420;
        [Hidden]
        public double WindowHeight
        {
            get => windowHeight;
            set => this.Change(ref windowHeight, value);
        }

        double windowWidth = 720;
        [Hidden]
        public double WindowWidth
        {
            get => windowWidth;
            set => this.Change(ref windowWidth, value);
        }

        public ExplorerWindowOptions() : base() { }

        public ExplorerWindowOptions(string filePath) : this()
        {
            FilePath = filePath;
        }

        public static Result Load(string filePath, out ExplorerWindowOptions data)
        {
            var result = BinarySerializer.Deserialize(filePath, out object options);
            data = options as ExplorerWindowOptions ?? new ExplorerWindowOptions(filePath);
            return result;
        }

        public Result Deserialize(string filePath, out object data) => BinarySerializer.Deserialize(filePath, out data);

        public Result Save() => Serialize(this);

        public Result Serialize(object data) => Serialize(FilePath, data);

        public Result Serialize(string filePath, object data) => BinarySerializer.Serialize(filePath, data);

        public override string ToString() => "Options";
    }
}
