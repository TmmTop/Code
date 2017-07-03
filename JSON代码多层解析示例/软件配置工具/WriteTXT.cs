using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 软件配置工具
{
   public class WriteTXT
    {
        public void Write(string txtName, string txt)
        {
            string path = Application.StartupPath+ "\\配置文件备份\\";
            if (!Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
                DirectoryInfo dir = new DirectoryInfo(path);
                dir.Create();//自行判断一下是否存在。
            }
            FileStream fs = new FileStream(Application.StartupPath+ "\\配置文件备份\\" + txtName + ".json", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default); ;
            sw.Write(txt.Replace("'", "\""));//将字符串的单引号替换为双引号
            sw.Close();
            fs.Close();
        }
        public void Clear(string txtName)
        {
            string path = Application.StartupPath+ "\\配置文件备份\\" + txtName + ".json";
            if (File.Exists(path))
            {
                //如果存在则删除
                File.Delete(path);
            }
        }
        public void WriteConfig(string txtName, string txt)
        {
            string path = Application.StartupPath + "\\";
            if (!Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
                DirectoryInfo dir = new DirectoryInfo(path);
                dir.Create();//自行判断一下是否存在。
            }
            FileStream fs = new FileStream(Application.StartupPath + "\\" + txtName + ".json", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default); ;
            sw.Write(txt.Replace("'", "\""));//将字符串的单引号替换为双引号
            sw.Close();
            fs.Close();
        }
        public void ClearConfig(string txtName)
        {
            string path = Application.StartupPath + "\\" + txtName + ".json";
            if (File.Exists(path))
            {
                //如果存在则删除
                File.Delete(path);
            }
        }
    }
}
