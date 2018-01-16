using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOD_Club_Acrossdivisions
{
    class Def
    {
        /// <summary>
        /// 登入學校(已註解)
        /// </summary>
        public void Join()
        {
            ////條件
            ////<Request>
            ////　<Field>
            ////　　<All></All>
            ////　</Field>
            ////　<Condition>
            ////　　<Uid>74115</Uid>
            ////　</Condition>
            ////</Request>

            ////讀取已連限設定內容
            ////嘗試依據使用者曾經連線內容再次連限
            ////使用帳號密碼直接連線
            ////Connection conection = new Connection();
            ////conection.Connect("dev.sh_d", "ClubAcrossdivisions", "帳號", "密碼");

            ////取得另一個學校的連線
            ////Connection you = FISCA.Authentication.DSAServices.GetConnection("dev.jh_kh", "ClubAcrossdivisions");

            ////取得掛載模組之學校Contract
            //try
            //{

            //    //Connection me = FISCA.Authentication.DSAServices.GetConnection(Asspoint, "ClubAcrossdivisions");
            //    Connection me = new Connection();
            //    me.Connect(tool.Point, "ClubAcrossdivisions", FISCA.Authentication.DSAServices.PassportToken);
            //    me = me.AsContract("ClubAcrossdivisions");

            //    FISCA.DSAClient.XmlHelper _xml = new XmlHelper("<Reqluest/>");
            //    _xml.AddElement("Field");
            //    _xml.AddElement("Field", "All");
            //    _xml.AddElement("Condition");
            //    _xml.AddElement("Condition", "Uid", "74115");

            //    Envelope rsp = me.SendRequest("_.GetClubList", new Envelope(_xml));
            //    MsgBox.Show(XmlHelper.Format(rsp.XmlString));

            //    XElement clubs = XElement.Parse(rsp.Body.XmlString);

            //    foreach (XElement club in clubs.Elements("K12.clubrecord.universal"))
            //    {


            //    }
            //}
            //catch (Exception ex)
            //{


            //}
        }
    }
}
