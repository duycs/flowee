using AppShareServices.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerDomain.AgreegateModels.CustomerAgreegate
{
    public class PaymentMethod : Entity
    {
        [MaxLength(250)]
        public string Alias { get; set; }
        [MaxLength(36)]
        public string CardNumber { get; set; }
        [MaxLength(36)]
        public string SecurityNumber { get; set; }
        [MaxLength(250)]
        public string CardHolderName { get; set; }
        public DateTime Expiration { get; set; }
        public CardType CardType { get; set; }
    }
}
