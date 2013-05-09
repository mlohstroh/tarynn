using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using Tarynn.Sql;
using Tarynn.Dialogs;
using Analytics;
using TScript;
using TScript.Exceptions;

namespace Tarynn.Core
{
    public class Tarynn
    {
        Dictionary<string, SpecialResponse> specialResponses = new Dictionary<string, SpecialResponse>();
        FastStatement allStatements = new FastStatement();

        public delegate void Echo(object sender, TarynnEchoEventArgs e);
        public event Echo EchoEvent;

        public Tarynn()
        {
            LoadDatabase();         
            LoadSpecialResponses();
        }

        public Query RelateQuery(Query q)
        {
            TarynnEchoEventArgs e = new TarynnEchoEventArgs("What do you mean when you say, '" + q.OriginalText + "' ?");
            OnTarynnEcho(e);

            //RelateQueryDialog d = new RelateQueryDialog(q, );

            return q;
        }

        public Query TypeQuery(Query q)
        {
            return q;
        }

        public Query InitialQuery(string queryString)
        {
            Query q = new Query(queryString.ToLower());

            Statement statement = allStatements.GetStatement(q.OriginalText);
            if (statement == null)
            {
                //whoops, no such statement found
                q.State = QueryState.Unrelated;
            }
            else
            {
                q.State = QueryState.Typeless;
            }

            return q;
        }

        public string RunScript(string name)
        {
            try
            {
                Interpreter i = new Interpreter(name);
                if (i.Validate())
                {
                    TConsole.Info("Script was validated properly");
                }
                else
                {
                    TConsole.Error("Script failed validation");
                    return i.GetErrors();
                }
                return i.GetFinalText();
            }
            catch (TException ex)
            {
                return "Error: " + ex.Message;
            }
        }

        private void SetInitialGreeting()
        {
            TConsole.Info("Setting initial greeting");
            SpecialResponse r = new SpecialResponse();
            r.Key = "greeting";
            r.Value = "Hello Mark";
            specialResponses.Add("greeting", r);
            SpecialResponse.Insert(r);
        }

        private void LoadDatabase()
        {
            TConsole.Info("Initializing Database");
            SqlManager.SharedInstance.PerformNecessaryMigrations();
        }

        private void LoadSpecialResponses()
        {
            TConsole.Info("Loading special responses");
            SpecialResponse[] responses = (SpecialResponse[])SpecialResponse.All();
            foreach(SpecialResponse r in responses)
            {
                specialResponses.Add(r.Key, r);
            }
            if (!specialResponses.ContainsKey("greeting"))
            {
                SetInitialGreeting();
            }
        }

        private void OnTarynnEcho(TarynnEchoEventArgs e)
        {
            Echo newEcho = EchoEvent;
            if (newEcho != null)
            {
                newEcho(this, e);
            }
        }
    }
}
