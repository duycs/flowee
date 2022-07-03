using AppShareDomain.Models;
using AppShareServices.Models;
using System.ComponentModel.DataAnnotations;

namespace CustomerDomain.AgreegateModels.CustomerAgreegate
{
    public class Customer : Entity, IAggregateRoot
    {
        [MaxLength(250)]
        public string? FirstName { get; set; }
        [MaxLength(250)]
        public string? LastName { get; set; }
        [MaxLength(500)]
        public string? FullName { get; set; }
        [MaxLength(36)]
        public string Code { get; set; }
        [MaxLength(250)]
        public string? Email { get; set; }
        [MaxLength(36)]
        public string? Phone { get; set; }
        public string? Description { get; set; }

        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }

        public int PriorityLevelId { get; set; }

        public PriorityLevel PriorityLevel { get; set; }

        public ICollection<PaymentMethod>? PaymentMethods { get; set; }

        public static Customer Create(string email, string? firstName, string? lastName, string? phone, string? description, Currency? currency, PriorityLevel? priorityLevel, List<PaymentMethod>? paymentMethods)
        {
            return new Customer()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = phone,
                Description = description,
                Currency = currency,
                PriorityLevel = priorityLevel,
                PaymentMethods = paymentMethods
            };
        }

        public Customer PathUpdate(string? firstName, string? lastName, string? phone, string? description, Currency? currency, PriorityLevel? priorityLevel, List<PaymentMethod> paymentMethods)
        {
            if (!string.IsNullOrEmpty(firstName))
            {
                FirstName = firstName;
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                LastName = lastName;
            }

            if (!string.IsNullOrEmpty(phone))
            {
                Phone = phone;
            }

            if (!string.IsNullOrEmpty(description))
            {
                Description = description;
            }

            if (currency != null)
            {
                Currency = currency;
            }

            if (priorityLevel != null)
            {
                PriorityLevel = priorityLevel;
            }

            if (paymentMethods != null)
            {
                PaymentMethods = paymentMethods;
            }

            return this;
        }
    }
}
