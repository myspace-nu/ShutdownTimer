using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

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
                args.Add(argsArr[i].TrimStart('-', '/'), null);
            }
            else if ((argsArr[i].StartsWith("-") || argsArr[i].StartsWith("/")) && (argsArr[i + 1].StartsWith("-") || argsArr[i + 1].StartsWith("/")))
            {
                // Boolean parameter: "app.exe -switch -etc"
                args.Add(argsArr[i].TrimStart('-', '/'), null);
                i--;
            }
            else if (!argsArr[i].StartsWith("-") && !argsArr[i].StartsWith("/"))
            {
                // Sub command: "app.exe subcommand -etc"
                args.Add(argsArr[i], null);
                i--;
            }
            else
            {
                // Parameter with value: "app.exe -foo 'Lorem ipsum'"
                args.Add(argsArr[i].TrimStart('-', '/'), argsArr[i + 1]);
            }
        }
    }
    public string get(string param)
    {
        return (args.ContainsKey(param)) ? args[param].ToString() : null;
    }
    public bool exists(string param)
    {
        return args.ContainsKey(param);
    }
}
