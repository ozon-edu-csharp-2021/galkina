using System;
using System.Collections.Generic;
using System.Linq;
using OzonEdu.MerchandiseService.Domain.Exceptions;
using OzonEdu.MerchandiseService.Domain.Models;

namespace OzonEdu.MerchandiseService.Domain.AggregationModels.EmployeeAggregate
{
    public class Name: ValueObject
    {
        public string Value { get; }

        private Name(string name)
        {
            Value = name;
        }

        public static Name Create(string name)
        {
            if (name is not null && IsValidName(name))
            {
                return new Name(name);
            }
            
            throw new NameInvalidException($"Name is invalid: {name}");
        }
        
        public override string ToString() => Value;
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        private static bool IsValidName(string name)
        {
            if (name.Contains('-'))
            {
                string[] names = name.Split('-');
                
                if (names.Length == 2)
                    return IsValidName(names[0]) && IsValidName((names[1]));
                else
                    return false;
            }
                
            return name.Length > 1 && name.Length < 50 && name.All(c => Char.IsLetter(c));
        }
    }
}