using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

class CommandLineParameters
{
    // Hashtable args = new Hashtable();
    Dictionary<string, string> args = new Dictionary<string, string>();
    public CommandLineParameters()
    {
        string[] argsArr = Environment.GetCommandLineArgs();
        // string[] argsArr = new string[] { "Application.exe", "subcommand", "-switch", "-foo", "Lorem Ipsum" };
        for (int i = 0; i < argsArr.Length; i += 2)
        {
            if((argsArr[i].StartsWith("-") || argsArr[i].StartsWith("/")) && argsArr[i].Contains(":")){
                // Parameter and value: --foo:"Lorem Ipsum"
                string key = argsArr[i].Substring(0, argsArr[i].IndexOf(':')).TrimStart('-', '/');
                string value = argsArr[i].Substring(argsArr[i].IndexOf(':') + 1, argsArr[i].Length - argsArr[i].IndexOf(':') - 1).Trim('"');
                args.Add(key.ToLower(), value);
                i--;
            }
            else if (i + 1 == argsArr.Length)
            {
                // Single last parameter: "app.exe -switch" or "app.exe subcommand"
                args.Add(argsArr[i].TrimStart('-', '/').ToLower(), "");
            }
            else if ((argsArr[i].StartsWith("-") || argsArr[i].StartsWith("/")) && (argsArr[i + 1].StartsWith("-") || argsArr[i + 1].StartsWith("/")))
            {
                // Boolean parameter: "app.exe -switch -etc"
                args.Add(argsArr[i].TrimStart('-', '/').ToLower(), "");
                i--;
            }
            else if (!argsArr[i].StartsWith("-") && !argsArr[i].StartsWith("/"))
            {
                // Sub command: "app.exe subcommand -etc"
                args.Add(argsArr[i].ToLower(), "");
                i--;
            }
            else
            {
                // Parameter with value: "app.exe -foo 'Lorem ipsum'"
                args.Add(argsArr[i].TrimStart('-', '/').ToLower(), argsArr[i + 1]);
            }
        }
    }
    public string get(string param)
    {
        return (args.ContainsKey(param)) ? args[param].ToString() : "";
    }
    public void set(string param, string value)
    {
        args.Add(param, value);
    }
    public bool exists(string param)
    {
        return args.ContainsKey(param);
    }
    public string[] keys()
    {
        string[] keys = new string[args.Count];
        args.Keys.CopyTo(keys,0);
        return keys;
    }
    public void remove(string param)
    {
        if (args.ContainsKey(param))
            args.Remove(param);
    }
    public bool saveToFile(string filename="")
    {
        string config = "";
        string[] keys = new string[args.Count];
        args.Keys.CopyTo(keys, 0);
        for (int i=1; i<keys.Length; i++)
        {
            config += "--" + keys[i];
            config += (args[keys[i]].ToString().Length > 0) ? ":\""+args[keys[i]].ToString()+"\"" : "";
            config += "\n";
        }
        if (filename.Length == 0)
        {
            filename = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" +
                System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().Location) + ".defaults";
        }
        try
        {
            System.IO.File.WriteAllText(filename, config);
        } catch { return false; }
        return true;
    }
    public bool loadFromFile(string filename = "")
    {
        if (filename.Length == 0)
        {
            filename = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" +
                System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().Location) + ".defaults";
        }
        if (!System.IO.File.Exists(filename))
            return false;
        try
        {
            string[] config = System.IO.File.ReadAllLines(filename);
            for (int i = 0; i < config.Length; i++)
            {
                config[i] = config[i].TrimStart('-', '/');
                if (config[i].Contains(":"))
                {
                    string key = config[i].Substring(0, config[i].IndexOf(':'));
                    string value = config[i].Substring(config[i].IndexOf(':') + 1, config[i].Length - config[i].IndexOf(':') - 1);
                    value = value.Trim('"');
                    args.Add(key.ToLower(), value);
                }
            }
        } catch { return false; }
        return true;
    }
}
