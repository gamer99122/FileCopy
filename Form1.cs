using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileCopy
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            btnCopy.Enabled = false;

            string sourceDirectory = @"C:\HIS2";
            string targetDirectory = @"H:\HIS2Copy";

            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            Stopwatch stopwatch = Stopwatch.StartNew();

            CopyAll(diSource, diTarget);

            stopwatch.Stop();
            Console.WriteLine("Total elapsed time: {0} milliseconds", stopwatch.ElapsedMilliseconds);
            lblStatus.Text = $"Total elapsed time: {stopwatch.ElapsedMilliseconds/1000} 秒";
            Application.DoEvents();

            btnCopy.Enabled = true;
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                try
                {
                    string targetPath = Path.Combine(target.FullName, fi.Name);
                    if (File.Exists(targetPath))
                    {
                        FileInfo targetFi = new FileInfo(targetPath);
                        if (fi.LastWriteTimeUtc == targetFi.LastWriteTimeUtc)
                        {
                            Console.WriteLine("File {0} has not changed, skipping...", fi.Name);
                            continue;
                        }
                    }
                    fi.CopyTo(targetPath, true);
                }
                catch (IOException)
                {
                    Console.WriteLine("File {0} is in use, skipping...", fi.Name);
                    continue;
                }
            }

            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }



    }
}
