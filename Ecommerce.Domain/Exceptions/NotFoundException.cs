using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Exceptions
{
    public abstract class NotFoundException(string msg) : Exception(msg)
    {
    }
}
