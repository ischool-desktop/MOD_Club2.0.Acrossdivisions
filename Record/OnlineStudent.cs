using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MOD_Club_Acrossdivisions
{
    public class OnlineStudent
    {
        public OnlineStudent(XElement student)
        {
            if (student.Element("Id") != null)
                Id = student.Element("Id").Value;

            if (student.Element("Name") != null)
                Name = student.Element("Name").Value;

            if (student.Element("ClassName") != null)
                ClassName = student.Element("ClassName").Value;

            if (student.Element("DisplayOrder") != null)
                DisplayOrder = student.Element("DisplayOrder").Value;

            if (student.Element("TeacherName") != null)
                TeacherName = student.Element("TeacherName").Value;

            if (student.Element("DeptName") != null)
            {
                string[] st = student.Element("DeptName").Value.Split(':');
                if (st.Length > 1)
                    DeptName = st.GetValue(0).ToString();
            }

            if (student.Element("StudentNumber") != null)
                StudentNumber = student.Element("StudentNumber").Value;

            if (student.Element("SeatNo") != null)
                SeatNo = student.Element("SeatNo").Value;

            if (student.Element("Gender") != null)
                Gender = student.Element("Gender").Value;

            if (student.Element("GradeYear") != null)
                GradeYear = student.Element("GradeYear").Value;

            VolunteerList = new Dictionary<int, OnlineClub>();

            Random_Number = tool.random.Next(99999);
        }

        /// <summary>
        /// 學校
        /// </summary>
        public string School { get; set; }

        /// <summary>
        /// 學生系統編號
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 班級名稱
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 班級序號
        /// </summary>
        public string DisplayOrder { get; set; }

        /// <summary>
        /// 老師姓名
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// 科別名稱
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 座號
        /// </summary>
        public string SeatNo { get; set; }

        /// <summary>
        /// 學號
        /// </summary>
        public string StudentNumber { get; set; }

        /// <summary>
        /// 年級
        /// </summary>
        public string GradeYear { get; set; }

        /// <summary>
        /// 姓別
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 我的志願序清單
        /// 志願數 / 社團
        /// </summary>
        public Dictionary<int, OnlineClub> VolunteerList { get; set; }

        /// <summary>
        /// 學生是否具高優先權
        /// (可判斷學生上學期的社團名稱)
        /// </summary>
        public bool HighPriority
        {
            get
            {
                return !string.IsNullOrEmpty(LastClubName);
            }
        }

        /// <summary>
        /// 學生上學期的社團名稱
        /// </summary>
        public string LastClubName { get; set; }

        /// <summary>
        /// 亂數
        /// </summary>
        public int Random_Number { get; set; }

        /// <summary>
        /// 被分配成功後
        /// 本方法可由資訊社團中取得本校之社團物件
        /// </summary>
        public void SuccessSetupClub(OnlineMergerClub megr)
        {
            foreach (OnlineClub club in megr.AllClubList)
            {
                if (club.School == School)
                {
                    新參與社團 = club;
                    megr.Increase(this);
                }
            }
        }

        /// <summary>
        /// 是否被成功分配社團
        /// </summary>
        public bool JoinSuccess
        {
            get
            {
                if (新參與社團 != null)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// 分配成功的社團
        /// </summary>
        public OnlineClub 新參與社團 { get; set; }

        /// <summary>
        /// 學生是否原本已經有參與社團
        /// </summary>
        public OnlineSCJoin 原有社團 { get; set; }

        public bool IsLick
        {
            get
            {
                if (原有社團 != null)
                    return 原有社團.IsLock;
                else
                    return false;
            }
        }


        public string ErrorMessage { get; set; }

        /// <summary>
        /// 退出社團
        /// 1.清除目前已選擇的社團
        /// 2.減去社團的人數
        /// </summary>
        public void RetirementCommunity(Dictionary<string, OnlineMergerClub> MergerClub)
        {
            if (IsLick)
                return;

            if (原有社團 == null)
                return;

            if (MergerClub.ContainsKey(原有社團.ClubName))
            {
                OnlineMergerClub merger = MergerClub[原有社團.ClubName];
                merger.Decrease(this);
            }

            新參與社團 = null;
        }
    }
}
