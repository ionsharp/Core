using System;
using System.IO;
using System.Net;
using System.Text;

namespace Imagin.Common.Web
{
    /// <summary>
    /// Fetches streamed content.
    /// </summary>
    public class FetchStream
    {
        #region Properties
        /// <summary>
        /// Gets the response.
        /// </summary>
        public HttpWebResponse Response { get; private set; }

        /// <summary>
        /// Gets the response data.
        /// </summary>
        public byte[] ResponseData { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public void Load(string url)
        {
            try
            {
                var req = HttpWebRequest.Create(url) as HttpWebRequest;
                req.AllowAutoRedirect = false;

                Response = req.GetResponse() as HttpWebResponse;
                switch (Response.StatusCode)
                {
                    case HttpStatusCode.Found:
                        // This is a redirect to an error page, so ignore.
                        //Console.WriteLine("Found (302), ignoring ");
                        break;

                    case HttpStatusCode.OK:
                        // This is a valid page.
                        using (var sr = Response.GetResponseStream())
                        using (var ms = new MemoryStream())
                        {
                            for (int b; (b = sr.ReadByte()) != -1;)
                                ms.WriteByte((byte)b);
                            ResponseData = ms.ToArray();
                        }
                        break;

                    default:
                        // This is unexpected.
                        //Console.WriteLine(Response.StatusCode);
                        break;
                }
            }
            catch (WebException ex)
            {
                //Console.WriteLine(":Exception " + ex.Message);
                Response = ex.Response as HttpWebResponse;
            }
        }

        /// <summary>
        /// Gets the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static byte[] Get(string url)
        {
            var f = new Fetch();
            f.Load(url);
            return f.ResponseData;
        }

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            var encoder = string.IsNullOrEmpty(Response.ContentEncoding) ? Encoding.UTF8 : Encoding.GetEncoding(Response.ContentEncoding);
            if (ResponseData == null)
                return string.Empty;
            return encoder.GetString(ResponseData);
        }
        #endregion
    }
}
