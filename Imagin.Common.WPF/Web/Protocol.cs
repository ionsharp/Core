using System;

namespace Imagin.Common.Web
{
    [Serializable]
    public enum Protocol
    {
        [Abbreviation("FTP")]
        FileTransfer,
        [Abbreviation("SFTP")]
        SshFileTransfer,
        [Abbreviation("HTTP")]
        HypertextTransfer,
        [Abbreviation("OneDrive")]
        OneDrive,
        [Abbreviation("SCP")]
        SecureCopy,
        [Abbreviation("WebDAV")]
        WebDistributedAuthoringAndVersioning
    }
}