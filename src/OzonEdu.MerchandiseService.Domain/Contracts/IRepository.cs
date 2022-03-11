using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.Contracts
{
    public interface IRepository<T> where T : IAggregationRoot
    {
        
    }
}