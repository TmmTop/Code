        //===================================================================================================数据层写法
        public static int InsertConsignment(Consignment r)//成品发货单事务多条插入
        {
            using (var conn = DapperConnFactory.Instance.GetOpenConn())
            {
                conn.Open();
                //开户事务
                var trans = conn.BeginTransaction();
                var rows = conn.ExecuteScalar<int>(@"INSERT INTO [Consignment]([conid],[sumprice],[company],[carnumber],[remark],[scdate]) 
                                                     VALUES(@conid,@sumprice,@company,@carnumber,@remark,@scdate) SELECT SCOPE_IDENTITY() ", r, trans);
                if (rows > 0)
                {
                    Array.ForEach(r.ConsignmentList.ToArray(), x => x.conid = rows.ToString());
                    rows = conn.Execute(@"INSERT INTO [ConsignmentInfo]([cpid],[conid],[name],[batch],[num],[price],[sumprice],[norms]) 
                                          VALUES(@cpid,@conid,@name, @batch, @num, @price, @sumprice, @norms)", r.ConsignmentList, trans);
                    if (rows > 0)
                    {
                        trans.Commit();
                    }
                    else
                    {
                        trans.Rollback();
                    }
                }
                else
                {
                    trans.Rollback();
                }
                conn.Close();
                return rows;
            }
        }
        //===================================================================================================UI层数据绑定
        private void Frm成品送货单_Load(object sender, EventArgs e)
        {
            this.dgvData = new BindingList<ConsignmentInfo>(new List<ConsignmentInfo>());
            Dgv.DataSource = dgvData;
        }
        private BindingList<ConsignmentInfo> dgvData;
        private void btnAdd_Click(object sender, EventArgs e)//添加子表行
        {
            if (txtPName.Text == "" && txtPNo.Text == "")
            {
                MessageBox.Show("请补全信息！");
                return;
            }
            ConsignmentInfo obj = new ConsignmentInfo();
            obj.cpid = "Cpid" + TimeStamp.GetTimeStamp();//字表id
            obj.name = txtPName.Text;
            obj.batch = txtPNo.Text;
            obj.num = int.Parse(txtPCount.Value.ToString());
            obj.price = txtPPrice.Value;
            obj.sumprice = int.Parse(txtPCount.Value.ToString()) * txtPPrice.Value;
            obj.norms = txtPGuige.Text;
            dgvData.Add(obj);
        }
        private void btnRemove_Click(object sender, EventArgs e)//删除子表行
        {
            if (Dgv.Rows.Count > 0)
            {
                Dgv.Rows.RemoveAt(Dgv.CurrentRow.Index);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("成品送货信息为空！");
            }
        }
        private void Save_Click(object sender, EventArgs e)//提交保存主从表数据
        {
            if (!string.IsNullOrEmpty(txtPAccept.Text) && Dgv.Rows.Count > 0 && !string.IsNullOrEmpty(txtPSum.Text))
            {
                if (model != null)
                {
                    model.conid = "Conid" + TimeStamp.GetTimeStamp();//主表id
                    model.sumprice = decimal.Parse(txtPSum.Text.ToString());
                    model.company = txtPAccept.Text;
                    model.carnumber = txtPCard.Text;
                    model.remark = txtPNote.Text;
                    model.scdate = dateTimePicker1.Text;
                }
                model.ConsignmentList = this.dgvData;
                if (dal.Insert(model) > 0)
                {
                    MessageBox.Show("添加成功！");
                }
                else
                {
                    MessageBox.Show("添加失败！");
                };
            }
            else {
                MessageBox.Show("请补全数据！");
            }
        }
        //===================================================================================================Model
        public class Consignment //=================主表Model
        {

            public int id {get; set;} 
            public string conid {get; set;}     
            public decimal sumprice {get; set;}
            public string company {get; set;}
            public string carnumber {get; set;}
            public string remark {get; set;} 
            public string scdate {get; set;}
            public string createtime {get; set;}
            /// <summary>
            /// 成品明细表一对多
            /// </summary>
            public IList<ConsignmentInfo> ConsignmentList { get; set; }
        }            
        public class ConsignmentInfo
        {
            public int id { get; set; }
            public string cpid { get; set; }
            public string conid { get; set; }//此字段对应主表的id
            public string name { get; set; }
            public string batch { get; set; }
            public int num { get; set; }
            public decimal price { get; set; }
            public decimal sumprice { get; set; }
            public string norms { get; set; }
        }
        //===================================================================================================数据库语句




