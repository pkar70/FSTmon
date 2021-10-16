using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Permissions;
using System.IO;

namespace FSTmon
{
    class Program
    {
        private static string sRootFolder;

        static void Main(string[] args)
        {
            Console.WriteLine("FSTmon v0.1, FileSystemTree Monitor (C) PKAR\n");

            sRootFolder = Environment.CurrentDirectory;

            //string[] largs = Environment.GetCommandLineArgs();
            if (args.Length == 2) sRootFolder = args[1];

            if (!Directory.Exists(sRootFolder))
            {
                Console.WriteLine("FAIL: '" + sRootFolder + "' doesn't exist");
                return;
            }

            Console.WriteLine("Starting to monitor '" + sRootFolder + "' folder for renames...");

            Run(sRootFolder);
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private static void Run(string sRootFolder)
        {
            FileSystemWatcher watcher = new FileSystemWatcher(sRootFolder);
            watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // watcher.Filter = "*.*";  // default: all
            watcher.IncludeSubdirectories = true;
            // Add event handlers.
            //watcher.Changed += OnChanged;
            //watcher.Created += OnChanged;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;

            // Begin watching.
            watcher.EnableRaisingEvents = true;

            // Wait for the user to quit the program.
            Console.WriteLine("Press 'q' to quit the sample.");
            while (Console.Read() != 'q') ;
        }

        private static void AddLogEntry(string sLogLine)
        {
            Console.WriteLine(sLogLine);
           var oWrt = File.AppendText(sRootFolder + "\\FSTmon.log");
            oWrt.WriteLine(DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss ") + sLogLine);
            oWrt.Flush();
            oWrt.Dispose();
        }

        //// Define the event handlers.
        //private void OnChanged(object source, FileSystemEventArgs e)
        //{
        //// Specify what is done when a file is changed, created, or deleted.
        //Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");
        //    }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            //AddLogEntry($"REN \"{e.OldFullPath}\" \"{e.FullPath}\"");
            AddLogEntry($"REN \"{e.OldName}\" \"{e.Name}\"");
        }
        private static void OnDeleted(object source, FileSystemEventArgs e)
        {
            AddLogEntry($"REM DEL \"{e.FullPath}\"");
        }
    }
}
