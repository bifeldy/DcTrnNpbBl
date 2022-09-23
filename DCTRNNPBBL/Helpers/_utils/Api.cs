using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DCTRNNPBBL.Helpers._utils {

    public interface IApi {
        Task<HttpResponseMessage> GetData(string urlPath);
        Task<HttpResponseMessage> DeleteData(string urlPath);
        Task<HttpResponseMessage> PostData(string urlPath, object body);
        Task<HttpResponseMessage> PutData(string urlPath, object body);
    }

    public class CApi : IApi {

        public CApi () { }

        public async Task<HttpResponseMessage> GetData(string urlPath) {
            Uri uri = new Uri(urlPath);
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage {
                Method = HttpMethod.Get,
                RequestUri = uri
            };
            HttpClient httpClient = new HttpClient();
            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async Task<HttpResponseMessage> DeleteData(string urlPath) {
            Uri uri = new Uri(urlPath);
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage {
                Method = HttpMethod.Delete,
                RequestUri = uri
            };
            HttpClient httpClient = new HttpClient();
            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async Task<HttpResponseMessage> PostData(string urlPath, object body) {
            Uri uri = new Uri(urlPath);
            string json = JsonConvert.SerializeObject(body);
            HttpContent httpContent = new StringContent(json);
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage {
                Method = HttpMethod.Post,
                RequestUri = uri,
                Content = httpContent
            };
            HttpClient httpClient = new HttpClient();
            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async Task<HttpResponseMessage> PutData(string urlPath, object body) {
            Uri uri = new Uri(urlPath);
            string json = JsonConvert.SerializeObject(body);
            HttpContent httpContent = new StringContent(json);
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage {
                Method = HttpMethod.Put,
                RequestUri = uri,
                Content = httpContent
            };
            HttpClient httpClient = new HttpClient();
            return await httpClient.SendAsync(httpRequestMessage);
        }

    }

}
