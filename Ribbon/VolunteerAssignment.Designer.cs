namespace MOD_Club_Acrossdivisions
{
    partial class VolunteerAssignment
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnStart = new DevComponents.DotNetBar.ButtonX();
            this.btnExit = new DevComponents.DotNetBar.ButtonX();
            this.dataGridViewX1 = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.colSchool = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTeacher = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStudentCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNowUp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCLUBRecord = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.lbHelpS = new DevComponents.DotNetBar.LabelX();
            this.lbSelectClassCount = new DevComponents.DotNetBar.LabelX();
            this.lbHelpConSchool = new DevComponents.DotNetBar.LabelX();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.BackColor = System.Drawing.Color.Transparent;
            this.btnStart.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnStart.Location = new System.Drawing.Point(723, 576);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 25);
            this.btnStart.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "開始分配";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnExit
            // 
            this.btnExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnExit.Location = new System.Drawing.Point(804, 576);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 25);
            this.btnExit.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "離開";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // dataGridViewX1
            // 
            this.dataGridViewX1.AllowUserToAddRows = false;
            this.dataGridViewX1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewX1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewX1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewX1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSchool,
            this.colYear,
            this.colClass,
            this.colTeacher,
            this.colStudentCount,
            this.colNowUp,
            this.colCLUBRecord,
            this.colLock});
            this.dataGridViewX1.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewX1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewX1.Location = new System.Drawing.Point(13, 72);
            this.dataGridViewX1.Name = "dataGridViewX1";
            this.dataGridViewX1.RowTemplate.Height = 24;
            this.dataGridViewX1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewX1.Size = new System.Drawing.Size(866, 484);
            this.dataGridViewX1.TabIndex = 2;
            this.dataGridViewX1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewX1_CellDoubleClick);
            this.dataGridViewX1.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewX1_ColumnHeaderMouseClick);
            this.dataGridViewX1.SelectionChanged += new System.EventHandler(this.dataGridViewX1_SelectionChanged);
            // 
            // colSchool
            // 
            this.colSchool.DataPropertyName = "School";
            this.colSchool.HeaderText = "學校";
            this.colSchool.Name = "colSchool";
            this.colSchool.ReadOnly = true;
            this.colSchool.Width = 150;
            // 
            // colYear
            // 
            this.colYear.DataPropertyName = "GradeYear";
            this.colYear.HeaderText = "年級";
            this.colYear.Name = "colYear";
            this.colYear.ReadOnly = true;
            this.colYear.Width = 60;
            // 
            // colClass
            // 
            this.colClass.DataPropertyName = "ClassName";
            this.colClass.HeaderText = "班級";
            this.colClass.Name = "colClass";
            this.colClass.ReadOnly = true;
            // 
            // colTeacher
            // 
            this.colTeacher.DataPropertyName = "TeacherName";
            this.colTeacher.HeaderText = "老師";
            this.colTeacher.Name = "colTeacher";
            this.colTeacher.ReadOnly = true;
            this.colTeacher.Width = 80;
            // 
            // colStudentCount
            // 
            this.colStudentCount.DataPropertyName = "StudentCount";
            this.colStudentCount.HeaderText = "學生數";
            this.colStudentCount.Name = "colStudentCount";
            this.colStudentCount.ReadOnly = true;
            this.colStudentCount.Width = 80;
            // 
            // colNowUp
            // 
            this.colNowUp.DataPropertyName = "SelectCount";
            this.colNowUp.HeaderText = "已填志願人數";
            this.colNowUp.Name = "colNowUp";
            this.colNowUp.ReadOnly = true;
            this.colNowUp.Width = 110;
            // 
            // colCLUBRecord
            // 
            this.colCLUBRecord.DataPropertyName = "NumberOfParticipants";
            this.colCLUBRecord.HeaderText = "參與社團人數";
            this.colCLUBRecord.Name = "colCLUBRecord";
            this.colCLUBRecord.ReadOnly = true;
            this.colCLUBRecord.Width = 110;
            // 
            // colLock
            // 
            this.colLock.DataPropertyName = "LockNumber";
            this.colLock.HeaderText = "社團鎖定人數";
            this.colLock.Name = "colLock";
            this.colLock.ReadOnly = true;
            this.colLock.Width = 110;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(159, 26);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(158, 22);
            this.toolStripMenuItem2.Text = "檢視志願序清單";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // lbHelpS
            // 
            this.lbHelpS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbHelpS.AutoSize = true;
            this.lbHelpS.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lbHelpS.BackgroundStyle.Class = "";
            this.lbHelpS.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbHelpS.Location = new System.Drawing.Point(13, 562);
            this.lbHelpS.Name = "lbHelpS";
            this.lbHelpS.Size = new System.Drawing.Size(535, 39);
            this.lbHelpS.TabIndex = 4;
            this.lbHelpS.Text = "說明：(*)滑鼠右鍵可檢視[學生選填明細]與狀況　(*)分配社團志願請[選擇班級]後開始分配\r\n　　　 (*)點擊[年級]欄位標題可進行排序";
            // 
            // lbSelectClassCount
            // 
            this.lbSelectClassCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSelectClassCount.AutoSize = true;
            this.lbSelectClassCount.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lbSelectClassCount.BackgroundStyle.Class = "";
            this.lbSelectClassCount.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbSelectClassCount.Location = new System.Drawing.Point(602, 580);
            this.lbSelectClassCount.Name = "lbSelectClassCount";
            this.lbSelectClassCount.Size = new System.Drawing.Size(82, 21);
            this.lbSelectClassCount.TabIndex = 5;
            this.lbSelectClassCount.Text = "選擇班級：0";
            // 
            // lbHelpConSchool
            // 
            this.lbHelpConSchool.AutoSize = true;
            this.lbHelpConSchool.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.lbHelpConSchool.BackgroundStyle.Class = "";
            this.lbHelpConSchool.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.lbHelpConSchool.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbHelpConSchool.Location = new System.Drawing.Point(13, 9);
            this.lbHelpConSchool.Name = "lbHelpConSchool";
            this.lbHelpConSchool.Size = new System.Drawing.Size(112, 57);
            this.lbHelpConSchool.TabIndex = 6;
            this.lbHelpConSchool.Text = "已連線學校:\r\n學校名稱";
            // 
            // VolunteerAssignment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 604);
            this.Controls.Add(this.lbHelpConSchool);
            this.Controls.Add(this.lbSelectClassCount);
            this.Controls.Add(this.lbHelpS);
            this.Controls.Add(this.dataGridViewX1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnStart);
            this.DoubleBuffered = true;
            this.MaximizeBox = true;
            this.MinimizeBox = true;
            this.Name = "VolunteerAssignment";
            this.Text = "社團志願分配(跨部別)";
            this.Load += new System.EventHandler(this.VolunteerAssignment_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevComponents.DotNetBar.ButtonX btnStart;
        private DevComponents.DotNetBar.ButtonX btnExit;
        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewX1;
        private DevComponents.DotNetBar.LabelX lbHelpS;
        private DevComponents.DotNetBar.LabelX lbSelectClassCount;
        private DevComponents.DotNetBar.LabelX lbHelpConSchool;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSchool;
        private System.Windows.Forms.DataGridViewTextBoxColumn colYear;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTeacher;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStudentCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNowUp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCLUBRecord;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLock;
    }
}