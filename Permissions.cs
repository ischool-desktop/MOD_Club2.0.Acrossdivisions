using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOD_Club_Acrossdivisions
{
    public static class Permissions
    {
        public static string 連線 { get { return "MOD_Club_Acrossdivisions.Connection.cs"; } }
        public static bool 連線權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[連線].Executable;
            }
        }

        public static string 社團志願分配 { get { return "MOD_Club_Acrossdivisions.CommunityVolunteerAssignment.cs"; } }
        public static bool 社團志願分配權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[社團志願分配].Executable;
            }
        }

        public static string 社團點名單 { get { return "MOD_Club_Acrossdivisions.ClubPointList.cs"; } }
        public static bool 社團點名單權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[社團點名單].Executable;
            }
        }

        public static string 社團概況表 { get { return "MOD_Club_Acrossdivisions.SocietiesOverviewTable.cs"; } }
        public static bool 社團概況表權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[社團概況表].Executable;
            }
        }

        public static string 匯出選社結果 { get { return "MOD_Club_Acrossdivisions.ExportAcrossClubStudent"; } }
        public static bool 匯出選社結果權限
        {
            get
            {
                return FISCA.Permission.UserAcl.Current[匯出選社結果].Executable;
            }
        }

    }
}
