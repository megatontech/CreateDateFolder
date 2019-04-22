using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CreateDateFolder
{
    public partial class Form1 : Form
    {
        public string BaseFolder = "";
        public string BaseIndexContent = "";
        public string TemplateContent = "";

        public Form1()
        {
            InitializeComponent();
            BaseFolder = AppDomain.CurrentDomain.BaseDirectory;//获取当前应用程序所在目录的路径，最后包含“\”；
            BaseIndexContent ="<input type='hidden' id='hidDate' value='"+ DateTime.Now.ToShortDateString()+"' />";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// year
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            DateTime current = DateTime.Now;
            int year = current.Year;
            string yearFolder = BaseFolder + year;
            CreateFolder(yearFolder);
            for (int i = 1; i < 13; i++)
            {
                string monthFolder = yearFolder + "\\" + i;
                CreateFolder(monthFolder);
                int days = DateTime.DaysInMonth(year, i);
                for (int j = 1; j < days + 1; j++)
                {
                    string dayFolder = monthFolder + "\\" + j;
                    CreateFolder(dayFolder);
                }
            }
        }

        /// <summary>
        /// month
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            DateTime current = DateTime.Now;
            int year = current.Year;
            string yearFolder = BaseFolder + year;
            int month = current.Month;
            string monthFolder = yearFolder + "\\" + month;
            CreateFolder(monthFolder);
            int days = DateTime.DaysInMonth(year, month);
            for (int j = 1; j < days + 1; j++)
            {
                string dayFolder = monthFolder + "\\" + j;
                CreateFolder(dayFolder);
            }
        }

        /// <summary>
        /// index
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(BaseFolder))
            {
                var directory = getDirectory(BaseFolder);
                foreach (var item in directory)
                {
                    CreateIndexHtml(item);
                }
            }
        }

        private void CreateIndexHtml(string path)
        {
            try
            {
                // Determine whether the directory exists.
                if (!Directory.Exists(path))
                {
                    // Create the directory it does not exist.
                    Directory.CreateDirectory(path);
                }
                string fileStr = path + "\\index.html";
                if (!File.Exists(fileStr))
                {
                    File.Create(fileStr).Close();
                    if (!FileIsUsed(fileStr))
                    {
                        string templateStr = BaseFolder + "\\Template.txt";
                        if (File.Exists(templateStr))
                        {
                            TemplateContent = File.ReadAllText(templateStr,Encoding.UTF8);
                        }
                        File.WriteAllText(fileStr, TemplateContent + BaseIndexContent, Encoding.UTF8);
                    }
                    //FileStream fs = new FileStream(fileStr, FileMode.Append, FileAccess.Write);
                    //try
                    //{
                    //    using (StreamWriter m_streamWriter = new StreamWriter(fs))
                    //    {
                    //        m_streamWriter.Flush();
                    //        m_streamWriter.WriteLine(BaseIndexContent);
                    //        m_streamWriter.Flush();
                    //        m_streamWriter.Dispose();
                    //        m_streamWriter.Close();
                    //    }
                    //}
                    //catch (Exception ee)
                    //{
                    //    Console.WriteLine(ee.Message);
                    //}
                    //finally
                    //{
                    //    fs.Dispose();
                    //    fs.Close();
                    //}
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            finally { }
        }

        /// <summary>
        /// 获得指定路径下所有子目录名
        /// </summary>
        /// <param name="sw">文件写入流</param>
        /// <param name="path">文件夹路径</param>
        /// <param name="indent">输出时的缩进量</param>
        public static List<string> getDirectory(string path)
        {
            List<string> result = new List<string>();
            DirectoryInfo root = new DirectoryInfo(path);
            foreach (DirectoryInfo d in root.GetDirectories())
            {
                result.Add(d.FullName);
                result.AddRange(getDirectory(d.FullName));
            }
            return result;
        }

        private void CreateFolder(string path)
        {
            try
            {
                // Determine whether the directory exists.
                if (!Directory.Exists(path))
                {
                    // Create the directory it does not exist.
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            finally { }
        }

        /// <summary>
        /// 返回指示文件是否已被其它程序使用的布尔值
        /// </summary>
        /// <param name="fileFullName">文件的完全限定名，例如：“C:\MyFile.txt”。</param>
        /// <returns>如果文件已被其它程序使用，则为 true；否则为 false。</returns>
        public static Boolean FileIsUsed(String fileFullName)
        {
            Boolean result = false;
            //判断文件是否存在，如果不存在，直接返回 false
            if (!System.IO.File.Exists(fileFullName))
            {
                result = false;
            }//end: 如果文件不存在的处理逻辑
            else
            {//如果文件存在，则继续判断文件是否已被其它程序使用
             //逻辑：尝试执行打开文件的操作，如果文件已经被其它程序使用，则打开失败，抛出异常，根据此类异常可以判断文件是否已被其它程序使用。
                System.IO.FileStream fileStream = null;
                try
                {
                    fileStream = System.IO.File.Open(fileFullName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None);
                    result = false;
                }
                catch (System.IO.IOException ioEx)
                {
                    result = true;
                }
                catch (System.Exception ex)
                {
                    result = true;
                }
                finally
                {
                    if (fileStream != null)
                    {
                        fileStream.Close();
                    }
                }
            }//end: 如果文件存在的处理逻辑
             //返回指示文件是否已被其它程序使用的值
            return result;
        }//end method FileIsUsed
    }
}