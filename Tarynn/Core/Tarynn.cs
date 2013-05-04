using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using Tarynn.Sql;

namespace Tarynn.Core
{
    public class Tarynn
    {
        Dictionary<string, SpecialResponse> specialResponses = new Dictionary<string, SpecialResponse>();

        public Tarynn()
        {
            LoadDatabase();
        }

        private void LoadDatabase()
        {
            SqlManager.SharedInstance.Load();
            SqlManager.SharedInstance.PerformNecessaryMigrations();
        }
    }
}
