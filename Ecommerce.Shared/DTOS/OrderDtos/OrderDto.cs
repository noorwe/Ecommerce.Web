using Ecommerce.Shared.DTOS.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.DTOS.OrderDtos
{
    public class OrderDto
    {
        public string BasketId { get; set; } = null!;
        public int DeliveryMethodId { get; set; }

        public AddressDto Address { get; set; } = null!;
    }
}
