using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moq
{
    public class ObjectReferencedInBakingException : Exception
    {
        public ObjectReferencedInBakingException()
            : base("Object referencing in baking.")
        {
        }
    }
}
