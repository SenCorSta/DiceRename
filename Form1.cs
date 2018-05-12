using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiceRename
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static string foldername = "";
        static List<string> FullNameList = new List<string>();
        static List<string> ExtensionnameList = new List<string>();
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog()==DialogResult.OK)
            {
                foldername = fbd.SelectedPath;
                textBox1.Text = foldername;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                if (textBox1.Text != "")
                {
                    DirectoryInfo di = new DirectoryInfo(textBox1.Text);
                    if (di.Exists)
                    {
                        FullNameList = new List<string>();
                       
                        FindAllFile(di.FullName, checkBox1.Checked);
                        dataGridView1.Rows.Clear();
                        foreach (string item in FullNameList)
                        {
                            dataGridView1.Rows.Add(item);
                        }

                        //整理扩展名
                        bool isAllExtensionname = false;
                        ExtensionnameList = new List<string>();
                        foreach (string item in textBox4.Text.ToLower().Split(','))
                        {
                            ExtensionnameList.Add(item);
                        }
                        if (ExtensionnameList.Contains("*"))
                        {
                            isAllExtensionname = true;
                        }

                        //开始替换
                        int fixcount = 0;
                        string find = textBox2.Text;
                        string fix = textBox3.Text;
                        foreach (string item in FullNameList)
                        {
                            FileInfo fi = new FileInfo(item);
                            if (fi.Exists)
                            {
                                int index = fi.Name.LastIndexOf('.');
                                string realname = fi.Name.Substring(0, index);
                                string extensionname = fi.Name.Substring(index + 1, fi.Name.Length - index - 1);
                                if (isAllExtensionname||ExtensionnameList.Contains(extensionname))
                                {
                                    if (realname.Contains(find))
                                    {
                                        File.Move(fi.FullName, fi.Directory + "\\" + realname.Replace(find, fix) + "." + extensionname);
                                        fixcount++;
                                    }
                                }
                                
                            }
                        }
                        MessageBox.Show("替换成功!\n一共替换了" + fixcount + "处.");

                    }
                    else
                    {
                        MessageBox.Show("无效的目录!");
                    }
                }
                else
                {
                    MessageBox.Show("目录不能为空!");
                }
            }
            else
            {
                MessageBox.Show("查找的字符不能为空!");
            }
            
            
        }

        public void FindAllFile(string strPath, bool isAll) 
        {
            DirectoryInfo di=new DirectoryInfo(strPath);
            foreach (FileInfo item in di.GetFiles())
            {
                FullNameList.Add(item.FullName);
            }
            if (isAll)
            {
                foreach (DirectoryInfo item in di.GetDirectories())
                {
                    FindAllFile(item.FullName, isAll);
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            button4.Show();
            this.Width = 975;
            button3.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button3.Show();
            this.Width = 524;
            button4.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "版本号:Alpha-0.21"
                + "\n\n"
                + "开发日志:\n"
                + "2015年6月5日 发布Alpha-0.1版,实现基本功能(批量替换文件名).\n"
                + "2015年6月6日 Alpha-0.11 +保护扩展名不会被碰巧替换.\n"
                + "2015年6月6日 Alpha-0.21 +支持指定格式替换.\n"








                );
        }
    }
}
    