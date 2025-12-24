using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.DTOS.BasketDtos
{
    public class BasketDto
    {
        public string Id { get; set; } 

        public ICollection<BasketItemDto> Items { get; set; } = [];


    }
}
