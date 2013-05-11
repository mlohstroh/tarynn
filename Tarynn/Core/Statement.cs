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
        public string ResponseText { get; set; }
        public string ScriptName { get; set; }
        public int RelatedId { get; set; }

        /// <summary>
        /// Returns all the Statements in the database
        /// </summary>
        /// <returns></returns>
        public new static Statement[] All()
        {
            TableQuery<Statement> query = Sql.SqlManager.SharedInstance.Connection.Table<Statement>();

            return query.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public new static Statement Find(int id)
        {
            var query = Sql.SqlManager.SharedInstance.Connection.Table<Statement>().Where(v => v.Id == id);
            return (Statement)query.Single();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        public new static void Insert(DatabaseTable row)
        {
            Sql.SqlManager.SharedInstance.Connection.Insert(row);
        }
    }
}
