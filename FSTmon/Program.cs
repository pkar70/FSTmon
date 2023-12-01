using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Permissions;
using System.IO;
using System.Net.Security;

namespace FSTmon
{
    class Program
    {

        static private Config konfig;

        static void Main(string[] args)
        {
            // Console.WriteLine("FSTmon v0.1, FileSystemTree Monitor (C) PKAR\n");
            Console.WriteLine("FSTmon v0.7, FileSystemTree Monitor (C) PKAR\n");

            konfig = Config.FromCmdLine(args);
            if (konfig == null) return; // to będzie gdy /h, /?


            Console.WriteLine("Starting to monitor '" + konfig.root + "' folder for events..");

            RunMonitor();
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private static void RunMonitor()
        {
            FileSystemWatcher watcher = new FileSystemWatcher(konfig.root);
            watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Size;
            watcher.Filter = konfig.mask;
            watcher.IncludeSubdirectories = konfig.subdirs;
            // Add event handlers.
            if(konfig.changed) watcher.Changed += OnChanged;
            if (konfig.created) watcher.Created += OnCreated;
            if (konfig.deleted) watcher.Deleted += OnDeleted;
            if (konfig.renamed) watcher.Renamed += OnRenamed;

            // Begin watching.
            watcher.EnableRaisingEvents = true;

            // Wait for the user to quit the program.
            Console.WriteLine("Press 'q' to quit monitor");
            while (Console.Read() != 'q') ;
        }


        private static void AddLogEntry(string cmd, string sLogLine)
        {
            Console.WriteLine(cmd + " " + sLogLine);

            if (string.IsNullOrEmpty(konfig.log)) return;

           var oWrt = File.AppendText(konfig.log + "\\FSTmon.log");
            if (konfig.logCmd)
                sLogLine = cmd + " " + sLogLine;
            if (konfig.logDate)
                sLogLine = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss ") + sLogLine;
            oWrt.WriteLine(sLogLine);
            oWrt.Flush();
            oWrt.Dispose();
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            //AddLogEntry($"REN \"{e.OldFullPath}\" \"{e.FullPath}\"");
            AddLogEntry("REN",$"\"{e.OldName}\" \"{e.Name}\"");
        }
        private static void OnDeleted(object source, FileSystemEventArgs e)
        {
            AddLogEntry("DEL",$"\"{e.FullPath}\"");
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            AddLogEntry("REM CHG",$"\"{e.FullPath}\"");
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            AddLogEntry("REM ADD","\"{e.FullPath}\"");
        }
    }
}
