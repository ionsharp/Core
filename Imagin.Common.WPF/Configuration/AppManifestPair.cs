namespace Imagin.Common.Configuration
{
    public struct AppManifestPair
    {
        public readonly AppManifest Local;

        public readonly AppManifest Remote;

        public AppManifestPair(AppManifest local, AppManifest remote)
        {
            Local = local;
            Remote = remote;
        }
    }
}