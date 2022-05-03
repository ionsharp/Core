namespace Imagin.Common.Data
{
    public class Options : RemoteBinding
    {
        new RemoteBindingSource RemoteSource { set => base.RemoteSource = value; }

        public Options() : this(".") { }

        public Options(string path) : base(path) => RemoteSource = RemoteBindingSource.Options;
    }
}