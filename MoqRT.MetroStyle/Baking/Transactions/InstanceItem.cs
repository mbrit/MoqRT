using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Moq.Baking
{
    public class InstanceItem
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        public int HashCode { get; set; }
        public string Type { get; set; }
    }
}
