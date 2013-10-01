using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarynn.JSON
{
    public class DocumentHandler
    {
        public string RootPath { get; private set; }
        private List<Collection> loadedCollections = new List<Collection>();


        public DocumentHandler()
        {
            RootPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create);
        }
        
        public void LoadCollections()
        {
            //detect off of folder names
        }

        public void LoadCollections(string[] collections)
        {
            for (int i = 0; i < collections.Length; i++)
            {
                Collection col = new Collection(collections[i], this, true);
                loadedCollections.Add(col);
            }
        }
    }
}
