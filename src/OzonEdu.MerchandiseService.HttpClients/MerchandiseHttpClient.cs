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
        
        public async Task<MerchSetResponse> QueryMerchSet(int merchPackIndex, int size, CancellationToken token)
        {
            string requestUrl = string.Concat("api/merchandise", $"?merchPackIndex={merchPackIndex}", $"&size={size}");
            using var response = await _httpClient.GetAsync(requestUrl, token);
            var body = await response.Content.ReadAsStringAsync(token);
            return JsonSerializer.Deserialize<MerchSetResponse>(body);
        }

        public async Task<List<MerchSetResponse>> RetrieveIssuedMerchSetsInformation(int employeeId, CancellationToken token)
        {
            string requestUrl = string.Concat("api/merchandise", $"?employeeId={employeeId}");
            using var response = await _httpClient.GetAsync(requestUrl, token);
            var body = await response.Content.ReadAsStringAsync(token);
            return JsonSerializer.Deserialize<List<MerchSetResponse>>(body);
        }
    }
}