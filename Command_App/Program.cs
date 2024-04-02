using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Command_App
{
    internal class Program
    {
        public static string sourceFolderPath = @"\Assets\sourceFolder\";
        public static string destinationFolderPath = @"\Assets\destinationFolder\";
        public static string imagePath = sourceFolderPath + "image.png";
        public static string sourcePath = AssemblyDirectory + imagePath;
        public static string destinationPath = AssemblyDirectory + destinationFolderPath;
        static void Main(string[] args)
        {

            var path_s = sourceFolderPath;
            var root = Path.GetPathRoot(path_s);
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\" +
            DateTime.UtcNow.ToString("yyyyMMdd");

            // Display title as the C# console calculator app.
            Console.WriteLine("Commands\r");
            Console.WriteLine("------------------------\n");

            // Ask the user to type the second number.
            Console.WriteLine("Type command, and then press Enter");

            // Ask the user to choose an option.
            Console.WriteLine("Choose an option from the following list:");
            Console.WriteLine("\t1 - createFolder");
            Console.WriteLine("\t2 - queryFolder");
            Console.WriteLine("\t3 - copy");
            Console.WriteLine("\t4 - delete");
            Console.WriteLine("\t5 - download");
            Console.WriteLine("\t6 - wait");
            Console.WriteLine("\t7 - rowCount");
            Console.Write("Your option?\n");
            Console.Write("Type command ");

            if (args.Length == 0)
            {
                args = new string[] { "select" };
            }

            string commandName = Console.ReadLine();
            string folderPath = string.Empty;
            string filePath = string.Empty;
            string sourceFile = string.Empty;
            string outputFile = string.Empty;
            switch (commandName.ToLower())
            {
                case "createfolder":
                    folderPath = string.Empty;
                    sourceFile = string.Empty;
                    outputFile = string.Empty;
                    Console.WriteLine("Please give the Folder Path!");
                    folderPath = Console.ReadLine();
                    Console.WriteLine("Please give the New Folder Name!");
                    string folderName = Console.ReadLine();
                    CreateDirectory(folderPath, folderName);
                    Console.ReadLine();
                    break;
                case "queryfolder":
                    folderPath = string.Empty;
                    sourceFile = string.Empty;
                    outputFile = string.Empty;
                    Console.WriteLine("Please give the Folder Path!");
                    folderPath = Console.ReadLine();
                    QueryFolder(folderPath);
                    Console.WriteLine("Please see the all listed files!");
                    Console.ReadLine();
                    break;
                case "copy":
                    folderPath = string.Empty;
                    sourceFile = string.Empty;
                    outputFile = string.Empty;
                    Console.WriteLine("please give the source file!");
                    sourceFile = Console.ReadLine();
                    Console.WriteLine("please give the destination file!");
                    string destinationFile = Console.ReadLine();
                    CopyFile(sourceFile, destinationFile);
                    Console.WriteLine("command arguments for copy");
                    Console.ReadLine();
                    break;
                case "delete":
                    folderPath = string.Empty;
                    filePath = string.Empty;
                    outputFile = string.Empty;
                    Console.WriteLine("please give the file path!");
                    filePath = Console.ReadLine();
                    DeleteFile(filePath, false);
                    Console.WriteLine("command arguments for delete");
                    Console.ReadLine();
                    break;
                case "download":
                    folderPath = string.Empty;
                    sourceFile = string.Empty;
                    outputFile = string.Empty;
                    Console.WriteLine("please give the source url!");
                    sourceFile = Console.ReadLine();
                    Console.WriteLine("please give the output file!");
                    outputFile = Console.ReadLine();
                    DownloadFile(sourceFile, outputFile);
                    Console.WriteLine("command arguments for download");
                    Console.ReadLine();
                    break;
                case "wait":
                    folderPath = string.Empty;
                    Console.WriteLine("please enter wait time in second!");
                    int waittimeinsecond = Convert.ToInt32(Console.ReadLine());
                    WaitTimeInSecond(waittimeinsecond);
                    Console.WriteLine("command arguments for wait");
                    Console.ReadLine();
                    break;
                case "rowcount":
                    folderPath = string.Empty;
                    sourceFile = string.Empty;
                    Console.WriteLine("please give the source File!");
                    sourceFile = Console.ReadLine();
                    Console.WriteLine("please give searched string text!");
                    string searchString = Console.ReadLine();
                    int a;
                    ConditionalCountRowsFile(sourceFile, searchString, out a);
                    Console.WriteLine("Total rows count " + a);
                    Console.WriteLine("command arguments for rowCount");
                    Console.ReadLine();
                    break;
                default:
                    break;
            }
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var dir = AppDomain.CurrentDomain.BaseDirectory;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return System.IO.Path.GetDirectoryName(path);
            }
        }

        public static void CopyFile(string sourceFile, string destinationFile)
        {
            try
            {
                var fileName = System.IO.Path.GetFileName(sourceFile);
                var targetPath = System.IO.Path.GetDirectoryName(destinationFile);
                string destFile = System.IO.Path.Combine(targetPath, fileName);
                System.IO.File.Copy(sourceFile, destFile, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("CopyFile Exception " + ex);
            }
        }

        public static bool DeleteFile(string filePath, bool isDeleted)
        {
            try
            {
                var directoryName = System.IO.Path.GetDirectoryName(filePath);
                var fileName = System.IO.Path.GetFileName(filePath);
                DirectoryInfo dir = new DirectoryInfo(directoryName);
                foreach (FileInfo fi in dir.GetFiles())
                {
                    if (!string.IsNullOrEmpty(fi.Name) && fi.Name == fileName)
                    {
                        fi.Delete();
                        isDeleted = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DeleteFile Exception " + ex);
            }
            return isDeleted;
        }

        public static void DownloadFile(string sourceURL, string outputFile)
        {

            using (var client = new WebClient())
            {

                var of = string.Empty;
                if (outputFile.Contains(":"))
                {
                    of = outputFile;
                }
                else
                {
                    var directoryName = System.IO.Path.GetDirectoryName(sourceURL);
                    of = directoryName + "//" + outputFile;
                }

                string url = sourceURL;
                string savePath = of;

                client.DownloadFileCompleted += (s, e) => Console.WriteLine("Download file completed.");
                client.DownloadProgressChanged += (s, e) => Console.WriteLine($"Downloading {e.ProgressPercentage}%");
                client.DownloadFileAsync(new Uri(url), savePath);

                Thread.Sleep(5000);
            }
        }

        public static void WaitTimeInSecond(int waittimeinsecond)
        {
            Thread.Sleep(waittimeinsecond);
        }

        public static void ConditionalCountRowsFile(string sourceFile, string searchString, out int c)
        {
            c = 0;
            try
            {
                var directoryName = System.IO.Path.GetDirectoryName(sourceFile);
                var fileName = searchString;
                DirectoryInfo dir = new DirectoryInfo(directoryName);
                int rowCounter = 0;
                foreach (FileInfo fi in dir.GetFiles())
                {
                    rowCounter++;
                    if (!string.IsNullOrEmpty(fi.Name) && fi.Name.Contains(fileName))
                    {
                        c = rowCounter;
                        Console.WriteLine("searched string in rows " + rowCounter);
                        return;
                    }
                    c = rowCounter;
                    Console.WriteLine("searched string in rows " + rowCounter);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ConditionalCountRowsFile Exception " + ex);
            }
        }

        public static void CreateDirectory(string folderPath, string folderName)
        {
            bool isexits = false;
            var fullPath = folderPath + "/" + folderName;
            if (!System.IO.Directory.Exists(fullPath))
            {
                System.IO.Directory.CreateDirectory(fullPath);
                isexits = true;
                Console.WriteLine("Directory created succesfully!");
            }
            if (!isexits)
            {
                Console.WriteLine("Directory already exists with same name!");
            }
        }

        public static void QueryFolder(string folderPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(folderPath);
                int counter = 0;
                foreach (FileInfo fi in dir.GetFiles())
                {
                    counter++;
                    Console.WriteLine(counter + " " + fi.Name.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("QueryFolder Exception " + ex);
            }
        }

        public static void ReplaceFile(string FileToMoveAndDelete, string FileToReplace, string BackupOfFileToReplace)
        {
            try
            {
                File.Replace(FileToMoveAndDelete, FileToReplace, BackupOfFileToReplace, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ReplaceFile Exception " + ex);
            }

        }
    }
}
