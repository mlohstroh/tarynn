using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.IO;
using Tarynn.Core;

namespace Tarynn.Sql
{
    class SqlManager
    {
        private static SqlManager instance;

        SQLiteConnection connection;

        public SqlManager()
        {
           
        }

        public void Load()
        {
            if (!CheckForDatabase())
                CreateDatabase();

            connection = new SQLiteConnection("database.db");   
        }

        public void PerformNecessaryMigrations()
        {
            //include every class that needs a migration
            connection.CreateTable<Statement>();
            connection.CreateTable<SpecialResponse>();
        }

        private bool CheckForDatabase()
        {
            return File.Exists("database.db");
        }

        private void CreateDatabase()
        {
            File.Create("database.db");
        }

        public static SqlManager SharedInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SqlManager();
                }
                return instance;
            }
        }
    }
}
