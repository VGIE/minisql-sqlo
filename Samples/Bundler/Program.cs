using System;
using System.Reflection;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Diagnostics;

namespace Bundler
{
    class Program
    {
        public static void Main()
        {
            //Set the folder where this program is as the working directory
            //We need to do this so that relative paths work in the Azure agent
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            List<string> files = new List<string>();
            string version;

            string relPathToSolutionRootFolder = "../../../../../"; //relative path from Bundler.exe to the solution root folder 
            string mainExe = relPathToSolutionRootFolder + "Apps/DBManagerConsole/bin/Release/netcoreapp3.1/DBManagerConsole.exe";
            List<string> dependencies = new List<string>();
            GetDependencies(relPathToSolutionRootFolder + "Apps/DBManagerConsole/bin/Release/netcoreapp3.1/", "DBManagerConsole.exe", ref dependencies, true);
            string mainDll = relPathToSolutionRootFolder + "Apps/DBManagerConsole/bin/Release/netcoreapp3.1/DBManagerConsole.dll";
            string dbManagerDll = relPathToSolutionRootFolder + "Apps/DBManagerConsole/bin/Release/netcoreapp3.1/DBManager.dll";

            version = GetVersion(mainExe);
            if (version == null)
                return;

            string rootFolderInZip = "OurProject/"; //name of the folder created inside the zip file

            files.Add(mainExe);
            files.Add(mainDll);
            files.Add(dbManagerDll);

            string outputFile = relPathToSolutionRootFolder + "OurProject-" + version + ".zip"; //name of the output zip file

            Console.WriteLine("Compressing files");
            Compress(outputFile, files, rootFolderInZip, relPathToSolutionRootFolder);
            Console.WriteLine("Finished");
        }

        public static string GetVersion(string file)
        {
            if (!File.Exists(file))
            {
                Console.WriteLine("Could not find file: " + Path.GetFullPath(file));
                return null;
            }
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(file);

            return fvi.FileVersion;
        }

        public static void GetDependencies(string inFolder, string module, ref List<string> dependencyList, bool bRecursive = true)
        {
            string dename, modName;

            Assembly assembly = Assembly.LoadFrom(inFolder + module);

            foreach (AssemblyName assemblyName in assembly.GetReferencedAssemblies())
            {
                modName = assemblyName.Name + ".dll";
                dename = inFolder + modName;
                if (File.Exists(dename) && !dependencyList.Exists(name => name == dename))
                {
                    dependencyList.Add(dename);
                    if (bRecursive)
                        GetDependencies(inFolder, modName, ref dependencyList, false);
                }
            }
        }

        public static List<string> GetFilesInFolder(string folder, bool bRecursive, string filter = "*.*")
        {
            List<string> files = new List<string>();

            if (bRecursive)
                files.AddRange(Directory.EnumerateFiles(folder, filter, SearchOption.AllDirectories));
            else
                files.AddRange(Directory.EnumerateFiles(folder));
            return files;
        }

        public static void Compress(string outputFilename, List<string> files, string rootFolderInZip, string relPathToSolutionRootFolder)
        {
            uint numFilesAdded = 0;
            double totalNumFiles = (double)files.Count;
            using (FileStream zipToOpen = new FileStream(outputFilename, FileMode.Create))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    foreach (string file in files)
                    {
                        if (File.Exists(file))
                        {
                            archive.CreateEntryFromFile(file, rootFolderInZip + Path.GetFileName(file));
                            numFilesAdded++;
                        }
                        else Console.WriteLine("Couldn't find file: {0}", file);

                        Console.WriteLine("\rProgress: {0:F2}%", 100.0 * ((double)numFilesAdded) / totalNumFiles);
                    }
                    Console.WriteLine("\nSaving {0} files in  {1}", numFilesAdded, Path.GetFullPath(outputFilename));
                }
            }
        }
    }
}