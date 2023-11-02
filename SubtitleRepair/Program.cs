using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Abso_Tech_dependency;
using Microsoft.Win32;

namespace SubtitleRepair
{
   public static class Vars
    {
        public static double Version = 1.4;
        public static string RegPath = "Software\\Abso Tech\\Subtitle Repair";
        public static string Error = null;
        public static bool AllMessages = false;
        public static int FileCounter = 0;
        public static int FileCounterOK = 0;
        public static string[] AllowedExtensions = {".srt", ".lrc", ".ssa", ".sub", ".txt" };
    }
    public class Program
    {
        
        static void Main(string[] args)
        {
            if (Registry.CurrentUser.CreateSubKey("Software\\Abso Tech\\Subtitle Repair").GetValue("AllMessages") == null)
            {
                Registry.CurrentUser.CreateSubKey("Software\\Abso Tech\\Subtitle Repair").SetValue("AllMessages", 0);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
           
            Vars.Error = AbsoDep.SelfVerify();
            if (Vars.Error != null)
            {
                MessageBox.Show(Vars.Error, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(50);
            }

            if (AbsoDep.CheckDuplicateProcess() == true)
            {
                MessageBox.Show("Another instance of the application is running!\n" , "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }
            if (args.Length == 0)
            {
                Console.WriteLine("Please add at least one file!\nUsage: \"-file1 , -file2\"");
                Application.Run(new MainForm());
            }

            if (Convert.ToInt32(Registry.CurrentUser.CreateSubKey("Software\\Abso Tech\\Subtitle Repair").GetValue("AllMessages")) == 1)
                Vars.AllMessages = true;
            else
                Vars.AllMessages = false;
            foreach (string filename in args)
            {
                if (ExtensionCheck.CheckExtension(filename))
                {
                    Vars.FileCounter++;
                    try
                    {
                        Vars.FileCounterOK++;
                        File.SetAttributes(filename, FileAttributes.Normal);
                        string text = File.ReadAllText(filename, Encoding.Default);
                        text = text.Replace("þ", "ț");
                        text = text.Replace("ã", "ă");
                        text = text.Replace("º", "ș");
                        text = text.Replace("ª", "Ș");
                        text = text.Replace("Þ", "Ț");
                        File.WriteAllText(filename, text, Encoding.UTF8);
                        Console.WriteLine("Letters successfully replaced in " + filename + "!");
                        if (Vars.AllMessages == true)
                            MessageBox.Show("Letters successfully replaced in " + filename + "!", "Completed!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch
                    {
                        MessageBox.Show("Error replacing letters in  " + filename + "!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Environment.Exit(5);
                    }
                }
                else
                {
                    MessageBox.Show("Extension not supported (only plain text files are supported).\nFile:  " + filename + "  .", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(5);
                }
            }
            if (Vars.AllMessages == false)
            {
                if (Vars.FileCounter == Vars.FileCounterOK)
                {
                    MessageBox.Show("Letters successfully replaced in all files!", "Completed!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Letters successfully replaced in " + Vars.FileCounterOK.ToString() +"/" + Vars.FileCounter.ToString() + " files!", "Completed!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
