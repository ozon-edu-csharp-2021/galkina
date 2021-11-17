using System;

namespace OzonEdu.MerchandiseService.Domain.Exceptions
{
    public class MerchStatusException : Exception
    {
        public MerchStatusException()
        {
        }

        public MerchStatusException(string message) : base(message)
        {
        }

        public MerchStatusException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}