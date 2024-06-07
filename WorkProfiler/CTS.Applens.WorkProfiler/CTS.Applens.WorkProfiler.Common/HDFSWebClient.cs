using System;
using System.Net;


namespace CTS.Applens.WorkProfiler.Common
{
    public class HDFSWebClient : WebClient
    {
        private WebRequest request = null;
        private bool allowAutoRedirect;

        /// <summary>
        /// AllowAutoRedirect
        /// </summary>
        public bool AllowAutoRedirect
        {
            get { return allowAutoRedirect; }
            set
            {
                allowAutoRedirect = value;

            }
        }

        /// <summary>
        /// GetWebRequest
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        protected override WebRequest GetWebRequest(Uri address)
        {
            this.request = base.GetWebRequest(address);

            if (this.request is HttpWebRequest)
            {
                ((HttpWebRequest)this.request).AllowAutoRedirect = this.allowAutoRedirect;
            }

            return this.request;
        }

        /// <summary>
        /// StatusCode
        /// </summary>
        /// <returns></returns>
        public HttpStatusCode StatusCode()
        {
            HttpStatusCode result;

            if (this.request == null)
            {
                throw (new InvalidOperationException("Unable to retrieve the status  code," +
                    " maybe you haven't made a request yet."));
            }

            HttpWebResponse response = base.GetWebResponse(this.request)
                                       as HttpWebResponse;

            if (response != null)
            {
                result = response.StatusCode;
            }
            else
            {
                throw (new InvalidOperationException("Unable to retrieve the status   code," +
                    " maybe you haven't made a request yet."));
            }

            return result;
        }
    }
}
