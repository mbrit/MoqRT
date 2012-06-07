using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoqRT.Baking.Client
{
    internal class TestItemTreeNode<T> : TreeNode, ITreeItemNode
        where T : TestItem
    {
        internal T Item { get; private set; }

        internal TestItemTreeNode(T item)
        {
            this.Item = item;
            this.Text = item.Name;
            this.Checked = item.Include;
        }

        public virtual void HandleCheckChanged()
        {
            this.Item.Include = this.Checked;
        }
    }
}
