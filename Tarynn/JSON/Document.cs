using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tarynn.JSON
{
    public class Document
    {
        public Dictionary<string, Object> Contents { get; set; }

        private string _fileName = null;

        public Collection Collection { get; private set; }

        public Document(Collection collection)
        {
            Collection = collection;
        }

        public Document(Collection collection, Dictionary<string, Object> contents)
        {
            Collection = collection;
            this.Contents = contents;
        }

        public void Save()
        {
            if (IsNew())
            {
                
            }
        }

        public bool IsNew()
        {
            return _fileName == null;
        }
    }
}
