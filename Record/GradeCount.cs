using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOD_Club_Acrossdivisions
{
    public class GradeCount
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public GradeCount(string NowGrade)
        {
            _NowGrade = NowGrade; //年級
            SchoolGradeDic = new Dictionary<string, int>();
        }

        /// <summary>
        /// 人數目前為?
        /// </summary>
        public int Now_GradeLimit
        {
            get
            {
                int count = 0;
                foreach (string each in SchoolGradeDic.Keys)
                {
                    count += SchoolGradeDic[each];
                }
                return count;
            }
        }

        /// <summary>
        /// 目前是幾年級
        /// </summary>
        public string _NowGrade { get; set; }

        /// <summary>
        /// 各部別人數
        /// </summary>
        public Dictionary<string, int> SchoolGradeDic { get; set; }

        public void SetIncrease(string NowGrade, int count)
        {
            if (!SchoolGradeDic.ContainsKey(NowGrade))
            {
                SchoolGradeDic.Add(NowGrade, 0);
            }
            SchoolGradeDic[NowGrade] += count;
        }

        public void SetDecrease(string NowGrade, int count)
        {
            if (!SchoolGradeDic.ContainsKey(NowGrade))
            {
                SchoolGradeDic.Add(NowGrade, 0);
            }
            SchoolGradeDic[NowGrade] -= count;
        }

    }
}
