using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace ShutdownTimer
{
    class CommandLineParameters
    {
        Hashtable args = new Hashtable();
        public CommandLineParameters()
        {
            string[] argsArr = Environment.GetCommandLineArgs();
            // string[] argsArr = new string[] { "Application.exe", "subcommand", "-switch", "-foo", "Lorem Ipsum" };
            for (int i = 0; i < argsArr.Length; i += 2)
            {
                if (i + 1 == argsArr.Length)
                {
                    // Single last parameter: "app.exe -switch" or "app.exe subcommand"
                    args.Add(argsArr[i], "");
                }
                else if (argsArr[i].StartsWith("-") && argsArr[i + 1].StartsWith("-"))
                {
                    // Boolean parameter: "app.exe -switch -etc"
                    args.Add(argsArr[i], "");
                    i--;
                }
                else if (!argsArr[i].StartsWith("-"))
                {
                    // Sub command: "app.exe subcommand -etc"
                    args.Add(argsArr[i], "");
                    i--;
                }
                else
                {
                    // Parameter with value: "app.exe -foo 'Lorem ipsum'"
                    args.Add(argsArr[i], argsArr[i + 1]);
                }
            }
        }
        public string get(string param)
        {
            return (args.ContainsKey(param)) ? args[param].ToString() : "";
        }
        public bool exists(string param)
        {
            return args.ContainsKey(param);
        }
    }
}
