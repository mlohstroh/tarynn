using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LitJson;

namespace WitAI
{
    public class WitEntity
    {
        private JsonData _data; 

        public WitEntity(JsonData data)
        {
            // not even going to bother because these can always be different data structures
            _data = data;
        }

        public JsonData GetValue(string key)
        {
            try
            {
                var obj = _data[key];
                return obj;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
