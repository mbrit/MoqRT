using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Moq.Baked
{
    public abstract class BakedMock<T> : MockX<T>
        where T : class
    {
        protected BakedMock()
        {
        }
    }
}
