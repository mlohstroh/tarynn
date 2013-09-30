using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TScript.Methods
{
    public class SpotifyMethods : MethodPackage
    {
        public SpotifyMethods() 
            : base("Spotify Methods")
        {
            this.methodNames.Add("spotify_play");
            this.methodNames.Add("spotify_pause");
            this.methodNames.Add("spotify_list_playlists");
            this.methodNames.Add("spotify_select_playlist");
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
