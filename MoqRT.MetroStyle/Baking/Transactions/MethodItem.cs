using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Moq.Baking
{
    public class MethodItem
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        public int InstanceItemId { get; set; }
        public string Name { get; set; }
    }
}
