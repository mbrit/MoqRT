using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoqRT.Baking.Data
{
    public class SQLiteHelper
    {
        public static void EnsureSQLiteInFolder(string folder)
        {
            string path = Path.Combine(folder, "sqlite3.dll");
            if(!(File.Exists(path)))
            {
                using(var stream = typeof(SQLiteHelper).Assembly.GetManifestResourceStream("MoqRT.Baking.Resources.sqlite3.dll"))
                {
                    using(var file = new FileStream(path, FileMode.Create, FileAccess.Write))
                        stream.CopyTo(file);
                }
            }
        }
    }
}
