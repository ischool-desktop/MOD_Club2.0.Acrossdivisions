﻿namespace MOD_Club_Acrossdivisions.Ribbon
{
    partial class ExportAcrossDataForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.schooYearCbx = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.semesterCbx = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.leaveBtn = new DevComponents.DotNetBar.ButtonX();
            this.printBtn = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            this.labelX1.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(12, 12);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(45, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "學年度";
            // 
            // schooYearCbx
            // 
            this.schooYearCbx.DisplayMember = "Text";
            this.schooYearCbx.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.schooYearCbx.FormattingEnabled = true;
            this.schooYearCbx.ItemHeight = 19;
            this.schooYearCbx.Location = new System.Drawing.Point(63, 10);
            this.schooYearCbx.Name = "schooYearCbx";
            this.schooYearCbx.Size = new System.Drawing.Size(62, 25);
            this.schooYearCbx.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.schooYearCbx.TabIndex = 1;
            // 
            // labelX2
            // 
            this.labelX2.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(145, 12);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(34, 23);
            this.labelX2.TabIndex = 2;
            this.labelX2.Text = "學期";
            // 
            // semesterCbx
            // 
            this.semesterCbx.DisplayMember = "Text";
            this.semesterCbx.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.semesterCbx.FormattingEnabled = true;
            this.semesterCbx.ItemHeight = 19;
            this.semesterCbx.Location = new System.Drawing.Point(185, 10);
            this.semesterCbx.Name = "semesterCbx";
            this.semesterCbx.Size = new System.Drawing.Size(50, 25);
            this.semesterCbx.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.semesterCbx.TabIndex = 3;
            // 
            // labelX3
            // 
            this.labelX3.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = "";
            this.labelX3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX3.Location = new System.Drawing.Point(13, 67);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(242, 50);
            this.labelX3.TabIndex = 4;
            this.labelX3.Text = "*依學年度、學期設定，列印該學期間\r\n學生選社明細以及分發結果。\r\n\r\n\r\n";
            // 
            // leaveBtn
            // 
            this.leaveBtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.leaveBtn.BackColor = System.Drawing.Color.Transparent;
            this.leaveBtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.leaveBtn.Location = new System.Drawing.Point(180, 121);
            this.leaveBtn.Name = "leaveBtn";
            this.leaveBtn.Size = new System.Drawing.Size(75, 23);
            this.leaveBtn.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.leaveBtn.TabIndex = 5;
            this.leaveBtn.Text = "離開";
            this.leaveBtn.Click += new System.EventHandler(this.leaveBtn_Click_1);
            // 
            // printBtn
            // 
            this.printBtn.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.printBtn.BackColor = System.Drawing.Color.Transparent;
            this.printBtn.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.printBtn.Location = new System.Drawing.Point(99, 121);
            this.printBtn.Name = "printBtn";
            this.printBtn.Size = new System.Drawing.Size(75, 23);
            this.printBtn.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.printBtn.TabIndex = 6;
            this.printBtn.Text = "列印";
            this.printBtn.Click += new System.EventHandler(this.printBtn_Click_1);
            // 
            // ExportAcrossDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 156);
            this.Controls.Add(this.printBtn);
            this.Controls.Add(this.leaveBtn);
            this.Controls.Add(this.labelX3);
            this.Controls.Add(this.semesterCbx);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.schooYearCbx);
            this.Controls.Add(this.labelX1);
            this.DoubleBuffered = true;
            this.MaximumSize = new System.Drawing.Size(283, 195);
            this.MinimumSize = new System.Drawing.Size(283, 195);
            this.Name = "ExportAcrossDataForm";
            this.Text = "匯出選社結果(跨部別)";
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx schooYearCbx;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.Controls.ComboBoxEx semesterCbx;
        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.ButtonX leaveBtn;
        private DevComponents.DotNetBar.ButtonX printBtn;
    }
}