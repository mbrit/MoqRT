using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT
{
    public class StopBakingException : Exception
    {
        public StopBakingException()
            : base("Stop baking message received.")
        {
        }
    }
}
