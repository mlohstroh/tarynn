using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Tarynn.Core
{
    public class Statement : DatabaseTable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public string FullText { get; set; }

        public DatabaseTable[] All()
        {
            return null;
        }

        public DatabaseTable Find(int id)
        {
            return null;
        }

        public void Insert(DatabaseTable row)
        {
            Sql.SqlManager.SharedInstance.Connection.Insert(row);
        }
    }
}
