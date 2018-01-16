using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MOD_Club_Acrossdivisions
{
    public class OnlineSCJoin
    {
        public OnlineSCJoin(XElement club)
        {
            if (club.Element("Uid") != null)
                Uid = club.Element("Uid").Value;

            if (club.Element("Id") != null)
                StudentId = club.Element("Id").Value;

            if (club.Element("Name") != null)
                StudentName = club.Element("Name").Value;

            if (club.Element("ClubName") != null)
                ClubName = club.Element("ClubName").Value;

            if (club.Element("SchoolYear") != null)
                SchoolYear = club.Element("SchoolYear").Value;

            if (club.Element("Semester") != null)
                Semester = club.Element("Semester").Value;

            if (club.Element("ClubNumber") != null)
                ClubNumber = club.Element("ClubNumber").Value;

            if (club.Element("TeacherName") != null)
                TeacherName = club.Element("TeacherName").Value;

            if (club.Element("ClassName") != null)
                ClassName = club.Element("ClassName").Value;

            if (club.Element("SeatNo") != null)
                SeatNo = club.Element("SeatNo").Value;

            if (club.Element("Gender") != null)
                Gender = club.Element("Gender").Value;

            if (club.Element("StudentNumber") != null)
                StudentNumber = club.Element("StudentNumber").Value;

            if (club.Element("Lock") != null)
                if (club.Element("Lock").Value == "t")
                {
                    IsLock = true;
                }
                else
                {
                    IsLock = false;
                }
        }

        /// <summary>
        /// 社團參與記錄系統編號
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        /// 社團名稱
        /// </summary>
        public string ClubName { get; set; }

        /// <summary>
        /// 社團代碼
        /// </summary>
        public string ClubNumber { get; set; }

        /// <summary>
        /// 老師姓名
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// 班級名稱
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 座號
        /// </summary>
        public string SeatNo { get; set; }

        /// <summary>
        /// 姓別
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 學號
        /// </summary>
        public string StudentNumber { get; set; }

        /// <summary>
        /// 學生系統編號
        /// </summary>
        public string StudentId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 學年度
        /// </summary>
        public string SchoolYear { get; set; }

        /// <summary>
        /// 學期
        /// </summary>
        public string Semester { get; set; }

        /// <summary>
        /// 是否鎖定
        /// </summary>
        public bool IsLock { get; set; }
    }
}
