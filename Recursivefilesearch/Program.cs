﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Recursivefilesearch
{
    class Program
    {
        static System.Collections.Specialized.StringCollection log = new System.Collections.Specialized.StringCollection();
        private static List<string> _excludedDirectories = new List<string>() { "Windows", "AppData", "$WINDOWS.~BT", "MSOCache", "ProgramData", "Config.Msi", "$Recycle.Bin", "Recovery", "System Volume Information", "Documents and Settings", "Perflogs" };

        public string strPath = "Data Source = AP0C-PC\\VARSITY; Initial Catalog = DirIndexer; Integrated Security = SSPI";

        //method to check
        static bool isExcluded(List<string> exludedDirList, string target)
        {
            return exludedDirList.Any(d => new DirectoryInfo(target).Name.Equals(d));
        }

        static void Main()
        {
            string filePath = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\documents");
            Console.WriteLine(filePath);
            DirectoryInfo rootDir = new DirectoryInfo(filePath);
            WalkDirectoryTree(rootDir);

            // Write out all the files that could not be processed.
            Console.WriteLine("Files with restricted access:");
            foreach (string s in log)
            {
                Console.WriteLine(s);
            }
            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key");
            Console.ReadKey();
        }

        static void WalkDirectoryTree(System.IO.DirectoryInfo root)
        {
            FileInfo[] files = null;
            DirectoryInfo[] subDirs = null;
            
            // Process all the files directly under the root 
            try
            {
                files = root.GetFiles("*.*");
            }// This is thrown if even one of the files requires permissions greater than the application provides. 
            catch (UnauthorizedAccessException e)
            {
                log.Add(e.Message);
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if (files != null)
            {
                foreach (FileInfo fi in files)
                {
                    Console.WriteLine(fi.FullName);
                }

                subDirs = root.GetDirectories();

                foreach (DirectoryInfo dirInfo in subDirs)
                {
                    // Resursive call for each subdirectory.
                    WalkDirectoryTree(dirInfo);
                }
            }
            
        }
    }
}
