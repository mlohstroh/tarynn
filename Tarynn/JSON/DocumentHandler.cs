using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarynn.JSON
{
    public class DocumentHandler
    {
        private string _rootPath;
        //private List<


        public DocumentHandler()
        {
            _rootPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.Create);
        }

        public void LoadCollections()
        {

        }
    }
}
