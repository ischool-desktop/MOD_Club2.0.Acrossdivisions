using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using K12.Data;
using System.Xml;

namespace MOD_Club_Acrossdivisions
{
    public partial class VolunteerStudentForm : BaseForm
    {
        ClassRowInfo _VolRow { get; set; }

        /// <summary>
        /// 開始背景模式
        /// 取得學生選填的志願序內容
        /// </summary>
        BackgroundWorker BGW = new BackgroundWorker();

        Dictionary<string, OnlineClub> ClubDic { get; set; }

        List<string> StudentIDList = new List<string>();

        public VolunteerStudentForm(ClassRowInfo VolRow)
        {
            InitializeComponent();

            _VolRow = VolRow;
        }

        private void VolunteerStudentForm_Load(object sender, EventArgs e)
        {
            BGW.DoWork += new DoWorkEventHandler(BGW_DoWork);
            BGW.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BGW_RunWorkerCompleted);

            dataGridViewX1.Enabled = false;
            this.Text = "學生選填明細(資料取得中...)";

            //設定志願數
            SetColumn();

            BGW.RunWorkerAsync();
        }

        void BGW_DoWork(object sender, DoWorkEventArgs e)
        {
            #region 整理出社團基本資料

            List<DataGridViewRow> rowList = new List<DataGridViewRow>();
            _VolRow.StudentList.Sort(SortMergeList);

            //每一個學生
            foreach (OnlineStudent vr in _VolRow.StudentList)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridViewX1);
                row.Cells[2].Value = vr.Gender;

                string name = vr.Name;

                //是否鎖定
                if (vr.原有社團 != null)
                {
                    if (vr.原有社團.IsLock)
                    {
                        Color c = new Color();
                        row.DefaultCellStyle.BackColor = Color.GreenYellow;
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.Yellow;
                    }
                    name += "(" + vr.原有社團.ClubName + ")";
                }

                if (vr.HighPriority)
                {
                    row.Cells[0].Value = vr.SeatNo + "(＊)";
                }
                else
                {
                    row.Cells[0].Value = vr.SeatNo;
                }

                row.Cells[1].Value = name;

                int count = 2;
                if (vr.VolunteerList.Count > 0)
                {
                    foreach (int vol in vr.VolunteerList.Keys)
                    {
                        if (vol <= tool.學生選填志願數)
                        {
                            row.Cells[count + vol].Value = vr.VolunteerList[vol].ClubName;
                        }

                    }
                }

                rowList.Add(row);
            }

            foreach (OnlineStudent vr in _VolRow.StudentList)
            {
                if (vr.School == tool.Point) //本校學生
                {
                    if (vr.原有社團 == null)
                    {
                        StudentIDList.Add(vr.Id);
                    }
                }
            }

            e.Result = rowList;
            //ClubDic = tool.GetClub(ClubIDList);

            #endregion
        }

        void BGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dataGridViewX1.Enabled = true;
            this.Text = "學生選填明細";

            if (!e.Cancelled)
            {
                if (e.Error == null)
                {
                    List<DataGridViewRow> rowList = (List<DataGridViewRow>)e.Result;
                    dataGridViewX1.Rows.AddRange(rowList.ToArray());

                }
                else
                {
                    MsgBox.Show("已發生錯誤!!\n" + e.Error.Message);
                }
            }
            else
            {
                MsgBox.Show("資料取得動作已取消");
            }
        }

        /// <summary>
        /// 依據設定檔,建立Column
        /// </summary>
        private void SetColumn()
        {
            for (int x = 1; x <= tool.學生選填志願數; x++)
            {
                DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                column.Name = "ByVColumn" + x;
                column.HeaderText = "社團" + x;
                column.Width = 100;
                column.ReadOnly = true;
                dataGridViewX1.Columns.Add(column);
            }
        }

        private int SortMergeList(OnlineStudent s1, OnlineStudent s2)
        {
            string ss1 = s1.SeatNo.PadLeft(3, '0');
            string ss2 = s2.SeatNo.PadLeft(3, '0');
            return ss1.CompareTo(ss2);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 未選社學生加入待處理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MsgBox.Show("[加入待處理]功能\n限加入本模組掛載部別之學生!!");

            K12.Presentation.NLDPanels.Student.AddToTemp(StudentIDList);

            MsgBox.Show("已於[學生待處理]加入" + StudentIDList.Count + "名學生");
        }

        private void 清空學生待處理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            K12.Presentation.NLDPanels.Student.RemoveFromTemp(K12.Presentation.NLDPanels.Student.TempSource);
            MsgBox.Show("已清除[學生待處理]內所有學生!!");
        }
    }
}
