using Aspose.Words;
using FISCA.DSAClient;
using FISCA.Presentation;
using FISCA.Presentation.Controls;
using K12.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MOD_Club_Acrossdivisions
{
    public partial class SocietiesOverviewTable : BaseForm
    {
        //背景模式
        BackgroundWorker BGW = new BackgroundWorker();
        int _SchoolYear { get; set; }
        int _Semester { get; set; }

        string ConfigName = "SocietiesOverviewTableConfig";

        int 學生多少個 = 100;

        List<LoginSchool> LoginSchoolList { get; set; }

        Dictionary<string, string> LoginSchoolDic { get; set; }

        /// <summary>
        /// 連線後,Call Service 取得之學校相關內容
        /// </summary>
        Dictionary<string, AcrossRecord> SchoolClubDic { get; set; }

        /// <summary>
        /// 國高中共同資源社團
        /// </summary>
        Dictionary<string, OnlineMergerClub> MergerClubDic { get; set; }

        public SocietiesOverviewTable()
        {
            InitializeComponent();
        }

        private void SocietiesOverviewTable_Load(object sender, EventArgs e)
        {
            intSchoolYear.Value = int.Parse(K12.Data.School.DefaultSchoolYear);
            intSemester.Value = int.Parse(K12.Data.School.DefaultSemester);

            BGW.DoWork += BGW_DoWork;
            BGW.RunWorkerCompleted += BGW_RunWorkerCompleted;

            if (FISCA.Authentication.DSAServices.PassportToken == null)
            {
                MsgBox.Show("請使用ischool Account登入\n再使用跨部別功能!!");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!BGW.IsBusy)
            {
                btnSave.Enabled = false;
                _SchoolYear = intSchoolYear.Value;
                _Semester = intSemester.Value;

                BGW.RunWorkerAsync();
            }
            else
            {
                MsgBox.Show("系統忙碌中,稍後再試...");
            }
        }

        void BGW_DoWork(object sender, DoWorkEventArgs e)
        {
            #region 資料整理

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
            SchoolClubDic = SchoolClubDetail(LoginSchoolList);

            //資料整合(以掛載模組為主)
            MergerClubDic = tool.ResourceMerger(SchoolClubDic);


            //取得志願
            foreach (string each in SchoolClubDic.Keys)
            {
                //設定目前社團人數狀況
                SchoolClubDic[each].設定目前社團人數(MergerClubDic);
            }

            #endregion

            #region 報表範本整理

            Campus.Report.ReportConfiguration ConfigurationInCadre = new Campus.Report.ReportConfiguration(ConfigName);
            Aspose.Words.Document Template;

            if (ConfigurationInCadre.Template == null)
            {
                //如果範本為空,則建立一個預設範本
                Campus.Report.ReportConfiguration ConfigurationInCadre_1 = new Campus.Report.ReportConfiguration(ConfigName);
                ConfigurationInCadre_1.Template = new Campus.Report.ReportTemplate(Properties.Resources.社團概況表_範本, Campus.Report.TemplateType.Word);
                Template = ConfigurationInCadre_1.Template.ToDocument();
            }
            else
            {
                //如果已有範本,則取得樣板
                Template = ConfigurationInCadre.Template.ToDocument();
            }

            #endregion


            DataTable table = new DataTable();
            table.Columns.Add("學校名稱");
            table.Columns.Add("學年度");
            table.Columns.Add("學期");
            table.Columns.Add("列印日期");

            for (int x = 1; x <= 學生多少個; x++)
            {
                table.Columns.Add(string.Format("類型{0}", x));
                table.Columns.Add(string.Format("代碼{0}", x));
                table.Columns.Add(string.Format("社團名稱{0}", x));
                table.Columns.Add(string.Format("指導老師{0}", x));
                table.Columns.Add(string.Format("科別限制{0}", x));
                table.Columns.Add(string.Format("男女限制{0}", x));

                foreach (string each in SchoolClubDic.Keys)
                {
                    table.Columns.Add(string.Format("1年級{0}{1}", SchoolClubDic[each].SchoolRemake, x));
                    table.Columns.Add(string.Format("2年級{0}{1}", SchoolClubDic[each].SchoolRemake, x));
                    table.Columns.Add(string.Format("3年級{0}{1}", SchoolClubDic[each].SchoolRemake, x));
                }

                table.Columns.Add(string.Format("1年級總{0}", x));
                table.Columns.Add(string.Format("2年級總{0}", x));
                table.Columns.Add(string.Format("3年級總{0}", x));

                table.Columns.Add(string.Format("總人數{0}", x));
                table.Columns.Add(string.Format("場地{0}", x));
            }

            DataRow row = table.NewRow();
            row["學校名稱"] = K12.Data.School.ChineseName;
            row["學年度"] = _SchoolYear;
            row["學期"] = _Semester;
            row["列印日期"] = DateTime.Today.ToShortDateString();
            int y = 1;
            foreach (string each in MergerClubDic.Keys)
            {
                if (y <= 學生多少個)
                {
                    OnlineMergerClub club = MergerClubDic[each];

                    row[string.Format("類型{0}", y)] = club.ClubCategory;
                    row[string.Format("代碼{0}", y)] = club.ClubNumber;
                    row[string.Format("社團名稱{0}", y)] = club.ClubName;
                    row[string.Format("指導老師{0}", y)] = club.TeacherName;
                    row[string.Format("男女限制{0}", y)] = club.GenderRestrict;
                    row[string.Format("總人數{0}", y)] = club.NowStudentCount;
                    row[string.Format("場地{0}", y)] = club.Location;

                    int Grade1 = 0;
                    int Grade2 = 0;
                    int Grade3 = 0;

                    foreach (string gr1 in club.Grade1.SchoolGradeDic.Keys)
                    {
                        row[string.Format("1年級{0}{1}", LoginSchoolDic[gr1], y)] = club.Grade1.SchoolGradeDic[gr1].ToString();
                        Grade1 += club.Grade1.SchoolGradeDic[gr1];
                    }

                    foreach (string gr2 in club.Grade2.SchoolGradeDic.Keys)
                    {
                        row[string.Format("2年級{0}{1}", LoginSchoolDic[gr2], y)] = club.Grade2.SchoolGradeDic[gr2].ToString();
                        Grade2 += club.Grade2.SchoolGradeDic[gr2];
                    }

                    foreach (string gr3 in club.Grade3.SchoolGradeDic.Keys)
                    {
                        row[string.Format("3年級{0}{1}", LoginSchoolDic[gr3], y)] = club.Grade3.SchoolGradeDic[gr3].ToString();
                        Grade3 += club.Grade3.SchoolGradeDic[gr3];
                    }

                    if (Grade1 != 0)
                        row[string.Format("1年級總{0}", y)] = Grade1;
                    if (Grade2 != 0)
                        row[string.Format("2年級總{0}", y)] = Grade2;
                    if (Grade3 != 0)
                        row[string.Format("3年級總{0}", y)] = Grade3;

                    y++;
                }
            }

            table.Rows.Add(row);

            Document PageOne = (Document)Template.Clone(true);
            PageOne.MailMerge.Execute(table);
            e.Result = PageOne;

        }

        void BGW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Text = "社團概況表(跨部別)";
            btnSave.Enabled = true;

            if (!e.Cancelled)
            {
                if (e.Error == null)
                {
                    SaveFileDialog SaveFileDialog1 = new SaveFileDialog();
                    SaveFileDialog1.Filter = "Word (*.doc)|*.doc|所有檔案 (*.*)|*.*";
                    SaveFileDialog1.FileName = "社團概況表(跨部別)";

                    //資料
                    try
                    {
                        if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            Aspose.Words.Document inResult = (Aspose.Words.Document)e.Result;

                            inResult.Save(SaveFileDialog1.FileName);
                            Process.Start(SaveFileDialog1.FileName);
                            MotherForm.SetStatusBarMessage("社團概況表(跨部別),列印完成!!");
                        }
                        else
                        {
                            FISCA.Presentation.Controls.MsgBox.Show("檔案未儲存");
                            return;
                        }
                    }
                    catch
                    {
                        FISCA.Presentation.Controls.MsgBox.Show("檔案儲存錯誤,請檢查檔案是否開啟中!!");
                        MotherForm.SetStatusBarMessage("檔案儲存錯誤,請檢查檔案是否開啟中!!");
                    }
                }
                else
                {
                    MsgBox.Show("列印發生錯誤..\n" + e.Error.Message);
                }
            }
            else
            {
                MsgBox.Show("作業已取消");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 取得各校之相關基本資料(社團/學生)
        /// </summary>
        public Dictionary<string, AcrossRecord> SchoolClubDetail(List<LoginSchool> LoginSchoolList)
        {
            Dictionary<string, AcrossRecord> AcrossDic = new Dictionary<string, AcrossRecord>();
            foreach (LoginSchool login in LoginSchoolList)
            {
                Connection me = new Connection();
                me.Connect(login.School_Name, tool._contract, FISCA.Authentication.DSAServices.PassportToken);
                me = me.AsContract(tool._contract);

                //取得 - 學年度/學期
                //目前系統中有參與記錄的學生 & 是否鎖定
                Dictionary<string, OnlineSCJoin> StudentSCJoinDic = RunService.GetSCJoinStudent(me, _SchoolYear.ToString(), _Semester.ToString());

                AcrossRecord ar = new AcrossRecord();
                ar.School = login.School_Name;
                ar.SchoolRemake = login.Remark;

                //取得可選社之學生
                //年級/班級/座號/姓名/科別
                //StudentSCJoinDic - 是否有社團參與記錄
                //StudentPresidentDic - 是否有'前期'社長副/社長記錄
                ar.StudentDic = GetStuentList(me, StudentSCJoinDic);

                //取得預設(學年度,學期)學校之社團記錄
                ar.ClubDic = RunService.GetClubList(me, _SchoolYear.ToString(), _Semester.ToString());

                if (!AcrossDic.ContainsKey(login.School_Name))
                {
                    AcrossDic.Add(login.School_Name, ar);
                }
            }

            return AcrossDic;
        }

        /// <summary>
        /// 取得學生清單
        /// </summary>
        public Dictionary<string, OnlineStudent> GetStuentList(Connection me, Dictionary<string, OnlineSCJoin> SCJoinDic)
        {
            Envelope rsp = me.SendRequest("_.GetStudentsCanChoose", new Envelope(RunService.GetRequest(null, null)));
            XElement students = XElement.Parse(rsp.Body.XmlString);

            Dictionary<string, OnlineStudent> StudentDic = new Dictionary<string, OnlineStudent>();
            foreach (XElement stud in students.Elements("Student"))
            {
                OnlineStudent os = new OnlineStudent(stud);
                os.School = me.AccessPoint.Name;

                if (!StudentDic.ContainsKey(os.Id))
                {
                    StudentDic.Add(os.Id, os);
                }

                //是否有社團參與記錄
                if (SCJoinDic.ContainsKey(os.Id))
                {
                    os.原有社團 = SCJoinDic[os.Id];
                }
            }
            return StudentDic;
        }

        private void lbDefTemp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //取得設定檔
            Campus.Report.ReportConfiguration ConfigurationInCadre = new Campus.Report.ReportConfiguration(ConfigName);
            Campus.Report.TemplateSettingForm TemplateForm;
            //畫面內容(範本內容,預設樣式
            if (ConfigurationInCadre.Template != null)
            {
                TemplateForm = new Campus.Report.TemplateSettingForm(ConfigurationInCadre.Template, new Campus.Report.ReportTemplate(Properties.Resources.社團概況表_範本, Campus.Report.TemplateType.Word));
            }
            else
            {
                ConfigurationInCadre.Template = new Campus.Report.ReportTemplate(Properties.Resources.社團概況表_範本, Campus.Report.TemplateType.Word);
                TemplateForm = new Campus.Report.TemplateSettingForm(ConfigurationInCadre.Template, new Campus.Report.ReportTemplate(Properties.Resources.社團概況表_範本, Campus.Report.TemplateType.Word));
            }

            //預設名稱
            TemplateForm.DefaultFileName = "社團概況表_範本";

            //如果回傳為OK
            if (TemplateForm.ShowDialog() == DialogResult.OK)
            {
                //設定後樣試,回傳
                ConfigurationInCadre.Template = TemplateForm.Template;
                //儲存
                ConfigurationInCadre.Save();
            }
        }

        private void lbTempAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "另存新檔";
            sfd.FileName = "社團點名表_合併欄位總表.doc";
            sfd.Filter = "Word檔案 (*.doc)|*.doc|所有檔案 (*.*)|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FileStream fs = new FileStream(sfd.FileName, FileMode.Create);
                    fs.Write(Properties.Resources.社團點名表_合併欄位總表, 0, Properties.Resources.社團點名表_合併欄位總表.Length);
                    fs.Close();
                    System.Diagnostics.Process.Start(sfd.FileName);
                }
                catch
                {
                    FISCA.Presentation.Controls.MsgBox.Show("指定路徑無法存取。", "另存檔案失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

    }
}
