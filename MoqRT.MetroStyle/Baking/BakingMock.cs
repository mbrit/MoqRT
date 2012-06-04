using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Moq.Driver;

namespace Moq.Baking
{
    public sealed class BakingMock<T> : Mock<T>
        where T : class
    {
        private RealMockWrapper<T> RealMock { get; set; }

        internal BakingMock()
        {
            // create a real mock...
            this.RealMock = RealMockFactory.Create<T>();
        }

        public override T Object
        {
            get
            {
                throw new ObjectReferencedInBakingException();
            }
        }
    }
}
