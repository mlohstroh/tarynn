using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Matrix.Xmpp.XHtmlIM;
using TModules.Core;
using WitAI;
using Analytics;

namespace TModules
{
    public class PlatformManager
    {
        private Dictionary<string, Platform> _platforms = new Dictionary<string, Platform>();
        public ModuleManager Host { get; private set; }

        public PlatformManager(ModuleManager host)
        {
            Host = host;
        }

        public void Init()
        {
            AutoloadPlatformsAndResponses();

            //GenericPlatform g = new GenericPlatform(_platformManager);
            //_platformManager.RegisterPlatform(g);
            //g.AddResponder("weather", new ForecastIOResponder());
            // we can do autoloading of dlls after here
        }

        private void AutoloadPlatformsAndResponses()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var platformTypeList = new List<Type>();
            var responderTypeList = new List<Type>();

            foreach (var type in assembly.GetTypes())
            {
                if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(Platform)))
                {
                    platformTypeList.Add(type);
                }

                if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(ResourceResponder)))
                {
                    responderTypeList.Add(type);
                }
            }

            Type[] constructorTypes = new Type[1] { typeof(PlatformManager) };

            foreach (var type in platformTypeList)
            {
                var list = type.GetConstructors();
                
                var properConstructor = type.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, CallingConventions.HasThis,
                    constructorTypes, null);
                if (properConstructor != null)
                {
                    Platform p = (Platform) properConstructor.Invoke(new object[] {this});
                    try
                    {
                        if (p != null)
                        {
                            RegisterPlatform(p);
                            TConsole.InfoFormat("Platform {0} autoloaded.", p);
                        }
                    }
                    catch (DuplicatePlatformException)
                    {
                        TConsole.ErrorFormat("There is already a platform named {0}. Cannot auto register.", p.Name);
                    }

                }
                else
                {
                    TConsole.ErrorFormat("Type {0} does not have the appropriate constuctor and will not be created", type);
                }
            }

            foreach (var type in responderTypeList)
            {
                ResourceResponder responder = (ResourceResponder) assembly.CreateInstance(type.FullName);

                Debug.Assert(responder != null, "responder != null");
                var platform = PlatformForName(responder.PlatformName);

                if (platform != null)
                {
                    try
                    {
                        platform.AddResponder(responder.Resource, responder);
                        TConsole.InfoFormat("Responder {0} for resource {1} autoloaded.", responder, responder.Resource);
                    }
                    catch (DuplicateResourceResponderException)
                    {
                        TConsole.ErrorFormat("Duplicate responder for resource {0}", responder.Resource);
                    }
                    catch (Exception ex)
                    {
                        TConsole.ErrorFormat("An unknown exception was thrown for responder {0}. Exception: {1}", type, ex);
                    }   
                }
                else
                {
                    TConsole.ErrorFormat("The responder {0} was trying to register for platform {1}, but that platform did not exist.", 
                        type, responder.PlatformName);
                }

            }
        }

        public void RegisterPlatform(Platform p)
        {
            if (_platforms.ContainsKey(p.Name))
            {
                throw new DuplicatePlatformException();
            }
            else
            {
                _platforms.Add(p.Name, p);
            }
        }

        
        public Platform PlatformForName(string name)
        {
            if (_platforms.ContainsKey(name))
                return _platforms[name];
            return null;
        }

        public string Respond(string platform, string resource, WitOutcome outcome)
        {
            Platform p;
            if (_platforms.TryGetValue(platform, out p))
            {
                return p.Respond(resource, outcome);
            }
            return null;
        }
    }
}
