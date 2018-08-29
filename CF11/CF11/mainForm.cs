using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using APS168_W32;
using APS_Define_W32;
using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro.Display;
using Cognex.VisionPro.Caliper;
using Cognex.VisionPro.Dimensioning;


namespace CF11
{
    #region Enumeration
    public enum PRODUCT { None, DRL_L, DRL_R, LOWPOWER_L, LOWPOWER_R, DRLVAVE_L, DRLVAVE_R };
    public enum PROCESSMSG
    {
        GoHomeFirst,
        PutProductAndClickPhysicalButton,
        FirstInspection,
        CCDGoHomeManual,
        CCDGoHomeAuto,
        ResultOK,
        ResultNG,
        ClickPhysicalButtonAfterLockScrew,
        SecondInspection,
    };
    public enum MAINTAB { USER, EDITOR, OPERATOR, HELPER, OTHER };
    public enum ITEM { BARCODE, LED1X, LED1Y, LED2X, LED2Y, RESULT, TIME };
    #endregion

    public partial class mainForm : Form
    {
        #region Global Variable
        // Global Variable
        public bool isLogin = false;
        private SaveFile saveFile = new SaveFile();
        private double total_num = 0.0, ok_num = 0.0, ok_percentage = 0.0;
        #endregion

        #region Form Initialize
        public mainForm()
        {
            InitializeComponent();
            this.ConnectObjectEvent();

            this.Task_SystemStrip();

            this.Display();

            // thread start
            this.OperationThread = new Thread(this.OperationThreadRun);    
            this.OperationThread.Name = "Thread: Operation Run!";
            this.OperationThread.Priority = ThreadPriority.AboveNormal;
            this.OperationThread.IsBackground = true;
            this.OperationThread.Start();

            this.ShowMainTab(this.mainTabControl, false);
           // this.mainTabControl.Enabled = false;
            this.Task_GoHomeWhenBoot();

        }

        private void ConnectObjectEvent()
        {
            // TabControl DrawItem
            this.userTabControl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.userTabControl_DrawItem);
            this.editTabControl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.editTabControl_DrawItem);
            this.operateTabControl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.operateTabControl_DrawItem);
            this.helpTabControl.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.helpTabControl_DrawItem);
            // TabControl SelectedIndexChanged
            this.pictureTabControl.SelectedIndexChanged += new System.EventHandler(this.pictureTabControl_SelectedIndexChanged);
            // Button Click
            this.shutDownButton.Click += new System.EventHandler(this.shutDownButton_Click);
            this.shutDownToolStripSplitButton.ButtonClick += new System.EventHandler(this.shutDownToolStripSplitButton_ButtonClick);
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            this.changePasswordButton.Click += new System.EventHandler(this.changePasswordButton_Click);
            this.initializeButton.Click += new System.EventHandler(this.initializeButton_Click);
            this.busConnectButton.Click += new System.EventHandler(this.busConnectButton_Click);
            this.busDisconnectButton.Click += new System.EventHandler(this.busDisconnectButton_Click);
            this.closeMotionCardButton.Click += new System.EventHandler(this.closeMotionCardButton_Click);
            this.servoOnButton.Click += new System.EventHandler(this.servoOnButton_Click);
            this.servoOffButton.Click += new System.EventHandler(this.servoOffButton_Click);
            this.absoluteMoveButton.Click += new System.EventHandler(this.absoluteMoveButton_Click);
            this.resetCounterButton.Click += new System.EventHandler(this.resetCounterButton_Click);
            this.relativeJogPlusButton.Click += new System.EventHandler(this.relativeJogPlusButton_Click);
            this.relativeJogMinusButton.Click += new System.EventHandler(this.relativeJogMinusButton_Click);
            this.motionUnlockButton.Click += new System.EventHandler(this.motionUnlockButton_Click);
            this.motionStopButton.Click += new System.EventHandler(this.motionStopButton_Click);
            // Panel Paint
            this.showPassNgPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.showPassNgPanel_Paint);
            // RadioButton
            this.selectDrlVaveRightRadioButton.CheckedChanged += new System.EventHandler(this.SelectRadioButton_CheckedChanged);
            this.selectDrlVaveLeftRadioButton.CheckedChanged += new System.EventHandler(this.SelectRadioButton_CheckedChanged);
            this.selectLowPowerRightRadioButton.CheckedChanged += new System.EventHandler(this.SelectRadioButton_CheckedChanged);
            this.selectLowPowerLeftRadioButton.CheckedChanged += new System.EventHandler(this.SelectRadioButton_CheckedChanged);
            this.selectDrlRightRadioButton.CheckedChanged += new System.EventHandler(this.SelectRadioButton_CheckedChanged);
            this.selectDrlLeftRadioButton.CheckedChanged += new System.EventHandler(this.SelectRadioButton_CheckedChanged);
            this.SelectNoneRadioButton.CheckedChanged += new System.EventHandler(this.SelectRadioButton_CheckedChanged);
        }

        private void Display()
        {
            this.cardIdComboBox.SelectedIndex = 0;
            this.axisIdComboBox.SelectedIndex = 0;
            this.speedPercentageComboBox.SelectedIndex = 1;

            this.productInfoDataGridView.Rows.Clear();
            this.productInfoDataGridView.Rows.Add();
            this.productInfoDataGridView.Rows[this.product_num].Selected = true;
        }
        #endregion

        #region TabControl DrawItem
        private void DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush _textBrush;

            // Get the item from the collection.
            TabPage _tabPage = ((TabControl)sender).TabPages[e.Index];

            // Get the real bounds for the tab rectangle.
            Rectangle _tabBounds = ((TabControl)sender).GetTabRect(e.Index);

            if (e.State == DrawItemState.Selected)
            {

                // Draw a different background color, and don't paint a focus rectangle.
                _textBrush = new SolidBrush(Color.Black);
                g.FillRectangle(new SolidBrush(SystemColors.Control), e.Bounds);
            }
            else
            {
                _textBrush = new System.Drawing.SolidBrush(e.ForeColor);
                g.FillRectangle(new SolidBrush(SystemColors.ControlDark), e.Bounds);
                //e.DrawBackground();
            }

            // Use our own font.
            Font _tabFont = new Font("標楷體", (float)12.0, FontStyle.Regular, GraphicsUnit.Point);

            // Draw string. Center the text.
            StringFormat _stringFlags = new StringFormat();
            _stringFlags.Alignment = StringAlignment.Center;
            _stringFlags.LineAlignment = StringAlignment.Center;
            g.DrawString(_tabPage.Text, _tabFont, _textBrush, _tabBounds, new StringFormat(_stringFlags));
        }

        private void userTabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            this.DrawItem(sender, e);
        }
        private void editTabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            this.DrawItem(sender, e);
        }
        private void operateTabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            this.DrawItem(sender, e);
        }
        private void helpTabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            this.DrawItem(sender, e);
        }
        #endregion

        #region Shutdown Program
        private bool isShutDown = false;
        private void shutDownProgram()
        {
            if (DialogResult.OK == MessageBox.Show("確定離開程序？", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
            {
                this.showLiveProductDataTimer.Stop();
                this.isThreadStart = false;
                this.isShutDown = true;
                System.Threading.Thread.Sleep(600);
                this.OperationThread.Abort();

                // 關閉 cognex 使用
                Cognex.VisionPro.CogFrameGrabbers framegrabbers = new Cognex.VisionPro.CogFrameGrabbers();
                foreach (Cognex.VisionPro.ICogFrameGrabber fg in framegrabbers)
                    fg.Disconnect(false);

                this.Close();
            }
        }
        private void shutDownButton_Click(object sender, EventArgs e)
        {
            this.shutDownProgram();
        }
        private void shutDownToolStripSplitButton_ButtonClick(object sender, EventArgs e)
        {
            this.shutDownProgram();
        }
        #endregion

        #region Go Home When Boot
        private bool isGoHomeWhenBoot = false;
        private delegate void ShowMainTabHandler(Control Object, bool isEnable);
        private void ShowMainTab(Control Object, bool isEnable)       
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ShowMainTabHandler(ShowMainTab), Object, isEnable);
            }
            else
            {
                Object.Enabled = isEnable;
            }
        }
        private void Task_GoHomeWhenBoot()
        {
            this.isGoHomeWhenBoot = true;
            var task_time = Task.Factory.StartNew(() =>
            {
                System.Threading.Thread.Sleep(500);

                this.InitializeMotionControl(false, 0);
                this.BusConnect(false);
                this.ServoControl(ON, false, 0);

                this.MotionParameter();
                this.RelMove(0, -200, 100, false, 10000);    // negative, unit:mm
                while (false == this.axis_status.Mio_MEL)
                {
                    this.motionStatusDetection();
                    System.Threading.Thread.Sleep(200);
                }

                this.ServoControl(OFF, false, 0);
                this.BusDisconnect();
                this.CloseMotionCard();

                this.isGoHomeWhenBoot = false;
                this.ShowMainTab(this.mainTabControl, true);
            });
        }
        #endregion

        #region System Strip
        private delegate void ShowSystemTimeHandler(Object Object, string Text);
        public void ShowSystemTime(Object Object, String Text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ShowSystemTimeHandler(ShowSystemTime), Object, Text);
            }
            else
            {
                ((ToolStripStatusLabel)Object).Text = Text;
            }
        }
       
        private void Task_SystemStrip()
        {
            var task_time = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(500);
                while (false == this.isShutDown)
                {
                    this.ShowSystemTime(systemTimeToolStripStatusLabel, DateTime.Now.ToString("yyyy/MM/dd hh:mm"));
                    Thread.Sleep(15000);
                }
            });
        }
        #endregion

        #region User Information
        // for login
        private void showChangePasswordSettings(bool isShown)
        {
            this.changePasswordButton.Enabled = isShown;
            if (true == this.newPasswordTextBox.Visible)
            {
                this.showNewPassword(false);
            }

            this.accountTextBox.Enabled = !isShown;
            this.passwordTextBox.Enabled = !isShown;
        }
        private void showNewPassword(bool isShown)
        {
            this.newPasswordLabel.Visible = isShown;
            this.newPasswordTextBox.Visible = isShown;
            this.comfirmNewPasswordLabel.Visible = isShown;
            this.comfirmNewPasswordTextBox.Visible = isShown;
        }
        private void loginButton_Click(object sender, EventArgs e)
        {
            string accoout = Properties.Settings.Default.account;
            string password = Properties.Settings.Default.password;
            const int correct = 0;

            if (false == this.isLogin)
            {
                if (correct == string.Compare(accoout, accountTextBox.Text, false))
                {
                    if (correct == string.Compare(password, passwordTextBox.Text, false))
                    {
                        this.isLogin = true;
                        this.showChangePasswordSettings(this.isLogin);
                        this.accountToolStripStatusLabel.Text = this.accountTextBox.Text;
                        this.loginButton.Text = "登出";
                    }
                    else
                    {
                        MessageBox.Show("密碼輸入錯誤！", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("用戶輸入錯誤！", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                this.accountTextBox.Text = string.Empty;
                this.passwordTextBox.Text = string.Empty;

                this.isLogin = false;
                this.showChangePasswordSettings(this.isLogin);
                this.accountToolStripStatusLabel.Text = "None";
                this.loginButton.Text = "登入";
                this.accountTextBox.Focus();
            }
        }
        // for change password
        private void changePasswordButton_Click(object sender, EventArgs e)
        {
            string password = Properties.Settings.Default.password;
            const int correct = 0;

            if ("更換密碼" == ((Button)sender).Text)
            {
                if (true == this.newPasswordTextBox.Visible)
                {
                    if ((string.Empty != this.newPasswordTextBox.Text) && (string.Empty != this.comfirmNewPasswordTextBox.Text))
                    {
                        if (correct == string.Compare(this.newPasswordTextBox.Text, this.comfirmNewPasswordTextBox.Text))
                        {
                            this.loginButton.Enabled = false;
                            this.passwordTextBox.Enabled = true;
                            this.newPasswordTextBox.Enabled = false;
                            this.comfirmNewPasswordTextBox.Enabled = false;
                            ((Button)sender).Text = "確認原密碼";
                            this.passwordTextBox.Text = string.Empty;
                            this.passwordTextBox.Focus();
                        }
                        else
                        {
                            MessageBox.Show("新密碼確認錯誤，請重新輸入！", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            this.newPasswordTextBox.Text = string.Empty;
                            this.comfirmNewPasswordTextBox.Text = string.Empty;
                            // 
                            this.loginButton.Enabled = true;
                            this.passwordTextBox.Enabled = false;
                            this.newPasswordTextBox.Enabled = true;
                            this.comfirmNewPasswordTextBox.Enabled = true;
                            ((Button)sender).Text = "更換密碼";
                        }
                    }
                    else
                    {
                        // MessageBox.Show("新密碼不可為空白！", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.showNewPassword(false);
                        // 
                        this.loginButton.Enabled = true;
                        this.passwordTextBox.Enabled = false;
                        this.newPasswordTextBox.Enabled = true;
                        this.comfirmNewPasswordTextBox.Enabled = true;
                        ((Button)sender).Text = "更換密碼";
                    }
                }
                else
                {
                    this.newPasswordTextBox.Text = string.Empty;
                    this.comfirmNewPasswordTextBox.Text = string.Empty;
                    this.showNewPassword(true);
                }
            }
            else if ("確認原密碼" == ((Button)sender).Text)
            {
                if (correct == string.Compare(password, passwordTextBox.Text, false))
                {
                    // 1. set new password to setting file
                    Properties.Settings.Default.password = this.newPasswordTextBox.Text;

                    // 2. log out
                    this.loginButton.Enabled = true;
                    this.newPasswordTextBox.Enabled = true;
                    this.comfirmNewPasswordTextBox.Enabled = true;
                    this.loginButton.PerformClick();

                    // 3. set focus
                    this.accountTextBox.Focus();
                }
                else
                {
                    MessageBox.Show("密碼輸入錯誤！", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    this.passwordTextBox.Text = string.Empty;

                    this.passwordTextBox.Text = Properties.Settings.Default.password;
                    this.loginButton.Enabled = true;
                    this.passwordTextBox.Enabled = false;
                    this.newPasswordTextBox.Enabled = true;
                    this.comfirmNewPasswordTextBox.Enabled = true;
                }
                // 
                ((Button)sender).Text = "更換密碼";
                this.newPasswordTextBox.Text = string.Empty;
                this.comfirmNewPasswordTextBox.Text = string.Empty;
            }
            else
            {
                //
                this.loginButton.Enabled = true;
                this.passwordTextBox.Enabled = false;
                this.newPasswordTextBox.Enabled = true;
                this.comfirmNewPasswordTextBox.Enabled = true;
                ((Button)sender).Text = "更換密碼";
            }
        }
        #endregion

        private bool isHomingManual = false;
        private bool isScreenSplit = false;
        private int picture_old_selected = 0;
        private void pictureTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (((TabControl)sender).SelectedIndex)
            {
                case 0:
                    {
                        this.picture_old_selected = 0;
                        this.isScreenSplit = false;
                        break;
                    }
                case 1:
                    {
                        this.picture_old_selected = 1;
                        this.isScreenSplit = true;
                        break;
                    }
                case 2:
                    {
                        // 1. set selected index as 0
                        this.pictureTabControl.SelectedIndex = this.picture_old_selected;
                        // 2. CCD homing.
                        switch (process_msg)
                        {
                            case PROCESSMSG.GoHomeFirst:
                            case PROCESSMSG.PutProductAndClickPhysicalButton:
                            case PROCESSMSG.CCDGoHomeManual:
                            case PROCESSMSG.CCDGoHomeAuto:
                            case PROCESSMSG.ResultOK:
                            case PROCESSMSG.ResultNG:
                            case PROCESSMSG.ClickPhysicalButtonAfterLockScrew:
                                break;
                            case PROCESSMSG.FirstInspection:
                            case PROCESSMSG.SecondInspection:
                                this.isHomingManual = true;
                                break;
                            default:
                                break;
                        }
                        break;
                    }
                default:
                    break;
            }


        }

        #region Motion Control
        private Int16 CardID = 0;	      //Card number for setting.
        private Int16 BusNo = 1;        //Bus number for setting,  Motion Net BusNo is 0.
        private bool FunctionFail = false;
        private Int32 Start_Axis_ID = 0;  //First Axis number in Motion Net bus.
        private const Int32 ON = 1, OFF = 0;
        private const Int32 YES = 1, NOT = 0;
        private Axis_Status axis_status;
        // Motion Parameter
        private void MotionParameter()
        {
            //
            int mm_per_cycle = 0, pulse_per_cycle = 0;

            int.TryParse(this.mmPerCycleTextBox.Text, out mm_per_cycle);
            int.TryParse(this.pulsePerCycleTextBox.Text, out pulse_per_cycle);

            if (0 == pulse_per_cycle)
            {
                // default mmPerPulse
            }
            else
            {
                this.mmPerPulse = (double)mm_per_cycle / (double)pulse_per_cycle;
            }

            // motion status
            this.axis_status.Mts_HMV = false;
            this.axis_status.Mts_SMV = false;
            this.axis_status.Mio_ALM = false;
            this.axis_status.Mio_EMG = false;
            this.axis_status.Mio_MEL = false;
            this.axis_status.Mio_ORG = false;
            this.axis_status.Mio_PEL = false;
            this.axis_status.Mio_SVON = false;
        }
        // Error code
        private void Function_Result(Int32 Ret)
        {
            if (Ret != 0)
            {
                MessageBox.Show("Function Fail, ErrorCode  " + Ret.ToString());
                FunctionFail = true;
            }
            else
            {
                FunctionFail = false;
            }
        }
        #region 軸控相關方法
        double mmPerPulse = 0.0;              //每個脈波的實際距離 (計算方式: 螺桿Pitch/一圈的脈波數量)
        double maxSpeedPulse = 0.0;           //預設100% 速度per秒  (目前設定為1萬pulse1=1轉)
        //單軸絕對位置移動(mm, speed percent)
        public void AbsMove(int AxisID, double pos, int speed_percent)
        {
            this.maxSpeedPulse = int.Parse(this.maxSpeedPulseTextBox.Text.ToString());
            //將mm轉換成實際脈波數
            int PosPulse = (int)(pos / mmPerPulse);
            //將速度百分比轉換成實際速度
            int SpeedPulse = (int)(maxSpeedPulse * speed_percent / 100);

            if (APS168.APS_absolute_move(AxisID, PosPulse, SpeedPulse) != 0)
            {
                MessageBox.Show("絕對位置移動錯誤");
            }
        }
        //單軸相對位置移動(mm)
        public void RelMove(int AxisID, double distance, int speed_percent, bool isTesting, int max_speed_pulse)
        {
            if (true == isTesting)
            {
                this.maxSpeedPulse = int.Parse(this.maxSpeedPulseTextBox.Text.ToString());
            }
            else
            {
                this.maxSpeedPulse = max_speed_pulse;
            }
            //將mm轉換成實際脈波數
            int DistPulse = (int)(distance / mmPerPulse);
            //將速度百分比轉換成實際速度
            int SpeedPulse = (int)(maxSpeedPulse * speed_percent / 100);

            if (APS168.APS_relative_move(AxisID, DistPulse, SpeedPulse) != 0)
            {
                MessageBox.Show("相對位置移動錯誤");
            }
        }
        #endregion
        // Initialize
        private delegate void ShowMotionInfoHandler(Control Object, string Text);
        private void ShowMotionInfo(Control Object, String Text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ShowMotionInfoHandler(ShowMotionInfo), Object, Text);
            }
            else
            {
                Object.Text = Text;
            }
        }
        private void InitializeMotionControl(bool isTesting, int card_id)
        {
            //Initial++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            Int32 DPAC_ID_Bits = 0;
            Int32 Info = 0;

            this.CardID = (short)card_id;
            if (APS168.APS_initial(ref DPAC_ID_Bits, 0) == 0)
            {
                //Initial Card, CardID is assigned by system.
                //Get Card information
                APS168.APS_get_device_info(CardID, 0x10, ref Info);		//Get Driver Version
                this.ShowMotionInfo(this.Edit_ShowMaster_DriverVer, Info.ToString());
                Info = APS168.APS_version();                            //Get DLL Version
                this.ShowMotionInfo(this.Edit_ShowMaster_DLLVer, Info.ToString());
                APS168.APS_get_device_info(CardID, 0x20, ref Info);		//Get CPLD Version
                this.ShowMotionInfo(this.Edit_ShowMaster_CPLDVer, Info.ToString());

                //Set Motion Net Parameter
                Function_Result(APS168.APS_set_field_bus_param(CardID, BusNo, (Int32)APS_Define.PRF_TRANSFER_RATE, 3));     //Set PRF_TRANSFER_RATE: 3 (12M)
            }
            else
            {
                MessageBox.Show("Initial Fail, sample close!!");
                APS168.APS_close();
            }
        }
        private void initializeButton_Click(object sender, EventArgs e)
        {
            this.CardID = (short)this.cardIdComboBox.SelectedIndex;
            this.InitializeMotionControl(true, this.CardID);
            if (false == FunctionFail)
            {
                ((Button)sender).Enabled = false;
                this.cardIdComboBox.Enabled = false;
            }
        }
        // Close
        private delegate void ClearMotionCardHandler(Control Object, string Text);
        private void ClearMotionCard(Control Object, String Text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ClearMotionCardHandler(ClearMotionCard), Object, Text);
            }
            else
            {
                Object.Text = Text;
            }
        }
        private void CloseMotionCard()
        {
            this.ClearMotionCard(this.Edit_ShowMaster_DriverVer, "0");
            this.ClearMotionCard(this.Edit_ShowMaster_DLLVer, "0");
            this.ClearMotionCard(this.Edit_ShowMaster_CPLDVer, "0");

            this.BusDisconnect();

            APS168.APS_close();
            this.CardID = -1;
        }
        private void closeMotionCardButton_Click(object sender, EventArgs e)
        {
            this.CloseMotionCard();
            this.initializeButton.Enabled = true;
            this.cardIdComboBox.Enabled = true;
        }
        // Connect
        private void BusConnect(bool isTesting)
        {
            //開始連線  (模組沒上電會錯誤)
            Function_Result(APS168.APS_start_field_bus(CardID, BusNo, Start_Axis_ID));

            if (!FunctionFail)
            {
                //Set Axis Parameter
                int AxisNo = this.CardID;

                //IO and Pulse Mode
                Function_Result(APS168.APS_set_axis_param(AxisNo, (Int32)APS_Define.PRA_ALM_LOGIC, 0));       //Set PRA_ALM_LOGIC
                Function_Result(APS168.APS_set_axis_param(AxisNo, (Int32)APS_Define.PRA_PLS_IPT_MODE, 2));    //Set PRA_PLS_IPT_MODE = 4xAB
                Function_Result(APS168.APS_set_axis_param(AxisNo, (Int32)APS_Define.PRA_PLS_OPT_MODE, 4));    //Set PRA_PLS_OPT_MODE = CW/CCW
                Function_Result(APS168.APS_set_axis_param(AxisNo, (Int32)APS_Define.PRA_PLS_IPT_LOGIC, 0));   //Set PRA_PLS_IPT_LOGIC
                Function_Result(APS168.APS_set_axis_param(AxisNo, (Int32)APS_Define.PRA_FEEDBACK_SRC, 1));    //Select feedback conter

                //Function_Result(APS168.APS_set_servo_on(AxisNo, 1));
                Function_Result(APS168.APS_set_axis_param(AxisNo, (Int32)APS_Define.PRA_SERVO_LOGIC, 0));     //Set SERVO output logic  0: Active low

                //Single Move Parameter
                Function_Result(APS168.APS_set_axis_param(AxisNo, (Int32)APS_Define.PRA_CURVE, 1));			  //Set PRA_CURVE  0:T-Curve 1:S-Curve
                Function_Result(APS168.APS_set_axis_param(AxisNo, (Int32)APS_Define.PRA_ACC, 1000357));	      //Set PRA_ACC 
                Function_Result(APS168.APS_set_axis_param(AxisNo, (Int32)APS_Define.PRA_DEC, 1000357));   	  //Set PRA_DEC
                Function_Result(APS168.APS_set_axis_param(AxisNo, (Int32)APS_Define.PRA_VS, 0));			  //Set PRA_VS

                //Home Move Parameter
                Function_Result(APS168.APS_set_axis_param(AxisNo, (Int32)APS_Define.PRA_ORG_LOGIC, 1));       //1: Inverse
                Function_Result(APS168.APS_set_axis_param(AxisNo, (Int32)APS_Define.PRA_HOME_MODE, 1));		  //Set PRA_HOME_MODE
                Function_Result(APS168.APS_set_axis_param(AxisNo, (Int32)APS_Define.PRA_HOME_DIR, 1));		  //Set PRA_HOME_DIR   0:Positive 1:Negative
                Function_Result(APS168.APS_set_axis_param(AxisNo, (Int32)APS_Define.PRA_HOME_VM, 50000));	  //Set PRA_HOME_VM
                Function_Result(APS168.APS_set_axis_param(AxisNo, (Int32)APS_Define.PRA_HOME_EZA, 0));		  //Set PRA_HOME_EZA 
                Function_Result(APS168.APS_set_axis_param(AxisNo, (Int32)APS_Define.PRA_HOME_VO, 10));		  //Set PRA_HOME_VO
                Function_Result(APS168.APS_set_axis_param(AxisNo, (Int32)APS_Define.PRA_HOME_OFFSET, 500));	  //Set PRA_HOME_OFFSET 

                //MessageBox.Show("Bus Connected!!");
                this.isShowFB = true;
                if ((false == this.isGoHomeWhenBoot) && (true == isTesting))
                {
                    this.Task_MotionFeedBack();
                    this.motionStatusTimer.Start();
                }
            }
            else
            {
                //FunctionFail = false;
                MessageBox.Show("Connect Fail!!");
            }
        }
        private void busConnectButton_Click(object sender, EventArgs e)
        {
            this.BusConnect(true);
            if (false == FunctionFail)
            {
                ((Button)sender).Enabled = false;
            }
        }
        // Disconnect
        private delegate void ClearMotionStatusIOHandler(Control Object, Color BackColor);
        private void ClearMotionStatusIO(Control Object, Color BackColor)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ClearMotionStatusIOHandler(ClearMotionStatusIO), Object, BackColor);
            }
            else
            {
                Object.BackColor = BackColor;
            }
        }
        private void BusDisconnect()
        {
            int AxisNo = this.CardID;
            if (true == this.motionStatusTimer.Enabled)
            {
                if (true == this.axis_status.Mio_SVON)
                {
                    this.ServoControl(OFF, true, 0);
                }
                this.motionStatusTimer.Stop();
            }

            #region Clear motion status I/O
            this.ClearMotionStatusIO(this.hmvPictureBox, SystemColors.Control);
            this.ClearMotionStatusIO(this.smvPictureBox, SystemColors.Control);
            this.ClearMotionStatusIO(this.svonPictureBox, SystemColors.Control);
            this.ClearMotionStatusIO(this.emgPictureBox, SystemColors.Control);
            this.ClearMotionStatusIO(this.pelPictureBox, SystemColors.Control);
            this.ClearMotionStatusIO(this.melPictureBox, SystemColors.Control);
            this.ClearMotionStatusIO(this.orgPictureBox, SystemColors.Control);
            this.ClearMotionStatusIO(this.almPictureBox, SystemColors.Control);
            #endregion

            Function_Result(APS168.APS_stop_field_bus(CardID, BusNo));
            this.isShowFB = false;
        }
        private void busDisconnectButton_Click(object sender, EventArgs e)
        {
            this.BusDisconnect();
            this.busConnectButton.Enabled = true;
        }
        // Servo On & Servo Off
        private void ServoControl(int status, bool isTesting, int card_id)
        {
            int AxisNo;
            if (true == isTesting)
            {
                AxisNo = this.CardID;
            }
            else
            {
                AxisNo = card_id;
            }
            Function_Result(APS168.APS_emg_stop(AxisNo));
            Function_Result(APS168.APS_set_servo_on(AxisNo, status));
        }
        private void servoOnButton_Click(object sender, EventArgs e)
        {
            this.ServoControl(ON, true, 0);
            if (false == FunctionFail)
            {
                ((Button)sender).Enabled = false;
            }
        }
        private void servoOffButton_Click(object sender, EventArgs e)
        {
            this.ServoControl(OFF, true, 0);
            this.servoOnButton.Enabled = true;
        }
        // Absolute Move
        private void absoluteMoveButton_Click(object sender, EventArgs e)
        {
            // depend on Pos = this.absolutePosTextBox

            //輸出絕對位置
            double Pos = 0.0;
            int SpeedPercent = 0;
            int AxisNo = this.CardID;

            if (double.TryParse(this.absolutePosTextBox.Text, out Pos) && int.TryParse(this.speedPercentageComboBox.Text, out SpeedPercent))
            {
                //實際距離換算成脈波數
                //int myPosPulse = (int)(Pos / mmPerPulse);
                //將速度百分比換算成實際脈波數
                //int mySpeedPulse = (int)(MaxSpeedPulse * SpeedPercent / 100);
                //絕對移動
                this.AbsMove(AxisNo, Pos, SpeedPercent);
            }
            else
            {
                MessageBox.Show("請填入正確數字");
            }
        }
        // Reset Counter
        private void resetCounterButton_Click(object sender, EventArgs e)
        {
            int AxisNo = this.CardID;
            Function_Result(APS168.APS_set_command(AxisNo, 0));
            Function_Result(APS168.APS_set_position(AxisNo, 0));
        }
        // Jog
        private void manualJog(int distance)    // for testing
        {
            //輸出相對位置 (mm)
            int Distance = distance;
            int SpeedPercent = 0;
            int AxisNo = this.CardID;

            if (int.TryParse(this.speedPercentageComboBox.SelectedItem.ToString(), out SpeedPercent))
            {
                ////實際距離換算成脈波數
                //int myDistancePulse = (int)(myDistance / mmPerPulse);
                ////將速度百分比換算成實際脈波數
                //int mySpeedPulse = (int)(MaxSpeedPulse * mySpeedPercent / 100);
                //相對移動
                RelMove(AxisNo, Distance, SpeedPercent,true,0);
            }
            else
            {
                MessageBox.Show("請填入正確數字");
            }
        }
        private void relativeJogPlusButton_Click(object sender, EventArgs e)
        {
            this.manualJog(int.Parse(this.absolutePosTextBox.Text.ToString()));     // positive
        }
        private void relativeJogMinusButton_Click(object sender, EventArgs e)
        {
            this.manualJog(-int.Parse(this.absolutePosTextBox.Text.ToString()));     // negative
        }
        // Unlock for Engineer
        private void motionUnlockButton_Click(object sender, EventArgs e)
        {
            if ("Lock" == this.motionUnlockButton.Text)
            {
                if (true == this.motionStatusTimer.Enabled && true == axis_status.Mio_SVON)
                {
                    this.servoOffButton.PerformClick();
                }
                this.busDisconnectButton.PerformClick();
                this.closeMotionCardButton.PerformClick();

                this.editMotionPanel.Enabled = false;
                this.motionUnlockButton.Text = "Unlock";
            }
            else if ("Unlock" == this.motionUnlockButton.Text)
            {
                this.editMotionPanel.Enabled = true;

                this.motionUnlockButton.Text = "Lock";
            }

        }

        // Motion Status & I/O Status
        private void PaintMotionStatus(object sender, bool status)
        {
            if (true == status)
            {
                ((PictureBox)sender).BackColor = Color.Lime;
            }
            else
            {
                ((PictureBox)sender).BackColor = SystemColors.Control;
            }
        }
        private void motionStatusDetection()
        {
            // motion status
            int AxisNo = this.CardID;

            /////////////////////////////////////////////////////////////////////
            //
            // ==========================MotionStatus===========================
            //
            /////////////////////////////////////////////////////////////////////
            Int32 MotionSts = APS168.APS_motion_status(AxisNo);
            //將整數轉換成位元組陣列
            byte[] BytesArray_MST = System.BitConverter.GetBytes(MotionSts);
            //將位元組陣列轉換成
            System.Collections.BitArray Temp_MST = new System.Collections.BitArray(BytesArray_MST);

            bool[] BitArray_MST = new bool[32];
            for (int i = 0; i < Temp_MST.Length; i++)
            {
                BitArray_MST[i] = Temp_MST[i];
            }

            //HMV  歸原點是否移動中
            if (BitArray_MST[(int)APS_Define.MTS_HMV] == true)
            { this.axis_status.Mts_HMV = true; }
            else
            { this.axis_status.Mts_HMV = false; }
            this.PaintMotionStatus(this.hmvPictureBox, this.axis_status.Mts_HMV);
            //SMV  單軸是否移動中
            if (BitArray_MST[(int)APS_Define.MTS_SMV] == true)
            { this.axis_status.Mts_SMV = true; }
            else
            { this.axis_status.Mts_SMV = false; }
            this.PaintMotionStatus(this.smvPictureBox, this.axis_status.Mts_SMV);

            /////////////////////////////////////////////////////////////////////
            //
            // ==========================Motion IO Status========================
            //
            /////////////////////////////////////////////////////////////////////
            Int32 IoSts = APS168.APS_motion_io_status(AxisNo);
            //將整數轉換成位元組陣列
            byte[] BytesArray_MIO = System.BitConverter.GetBytes(IoSts);
            //將位元組陣列轉換成位元陣列
            System.Collections.BitArray Temp_MIO = new System.Collections.BitArray(BytesArray_MIO);

            bool[] myBitArray_MIO = new bool[32];
            for (int i = 0; i < Temp_MIO.Length; i++)
            {
                myBitArray_MIO[i] = Temp_MIO[i];
            }

            //ALM  異常是否發生
            if (myBitArray_MIO[(int)APS_Define.MIO_ALM] == true)
            { this.axis_status.Mio_ALM = true; }
            else
            { this.axis_status.Mio_ALM = false; }
            this.PaintMotionStatus(this.almPictureBox, this.axis_status.Mio_ALM);
            //PEL  正極限是否觸發
            if (myBitArray_MIO[(int)APS_Define.MIO_PEL] == true)
            { this.axis_status.Mio_PEL = true; }
            else
            { this.axis_status.Mio_PEL = false; }
            this.PaintMotionStatus(this.pelPictureBox, this.axis_status.Mio_PEL);
            //MEL  負極限是否觸發
            if (myBitArray_MIO[(int)APS_Define.MIO_MEL] == true)
            { this.axis_status.Mio_MEL = true; }
            else
            { this.axis_status.Mio_MEL = false; }
            this.PaintMotionStatus(this.melPictureBox, this.axis_status.Mio_MEL);
            //ORG  原點是否觸發
            if (myBitArray_MIO[(int)APS_Define.MIO_ORG] == true)
            { this.axis_status.Mio_ORG = true; }
            else
            { this.axis_status.Mio_ORG = false; }
            this.PaintMotionStatus(this.orgPictureBox, this.axis_status.Mio_ORG);
            //EMG  急停是否觸發
            if (myBitArray_MIO[(int)APS_Define.MIO_EMG] == true)
            { this.axis_status.Mio_EMG = true; }
            else
            { this.axis_status.Mio_EMG = false; }
            this.PaintMotionStatus(this.emgPictureBox, this.axis_status.Mio_EMG);
            //SVON  是否ServoOn
            if (myBitArray_MIO[(int)APS_Define.MIO_SVON] == true)
            { this.axis_status.Mio_SVON = true; }
            else
            { this.axis_status.Mio_SVON = false; }
            this.PaintMotionStatus(this.svonPictureBox, this.axis_status.Mio_SVON);

            /////////////////////////////////////////////////////////////////////
            //
            // ==========================General DIO========================
            //
            /////////////////////////////////////////////////////////////////////
            Int32 DI_Value = 0;
            APS168.APS_get_field_bus_d_input(CardID, BusNo, 0, ref DI_Value);
            this.showDIDataTextBox.Text = DI_Value.ToString("x");
        }
        private void motionStatusTimer_Tick(object sender, EventArgs e)
        {
            this.motionStatusDetection();
        }

        #region Motion Feedback Command & Position
        private bool isShowFB = false;
        private delegate void ShowMotionFBHandler(Control Object, string Text);
        private void ShowMotionFB(Control Object, string Text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ShowMotionFBHandler(ShowMotionFB), Object, Text);
            }
            else
            {
                Object.Text = Text;
            }
        }       
        private void Task_MotionFeedBack()
        {
            int AxisNo = 0;
            double fb_command = 0.0, fb_position = 0.0;
            Int32 CommandPulse = 0;
            Int32 PositionPulse = 0;

            var task_time = Task.Factory.StartNew(() =>
            {
                while (true == this.isShowFB)
                {
                    AxisNo = this.CardID;
                    //取值(FeedBack Data)
                    //回饋的命令脈波位置  CommandPulse
                    APS168.APS_get_command(AxisNo, ref CommandPulse);
                    fb_command = CommandPulse * this.mmPerPulse;
                    //脈波位置  Pulse
                    APS168.APS_get_position(AxisNo, ref PositionPulse);
                    fb_position = PositionPulse * this.mmPerPulse;

                    this.ShowMotionFB(this.fbCommandLabel, fb_command.ToString("#0.000"));
                    this.ShowMotionFB(this.fbPositionLabel, fb_position.ToString("#0.000"));
                    Thread.Sleep(50);
                }
            });
        }
        #endregion

        // Stop
        private void motionStopButton_Click(object sender, EventArgs e)
        {
            int AxisNo = this.CardID;
            if (APS168.APS_emg_stop(AxisNo) != 0)
            {
                MessageBox.Show("急停錯誤");
            }
        }
        // Homing -- Not regular go home step.
        private void motionHomeButton_Click(object sender, EventArgs e)
        {
            //int AxisNo = this.CardID;
            //if (APS168.APS_home_move(AxisNo) != 0)
            //{
            //    MessageBox.Show("Homing 錯誤！");
            //}
        }
        #endregion

        #region Product
        private PRODUCT current_product = PRODUCT.None;
        public CProductInfo product_info = null;
        private delegate void showBasicLedCoordinateHandler(Object Object, string text);
        public void showBasicLedCoordinate(Object Object, string text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new showBasicLedCoordinateHandler(showBasicLedCoordinate), Object, text);
            }
            else
            {
                ((Label)Object).Text = text;
            }
        }
        private void ShowSGSLedCoordinate()
        {
            this.showBasicLedCoordinate(this.basicLed1XLabel, this.product_info.SGS_Led.x1.ToString());
            this.showBasicLedCoordinate(this.basicLed1YLabel, this.product_info.SGS_Led.y1.ToString());
            this.showBasicLedCoordinate(this.basicLed2XLabel, this.product_info.SGS_Led.x2.ToString());
            this.showBasicLedCoordinate(this.basicLed2YLabel, this.product_info.SGS_Led.y2.ToString());
        }

        private bool isStartProgressBar = false;
        private delegate void showProgressBarHandler(Object Object, int value);
        public void showProgressBar(Object Object, int value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new showProgressBarHandler(showProgressBar), Object, value);
            }
            else
            {
                ((ToolStripProgressBar)Object).Value = value;
            }
        }
        private void Task_ProgressBar()
        {
            var task_time = Task.Factory.StartNew(() =>
            {
                int value = 0;
                this.showProgressBar(this.toolStripProgressBar1, value);
                while (true == this.isStartProgressBar)
                {
                    value += 4;
                    this.showProgressBar(this.toolStripProgressBar1, value);
                    Thread.Sleep(100);
                }
                value = 1000;
                this.showProgressBar(this.toolStripProgressBar1, value);
            });
        }
        private string vpp_file_path_product = string.Empty, vpp_file_path_golden_sample = string.Empty, vpp_file_path_draw_line = string.Empty;
        private void SelectRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (true == ((RadioButton)sender).Checked)
            {
                this.product_info = null;

                switch (((RadioButton)sender).Name)
                {
                    case "selectDrlLeftRadioButton":
                        this.current_product = PRODUCT.DRL_L;
                        this.vpp_file_path_product = "..\\..\\vpp\\DRL_L\\DRL.vpp";
                        this.vpp_file_path_golden_sample = "..\\..\\vpp\\DRL_L\\gs_DRL.vpp";
                        this.vpp_file_path_draw_line = "..\\..\\vpp\\DRL_L\\draw_line.vpp";
                        break;
                    case "selectDrlRightRadioButton":
                        this.current_product = PRODUCT.DRL_R;
                        this.vpp_file_path_product = "..\\..\\vpp\\DRL_R\\DRL.vpp";
                        this.vpp_file_path_golden_sample = "..\\..\\vpp\\DRL_R\\gs_DRL.vpp";
                        this.vpp_file_path_draw_line = "..\\..\\vpp\\DRL_R\\draw_line.vpp";
                        break;
                    case "selectLowPowerLeftRadioButton":
                        this.current_product = PRODUCT.LOWPOWER_L;
                        this.vpp_file_path_product = string.Empty;
                        this.vpp_file_path_golden_sample = string.Empty;
                        this.vpp_file_path_draw_line = string.Empty;
                        break;
                    case "selectLowPowerRightRadioButton":
                        this.current_product = PRODUCT.LOWPOWER_R;
                        this.vpp_file_path_product = string.Empty;
                        this.vpp_file_path_golden_sample = string.Empty;
                        this.vpp_file_path_draw_line = string.Empty;
                        break;
                    case "selectDrlVaveLeftRadioButton":
                        this.current_product = PRODUCT.DRLVAVE_L;
                        this.vpp_file_path_product = "..\\..\\vpp\\VAVE_L\\vave_20140810_1.vpp";
                        this.vpp_file_path_golden_sample = "..\\..\\vpp\\VAVE_L\\gs_vave_20140809.vpp";
                        this.vpp_file_path_draw_line = "..\\..\\vpp\\VAVE_L\\draw_line.vpp";
                        break;
                    case "selectDrlVaveRightRadioButton":
                        this.current_product = PRODUCT.DRLVAVE_R;
                        this.vpp_file_path_product = "..\\..\\vpp\\VAVE_R\\vave_20140810_1.vpp";
                        this.vpp_file_path_golden_sample = "..\\..\\vpp\\VAVE_R\\gs_vave_20140809.vpp";
                        this.vpp_file_path_draw_line = "..\\..\\vpp\\VAVE_R\\draw_line.vpp";
                        break;
                    default:
                        this.current_product = PRODUCT.None;
                        this.vpp_file_path_product = string.Empty;
                        this.vpp_file_path_golden_sample = string.Empty;
                        this.vpp_file_path_draw_line = string.Empty;
                        break;
                }

                if (PRODUCT.None != this.current_product)
                {
                    if (string.Empty == this.vpp_file_path_golden_sample || string.Empty == this.vpp_file_path_product || string.Empty == this.vpp_file_path_draw_line)
                    {
                        MessageBox.Show("查無檔案，請重新確認！");
                    }
                    else
                    {
                        this.product_info = new CProductInfo(this.current_product);

                        this.isStartProgressBar = true;
                        this.Task_ProgressBar();
                        this.Task_LoadVpp();
                    }
                }
                this.currentProductToolStripStatusLabel.Text = ((RadioButton)sender).Text;
            }
        }
        private void Task_LoadVpp()
        {
            var task_time = Task.Factory.StartNew(() =>
            {
                this.LoadGoldenSampleVpp();
                this.LoadProductVpp();
                this.LoadProductDrawVpp();
                this.isStartProgressBar = false;

                this.ShowSGSLedCoordinate();
            });
        }

        private CogToolBlock productCogToolBlock;
        private void LoadProductVpp()
        {
            this.productCogToolBlock = new CogToolBlock();

            //this.vpp_file_path_product = "..\\..\\vpp\\vave_20140810_1.vpp";
            string fullPath = Path.GetFullPath(this.vpp_file_path_product);
            // 檢查檔案存歿
            if (true == File.Exists(fullPath))
            {
                // 有
                string file_path = string.Concat(fullPath);    // 讀取檔案路徑
                this.productCogToolBlock = (CogToolBlock)CogSerializer.LoadObjectFromFile(file_path);     //讀取ToolBlock                
            }
            else
            {
                //沒有
                MessageBox.Show(fullPath + " 檔案不存在！", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //use delegate:  this.SelectNoneRadioButton.Checked = true;
            }
        }
        private void LoadGoldenSampleVpp()
        {
            CogToolBlock goldneSampleCogToolBlock = new CogToolBlock();

            //this.vpp_file_path_golden_sample = "..\\..\\vpp\\gs_vave_20140809.vpp";
            string fullPath = Path.GetFullPath(this.vpp_file_path_golden_sample);
            // 檢查檔案存歿
            if (true == File.Exists(fullPath))
            {
                // 有
                string file_path = string.Concat(fullPath);    // 讀取檔案路徑
                goldneSampleCogToolBlock = (CogToolBlock)CogSerializer.LoadObjectFromFile(file_path);     //讀取ToolBlock

                this.product_info.Basic_Led.x1 = (double)goldneSampleCogToolBlock.Outputs["Led_1_CenterX"].Value ;//- this.product_info.Basic_Led.translation_x;
                this.product_info.Basic_Led.y1 = (double)goldneSampleCogToolBlock.Outputs["Led_1_CenterY"].Value ;//- this.product_info.Basic_Led.translation_y;
                this.product_info.Basic_Led.x2 = (double)goldneSampleCogToolBlock.Outputs["Led_2_CenterX"].Value ;//- this.product_info.Basic_Led.translation_x;
                this.product_info.Basic_Led.y2 = (double)goldneSampleCogToolBlock.Outputs["Led_2_CenterY"].Value; //-this.product_info.Basic_Led.translation_y;
            }
            else
            {
                //沒有
                MessageBox.Show(fullPath + " 檔案不存在！", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //use delegate:  this.SelectNoneRadioButton.Checked = true;
            }

            //// 關閉 cognex 使用
            //Cognex.VisionPro.CogFrameGrabbers framegrabbers = new Cognex.VisionPro.CogFrameGrabbers();
            //foreach (Cognex.VisionPro.ICogFrameGrabber fg in framegrabbers)
            //    fg.Disconnect(false);
        }
        private void LoadProductDrawVpp()
        {
            //this.vpp_file_path_draw_line = "..\\..\\vpp\\draw_line.vpp";
            string fullPath = Path.GetFullPath(this.vpp_file_path_draw_line);
            // 檢查檔案存歿
            if (true == File.Exists(fullPath))
            {
                // 有
                string file_path = string.Concat(fullPath);    // 讀取檔案路徑
                this.showCogToolBlock = (CogToolBlock)CogSerializer.LoadObjectFromFile(file_path);     //讀取ToolBlock                
            }
            else
            {
                //沒有
                MessageBox.Show(fullPath + " 檔案不存在！", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //use delegate:  this.SelectNoneRadioButton.Checked = true;
            }
        }

        private void showPassNgPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (true == this.isShowPassPanel)
            {
                if (NOT == this.in_isProductOK)
                {
                    this.showPassNgPanel.BackColor = Color.Red;
                    g.DrawString("NG", new Font(FontFamily.GenericSerif, 36f, FontStyle.Regular), new SolidBrush(SystemColors.ControlDarkDark),
                         new Rectangle(0, 0, ((Panel)sender).Width - 2, ((Panel)sender).Height - 2),
                         new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                }
                else if (YES == this.in_isProductOK)
                {
                    this.showPassNgPanel.BackColor = Color.Green;
                    g.DrawString("Pass", new Font(FontFamily.GenericSerif, 36f, FontStyle.Regular), new SolidBrush(SystemColors.Control),
                        new Rectangle(0, 0, ((Panel)sender).Width - 2, ((Panel)sender).Height - 2),
                        new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                }
            }
            else
            {
                this.showPassNgPanel.BackColor = SystemColors.Control;
                g.DrawString("Wait!", new Font(FontFamily.GenericSerif, 36f, FontStyle.Regular), new SolidBrush(Color.Black),
                    new Rectangle(0, 0, ((Panel)sender).Width - 2, ((Panel)sender).Height - 2),
                    new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }
        }
        private void showLiveProductDataTimer_Tick(object sender, EventArgs e)
        {
            this.ShowProcessMsg(this.process_msg);

            switch (this.process_msg)
            {
                case PROCESSMSG.GoHomeFirst:
                case PROCESSMSG.PutProductAndClickPhysicalButton:
                case PROCESSMSG.ClickPhysicalButtonAfterLockScrew:
                    break;
                case PROCESSMSG.FirstInspection:
                case PROCESSMSG.CCDGoHomeManual:
                case PROCESSMSG.CCDGoHomeAuto:
                case PROCESSMSG.ResultOK:
                case PROCESSMSG.ResultNG:
                case PROCESSMSG.SecondInspection:
                    {
                        CProductInfo _product_info = this.product_info;
                        bool isLiveData = this.isShowLiveData;

                        this.productBarcodeLabel.Text = _product_info.barcode;
                        this.productLed1XLabel.Text = _product_info.ShowSGS_Led.x1.ToString("#0.00");
                        this.productLed1YLabel.Text = _product_info.ShowSGS_Led.y1.ToString("#0.00");
                        this.productLed2XLabel.Text = _product_info.ShowSGS_Led.x2.ToString("#0.00");
                        this.productLed2YLabel.Text = _product_info.ShowSGS_Led.y2.ToString("#0.00");
                        this.productResultLabel.Text = _product_info.inspection_result;
                        //this.productTimeLabel.Text = _product_info.inspection_time;

                        if (false == isLiveData)
                        {
                            //int row = this.productInfoDataGridView.CurrentCell.RowIndex;
                            int row = this.product_num;

                            this.productInfoDataGridView[(int)ITEM.BARCODE, row].Value = _product_info.barcode;
                            this.productInfoDataGridView[(int)ITEM.LED1X, row].Value = _product_info.ShowSGS_Led.x1.ToString("#0.000");
                            this.productInfoDataGridView[(int)ITEM.LED1Y, row].Value = _product_info.ShowSGS_Led.y1.ToString("#0.000");
                            this.productInfoDataGridView[(int)ITEM.LED2X, row].Value = _product_info.ShowSGS_Led.x2.ToString("#0.000");
                            this.productInfoDataGridView[(int)ITEM.LED2Y, row].Value = _product_info.ShowSGS_Led.y2.ToString("#0.000");
                            this.productInfoDataGridView[(int)ITEM.RESULT, row].Value = _product_info.inspection_result;
                            this.productInfoDataGridView[(int)ITEM.TIME, row].Value = _product_info.inspection_time_12;

                            this.product_num++;
                            this.productInfoDataGridView.Rows.Insert(this.product_num, 1);
                            this.productInfoDataGridView.Rows[this.product_num].Selected = true;


                            this.saveFile.CombineContentText(_product_info.barcode, _product_info.inspection_time_12,
                                _product_info.ShowSGS_Led.x1.ToString("#0.000"),
                                _product_info.ShowSGS_Led.y1.ToString("#0.000"),
                                _product_info.ShowSGS_Led.x2.ToString("#0.000"),
                                _product_info.ShowSGS_Led.y2.ToString("#0.000"));
                            this.saveFile.WriteMES(_product_info.barcode, _product_info.inspection_time_24, "000");

                            this.ShowOkNumCalculate();
                            isShowLiveData = true;
                        }
                    }
                    break;
                default:
                    break;
            }

        }

        private void ShowOkNumCalculate()
        {
            this.total_num++;
            if ("OK" == this.product_info.inspection_result)
            { 
                this.ok_num++;
            }
            this.ok_percentage = this.ok_num/this.total_num;

            this.totalNumLabel.Text = this.total_num.ToString("#0");
            this.okNumLabel.Text = this.ok_num.ToString("#0");
            this.okPercentageLabel.Text = this.ok_percentage.ToString("#0.00");
        }

        private void productNumAsZerobutton_Click(object sender, EventArgs e)
        {
            //this.in_isInGoldenSampleRegion = YES;
            this.total_num = 0.0;
            this.ok_num = 0.0;
            this.ok_percentage = 0.0;

            this.totalNumLabel.Text = this.total_num.ToString("#0");
            this.okNumLabel.Text = this.ok_num.ToString("#0");
            this.okPercentageLabel.Text = this.ok_percentage.ToString("#0.00");
        }
        #endregion

        #region Operate Process
        private void ShowProcessMsg(PROCESSMSG msg)
        {
            switch (msg)
            {
                case PROCESSMSG.GoHomeFirst:
                    this.processMsgLabel.Text = "返回原點中，請稍後！";
                    break;
                case PROCESSMSG.PutProductAndClickPhysicalButton:
                    this.processMsgLabel.Text = "請放入產品，完成後按下啟動鈕！";
                    break;
                case PROCESSMSG.FirstInspection:
                    this.processMsgLabel.Text = "第一次檢測中！";
                    break;
                case PROCESSMSG.CCDGoHomeManual:
                    this.processMsgLabel.Text = "手動回原點中，請稍後！";
                    break;
                case PROCESSMSG.CCDGoHomeAuto:
                    this.processMsgLabel.Text = "返回原點中，請稍後！";
                    break;
                case PROCESSMSG.ResultOK:
                    this.processMsgLabel.Text = "合格產品！";
                    break;
                case PROCESSMSG.ResultNG:
                    this.processMsgLabel.Text = "不合格產品！";
                    break;
                case PROCESSMSG.ClickPhysicalButtonAfterLockScrew:
                    this.processMsgLabel.Text = "請鎖上兩顆螺絲，完成後按下啟動鈕！";
                    break;
                case PROCESSMSG.SecondInspection:
                    this.processMsgLabel.Text = "第二次檢測中！";
                    break;
                default:
                    break;
            }
        }

        private Thread OperationThread;
        private bool isThreadStart = false;
        private void OperationThreadRun()
        {
            while (false == this.isShutDown)
            {
                if ((int)MAINTAB.OPERATOR == this.current_main_tab)
                {
                    this.isThreadStart = true;
                    // motion & CCD start
                    this.InitializeMotionControl(false, 0);
                    this.BusConnect(false);
                    this.ServoControl(ON, false, 0);        // default card id = 0;

                    while (true == isThreadStart)
                    {
                        this.RunInput();
                        this.RunProcess();
                        this.RunOutput();

                        System.Threading.Thread.Sleep(20);
                    }

                    // motion & CCD stop
                    this.ServoControl(OFF, false, 0);       // default card id = 0;
                    this.BusDisconnect();
                    this.CloseMotionCard();
                }
                System.Threading.Thread.Sleep(500);
                Console.WriteLine("Out of OPERATOR");
            }
            Console.WriteLine("THREAD STOP");
        }

        private PROCESSMSG process_msg = PROCESSMSG.GoHomeFirst;

        private bool isShowPassPanel = false;

        private Int32 in_isHomeNow = NOT;
        private Int32 in_isHomeSearching = NOT;
        private Int32 in_isStartSignal = OFF;
        private Int32 in_isInInspectPosition = OFF;
        private Int32 in_isInZeroPosition = OFF;
        private Int32 in_isProductOK = NOT;
        private Int32 in_isStartInspection = OFF;
        private Int32 in_isHomingManual = OFF;
        private Int32 in_isFirstInspection = NOT;
        private Int32 in_isInGoldenSampleRegion = NOT;

        private Int32 virturl_IO = OFF;
        private void showPassNgPanel_Click(object sender, EventArgs e)
        {
            this.virturl_IO = ON;
        }
        private int count = 0;
        private bool mySleep(int max)
        {
            bool ret = false;
            if (max >= this.count)
            {
                this.count++;
            }
            else
            {
                ret = true;
            }
            return ret;
        }
        private void RunInput()
        {
            Int32 DI_Value = 0;
            Int32 PositionPulse = 0;
            Int32 MotionSts = 0;
          
            switch (this.process_msg)
            {
                case PROCESSMSG.GoHomeFirst:                    
                        byte[] BytesArray_MIO;
                        System.Collections.BitArray Temp_MIO;
                        bool[] BitArray_MIO = new bool[32];
                    BytesArray_MIO = System.BitConverter.GetBytes(APS168.APS_motion_io_status(0)); //將整數轉換成位元組陣列 
                    Temp_MIO = new System.Collections.BitArray(BytesArray_MIO);  //將位元組陣列轉換成位元陣列
                    BitArray_MIO[(int)APS_Define.MIO_MEL] = Temp_MIO[(int)APS_Define.MIO_MEL];
                    //MEL  負極限是否觸發
                    if (BitArray_MIO[(int)APS_Define.MIO_MEL] == true)
                    {
                        this.in_isHomeNow = YES;
                    }
                    break;
                case PROCESSMSG.PutProductAndClickPhysicalButton:
                    // wait physical button input
                    APS168.APS_get_field_bus_d_input(CardID, BusNo, 0, ref DI_Value);
                    this.in_isStartSignal = DI_Value;   // i = ON, 0 = OFF

                    //if (this.virturl_IO == ON)
                    //{
                    //    this.in_isStartSignal = ON;
                    //    this.virturl_IO = OFF;
                    //}
                    //else
                    //{
                    //    this.in_isStartSignal = OFF;
                    //}
                    PositionPulse = 0;
                    this.in_isInGoldenSampleRegion = NOT;
                    this.in_isStartInspection = OFF;
                    break;
                case PROCESSMSG.FirstInspection:
                    // wait CCD go out position, check position = 165000 & smv = false
                    if (OFF == this.in_isInInspectPosition)
                    {
                        APS168.APS_get_position(0, ref PositionPulse);

                        MotionSts = APS168.APS_motion_status(0);
                        byte[] BytesArray_MST;
                        System.Collections.BitArray Temp_MST;
                        bool[] BitArray_MST = new bool[32];
                        BytesArray_MST = System.BitConverter.GetBytes(MotionSts);
                        Temp_MST = new System.Collections.BitArray(BytesArray_MST);
                        BitArray_MST[(int)APS_Define.MTS_SMV] = Temp_MST[(int)APS_Define.MTS_SMV];

                        if (165000 == PositionPulse && false == BitArray_MST[(int)APS_Define.MTS_SMV])
                        {
                            if (true == this.mySleep(10))
                            {
                                this.in_isInInspectPosition = ON;
                                this.in_isStartInspection = ON;
                                this.count = 0;
                            }
                        }
                        else
                        {
                            this.in_isInGoldenSampleRegion = NOT;
                        }
                    }

                    // wait CCD go home manual.
                    if (true == isHomingManual)
                    {
                        this.in_isHomingManual = ON;
                        this.isHomingManual = false;
                    }
                    break;
                case PROCESSMSG.CCDGoHomeManual:
                    // wait ORG signal
                    if (OFF == this.in_isInInspectPosition)
                    {
                        APS168.APS_get_position(0, ref PositionPulse);

                        MotionSts = APS168.APS_motion_status(0);
                        byte[] BytesArray_MST;
                        System.Collections.BitArray Temp_MST;
                        bool[] BitArray_MST = new bool[32];
                        BytesArray_MST = System.BitConverter.GetBytes(MotionSts);
                        Temp_MST = new System.Collections.BitArray(BytesArray_MST);
                        BitArray_MST[(int)APS_Define.MTS_SMV] = Temp_MST[(int)APS_Define.MTS_SMV];

                        if (0 == PositionPulse && false == BitArray_MST[(int)APS_Define.MTS_SMV])
                        {
                            this.in_isInZeroPosition = ON;
                            this.in_isHomingManual = OFF;
                        }
                    }
                    break;
                case PROCESSMSG.CCDGoHomeAuto:
                    // wait ORG signal
                    if (OFF == this.in_isInInspectPosition)
                    {
                        APS168.APS_get_position(0, ref PositionPulse);
                        byte[] BytesArray_MST;
                        System.Collections.BitArray Temp_MST;
                        bool[] BitArray_MST = new bool[32];
                        MotionSts = APS168.APS_motion_status(0);
                        BytesArray_MST = System.BitConverter.GetBytes(MotionSts);
                        Temp_MST = new System.Collections.BitArray(BytesArray_MST);
                        BitArray_MST[(int)APS_Define.MTS_SMV] = Temp_MST[(int)APS_Define.MTS_SMV];

                        if (0 == PositionPulse && false == BitArray_MST[(int)APS_Define.MTS_SMV])
                        {
                            this.in_isInZeroPosition = ON;
                            this.in_isHomingManual = OFF;
                        }
                    }
                    break;
                case PROCESSMSG.ResultOK:
                    // product is ok, do nothing
                    break;
                case PROCESSMSG.ResultNG:
                    // product is ng, do nothing
                    break;
                case PROCESSMSG.ClickPhysicalButtonAfterLockScrew:
                    // wait physical button input
                    APS168.APS_get_field_bus_d_input(CardID, BusNo, 0, ref DI_Value);
                    this.in_isStartSignal = DI_Value;   // i = ON, 0 = OFF

                    //if (this.virturl_IO == ON)
                    //{
                    //    this.in_isStartSignal = ON;
                    //    this.virturl_IO = OFF;
                    //}
                    //else
                    //{
                    //    this.in_isStartSignal = OFF;
                    //}

                    this.in_isInGoldenSampleRegion = NOT;
                    PositionPulse = 0;
                    this.in_isStartInspection = OFF;
                    break;
                case PROCESSMSG.SecondInspection:
                    // wait CCD go out position, check position = 165000 & smv = false
                    if (OFF == this.in_isInInspectPosition)
                    {
                        APS168.APS_get_position(0, ref PositionPulse);
                        byte[] BytesArray_MST;
                        System.Collections.BitArray Temp_MST;
                        bool[] BitArray_MST = new bool[32];
                        MotionSts = APS168.APS_motion_status(0);
                        BytesArray_MST = System.BitConverter.GetBytes(MotionSts);
                        Temp_MST = new System.Collections.BitArray(BytesArray_MST);
                        BitArray_MST[(int)APS_Define.MTS_SMV] = Temp_MST[(int)APS_Define.MTS_SMV];

                        if (165000 == PositionPulse && false == BitArray_MST[(int)APS_Define.MTS_SMV])
                        {
                            if (true == this.mySleep(10))
                            {
                                this.in_isInInspectPosition = ON;
                                this.in_isStartInspection = ON;
                                this.count = 0;
                            }
                        }
                        else
                        {
                            this.in_isInGoldenSampleRegion = NOT;
                        }
                    }

                    // wait CCD go home manual.
                    if (true == isHomingManual)
                    {
                        this.in_isHomingManual = ON;
                        this.isHomingManual = false;
                    }
                    break;
                default:
                    break;
            }
        }
        private void RunProcess()
        {
            switch (this.process_msg)
            {
                case PROCESSMSG.GoHomeFirst:
                    if (NOT == this.in_isHomeNow && NOT == this.in_isHomeSearching)
                    {
                        this.RelMove(0, -200, 100, false, 10000);    // negative, unit:mm
                        this.in_isHomeSearching = YES;
                    }
                    break;
                case PROCESSMSG.PutProductAndClickPhysicalButton:
                    break;
                case PROCESSMSG.FirstInspection:
                    // if CCD is in outside position
                    // Do Vision inspection
                    if (ON == this.in_isStartInspection)
                    {
                        //this.product_info;
                        //this.in_isProductOK = NOT;
                        this.ImageProcess();
                        this.in_isInGoldenSampleRegion = this.CheckLedCoordinate();
                    }
                    // check whether product result = OK or NG
                    // set image LED data and result.

                    // if CCD go home manual
                    if (ON == this.in_isHomingManual)
                    {
                        // stop Vision inspection
                    }
                    break;
                case PROCESSMSG.CCDGoHomeManual:
                    break;
                case PROCESSMSG.CCDGoHomeAuto:
                    break;
                case PROCESSMSG.ResultOK:
                    break;
                case PROCESSMSG.ResultNG:
                    break;
                case PROCESSMSG.ClickPhysicalButtonAfterLockScrew:
                    break;
                case PROCESSMSG.SecondInspection:
                    // if CCD is in outside position
                    // Do Vision inspection
                    if (ON == this.in_isStartInspection)
                    {
                        //this.product_info;
                        //this.in_isProductOK = NOT; 
                        this.ImageProcess();
                        this.in_isInGoldenSampleRegion = this.CheckLedCoordinate();
                    }
                    // check whether product result = OK or NG
                    // set image LED data and result.

                    // if CCD go home manual
                    if (ON == this.in_isHomingManual)
                    {
                        // stop Vision inspection
                    }
                    break;
                default:
                    break;
            }
        }
        bool isShowLiveData = true;
        private void RunOutput()
        {
            switch (this.process_msg)
            {
                case PROCESSMSG.GoHomeFirst:
                    if (YES == this.in_isHomeNow)
                    {
                        Function_Result(APS168.APS_set_command(0, 0));
                        Function_Result(APS168.APS_set_position(0, 0));
                        // set msg
                        this.process_msg = PROCESSMSG.PutProductAndClickPhysicalButton;
                        this.in_isHomeSearching = NOT;
                    }
                    break;
                case PROCESSMSG.PutProductAndClickPhysicalButton:
                    if (ON == this.in_isStartSignal)
                    {
                        // motion go out position
                        this.AbsMove(0, 165, 80);      // 165mm = 165000 pulse

                        // set msg
                        this.process_msg = PROCESSMSG.FirstInspection;
                        this.in_isFirstInspection = YES;
                        //this.in_isProductOK = NOT;
                        this.isShowPassPanel = false;
                        this.product_info.ClearCurrentData();
                    }
                    break;
                case PROCESSMSG.FirstInspection:
                    // if CCD is in outside position

                    // condition 1.
                    // output image LED data, and I use ShowLiveProductDataTimer to display this information
                  
                    if (YES == this.in_isInGoldenSampleRegion)
                    {
                        //this.in_isProductOK = YES;
                        this.in_isInGoldenSampleRegion = NOT;
                        this.product_info.inspection_result = "OK";
                        // CCD go home Auto
                        AbsMove(0, 0, 80);     
                        // set msg
                        this.process_msg = PROCESSMSG.CCDGoHomeAuto;
                        this.in_isInInspectPosition = OFF;
                    }
                    else
                    {
                        //this.in_isProductOK = NOT;
                        this.product_info.inspection_result = "NG";
                    }

                    // condition 2. 
                    // save latest image LED data
                    if (ON == this.in_isHomingManual)
                    { 
                        this.product_info.inspection_time_12 = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
                        this.product_info.inspection_time_24 = DateTime.Now.ToString("yyyyMMddHHmms");
                        isShowLiveData = false;
                      
                        // CCD go home Manual
                        AbsMove(0, 0, 80);
                        // set msg
                        this.process_msg = PROCESSMSG.CCDGoHomeManual;
                        this.in_isInInspectPosition = OFF;
                    }

                    break;
                case PROCESSMSG.CCDGoHomeManual:
                    // if get ORG signal
                    // this pruduct result = NG
                    if (ON == in_isInZeroPosition)
                    {
                        // this product is fail.
                        this.in_isInZeroPosition = OFF;
                        // set msg
                        this.process_msg = PROCESSMSG.ResultNG; 
                    }
                    break;
                case PROCESSMSG.CCDGoHomeAuto:
                    // if get ORG signal
                    if (ON == in_isInZeroPosition)
                    {
                        if (YES == in_isFirstInspection)
                        {
                            // and if this is first inspection
                            // set msg
                            this.process_msg = PROCESSMSG.ClickPhysicalButtonAfterLockScrew;
                        }
                        else if (NOT == in_isFirstInspection)
                        {
                            // and if this is second inspection
                            // set pruduct result = OK
                            // set msg
                            this.process_msg = PROCESSMSG.ResultOK;
                        }
                        this.in_isInZeroPosition = OFF;
                    }
                    break;
                case PROCESSMSG.ResultOK:
                    // set pruduct result = OK
                    this.isShowPassPanel = true;
                    this.in_isProductOK = YES;
                    // set MES
                    // set msg
                    this.process_msg = PROCESSMSG.GoHomeFirst;
                    break;
                case PROCESSMSG.ResultNG:
                    // set pruduct result = NG
                    this.isShowPassPanel = true;
                    this.in_isProductOK = NOT;
                    // set MES
                    // set msg
                    this.process_msg = PROCESSMSG.GoHomeFirst;
                    break;
                case PROCESSMSG.ClickPhysicalButtonAfterLockScrew:
                    if (ON == this.in_isStartSignal)
                    {
                        // motion go out position
                        this.AbsMove(0, 165, 100);      // 165mm = 165000 pulse

                        // set msg
                        this.process_msg = PROCESSMSG.SecondInspection;
                        this.in_isFirstInspection = NOT;
                        //this.in_isProductOK = NOT;
                        this.product_info.ClearCurrentData();
                    }
                    break;
                case PROCESSMSG.SecondInspection:
                    // if CCD is in outside position

                    // condition 1.
                    // output image LED data, and I use ShowLiveProductDataTimer to display this information
                    if (YES == this.in_isInGoldenSampleRegion)
                    {
                        this.in_isInGoldenSampleRegion = NOT;
                        this.product_info.inspection_result = "OK";

                        this.product_info.inspection_time_12 = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
                        this.product_info.inspection_time_24 = DateTime.Now.ToString("yyyyMMddHHmms");

                        this.isShowLiveData = false;
                        // CCD go home Auto
                        AbsMove(0, 0, 80);
                        // set msg
                        this.process_msg = PROCESSMSG.CCDGoHomeAuto;
                        this.in_isInInspectPosition = OFF;
                    }
                    else
                    {
                        //this.in_isProductOK = NOT;
                        this.product_info.inspection_result = "NG";
                    }


                    // condition 2. 
                    // save latest image LED data
                    if (ON == this.in_isHomingManual)
                    {
                        this.product_info.inspection_time_12 = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
                        this.product_info.inspection_time_24 = DateTime.Now.ToString("yyyyMMddHHmms");
                        isShowLiveData = false;
                        // CCD go home Manual
                        AbsMove(0, 0, 80);
                        // set msg
                        this.process_msg = PROCESSMSG.CCDGoHomeManual;
                        this.in_isInInspectPosition = OFF;
                    }

                    break;
                default:
                    break;
            }
        }

        private void ImageProcess()
        {
            this.productCogToolBlock.Run();

            this.product_info.barcode = (string)this.productCogToolBlock.Outputs["Barcodet_DecodedString"].Value;
            
            this.product_info.Current_Led.x1 = (double)this.productCogToolBlock.Outputs["Led_1_X"].Value;
            this.product_info.Current_Led.y1 = (double)this.productCogToolBlock.Outputs["Led_1_Y"].Value;
            this.product_info.Current_Led.x2 = (double)this.productCogToolBlock.Outputs["Led_2_X"].Value;
            this.product_info.Current_Led.y2 = (double)this.productCogToolBlock.Outputs["Led_2_Y"].Value;
    
            // for show draw line pictrue 
            this.showCogToolBlock.Inputs["Led_1_X"].Value = this.product_info.Current_Led.x1;
            this.showCogToolBlock.Inputs["Led_1_Y"].Value = this.product_info.Current_Led.y1;
            this.showCogToolBlock.Inputs["Led_2_X"].Value = this.product_info.Current_Led.x2;
            this.showCogToolBlock.Inputs["Led_2_Y"].Value = this.product_info.Current_Led.y2;
            this.showCogToolBlock.Inputs["Radius"].Value = 0.08;

            double offset = 0.0;
            if (YES == in_isFirstInspection)
            {
                offset = this.product_info.first_offset;
            }
            else
            {
                offset = this.product_info.second_offset;
            }
            this.showCogToolBlock.Inputs["Left_1_X"].Value = this.product_info.Basic_Led.x1 - offset;
            this.showCogToolBlock.Inputs["Top_1_Y"].Value = this.product_info.Basic_Led.y1 - offset;
            this.showCogToolBlock.Inputs["Right_1_X"].Value = this.product_info.Basic_Led.x1 + offset;
            this.showCogToolBlock.Inputs["Bottom_1_Y"].Value = this.product_info.Basic_Led.y1 + offset;

            this.showCogToolBlock.Inputs["Left_2_X"].Value = this.product_info.Basic_Led.x2 - offset;
            this.showCogToolBlock.Inputs["Top_2_Y"].Value = this.product_info.Basic_Led.y2 - offset;
            this.showCogToolBlock.Inputs["Right_2_X"].Value = this.product_info.Basic_Led.x2 + offset;
            this.showCogToolBlock.Inputs["Bottom_2_Y"].Value = this.product_info.Basic_Led.y2 + offset;

            circle = (CogCreateCircleTool)showCogToolBlock.Tools["CogCreateCircleTool1"];
            line = (CogCreateSegmentTool)showCogToolBlock.Tools["CogCreateSegmentTool_2_Left"];
            circle.GetOutputCircle().Color = CogColorConstants.Red;
            line.Segment.Color = CogColorConstants.Red;

            this.showCogToolBlock.Run();
            //if (false == this.isScreenSplit)
            //{
            this.cogRecordDisplay3.Record = this.showCogToolBlock.CreateLastRunRecord().SubRecords["CogIPOneImageTool1.OutputImage"];
            //}
            //else
            //{
            this.cogRecordDisplay1.Record = this.showCogToolBlock.CreateLastRunRecord().SubRecords["CogIPOneImageTool2.OutputImage"];
            this.cogRecordDisplay2.Record = this.showCogToolBlock.CreateLastRunRecord().SubRecords["CogIPOneImageTool3.OutputImage"];
            //}

            // translate to SGS coordinate
            this.product_info.ShowSGS_Led.x1 = this.product_info.Current_Led.x1 - this.product_info.Basic_Led.x1;
            this.product_info.ShowSGS_Led.y1 = this.product_info.Current_Led.y1 - this.product_info.Basic_Led.y1;
            this.product_info.ShowSGS_Led.x2 = this.product_info.Current_Led.x2 - this.product_info.Basic_Led.x2;
            this.product_info.ShowSGS_Led.y2 = this.product_info.Current_Led.y2 - this.product_info.Basic_Led.y2;

            this.product_info.ShowSGS_Led.x1 = this.product_info.SGS_Led.x1 + this.product_info.ShowSGS_Led.x1;
            this.product_info.ShowSGS_Led.y1 = this.product_info.SGS_Led.y1 + this.product_info.SGS_Led.y_dir * this.product_info.ShowSGS_Led.y1;
            this.product_info.ShowSGS_Led.x2 = this.product_info.SGS_Led.x2 + this.product_info.ShowSGS_Led.x2;
            this.product_info.ShowSGS_Led.y2 = this.product_info.SGS_Led.y2 + this.product_info.SGS_Led.y_dir * this.product_info.ShowSGS_Led.y2;
        }
        CogCreateCircleTool circle = new CogCreateCircleTool();
        CogCreateSegmentTool line = new CogCreateSegmentTool();

        private int CheckLedCoordinate()
        {
            int isInGoldenSampleRegion = NOT;
            double offset = 0.0;
            if (YES == in_isFirstInspection)
            {
                offset = this.product_info.first_offset;
            }
            else
            {
                offset = this.product_info.second_offset;
            }

            if ((this.product_info.Current_Led.x1 <= this.product_info.Basic_Led.x1 + offset)
             && (this.product_info.Current_Led.x1 >= this.product_info.Basic_Led.x1 - offset)
             && (this.product_info.Current_Led.y1 <= this.product_info.Basic_Led.y1 + offset)
             && (this.product_info.Current_Led.y1 >= this.product_info.Basic_Led.y1 - offset))
            {
                if ((this.product_info.Current_Led.x2 <= this.product_info.Basic_Led.x2 + offset)
                 && (this.product_info.Current_Led.x2 >= this.product_info.Basic_Led.x2 - offset)
                 && (this.product_info.Current_Led.y2 <= this.product_info.Basic_Led.y2 + offset)
                 && (this.product_info.Current_Led.y2 >= this.product_info.Basic_Led.y2 - offset))
                {
                    isInGoldenSampleRegion = YES;
                }
            }


            return isInGoldenSampleRegion;
        }
        #endregion
        
        private MAINTAB old_main_tab = MAINTAB.USER;
        private int current_main_tab = -1;
        private int product_num = 0;
        CogAcqFifoTool myCogAcqFifoTool = new CogAcqFifoTool();
        private CogToolBlock showCogToolBlock = new CogToolBlock();
        private void mainTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (old_main_tab)
            {
                case MAINTAB.USER:
                    break;
                case MAINTAB.EDITOR:
                    if ("Lock" == this.motionUnlockButton.Text)
                    {
                        //if(DialogResult.Yes == MessageBox.Show("運動參數上鎖？","Warning",MessageBoxButtons.OKCancel))
                        //this.motionUnlockButton.PerformClick();
                        System.Threading.Thread.Sleep(100);
                    }
                    break;
                case MAINTAB.OPERATOR:
                    // stop inspection step 
                    // 1. motion OFF
                    // 2. CCD OFF
                    this.cogRecordDisplay3.StopLiveDisplay();
                    // 3. clear Msg
                    // 4.
                    this.isThreadStart = false;   
                    this.showLiveProductDataTimer.Stop();
                    
                    break;
                case MAINTAB.HELPER:
                    break;
                case MAINTAB.OTHER:
                    break;
                default:
                    break;
            }

            this.current_main_tab = ((TabControl)sender).SelectedIndex;
            switch (this.current_main_tab)
            {
                case (int)MAINTAB.USER:
                    this.old_main_tab = MAINTAB.USER;



                    break;
                case (int)MAINTAB.EDITOR:
                    if (true == this.isLogin)
                    {
                        this.old_main_tab = MAINTAB.EDITOR;

                        this.MotionParameter();
                    }
                    else
                    {
                        this.mainTabControl.SelectedIndex = (int)this.old_main_tab;
                    }

                    break;
                case (int)MAINTAB.OPERATOR:
                    if (true == this.isLogin)
                    {
                        if (null != this.product_info && this.toolStripProgressBar1.Value == 1000)
                        {
                            this.old_main_tab = MAINTAB.OPERATOR;

                            this.MotionParameter();

                            // start inspection step 
                            // 1. motion ON
                            // 2. CCD ON
                            //this.cogRecordDisplay3.Image = this.myCogAcqFifoTool.OutputImage;
                            ////this.cogRecordDisplay3.StartLiveDisplay(myCogAcqFifoTool.Operator);
                            //this.cogRecordDisplay1.Fit(true);
                            //this.cogRecordDisplay2.Fit(true);
                            //this.cogRecordDisplay3.Fit(true);
                            this.cogRecordDisplay1.AutoFit = true;
                            this.cogRecordDisplay2.AutoFit = true;
                            this.cogRecordDisplay3.AutoFit = true;
                            // 3. Show start Msg
                            this.in_isHomeNow = NOT;
                            this.process_msg = PROCESSMSG.GoHomeFirst;
                            this.showLiveProductDataTimer.Start();
                        }
                        else
                        {
                            this.mainTabControl.SelectedIndex = (int)this.old_main_tab;
                        }
                    }
                    else
                    {
                        this.mainTabControl.SelectedIndex = (int)this.old_main_tab;
                    }
                    break;
                case (int)MAINTAB.HELPER:
                    if (true == this.isLogin)
                    {
                        this.old_main_tab = MAINTAB.HELPER;
                    }

                    else
                    {
                        this.mainTabControl.SelectedIndex = (int)this.old_main_tab;
                    }

                    break;
                default:
                    break;
            }
        }

        private void firstOffsetButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.first_offset = double.Parse(this.firstOffsetTextBox.Text);
            if (null != this.product_info)
            {
                this.product_info.first_offset = double.Parse(this.firstOffsetTextBox.Text);
            }
        }
        private void secondOffsetButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.second_offset = double.Parse(this.secondOffsetTextBox.Text); 
            if (null != this.product_info)
            {
                this.product_info.second_offset = double.Parse(this.secondOffsetTextBox.Text);
            }
        }

        private void refreshOKNGtimer_Tick(object sender, EventArgs e)
        {
            this.showPassNgPanel.Refresh();
        }
        
     }

    public struct Axis_Status
    {
        public bool Mts_HMV;
        public bool Mts_SMV;
        public bool Mio_ALM;
        public bool Mio_PEL;
        public bool Mio_MEL;
        public bool Mio_ORG;
        public bool Mio_EMG;
        public bool Mio_SVON;

    }

    public struct SCoordinate
    {
        public double x1, y1, x2, y2;
        public double y_dir;
    }
    
    public class CProductInfo
    {
        public PRODUCT product;

        public string barcode;
        public string inspection_result;
        public string inspection_time_12, inspection_time_24;
        public SCoordinate Current_Led, Basic_Led, SGS_Led, ShowSGS_Led;
        public double first_offset,second_offset;

        public CProductInfo(PRODUCT _product)
        {
            this.product = _product;

            this.barcode = string.Empty;

            this.Current_Led = new SCoordinate() { x1 = 0.0, x2 = 0.0, y1 = 0.0, y2 = 0.0 };
            this.Basic_Led = new SCoordinate() { x1 = 0.0, x2 = 0.0, y1 = 0.0, y2 = 0.0 };
            this.SGS_Led = new SCoordinate() { x1 = 0.0, x2 = 0.0, y1 = 0.0, y2 = 0.0 };
            this.ShowSGS_Led = new SCoordinate() { x1 = 0.0, x2 = 0.0, y1 = 0.0, y2 = 0.0 };
         
            this.inspection_result = string.Empty;
            this.inspection_time_12 = string.Empty;
            this.inspection_time_24 = string.Empty;

            this.first_offset = Properties.Settings.Default.first_offset;
            this.second_offset = Properties.Settings.Default.second_offset;

            this.GetLedParameters(_product);
        }

        public void ClearCurrentData()
        {
            this.Current_Led.x1 = 0;
            this.Current_Led.y1 = 0;
            this.Current_Led.x2 = 0;
            this.Current_Led.y2 = 0;
        }

        private void GetLedParameters(PRODUCT _product)
        {
            switch (_product)
            {
                case PRODUCT.DRL_L:
                    this.SGS_Led.x1 = Properties.Settings.Default.sgs_drl_L_led1_x;
                    this.SGS_Led.y1 = Properties.Settings.Default.sgs_drl_L_led1_y;
                    this.SGS_Led.x2 = Properties.Settings.Default.sgs_drl_L_led2_x;
                    this.SGS_Led.y2 = Properties.Settings.Default.sgs_drl_L_led2_y;

                    this.SGS_Led.y_dir = Properties.Settings.Default.sgs_drl_L_ydir;
                    break;
                case PRODUCT.DRL_R:
                    this.SGS_Led.x1 = Properties.Settings.Default.sgs_drl_R_led1_x;
                    this.SGS_Led.y1 = Properties.Settings.Default.sgs_drl_R_led1_y;
                    this.SGS_Led.x2 = Properties.Settings.Default.sgs_drl_R_led2_x;
                    this.SGS_Led.y2 = Properties.Settings.Default.sgs_drl_R_led2_y;

                    this.SGS_Led.y_dir = Properties.Settings.Default.sgs_drl_R_ydir;
                    break;
                case PRODUCT.LOWPOWER_L:
                    this.SGS_Led.x1 = Properties.Settings.Default.sgs_low_power_L_led1_x;
                    this.SGS_Led.y1 = Properties.Settings.Default.sgs_low_power_L_led1_y;
                    this.SGS_Led.x2 = Properties.Settings.Default.sgs_low_power_L_led2_x;
                    this.SGS_Led.y2 = Properties.Settings.Default.sgs_low_power_L_led2_y;

                    this.SGS_Led.y_dir = Properties.Settings.Default.sgs_low_power_L_ydir;
                    break;
                case PRODUCT.LOWPOWER_R:
                    this.SGS_Led.x1 = Properties.Settings.Default.sgs_low_power_R_led1_x;
                    this.SGS_Led.y1 = Properties.Settings.Default.sgs_low_power_R_led1_y;
                    this.SGS_Led.x2 = Properties.Settings.Default.sgs_low_power_R_led2_x;
                    this.SGS_Led.y2 = Properties.Settings.Default.sgs_low_power_R_led2_y;

                    this.SGS_Led.y_dir = Properties.Settings.Default.sgs_low_power_R_ydir;
                    break;
                case PRODUCT.DRLVAVE_L:                    
                    this.SGS_Led.x1 = Properties.Settings.Default.sgs_vave_L_led1_x;
                    this.SGS_Led.y1 = Properties.Settings.Default.sgs_vave_L_led1_y;
                    this.SGS_Led.x2 = Properties.Settings.Default.sgs_vave_L_led2_x;
                    this.SGS_Led.y2 = Properties.Settings.Default.sgs_vave_L_led2_y;

                    this.SGS_Led.y_dir = Properties.Settings.Default.sgs_vave_L_ydir;
                    break;
                case PRODUCT.DRLVAVE_R:
                    this.SGS_Led.x1 = Properties.Settings.Default.sgs_vave_R_led1_x;
                    this.SGS_Led.y1 = Properties.Settings.Default.sgs_vave_R_led1_y;
                    this.SGS_Led.x2 = Properties.Settings.Default.sgs_vave_R_led2_x;
                    this.SGS_Led.y2 = Properties.Settings.Default.sgs_vave_R_led2_y;

                    this.SGS_Led.y_dir = Properties.Settings.Default.sgs_vave_L_ydir;
                    break;
                default:
                    break;
            }
        }
        //public void SetLedParameters(PRODUCT _product, bool isBasic)
        //{
        //    switch (_product)
        //    {
        //        case PRODUCT.DRL_L:
        //            if (true == isBasic)
        //            {
        //                Properties.Settings.Default.drl_L_led1_x = this.Basic_Led.x1;
        //                Properties.Settings.Default.drl_L_led1_y = this.Basic_Led.y1;
        //                Properties.Settings.Default.drl_L_led2_x = this.Basic_Led.x2;
        //                Properties.Settings.Default.drl_L_led2_y = this.Basic_Led.y2;
        //            }
        //            else
        //            {
        //                Properties.Settings.Default.drl_L_led1_xoff = this.Offset_Led.x1;
        //                Properties.Settings.Default.drl_L_led1_yoff = this.Offset_Led.y1;
        //                Properties.Settings.Default.drl_L_led2_xoff = this.Offset_Led.x2;
        //                Properties.Settings.Default.drl_L_led2_yoff = this.Offset_Led.y2;
        //            }
        //            break;
        //        case PRODUCT.DRL_R:
        //            if (true == isBasic)
        //            {
        //                Properties.Settings.Default.drl_R_led1_x = this.Basic_Led.x1;
        //                Properties.Settings.Default.drl_R_led1_y = this.Basic_Led.y1;
        //                Properties.Settings.Default.drl_R_led2_x = this.Basic_Led.x2;
        //                Properties.Settings.Default.drl_R_led2_y = this.Basic_Led.y2;
        //            }
        //            else
        //            {
        //                Properties.Settings.Default.drl_R_led1_xoff = this.Offset_Led.x1;
        //                Properties.Settings.Default.drl_R_led1_yoff = this.Offset_Led.y1;
        //                Properties.Settings.Default.drl_R_led2_xoff = this.Offset_Led.x2;
        //                Properties.Settings.Default.drl_R_led2_yoff = this.Offset_Led.y2;
        //            }
        //            break;
        //        case PRODUCT.LOWPOWER_L:
        //            if (true == isBasic)
        //            {
        //                Properties.Settings.Default.low_power_L_led1_x = this.Basic_Led.x1;
        //                Properties.Settings.Default.low_power_L_led1_y = this.Basic_Led.y1;
        //                Properties.Settings.Default.low_power_L_led2_x = this.Basic_Led.x2;
        //                Properties.Settings.Default.low_power_L_led2_y = this.Basic_Led.y2;
        //            }
        //            else
        //            {
        //                Properties.Settings.Default.low_power_L_led1_xoff = this.Offset_Led.x1;
        //                Properties.Settings.Default.low_power_L_led1_yoff = this.Offset_Led.y1;
        //                Properties.Settings.Default.low_power_L_led2_xoff = this.Offset_Led.x2;
        //                Properties.Settings.Default.low_power_L_led2_yoff = this.Offset_Led.y2;
        //            }
        //            break;
        //        case PRODUCT.LOWPOWER_R:
        //            if (true == isBasic)
        //            {
        //                Properties.Settings.Default.low_power_R_led1_x = this.Basic_Led.x1;
        //                Properties.Settings.Default.low_power_R_led1_y = this.Basic_Led.y1;
        //                Properties.Settings.Default.low_power_R_led2_x = this.Basic_Led.x2;
        //                Properties.Settings.Default.low_power_R_led2_y = this.Basic_Led.y2;
        //            }
        //            else
        //            {
        //                Properties.Settings.Default.low_power_R_led1_xoff = this.Offset_Led.x1;
        //                Properties.Settings.Default.low_power_R_led1_yoff = this.Offset_Led.y1;
        //                Properties.Settings.Default.low_power_R_led2_xoff = this.Offset_Led.x2;
        //                Properties.Settings.Default.low_power_R_led2_yoff = this.Offset_Led.y2;
        //            }
        //            break;
        //        case PRODUCT.DRLVAVE_L:
        //            if (true == isBasic)
        //            {
        //                Properties.Settings.Default.drl_vave_L_led1_x = this.Basic_Led.x1;
        //                Properties.Settings.Default.drl_vave_L_led1_y = this.Basic_Led.y1;
        //                Properties.Settings.Default.drl_vave_L_led2_x = this.Basic_Led.x2;
        //                Properties.Settings.Default.drl_vave_L_led2_y = this.Basic_Led.y2;
        //            }
        //            else
        //            {
        //                Properties.Settings.Default.drl_vave_L_led1_xoff = this.Offset_Led.x1;
        //                Properties.Settings.Default.drl_vave_L_led1_yoff = this.Offset_Led.y1;
        //                Properties.Settings.Default.drl_vave_L_led2_xoff = this.Offset_Led.x2;
        //                Properties.Settings.Default.drl_vave_L_led2_yoff = this.Offset_Led.y2;
        //            }
        //            break;
        //        case PRODUCT.DRLVAVE_R: if (true == isBasic)
        //            {
        //                Properties.Settings.Default.drl_vave_R_led1_x = this.Basic_Led.x1;
        //                Properties.Settings.Default.drl_vave_R_led1_y = this.Basic_Led.y1;
        //                Properties.Settings.Default.drl_vave_R_led2_x = this.Basic_Led.x2;
        //                Properties.Settings.Default.drl_vave_R_led2_y = this.Basic_Led.y2;
        //            }
        //            else
        //            {
        //                Properties.Settings.Default.drl_vave_R_led1_xoff = this.Offset_Led.x1;
        //                Properties.Settings.Default.drl_vave_R_led1_yoff = this.Offset_Led.y1;
        //                Properties.Settings.Default.drl_vave_R_led2_xoff = this.Offset_Led.x2;
        //                Properties.Settings.Default.drl_vave_R_led2_yoff = this.Offset_Led.y2;
        //            }
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }

}
