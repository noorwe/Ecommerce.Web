using Ecommerce.Shared.DTOS.IdentityDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.DTOS.OrderDtos
{
    public class OrderToReturnDto
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; } = null!;
        public DateTimeOffset OrderDate { get; set; }
        public AddressDto Address { get; set; } = null!;
        public string DeliveryMethod { get; set; } = null!;

        public string OrderStatus { get; set; } = null!;

        public ICollection<OrderItemDto> Items { get; set; } 

        public decimal SubTotal { get; set; }

        public decimal Total { get; set; }
    }
}
