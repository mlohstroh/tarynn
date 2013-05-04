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

        public DatabaseTable[] All()
        {
            
        }

        public DatabaseTable Find(int id)
        {
            return null;
        }
    }
}
