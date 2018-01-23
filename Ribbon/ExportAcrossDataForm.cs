using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using FISCA.UDT;
using K12.Club.Volunteer;

namespace MOD_Club_Acrossdivisions.Ribbon
{
    public partial class ExportAcrossDataForm : BaseForm
    {
        public ExportAcrossDataForm()
        {
            InitializeComponent();

            // Init Combobox
            AccessHelper access = new AccessHelper();
            List<K12.Club.Volunteer.UDT.OpenSchoolYearSemester> sysList = access.Select<K12.Club.Volunteer.UDT.OpenSchoolYearSemester>();

            // SchoolYear
            schooYearCbx.Text = sysList[0].SchoolYear;
            for (int i = 0; i < 3; i++)
            {
                schooYearCbx.Items.Add(int.Parse(sysList[0].SchoolYear) - i);
            }
            // Semester
            semesterCbx.Text = sysList[0].Semester;
            semesterCbx.Items.Add(1);
            semesterCbx.Items.Add(2);
        }

        private void printBtn_Click_1(object sender, EventArgs e)
        {
            ExportAcrossClubStudent a = new ExportAcrossClubStudent(schooYearCbx.Text, semesterCbx.Text);
            this.Close();
        }

        private void leaveBtn_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
