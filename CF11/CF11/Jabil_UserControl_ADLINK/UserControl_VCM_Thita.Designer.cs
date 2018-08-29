namespace Jabil_UserControl_ADLINK
{
    partial class UserControl_VCM_Thita
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
            this.groupBox_Motor0 = new System.Windows.Forms.GroupBox();
            this.btn_Motor0_Reset = new System.Windows.Forms.Button();
            this.btn_Motor0_Stop = new System.Windows.Forms.Button();
            this.label_Axis0_ID = new System.Windows.Forms.Label();
            this.groupBox_FB_Motor0 = new System.Windows.Forms.GroupBox();
            this.pictureBox_Motor0_ALM = new System.Windows.Forms.PictureBox();
            this.pictureBox_Motor0_EMG = new System.Windows.Forms.PictureBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.pictureBox_Motor0_ORG = new System.Windows.Forms.PictureBox();
            this.label_Motor0_FB_Pos = new System.Windows.Forms.Label();
            this.pictureBox_Motor0_SVON = new System.Windows.Forms.PictureBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label_Motor0_FB_Command = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.pictureBox_Motor0_MEL = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox_Motor0_SMV = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox_Motor0_PEL = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox_Motor0_HMV = new System.Windows.Forms.PictureBox();
            this.groupBox_Command_Motor0 = new System.Windows.Forms.GroupBox();
            this.textBox_Angle = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.btn_Motor0_JogPlus = new System.Windows.Forms.Button();
            this.comboBox_Speed = new System.Windows.Forms.ComboBox();
            this.btn_Motor0_JogMinus = new System.Windows.Forms.Button();
            this.btn_Motor0_Move = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.timer_StatusDisplay = new System.Windows.Forms.Timer(this.components);
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox_Motor0.SuspendLayout();
            this.groupBox_FB_Motor0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Motor0_ALM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Motor0_EMG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Motor0_ORG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Motor0_SVON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Motor0_MEL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Motor0_SMV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Motor0_PEL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Motor0_HMV)).BeginInit();
            this.groupBox_Command_Motor0.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_Motor0
            // 
            this.groupBox_Motor0.Controls.Add(this.btn_Motor0_Reset);
            this.groupBox_Motor0.Controls.Add(this.btn_Motor0_Stop);
            this.groupBox_Motor0.Controls.Add(this.label_Axis0_ID);
            this.groupBox_Motor0.Controls.Add(this.groupBox_FB_Motor0);
            this.groupBox_Motor0.Controls.Add(this.groupBox_Command_Motor0);
            this.groupBox_Motor0.Controls.Add(this.label8);
            this.groupBox_Motor0.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox_Motor0.Location = new System.Drawing.Point(28, 31);
            this.groupBox_Motor0.Name = "groupBox_Motor0";
            this.groupBox_Motor0.Size = new System.Drawing.Size(481, 279);
            this.groupBox_Motor0.TabIndex = 1;
            this.groupBox_Motor0.TabStop = false;
            this.groupBox_Motor0.Text = "軸0";
            // 
            // btn_Motor0_Reset
            // 
            this.btn_Motor0_Reset.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Motor0_Reset.Location = new System.Drawing.Point(351, 225);
            this.btn_Motor0_Reset.Name = "btn_Motor0_Reset";
            this.btn_Motor0_Reset.Size = new System.Drawing.Size(86, 40);
            this.btn_Motor0_Reset.TabIndex = 4;
            this.btn_Motor0_Reset.Text = "Reset";
            this.btn_Motor0_Reset.UseVisualStyleBackColor = true;
            this.btn_Motor0_Reset.Click += new System.EventHandler(this.btn_Motor0_Reset_Click);
            // 
            // btn_Motor0_Stop
            // 
            this.btn_Motor0_Stop.BackColor = System.Drawing.Color.Red;
            this.btn_Motor0_Stop.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Motor0_Stop.Location = new System.Drawing.Point(236, 225);
            this.btn_Motor0_Stop.Name = "btn_Motor0_Stop";
            this.btn_Motor0_Stop.Size = new System.Drawing.Size(86, 40);
            this.btn_Motor0_Stop.TabIndex = 3;
            this.btn_Motor0_Stop.Text = "STOP";
            this.btn_Motor0_Stop.UseVisualStyleBackColor = false;
            // 
            // label_Axis0_ID
            // 
            this.label_Axis0_ID.AutoSize = true;
            this.label_Axis0_ID.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_Axis0_ID.Location = new System.Drawing.Point(119, 21);
            this.label_Axis0_ID.Name = "label_Axis0_ID";
            this.label_Axis0_ID.Size = new System.Drawing.Size(22, 19);
            this.label_Axis0_ID.TabIndex = 4;
            this.label_Axis0_ID.Text = "X";
            // 
            // groupBox_FB_Motor0
            // 
            this.groupBox_FB_Motor0.Controls.Add(this.label10);
            this.groupBox_FB_Motor0.Controls.Add(this.label9);
            this.groupBox_FB_Motor0.Controls.Add(this.pictureBox_Motor0_ALM);
            this.groupBox_FB_Motor0.Controls.Add(this.pictureBox_Motor0_EMG);
            this.groupBox_FB_Motor0.Controls.Add(this.label19);
            this.groupBox_FB_Motor0.Controls.Add(this.label16);
            this.groupBox_FB_Motor0.Controls.Add(this.pictureBox_Motor0_ORG);
            this.groupBox_FB_Motor0.Controls.Add(this.label_Motor0_FB_Pos);
            this.groupBox_FB_Motor0.Controls.Add(this.pictureBox_Motor0_SVON);
            this.groupBox_FB_Motor0.Controls.Add(this.label17);
            this.groupBox_FB_Motor0.Controls.Add(this.label15);
            this.groupBox_FB_Motor0.Controls.Add(this.label_Motor0_FB_Command);
            this.groupBox_FB_Motor0.Controls.Add(this.label6);
            this.groupBox_FB_Motor0.Controls.Add(this.label12);
            this.groupBox_FB_Motor0.Controls.Add(this.pictureBox_Motor0_MEL);
            this.groupBox_FB_Motor0.Controls.Add(this.label1);
            this.groupBox_FB_Motor0.Controls.Add(this.pictureBox_Motor0_SMV);
            this.groupBox_FB_Motor0.Controls.Add(this.label5);
            this.groupBox_FB_Motor0.Controls.Add(this.label3);
            this.groupBox_FB_Motor0.Controls.Add(this.pictureBox_Motor0_PEL);
            this.groupBox_FB_Motor0.Controls.Add(this.label4);
            this.groupBox_FB_Motor0.Controls.Add(this.pictureBox_Motor0_HMV);
            this.groupBox_FB_Motor0.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox_FB_Motor0.Location = new System.Drawing.Point(219, 19);
            this.groupBox_FB_Motor0.Name = "groupBox_FB_Motor0";
            this.groupBox_FB_Motor0.Size = new System.Drawing.Size(249, 194);
            this.groupBox_FB_Motor0.TabIndex = 2;
            this.groupBox_FB_Motor0.TabStop = false;
            this.groupBox_FB_Motor0.Text = "FeedBack";
            // 
            // pictureBox_Motor0_ALM
            // 
            this.pictureBox_Motor0_ALM.Location = new System.Drawing.Point(172, 155);
            this.pictureBox_Motor0_ALM.Name = "pictureBox_Motor0_ALM";
            this.pictureBox_Motor0_ALM.Size = new System.Drawing.Size(25, 22);
            this.pictureBox_Motor0_ALM.TabIndex = 10;
            this.pictureBox_Motor0_ALM.TabStop = false;
            // 
            // pictureBox_Motor0_EMG
            // 
            this.pictureBox_Motor0_EMG.Location = new System.Drawing.Point(172, 104);
            this.pictureBox_Motor0_EMG.Name = "pictureBox_Motor0_EMG";
            this.pictureBox_Motor0_EMG.Size = new System.Drawing.Size(25, 22);
            this.pictureBox_Motor0_EMG.TabIndex = 10;
            this.pictureBox_Motor0_EMG.TabStop = false;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label19.Location = new System.Drawing.Point(169, 139);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(34, 13);
            this.label19.TabIndex = 11;
            this.label19.Text = "ALM";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label16.Location = new System.Drawing.Point(169, 87);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(34, 13);
            this.label16.TabIndex = 11;
            this.label16.Text = "EMG";
            // 
            // pictureBox_Motor0_ORG
            // 
            this.pictureBox_Motor0_ORG.Location = new System.Drawing.Point(119, 155);
            this.pictureBox_Motor0_ORG.Name = "pictureBox_Motor0_ORG";
            this.pictureBox_Motor0_ORG.Size = new System.Drawing.Size(25, 22);
            this.pictureBox_Motor0_ORG.TabIndex = 10;
            this.pictureBox_Motor0_ORG.TabStop = false;
            // 
            // label_Motor0_FB_Pos
            // 
            this.label_Motor0_FB_Pos.AutoSize = true;
            this.label_Motor0_FB_Pos.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_Motor0_FB_Pos.Location = new System.Drawing.Point(134, 55);
            this.label_Motor0_FB_Pos.Name = "label_Motor0_FB_Pos";
            this.label_Motor0_FB_Pos.Size = new System.Drawing.Size(35, 19);
            this.label_Motor0_FB_Pos.TabIndex = 4;
            this.label_Motor0_FB_Pos.Text = "OO";
            // 
            // pictureBox_Motor0_SVON
            // 
            this.pictureBox_Motor0_SVON.Location = new System.Drawing.Point(119, 104);
            this.pictureBox_Motor0_SVON.Name = "pictureBox_Motor0_SVON";
            this.pictureBox_Motor0_SVON.Size = new System.Drawing.Size(25, 22);
            this.pictureBox_Motor0_SVON.TabIndex = 10;
            this.pictureBox_Motor0_SVON.TabStop = false;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label17.Location = new System.Drawing.Point(112, 139);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(33, 13);
            this.label17.TabIndex = 11;
            this.label17.Text = "ORG";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label15.Location = new System.Drawing.Point(112, 87);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(41, 13);
            this.label15.TabIndex = 11;
            this.label15.Text = "SVON";
            // 
            // label_Motor0_FB_Command
            // 
            this.label_Motor0_FB_Command.AutoSize = true;
            this.label_Motor0_FB_Command.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_Motor0_FB_Command.Location = new System.Drawing.Point(134, 23);
            this.label_Motor0_FB_Command.Name = "label_Motor0_FB_Command";
            this.label_Motor0_FB_Command.Size = new System.Drawing.Size(35, 19);
            this.label_Motor0_FB_Command.TabIndex = 4;
            this.label_Motor0_FB_Command.Text = "OO";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.Location = new System.Drawing.Point(64, 140);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "MEL";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label12.Location = new System.Drawing.Point(64, 88);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(34, 13);
            this.label12.TabIndex = 11;
            this.label12.Text = "SMV";
            // 
            // pictureBox_Motor0_MEL
            // 
            this.pictureBox_Motor0_MEL.Location = new System.Drawing.Point(68, 155);
            this.pictureBox_Motor0_MEL.Name = "pictureBox_Motor0_MEL";
            this.pictureBox_Motor0_MEL.Size = new System.Drawing.Size(25, 22);
            this.pictureBox_Motor0_MEL.TabIndex = 10;
            this.pictureBox_Motor0_MEL.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(55, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "Position:";
            // 
            // pictureBox_Motor0_SMV
            // 
            this.pictureBox_Motor0_SMV.Location = new System.Drawing.Point(68, 104);
            this.pictureBox_Motor0_SMV.Name = "pictureBox_Motor0_SMV";
            this.pictureBox_Motor0_SMV.Size = new System.Drawing.Size(25, 22);
            this.pictureBox_Motor0_SMV.TabIndex = 10;
            this.pictureBox_Motor0_SMV.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(13, 140);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "PEL";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(13, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "HMV";
            // 
            // pictureBox_Motor0_PEL
            // 
            this.pictureBox_Motor0_PEL.Location = new System.Drawing.Point(16, 155);
            this.pictureBox_Motor0_PEL.Name = "pictureBox_Motor0_PEL";
            this.pictureBox_Motor0_PEL.Size = new System.Drawing.Size(25, 22);
            this.pictureBox_Motor0_PEL.TabIndex = 10;
            this.pictureBox_Motor0_PEL.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(39, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 19);
            this.label4.TabIndex = 3;
            this.label4.Text = "Command:";
            // 
            // pictureBox_Motor0_HMV
            // 
            this.pictureBox_Motor0_HMV.Location = new System.Drawing.Point(16, 104);
            this.pictureBox_Motor0_HMV.Name = "pictureBox_Motor0_HMV";
            this.pictureBox_Motor0_HMV.Size = new System.Drawing.Size(25, 22);
            this.pictureBox_Motor0_HMV.TabIndex = 10;
            this.pictureBox_Motor0_HMV.TabStop = false;
            // 
            // groupBox_Command_Motor0
            // 
            this.groupBox_Command_Motor0.Controls.Add(this.textBox_Angle);
            this.groupBox_Command_Motor0.Controls.Add(this.label14);
            this.groupBox_Command_Motor0.Controls.Add(this.label13);
            this.groupBox_Command_Motor0.Controls.Add(this.btn_Motor0_JogPlus);
            this.groupBox_Command_Motor0.Controls.Add(this.comboBox_Speed);
            this.groupBox_Command_Motor0.Controls.Add(this.btn_Motor0_JogMinus);
            this.groupBox_Command_Motor0.Controls.Add(this.btn_Motor0_Move);
            this.groupBox_Command_Motor0.Controls.Add(this.label7);
            this.groupBox_Command_Motor0.Controls.Add(this.label2);
            this.groupBox_Command_Motor0.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox_Command_Motor0.Location = new System.Drawing.Point(13, 54);
            this.groupBox_Command_Motor0.Name = "groupBox_Command_Motor0";
            this.groupBox_Command_Motor0.Size = new System.Drawing.Size(200, 188);
            this.groupBox_Command_Motor0.TabIndex = 1;
            this.groupBox_Command_Motor0.TabStop = false;
            this.groupBox_Command_Motor0.Text = "Command";
            // 
            // textBox_Angle
            // 
            this.textBox_Angle.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox_Angle.Location = new System.Drawing.Point(83, 26);
            this.textBox_Angle.Name = "textBox_Angle";
            this.textBox_Angle.Size = new System.Drawing.Size(55, 30);
            this.textBox_Angle.TabIndex = 1;
            this.textBox_Angle.Text = "10";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label14.Location = new System.Drawing.Point(142, 29);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(57, 19);
            this.label14.TabIndex = 4;
            this.label14.Text = "degree";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label13.Location = new System.Drawing.Point(143, 66);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(24, 19);
            this.label13.TabIndex = 4;
            this.label13.Text = "%";
            // 
            // btn_Motor0_JogPlus
            // 
            this.btn_Motor0_JogPlus.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Motor0_JogPlus.Location = new System.Drawing.Point(24, 142);
            this.btn_Motor0_JogPlus.Name = "btn_Motor0_JogPlus";
            this.btn_Motor0_JogPlus.Size = new System.Drawing.Size(68, 30);
            this.btn_Motor0_JogPlus.TabIndex = 1;
            this.btn_Motor0_JogPlus.Text = "JOG+";
            this.btn_Motor0_JogPlus.UseVisualStyleBackColor = true;
            this.btn_Motor0_JogPlus.Click += new System.EventHandler(this.btn_Motor0_JogPlus_Click);
            // 
            // comboBox_Speed
            // 
            this.comboBox_Speed.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.comboBox_Speed.FormattingEnabled = true;
            this.comboBox_Speed.Items.AddRange(new object[] {
            "1",
            "5",
            "10",
            "30",
            "50",
            "80",
            "100"});
            this.comboBox_Speed.Location = new System.Drawing.Point(84, 63);
            this.comboBox_Speed.Name = "comboBox_Speed";
            this.comboBox_Speed.Size = new System.Drawing.Size(55, 27);
            this.comboBox_Speed.TabIndex = 1;
            this.comboBox_Speed.Text = "5";
            // 
            // btn_Motor0_JogMinus
            // 
            this.btn_Motor0_JogMinus.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Motor0_JogMinus.Location = new System.Drawing.Point(98, 142);
            this.btn_Motor0_JogMinus.Name = "btn_Motor0_JogMinus";
            this.btn_Motor0_JogMinus.Size = new System.Drawing.Size(68, 30);
            this.btn_Motor0_JogMinus.TabIndex = 2;
            this.btn_Motor0_JogMinus.Text = "JOG-";
            this.btn_Motor0_JogMinus.UseVisualStyleBackColor = true;
            this.btn_Motor0_JogMinus.Click += new System.EventHandler(this.btn_Motor0_JogMinus_Click);
            // 
            // btn_Motor0_Move
            // 
            this.btn_Motor0_Move.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_Motor0_Move.Location = new System.Drawing.Point(55, 100);
            this.btn_Motor0_Move.Name = "btn_Motor0_Move";
            this.btn_Motor0_Move.Size = new System.Drawing.Size(83, 30);
            this.btn_Motor0_Move.TabIndex = 2;
            this.btn_Motor0_Move.Text = "Move";
            this.btn_Motor0_Move.UseVisualStyleBackColor = true;
            this.btn_Motor0_Move.Click += new System.EventHandler(this.btn_Motor0_Move_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.Location = new System.Drawing.Point(26, 66);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 19);
            this.label7.TabIndex = 3;
            this.label7.Text = "Speed:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(26, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 19);
            this.label2.TabIndex = 2;
            this.label2.Text = "Angle:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label8.Location = new System.Drawing.Point(51, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 19);
            this.label8.TabIndex = 4;
            this.label8.Text = "Axis ID: ";
            // 
            // timer_StatusDisplay
            // 
            this.timer_StatusDisplay.Tick += new System.EventHandler(this.timer_StatusDisplay_Tick);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label9.Location = new System.Drawing.Point(182, 55);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(57, 19);
            this.label9.TabIndex = 12;
            this.label9.Text = "degree";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("新細明體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label10.Location = new System.Drawing.Point(182, 23);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(57, 19);
            this.label10.TabIndex = 13;
            this.label10.Text = "degree";
            // 
            // UserControl_VCM_Thita
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox_Motor0);
            this.Name = "UserControl_VCM_Thita";
            this.Size = new System.Drawing.Size(512, 338);
            this.groupBox_Motor0.ResumeLayout(false);
            this.groupBox_Motor0.PerformLayout();
            this.groupBox_FB_Motor0.ResumeLayout(false);
            this.groupBox_FB_Motor0.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Motor0_ALM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Motor0_EMG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Motor0_ORG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Motor0_SVON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Motor0_MEL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Motor0_SMV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Motor0_PEL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Motor0_HMV)).EndInit();
            this.groupBox_Command_Motor0.ResumeLayout(false);
            this.groupBox_Command_Motor0.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_Motor0;
        private System.Windows.Forms.Button btn_Motor0_Reset;
        private System.Windows.Forms.Button btn_Motor0_Stop;
        private System.Windows.Forms.Label label_Axis0_ID;
        private System.Windows.Forms.GroupBox groupBox_FB_Motor0;
        private System.Windows.Forms.PictureBox pictureBox_Motor0_ALM;
        private System.Windows.Forms.PictureBox pictureBox_Motor0_EMG;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.PictureBox pictureBox_Motor0_ORG;
        private System.Windows.Forms.Label label_Motor0_FB_Pos;
        private System.Windows.Forms.PictureBox pictureBox_Motor0_SVON;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label_Motor0_FB_Command;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.PictureBox pictureBox_Motor0_MEL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox_Motor0_SMV;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox_Motor0_PEL;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox_Motor0_HMV;
        private System.Windows.Forms.GroupBox groupBox_Command_Motor0;
        private System.Windows.Forms.TextBox textBox_Angle;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btn_Motor0_JogPlus;
        private System.Windows.Forms.ComboBox comboBox_Speed;
        private System.Windows.Forms.Button btn_Motor0_JogMinus;
        private System.Windows.Forms.Button btn_Motor0_Move;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Timer timer_StatusDisplay;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
    }
}
