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
            LoadSpecialResponses();
        }

        private void LoadDatabase()
        {
            SqlManager.SharedInstance.PerformNecessaryMigrations();
        }

        private void LoadSpecialResponses()
        {
            SpecialResponse[] responses = (SpecialResponse[])SpecialResponse.All();
            foreach(SpecialResponse r in responses)
            {
                Console.WriteLine("{0} : {1}", r.Key, r.Value);
            }
        }
    }
}
