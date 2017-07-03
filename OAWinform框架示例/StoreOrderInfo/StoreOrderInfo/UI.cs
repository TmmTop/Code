using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetDimension.NanUI;

namespace StoreOrderInfo
{
    public partial class UI : HtmlUIForm
    {
        public UI() : base("embedded://Code/index.html")
        {
            InitializeComponent();
           
        }
    }
}
// GlobalObject.Add("hostEditor", new JsCodeEditorObject(this));
//private void JsCodeEditorObject_ExecuteSetClean(object sender, Chromium.Remote.Event.CfrV8HandlerExecuteEventArgs e)
//{
//    if (e.Arguments.Length > 0)
//    {
//        parentForm.isClean = e.Arguments.First(p => p.IsBool).BoolValue;
//    }
//}
