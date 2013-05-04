using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarynn.Core
{
    public abstract class DatabaseTable
    {
        public static DatabaseTable[] All() { return null; }
        public static DatabaseTable Find(int id) { return null; }
        public static void Insert(DatabaseTable row)
        {
            Sql.SqlManager.SharedInstance.Connection.Insert(row);
        }
    }
}
