using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using SQLite;

namespace Tarynn.Core
{
    public class FastStatement
    {
        private Dictionary<char, Dictionary<string, Statement>> statements;

        public FastStatement()
        {
            statements = new Dictionary<char, Dictionary<string, Statement>>();
            LoadDictionaries();
        }

        private void LoadDictionaries()
        {
            Console.WriteLine("Loading statements");
            int timeLapse = Environment.TickCount;
            Statement[] statements;
            TableQuery<Statement> query = Sql.SqlManager.SharedInstance.Connection.Table<Statement>();
            statements = (Statement[])query.ToArray();
            foreach (Statement s in statements)
            {

            }
            Console.WriteLine("Loaded all statements");
            Console.WriteLine("Total Time {0}", Environment.TickCount - timeLapse);
        }


        public Statement GetStatement(string sentence)
        {
            Dictionary<string, Statement> resultDict;
            char targetChar = sentence.ToCharArray()[0];
            statements.TryGetValue(targetChar, out resultDict);



            return null;
        }
    }
}
