using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Infrastructure.Models;
using OzonEdu.StockApi.Grpc;

namespace OzonEdu.MerchandiseService.Infrastructure.Handlers.Aggregate.RequestsToStockApi
{
    public static class RequestToStockApi
    {
        public static async Task<bool> DoesSkusAvailable(StockApiGrpc.StockApiGrpcClient stockServiceClient, List<long> skus, CancellationToken cancellationToken)
        {
            var skusRequest = new SkusRequest();
            skusRequest.Skus.AddRange(skus);
            var availabilityResponse =
                await stockServiceClient.GetStockItemsAvailabilityAsync(skusRequest,
                    cancellationToken: cancellationToken);
            
            Items available = JsonSerializer.Deserialize<Items>(availabilityResponse.ToString());
            
            for(int i = 0; i < available.items.Count; i++)
            {
                if(available.items[i].quantity < 1)
                    return false;
            }

            return true;
        }

        public static async Task<bool> ReserveSkus(StockApiGrpc.StockApiGrpcClient stockServiceClient, List<long> skus, CancellationToken cancellationToken)
        {
            var stockRequestItems = skus
                .Select(sku => new SkuQuantityItem()
                {
                    Sku = sku,
                    Quantity = 1
                }).ToArray();
            
            var giveOutItemsRequest = new GiveOutItemsRequest();
            giveOutItemsRequest.Items.AddRange(stockRequestItems);
            var reserveResponse =
                await stockServiceClient.GiveOutItemsAsync(giveOutItemsRequest,
                    cancellationToken: cancellationToken);
            
            string result = reserveResponse.Result.ToString();
            
            return result == "Successful";
        }
    }
}