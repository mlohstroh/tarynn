using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TScript.Methods;
using TScript.Exceptions;

namespace TScript
{
    public class Interpreter
    {
        ScriptLoader loader;
        private List<string> mErrors = new List<string>();
        private Dictionary<string, TObject> scriptObjects = new Dictionary<string, TObject>(20);
        private List<MethodPackage> requiredPackages;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">The file name of the script</param>
        public Interpreter(string name)
        {
            loader = new ScriptLoader(name, this);
        }

        /// <summary>
        /// The parent should invoke this to make sure the script will mostly run
        /// </summary>
        /// <returns>Whether or not the script is healthy</returns>
        public bool Validate()
        {
            return loader.Validate();
        }

        /// <summary>
        /// This is where the magic happens
        /// </summary>
        /// <returns>The final output value of the script</returns>
        public string GetFinalText()
        {
            //this is required so we can start reading
            loader.OpenStream();

            this.requiredPackages = loader.RequiredPackages;
            string line;
            string finalValue = "";
            bool programDone = false;
            while ((line = loader.NextLine()) != null && !programDone)
            {
                string method = StripMethodFromLine(line);
                string[] argNames = GetArgsFromLine(line);

                switch (method)
                {
                    case "use":
                        break;
                    case "new":
                        HandleMethodNew(argNames);
                        break;
                    case "add": 
                        break;
                    case "sub":
                        break;
                    case "return":
                        finalValue = HandleReturnStatement(argNames);
                        programDone = true;
                        break;
                    default:
                        //search for package to pass info on to
                        MethodPackage p = this.GetPackageFromMethod(method);
                        TObjectChange request = p.GetResultForMethod(method, GetValuesForArgNames(argNames));
                        HandleObjectChangeRequest(request);
                        break;
                }
            }

            //close the stream when done
            loader.CloseStream();
            //give whoever wants the result back
            return finalValue;
        }

        /// <summary>
        /// Gets a nice friendly string full of errors
        /// </summary>
        /// <returns></returns>
        public string GetErrors()
        {
            StringBuilder b = new StringBuilder();

            foreach(string s in mErrors)
            {
                b.Append(s + "\n");
            }

            return b.ToString();
        }

        /// <summary>
        /// Adds error to the interpreter. This normally occurs after validation
        /// </summary>
        /// <param name="error">The error to add</param>
        public void AddError(string error)
        {
            mErrors.Add(error);
        }

        private object[] GetValuesForArgNames(string[] args)
        {
            object[] objArgs = new object[args.Length];

            for(int i = 0; i < args.Length; i++)
            {
                TObject possibleObject;
                if (this.scriptObjects.TryGetValue(args[i], out possibleObject))
                {
                    objArgs[i] = possibleObject;
                }
                else
                {
                    objArgs[i] = args[i];
                }
            }

            return objArgs;
        }

        private string StripMethodFromLine(string line)
        {
            return line.Substring(0, line.IndexOf('('));
        }

        private string[] GetArgsFromLine(string line)
        {
            char[] para = new char[] { '(', ')' };

            line = line.Remove(0, line.IndexOf('('));
            line = line.Trim(para);
            string[] trimmedArgs = line.Split(',');
            for(int i = 0; i < trimmedArgs.Length; i++)
            {
                string s = trimmedArgs[i].Trim();
                trimmedArgs[i] = s;
            }

            return trimmedArgs;
        }

        private void HandleObjectChangeRequest(TObjectChange request)
        {
            if (VerifyRequest(request))
            {
                this.scriptObjects.Remove(request.objectOne.Name);
                this.scriptObjects.Add(request.objectTwo.Name, request.objectTwo);
            }
            else
            {
                throw new FatalException("Script failed during ObjectChange request");
            }
        }

        private bool VerifyRequest(TObjectChange request)
        {
            //so hacky, but whatever 
            return request.objectOne.Name == request.objectTwo.Name && request.objectOne.InnerType == request.objectTwo.InnerType;
        }


        private MethodPackage GetPackageFromType(string typeName)
        {
            foreach (MethodPackage p in requiredPackages)
            {
                if(p.SupportsType(typeName))
                    return p;
            }
            return null;
        }

        private MethodPackage GetPackageFromMethod(string methodName)
        {
            foreach (MethodPackage p in requiredPackages)
            {
                if (p.MethodExists(methodName))
                    return p;
            }

            //this should NEVER happen
            throw new FatalException("Validation Failed during Interpreter Phase");
        }

        private void HandleMethodNew(string[] argNames)
        {
            MethodPackage package = GetPackageFromType(argNames[0]);
            if (package == null)
            {
                //we have an error
                AddError("Unsupported Type: " + argNames[0]);
                throw new UnsupportedType();
            }
            //make the package make the object
            TObject obj = package.GetNewObjectForType(argNames[0], argNames[1]);
            //then add the object in to our list
            this.scriptObjects.Add(argNames[1], obj);
        }

        private string HandleReturnStatement(string[] argNames)
        {
            string finalValue = "";
            
            if (scriptObjects.ContainsKey(argNames[0]))
            {
                TObject obj;
                this.scriptObjects.TryGetValue(argNames[0], out obj);
                finalValue = obj.Value.ToString();
            }
            else
            {
                finalValue = argNames[0];    
            }
            return finalValue;
        }
    }
}
