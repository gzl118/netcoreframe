using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANS.WebApi.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AnonymousFilter : Attribute
    {
    }
}
