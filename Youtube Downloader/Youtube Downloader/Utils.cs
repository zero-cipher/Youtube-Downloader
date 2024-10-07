using System;
using System.Collections.Generic;
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
    }
}
