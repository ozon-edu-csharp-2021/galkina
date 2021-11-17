using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.HttpModels;

namespace OzonEdu.MerchandiseService.HttpClients
{
    public class MerchandiseHttpClient : IMerchandiseHttpClient
    {
        private readonly HttpClient _httpClient;

        public MerchandiseHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<MerchPackResponse> QueryMerchSet(long employeeId, int merchPackIndex, string size, CancellationToken token)
        {
            string requestUrl = string.Concat("api/merchandise", $"?employeeId={employeeId}", $"?merchPackIndex={merchPackIndex}", $"&size={size}");
            using var response = await _httpClient.GetAsync(requestUrl, token);
            var body = await response.Content.ReadAsStringAsync(token);
            return JsonSerializer.Deserialize<MerchPackResponse>(body);
        }

        public async Task<List<MerchPackResponse>> RetrieveIssuedMerchSetsInformation(long employeeId, CancellationToken token)
        {
            string requestUrl = string.Concat("api/merchandise", $"?employeeId={employeeId}");
            using var response = await _httpClient.GetAsync(requestUrl, token);
            var body = await response.Content.ReadAsStringAsync(token);
            return JsonSerializer.Deserialize<List<MerchPackResponse>>(body);
        }
    }
}