using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Youtube_Downloader
{
    static class Utils
    {

        public static string SelectFolder(string defaultValue)
        {
            // create dialog object
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();

            // initialize dialog
            dialog.InitialDirectory = defaultValue;
            dialog.IsFolderPicker = true;               // select folder
            
            // show dialog
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                return dialog.FileName;
            }

            return defaultValue;
        }

        internal static long GetFileSize(string saveFileName)
        {
            FileInfo fi = new FileInfo(saveFileName);
            if (fi.Exists)
                return fi.Length;

            return 0;
        }

        internal static void DeleteFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return;
            FileInfo fi = new FileInfo(fileName);
            if (fi.Exists)
                fi.Delete();
        }
    }
}
