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
    public partial class EnglishTableForm : BaseForm
    {
        public EnglishTableForm()
        {
            InitializeComponent();
        }

        private void EnglishTableForm_Load(object sender, EventArgs e)
        {

            List<EnglishTable> EngList = tool._A.Select<EnglishTable>();
            foreach (EnglishTable each in EngList)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridViewX1);
                row.Cells[0].Value = each.ClubName;
                row.Cells[1].Value = each.English;
                dataGridViewX1.Rows.Add(row);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            tool._A.DeletedValues(tool._A.Select<EnglishTable>());

            List<EnglishTable> list = new List<EnglishTable>();

            foreach (DataGridViewRow row in dataGridViewX1.Rows)
            {
                if (row.IsNewRow)
                    continue;

                EnglishTable et = new EnglishTable();
                et.ClubName = "" + row.Cells[0].Value;
                et.English = "" + row.Cells[1].Value;
                list.Add(et);
            }

            tool._A.InsertValues(list);

            MsgBox.Show("儲存完成!!");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = "社團中英文對照表";
            saveFileDialog1.Filter = "Excel (*.xls)|*.xls";
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;

            DataGridViewExport export = new DataGridViewExport(dataGridViewX1);
            export.Save(saveFileDialog1.FileName);

            if (new CompleteForm().ShowDialog() == DialogResult.Yes)
                System.Diagnostics.Process.Start(saveFileDialog1.FileName);
        }
    }
}
