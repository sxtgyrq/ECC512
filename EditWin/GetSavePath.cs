using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EditWin
{
    public class GetSavePath
    {
        public static string Get(out bool success)
        {
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();
            path.Dispose();
            success = true;
            return path.SelectedPath;
        }
    }
}
