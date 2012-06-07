using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Baking
{
    [Serializable]
    public class TestSession : TestItem
    {
        public List<TestClass> Classes { get; private set; }

        internal TestSession(string name)
            : base(name)
        {
            this.Classes = new List<TestClass>();
        }
    }
}
