using FISCA.DSAClient;
using FISCA.Presentation.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MOD_Club_Acrossdivisions
{
    static public class RunService
    {

        #region 社團清單

        /// <summary>
        /// 取得預設(學年度/學期)社團清單
        /// </summary>
        static public Dictionary<string, OnlineClub> GetDefClubList(Connection me)
        {
            return GetClubList(me, K12.Data.School.DefaultSchoolYear, K12.Data.School.DefaultSemester);
        }

        /// <summary>
        /// 取得指定之(學年度/學期)社團清單
        /// </summary>
        static public Dictionary<string, OnlineClub> GetClubList(Connection me, string SchoolYear, string Semester)
        {
            Envelope rsp = me.SendRequest("_.GetClubList", new Envelope(GetRequest(SchoolYear, Semester)));
            XElement clubs = XElement.Parse(rsp.Body.XmlString);

            Dictionary<string, OnlineClub> ClubDic = new Dictionary<string, OnlineClub>();
            foreach (XElement club in clubs.Elements("K12.clubrecord.universal"))
            {
                OnlineClub cr = new OnlineClub(club);
                cr.School = me.AccessPoint.Name;
                if (!ClubDic.ContainsKey(cr.UID))
                {
                    ClubDic.Add(cr.UID, cr);
                }
            }
            return ClubDic;
        }

        #endregion

        #region 志願序內容
        /// <summary>
        /// 取得預設(學年度/學期)志願序內容
        /// </summary>
        static public List<OnlineVolunteer> GetDefVolunteer(Connection me, LoginSchool login)
        {
            return GetVolunteer(me, login, K12.Data.School.DefaultSchoolYear, K12.Data.School.DefaultSemester);
        }

        /// <summary>
        /// 取得志願序內容
        /// </summary>
        static public List<OnlineVolunteer> GetVolunteer(Connection me, LoginSchool login, string SchoolYear, string Semester)
        {
            Envelope rsp = me.SendRequest("_.GetStudentVolunteer", new Envelope(GetRequest(SchoolYear, Semester)));
            XElement Volunteers = XElement.Parse(rsp.Body.XmlString);
            List<OnlineVolunteer> VolunteerList = new List<OnlineVolunteer>();

            foreach (XElement Volunteer in Volunteers.Elements("K12.volunteer.universal"))
            {
                OnlineVolunteer cr = new OnlineVolunteer(Volunteer);
                cr.School = login.School_Name;
                VolunteerList.Add(cr);
            }
            return VolunteerList;
        }
        #endregion

        #region 學生社團參與記錄

        /// <summary>
        /// 取得預設(學年度/學期)學生社團參與記錄
        /// </summary>
        public static Dictionary<string, OnlineSCJoin> GetDefSCJoinStudent(Connection me)
        {
            return GetSCJoinStudent(me, K12.Data.School.DefaultSchoolYear, K12.Data.School.DefaultSemester);
        }

        /// <summary>
        /// 取得學生社團參與記錄
        /// </summary>
        public static Dictionary<string, OnlineSCJoin> GetSCJoinStudent(Connection me, string SchoolYear, string Semester)
        {
            Dictionary<string, OnlineSCJoin> dic = new Dictionary<string, OnlineSCJoin>();

            Envelope rsp = me.SendRequest("_.GetStudentSCJoin", new Envelope(GetRequest(SchoolYear, Semester)));
            XElement clubs = XElement.Parse(rsp.Body.XmlString);

            foreach (XElement club in clubs.Elements("Student"))
            {
                OnlineSCJoin ol = new OnlineSCJoin(club);
                if (!dic.ContainsKey(ol.StudentId))
                {
                    dic.Add(ol.StudentId, ol);
                }
            }
            return dic;
        }

        #endregion

        #region 上學期有擔任社團幹部的學生

        /// <summary>
        /// 取得預設(學年度/學期)的前一個(學年度/學期)社團幹部之學生
        /// </summary>
        static public Dictionary<string, OnlinePresident> GetDefPresident(Connection me)
        {
            if (K12.Data.School.DefaultSemester == "2")
            {
                //如果學期為2,則減一學期,並取得第一學期是否擔任過社長
                int school = int.Parse(K12.Data.School.DefaultSchoolYear);
                int semester = int.Parse(K12.Data.School.DefaultSemester) - 1;

                return GetPresident(me, school.ToString(), semester.ToString());
            }
            else
            {
                Dictionary<string, OnlinePresident> dic = new Dictionary<string, OnlinePresident>();
                return dic;
            }
        }

        /// <summary>
        /// 取得參與過社團幹部之學生
        /// </summary>
        static public Dictionary<string, OnlinePresident> GetPresident(Connection me, string SchoolYear, string Semester)
        {
            Dictionary<string, OnlinePresident> dic = new Dictionary<string, OnlinePresident>();
            List<OnlinePresident> list = new List<OnlinePresident>();

            Envelope rsp = me.SendRequest("_.GetStudentPresident", new Envelope(GetRequest(SchoolYear, Semester)));
            XElement clubs = XElement.Parse(rsp.Body.XmlString);

            foreach (XElement President in clubs.Elements("K12.resultscore.universal"))
            {
                OnlinePresident op = new OnlinePresident(President);
                if (!dic.ContainsKey(op.RefStudentId))
                {
                    dic.Add(op.RefStudentId, op);
                }
            }
            return dic;
        }

        #endregion

        #region 社團志願分配

        /// <summary>
        /// 取得學生清單
        /// </summary>
        static public Dictionary<string, OnlineStudent> GetStuentList(Connection me, Dictionary<string, OnlineSCJoin> SCJoinDic, Dictionary<string, OnlinePresident> PresidentDic)
        {
            Envelope rsp = me.SendRequest("_.GetStudentsCanChoose", new Envelope(GetRequest(null, null)));
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

                //是否有前學期社團幹部
                if (PresidentDic.ContainsKey(os.Id))
                {
                    os.LastClubName = PresidentDic[os.Id].ClubName;
                }

            }
            return StudentDic;
        }

        /// <summary>
        /// 取得資料基本式
        /// </summary>
        static public FISCA.DSAClient.XmlHelper GetRequest(string SchoolYear, string Semester)
        {
            FISCA.DSAClient.XmlHelper _xml;
            if (!string.IsNullOrEmpty(SchoolYear) && !string.IsNullOrEmpty(Semester))
            {
                _xml = new XmlHelper("<Reqluest/>");
                _xml.AddElement("Field");
                _xml.AddElement("Field", "All");
                _xml.AddElement("Condition");
                _xml.AddElement("Condition", "SchoolYear", SchoolYear);
                _xml.AddElement("Condition", "Semester", Semester);
            }
            else
            {
                _xml = new XmlHelper("<Reqluest/>");
                _xml.AddElement("Field");
                _xml.AddElement("Field", "All");
                _xml.AddElement("Condition");
            }

            return _xml;
        } 

        #endregion

        #region 點名單專用Service

        /// <summary>
        /// 取得本學年度學期的學生社團參與記錄
        /// </summary>
        public static Dictionary<string, OnlineSCJoin> GetSCJoinByClubName(Connection me, List<string> ClubNameList)
        {
            return GetSCJoinList(me, K12.Data.School.DefaultSchoolYear, K12.Data.School.DefaultSemester, ClubNameList);
        }

        /// <summary>
        /// 取得學生社團參與記錄
        /// </summary>
        public static Dictionary<string, OnlineSCJoin> GetSCJoinList(Connection me, string SchoolYear, string Semester, List<string> ClubNameList)
        {
            Dictionary<string, OnlineSCJoin> dic = new Dictionary<string, OnlineSCJoin>();

            Envelope rsp = me.SendRequest("_.GetStudentSCJoin", new Envelope(GetSCJoinRequest(SchoolYear, Semester, ClubNameList)));
            XElement clubs = XElement.Parse(rsp.Body.XmlString);

            foreach (XElement stud in clubs.Elements("Student"))
            {
                OnlineSCJoin ol = new OnlineSCJoin(stud);
                if (!dic.ContainsKey(ol.StudentId))
                {
                    dic.Add(ol.StudentId, ol);
                }
            }
            return dic;
        }

        /// <summary>
        /// 取得學生學年度學期社團參與記錄
        /// </summary>
        static public FISCA.DSAClient.XmlHelper GetSCJoinRequest(string SchoolYear, string Semester, List<string> ClubNameList)
        {
            FISCA.DSAClient.XmlHelper _xml;
            _xml = new FISCA.DSAClient.XmlHelper("<Reqluest/>");
            _xml.AddElement("Field");
            _xml.AddElement("Field", "All");
            _xml.AddElement("Condition");

            if (!string.IsNullOrEmpty(SchoolYear) && !string.IsNullOrEmpty(Semester))
            {

                _xml.AddElement("Condition", "SchoolYear", SchoolYear);
                _xml.AddElement("Condition", "Semester", Semester);
                foreach (string each in ClubNameList)
                {
                    _xml.AddElement("Condition", "ClubName", each);
                }
            }

            return _xml;
        }

        #endregion
    }
}
