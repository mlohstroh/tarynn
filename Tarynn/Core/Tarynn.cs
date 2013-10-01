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
using TModules;
using Tarynn.JSON;

namespace Tarynn.Core
{
    public class Tarynn
    {
        Dictionary<string, SpecialResponse> specialResponses = new Dictionary<string, SpecialResponse>();
        FastStatement allStatements;

        public delegate void Echo(object sender, TarynnEchoEventArgs e);
        public event Echo EchoEvent;

        Interpreter mInterpreter;
        ModuleManager mManager;

        DocumentHandler mJson;

        public Tarynn()
        {
            
        //    LoadDatabase();         
          //  LoadSpecialResponses();
            LoadManger();
        }

        private void LoadJsonDocuments()
        {
            mJson = new DocumentHandler();
        }

        private void LoadManger()
        {
            mManager = new ModuleManager();
        }

        public string RespondTo(string message)
        {
            return mManager.RespondTo(message);
        }

        public Query RelateQuery(Query q)
        {
            TConsole.Info("Relaing Query");
            TarynnEchoEventArgs e = new TarynnEchoEventArgs("What do you mean when you say, '" + q.OriginalText + "' ?\n");
            OnTarynnEcho(e);

            RelateQueryDialog d = new RelateQueryDialog(q);
            d.ShowDialog();

            q = d.FinalQuery;

            TConsole.Info("Inserting new statement");
            //attach
            allStatements.InsertStatement(q.AttachedStatement);

            return q;
        }

        public Query TypeQuery(Query q)
        {
            return q;
        }

        public Query InitialQuery(string queryString)
        {
            Query q = new Query(queryString.ToLower(), this);

            Statement statement = allStatements.GetStatement(q.OriginalText);
            if (statement == null)
            {
                //whoops, no such statement found
                q.State = QueryState.Unrelated;
            }
            else
            {
                q.AttachedStatement = statement;
            }

            return q;
        }

        public string RunScript(string name)
        {
            try
            {
                mInterpreter = new Interpreter(name);
                if (mInterpreter.Validate())
                {
                    TConsole.Info("Script was validated properly");
                }
                else
                {
                    TConsole.Error("Script failed validation");
                    return mInterpreter.GetErrors();
                }
                return mInterpreter.GetFinalText();
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
            TConsole.Info("Echoing back to client");
            Echo newEcho = EchoEvent;
            if (newEcho != null)
            {
                newEcho(this, e);
            }
        }
    }
}
