using FISCA.Presentation.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MOD_Club_Acrossdivisions
{
    public partial class VolunteerLOG : BaseForm
    {
        public VolunteerLOG(List<LogAssignRecord> LogDic)
        {
            InitializeComponent();

            foreach (LogAssignRecord each in LogDic)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridViewX1);
                row.Cells[0].Value = each.部別;
                row.Cells[1].Value = each.班級;
                row.Cells[2].Value = each.座號;
                row.Cells[3].Value = each.姓名;
                row.Cells[4].Value = each.志願;
                row.Cells[5].Value = each.社團名稱;
                dataGridViewX1.Rows.Add(row);
            }

        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = "志願分配結果";
            saveFileDialog1.Filter = "Excel (*.xls)|*.xls";
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;

            DataGridViewExport export = new DataGridViewExport(dataGridViewX1);
            export.Save(saveFileDialog1.FileName);

            if (new CompleteForm().ShowDialog() == DialogResult.Yes)
                System.Diagnostics.Process.Start(saveFileDialog1.FileName);
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
