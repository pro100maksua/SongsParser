using Microsoft.Win32;
using SongsParser.Interfaces;

namespace SongsParser.Services
{
    public class BrowserService : IBrowserService
    {
        public string BrowseFile()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*",
                DefaultExt = ".csv"
            };
            
            var fileName = string.Empty;
            var result = dialog.ShowDialog();
            if (result == true && !string.IsNullOrWhiteSpace(dialog.FileName))
            {
                fileName = dialog.FileName;
            }

            return fileName;
        }
    }
}