using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Exceptions
{
    public sealed class BasketNotFoundException(string key) : NotFoundException($"Busket With Id : {key} Is Not Found")
    {
    }
}
