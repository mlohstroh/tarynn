using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Tarynn.Core
{
    public class SpecialResponse : DatabaseTable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public string Key { get; set; }
        public string Value { get; set; }

        public new static DatabaseTable[] All()
        {
            DatabaseTable[] rows;
            //query the table
            TableQuery<SpecialResponse> query = Sql.SqlManager.SharedInstance.Connection.Table<SpecialResponse>();
            rows = query.ToArray();
            //return the result
            return rows;
        }

        public new static DatabaseTable Find(int id)
        {
            return null;
        }
    }
}
