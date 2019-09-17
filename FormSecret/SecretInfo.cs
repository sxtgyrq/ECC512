using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormSecret
{
    public partial class SecretInfo : Form
    {

        public SecretInfo()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // this.dn(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();

        }

        private void SecretInfo_DragDrop(object sender, DragEventArgs e)
        {
            //var ss = (string[])e.Data.GetData(DataFormats.FileDrop);
            //           MessageBox.Show(ss[0]);
        }

        private void SecretInfo_DragEnter(object sender, DragEventArgs e)
        {
            var ss = (string[])e.Data.GetData(DataFormats.FileDrop);
            //  MessageBox.Show(ss[0]);
            if (this.textBox1.Focused)
            {
                try
                {
                    var v = ECCMain.LockAndKeyRead.Get(ss[0]);
                    this.textBox1.Text = v;
                }
                catch
                { }
            }
            else if (this.textBox2.Focused)
            {
                try
                {
                    var v = ECCMain.LockAndKeyRead.Get(ss[0]);
                    this.textBox1.Text = v;
                }
                catch
                { }
            }
            else if (this.textBox3.Focused)
            {
                this.textBox1.Text = ss[0];
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //if (ECCMain.Format.regexOfPublic64.IsMatch(this.textBox1.Text))
            //{
            //    this.textBox1.BackColor = System.Drawing.Color.Green;
            //}
            //else
            //{
            //    this.textBox1.BackColor = System.Drawing.Color.Red;
            //}
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox3.Text=  dialog.SelectedPath;
            }
        }
    }
}
