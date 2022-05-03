using System;
using System.Xml.Linq;

namespace Imagin.Common.Configuration
{
    public struct AppManifest
    {
        Version framework;
        public Version Framework => framework;

        readonly string path;
        public string Path => path;

        DateTime? published;
        public DateTime? Published => published;

        string securityToken;
        public string SecurityToken => securityToken;

        readonly long size;
        public long Size => size;

        readonly Stability stability;
        public Stability Stability => stability;

        Version version;
        public Version Version => version;

        public AppManifest(string Path)
        {
            framework = default;
            stability = default;
            version = default;
            path = Path;
            published = default;
            securityToken = default;
            size = default;
        }

        public bool Load(string Data)
        {
            try
            {
                var xml = XDocument.Parse(Data);
                if (xml.Root.Name.LocalName != "App")
                    return false;

                version = new Version(xml.Root.Element("Version").Value);

                framework = new Version(xml.Root.Element("Framework").Value);

                var stability = Stability.Unknown;
                stability = !Enum.TryParse(xml.Root.Element("Type").Value, out stability) ? Stability.Unknown : Stability;

                securityToken = xml.Root.Element("SecurityToken").Value;

                var element = xml.Root.Element("Published");
                if (element != null)
                {
                    DateTime Published;
                    DateTime.TryParse(element.Value, out Published);
                    published = Published;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}