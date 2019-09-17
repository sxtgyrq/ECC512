using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormSecret
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false); 
            Application.Run(new Form1( ));
        }

        static void doStep(int step)
        {
            switch (step)
            {
                case 0:
                    {
                        FormDelegate.doNext dn = new FormDelegate.doNext(doStep);
                        var f = new SecretInfo();
                        f.ShowDialog();

                    }; break;
                case 1:
                    {
                        
                    }; break;
                case 2: { }; break;
                case 3: { }; break;
            }
        }
    }
}
