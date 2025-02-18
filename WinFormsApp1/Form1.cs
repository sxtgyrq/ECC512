using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        Random rm = new Random(DateTime.Now.GetHashCode());
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;  // 去除边框
            this.WindowState = FormWindowState.Maximized; // 最大化窗口

            this.rmValue = this.rm.Next(0, this.label1.Text.Length);
        }
        int state = 0;
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            this.button1.Left = (this.ClientSize.Width - this.button1.Width) / 2;
            button1.Top = (this.ClientSize.Height / 2 + 2 * button1.Height);
            this.button1.Text = "我已经知晓";

            this.textBox1.Left = (this.ClientSize.Width - this.button1.Width) / 2;
            this.textBox1.Top = button1.Top - this.textBox1.Height - 5;

            //this.label1.Font.



            label1.AutoSize = false;  // 关闭自动调整大小
            //label1.Width = 1500;       // 设定固定宽度
            //label1.Height = 300;      // 设定足够的高度
            label1.AutoSize = true;
            label1.TextAlign = ContentAlignment.MiddleCenter;  // 居中对齐
            this.label1.Left = (this.ClientSize.Width - this.label1.Width) / 2;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

         


        int rmValue = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            this.startValue = 0;
            switch (state)
            {
                case 0:
                    {

                        if (this.textBox1.Text.Trim() == this.label1.Text[rmValue].ToString())
                        {
                            this.state++;
                            this.label1.Text = "想想购买无人机时给转账的那个王老板";
                            this.rmValue = this.rm.Next(0, this.label1.Text.Length);
                            this.label1.Left = (this.ClientSize.Width - this.label1.Width) / 2;
                        }
                        else
                        {
                            this.textBox1.Text = "";
                            MessageBox.Show($"请输入{this.label1.Text[rmValue].ToString()}");
                        }
                    }; break;
                case 1:
                    {
                        if (this.textBox1.Text.Trim() == this.label1.Text[rmValue].ToString())
                        {
                            this.state++;
                            this.label1.Text = $"想想那个假得不能再假的马斯克演讲";

                            this.rmValue = this.rm.Next(0, this.label1.Text.Length);
                            this.label1.Left = (this.ClientSize.Width - this.label1.Width) / 2;
                        }
                        else
                        {
                            this.textBox1.Text = "";
                            MessageBox.Show($"请输入{this.label1.Text[rmValue].ToString()}");
                        }
                    }; break;
                case 2:
                    {
                        if (this.textBox1.Text.Trim() == this.label1.Text[rmValue].ToString())
                        {
                            this.state++;
                            this.label1.Text = $"YouTube视频二维码指向网页双倍返还";

                            this.rmValue = this.rm.Next(0, this.label1.Text.Length);
                            this.label1.Left = (this.ClientSize.Width - this.label1.Width) / 2;
                        }
                        else
                        {
                            this.textBox1.Text = "";
                            MessageBox.Show($"请输入{this.label1.Text[rmValue].ToString()}");
                        }
                    }; break;
                case 3:
                    {
                        if (this.textBox1.Text.Trim() == this.label1.Text[rmValue].ToString())
                        {
                            this.state++;
                            this.label1.Text = $"提这些历史不是让自己自责是让你警惕";

                            this.rmValue = this.rm.Next(0, this.label1.Text.Length);
                            this.label1.Left = (this.ClientSize.Width - this.label1.Width) / 2;
                        }
                        else
                        {
                            this.textBox1.Text = "";
                            MessageBox.Show($"请输入{this.label1.Text[rmValue].ToString()}");
                        }
                    }; break;
                case 4:
                    {
                        if (this.textBox1.Text.Trim() == this.label1.Text[rmValue].ToString())
                        {
                            this.state++;
                            this.label1.Text = $"You_are_not_sharp_enough_and_lack_awareness.";

                            this.rmValue = this.rm.Next(0, this.label1.Text.Length);
                            this.label1.Left = (this.ClientSize.Width - this.label1.Width) / 2;
                        }
                        else
                        {
                            this.textBox1.Text = "";
                            MessageBox.Show($"请输入{this.label1.Text[rmValue].ToString()}");
                        }
                    }; break;
                case 5:
                    {
                        if (this.textBox1.Text.Trim() == this.label1.Text[rmValue].ToString())
                        {
                            this.state++;
                            this.label1.Text = $"当开始怀疑是骗局时，百分百是骗局。";

                            this.rmValue = this.rm.Next(0, this.label1.Text.Length);
                            this.label1.Left = (this.ClientSize.Width - this.label1.Width) / 2;
                        }
                        else
                        {
                            this.textBox1.Text = "";
                            MessageBox.Show($"请输入{this.label1.Text[rmValue].ToString()}");
                        }
                    }; break;
                case 6:
                    {
                        if (this.textBox1.Text.Trim() == this.label1.Text[rmValue].ToString())
                        {
                            this.state++;
                            this.label1.Text = $"先问自己给谁转？为什么转？";

                            this.rmValue = this.rm.Next(0, this.label1.Text.Length);
                            this.label1.Left = (this.ClientSize.Width - this.label1.Width) / 2;
                        }
                        else
                        {
                            this.textBox1.Text = "";
                            MessageBox.Show($"请输入{this.label1.Text[rmValue].ToString()}");
                        }
                    }; break;
                case 7:
                    {
                        if (this.textBox1.Text.Trim() == this.label1.Text[rmValue].ToString())
                        {
                            this.state++;
                            this.label1.Text = $"陌生人陌生软件的地址=骗局";

                            this.rmValue = this.rm.Next(0, this.label1.Text.Length);
                            this.label1.Left = (this.ClientSize.Width - this.label1.Width) / 2;
                        }
                        else
                        {
                            this.textBox1.Text = "";
                            MessageBox.Show($"请输入{this.label1.Text[rmValue].ToString()}");
                        }
                    }; break;
                case 8:
                    {
                        if (this.textBox1.Text.Trim() == this.label1.Text[rmValue].ToString())
                        {
                            this.state++;
                            this.label1.Text = $"高回报=骗局";

                            this.rmValue = this.rm.Next(0, this.label1.Text.Length);
                            this.label1.Left = (this.ClientSize.Width - this.label1.Width) / 2;
                        }
                        else
                        {
                            this.textBox1.Text = "";
                            MessageBox.Show($"请输入{this.label1.Text[rmValue].ToString()}");
                        }
                    }; break;
                case 9:
                    {
                        if (this.textBox1.Text.Trim() == this.label1.Text[rmValue].ToString())
                        {
                            this.state++;
                            this.label1.Text = $"三思而后行思金额思对象思原因";

                            this.rmValue = this.rm.Next(0, this.label1.Text.Length);
                            this.label1.Left = (this.ClientSize.Width - this.label1.Width) / 2;
                        }
                        else
                        {
                            this.textBox1.Text = "";
                            MessageBox.Show($"请输入{this.label1.Text[rmValue].ToString()}");
                        }
                    }; break;
                case 10:
                    {
                        if (this.textBox1.Text.Trim() == this.label1.Text[rmValue].ToString())
                        {
                            this.state++;
                            this.label1.Text = $"再等三个小时又何妨";

                            this.rmValue = this.rm.Next(0, this.label1.Text.Length);
                            this.label1.Left = (this.ClientSize.Width - this.label1.Width) / 2;
                        }
                        else
                        {
                            this.textBox1.Text = "";
                            MessageBox.Show($"请输入{this.label1.Text[rmValue].ToString()}");
                        }
                    }; break;
                case 11:
                    {
                        this.state++;
                        this.button1.Text = "退出";
                        this.label1.Text = $"警惕的自己祝您转账愉快。";
                        this.rmValue = this.rm.Next(0, this.label1.Text.Length);
                    }; break;
                case 12:
                    {
                        if (this.textBox1.Text.Trim() == this.label1.Text[rmValue].ToString())
                        {
                            Process.Start("cmd.exe", "/c start \"\" \"D:\\Program Files (x86)\\Electrum\\electrum-4.4.6-main.exe\"");

                            this.Dispose();

                        }
                        else
                        {
                            this.textBox1.Text = "";
                            MessageBox.Show($"请输入{this.label1.Text[rmValue].ToString()}");
                        }
                    }; break;

            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        string labelText { get { return this.label1.Text; } }

        int startValue = 0;
        private void label1_MouseClick(object sender, MouseEventArgs e)
        {
            this.startValue = startValue % this.label1.Text.Length;

            this.textBox1.Text = this.label1.Text[this.startValue].ToString();
            this.startValue++;

            if (this.startValue % 2 == 0)
            { this.BackColor = Color.AliceBlue; }
           // else { this.BackColor = Color.Red; }

            //int clickedIndex = GetClickedCharacterIndex(label1, e.Location);
            //if (clickedIndex != -1)
            //{
            //    char clickedChar = labelText[clickedIndex];
            //    MessageBox.Show($"You clicked on: '{clickedChar}' at index {clickedIndex}");
            //}
        }


        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            //this.startValue = startValue % this.label1.Text.Length;

            //this.textBox1.Text = this.label1.Text[this.startValue].ToString();
            //this.startValue++;

            //if (this.startValue % 2 == 0)
            //{ this.BackColor = Color.AliceBlue; }
            //else { this.BackColor = Color.Red; }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            //if (this.label1.Text.Length > 0)
            //{
            //    float percentage = (float)e.X / this.ClientSize.Width;
            //    this.startValue = Convert.ToInt32(percentage * this.label1.Text.Length) % this.label1.Text.Length;
            //    if (this.textBox1.Text.Length != 1)
            //        this.textBox1.Text = $"Mouse X: {e.X}, index: {this.startValue + 1},{this.label1.Text[this.startValue].ToString()}%";
            //}
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.label1.Text.Length > 0)
            {
                float percentage = (float)e.X / this.label1.Width;
                this.startValue = Convert.ToInt32(percentage * this.label1.Text.Length) % this.label1.Text.Length;
                if (this.textBox1.Text.Length != 1)
                    this.textBox1.Text = $"----{this.label1.Text[this.startValue].ToString()}-------";
            }
        }
    }
}
