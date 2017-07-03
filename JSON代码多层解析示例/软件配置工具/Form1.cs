using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Collections;

namespace 软件配置工具
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        ReadeJSON json = new ReadeJSON();
        private void Form1_Load(object sender, EventArgs e)
        {
            BindCG();
            BindJJ();
            BindGX();
        }

        private void btnSave_Click(object sender, EventArgs e)//保存用户修改后的配置
        {

            SaveConfig();
        }

        private void btnInit_Click(object sender, EventArgs e)//初始化配置 将配置文件写出至   配置文件/config.json
        {
            try
            {
                WriteTXT txt = new WriteTXT();
                string txtStr = "{'橱柜信息':[{'板材材质':[{'id':'0','value':'板材材质1'},{'id':'1','value':'板材材质2'},{'id':'2','value':'板材材质3'},{'id':'3','value':'板材材质4'}]},{'台面色号':[{'id':'0','value':'台面色号0'},{'id':'1','value':'台面色号1'},{'id':'2','value':'台面色号2'},{'id':'3','value':'台面色号3'}]},{'炉台拉篮':[{'id':'0','value':'炉台拉篮0'},{'id':'1','value':'炉台拉篮1'},{'id':'2','value':'炉台拉篮2'},{'id':'3','value':'炉台拉篮3'}]},{'水槽型号':[{'id':'0','value':'水槽型号0'},{'id':'1','value':'水槽型号1'},{'id':'2','value':'水槽型号2'},{'id':'3','value':'水槽型号3'}]},{'台面边型':[{'id':'0','value':'台面边型0'},{'id':'1','value':'台面边型1'},{'id':'2','value':'台面边型2'},{'id':'3','value':'台面边型3'}]},{'常用五金':[{'id':'0','value':'常用五金0'},{'id':'1','value':'常用五金1'},{'id':'2','value':'常用五金2'},{'id':'3','value':'常用五金3'}]},{'调味拉篮':[{'id':'0','value':'调味拉篮0'},{'id':'1','value':'调味拉篮1'},{'id':'2','value':'调味拉篮2'},{'id':'3','value':'调味拉篮3'}]},{'板材色号':[{'id':'0','value':'板材色号'},{'id':'1','value':'板材色号1'},{'id':'2','value':'板材色号2'},{'id':'3','value':'板材色号3'}]}],'家具信息':[{'家具名称':[{'id':'0','value':'家具名称0'},{'id':'1','value':'家具名称1'},{'id':'2','value':'家具名称2'},{'id':'3','value':'家具名称3'}]},{'板材色号':[{'id':'0','value':'板材色号0'},{'id':'1','value':'板材色号1'},{'id':'2','value':'板材色号2'},{'id':'3','value':'板材色号3'}]},{'门板下单':[{'id':'0','value':'门板下单0'},{'id':'1','value':'门板下单1'},{'id':'2','value':'门板下单2'},{'id':'3','value':'门板下单3'}]},{'板材材质':[{'id':'0','value':'板材材质0'},{'id':'1','value':'板材材质1'},{'id':'2','value':'板材材质2'},{'id':'3','value':'板材材质3'}]},{'常用五金':[{'id':'0','value':'常用五金0'},{'id':'1','value':'常用五金1'},{'id':'2','value':'常用五金2'},{'id':'3','value':'常用五金3'}]},{'移门信息':[{'id':'0','value':'移门信息0'},{'id':'1','value':'移门信息1'},{'id':'2','value':'移门信息2'},{'id':'3','value':'移门信息3'}]},{'家具设计':[{'id':'0','value':'中式风格'},{'id':'1','value':'欧美风格'},{'id':'2','value':'日韩风格'},{'id':'3','value':'自定义风格'}]}],'工序信息':[{'工序配置':[{'id':'0','value':'拆单'},{'id':'1','value':'开料'},{'id':'2','value':'封边'},{'id':'3','value':'排孔'},{'id':'4','value':'打包'},{'id':'5','value':'入库'},{'id':'6','value':'发货'}]}]}";
                txt.Clear("config");//先删除在创建
                txt.Write("config", txtStr);
                MessageBox.Show("配置文件恢复成功，请将恢复的配置文件拷入软件根目录！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void SaveConfig()
        {
            try
            {
                var bcczlist = this.dgvCG.Rows.OfType<DataGridViewRow>().Select(x => x.Cells[0].Value).Where(x => x != null && !string.IsNullOrEmpty(x.ToString()) && !string.IsNullOrWhiteSpace(x.ToString())).ToList();
                var tmshlist = this.dgvCG.Rows.OfType<DataGridViewRow>().Select(x => x.Cells[1].Value).Where(x => x != null && !string.IsNullOrEmpty(x.ToString()) && !string.IsNullOrWhiteSpace(x.ToString())).ToList();
                var ltlllist = this.dgvCG.Rows.OfType<DataGridViewRow>().Select(x => x.Cells[2].Value).Where(x => x != null && !string.IsNullOrEmpty(x.ToString()) && !string.IsNullOrWhiteSpace(x.ToString())).ToList();
                var scxhlist = this.dgvCG.Rows.OfType<DataGridViewRow>().Select(x => x.Cells[3].Value).Where(x => x != null && !string.IsNullOrEmpty(x.ToString()) && !string.IsNullOrWhiteSpace(x.ToString())).ToList();
                var tmbxlist = this.dgvCG.Rows.OfType<DataGridViewRow>().Select(x => x.Cells[4].Value).Where(x => x != null && !string.IsNullOrEmpty(x.ToString()) && !string.IsNullOrWhiteSpace(x.ToString())).ToList();
                var cywjlist = this.dgvCG.Rows.OfType<DataGridViewRow>().Select(x => x.Cells[5].Value).Where(x => x != null && !string.IsNullOrEmpty(x.ToString()) && !string.IsNullOrWhiteSpace(x.ToString())).ToList();
                var twlllist = this.dgvCG.Rows.OfType<DataGridViewRow>().Select(x => x.Cells[6].Value).Where(x => x != null && !string.IsNullOrEmpty(x.ToString()) && !string.IsNullOrWhiteSpace(x.ToString())).ToList();
                var bcshlist = this.dgvCG.Rows.OfType<DataGridViewRow>().Select(x => x.Cells[7].Value).Where(x => x != null && !string.IsNullOrEmpty(x.ToString()) && !string.IsNullOrWhiteSpace(x.ToString())).ToList();
                string bccz = "";
                string tmsh = "";
                string ltll = "";
                string scxh = "";
                string tmbx = "";
                string cywj = "";
                string twll = "";
                string bcsh = "";
                for (int i = 0; i < bcczlist.Count; i++)
                {
                    bccz += "{'id':" + "'" + i + "'," + "'value':'" + bcczlist[i].ToString() + "'},";
                }
                for (int i = 0; i < tmshlist.Count; i++)
                {
                    tmsh += "{'id':" + "'" + i + "'," + "'value':'" + tmshlist[i].ToString() + "'},";
                }
                for (int i = 0; i < ltlllist.Count; i++)
                {
                    ltll += "{'id':" + "'" + i + "'," + "'value':'" + ltlllist[i].ToString() + "'},";
                }
                for (int i = 0; i < scxhlist.Count; i++)
                {
                    scxh += "{'id':" + "'" + i + "'," + "'value':'" + scxhlist[i].ToString() + "'},";
                }
                for (int i = 0; i < tmbxlist.Count; i++)
                {
                    tmbx += "{'id':" + "'" + i + "'," + "'value':'" + tmbxlist[i].ToString() + "'},";
                }
                for (int i = 0; i < cywjlist.Count; i++)
                {
                    cywj += "{'id':" + "'" + i + "'," + "'value':'" + cywjlist[i].ToString() + "'},";
                }
                for (int i = 0; i < twlllist.Count; i++)
                {
                    twll += "{'id':" + "'" + i + "'," + "'value':'" + twlllist[i].ToString() + "'},";
                }
                for (int i = 0; i < bcshlist.Count; i++)
                {
                    bcsh += "{'id':" + "'" + i + "'," + "'value':'" + bcshlist[i].ToString() + "'},";
                }
                string cginfo =
                    "{'橱柜信息': [{'板材材质': [" + bccz + "]}," +
                    "{'台面色号': [" + tmsh + "]}," +
                    "{'炉台拉篮': [" + ltll + "]}," +
                    "{'水槽型号': [" + scxh + "]}," +
                    "{'台面边型': [" + tmbx + "]}," +
                    "{'常用五金': [" + cywj + "]}," +
                    "{'调味拉篮': [" + twll + "]}," +
                    "{'板材色号': [" + bcsh + "]}],";


                var jjmclist = this.dgvJJ.Rows.OfType<DataGridViewRow>().Select(x => x.Cells[0].Value).Where(x => x != null && !string.IsNullOrEmpty(x.ToString()) && !string.IsNullOrWhiteSpace(x.ToString())).ToList();
                var bcshslist = this.dgvJJ.Rows.OfType<DataGridViewRow>().Select(x => x.Cells[1].Value).Where(x => x != null && !string.IsNullOrEmpty(x.ToString()) && !string.IsNullOrWhiteSpace(x.ToString())).ToList();
                var mbxdlist = this.dgvJJ.Rows.OfType<DataGridViewRow>().Select(x => x.Cells[2].Value).Where(x => x != null && !string.IsNullOrEmpty(x.ToString()) && !string.IsNullOrWhiteSpace(x.ToString())).ToList();
                var bcczslist = this.dgvJJ.Rows.OfType<DataGridViewRow>().Select(x => x.Cells[3].Value).Where(x => x != null && !string.IsNullOrEmpty(x.ToString()) && !string.IsNullOrWhiteSpace(x.ToString())).ToList();
                var cywjslist = this.dgvJJ.Rows.OfType<DataGridViewRow>().Select(x => x.Cells[4].Value).Where(x => x != null && !string.IsNullOrEmpty(x.ToString()) && !string.IsNullOrWhiteSpace(x.ToString())).ToList();
                var ymxxlist = this.dgvJJ.Rows.OfType<DataGridViewRow>().Select(x => x.Cells[5].Value).Where(x => x != null && !string.IsNullOrEmpty(x.ToString()) && !string.IsNullOrWhiteSpace(x.ToString())).ToList();
                var jjsjlist = this.dgvJJ.Rows.OfType<DataGridViewRow>().Select(x => x.Cells[6].Value).Where(x => x != null && !string.IsNullOrEmpty(x.ToString()) && !string.IsNullOrWhiteSpace(x.ToString())).ToList();
                string jjmc = "";
                string bcshs = "";
                string mbxd = "";
                string bcczs = "";
                string cywjs = "";
                string ymxx = "";
                string jjsj = "";
                for (int i = 0; i < jjmclist.Count; i++)
                {
                    jjmc += "{'id':" + "'" + i + "'," + "'value':'" + jjmclist[i].ToString() + "'},";
                }
                for (int i = 0; i < bcshslist.Count; i++)
                {
                    bcshs += "{'id':" + "'" + i + "'," + "'value':'" + bcshslist[i].ToString() + "'},";
                }
                for (int i = 0; i < mbxdlist.Count; i++)
                {
                    mbxd += "{'id':" + "'" + i + "'," + "'value':'" + mbxdlist[i].ToString() + "'},";
                }
                for (int i = 0; i < bcczslist.Count; i++)
                {
                    bcczs += "{'id':" + "'" + i + "'," + "'value':'" + bcczslist[i].ToString() + "'},";
                }
                for (int i = 0; i < cywjslist.Count; i++)
                {
                    cywjs += "{'id':" + "'" + i + "'," + "'value':'" + cywjslist[i].ToString() + "'},";
                }
                for (int i = 0; i < ymxxlist.Count; i++)
                {
                    ymxx += "{'id':" + "'" + i + "'," + "'value':'" + ymxxlist[i].ToString() + "'},";
                }
                for (int i = 0; i < jjsjlist.Count; i++)
                {
                    jjsj += "{'id':" + "'" + i + "'," + "'value':'" + jjsjlist[i].ToString() + "'},";
                }
                string jjinfo =
                    "'家具信息': [{'家具名称': [" + jjmc + "]}," +
                    "{'板材色号': [" + bcshs + "]}," +
                    "{'门板下单': [" + mbxd + "]}," +
                    "{'板材材质': [" + bcczs + "]}," +
                    "{'常用五金': [" + cywjs + "]}," +
                    "{'移门信息': [" + ymxx + "]}," +
                    "{'家具设计': [" + jjsj + "]}],";

                var gxpzlist = this.dgvGX.Rows.OfType<DataGridViewRow>().Select(x => x.Cells[0].Value).Where(x => x != null && !string.IsNullOrEmpty(x.ToString()) && !string.IsNullOrWhiteSpace(x.ToString())).ToList();
                string gxpz = "";
                for (int i = 0; i < gxpzlist.Count; i++)
                {
                    gxpz += "{'id':" + "'" + i + "'," + "'value':'" + gxpzlist[i].ToString() + "'},";
                }
                string gxinfo =
                     "'工序信息': [{'工序配置': [" + gxpz + "]}]}";
                WriteTXT txt = new WriteTXT();
                string allinfo = cginfo + jjinfo + gxinfo;
                txt.ClearConfig("config");
                txt.WriteConfig("config", allinfo.Replace(",]", "]"));
                MessageBox.Show("配置文件保存成功，请重新打开家具管理系统！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void BindCG()
        {
            //=======================================绑定橱柜信息=============================================================
            DataTable dt = new DataTable();
            dt.Columns.Add("板材材质", typeof(string));
            dt.Columns.Add("台面色号", typeof(string));
            dt.Columns.Add("炉台拉篮", typeof(string));
            dt.Columns.Add("水槽型号", typeof(string));
            dt.Columns.Add("台面边型", typeof(string));
            dt.Columns.Add("常用五金", typeof(string));
            dt.Columns.Add("调味拉篮", typeof(string));
            dt.Columns.Add("板材色号", typeof(string));
            var re = json.getJsonNum("橱柜信息");
            int nums = json.SubstringCount(re[0], "id");//获取json中id出现的次数
            Dictionary<int, string> kvDictonary = new Dictionary<int, string>();//板材材质
            Dictionary<int, string> kvDictonary1 = new Dictionary<int, string>();//台面色号
            Dictionary<int, string> kvDictonary2 = new Dictionary<int, string>();//炉台拉篮
            Dictionary<int, string> kvDictonary3 = new Dictionary<int, string>();//水槽型号
            Dictionary<int, string> kvDictonary4 = new Dictionary<int, string>();//台面边型
            Dictionary<int, string> kvDictonary5 = new Dictionary<int, string>();//常用五金
            Dictionary<int, string> kvDictonary6 = new Dictionary<int, string>();//调味拉篮
            Dictionary<int, string> kvDictonary7 = new Dictionary<int, string>();//板材色号
            for (int i = 0; i < nums; i++)
            {
                kvDictonary.Add(i, json.GetConfig()["橱柜信息"][0]["板材材质"][i]["value"].ToString());
                kvDictonary1.Add(i, json.GetConfig()["橱柜信息"][1]["台面色号"][i]["value"].ToString());
                kvDictonary2.Add(i, json.GetConfig()["橱柜信息"][2]["炉台拉篮"][i]["value"].ToString());
                kvDictonary3.Add(i, json.GetConfig()["橱柜信息"][3]["水槽型号"][i]["value"].ToString());
                kvDictonary4.Add(i, json.GetConfig()["橱柜信息"][4]["台面边型"][i]["value"].ToString());
                kvDictonary5.Add(i, json.GetConfig()["橱柜信息"][5]["常用五金"][i]["value"].ToString());
                kvDictonary6.Add(i, json.GetConfig()["橱柜信息"][6]["调味拉篮"][i]["value"].ToString());
                kvDictonary7.Add(i, json.GetConfig()["橱柜信息"][7]["板材色号"][i]["value"].ToString());
            }
            for (int i = 0; i < nums; i++)
            {
                DataRow dr = dt.NewRow();
                dr["板材材质"] = kvDictonary[i];
                dr["台面色号"] = kvDictonary1[i];
                dr["炉台拉篮"] = kvDictonary2[i];
                dr["水槽型号"] = kvDictonary3[i];
                dr["台面边型"] = kvDictonary4[i];
                dr["常用五金"] = kvDictonary5[i];
                dr["调味拉篮"] = kvDictonary6[i];
                dr["板材色号"] = kvDictonary7[i];
                dt.Rows.Add(dr);
            }
            dgvCG.DataSource = dt;
            //=======================================绑定橱柜信息=============================================================
        }
        public void BindJJ()
        {
            //=======================================绑定家具信息=============================================================
            DataTable dt = new DataTable();
            dt.Columns.Add("家具名称", typeof(string));
            dt.Columns.Add("板材色号", typeof(string));
            dt.Columns.Add("门板下单", typeof(string));
            dt.Columns.Add("板材材质", typeof(string));
            dt.Columns.Add("常用五金", typeof(string));
            dt.Columns.Add("移门信息", typeof(string));
            dt.Columns.Add("家具设计", typeof(string));
            var re = json.getJsonNum("家具信息");
            int nums = json.SubstringCount(re[0], "id");//获取json中id出现的次数
            Dictionary<int, string> kvDictonary = new Dictionary<int, string>();//板材材质
            Dictionary<int, string> kvDictonary1 = new Dictionary<int, string>();//台面色号
            Dictionary<int, string> kvDictonary2 = new Dictionary<int, string>();//炉台拉篮
            Dictionary<int, string> kvDictonary3 = new Dictionary<int, string>();//水槽型号
            Dictionary<int, string> kvDictonary4 = new Dictionary<int, string>();//台面边型
            Dictionary<int, string> kvDictonary5 = new Dictionary<int, string>();//常用五金
            Dictionary<int, string> kvDictonary6 = new Dictionary<int, string>();//调味拉篮
            Dictionary<int, string> kvDictonary7 = new Dictionary<int, string>();//板材色号
            for (int i = 0; i < nums; i++)
            {
                kvDictonary.Add(i, json.GetConfig()["家具信息"][0]["家具名称"][i]["value"].ToString());
                kvDictonary1.Add(i, json.GetConfig()["家具信息"][1]["板材色号"][i]["value"].ToString());
                kvDictonary2.Add(i, json.GetConfig()["家具信息"][2]["门板下单"][i]["value"].ToString());
                kvDictonary3.Add(i, json.GetConfig()["家具信息"][3]["板材材质"][i]["value"].ToString());
                kvDictonary4.Add(i, json.GetConfig()["家具信息"][4]["常用五金"][i]["value"].ToString());
                kvDictonary5.Add(i, json.GetConfig()["家具信息"][5]["移门信息"][i]["value"].ToString());
                kvDictonary6.Add(i, json.GetConfig()["家具信息"][6]["家具设计"][i]["value"].ToString());
            }
            for (int i = 0; i < nums; i++)
            {
                DataRow dr = dt.NewRow();
                dr["家具名称"] = kvDictonary[i];
                dr["板材色号"] = kvDictonary1[i];
                dr["门板下单"] = kvDictonary2[i];
                dr["板材材质"] = kvDictonary3[i];
                dr["常用五金"] = kvDictonary4[i];
                dr["移门信息"] = kvDictonary5[i];
                dr["家具设计"] = kvDictonary6[i];
                dt.Rows.Add(dr);
            }
            dgvJJ.DataSource = dt;
            //=======================================绑定橱柜信息=============================================================

        }
        public void BindGX()
        {
            //=======================================绑定橱柜信息=============================================================
            DataTable dt = new DataTable();
            dt.Columns.Add("工序配置", typeof(string));
            var re = json.getJsonNumOnes("工序信息");
            int nums = json.SubstringCount(re[0], "id");//获取json中id出现的次数
            Dictionary<int, string> kvDictonary = new Dictionary<int, string>();//工序配置
            for (int i = 0; i < nums; i++)
            {
                kvDictonary.Add(i, json.GetConfig()["工序信息"][0]["工序配置"][i]["value"].ToString());
            }
            for (int i = 0; i < nums; i++)
            {
                DataRow dr = dt.NewRow();
                dr["工序配置"] = kvDictonary[i];
                dt.Rows.Add(dr);
            }
            dgvGX.DataSource = dt;
            //=======================================绑定橱柜信息=============================================================
        }
    }
}
