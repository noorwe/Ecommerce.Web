using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.OrderModule
{
    public class Order:BaseEntity<Guid>
    {
        public Order()
        {
            
        }
        public Order(string userEmail, OrderAddress address, DeliveryMethod deliveryMethod, int deliveryMethodId, decimal subTotal, ICollection<OrderItem> items)
        {
            UserEmail = userEmail;
            Address = address;
            DeliveryMethod = deliveryMethod;
            DeliveryMethodId = deliveryMethodId;
            SubTotal = subTotal;
            Items = items;
        }

        public string UserEmail { get; set; } = null!;
        public OrderAddress Address { get; set; } = null!;
        public DeliveryMethod DeliveryMethod { get; set; } = null!;
        public int DeliveryMethodId { get; set; }
        public decimal SubTotal { get; set; }
        public ICollection<OrderItem> Items { get; set; } = [];


        public OrderStatus OrderStatus { get; set; }


        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        [NotMapped]
        public decimal Total { get => SubTotal + DeliveryMethod.Price; }

    }
}
