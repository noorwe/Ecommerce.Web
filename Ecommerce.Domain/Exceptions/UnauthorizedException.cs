using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Exceptions
{
    public sealed class UnauthorizedException(string msg = "Invalid Email Or Password") : Exception(msg)
    {
    }
}
