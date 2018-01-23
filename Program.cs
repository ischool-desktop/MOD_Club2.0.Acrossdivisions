using FISCA;
using FISCA.Permission;
using FISCA.Presentation;
using K12.Data.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Words;
using System.IO;
using System.Windows.Forms;

namespace MOD_Club_Acrossdivisions
{
    public class Program
    {
        [MainMethod()]
        static public void Main()
        {

            ServerModule.AutoManaged("http://module.ischool.com.tw/module/138/Club_Acrossdivisions/udm.xml");

            #region 處理UDT Table沒有的問題

            ConfigData cd = K12.Data.School.Configuration["跨部別社團UDT載入設定"];
            bool checkClubUDT = false;

            string name = "社團UDT是否已載入_20140709";
            //如果尚無設定值,預設為
            if (string.IsNullOrEmpty(cd[name]))
            {
                cd[name] = "false";
            }

            //檢查是否為布林
            bool.TryParse(cd[name], out checkClubUDT);

            if (!checkClubUDT)
            {
                tool._A.Select<EnglishTable>("UID = '00000'");
                tool._A.Select<LoginSchool>("UID = '00000'");

                cd[name] = "true";
                cd.Save();
            }

            #endregion

            RibbonBarItem InClass = FISCA.Presentation.MotherForm.RibbonBarItems["社團", "跨部別"];
            InClass["志願分配部別設定(跨部別)"].Enable = Permissions.連線權限;
            InClass["志願分配部別設定(跨部別)"].Image = Properties.Resources.asymmetric_network_64;
            InClass["志願分配部別設定(跨部別)"].Click += delegate
            {
                NowConnection con = new NowConnection();
                con.ShowDialog();
            };

            InClass["社團志願分配(跨部別)"].Image = Properties.Resources.flip_horizontal_64;
            InClass["社團志願分配(跨部別)"].Enable = Permissions.社團志願分配權限;
            InClass["社團志願分配(跨部別)"].Click += delegate
            {
                VolunteerAssignment cva = new VolunteerAssignment();
                cva.ShowDialog();
            };
            RibbonBarItem InClass2 = FISCA.Presentation.MotherForm.RibbonBarItems["社團", "資料統計"];
            InClass2["報表"].Image = Properties.Resources.Report;
            InClass2["報表"].Size = RibbonBarButton.MenuButtonSize.Large;
            InClass2["報表"]["社團點名單(跨部別)"].Enable = Permissions.社團點名單權限;
            InClass2["報表"]["社團點名單(跨部別)"].Click += delegate
            {
                ClubPointForm cpl = new ClubPointForm();
                cpl.ShowDialog();
            };

            InClass2["報表"]["社團概況表(跨部別)"].Enable = Permissions.社團概況表權限;
            InClass2["報表"]["社團概況表(跨部別)"].Click += delegate
            {
                SocietiesOverviewTable sot = new SocietiesOverviewTable();
                sot.ShowDialog();
            };

            InClass2["報表"]["匯出選社結果(跨部別)"].Enable = Permissions.匯出選社結果權限;
            InClass2["報表"]["匯出選社結果(跨部別)"].Click += delegate
            {
                Ribbon.ExportAcrossDataForm ex = new Ribbon.ExportAcrossDataForm();
                ex.ShowDialog();
            };

            //是否能夠只用單一代碼,決定此模組之使用
            Catalog detail1;
            detail1 = RoleAclSource.Instance["社團"]["功能項目"];
            detail1.Add(new RibbonFeature(Permissions.連線, "連線_跨部別"));
            detail1.Add(new RibbonFeature(Permissions.社團志願分配, "社團志願分配_跨部別"));
            

            detail1 = RoleAclSource.Instance["社團"]["報表"];
            detail1.Add(new RibbonFeature(Permissions.社團點名單, "社團點名單_跨部別"));
            detail1.Add(new RibbonFeature(Permissions.社團概況表, "社團概況表_跨部別"));
            detail1.Add(new RibbonFeature(Permissions.匯出選社結果, "匯出選社結果_跨部別"));
        }
    }
}
