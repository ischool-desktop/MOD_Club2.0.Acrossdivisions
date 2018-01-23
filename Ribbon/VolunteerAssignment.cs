using FISCA.DSAClient;
using FISCA.DSAUtil;
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
using System.Xml;
using System.Xml.Linq;

namespace MOD_Club_Acrossdivisions
{
    public partial class VolunteerAssignment : BaseForm
    {
        /// <summary>
        /// 連線後,Call Service 取得之學校相關內容
        /// </summary>
        Dictionary<string, AcrossRecord> SchoolClubDic { get; set; }

        List<ClassRowInfo> RowList { get; set; }

        /// <summary>
        /// 國高中共同資源社團
        /// </summary>
        Dictionary<string, OnlineMergerClub> MergerClubDic { get; set; }

        BackgroundWorker BGW { get; set; }
        BackgroundWorker BGW_Run { get; set; }

        List<LoginSchool> LoginSchoolList { get; set; }

        Dictionary<string, string> LoginSchoolDic { get; set; }

        List<LogAssignRecord> LogDic { get; set; }

        public VolunteerAssignment()
        {
            InitializeComponent();

            BGW = new BackgroundWorker();
            BGW.RunWorkerCompleted += BGW_RunWorkerCompleted;
            BGW.DoWork += BGW_DoWork;

            BGW_Run = new BackgroundWorker();
            BGW_Run.RunWorkerCompleted += BGW_Run_RunWorkerCompleted;
            BGW_Run.DoWork += BGW_Run_DoWork;


        }

        private void VolunteerAssignment_Load(object sender, EventArgs e)
        {
            this.Text = "社團志願分配(跨部別)　資料取得中...";
            dataGridViewX1.Enabled = false;
            btnStart.Enabled = false;
            BGW.RunWorkerAsync();
        }

        void BGW_DoWork(object sender, DoWorkEventArgs e)
        {
            GetMountingShoolConfig();

            //是否可連線&是否可取得相關資料
            if (!tool.UserConnection())
                e.Cancel = true;

            //取得學校連線資料
            LoginSchoolList = tool._A.Select<LoginSchool>();
            LoginSchoolDic = new Dictionary<string, string>();
            foreach (LoginSchool s in LoginSchoolList)
            {
                if (!LoginSchoolDic.ContainsKey(s.School_Name))
                {
                    LoginSchoolDic.Add(s.School_Name, s.Remark);
                }
            }

            //取得學校相關資料
            SchoolClubDic = tool.SchoolClubDetail(LoginSchoolList);

            //資料整合(以掛載模組為主)
            MergerClubDic = tool.ResourceMerger(SchoolClubDic);

            //取得志願
            foreach (string each in SchoolClubDic.Keys)
            {
                //設定志願序
                SchoolClubDic[each].開始進行志願序資料整合();

                //整理班級學生資料
                SchoolClubDic[each].班級分類(LoginSchoolDic);

                //設定目前社團人數狀況
                SchoolClubDic[each].設定目前社團人數(MergerClubDic);

            }
        }

        void BGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Text = "社團志願分配(跨部別)";
            dataGridViewX1.Enabled = true;
            btnStart.Enabled = true;

            if (!e.Cancelled)
            {
                if (e.Error == null)
                {
                    #region 畫面處理

                    RowList = new List<ClassRowInfo>();

                    foreach (string each in SchoolClubDic.Keys)
                    {
                        List<ClassRowInfo> list = new List<ClassRowInfo>();
                        list.AddRange(SchoolClubDic[each].infoDic.Values);
                        list.Sort(SortClassIndex);
                        RowList.AddRange(list);
                    }

                    dataGridViewX1.AutoGenerateColumns = false;

                    dataGridViewX1.DataSource = RowList;

                    List<string> list_school = new List<string>();
                    foreach (LoginSchool each in LoginSchoolList)
                    {
                        list_school.Add(each.School_Name + "(" + LoginSchoolDic[each.School_Name] + ")");

                    }

                    lbHelpConSchool.Text = string.Format("{0}學年度 第{1}學期\n已連線學校:{2}", K12.Data.School.DefaultSchoolYear, K12.Data.School.DefaultSemester, string.Join("，", list_school));

                    #endregion
                }
                else
                {
                    MsgBox.Show("背景作業發生錯誤!\n" + e.Error.Message);
                }
            }
            else
            {
                MsgBox.Show("連線學校發生錯誤!!\n請至[跨部別->連線]功能修正錯誤");
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (dataGridViewX1.SelectedRows.Count < 1)
            {
                MsgBox.Show("請選擇班級後開始分配作業!!");
                return;
            }

            this.Text = "社團志願分配(跨部別)　開始志願分配...";
            btnStart.Enabled = false;

            //開始志願分配作業
            //學生清單

            List<ClassRowInfo> RowDataList = new List<ClassRowInfo>();
            foreach (DataGridViewRow row in dataGridViewX1.SelectedRows)
            {
                ClassRowInfo rowData = (ClassRowInfo)row.DataBoundItem;
                RowDataList.Add(rowData);
            }

            BGW_Run.RunWorkerAsync(RowDataList);
        }

        void BGW_Run_DoWork(object sender, DoWorkEventArgs e)
        {
            LogDic = new List<LogAssignRecord>();


            //資源整合社團 - MergerClubDic

            //目前要進行社團志願處理的班級 - RowDataList
            List<ClassRowInfo> RowDataList = (List<ClassRowInfo>)e.Argument;

            if (tool.已有社團記錄時) //有社團記錄時,不進行退社(True覆蓋,False略過)
            {
                //進行已選學生的退社處理
                foreach (ClassRowInfo cl in RowDataList)
                {
                    foreach (OnlineStudent student in cl.StudentList)
                    {
                        //退出社團
                        student.RetirementCommunity(MergerClubDic);
                    }
                }
            }

            //開始社團分配
            VolunteerJoinBox os = StartAllocation(RowDataList);

            #region 新增社團記錄

            Dictionary<string, XmlHelper> InsertDic = os.GetNewStudSCJoinList();
            foreach (string School_Name in InsertDic.Keys)
            {

                Connection me = new Connection();
                me.Connect(School_Name, tool._contract, FISCA.Authentication.DSAServices.PassportToken);
                me = me.AsContract(tool._contract);
                try
                {
                    Envelope rsp = me.SendRequest("_.InsterScjoin", new Envelope(InsertDic[School_Name]));
                }
                catch (Exception ex)
                {
                    MsgBox.Show("新增社團參與記錄發生錯誤!!\n" + ex.Message);
                }
            }

            #endregion

            #region 刪除社團記錄

            if (tool.已有社團記錄時)
            {
                Dictionary<string, XmlHelper> DeleteDic = os.GetDeleteStudSCJoinList();

                foreach (string School_Name in DeleteDic.Keys)
                {
                    Connection me = new Connection();
                    me.Connect(School_Name, tool._contract, FISCA.Authentication.DSAServices.PassportToken);
                    me = me.AsContract(tool._contract);
                    try
                    {
                        Envelope rsp = me.SendRequest("_.DeleteScjoin", new Envelope(DeleteDic[School_Name]));
                    }
                    catch (Exception ex)
                    {
                        MsgBox.Show("清除社團參與記錄發生錯誤!!\n" + ex.Message);
                    }
                }
            }

            #endregion

            //LOG未完成
            StringBuilder sbLog_s = new StringBuilder();
            sbLog_s.AppendLine("志願分配結果如下:");
            //LOG
            foreach (LogAssignRecord la in LogDic)
            {
                if (!string.IsNullOrEmpty(la.其它))
                {
                    sbLog_s.AppendLine("部別「" + la.部別 + "」班級「" + la.班級 + "」座號「" + la.座號 + "」學生「" + la.姓名 + "」志願「" + la.志願 + "」社團名稱「" + la.社團名稱 + "」(高優先學生)");
                }
                else
                {
                    sbLog_s.AppendLine("部別「" + la.部別 + "」班級「" + la.班級 + "」座號「" + la.座號 + "」學生「" + la.姓名 + "」志願「" + la.志願 + "」社團名稱「" + la.社團名稱 + "」");
                }
            }
            FISCA.LogAgent.ApplicationLog.Log("[特殊歷程]", "志願分配", sbLog_s.ToString());


            LogDic.Sort(SortLogRecord);
            StringBuilder sbLog = new StringBuilder();
            sbLog.AppendLine("志願分配結果如下:");
            //LOG
            foreach (LogAssignRecord la in LogDic)
            {
                if (!string.IsNullOrEmpty(la.其它))
                {
                    sbLog.AppendLine("部別「" + la.部別 + "」班級「" + la.班級 + "」座號「" + la.座號 + "」學生「" + la.姓名 + "」志願「" + la.志願 + "」社團名稱「" + la.社團名稱 + "」(高優先學生)");
                }
                else
                {
                    sbLog.AppendLine("部別「" + la.部別 + "」班級「" + la.班級 + "」座號「" + la.座號 + "」學生「" + la.姓名 + "」志願「" + la.志願 + "」社團名稱「" + la.社團名稱 + "」");
                }
            }
            FISCA.LogAgent.ApplicationLog.Log("社團跨部別分配", "志願分配", sbLog.ToString());

        }

        /// <summary>
        /// 開始進行社團分配作業
        /// </summary>
        private VolunteerJoinBox StartAllocation(List<ClassRowInfo> RowDataList)
        {

            //Step1.進行學生優先權判斷(社長副,社長)
            VolunteerJoinBox os = PriorityAllocation(RowDataList);

            #region 高優先學生

            //設定亂數
            SetRan(os.Step1Student);

            //開始進行社團分配之前,先進行亂數排序
            os.Step1Student.Sort(SortStudent);

            if (os.Step1Student.Count > 0)
            {
                Step1(os.Step1Student);
            }

            #endregion

            #region 一般學生

            List<OnlineStudent> list = new List<OnlineStudent>();
            list.AddRange(os.Step1Student);

            //第一階段未入設之學生
            //加入第二階段的亂數設定
            foreach (OnlineStudent stud in list)
            {
                if (!stud.JoinSuccess)
                {
                    os.Step2Student.Add(stud);

                    if (os.Step1Student.Contains(stud))
                    {
                        os.Step1Student.Remove(stud);
                    }
                }
            }

            //設定亂數
            SetRan(os.Step2Student);

            //開始進行社團分配之前,先進行亂數排序
            os.Step2Student.Sort(SortStudent);

            if (os.Step2Student.Count > 0)
            {
                for (int Number = 1; Number <= tool.學生選填志願數; Number++)
                {
                    Step2(os.Step2Student, Number);
                }
            }

            #endregion

            return os;

            //進行學生原社團資料刪除
        }

        /// <summary>
        /// 學生優先權判斷
        /// </summary>
        private VolunteerJoinBox PriorityAllocation(List<ClassRowInfo> list)
        {
            VolunteerJoinBox os = new VolunteerJoinBox();

            foreach (ClassRowInfo row in list) //班級
            {
                foreach (OnlineStudent student in row.StudentList) //學生
                {
                    //如果是鎖定學生,就不加入社團分配清單
                    if (student.IsLick)
                        continue;

                    if (!tool.已有社團記錄時 && student.原有社團 != null)
                        continue;

                    //學生是否有高優先權,會進行第一輪的志願分配
                    if (student.HighPriority)
                    {
                        os.Step1Student.Add(student);
                    }
                    else
                    {
                        os.Step2Student.Add(student);
                    }
                }
            }

            return os;
        }

        //高優先權學生(只處理第一志願)
        private void Step1(List<OnlineStudent> studList)
        {
            foreach (OnlineStudent stud in studList)
            {
                //沒有參與社團,尚未分配成功
                if (!stud.JoinSuccess)
                {
                    VolunteersDistributor(stud, 1, true);
                }
            }
        }

        private void Step2(List<OnlineStudent> studList, int Number)
        {
            foreach (OnlineStudent stud in studList)
            {
                //沒有參與社團,尚未分配成功
                if (!stud.JoinSuccess)
                {
                    VolunteersDistributor(stud, Number, false);
                }
            }

        }

        /// <summary>
        /// 設定新的亂數
        /// </summary>
        private void SetRan(List<OnlineStudent> list)
        {
            Random random = new Random();
            foreach (OnlineStudent each in list)
            {
                each.Random_Number = tool.random.Next(99999);
            }
        }

        /// <summary>
        /// 進行學生的志願分配
        /// </summary>
        private void VolunteersDistributor(OnlineStudent oStudent, int Number, bool IsStep1)
        {
            string message = "";
            if (oStudent.VolunteerList.Count != 0)
            {
                //如果學生身上有此志願,無此志願則視為選社失敗
                if (oStudent.VolunteerList.ContainsKey(Number))
                {
                    OnlineClub oc = oStudent.VolunteerList[Number];
                    if (IsStep1) //高優先權
                    {
                        if (oc.ClubName == oStudent.LastClubName)
                        {
                            #region 如果共同資源包含本社團

                            if (MergerClubDic.ContainsKey(oc.ClubName))
                            {
                                OnlineMergerClub Mclub = MergerClubDic[oc.ClubName];

                                //不符資格,將帶有錯誤訊息
                                oStudent.ErrorMessage = Mclub.EligibilityCheck(oStudent);

                                //符合資格
                                if (string.IsNullOrEmpty(oStudent.ErrorMessage))
                                {
                                    //符合資格
                                    oStudent.SuccessSetupClub(Mclub);

                                    LogAssignRecord lar = new LogAssignRecord();
                                    if (string.IsNullOrEmpty(LoginSchoolDic[oStudent.School]))
                                    {
                                        lar.部別 = oStudent.School + "(" + LoginSchoolDic[oStudent.School] + ")";
                                    }
                                    else
                                    {
                                        lar.部別 = oStudent.School;
                                    }
                                    lar.班級 = oStudent.ClassName;
                                    lar.姓名 = oStudent.Name;
                                    lar.座號 = oStudent.SeatNo;
                                    lar.志願 = Number.ToString();
                                    lar.社團名稱 = Mclub.ClubName;
                                    lar.其它 = "(高優先學生)";
                                    LogDic.Add(lar);

                                }
                            }

                            #endregion
                        }
                    }
                    else //第二優先權
                    {
                        #region 如果共同資源包含本社團

                        if (MergerClubDic.ContainsKey(oc.ClubName))
                        {
                            OnlineMergerClub Mclub = MergerClubDic[oc.ClubName];

                            //不符資格,將帶有錯誤訊息
                            oStudent.ErrorMessage = Mclub.EligibilityCheck(oStudent);

                            //符合資格
                            if (string.IsNullOrEmpty(oStudent.ErrorMessage))
                            {
                                //符合資格
                                oStudent.SuccessSetupClub(Mclub);

                                LogAssignRecord lar = new LogAssignRecord();

                                lar.部別 = GetSchoolName(oStudent.School);
                                lar.班級 = oStudent.ClassName;
                                lar.姓名 = oStudent.Name;
                                lar.座號 = oStudent.SeatNo;
                                lar.志願 = Number.ToString();
                                lar.社團名稱 = Mclub.ClubName;
                                lar.其它 = "";
                                LogDic.Add(lar);
                            }
                        }

                        #endregion
                    }
                }
            }
        }

        private string GetSchoolName(string p)
        {
            if (LoginSchoolDic.ContainsKey(p))
            {
                return p + "(" + LoginSchoolDic[p] + ")";
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 學生亂數排序
        /// </summary>
        private int SortStudent(OnlineStudent os1, OnlineStudent os2)
        {
            return os1.Random_Number.CompareTo(os2.Random_Number);
        }

        /// <summary>
        /// 班級排序
        /// </summary>
        private int SortClassIndex(ClassRowInfo class1, ClassRowInfo class2)
        {
            string aaa1 = class1.GradeYear.PadLeft(2, '0');
            aaa1 += class1.DisplayOrder.PadLeft(3, '0');
            aaa1 += class1.ClassName.PadLeft(10, '0');

            string bbb1 = class2.GradeYear.PadLeft(2, '0');
            bbb1 += class2.DisplayOrder.PadLeft(3, '0');
            bbb1 += class2.ClassName.PadLeft(10, '0');

            return aaa1.CompareTo(bbb1);
        }

        void BGW_Run_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Text = "社團志願分配(跨部別)";
            btnStart.Enabled = true;
            if (!e.Cancelled)
            {
                if (e.Error == null)
                {

                    K12.Club.Volunteer.ClubEvents.RaiseAssnChanged();

                    this.Text = "社團志願分配(跨部別)　資料取得中...";
                    btnStart.Enabled = false;
                    BGW.RunWorkerAsync();
                    DialogResult dr = MsgBox.Show("社團分配完成!!\n您是否要檢視分配狀況總表?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
                    if (dr == System.Windows.Forms.DialogResult.Yes)
                    {
                        //將LOG清單列印出來...
                        LogDic.Sort(SortLogRecord);

                        VolunteerLOG log = new VolunteerLOG(LogDic);
                        log.ShowDialog();
                    }
                }
                else
                {
                    MsgBox.Show("背景作業發生錯誤!\n" + e.Error.Message);
                }
            }
            else
            {
                MsgBox.Show("已停止志願分配!!!");
            }
        }

        private int SortLogRecord(LogAssignRecord lar1, LogAssignRecord lar2)
        {
            string Name1 = lar1.部別.PadLeft(20, '0');
            Name1 += lar1.班級.PadLeft(10, '0');
            Name1 += lar1.座號.PadLeft(3, '0');

            string Name2 = lar2.部別.PadLeft(20, '0');
            Name2 += lar2.班級.PadLeft(10, '0');
            Name2 += lar2.座號.PadLeft(3, '0');

            return Name1.CompareTo(Name2);
        }

        /// <summary>
        /// 取得模組掛載學校的設定值
        /// </summary>
        private void GetMountingShoolConfig()
        {
            //取得學校志願設定檔
            DataTable dt = tool._Q.Select("select * from $k12.config.universal");
            foreach (DataRow row in dt.Rows)
            {
                string name = "" + row["config_name"];
                if (name == "學生選填志願數")
                {
                    tool.學生選填志願數 = int.Parse("" + row["content"]);
                }

                if (name == "已有社團記錄時")
                {
                    tool.已有社團記錄時 = bool.Parse("" + row["content"]);
                }
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridViewX1_SelectionChanged(object sender, EventArgs e)
        {
            lbSelectClassCount.Text = "選擇班級：" + dataGridViewX1.SelectedRows.Count;

            toolStripMenuItem2.Enabled = (dataGridViewX1.SelectedRows.Count == 1);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //開啟學生的志願序相關資料畫面
            if (dataGridViewX1.SelectedRows.Count == 1)
            {
                foreach (DataGridViewRow row in dataGridViewX1.SelectedRows)
                {
                    ClassRowInfo rowData = (ClassRowInfo)row.DataBoundItem;
                    VolunteerStudentForm vsf = new VolunteerStudentForm(rowData);
                    vsf.ShowDialog();
                    break;
                }
            }
        }

        private void dataGridViewX1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                //開啟學生的志願序相關資料畫面
                if (dataGridViewX1.SelectedRows.Count == 1)
                {
                    foreach (DataGridViewRow row in dataGridViewX1.SelectedRows)
                    {
                        ClassRowInfo rowData = (ClassRowInfo)row.DataBoundItem;
                        VolunteerStudentForm vsf = new VolunteerStudentForm(rowData);
                        vsf.ShowDialog();
                        break;
                    }
                }
            }
        }

        private void dataGridViewX1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == colYear.Index)
            {
                RowList.Sort(SortRowList);

                dataGridViewX1.Refresh();
            }
        }

        private int SortRowList(ClassRowInfo cri1, ClassRowInfo cri2)
        {
            string name1 = cri1.GradeYear.PadLeft(3, '0');

            name1 += cri1.School.PadLeft(30, '0');
            name1 += cri1.ClassName.PadLeft(10, '0');

            string name2 = cri2.GradeYear.PadLeft(3, '0');
            name2 += cri2.School.PadLeft(30, '0');
            name2 += cri2.ClassName.PadLeft(10, '0');

            return name1.CompareTo(name2);
        }
    }

    public class LogAssignRecord
    {
        public string 部別 { get; set; }
        public string 姓名 { get; set; }
        public string 座號 { get; set; }
        public string 班級 { get; set; }
        public string 志願 { get; set; }
        public string 社團名稱 { get; set; }
        public string 其它 { get; set; }
    }
}
