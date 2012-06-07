using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoqRT.Baking.Client
{
    internal class TestClassTreeNode : TestItemTreeNode<TestClass>
    {
        internal TestClassTreeNode(TestClass c)
            : base(c)
        {
            foreach (TestMethod method in c.Methods)
                this.Nodes.Add(new TestMethodTreeNode(method));
        }

        public override void HandleCheckChanged()
        {
            base.HandleCheckChanged();

            foreach (TreeNode node in Nodes)
                node.Checked = this.Checked;
        }
    }
}
