using FISCA.DSAUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MOD_Club_Acrossdivisions
{
    public class AcrossRecord
    {
        //一個包含了學校DomainName
        //和學校端學生/志願/社團...等相關資料的物件

        public AcrossRecord()
        {
            StudentDic = new Dictionary<string, OnlineStudent>();
            ClubDic = new Dictionary<string, OnlineClub>();
            VolunteerList = new List<OnlineVolunteer>();
            infoDic = new Dictionary<string, ClassRowInfo>();
        }

        /// <summary>
        /// 開始進行志願序資料整合
        /// </summary>
        public void 開始進行志願序資料整合()
        {
            //整理志願序資料
            foreach (OnlineVolunteer each in VolunteerList)
            {
                if (StudentDic.ContainsKey(each.RefStudentId))
                {
                    //學生
                    OnlineStudent OnlineStud = StudentDic[each.RefStudentId];

                    if (!string.IsNullOrEmpty(each.Content))
                    {
                        XmlElement xml = DSXmlHelper.LoadXml(each.Content);
                        int ClubNumber = 1;
                        foreach (XmlElement node in xml.SelectNodes("Club"))
                        {
                            if (node.GetAttribute("Index") == ClubNumber.ToString())
                            {
                                string clubID = node.GetAttribute("Ref_Club_ID");
                                if (ClubDic.ContainsKey(clubID))
                                {
                                    //社團
                                    OnlineClub OnlineClub = ClubDic[clubID];
                                    if (!OnlineStud.VolunteerList.ContainsKey(ClubNumber))
                                    {
                                        OnlineStud.VolunteerList.Add(ClubNumber, OnlineClub);
                                    }
                                }
                            }
                            ClubNumber++;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 開始進行班級分類
        /// </summary>
        public void 班級分類(Dictionary<string, string> dic)
        {
            foreach (OnlineStudent each in StudentDic.Values)
            {
                if (infoDic.ContainsKey(each.ClassName))
                {
                    infoDic[each.ClassName].StudentList.Add(each);

                    if (each.VolunteerList.Count > 0)
                    {
                        infoDic[each.ClassName].SelectCount++; //已選填人數
                    }

                    if (each.原有社團 != null)
                    {
                        infoDic[each.ClassName].NumberOfParticipants++; //ClubJoin不為null,社團參與人數+1

                        if (each.原有社團.IsLock)
                        {
                            infoDic[each.ClassName].LockNumber++; //社團鎖定人數
                        }
                    }
                }
                else
                {
                    ClassRowInfo info = new ClassRowInfo();

                    info.School = GetSchoolName(dic, each.School);
                    info.ClassName = each.ClassName; //班級名稱
                    info.GradeYear = each.GradeYear; //年級
                    info.DisplayOrder = each.DisplayOrder; //班級排序
                    info.TeacherName = each.TeacherName; //老師
                    info.StudentList.Add(each);

                    if (each.VolunteerList.Count > 0)
                    {
                        info.SelectCount++; //已選填人數
                    }

                    if (each.原有社團 != null)
                    {
                        info.NumberOfParticipants++; //ClubJoin不為null,社團參與人數+1

                        if (each.原有社團.IsLock)
                        {
                            info.LockNumber++; //社團鎖定人數
                        }
                    }

                    infoDic.Add(each.ClassName, info);

                }
            }
        }

        private string GetSchoolName(Dictionary<string, string> dic, string p)
        {
            if (dic.ContainsKey(p))
            {
                if (!string.IsNullOrEmpty(dic[p]))
                {
                    return p + "(" + dic[p] + ")"; //學校
                }
                else
                    return p; //學校
            }
            else
                return p; //學校
        }

        /// <summary>
        /// 學校DoMainName
        /// </summary>
        public string School { get; set; }

        /// <summary>
        /// 學校備註
        /// </summary>
        public string SchoolRemake { get; set; }

        /// <summary>
        /// 學校所有學生資料(字典)
        /// </summary>
        public Dictionary<string, OnlineStudent> StudentDic { get; set; }

        /// <summary>
        /// 學校所有社團資料
        /// </summary>
        public Dictionary<string, OnlineClub> ClubDic { get; set; }

        /// <summary>
        /// 學校所有志願序清單
        /// </summary>
        public List<OnlineVolunteer> VolunteerList { get; set; }

        public Dictionary<string, ClassRowInfo> infoDic { get; set; }

        public void 設定目前社團人數(Dictionary<string, OnlineMergerClub> MergerClubDic)
        {
            //依據學生身上被鎖定的志願,來count人數

            foreach (OnlineStudent stud in StudentDic.Values)
            {
                //1.是否覆蓋,影響目前學生數的計算
                //2.是否鎖定,也影響學生數的計算

                if (stud.原有社團 != null)
                {
                    if (MergerClubDic.ContainsKey(stud.原有社團.ClubName))
                    {
                        MergerClubDic[stud.原有社團.ClubName].Increase(stud);

                        //if (stud.GradeYear == "1")
                        //{
                        //    MergerClubDic[stud.原有社團.ClubName].Now_Grade1Limit++;
                        //}
                        //else if (stud.GradeYear == "2")
                        //{
                        //    MergerClubDic[stud.原有社團.ClubName].Now_Grade2Limit++;
                        //}
                        //else if (stud.GradeYear == "3")
                        //{
                        //    MergerClubDic[stud.原有社團.ClubName].Now_Grade3Limit++;
                        //}
                        //MergerClubDic[stud.原有社團.ClubName].NowStudentCount++;
                    }
                }

            }
        }
    }
}
