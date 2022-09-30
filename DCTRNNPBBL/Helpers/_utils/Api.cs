/**
 * 
 * Author       :: Basilius Bias Astho Christyono
 * Phone        :: (+62) 889 236 6466
 * 
 * Department   :: IT SD 03
 * Mail         :: bias@indomaret.co.id
 * 
 * Catatan      :: External API Call
 *              :: Harap Didaftarkan Ke DI Container
 * 
 */

using System;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace DCTRNNPBBL.Helpers._utils {

    public interface IApi {
        T JsonToObj<T>(string j2o);
        string ObjectToJson(object body);
        Task<HttpResponseMessage> GetData(string urlPath);
        Task<HttpResponseMessage> DeleteData(string urlPath);
        Task<HttpResponseMessage> PostData(string urlPath, string jsonBody);
        Task<HttpResponseMessage> PutData(string urlPath, string jsonBody);
    }

    public class CApi : IApi {

        public CApi () { }

        public T JsonToObj<T>(string j2o) {
            return JsonConvert.DeserializeObject<T>(j2o);
        }

        public string ObjectToJson(object o2j) {
            return JsonConvert.SerializeObject(o2j);
        }

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

        public async Task<HttpResponseMessage> PostData(string urlPath, string jsonBody) {
            Uri uri = new Uri(urlPath);
            HttpContent httpContent = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage {
                Method = HttpMethod.Post,
                RequestUri = uri,
                Content = httpContent
            };
            HttpClient httpClient = new HttpClient();
            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async Task<HttpResponseMessage> PutData(string urlPath, string jsonBody) {
            Uri uri = new Uri(urlPath);
            HttpContent httpContent = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");
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
