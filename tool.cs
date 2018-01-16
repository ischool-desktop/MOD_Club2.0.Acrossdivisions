using FISCA.DSAClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOD_Club_Acrossdivisions
{
    static public class tool
    {
        /// <summary>
        /// 取得目前本機的AccessPoint
        /// </summary>
        static public string Point = FISCA.Authentication.DSAServices.AccessPoint;

        /// <summary>
        /// 使用者記錄檔
        /// </summary>
        static public string ConfigCode = "MOD_Club_Acrossdivisions.NowConnection.cs";

        /// <summary>
        /// 社團相關ConTract名稱
        /// </summary>
        static public string _contract = "ClubAcrossdivisions";

        static public Random random = new Random();

        /// <summary>
        /// 使用者記錄檔
        /// </summary>
        static public string ConfigName = "連線學校";

        static public int 學生選填志願數 { get; set; }
        static public bool 已有社團記錄時 { get; set; } //True為覆蓋

        static public FISCA.UDT.AccessHelper _A = new FISCA.UDT.AccessHelper();

        static public FISCA.Data.QueryHelper _Q = new FISCA.Data.QueryHelper();

        /// <summary>
        /// 判斷是不是數字
        /// </summary>
        static public bool CheckValue(string x)
        {
            if (!string.IsNullOrEmpty(x))
            {
                int y;
                if (int.TryParse(x, out y))
                {
                    return true;
                }
            }
            //是空值,不是數字
            return false;
        }

        /// <summary>
        /// 取得連線學校,檢查是否可以連線
        /// </summary>
        static public bool UserConnection()
        {
            //取得連線學校清單
            List<LoginSchool> LoginSchoolList = tool._A.Select<LoginSchool>();

            if (LoginSchoolList.Count() < 1)
            {
                return false;
            }
            else
            {
                foreach (LoginSchool each in LoginSchoolList)
                {
                    //檢查
                    Exception ex = tool.TestConnection(each.School_Name);
                    if (ex != null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 傳入學校位置
        /// 檢查是否有登入權限
        /// </summary>
        static public Exception TestConnection(string AccessPoint)
        {
            try
            {
                Connection me = new Connection();
                me.Connect(AccessPoint, _contract, FISCA.Authentication.DSAServices.PassportToken);
            }
            catch (Exception ex)
            {
                return ex;
            }
            return null;
        }

        /// <summary>
        /// 測試這個學校是否可以連限
        /// 如果可以回傳 "空字串"
        /// 如果失敗回傳 "錯誤訊息"
        /// (本功能會將錯誤訊息回傳至 johnny5)
        /// </summary>
        static public string CheckAccount(string AccessPoint)
        {
            Exception ex = TestConnection(AccessPoint);
            string message = "";
            if (ex != null)
            {
                SmartSchool.ErrorReporting.ReportingService.ReportException(ex);

                if (ex.Message == "Contract Not found:ClubAcrossdivisions")
                {
                    message = "失敗：Service不存在";
                }
                else if (ex.Message.Contains("User doesn't exist"))
                {
                    message = "失敗：帳號不存在";
                }
                else if (ex.Message.Contains("解析名稱失敗"))
                {
                    message = "失敗：解析名稱失敗";
                }
                else if (ex.Message == "並未將物件參考設定為物件的執行個體。")
                {
                    message = "失敗：登入帳號不是ischool Account";
                }
                else
                {
                    message = "失敗：其它「" + ex.Message + "」";
                }
            }
            else
                message = "成功";

            return message;
        }

        /// <summary>
        /// 取得各校之相關基本資料(社團/學生/志願序)
        /// </summary>
        static public Dictionary<string, AcrossRecord> SchoolClubDetail(List<LoginSchool> LoginSchoolList)
        {
            Dictionary<string, AcrossRecord> AcrossDic = new Dictionary<string, AcrossRecord>();

            if (FISCA.Authentication.DSAServices.PassportToken != null)
            {
                foreach (LoginSchool login in LoginSchoolList)
                {
                    Connection me = new Connection();
                    me.Connect(login.School_Name, _contract, FISCA.Authentication.DSAServices.PassportToken);
                    me = me.AsContract(_contract);

                    //取得本學年度/學期
                    //目前系統中有參與記錄的學生 & 是否鎖定
                    Dictionary<string, OnlineSCJoin> StudentSCJoinDic = RunService.GetDefSCJoinStudent(me);

                    //取得指定學年度學期的社長副社長清單
                    Dictionary<string, OnlinePresident> StudentPresidentDic = RunService.GetDefPresident(me);

                    AcrossRecord ar = new AcrossRecord();
                    ar.School = login.School_Name;
                    ar.SchoolRemake = login.Remark;

                    //取得可選社之學生
                    //年級/班級/座號/姓名/科別
                    //StudentSCJoinDic - 是否有社團參與記錄
                    //StudentPresidentDic - 是否有'前期'社長副/社長記錄
                    ar.StudentDic = RunService.GetStuentList(me, StudentSCJoinDic, StudentPresidentDic);

                    //取得預設(學年度,學期)學校之社團記錄
                    ar.ClubDic = RunService.GetDefClubList(me);

                    //取得學生之選填志願內容
                    ar.VolunteerList = RunService.GetDefVolunteer(me, login);

                    if (!AcrossDic.ContainsKey(login.School_Name))
                    {
                        AcrossDic.Add(login.School_Name, ar);
                    }
                }
            }

            return AcrossDic;
        }

        //進行社團資源整合
        static public Dictionary<string, OnlineMergerClub> ResourceMerger(Dictionary<string, AcrossRecord> SchoolClubDic)
        {
            Dictionary<string, OnlineMergerClub> Mergerdic = new Dictionary<string, OnlineMergerClub>();

            foreach (AcrossRecord Across in SchoolClubDic.Values)
            {
                foreach (OnlineClub club in Across.ClubDic.Values)
                {
                    if (!Mergerdic.ContainsKey(club.ClubName))
                    {
                        OnlineMergerClub Mclub = new OnlineMergerClub(tool.Point);
                        Mergerdic.Add(club.ClubName, Mclub);
                    }

                    Mergerdic[club.ClubName].AddClub(club);
                }

            }

            return Mergerdic;
        }

        /// <summary>
        /// 依編號取代為星期
        /// </summary>
        public static string CheckWeek(string x)
        {
            if (x == "Monday")
            {
                return "一";
            }
            else if (x == "Tuesday")
            {
                return "二";
            }
            else if (x == "Wednesday")
            {
                return "三";
            }
            else if (x == "Thursday")
            {
                return "四";
            }
            else if (x == "Friday")
            {
                return "五";
            }
            else if (x == "Saturday")
            {
                return "六";
            }
            else
            {
                return "日";
            }
        }

    }
}
