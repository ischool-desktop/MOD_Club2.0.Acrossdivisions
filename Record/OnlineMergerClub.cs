using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MOD_Club_Acrossdivisions
{
    public class OnlineMergerClub
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public OnlineMergerClub(string MasterSchool)
        {
            AllClubList = new List<OnlineClub>();
            //DeptRestrict = new List<string>();
            _MasterSchool = MasterSchool;

            Grade1 = new GradeCount("1");
            Grade2 = new GradeCount("2");
            Grade3 = new GradeCount("3");


        }

        /// <summary>
        /// 社團初始化
        /// </summary>
        public void AddClub(OnlineClub club)
        {
            AllClubList.Add(club);

            //如果是模組掛載學校,則指定為主要社團
            if (club.School == _MasterSchool)
                _MsaterClub = club;

            //如果沒有主要社團,則以其它社團為資料來源
            if (_MsaterClub == null)
                _MsaterClub = club;
        }

        #region Check

        /// <summary>
        /// 學生是否符合本社團資格
        /// </summary>
        public string EligibilityCheck(OnlineStudent oStudent)
        {
            List<string> list = new List<string>();

            string message = "";

            //科別是否符合
            message = CheckDept(oStudent);
            if (!string.IsNullOrEmpty(message))
            {
                list.Add(message);
            }

            //男女是否符合
            message = CheckGender(oStudent);
            if (!string.IsNullOrEmpty(message))
            {
                list.Add(message);
            }

            //人數是否已達上限
            message = CheckLimit(oStudent);
            if (!string.IsNullOrEmpty(message))
            {
                list.Add(message);
            }

            return string.Join("，", list);

        }

        /// <summary>
        /// 檢查人數上限
        /// </summary>
        private string CheckLimit(OnlineStudent oStudent)
        {
            if (tool.CheckValue(Limit))
            {
                if (int.Parse(Limit) <= NowStudentCount)
                {
                    return "總人數已達上限";
                }
            }

            string message = oStudent.GradeYear + "年級人數已達上限";

            if (oStudent.GradeYear == "1" || oStudent.GradeYear == "4" || oStudent.GradeYear == "7" || oStudent.GradeYear == "10")
            {
                if (tool.CheckValue(Grade1Limit))
                {
                    if (int.Parse(Grade1Limit) <= Now_Grade1Limit)
                    {
                        return message;
                    }
                }
            }
            else if (oStudent.GradeYear == "2" || oStudent.GradeYear == "5" || oStudent.GradeYear == "8" || oStudent.GradeYear == "11")
            {
                if (tool.CheckValue(Grade2Limit))
                {
                    if (int.Parse(Grade2Limit) <= Now_Grade2Limit)
                    {
                        return message;
                    }
                }
            }
            else if (oStudent.GradeYear == "3" || oStudent.GradeYear == "6" || oStudent.GradeYear == "9" || oStudent.GradeYear == "12")
            {
                if (tool.CheckValue(Grade3Limit))
                {
                    if (int.Parse(Grade3Limit) <= Now_Grade3Limit)
                    {
                        return message;
                    }
                }
            }

            return "";
        }

        /// <summary>
        /// 檢查男女生
        /// </summary>
        private string CheckGender(OnlineStudent oStudent)
        {
            if (!string.IsNullOrEmpty(GenderRestrict))
            {
                if (oStudent.Gender != GenderRestrict)
                    return "男女性別不符";
            }
            return "";
        }

        /// <summary>
        /// 檢查科別
        /// </summary>
        private string CheckDept(OnlineStudent oStudent)
        {
            if (DeptRestrict.Count > 0) //無科別限制
            {
                if (!DeptRestrict.Contains(oStudent.DeptName)) //不包含在內,即為科別不符
                {
                    return "科別不符";
                }
            }
            return "";
        }

        #endregion


        /// <summary>
        /// 重要屬性 - 本地學校
        /// </summary>
        public string _MasterSchool { get; set; }

        /// <summary>
        /// 重要屬性 - 本地社團
        /// </summary>
        public OnlineClub _MsaterClub { get; set; }

        /// <summary>
        /// 合併資源的社團資料
        /// </summary>
        public List<OnlineClub> AllClubList { get; set; }

        #region 核心屬性

        /// <summary>
        /// 學年度
        /// </summary>
        public string SchoolYear
        {
            get
            {
                return _MsaterClub.SchoolYear;
            }
        }

        /// <summary>
        /// 學期
        /// </summary>
        public string Semester
        {
            get
            {
                return _MsaterClub.Semester;
            }
        }

        /// <summary>
        /// 社團名稱
        /// </summary>
        public string ClubName
        {
            get
            {
                return _MsaterClub.ClubName;
            }
        }

        /// <summary>
        /// 性別限制
        /// </summary>
        public string GenderRestrict
        {
            get
            {
                return _MsaterClub.GenderRestrict;
            }
        }

        /// <summary>
        /// 場地
        /// </summary>
        public string Location
        {
            get
            {
                return _MsaterClub.Location;
            }
        }

        /// <summary>
        /// 代碼
        /// </summary>
        public string ClubNumber
        {
            get
            {
                return _MsaterClub.ClubNumber;
            }
        }

        /// <summary>
        /// 指導老師
        /// </summary>
        public string TeacherName
        {
            get
            {
                return _MsaterClub.TeacherName;
            }
        }

        /// <summary>
        /// 類型
        /// </summary>
        public string ClubCategory
        {
            get
            {
                return _MsaterClub.ClubCategory;
            }
        }

        /// <summary>
        /// 科別限制
        /// </summary>
        public List<string> DeptRestrict
        {
            get
            {
                return _MsaterClub.DeptRestrict;
            }
        }

        #endregion

        #region 人數上限

        /// <summary>
        /// 一年級人數上限
        /// </summary>
        public string Grade1Limit
        {
            get
            {
                return _MsaterClub.Grade1Limit;
            }
        }

        /// <summary>
        /// 二年級人數上限
        /// </summary>
        public string Grade2Limit
        {
            get
            {
                return _MsaterClub.Grade2Limit;
            }
        }

        /// <summary>
        /// 三年級人數上限
        /// </summary>
        public string Grade3Limit
        {
            get
            {
                return _MsaterClub.Grade3Limit;
            }
        }

        /// <summary>
        /// 總人數上限
        /// </summary>
        public string Limit
        {
            get
            {
                return _MsaterClub.Limit;
            }
        }

        #endregion

        #region 目前統計部份


        public GradeCount Grade1 { get; set; }
        public GradeCount Grade2 { get; set; }
        public GradeCount Grade3 { get; set; }


        /// <summary>
        /// 目前一年級人數
        /// </summary>
        public int Now_Grade1Limit
        {
            get
            {
                return Grade1.Now_GradeLimit;
            }
        }

        /// <summary>
        /// 目前二年級人數
        /// </summary>
        public int Now_Grade2Limit
        {
            get
            {
                return Grade2.Now_GradeLimit;
            }
        }

        /// <summary>
        /// 目前三年級人數
        /// </summary>
        public int Now_Grade3Limit
        {
            get
            {
                return Grade3.Now_GradeLimit;
            }
        }

        /// <summary>
        /// 目前人數
        /// </summary>
        public int NowStudentCount { get; set; }

        /// <summary>
        /// 增加人數統計
        /// </summary>
        public void Increase(OnlineStudent student)
        {

            if (student.GradeYear == "1" || student.GradeYear == "4" || student.GradeYear == "7" || student.GradeYear == "10")
            {
                Grade1.SetIncrease(student.School, 1);
            }
            else if (student.GradeYear == "2" || student.GradeYear == "5" || student.GradeYear == "8" || student.GradeYear == "11")
            {
                Grade2.SetIncrease(student.School, 1);
            }
            else if (student.GradeYear == "3" || student.GradeYear == "6" || student.GradeYear == "9" || student.GradeYear == "12")
            {
                Grade3.SetIncrease(student.School, 1);
            }
            NowStudentCount++;
        }

        /// <summary>
        /// 減少人數
        /// </summary>
        public void Decrease(OnlineStudent student)
        {

            if (student.GradeYear == "1" || student.GradeYear == "4" || student.GradeYear == "7" || student.GradeYear == "10")
            {
                Grade1.SetDecrease(student.School, 1);
            }
            else if (student.GradeYear == "2" || student.GradeYear == "5" || student.GradeYear == "8" || student.GradeYear == "11")
            {
                Grade2.SetDecrease(student.School, 1);
            }
            else if (student.GradeYear == "3" || student.GradeYear == "6" || student.GradeYear == "9" || student.GradeYear == "12")
            {
                Grade3.SetDecrease(student.School, 1);
            }
            NowStudentCount--;
        }

        #endregion

    }
}
