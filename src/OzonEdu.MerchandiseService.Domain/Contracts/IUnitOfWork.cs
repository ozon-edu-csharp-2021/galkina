using System;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.AggregationModels.MerchPackAggregate;

namespace OzonEdu.MerchandiseService.Domain.Contracts
{
    public interface IUnitOfWork
    {
        ValueTask StartTransaction(CancellationToken token = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}