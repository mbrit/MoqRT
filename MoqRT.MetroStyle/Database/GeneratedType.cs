using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Moq.Database
{
    public class GeneratedType
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        public string TargetType { get; set; }
        public string ProxyType { get; set; }
    }
}
