using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace IGCIT_Helper {
    class IGCITHttpClient: HttpClient {
        private static IGCITHttpClient _instance = null;
        public static IGCITHttpClient Instance {
            get {
                _instance = _instance ?? new IGCITHttpClient();

                return _instance;
            }

            private set {}
        }

        private IGCITHttpClient(): base() {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12;

            BaseAddress = new Uri("https://www.musicianwall.altervista.org");
            DefaultRequestHeaders.Accept.Clear();
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
