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
            this.FormBorderStyle = FormBorderStyle.None;  // ȥ���߿�
            this.WindowState = FormWindowState.Maximized; // ��󻯴���

            this.rmValue = this.rm.Next(0, this.label1.Text.Length);
        }
        int state = 0;
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            this.button1.Left = (this.ClientSize.Width - this.button1.Width) / 2;
            button1.Top = (this.ClientSize.Height / 2 + 2 * button1.Height);
            this.button1.Text = "���Ѿ�֪��";

            this.textBox1.Left = (this.ClientSize.Width - this.button1.Width) / 2;
            this.textBox1.Top = button1.Top - this.textBox1.Height - 5;

            //this.label1.Font.



            label1.AutoSize = false;  // �ر��Զ�������С
            //label1.Width = 1500;       // �趨�̶����
            //label1.Height = 300;      // �趨�㹻�ĸ߶�
            label1.AutoSize = true;
            label1.TextAlign = ContentAlignment.MiddleCenter;  // ���ж���
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
                            this.label1.Text = "���빺�����˻�ʱ��ת�˵��Ǹ����ϰ�";
                            this.rmValue = this.rm.Next(0, this.label1.Text.Length);
                            this.label1.Left = (this.ClientSize.Width - this.label1.Width) / 2;
                        }
                        else
                        {
                            this.textBox1.Text = "";
                            MessageBox.Show($"������{this.label1.Text[rmValue].ToString()}");
                        }
                    }; break;
                case 1:
                    {
                        if (this.textBox1.Text.Trim() == this.label1.Text[rmValue].ToString())
                        {
                            this.state++;
                            this.label1.Text = $"�����Ǹ��ٵò����ټٵ���˹���ݽ�";

                            this.rmValue = this.rm.Next(0, this.label1.Text.Length);
                            this.label1.Left = (this.ClientSize.Width - this.label1.Width) / 2;
                        }
                        else
                        {
                            this.textBox1.Text = "";
                            MessageBox.Show($"������{this.label1.Text[rmValue].ToString()}");
                        }
                    }; break;
                case 2:
                    {
                        if (this.textBox1.Text.Trim() == this.label1.Text[rmValue].ToString())
                        {
                            this.state++;
                            this.label1.Text = $"YouTube��Ƶ��ά��ָ����ҳ˫������";

                            this.rmValue = this.rm.Next(0, this.label1.Text.Length);
                            this.label1.Left = (this.ClientSize.Width - this.label1.Width) / 2;
                        }
                        else
                        {
                            this.textBox1.Text = "";
                            MessageBox.Show($"������{this.label1.Text[rmValue].ToString()}");
                        }
                    }; break;
                case 3:
                    {
                        if (this.textBox1.Text.Trim() == this.label1.Text[rmValue].ToString())
                        {
                            this.state++;
                            this.label1.Text = $"����Щ��ʷ�������Լ����������㾯��";

                            this.rmValue = this.rm.Next(0, this.label1.Text.Length);
                            this.label1.Left = (this.ClientSize.Width - this.label1.Width) / 2;
                        }
                        else
                        {
                            this.textBox1.Text = "";
                            MessageBox.Show($"������{this.label1.Text[rmValue].ToString()}");
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
                            MessageBox.Show($"������{this.label1.Text[rmValue].ToString()}");
                        }
                    }; break;
                case 5:
                    {
                        if (this.textBox1.Text.Trim() == this.label1.Text[rmValue].ToString())
                        {
                            this.state++;
                            this.label1.Text = $"����ʼ������ƭ��ʱ���ٷְ���ƭ�֡�";

                            this.rmValue = this.rm.Next(0, this.label1.Text.Length);
                            this.label1.Left = (this.ClientSize.Width - this.label1.Width) / 2;
                        }
                        else
                        {
                            this.textBox1.Text = "";
                            MessageBox.Show($"������{this.label1.Text[rmValue].ToString()}");
                        }
                    }; break;
                case 6:
                    {
                        if (this.textBox1.Text.Trim() == this.label1.Text[rmValue].ToString())
                        {
                            this.state++;
                            this.label1.Text = $"�����Լ���˭ת��Ϊʲôת��";

                            this.rmValue = this.rm.Next(0, this.label1.Text.Length);
                            this.label1.Left = (this.ClientSize.Width - this.label1.Width) / 2;
                        }
                        else
                        {
                            this.textBox1.Text = "";
                            MessageBox.Show($"������{this.label1.Text[rmValue].ToString()}");
                        }
                    }; break;
                case 7:
                    {
                        if (this.textBox1.Text.Trim() == this.label1.Text[rmValue].ToString())
                        {
                            this.state++;
                            this.label1.Text = $"İ����İ������ĵ�ַ=ƭ��";

                            this.rmValue = this.rm.Next(0, this.label1.Text.Length);
                            this.label1.Left = (this.ClientSize.Width - this.label1.Width) / 2;
                        }
                        else
                        {
                            this.textBox1.Text = "";
                            MessageBox.Show($"������{this.label1.Text[rmValue].ToString()}");
                        }
                    }; break;
                case 8:
                    {
                        if (this.textBox1.Text.Trim() == this.label1.Text[rmValue].ToString())
                        {
                            this.state++;
                            this.label1.Text = $"�߻ر�=ƭ��";

                            this.rmValue = this.rm.Next(0, this.label1.Text.Length);
                            this.label1.Left = (this.ClientSize.Width - this.label1.Width) / 2;
                        }
                        else
                        {
                            this.textBox1.Text = "";
                            MessageBox.Show($"������{this.label1.Text[rmValue].ToString()}");
                        }
                    }; break;
                case 9:
                    {
                        if (this.textBox1.Text.Trim() == this.label1.Text[rmValue].ToString())
                        {
                            this.state++;
                            this.label1.Text = $"��˼������˼���˼����˼ԭ��";

                            this.rmValue = this.rm.Next(0, this.label1.Text.Length);
                            this.label1.Left = (this.ClientSize.Width - this.label1.Width) / 2;
                        }
                        else
                        {
                            this.textBox1.Text = "";
                            MessageBox.Show($"������{this.label1.Text[rmValue].ToString()}");
                        }
                    }; break;
                case 10:
                    {
                        if (this.textBox1.Text.Trim() == this.label1.Text[rmValue].ToString())
                        {
                            this.state++;
                            this.label1.Text = $"�ٵ�����Сʱ�ֺη�";

                            this.rmValue = this.rm.Next(0, this.label1.Text.Length);
                            this.label1.Left = (this.ClientSize.Width - this.label1.Width) / 2;
                        }
                        else
                        {
                            this.textBox1.Text = "";
                            MessageBox.Show($"������{this.label1.Text[rmValue].ToString()}");
                        }
                    }; break;
                case 11:
                    {
                        this.state++;
                        this.button1.Text = "�˳�";
                        this.label1.Text = $"������Լ�ף��ת����졣";
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
                            MessageBox.Show($"������{this.label1.Text[rmValue].ToString()}");
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
