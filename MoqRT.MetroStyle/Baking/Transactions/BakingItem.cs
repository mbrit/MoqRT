using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Moq.Baking
{
    public class BakingItem
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        public int MethodItemId { get; set; }
        public string Expression { get; set; }
    }
}
