namespace Jabil_UserControl_ADLINK
{
    partial class UserControl_PointsTeach_
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView_Points = new System.Windows.Forms.DataGridView();
            this.Column_Item = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label4 = new System.Windows.Forms.Label();
            this.label_TableName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_Teach = new System.Windows.Forms.Button();
            this.btn_Save = new System.Windows.Forms.Button();
            this.btn_Recovery = new System.Windows.Forms.Button();
            this.comboBox_Points = new System.Windows.Forms.ComboBox();
            this.timer_CoordinateDisplay = new System.Windows.Forms.Timer(this.components);
            this.btn_JumpHome = new System.Windows.Forms.Button();
            this.btn_EmgStop = new System.Windows.Forms.Button();
            this.btn_TableGoHome = new System.Windows.Forms.Button();
            this.btn_Jump = new System.Windows.Forms.Button();
            this.comboBox_Motor_Speed = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Points)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(1, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "平台名稱:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(214, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "點位";
            // 
            // dataGridView_Points
            // 
            this.dataGridView_Points.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Points.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_Item,
            this.Column_Name});
            this.dataGridView_Points.Location = new System.Drawing.Point(218, 64);
            this.dataGridView_Points.Name = "dataGridView_Points";
            this.dataGridView_Points.RowTemplate.Height = 24;
            this.dataGridView_Points.Size = new System.Drawing.Size(397, 258);
            this.dataGridView_Points.TabIndex = 8;
            // 
            // Column_Item
            // 
            this.Column_Item.HeaderText = "Item";
            this.Column_Item.Name = "Column_Item";
            this.Column_Item.Width = 40;
            // 
            // Column_Name
            // 
            this.Column_Name.HeaderText = "Name";
            this.Column_Name.Name = "Column_Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(1, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 19);
            this.label4.TabIndex = 9;
            this.label4.Text = "目前位置";
            // 
            // label_TableName
            // 
            this.label_TableName.AutoSize = true;
            this.label_TableName.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_TableName.Location = new System.Drawing.Point(96, 47);
            this.label_TableName.Name = "label_TableName";
            this.label_TableName.Size = new System.Drawing.Size(19, 16);
            this.label_TableName.TabIndex = 2;
            this.label_TableName.Text = "O";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label3.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(376, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(229, 19);
            this.label3.TabIndex = 7;
            this.label3.Text = "教點前請先重新設定原點";
            // 
            // btn_Teach
            // 
            this.btn_Teach.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Teach.Location = new System.Drawing.Point(124, 328);
            this.btn_Teach.Name = "btn_Teach";
            this.btn_Teach.Size = new System.Drawing.Size(74, 40);
            this.btn_Teach.TabIndex = 10;
            this.btn_Teach.Text = "教點";
            this.btn_Teach.UseVisualStyleBackColor = true;
            this.btn_Teach.Click += new System.EventHandler(this.btn_Teach_Click);
            // 
            // btn_Save
            // 
            this.btn_Save.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Save.Location = new System.Drawing.Point(508, 328);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(83, 30);
            this.btn_Save.TabIndex = 11;
            this.btn_Save.Text = "儲存";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // btn_Recovery
            // 
            this.btn_Recovery.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Recovery.Location = new System.Drawing.Point(404, 328);
            this.btn_Recovery.Name = "btn_Recovery";
            this.btn_Recovery.Size = new System.Drawing.Size(83, 30);
            this.btn_Recovery.TabIndex = 13;
            this.btn_Recovery.Text = "恢復";
            this.btn_Recovery.UseVisualStyleBackColor = true;
            this.btn_Recovery.Click += new System.EventHandler(this.btn_Recovery_Click);
            // 
            // comboBox_Points
            // 
            this.comboBox_Points.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.comboBox_Points.FormattingEnabled = true;
            this.comboBox_Points.Location = new System.Drawing.Point(14, 295);
            this.comboBox_Points.Name = "comboBox_Points";
            this.comboBox_Points.Size = new System.Drawing.Size(184, 27);
            this.comboBox_Points.TabIndex = 14;
            // 
            // timer_CoordinateDisplay
            // 
            this.timer_CoordinateDisplay.Tick += new System.EventHandler(this.timer_CoordinateDisplay_Tick);
            // 
            // btn_JumpHome
            // 
            this.btn_JumpHome.Location = new System.Drawing.Point(621, 219);
            this.btn_JumpHome.Name = "btn_JumpHome";
            this.btn_JumpHome.Size = new System.Drawing.Size(88, 40);
            this.btn_JumpHome.TabIndex = 57;
            this.btn_JumpHome.Text = "回原點";
            this.btn_JumpHome.UseVisualStyleBackColor = true;
            this.btn_JumpHome.Visible = false;
            this.btn_JumpHome.Click += new System.EventHandler(this.btn_JumpHome_Click);
            // 
            // btn_EmgStop
            // 
            this.btn_EmgStop.BackColor = System.Drawing.Color.Red;
            this.btn_EmgStop.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_EmgStop.Location = new System.Drawing.Point(621, 265);
            this.btn_EmgStop.Name = "btn_EmgStop";
            this.btn_EmgStop.Size = new System.Drawing.Size(88, 85);
            this.btn_EmgStop.TabIndex = 56;
            this.btn_EmgStop.Text = "STOP";
            this.btn_EmgStop.UseVisualStyleBackColor = false;
            this.btn_EmgStop.Click += new System.EventHandler(this.btn_EmgStop_Click);
            // 
            // btn_TableGoHome
            // 
            this.btn_TableGoHome.BackColor = System.Drawing.Color.Lime;
            this.btn_TableGoHome.Location = new System.Drawing.Point(611, 3);
            this.btn_TableGoHome.Name = "btn_TableGoHome";
            this.btn_TableGoHome.Size = new System.Drawing.Size(88, 40);
            this.btn_TableGoHome.TabIndex = 55;
            this.btn_TableGoHome.Text = "重新設定原點";
            this.btn_TableGoHome.UseVisualStyleBackColor = false;
            this.btn_TableGoHome.Click += new System.EventHandler(this.btn_TableGoHome_Click);
            // 
            // btn_Jump
            // 
            this.btn_Jump.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Jump.Location = new System.Drawing.Point(218, 329);
            this.btn_Jump.Name = "btn_Jump";
            this.btn_Jump.Size = new System.Drawing.Size(74, 39);
            this.btn_Jump.TabIndex = 58;
            this.btn_Jump.Text = "Jump";
            this.btn_Jump.UseVisualStyleBackColor = true;
            this.btn_Jump.Click += new System.EventHandler(this.btn_Jump_Click);
            // 
            // comboBox_Motor_Speed
            // 
            this.comboBox_Motor_Speed.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.comboBox_Motor_Speed.FormattingEnabled = true;
            this.comboBox_Motor_Speed.Items.AddRange(new object[] {
            "1",
            "5",
            "10",
            "30",
            "50",
            "80",
            "100"});
            this.comboBox_Motor_Speed.Location = new System.Drawing.Point(68, 256);
            this.comboBox_Motor_Speed.Name = "comboBox_Motor_Speed";
            this.comboBox_Motor_Speed.Size = new System.Drawing.Size(55, 27);
            this.comboBox_Motor_Speed.TabIndex = 59;
            this.comboBox_Motor_Speed.Text = "5";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label13.Location = new System.Drawing.Point(128, 259);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(24, 19);
            this.label13.TabIndex = 61;
            this.label13.Text = "%";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.Location = new System.Drawing.Point(10, 259);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 19);
            this.label7.TabIndex = 60;
            this.label7.Text = "速度: ";
            // 
            // UserControl_PointsTeach_
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.comboBox_Motor_Speed);
            this.Controls.Add(this.btn_Jump);
            this.Controls.Add(this.btn_JumpHome);
            this.Controls.Add(this.btn_EmgStop);
            this.Controls.Add(this.btn_TableGoHome);
            this.Controls.Add(this.comboBox_Points);
            this.Controls.Add(this.btn_Recovery);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.btn_Teach);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dataGridView_Points);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label_TableName);
            this.Controls.Add(this.label1);
            this.Name = "UserControl_PointsTeach_";
            this.Size = new System.Drawing.Size(712, 386);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Points)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView_Points;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Item;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Name;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_TableName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_Teach;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Button btn_Recovery;
        private System.Windows.Forms.ComboBox comboBox_Points;
        private System.Windows.Forms.Timer timer_CoordinateDisplay;
        private System.Windows.Forms.Button btn_JumpHome;
        private System.Windows.Forms.Button btn_EmgStop;
        private System.Windows.Forms.Button btn_TableGoHome;
        private System.Windows.Forms.Button btn_Jump;
        private System.Windows.Forms.ComboBox comboBox_Motor_Speed;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label7;
    }
}
