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

namespace MOD_Club_Acrossdivisions
{
    public class Volunteer
    {
        public string RefStudentID;
        public string SchoolYear;
        public string Semester;
        public XDocument Content;
    }

    public class SCJoin
    {
        public string ID;
        public string Name;
        public string StudentNumber;
        public string ClassName;
        public string SeatNo;
        public string ClubName;
        public string clubID;
        public string SchoolYear;
        public string Semester;
        public string Lock;
    }

    class ExportAcrossClubStudent
    {
        /// <summary>
        /// 連線學校
        /// </summary>
        List<LoginSchool> loginSchoolList { get; set; }

        public ExportAcrossClubStudent()
        {
            AccessHelper access = new AccessHelper();
            loginSchoolList = access.Select<LoginSchool>();

            // 取得選社學年度學期
            List< K12.Club.Volunteer.UDT.OpenSchoolYearSemester> ss = access.Select<K12.Club.Volunteer.UDT.OpenSchoolYearSemester>();

            //DIC
            Dictionary<string,Volunteer> volunteearDic = new Dictionary<string, Volunteer>();
            Dictionary<string, SCJoin> scjoinDic = new Dictionary<string, SCJoin>();

            //--
            Workbook wb = new Workbook();
            wb.Open(new MemoryStream(Properties.Resources.匯出選社結果_跨部別__範本), FileFormatType.Excel2007Xlsx);
            Worksheet ws = wb.Worksheets[0];

            foreach (LoginSchool school in loginSchoolList)
            {
                Connection connection = new Connection();
                connection.Connect(school.School_Name, "ClubAcrossdivisions", FISCA.Authentication.DSAServices.PassportToken);

                Envelope rsp = connection.SendRequest("_.GetAllStudent", new Envelope(RunService.GetRequest(ss[0].SchoolYear, ss[0].Semester)));

                // 取得學生志願序
                rsp = connection.SendRequest("_.GetStudentVolunteer",new Envelope(RunService.GetRequest(ss[0].SchoolYear, ss[0].Semester)));
                XDocument volunteerDoc = XDocument.Parse(rsp.Body.XmlString);
                List<XElement> volunteers = volunteerDoc.Element("Response").Elements("K12.volunteer.universal").ToList();
                foreach (XElement volunteer in volunteers)
                {
                    Volunteer vol = new Volunteer();
                    vol.RefStudentID = volunteer.Element("RefStudentId").Value;
                    vol.SchoolYear = volunteer.Element("SchoolYear").Value;
                    vol.Content = XDocument.Parse(volunteer.Element("Content").Value);
                    // 學生只會有一筆志願序紀錄
                    if (volunteearDic.ContainsKey(vol.RefStudentID))
                    {
                        MessageBox.Show(vol.RefStudentID);
                    }
                    if (!volunteearDic.ContainsKey(vol.RefStudentID))
                    {
                        volunteearDic.Add(vol.RefStudentID, vol);
                    }
                    
                }

                // 取得學生入選紀錄
                rsp = connection.SendRequest("_.GetStudentSCJoin",new Envelope(RunService.GetRequest(ss[0].SchoolYear, ss[0].Semester)));
                XDocument scjDoc = XDocument.Parse(rsp.Body.XmlString);
                List<XElement> scjoins = scjDoc.Element("Response").Elements("Student").ToList();
                foreach (XElement scjoin in scjoins)
                {
                    SCJoin scj = new SCJoin();
                    scj.ID = scjoin.Element("Id").Value;
                    scj.Name = scjoin.Element("Name").Value;
                    scj.ClubName = scjoin.Element("ClubName").Value;
                    scj.clubID = scjoin.Element("ClubID").Value;
                    scj.SchoolYear = scjoin.Element("SchoolYear").Value;
                    scj.Semester = scjoin.Element("Semester").Value;
                    scj.ClassName = scjoin.Element("ClassName").Value;
                    scj.Lock = scjoin.Element("Lock").Value;
                    scj.StudentNumber = scjoin.Element("StudentNumber").Value;
                    scjoinDic.Add(scj.ID,scj);
                }

                // 取得所有學生
                XDocument allStudentDoc = XDocument.Parse(rsp.Body.XmlString);
                List<XElement> students = allStudentDoc.Element("Response").Elements("Student").ToList();
                students.OrderBy(x => x.Element("ClassName").Value).ToList();
                students.OrderBy(x => x.Element("SeatNo").Value).ToList();

                int index = 1;
                foreach (XElement student in students)
                {
                    ws.Cells.CopyRow(ws.Cells,1,index);
                    ws.Cells[index, 0].PutValue(student.Element("ClassName").Value);
                    ws.Cells[index, 1].PutValue(student.Element("SeatNo").Value);
                    ws.Cells[index, 2].PutValue(student.Element("Name").Value);
                    ws.Cells[index, 3].PutValue(scjoinDic[student.Element("Id").Value].StudentNumber);
                    ws.Cells[index, 4].PutValue(scjoinDic[student.Element("Id").Value].ClubName);
                    ws.Cells[index, 5].PutValue(scjoinDic[student.Element("Id").Value].Lock == "t" ? "是" : "");

                    List<XElement> club = volunteearDic[student.Element("Id").Value].Content.Element("Content").Element("xml").Elements("Club").ToList();

                    index++;
                }
                // list_tallItems.Sort((x, y) => { return -x.Height.CompareTo(y.Height); });
            }

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
    }
}
