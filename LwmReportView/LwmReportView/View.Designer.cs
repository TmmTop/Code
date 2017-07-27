namespace LwmReportView
{
    partial class View
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.CupBox = new System.Windows.Forms.WebBrowser();
            this.panel1 = new System.Windows.Forms.Panel();
            this.菜单 = new System.Windows.Forms.ToolStrip();
            this.打印预览 = new System.Windows.Forms.ToolStripButton();
            this.直接打印 = new System.Windows.Forms.ToolStripButton();
            this.导出EXCEL = new System.Windows.Forms.ToolStripButton();
            this.导出PDF = new System.Windows.Forms.ToolStripButton();
            this.导出Word = new System.Windows.Forms.ToolStripButton();
            this.导出JSON = new System.Windows.Forms.ToolStripButton();
            this.使用帮助 = new System.Windows.Forms.ToolStripButton();
            this.关于 = new System.Windows.Forms.ToolStripButton();
            this.panel1.SuspendLayout();
            this.菜单.SuspendLayout();
            this.SuspendLayout();
            // 
            // CupBox
            // 
            this.CupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CupBox.Location = new System.Drawing.Point(0, 35);
            this.CupBox.MinimumSize = new System.Drawing.Size(20, 20);
            this.CupBox.Name = "CupBox";
            this.CupBox.ScrollBarsEnabled = false;
            this.CupBox.Size = new System.Drawing.Size(817, 547);
            this.CupBox.TabIndex = 4;
            this.CupBox.Url = new System.Uri("", System.UriKind.Relative);
            this.CupBox.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.CupBox_DocumentCompleted);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.菜单);
            this.panel1.Location = new System.Drawing.Point(0, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 30);
            this.panel1.TabIndex = 5;
            // 
            // 菜单
            // 
            this.菜单.BackColor = System.Drawing.Color.Transparent;
            this.菜单.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打印预览,
            this.直接打印,
            this.导出EXCEL,
            this.导出PDF,
            this.导出Word,
            this.导出JSON,
            this.使用帮助,
            this.关于});
            this.菜单.Location = new System.Drawing.Point(0, 0);
            this.菜单.Name = "菜单";
            this.菜单.Size = new System.Drawing.Size(800, 29);
            this.菜单.TabIndex = 0;
            this.菜单.Text = "toolStrip1";
            // 
            // 打印预览
            // 
            this.打印预览.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.打印预览.Image = global::LwmReportView.Properties.Resources.cd;
            this.打印预览.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.打印预览.Name = "打印预览";
            this.打印预览.Size = new System.Drawing.Size(94, 26);
            this.打印预览.Text = "打印预览";
            this.打印预览.Click += new System.EventHandler(this.打印预览_Click);
            // 
            // 直接打印
            // 
            this.直接打印.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.直接打印.Image = global::LwmReportView.Properties.Resources.vv_xpincon_26;
            this.直接打印.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.直接打印.Name = "直接打印";
            this.直接打印.Size = new System.Drawing.Size(94, 26);
            this.直接打印.Text = "直接打印";
            this.直接打印.Click += new System.EventHandler(this.直接打印_Click);
            // 
            // 导出EXCEL
            // 
            this.导出EXCEL.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.导出EXCEL.Image = global::LwmReportView.Properties.Resources.vv_xpincon_32;
            this.导出EXCEL.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.导出EXCEL.Name = "导出EXCEL";
            this.导出EXCEL.Size = new System.Drawing.Size(111, 26);
            this.导出EXCEL.Text = "导出EXCEL";
            // 
            // 导出PDF
            // 
            this.导出PDF.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.导出PDF.Image = global::LwmReportView.Properties.Resources.save;
            this.导出PDF.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.导出PDF.Name = "导出PDF";
            this.导出PDF.Size = new System.Drawing.Size(95, 26);
            this.导出PDF.Text = "导出PDF";
            // 
            // 导出Word
            // 
            this.导出Word.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.导出Word.Image = global::LwmReportView.Properties.Resources.add_home;
            this.导出Word.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.导出Word.Name = "导出Word";
            this.导出Word.Size = new System.Drawing.Size(108, 26);
            this.导出Word.Text = "导出Word";
            // 
            // 导出JSON
            // 
            this.导出JSON.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.导出JSON.Image = global::LwmReportView.Properties.Resources.add_to_database;
            this.导出JSON.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.导出JSON.Name = "导出JSON";
            this.导出JSON.Size = new System.Drawing.Size(107, 26);
            this.导出JSON.Text = "导出JSON";
            // 
            // 使用帮助
            // 
            this.使用帮助.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.使用帮助.Image = global::LwmReportView.Properties.Resources.vv_xpincon_30;
            this.使用帮助.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.使用帮助.Name = "使用帮助";
            this.使用帮助.Size = new System.Drawing.Size(94, 26);
            this.使用帮助.Text = "使用帮助";
            // 
            // 关于
            // 
            this.关于.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.关于.Image = global::LwmReportView.Properties.Resources.vv_xpincon_19;
            this.关于.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.关于.Name = "关于";
            this.关于.Size = new System.Drawing.Size(62, 26);
            this.关于.Text = "关于";
            // 
            // View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.CupBox);
            this.Name = "View";
            this.Size = new System.Drawing.Size(800, 582);
            this.Load += new System.EventHandler(this.View_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.菜单.ResumeLayout(false);
            this.菜单.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.WebBrowser CupBox;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.ToolStrip 菜单;
        public System.Windows.Forms.ToolStripButton 打印预览;
        public System.Windows.Forms.ToolStripButton 直接打印;
        public System.Windows.Forms.ToolStripButton 导出EXCEL;
        public System.Windows.Forms.ToolStripButton 导出PDF;
        public System.Windows.Forms.ToolStripButton 导出Word;
        public System.Windows.Forms.ToolStripButton 导出JSON;
        public System.Windows.Forms.ToolStripButton 使用帮助;
        public System.Windows.Forms.ToolStripButton 关于;
    }
}
