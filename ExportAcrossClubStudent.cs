using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.UDT;
using FISCA.DSAClient;
using System.Xml.Linq;
using Aspose.Cells;
using System.IO;
using FISCA.Presentation;
using System.Windows.Forms;
using System.Diagnostics;
using K12.Club.Volunteer;
using System.ComponentModel;
using System.Data;
using FISCA.Data;
using K12.Data;

namespace MOD_Club_Acrossdivisions
{
    class ExportAcrossClubStudent
    {
        /// <summary>
        /// 連線學校
        /// </summary>
        List<LoginSchool> loginSchoolList { get; set; }

        // DIC 社團ID 社團名稱
        Dictionary<string, string> clubDic = new Dictionary<string, string>();

        public ExportAcrossClubStudent(string sy,string s)
        {
            Workbook wb = new Workbook();
            Exception exc = null;
            BackgroundWorker bgw = new BackgroundWorker() { WorkerReportsProgress = true };
            bgw.ProgressChanged += delegate (object sender,ProgressChangedEventArgs e)
            {
                MotherForm.SetStatusBarMessage("匯出選社結果(跨部別)",e.ProgressPercentage);
            };
            bgw.DoWork += delegate 
            {
                try
                {
                    #region 跨部別
                    {
                        bgw.ReportProgress(1);
                        AccessHelper access = new AccessHelper();
                        loginSchoolList = access.Select<LoginSchool>();
                        //DIC
                        Dictionary<string, OnlineVolunteer> volunteerDic = new Dictionary<string, OnlineVolunteer>();
                        Dictionary<string, OnlineSCJoin> scjoinDic = new Dictionary<string, OnlineSCJoin>();
                        Dictionary<string, string> clubNameDic = new Dictionary<string, string>();
                        //--
                        wb.Open(new MemoryStream(Properties.Resources.匯出選社結果_跨部別__範本), FileFormatType.Excel2007Xlsx);

                        Worksheet[] ws = new Worksheet[2];
                        int sheet = 0;
                        int p = 10;
                        foreach (LoginSchool school in loginSchoolList)
                        {
                            p = p + 90 / loginSchoolList.Count;
                            bgw.ReportProgress(p);
                            ws[sheet] = wb.Worksheets[sheet];
                            
                            Connection connection = new Connection();
                            connection.Connect(school.School_Name, "ClubAcrossdivisions", FISCA.Authentication.DSAServices.PassportToken);
                            ws[sheet].Name = school.GetFullName();

                            // 取得社團清單
                            Envelope rsp = connection.SendRequest("_.GetClubList", new Envelope(RunService.GetRequest(sy, s)));
                            XDocument clubDoc = XDocument.Parse(rsp.Body.XmlString);
                            List<XElement> clubList = clubDoc.Element("Response").Elements("K12.clubrecord.universal").ToList();
                            foreach (XElement club in clubList)
                            {
                                clubNameDic.Add(club.Element("Uid").Value, club.Element("ClubName").Value);
                            }
                            // 取得學生志願序
                            volunteerDic.Clear();
                            rsp = connection.SendRequest("_.GetStudentVolunteer", new Envelope(RunService.GetRequest(sy, s)));
                            XDocument volunteerDoc = XDocument.Parse(rsp.Body.XmlString);
                            List<XElement> volunteers = volunteerDoc.Element("Response").Elements("K12.volunteer.universal").ToList();
                            foreach (XElement volunteer in volunteers)
                            {
                                OnlineVolunteer vol = new OnlineVolunteer(volunteer);

                                volunteerDic.Add(vol.RefStudentId,vol);
                            }

                            // 取得學生入選紀錄
                            scjoinDic.Clear();
                            rsp = connection.SendRequest("_.GetStudentSCJoin", new Envelope(RunService.GetRequest(sy, s)));
                            XDocument scjDoc = XDocument.Parse(rsp.Body.XmlString);
                            List<XElement> scjoins = scjDoc.Element("Response").Elements("Student").ToList();
                            foreach (XElement scjoin in scjoins)
                            {
                                OnlineSCJoin scj = new OnlineSCJoin(scjoin);
                                scjoinDic.Add(scj.StudentId, scj);
                            }

                            // 讀取志願數上限設定
                            List<ConfigRecord> crList = access.Select<K12.Club.Volunteer.ConfigRecord>();
                            for (int i = 6; i < int.Parse(crList[0].Content) + 6; i++)
                            {
                                ws[sheet].Cells.CopyColumn(ws[sheet].Cells, 4, i);
                                ws[sheet].Cells[0, i].PutValue("志願" + (i - 5));
                            }

                            // 取得所有學生
                            rsp = connection.SendRequest("_.GetAllStudent", new Envelope(RunService.GetRequest(sy, s)));
                            XDocument allStudentDoc = XDocument.Parse(rsp.Body.XmlString);
                            List<XElement> students = allStudentDoc.Element("Response").Elements("Student").ToList();

                            int index = 1;
                            foreach (XElement student in students)
                            {
                                if (scjoinDic.ContainsKey(student.Element("Id").Value))
                                {
                                    ws[sheet].Cells.CopyRow(ws[sheet].Cells, 1, index);
                                    ws[sheet].Cells[index, 0].PutValue(student.Element("ClassName").Value);
                                    ws[sheet].Cells[index, 1].PutValue(student.Element("SeatNo").Value);
                                    ws[sheet].Cells[index, 2].PutValue(student.Element("Name").Value);
                                    ws[sheet].Cells[index, 3].PutValue(student.Element("StudentNumber").Value);
                                    ws[sheet].Cells[index, 4].PutValue(scjoinDic[student.Element("Id").Value].ClubName);
                                    ws[sheet].Cells[index, 5].PutValue(scjoinDic[student.Element("Id").Value].IsLock == true ? "是" : "");

                                    List<XElement> club = XDocument.Parse(volunteerDic[student.Element("Id").Value].Content).Element("xml").Elements("Club").ToList();
                                    int i = 1;
                                    // 取得志願數設定
                                    int clubCount = int.Parse("" + crList[0].Content);
                                    foreach (XElement clubID in club)
                                    {
                                        ws[sheet].Cells[index, 5 + i].PutValue(clubNameDic[clubID.Attribute("Ref_Club_ID").Value]);
                                        if (i == clubCount)
                                            break;
                                        i++;
                                    }
                                    index++;
                                }
                            }
                            sheet++;
                        }
                    }
                    #endregion

                    #region 
                    {
//                        bgw.ReportProgress(51);
//                        AccessHelper access = new AccessHelper();
//                        List<CLUBRecord> _clubList = access.Select<CLUBRecord>(/*"school_year = "+School.DefaultSchoolYear+" AND semester = "+ School.DefaultSemester*/).ToList();
//                        foreach (CLUBRecord club in _clubList)
//                        {
//                            clubDic.Add(club.UID, club.ClubName);
//                        }
//                        //取得Sheet
//                        Worksheet ws = wb.Worksheets[0];
//                        ws.Name = School.ChineseName;
//                        QueryHelper qh = new QueryHelper();

//                        #region SQL
//                        string selectSQL = string.Format(@"
//SELECT 
//	student.id
//    , class.class_name
//    , class.grade_year
//    , seat_no,name
//    , student_number
//    , student.ref_class_id
//    , scjoin.ref_club_id
//    , scjoin.lock
//    , clubrecord.club_name
//    , clubrecord.school_year
//    , clubrecord.semester
//    , volunteer.content
//    , $k12.config.universal.content as wish_limit
//FROM 
//    student 
//    LEFT OUTER JOIN class on class.id = student.ref_class_id
//    LEFT OUTER JOIN $k12.scjoin.universal AS scjoin
//        ON scjoin.ref_student_id::bigint = student.id
//    LEFT OUTER JOIN $k12.clubrecord.universal AS clubrecord 
//        ON clubrecord.uid = scjoin.ref_club_id::bigint
//    LEFT OUTER JOIN $k12.volunteer.universal AS volunteer
//        ON volunteer.ref_student_id::bigint = student.id 
//            AND volunteer.school_year = clubrecord.school_year
//            AND volunteer.semester = clubrecord.semester
//    LEFT OUTER JOIN $k12.config.universal ON config_name = '學生選填志願數'
//WHERE 
//    student.status in (1, 2) 
//    AND clubrecord.school_year = {0} 
//    AND clubrecord.semester = {1}
//ORDER BY 
//    class.grade_year
//    , class.display_order
//    , class.class_name
//    , student.seat_no
//    , student.id"
//                        , sy, s);
//                        #endregion

//                        // 取得學生社團資料
//                        DataTable dt = qh.Select(selectSQL);

//                        bgw.ReportProgress(55);
//                        int index = 1;
//                        foreach (DataRow dr in dt.Rows)
//                        {
//                            bgw.ReportProgress(55 + 45 * index / dt.Rows.Count);
//                            int wishLimit = (dr["wish_limit"] == null ? 5 : int.Parse("" + dr["wish_limit"]));
//                            if (index == 1)
//                            {
//                                int count = 6;
//                                int countLimit = count + wishLimit;
//                                for (int i = count; i < countLimit; i++)
//                                {
//                                    ws.Cells.CopyColumn(ws.Cells, 4, i);
//                                    ws.Cells[0, i].PutValue("志願" + (i - 5));
//                                }
//                            }
//                            else
//                                ws.Cells.CopyRow(ws.Cells, 1, index);

//                            ws.Cells[index, 0].PutValue("" + dr["class_name"]);
//                            ws.Cells[index, 1].PutValue("" + dr["seat_no"]);
//                            ws.Cells[index, 2].PutValue("" + dr["name"]);
//                            ws.Cells[index, 3].PutValue("" + dr["student_number"]);
//                            ws.Cells[index, 4].PutValue("" + dr["club_name"]);
//                            ws.Cells[index, 5].PutValue(("" + dr["lock"]) == "true" ? "是" : "");

//                            // 學生志願序
//                            if (dr["content"] != null && "" + dr["content"] != "")
//                            {
//                                XDocument content = XDocument.Parse("" + dr["content"]);
//                                List<XElement> clubList = content.Element("xml").Elements("Club").ToList();
//                                int count = 6;
//                                int countLimit = count + wishLimit - 1;
//                                foreach (XElement club in clubList)
//                                {
//                                    ws.Cells[index, count].PutValue(clubDic[club.Attribute("Ref_Club_ID").Value]);
//                                    count++;
//                                    if (count > countLimit)
//                                        break;
//                                }
//                            }
//                            index++;
//                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    exc = ex;
                }
            };

            bgw.RunWorkerCompleted += delegate 
            {
                if (exc == null)
                {
                    #region Excel 存檔
                    {
                        SaveFileDialog SaveFileDialog1 = new SaveFileDialog();
                        SaveFileDialog1.Filter = "Excel (*.xls)|*.xls|所有檔案 (*.*)|*.*";
                        SaveFileDialog1.FileName = "匯出選社結果(跨部別)";
                        try
                        {
                            if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
                            {
                                wb.Save(SaveFileDialog1.FileName);
                                Process.Start(SaveFileDialog1.FileName);
                                MotherForm.SetStatusBarMessage("匯出選社結果(跨部別),列印完成!!");

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
                    #endregion
                }
                else
                {
                    throw new Exception("匯出選社結果 發生錯誤", exc);
                }
            };

            bgw.RunWorkerAsync();
        }
    }
}
