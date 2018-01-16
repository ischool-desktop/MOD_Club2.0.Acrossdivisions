using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MOD_Club_Acrossdivisions
{
    public class OnlinePresident
    {

        public OnlinePresident(XElement President)
        {
            if (President.Element("RefStudentId") != null)
                RefStudentId = President.Element("RefStudentId").Value;

            if (President.Element("StudentName") != null)
                StudentName = President.Element("StudentName").Value;

            if (President.Element("ClubName") != null)
                ClubName = President.Element("ClubName").Value;

            if (President.Element("SchoolYear") != null)
                SchoolYear = President.Element("SchoolYear").Value;

            if (President.Element("Semester") != null)
                Semester = President.Element("Semester").Value;
        }


        /// <summary>
        /// 學生系統編號
        /// </summary>
        public string RefStudentId { get; set; }

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
        /// 社團名稱
        /// </summary>
        public string ClubName { get; set; }

        /// <summary>
        /// 擔任社長或副社長
        /// </summary>
        public bool IsPorV { get; set; }
    }
}
