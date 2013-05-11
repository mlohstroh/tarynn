using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spotify;

namespace TScript.Methods
{
    public class SpotifyMethods : MethodPackage
    {
        public SpotifyMethods() 
            : base("Spotify Methods")
        {
        }

        public override TObject GetNewObjectForType(string type, string name)
        {
            return TObject.Empty(); 
        }

        public override TObjectChange GetResultForMethod(string method, object[] args)
        {
            return TObjectChange.Empty();
        }
    }
}
