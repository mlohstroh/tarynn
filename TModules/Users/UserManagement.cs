using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpCouch;
using LitJson;
using System.Text.RegularExpressions;
using TModules.Core;
using TModules.DefaultModules;

namespace TModules.Users
{
    public class UserManagement : TModule
    {
        private DB _couch = new DB();

        const string SERVER_ADDRESS = "http://127.0.0.1:5984";
        const string DB_NAME = "tarynn-users";

        private Dictionary<string, User> _allUsers = new Dictionary<string, User>();

        public UserManagement(ModuleManager host)
            : base("Users", host)
        {
            InitDatabase();

            AddCallback("hello this is (.*)", SignInUser);
        }

        private void InitDatabase()
        {
            if (!DBExists(DB_NAME))
            {
                _couch.CreateDatabase(SERVER_ADDRESS, DB_NAME);
            }

            LoadDocs();
        }

        private void LoadDocs()
        {
            _allUsers.Clear();

            DocInfo[] docs = _couch.GetAllDocuments(SERVER_ADDRESS, DB_NAME);

            foreach (DocInfo doc in docs)
            {
                string json = _couch.GetDocument(SERVER_ADDRESS, DB_NAME, doc.ID);
                JsonData data = JsonMapper.ToObject(json);

                User u = new User(data["Name"].ToString());

                _allUsers.Add(doc.ID, u);
            }
        }

        public void CreateUser(string name)
        {
            User u = new User(name);
            _couch.CreateDocument(SERVER_ADDRESS, DB_NAME, JsonMapper.ToJson(u));
            SignedInUser = u;
        }

        public User SignedInUser { get; private set; }

        public void SignInUser(Match message)
        {
            string name = message.Groups[1].Value;

            foreach (User u in _allUsers.Values)
            {
                if (u.Name == name)
                {
                    SignedInUser = u;
                    Host.SpeakEventually("Hello " + name +". Welcome back");
                    return;
                }
            }
            Host.SpeakEventually("Hello " + name + ". I haven't seen you before, so I just created a profile for you");
            CreateUser(name.ToLower());
        }

        public List<User> UserList
        {
            get
            {
                return _allUsers.Values.ToList();
            }
        }

        private bool DBExists(string name)
        {
            string[] dbNames = _couch.GetDatabases(SERVER_ADDRESS);
            foreach (string s in dbNames)
            {
                if (s == name)
                    return true;
            }
            return false;
        }
    }
}
