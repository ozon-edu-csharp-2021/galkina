using System;
using System.Threading;
using System.Threading.Tasks;
using OzonEdu.MerchandiseService.Domain.Contracts;

public class UnitOfWorkStub : IUnitOfWork, IDisposable
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(1);
    }

    public Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }

    private bool disposed = false;
 
    public virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            this.disposed = true;
        }
    }
 
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}