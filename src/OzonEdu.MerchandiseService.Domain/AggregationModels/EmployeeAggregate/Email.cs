using System.Collections.Generic;
using System.Text.RegularExpressions;
using OzonEdu.MerchandiseService.Domain.Exceptions;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate
{
    public class Email: ValueObject
    {
        public string Value { get; }

        private Email(string emailString)
        {
            Value = emailString;
        }

        public static Email Create(string emailString)
        {
            if (emailString is not null && IsValidEmail(emailString))
            {
                return new Email(emailString);
            }
            
            throw new EmailInvalidException($"Email is invalid: {emailString}");
        }
        
        public override string ToString() => Value;
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        private static bool IsValidEmail(string emailString)
            => Regex.IsMatch(emailString, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
    }
}