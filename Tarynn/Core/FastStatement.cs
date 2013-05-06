using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using System.Security.Cryptography;

namespace Tarynn.Core
{
    public class FastStatement
    {
        private Dictionary<char, Dictionary<string, Statement>> allStatements;

        public FastStatement()
        {
            allStatements = new Dictionary<char, Dictionary<string, Statement>>();
            LoadDictionaries();
        }

        public void SaveToDatabase()
        {
            foreach (Dictionary<string, Statement> d in allStatements.Values)
            {
                Sql.SqlManager.SharedInstance.Connection.UpdateAll(d.Values);
            }
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
                char targetChar = s.FullText.ToCharArray()[0];
                string key = ConvertToMd5(s.FullText);
                Dictionary<string, Statement> nestedDict;

                if (allStatements.ContainsKey(targetChar))
                {
                    allStatements.TryGetValue(targetChar, out nestedDict);
                    nestedDict.Add(key, s);
                }
                else
                {
                    nestedDict = new Dictionary<string, Statement>();
                    allStatements.Add(targetChar, nestedDict);
                }
                nestedDict.Add(key, s);
            }
            Console.WriteLine("Loaded all statements");
            Console.WriteLine("Total Time for {0} elements: {1}ms", statements.Length, Environment.TickCount - timeLapse);
        }


        public Statement GetStatement(string sentence)
        {
            Dictionary<string, Statement> resultDict;
            char targetChar = sentence.ToCharArray()[0];
            allStatements.TryGetValue(targetChar, out resultDict);

            string key = ConvertToMd5(sentence);
            Statement returnStatement;

            resultDict.TryGetValue(key, out returnStatement);

            return returnStatement;
        }

        private string ConvertToMd5(string s)
        {
            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(Encoding.ASCII.GetBytes(s));
            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < bytes.Length; i++)
            {
                sBuilder.Append(bytes[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return sBuilder.ToString();
        }

        public void InsertStatement(Statement statement)
        {

        }
    }
}
