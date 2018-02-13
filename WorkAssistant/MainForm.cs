using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorkAssistant
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            XmlConfig.Instance.LoadAppConfig();
            XmlConfig.Instance.LoadWorkConfig();
            InitProjectList();
        }

        private void InitProjectList()
        {
            if (XmlConfig.Instance.projects.Count <= 0)
                return;

            this.projectListView.View = View.Details;
            this.projectListView.GridLines = true;
            this.projectListView.FullRowSelect = true;
            this.projectListView.Scrollable = true;
            this.projectListView.ShowGroups = true;

            //创建列标题
            string[] columnTitles = new string[] { "名称", "源码工程(PC)", "源码工程(Android)", "Build工程(PC)", "Build工程(Android)", "svn_url" };
            for (int i = 0; i < columnTitles.Length; i++){
                ColumnHeader ch = this.projectListView.Columns.Add(columnTitles[i], 120, HorizontalAlignment.Left);
                ch.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            }

            //创建行
            this.projectListView.BeginUpdate();
            for (int i = 0; i < XmlConfig.Instance.projects.Count; i++)
            {
                ProjectConfig prj_cfg = XmlConfig.Instance.projects[i];
                ListViewItem lvi = new ListViewItem();
                lvi.Text = prj_cfg.name;
                lvi.SubItems.Add(prj_cfg.client_pc);
                lvi.SubItems.Add(prj_cfg.client_android);
                lvi.SubItems.Add(prj_cfg.client_build_pc);
                lvi.SubItems.Add(prj_cfg.client_build_android);
                lvi.SubItems.Add(prj_cfg.client_svn_url);
                this.projectListView.Items.Add(lvi);
                
            }
            this.projectListView.EndUpdate();
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            Console.WriteLine("MainForm_SizeChanged");
            this.tabControl1.Size = new Size();
            this.tabPage1.ResumeLayout(true);
        }
    }
}
