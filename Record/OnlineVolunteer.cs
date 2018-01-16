using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MOD_Club_Acrossdivisions
{
    public class OnlineVolunteer
    {
        /// <summary>
        /// 建構學生"即時"志願序資料
        /// </summary>
        public OnlineVolunteer(XElement volunteer)
        {

            if (volunteer.Element("Uid") != null)
                Uid = volunteer.Element("Uid").Value;

            if (volunteer.Element("RefStudentId") != null)
                RefStudentId = volunteer.Element("RefStudentId").Value;

            if (volunteer.Element("SchoolYear") != null)
                SchoolYear = volunteer.Element("SchoolYear").Value;

            if (volunteer.Element("Semester") != null)
                Semester = volunteer.Element("Semester").Value;

            if (volunteer.Element("Content") != null)
                Content = volunteer.Element("Content").Value;

        }

        //取得志願序狀態(學校,年級,科別,班級,座號,學號,姓名,志願序內容)

        /// <summary>
        /// 連結學校
        /// </summary>
        public string School { get; set; }

        /// <summary>
        /// 志願序系統編號
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        /// 學生系統編號
        /// </summary>
        public string RefStudentId { get; set; }

        /// <summary>
        /// 學年度
        /// </summary>
        public string SchoolYear { get; set; }

        /// <summary>
        /// 學期
        /// </summary>
        public string Semester { get; set; }

        /// <summary>
        /// 志願序內容
        /// </summary>
        public string Content { get; set; }



    }
}
