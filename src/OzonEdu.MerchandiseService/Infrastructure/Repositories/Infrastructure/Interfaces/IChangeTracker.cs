using System.Collections.Generic;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Infrastructure.Repositories.Infrastructure.Interfaces
{
    public interface IChangeTracker
    {
        IEnumerable<Entity> TrackedEntities { get; }
        
        public void Track(Entity entity);
    }
}