using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Baking.Client
{
    internal class TestMethodTreeNode : TestItemTreeNode<TestMethod>
    {
        internal TestMethodTreeNode(TestMethod method)
            : base(method)
        {
        }
    }
}
