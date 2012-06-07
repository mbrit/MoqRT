using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moq.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TestSessionAttribute : Attribute
    {
    }
}
