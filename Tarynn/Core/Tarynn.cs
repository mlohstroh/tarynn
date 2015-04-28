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
using TModules.Core;

namespace Tarynn.Core
{
    public class Tarynn
    {
        private TConsole _logger = new TConsole(typeof(Tarynn));
        Dictionary<string, SpecialResponse> specialResponses = new Dictionary<string, SpecialResponse>();
        FastStatement allStatements;

        public delegate void Echo(object sender, TarynnEchoEventArgs e);
        public event Echo EchoEvent;

        Interpreter mInterpreter;
        ModuleManager mManager;

        public Tarynn()
        {
            
        //    LoadDatabase();         
          //  LoadSpecialResponses();
            LoadManger();
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
            _logger.Info("Relaing Query");
            TarynnEchoEventArgs e = new TarynnEchoEventArgs("What do you mean when you say, '" + q.OriginalText + "' ?\n");
            OnTarynnEcho(e);

            RelateQueryDialog d = new RelateQueryDialog(q);
            d.ShowDialog();

            q = d.FinalQuery;

            _logger.Info("Inserting new statement");
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
                    _logger.Info("Script was validated properly");
                }
                else
                {
                    _logger.Error("Script failed validation");
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
            _logger.Info("Setting initial greeting");
            SpecialResponse r = new SpecialResponse();
            r.Key = "greeting";
            r.Value = "Hello Mark";
            specialResponses.Add("greeting", r);
            SpecialResponse.Insert(r);
        }

        private void LoadDatabase()
        {
            _logger.Info("Initializing Database");
            SqlManager.SharedInstance.PerformNecessaryMigrations();
        }

        private void LoadSpecialResponses()
        {
            _logger.Info("Loading special responses");
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
            _logger.Info("Echoing back to client");
            Echo newEcho = EchoEvent;
            if (newEcho != null)
            {
                newEcho(this, e);
            }
        }
    }
}
