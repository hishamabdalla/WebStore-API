using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Entities
{
    public class UserBasket
    {
        public string Id { get; set; }
        public List<BasketItem> BasketItems { get; set; } = new List<BasketItem>();

        public int? DeliveryMethodId {  get; set; }
        public string? PaymentIntentId {  get; set; }
        public string? ClientSecret {  get; set; }

    }
}
