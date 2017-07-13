using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using DoddleReport;
using DoddleReport.Writers;

namespace LwmReportView
{
    public partial class View: UserControl
    {
        public View()
        {
            InitializeComponent();
        }
        public void View_Load(object sender, EventArgs e)
        {
            this.CupBox.IsWebBrowserContextMenuEnabled = true; //禁用右键菜单           
            this.CupBox.WebBrowserShortcutsEnabled = true;//禁用键盘快捷键
            this.CupBox.AllowWebBrowserDrop = false;//以防止 WebBrowser 控件打开拖放到其上的文件。
            this.CupBox.IsWebBrowserContextMenuEnabled = false;//是否启用快捷键
            this.CupBox.WebBrowserShortcutsEnabled = false;//以防止 WebBrowser 控件响应快捷键。
            this.CupBox.ScriptErrorsSuppressed = false;//以防止 WebBrowser 控件显示脚本代码问题的错误信
            this.CupBox.ScrollBarsEnabled = false;//去掉WebBrowser本身自带的滚动条
        }
        public void LoadData<T>(string title, string subtitle, string footer,string header,IList<T> list) {    
            //=========================================================================================绑定测试数据
            FileStream fs = null;
            var query = list;
            // 创建报告和把我们的查询到一个报表资源
            var data = new Report(query.ToReportSource(), new DoddleReport.OpenXml.ExcelReportWriter());
            // 自定义文本字段
            data.TextFields.Title = title;
            data.TextFields.SubTitle = subtitle;
            data.TextFields.Footer = footer;
            data.TextFields.Header = header;
            data.RenderHints.BooleanCheckboxes = true;
            var writer = new HtmlReportWriter();
            string path = @"ReportView.html";
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                using (fs = File.Create(path))
                {
                    writer.WriteReport(data, fs);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            CupBox.Navigate(Application.StartupPath + @"\ReportView.html");
            //=========================================================================================绑定测试数据
        }
        public void 打印预览_Click(object sender, EventArgs e)
        {
            string keyName = @"Software\Microsoft\Internet Explorer\PageSetup";
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName, true))
            {
                if (key != null)
                {
                    key.SetValue("footer", "");
                    key.SetValue("header", "");
                }
            }
            CupBox.ShowPrintPreviewDialog();
        }

        public void 直接打印_Click(object sender, EventArgs e)
        {
            string keyName = @"Software\Microsoft\Internet Explorer\PageSetup";
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName, true))
            {
                if (key != null)
                {
                    key.SetValue("footer", "");
                    key.SetValue("header", "");
                }
            }
            CupBox.ShowPrintDialog();
        }

        public void CupBox_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (this.CupBox.ReadyState == WebBrowserReadyState.Complete)
            {
                HtmlElementCollection bodys = this.CupBox.Document.GetElementsByTagName("body");
                HtmlElement body = null;
                if (bodys.Count > 0)
                    body = bodys[0];
                if (body != null)
                {
                    if (body.Style != null)
                        body.Style += " overflow: hidden; overflow-y: auto";
                    else
                        body.Style = " overflow: hidden;overflow-y: auto ";
                }
            }
        }
    }
}
