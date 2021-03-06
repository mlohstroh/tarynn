﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TScript.Methods;
using TScript.Exceptions;
using TScript.Dialogs;
using Analytics;

namespace TScript
{
    public class Interpreter
    {
        private TConsole _logger = new TConsole(typeof(Interpreter));
        ScriptLoader loader;
        private List<string> mErrors = new List<string>();
        private Dictionary<string, TObject> scriptObjects = new Dictionary<string, TObject>(20);
        private List<MethodPackage> requiredPackages;

        private string currentLine;

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
            //clear objects before running more
            scriptObjects.Clear();
            _logger.Info("Running script");

            _logger.Debug("Starting time");
            Profiler.SharedInstance.StartProfiling("script_run");
            //this is required so we can start reading
            loader.OpenStream();

            this.requiredPackages = loader.RequiredPackages;
            string finalValue = "";
            bool programDone = false;
            int lineNumber = 0;

            while ((currentLine = loader.Lines[lineNumber++].ToString()) != null && !programDone)
            {
                string method = StripMethodFromLine(currentLine);
                string[] argNames = GetArgsFromLine(currentLine);

                switch (method)
                {
                    case "use":
                        //absoutely nothing
                        _logger.Debug("Using lib " + argNames[0]);
                        break;
                    case "new":
                        HandleMethodNew(argNames);
                        break;
                    case "add":
                        HandleAddMethod(argNames);
                        break;
                    case "sub":
                        HandleSubMethod(argNames);
                        break;
                    case "get_input":
                        HandleGetInput(argNames);
                        break;
                    case "return":
                        finalValue = HandleReturnStatement(argNames);
                        programDone = true;
                        break;
                    case "beq":
                        HandleBranch(method, argNames, lineNumber);
                        break;
                    case "bne":
                        HandleBranch(method, argNames, lineNumber);
                        break;
                    case "bgt":
                        HandleBranch(method, argNames, lineNumber);
                        break;
                    case "blt":
                        HandleBranch(method, argNames, lineNumber);
                        break;
                    default:
                        //search for package to pass info on to
                        MethodPackage p = this.GetPackageFromMethod(method);
                        //get the official request from 
                        TObjectChange request = p.GetResultForMethod(method, GetValuesForArgNames(argNames));
                        //if there is an actual request, then we handle it, if not, we don't care
                        if (!request.IsEmpty())
                        {
                            HandleObjectChangeRequest(request);
                        }
                        break;
                }
            }

            //close the stream when done
            loader.CloseStream();
            //give whoever wants the result back
            //int time = Profiler.SharedInstance.GetTimeForKey("script_run");
            //TConsole.Debug("Time for running script was: " + time.ToString() + " ms");

            return finalValue;
        }

        private int HandleBranch(string branchFunc, string[] args, int line)
        {
            object[] values = GetValuesForArgNames(args);
            object val1;
            object val2;
            if (values[0].GetType() == typeof(TObject))
                val1 = (TObject)values[0];
            else
                val1 = values[0];
            if (values[1].GetType() == typeof(TObject))
                val2 = (TObject)values[1];
            else
                val2 = values[1];
            int lineNumber = loader.Branches[args[2]];
             
            switch (branchFunc)
            {
                case "beq":
                    if (val1 == val2)
                        return lineNumber;
                    break;
                case "bne":
                    if (val1 != val2)
                        return lineNumber;
                    break;
                case "bgt":
                    int intVal1;
                    if (val1.GetType() == typeof(TObject))
                    {
                        intVal1 = (int)((TObject)val1).Value;
                    }
                    else
                    {
                        intVal1 = int.Parse(args[0]);
                    }
                    int intVal2;
                    if (val2.GetType() == typeof(TObject))
                    {
                        intVal2 = (int)((TObject)val2).Value;
                    }
                    else
                    {
                        intVal2 = int.Parse(args[0]);
                    }
                    if (intVal1 > intVal2)
                        return lineNumber;
                    break;
                case "blt":
                    if (val1.GetType() == typeof(TObject))
                    {
                        intVal1 = (int)((TObject)val1).Value;
                    }
                    else
                    {
                        intVal1 = int.Parse(args[0]);
                    }
                    if (val2.GetType() == typeof(TObject))
                    {
                        intVal2 = (int)((TObject)val2).Value;
                    }
                    else
                    {
                        intVal2 = int.Parse(args[0]);
                    }
                    if (intVal1 > intVal2)
                        return lineNumber;
                    break;
            }
            return line;
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

        public string GetRawTextFromArgIndex(int index)
        {
            char[] para = new char[] { '(', ')' };
            string line = currentLine.Remove(0, currentLine.IndexOf('('));
            line = line.Trim(para);
            string[] raw = line.Split(',');
            return raw[index];
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
                loader.CloseStream();
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

        private void HandleAddMethod(string[] argNames)
        {
            int first;
            if (!scriptObjects.ContainsKey(argNames[0]))
            {
                first = int.Parse(argNames[0]);
            }
            else
            {
                first = (int)GetObjectValue(argNames[0]);
            }

            int second;
            if(!scriptObjects.ContainsKey(argNames[1]))
            {
                second = int.Parse(argNames[1]);
            }
            else
            {
                second = (int)GetObjectValue(argNames[1]);
            }

            TObject destination;
            scriptObjects.TryGetValue(argNames[2], out destination);
            scriptObjects.Remove(argNames[2]);
            destination.Value = first + second;
            scriptObjects.Add(argNames[2], destination);
        }

        private void HandleSubMethod(string[] argNames)
        {
            int first;
            if (!scriptObjects.ContainsKey(argNames[0]))
            {
                first = int.Parse(argNames[0]);
            }
            else
            {
                first = (int)GetObjectValue(argNames[0]);
            }

            int second;
            if (!scriptObjects.ContainsKey(argNames[1]))
            {
                second = int.Parse(argNames[1]);
            }
            else
            {
                second = (int)GetObjectValue(argNames[1]);
            }

            TObject destination;
            scriptObjects.TryGetValue(argNames[2], out destination);
            scriptObjects.Remove(argNames[2]);
            destination.Value = first - second;
            scriptObjects.Add(argNames[2], destination);
        }

        private void HandleGetInput(string[] argNames)
        {
            InputForm input = new InputForm();
            input.ShowDialog();

            if (input.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                TObject obj;
                scriptObjects.TryGetValue(argNames[0], out obj);
                scriptObjects.Remove(argNames[0]);
                //grab the input result and put it in the object
                obj.Value = input.Result;

                scriptObjects.Add(argNames[0], obj);
            }
            else
            {
                throw new FatalException("Input never came, aborting script!");
            }
        }

        public object GetObjectValue(object obj)
        {
            object value;

            if (obj.GetType() == typeof(TObject))
            {
                TObject tObj = (TObject)obj;
                value = tObj.Value;
            }
            else
            {
                value = obj;
            }
            return value;
        }

        public object GetObjectValue(string name)
        {
            object obj;

            if (scriptObjects.ContainsKey(name))
            {
                TObject tObj;
                scriptObjects.TryGetValue(name, out tObj);
                obj = tObj.Value;
            }
            else
            {
                obj = name;
            }

            return obj;
        }

        public TObjectChange MakeChange(TObject obj, object newValue)
        {
            TObject newObj = new TObject(obj.InnerType, newValue, obj.Name);
            return new TObjectChange(obj, newObj);
        }
    }
}
