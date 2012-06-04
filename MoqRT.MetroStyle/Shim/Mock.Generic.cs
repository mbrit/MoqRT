using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Moq.Baking;

namespace Moq
{
    // @mbrit - 2012-06-02 - we're abstract because we're always generated...
    public abstract class MockX<T> : MockX
        where T : class
    {
        protected internal MockX()
        {
        }

        public abstract T Object
        {
            get;
        }
    }
}
