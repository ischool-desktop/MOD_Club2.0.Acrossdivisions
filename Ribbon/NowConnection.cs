using FISCA.DSAClient;
using FISCA.Presentation.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MOD_Club_Acrossdivisions
{
    public partial class NowConnection : BaseForm
    {

        Campus.Configuration.ConfigData cd { get; set; }
        public NowConnection()
        {
            InitializeComponent();
        }

        private void Connection_Load(object sender, EventArgs e)
        {
            List<LoginSchool> LoginSchoolList = tool._A.Select<LoginSchool>();

            if (LoginSchoolList.Count() > 0)
            {
                foreach (LoginSchool each in LoginSchoolList)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dataGridViewX1);
                    row.Cells[SchoolDomain.Index].Value = each.School_Name;
                    row.Cells[remake.Index].Value = each.Remark;
                    row.Cells[ColConnection.Index].Value = "測試";
                    dataGridViewX1.Rows.Add(row);

                    //檢查
                    Exception ex = tool.TestConnection(each.School_Name);
                    if (ex != null)
                        row.Cells[ColStatus.Index].Value = ex.Message;
                    else
                        row.Cells[ColStatus.Index].Value = "成功";
                }
            }
        }

        private void Connet(DataGridViewRow row)
        {
            LastObj _app = new LastObj();
            _app.row = row;
            _app.row.Cells[ColStatus.Index].Value = "連線中,請稍後...";

            BackgroundWorker BGW = new BackgroundWorker();
            BGW.DoWork += BGW_DoWork;
            BGW.RunWorkerCompleted += BGW_RunWorkerCompleted;
            BGW.RunWorkerAsync(_app);
        }

        void BGW_DoWork(object sender, DoWorkEventArgs e)
        {
            LastObj _app = (LastObj)e.Argument;
            _app.message = tool.CheckAccount("" + _app.row.Cells[SchoolDomain.Index].Value);
            e.Result = _app;
        }

        void BGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            LastObj _app = (LastObj)e.Result;
            if (!e.Cancelled)
            {
                if (e.Error == null)
                {
                    _app.row.Cells[ColStatus.Index].Value = _app.message;
                }
                else
                {
                    _app.row.Cells[ColStatus.Index].Value = "失敗：其它：" + e.Error.Message;
                }
            }
            else
            {
                _app.row.Cells[ColStatus.Index].Value = "失敗：其它：已取消連線";
            }
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewX1.CurrentRow.IsNewRow)
                return;

            //當使用點擊位置並非錯誤欄位
            if (e.ColumnIndex == ColConnection.Index)
            {
                DataGridViewRow row = dataGridViewX1.CurrentRow;
                Connet(row);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridViewX1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            if (e.Row.Index != -1)
            {
                DataGridViewRow row = dataGridViewX1.Rows[e.Row.Index - 1];
                row.Cells[ColConnection.Index].Value = "測試連線";
                row.Cells[ColStatus.Index].Value = "未連線";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool check = false;
            foreach (DataGridViewRow row in dataGridViewX1.Rows)
            {
                if (row.IsNewRow)
                    continue;

                if ("" + row.Cells[ColStatus.Index].Value != "成功")
                {
                    check = true;
                }
            }
            if (check)
            {
                MsgBox.Show("資料尚有錯誤,無法儲存!!");
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("已調整志願分配部別設定:");
            sb.AppendLine("原設定:");

            List<LoginSchool> delList = tool._A.Select<LoginSchool>();
            tool._A.DeletedValues(delList);
            foreach (LoginSchool each in delList)
            {
                sb.AppendLine("學校「" + each.School_Name + "」" + "部別名稱「" + each.Remark + "」");

            }

            sb.AppendLine("修改為:");

            List<LoginSchool> LoginSchoolList = new List<LoginSchool>();
            foreach (DataGridViewRow row in dataGridViewX1.Rows)
            {
                if (row.IsNewRow)
                    continue;

                if ("" + row.Cells[ColStatus.Index].Value == "成功")
                {
                    LoginSchool ls = new LoginSchool();
                    ls.School_Name = "" + row.Cells[SchoolDomain.Index].Value;
                    ls.Remark = "" + row.Cells[remake.Index].Value;
                    LoginSchoolList.Add(ls);

                    sb.AppendLine("學校「" + ls.School_Name + "」" + "部別名稱「" + ls.Remark + "」");
                }
            }

            if (LoginSchoolList.Count > 0)
            {
                tool._A.InsertValues(LoginSchoolList);
                FISCA.LogAgent.ApplicationLog.Log("志願分配部別設定", "連線學校", sb.ToString());

                MsgBox.Show("儲存成功");
                this.Close();
            }
            else
            {
                MsgBox.Show("未儲存任何資料!");
            }
        }
    }
    class LastObj
    {
        public DataGridViewRow row { get; set; }
        public string message = "";
    }
}
