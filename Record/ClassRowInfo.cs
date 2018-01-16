using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOD_Club_Acrossdivisions
{
    public class ClassRowInfo
    {
        public ClassRowInfo()
        {
            StudentList = new List<OnlineStudent>();
        }

        /// <summary>
        /// 學校
        /// </summary>
        public string School { get; set; }

        /// <summary>
        /// 年級
        /// </summary>
        public string GradeYear { get; set; }

        /// <summary>
        /// 班級
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 班級序號
        /// </summary>
        public string DisplayOrder { get; set; }

        /// <summary>
        /// 老師
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// 已選填人數
        /// </summary>
        public int SelectCount { get; set; }

        /// <summary>
        /// 社團參與人數
        /// </summary>
        public int NumberOfParticipants { get; set; }

        /// <summary>
        /// 社團鎖定人數
        /// </summary>
        public int LockNumber { get; set; }

        /// <summary>
        /// 社團參與人數
        /// </summary>
        public int StudentCount
        {
            get
            {
                return StudentList.Count;
            }
        }
        /// <summary>
        /// 學生清單
        /// </summary>
        public List<OnlineStudent> StudentList { get; set; }
    }
}
