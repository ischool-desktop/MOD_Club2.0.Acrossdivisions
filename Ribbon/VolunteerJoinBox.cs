using FISCA.DSAClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOD_Club_Acrossdivisions
{
    class VolunteerJoinBox
    {
        /// <summary>
        /// 社團分配盒
        /// </summary>
        public VolunteerJoinBox()
        {
            Step1Student = new List<OnlineStudent>();
            Step2Student = new List<OnlineStudent>();
        }

        public List<OnlineStudent> Step1Student { get; set; }
        public List<OnlineStudent> Step2Student { get; set; }

        public List<OnlineStudent> GetList
        {
            get
            {
                List<OnlineStudent> list = new List<OnlineStudent>();
                list.AddRange(Step1Student);
                list.AddRange(Step2Student);
                return list;
            }
        }

        /// <summary>
        /// 取得新增的社團學生參與記錄
        /// </summary>
        public Dictionary<string, XmlHelper> GetNewStudSCJoinList()
        {
            Dictionary<string, XmlHelper> dic = new Dictionary<string, XmlHelper>();

            foreach (OnlineStudent stud in GetList)
            {
                if (stud.JoinSuccess)
                {
                    if (!dic.ContainsKey(stud.School))
                    {
                        FISCA.DSAClient.XmlHelper _helper = new FISCA.DSAClient.XmlHelper("<Request/>");
                        dic.Add(stud.School, _helper);
                    }

                    //建立社團物件
                    dic[stud.School].AddElement("K12.scjoin.universal");
                    dic[stud.School].AddElement("K12.scjoin.universal", "Field");
                    dic[stud.School].AddElement("K12.scjoin.universal/Field", "Lock", "f");
                    dic[stud.School].AddElement("K12.scjoin.universal/Field", "RefClubId", stud.新參與社團.UID);
                    dic[stud.School].AddElement("K12.scjoin.universal/Field", "RefStudentId", stud.Id);
                }
            }

            return dic;
        }

        /// <summary>
        /// 取得要刪除的社團參與記錄
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, XmlHelper> GetDeleteStudSCJoinList()
        {
            Dictionary<string, XmlHelper> dic = new Dictionary<string, XmlHelper>();

            foreach (OnlineStudent stud in GetList)
            {
                if (stud.原有社團 != null)
                {
                    if (!dic.ContainsKey(stud.School))
                    {
                        FISCA.DSAClient.XmlHelper _helper = new XmlHelper("<Request/>");
                        dic.Add(stud.School, _helper);
                    }

                    //建立社團物件
                    dic[stud.School].AddElement("K12.scjoin.universal");
                    dic[stud.School].AddElement("K12.scjoin.universal", "Condition");
                    dic[stud.School].AddElement("K12.scjoin.universal/Condition", "Uid", stud.原有社團.Uid);

                }
            }

            return dic;
        }
    }
}
