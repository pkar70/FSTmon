using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSTmon
{
    internal class Config
    {
        public string root { get; set; }
        public string mask { get; set; }

        public string log { get; set; }
        public bool logDate { get; set; }
        public bool logCmd { get; set; }

        public bool subdirs {  get; set; }

        public bool changed { get; set; }
        public bool created { get; set; }
        public bool deleted { get; set; }
        public bool renamed { get; set; }

        static private void Help()
        {
            Console.WriteLine("/?, /h \t help (this) page");

            Console.WriteLine("/f FOLD\t run on given folder (default: startdir");
            Console.WriteLine("/s \t with subdirectories (default: false)");
            Console.WriteLine("/m MASK\t use mask (default: *.*)");

            Console.WriteLine("/l LOG \t log to file");
            Console.WriteLine("/ld \t add timestamp to log file");
            Console.WriteLine("/lc \t add cmd to log file");

            Console.WriteLine("react for events:");
            Console.WriteLine("/chg \t 'changed' event");
            Console.WriteLine("/new \t 'created' event");
            Console.WriteLine("/del \t 'deleted' event");
            Console.WriteLine("/ren \t 'renamed' event");
            Console.WriteLine("/all \t all above");

            Console.WriteLine("/dump \t dump current state of variables");
        }

        public Config()
        {
            mask = "*.*";
            root = Environment.CurrentDirectory;
        }


        static public Config FromCmdLine(string[] args)
        {
            Config konfigs = new Config();

            for(int iLp=0; iLp < args.Length; iLp++)
            {
                switch (args[iLp].ToLower())
                {
                    case "/?":
                        case "/h":
                        Help();
                        return null;
                    case "/f":
                        konfigs.root = args[++iLp];
                        break;
                    case "/m":
                        konfigs.mask = args[++iLp];
                        break;
                    case "/l":
                        konfigs.log = args[++iLp];
                        break;
                    case "/ld":
                        konfigs.logDate = true;
                        break;
                    case "/lc":
                        konfigs.logCmd = true;
                        break;
                    case "/s":
                    case "/r":
                        konfigs.subdirs = true;
                        break;
                    case "/chg":
                        konfigs.changed = true;
                        break;
                    case "/new":
                        konfigs.created = true;
                        break;
                    case "/del":
                        konfigs.deleted = true;
                        break;
                    case "/ren":
                        konfigs.renamed = true;
                        break;
                    case "/all":
                        konfigs.changed = true;
                        konfigs.created = true;
                        konfigs.deleted = true;
                        konfigs.renamed = true;
                        break;
                    case "/dump":
                        konfigs.Dump();
                        break;
                }
            }

            if (!Directory.Exists(konfigs.root))
            {
                Console.WriteLine($"FAIL: '{konfigs.root}' doesn't exist");
                return null;
            }

        if(!konfigs.changed && !konfigs.created && !konfigs.deleted && !konfigs.renamed)
            {
                Console.WriteLine($"FAIL: no event specified");
                return null;
            }

            return konfigs;
        }

        public void Dump()
        {
            Console.WriteLine($"/r: {root}");
            Console.WriteLine($"/m: {mask}");
            Console.WriteLine($"/l: {log}");
            Console.WriteLine($"/ld: {logDate}");
            Console.WriteLine($"/lc: {logCmd}");
            Console.WriteLine($"/s: {subdirs}");
            Console.WriteLine($"/chg: {changed}");
            Console.WriteLine($"/new: {created}");
            Console.WriteLine($"/del: {deleted}");
            Console.WriteLine($"/ren: {renamed}");
        }


    }
}
