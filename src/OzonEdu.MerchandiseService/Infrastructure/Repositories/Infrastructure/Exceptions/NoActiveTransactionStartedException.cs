using System;

namespace OzonEdu.MerchandiseService.Infrastructure.Repositories.Infrastructure.Exceptions
{
    public class NoActiveTransactionStartedException : Exception
    {
        public NoActiveTransactionStartedException() : base() { }
        public NoActiveTransactionStartedException(string message) : base(message) { }
        public NoActiveTransactionStartedException(string message, Exception inner) : base(message, inner) { }
    }
}