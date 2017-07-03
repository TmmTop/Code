using System.Data;
using System.Windows.Forms;
using BLL;
using Model;

namespace WFA
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            DataSet ds = GetData();
        }

        private DataSet GetData()
        {
            string errMsg;
            var obj = new ModelWFA
            {
                SPReturnType = AppEnum.SPReturnTypes.DataSet
            };
            DataSet ds = BllWFA.SelectAll(obj, out errMsg);
            return ds; 
        }
    }
}
