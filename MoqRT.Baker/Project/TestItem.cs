using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Baking
{
    [Serializable]
    public abstract class TestItem
    {
        public string Name { get; private set; }
        public bool Include { get; set; }

        protected TestItem(string name)
        {
            this.Name = name;
            this.Include = true;
        }
    }
}
