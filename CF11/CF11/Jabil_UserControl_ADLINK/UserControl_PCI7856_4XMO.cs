using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using APS168_W32;
using APS_Define_W32;
using System.Threading;

namespace Jabil_UserControl_ADLINK
{

    public partial class UserControl_PCI7856_4XMO : UserControl
    {
        Int16 CardID = 0;	      //Card 軸卡編號
        Int16 BusNo = 1;          //Bus number(Port ID) MNET為1  HSL為0
        Int32 Start_Axis_ID = 0;  //First Axis number in Motion Net bus.

        bool G_FunctionFail = false;    //判斷連線與動作是否出現錯誤
        UserClass_PCI7856_4XMO.Table G_Table = null;       //平台數據

        Label[] Labels_FB_Command = new Label[4];
        Label[] Labels_FB_Pos = new Label[4];
        PictureBox[] PictureBox_FB_HMV = new PictureBox[4];
        PictureBox[] PictureBox_FB_SMV = new PictureBox[4];
        PictureBox[] PictureBox_FB_ALM = new PictureBox[4];
        PictureBox[] PictureBox_FB_PEL = new PictureBox[4];
        PictureBox[] PictureBox_FB_MEL = new PictureBox[4];
        PictureBox[] PictureBox_FB_ORG = new PictureBox[4];
        PictureBox[] PictureBox_FB_EMG = new PictureBox[4];
        PictureBox[] PictureBox_FB_SVON = new PictureBox[4];

        Thread Thread_StatusGet;   //取得馬達狀態值

        #region "初始建構式"
        //初始化
        public UserControl_PCI7856_4XMO()
        {
            InitializeComponent();

            //陣列儲存
            Labels_FB_Command[0] = (Label)this.groupBox_FB_Motor0.Controls["label_Motor0_FB_Command"];
            Labels_FB_Command[1] = (Label)this.groupBox_FB_Motor1.Controls["label_Motor1_FB_Command"];
            Labels_FB_Command[2] = (Label)this.groupBox_FB_Motor2.Controls["label_Motor2_FB_Command"];
            Labels_FB_Command[3] = (Label)this.groupBox_FB_Motor3.Controls["label_Motor3_FB_Command"];
            Labels_FB_Pos[0] = (Label)this.groupBox_FB_Motor0.Controls["label_Motor0_FB_Pos"];
            Labels_FB_Pos[1] = (Label)this.groupBox_FB_Motor1.Controls["label_Motor1_FB_Pos"];
            Labels_FB_Pos[2] = (Label)this.groupBox_FB_Motor2.Controls["label_Motor2_FB_Pos"];
            Labels_FB_Pos[3] = (Label)this.groupBox_FB_Motor3.Controls["label_Motor3_FB_Pos"];

            PictureBox_FB_HMV[0] = (PictureBox)this.groupBox_FB_Motor0.Controls["pictureBox_Motor0_HMV"];
            PictureBox_FB_HMV[1] = (PictureBox)this.groupBox_FB_Motor1.Controls["pictureBox_Motor1_HMV"];
            PictureBox_FB_HMV[2] = (PictureBox)this.groupBox_FB_Motor2.Controls["pictureBox_Motor2_HMV"];
            PictureBox_FB_HMV[3] = (PictureBox)this.groupBox_FB_Motor3.Controls["pictureBox_Motor3_HMV"];

            PictureBox_FB_SMV[0] = (PictureBox)this.groupBox_FB_Motor0.Controls["pictureBox_Motor0_SMV"];
            PictureBox_FB_SMV[1] = (PictureBox)this.groupBox_FB_Motor1.Controls["pictureBox_Motor1_SMV"];
            PictureBox_FB_SMV[2] = (PictureBox)this.groupBox_FB_Motor2.Controls["pictureBox_Motor2_SMV"];
            PictureBox_FB_SMV[3] = (PictureBox)this.groupBox_FB_Motor3.Controls["pictureBox_Motor3_SMV"];

            PictureBox_FB_ALM[0] = (PictureBox)this.groupBox_FB_Motor0.Controls["pictureBox_Motor0_ALM"];
            PictureBox_FB_ALM[1] = (PictureBox)this.groupBox_FB_Motor1.Controls["pictureBox_Motor1_ALM"];
            PictureBox_FB_ALM[2] = (PictureBox)this.groupBox_FB_Motor2.Controls["pictureBox_Motor2_ALM"];
            PictureBox_FB_ALM[3] = (PictureBox)this.groupBox_FB_Motor3.Controls["pictureBox_Motor3_ALM"];

            PictureBox_FB_PEL[0] = (PictureBox)this.groupBox_FB_Motor0.Controls["pictureBox_Motor0_PEL"];
            PictureBox_FB_PEL[1] = (PictureBox)this.groupBox_FB_Motor1.Controls["pictureBox_Motor1_PEL"];
            PictureBox_FB_PEL[2] = (PictureBox)this.groupBox_FB_Motor2.Controls["pictureBox_Motor2_PEL"];
            PictureBox_FB_PEL[3] = (PictureBox)this.groupBox_FB_Motor3.Controls["pictureBox_Motor3_PEL"];

            PictureBox_FB_MEL[0] = (PictureBox)this.groupBox_FB_Motor0.Controls["pictureBox_Motor0_MEL"];
            PictureBox_FB_MEL[1] = (PictureBox)this.groupBox_FB_Motor1.Controls["pictureBox_Motor1_MEL"];
            PictureBox_FB_MEL[2] = (PictureBox)this.groupBox_FB_Motor2.Controls["pictureBox_Motor2_MEL"];
            PictureBox_FB_MEL[3] = (PictureBox)this.groupBox_FB_Motor3.Controls["pictureBox_Motor3_MEL"];

            PictureBox_FB_ORG[0] = (PictureBox)this.groupBox_FB_Motor0.Controls["pictureBox_Motor0_ORG"];
            PictureBox_FB_ORG[1] = (PictureBox)this.groupBox_FB_Motor1.Controls["pictureBox_Motor1_ORG"];
            PictureBox_FB_ORG[2] = (PictureBox)this.groupBox_FB_Motor2.Controls["pictureBox_Motor2_ORG"];
            PictureBox_FB_ORG[3] = (PictureBox)this.groupBox_FB_Motor3.Controls["pictureBox_Motor3_ORG"];

            PictureBox_FB_EMG[0] = (PictureBox)this.groupBox_FB_Motor0.Controls["pictureBox_Motor0_EMG"];
            PictureBox_FB_EMG[1] = (PictureBox)this.groupBox_FB_Motor1.Controls["pictureBox_Motor1_EMG"];
            PictureBox_FB_EMG[2] = (PictureBox)this.groupBox_FB_Motor2.Controls["pictureBox_Motor2_EMG"];
            PictureBox_FB_EMG[3] = (PictureBox)this.groupBox_FB_Motor3.Controls["pictureBox_Motor3_EMG"];

            PictureBox_FB_SVON[0] = (PictureBox)this.groupBox_FB_Motor0.Controls["pictureBox_Motor0_SVON"];
            PictureBox_FB_SVON[1] = (PictureBox)this.groupBox_FB_Motor1.Controls["pictureBox_Motor1_SVON"];
            PictureBox_FB_SVON[2] = (PictureBox)this.groupBox_FB_Motor2.Controls["pictureBox_Motor2_SVON"];
            PictureBox_FB_SVON[3] = (PictureBox)this.groupBox_FB_Motor3.Controls["pictureBox_Motor3_SVON"];

        }
        #endregion
        
        #region "功能"
        //初始設定(目前一個Table最多四軸)
        public void Initialize(UserClass_PCI7856_4XMO.Table _Table)
        {
            ////先將控制項隱藏，之後依據開啟的軸數依序顯示
            groupBox_Motor0.Visible = false;
            groupBox_Motor1.Visible = false;
            groupBox_Motor2.Visible = false;
            groupBox_Motor3.Visible = false;

            if (_Table.Axes.Length > 4)
            {
                MessageBox.Show("超過最大軸數(4)");
                return;
            }

            G_Table = _Table;

            Int32 DPAC_ID_Bits = 0;
            //軸卡初始化
            if (APS168.APS_initial(ref DPAC_ID_Bits, 0) == 0)
            {
                //設定軸卡連線設定  
                ConnectSettng();

                if (G_FunctionFail == true)
                { return; }

                //開始連線  (模組沒上電會錯誤)
                ConnectStart();
                
                if (G_FunctionFail == true)
                { return; }

                int Count_UserControl = 0;
                foreach (UserClass_PCI7856_4XMO.Axis myAxis in _Table.Axes)
                {
                    int myAxisID = myAxis.ID;
                    AxisSetting(myAxisID);              //各軸設定
                    GroupDisplay_AxisData(Count_UserControl, myAxis, _Table.Axes.Length);          //Group改字與隱藏Group
                    Count_UserControl += 1;
                }
                //全軸Servo On
                G_Table.ServoOn();

                //判斷設定是否完全成功
                if (G_FunctionFail == false)
                {
                    //取得FeedBack值
                    Thread_StatusGet = new Thread(Status_Get);
                    Thread_StatusGet.Start();
                    //顯示FeedBack值
                    timer_StatusDisplay.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("軸卡初始化失敗，請確認硬體後，重新開啟程式");
                APS168.APS_close();
                return;
            }
        }
        //取得FeedBack值
        private void Status_Get()
        {
            do
            {
                //取得FeedBack值
                AxisData_Get();

                Thread.Sleep(50);
            }
            while (true);

        }

        //設定軸卡連線設定
        private void ConnectSettng()
        {
            //Set PRF_TRANSFER_RATE: 3 
            int ErrorCode = APS168.APS_set_field_bus_param(CardID, BusNo, (Int32)APS_Define.PRF_TRANSFER_RATE, 3);
            if (ErrorCode == 0)
            {
                G_FunctionFail = false;
            }
            else
            {
                MessageBox.Show("連線設定錯誤, ErrorCode  " + ErrorCode.ToString());
                G_FunctionFail = true;
            }
        }

        //開始連線
        private void ConnectStart()
        {

           
            //判斷前端是否出現錯誤
            if (!G_FunctionFail)
            {
                int ErrorCode = APS168.APS_start_field_bus(CardID, BusNo, Start_Axis_ID);
                if (ErrorCode == 0)
                {
                    G_FunctionFail = false;
                }
                else
                {
                    MessageBox.Show("開始連線錯誤, ErrorCode  " + ErrorCode.ToString());
                    G_FunctionFail = true;
                }
            }
        }

        //各軸設定
        private void AxisSetting(int _AxisID)
        {
            //判斷前端是否出現錯誤
            if (!G_FunctionFail)
            {
                //設定軸卡參數

                int ErrorCodeCount = 0;
                //前後極限的邏輯

                //IO and Pulse Mode
                ErrorCodeCount += APS168.APS_set_axis_param(_AxisID, (Int32)APS_Define.PRA_ALM_LOGIC, 1);       //Set PRA_ALM_LOGIC       1: Active high
                ErrorCodeCount += APS168.APS_set_axis_param(_AxisID, (Int32)APS_Define.PRA_PLS_IPT_MODE, 2);    //Set PRA_PLS_IPT_MODE    2: A/B X4
                ErrorCodeCount += APS168.APS_set_axis_param(_AxisID, (Int32)APS_Define.PRA_PLS_OPT_MODE, 4);    //Set PRA_PLS_OPT_MODE    4: CW/CCW (AH)
                ErrorCodeCount += APS168.APS_set_axis_param(_AxisID, (Int32)APS_Define.PRA_PLS_IPT_LOGIC, 0);   //Set PRA_PLS_IPT_LOGIC   0: don not reverse counting direction
                //
                ErrorCodeCount += APS168.APS_set_axis_param(_AxisID, (Int32)APS_Define.PRA_FEEDBACK_SRC, 1);    //Select feedback conter  0: Ext. Encoder mode  1: Stepper mode 

                ErrorCodeCount += APS168.APS_set_axis_param(_AxisID, (Int32)APS_Define.PRA_SERVO_LOGIC, 0);     //Set SERVO output logic  0: Active low

                //Single Move Parameter
                ErrorCodeCount += APS168.APS_set_axis_param(_AxisID, (Int32)APS_Define.PRA_CURVE, 1);			  //Set PRA_CURVE  0:T-Curve 1:S-Curve
                ErrorCodeCount += APS168.APS_set_axis_param(_AxisID, (Int32)APS_Define.PRA_ACC, 1500000);		  //Set PRA_ACC 
                ErrorCodeCount += APS168.APS_set_axis_param(_AxisID, (Int32)APS_Define.PRA_DEC, 1500000);		  //Set PRA_DEC
                ErrorCodeCount += APS168.APS_set_axis_param(_AxisID, (Int32)APS_Define.PRA_VS, 0);			  //Set PRA_VS (Set homing start velocity)

                //Home Move Parameter
                ErrorCodeCount += APS168.APS_set_axis_param(_AxisID, (Int32)APS_Define.PRA_ORG_LOGIC, 1);       //1: Inverse
                //ErrorCodeCount += APS168.APS_set_axis_param(_AxisID, (Int32)APS_Define.PRA_HOME_MODE, 0);		  //Set PRA_HOME_MODE
                ErrorCodeCount += APS168.APS_set_axis_param(_AxisID, (Int32)APS_Define.PRA_HOME_MODE, 1);		  //Set PRA_HOME_MODE
                ErrorCodeCount += APS168.APS_set_axis_param(_AxisID, (Int32)APS_Define.PRA_HOME_DIR, 1);		  //Set PRA_HOME_DIR    0:Positive 1:Negative
                ErrorCodeCount += APS168.APS_set_axis_param(_AxisID, (Int32)APS_Define.PRA_HOME_VM, 50000);	  //Set PRA_HOME_VM     (Set homing maximum velocity. )
                ErrorCodeCount += APS168.APS_set_axis_param(_AxisID, (Int32)APS_Define.PRA_HOME_EZA, 0);		  //Set PRA_HOME_EZA    (0: Not enabl)
                ErrorCodeCount += APS168.APS_set_axis_param(_AxisID, (Int32)APS_Define.PRA_HOME_VO, 10);		  //Set PRA_HOME_VO     (Homing leave home velocity)
                ErrorCodeCount += APS168.APS_set_axis_param(_AxisID, (Int32)APS_Define.PRA_HOME_VM, 50000);		  //Set PRA_HOME_VM     Homing speed
                ErrorCodeCount += APS168.APS_set_axis_param(_AxisID, (Int32)APS_Define.PRA_HOME_OFFSET, 500);	  //Set PRA_HOME_OFFSET (Homing leave home distance – Specify ORG offset)

                //個人設定(跟驅動器設定的正反轉方向有關)
                if (_AxisID == 0 || _AxisID == 2)
                {
                    ErrorCodeCount += APS168.APS_set_axis_param(_AxisID, (Int32)APS_Define.PRA_PLS_IPT_LOGIC, 1);   //Set PRA_PLS_IPT_LOGIC   1: reverse counting direction
                }
                else
                {
                    ErrorCodeCount += APS168.APS_set_axis_param(_AxisID, (Int32)APS_Define.PRA_PLS_IPT_LOGIC, 0);   //Set PRA_PLS_IPT_LOGIC   0: don not reverse counting direction
                }
                

                if (ErrorCodeCount == 0)
                {
                    G_FunctionFail = false;
                }
                else
                {
                    MessageBox.Show("軸卡設定錯誤");
                    G_FunctionFail = true;
                }


            }
        }

        //關閉
        public void Close()
        {
            //取狀態的執行緒
            if (Thread_StatusGet != null)
            { Thread_StatusGet.Abort(); }

            if (G_FunctionFail == false)
            {
                //全軸Servo Off
                G_Table.ServoOff();
            }

            APS168.APS_stop_field_bus(CardID, BusNo);
            APS168.APS_close();
        }

        //各軸ServoOn或Off
        private void Axis_Servo(int _Servo, int _AxisID)
        {
            //判斷是否連線成功
            if (!G_FunctionFail)
            {

                int ErrorCode = 0;
                APS168.APS_set_servo_on(_AxisID, (int)_Servo);    //Servo On

                if (ErrorCode == 0)
                {
                    G_FunctionFail = false;
                }
                else
                {
                    MessageBox.Show("ServoOn錯誤");
                    G_FunctionFail = true;
                }

            }
        }

        

        //取得馬達回饋資訊
        private void AxisData_Get()
        {
            //取得Axis狀態值
            G_Table.AxisStatus_Get();
            //計算Table狀態值
            G_Table.TableStatus_Get();
        }
        #endregion

        #region "事件函式"

        #region"按鈕"
        private void btn_Motor_Move_Click(object sender, EventArgs e)
        {
            Button myEventButton = (Button)sender;
            TextBox myTextBox = null;
            ComboBox myComboBox = null;
            int myAxisID = -1;

            UserClass_PCI7856_4XMO.Axis mySearchAxis = null; //"軸資訊"(由軸ID進行搜尋  最後由函數輸出命令)

            //判斷第幾個馬達的按鈕
            if (myEventButton.Name == "btn_Motor0_Move")
            {
                myTextBox = (TextBox)this.groupBox_Command_Motor0.Controls["textBox_Motor0_Pos"];
                myComboBox = (ComboBox)this.groupBox_Command_Motor0.Controls["comboBox_Motor0_Speed"];
                myAxisID = int.Parse(label_Axis0_ID.Text);
            }
            else if (myEventButton.Name == "btn_Motor1_Move")
            {
                myTextBox = (TextBox)this.groupBox_Command_Motor1.Controls["textBox_Motor1_Pos"];
                myComboBox = (ComboBox)this.groupBox_Command_Motor1.Controls["comboBox_Motor1_Speed"];
                myAxisID = int.Parse(label_Axis1_ID.Text);
            }
            else if (myEventButton.Name == "btn_Motor2_Move")
            {
                myTextBox = (TextBox)this.groupBox_Command_Motor2.Controls["textBox_Motor2_Pos"];
                myComboBox = (ComboBox)this.groupBox_Command_Motor2.Controls["comboBox_Motor2_Speed"];
                myAxisID = int.Parse(label_Axis2_ID.Text);
            }
            else if (myEventButton.Name == "btn_Motor3_Move")
            {
                myTextBox = (TextBox)this.groupBox_Command_Motor3.Controls["textBox_Motor3_Pos"];
                myComboBox = (ComboBox)this.groupBox_Command_Motor3.Controls["comboBox_Motor3_Speed"];
                myAxisID = int.Parse(label_Axis3_ID.Text);
            }
            else
            { MessageBox.Show("無此按鈕"); }
           
            //搜尋指定的Axis
            for (int i = 0; i < G_Table.Axes.Length; i++)
            {
                if (myAxisID == G_Table.Axes[i].ID)
                {
                    mySearchAxis = G_Table.Axes[i];
                }
            }

            //輸出絕對位置
            double myPos;
            int mySpeedPercent;

            if (double.TryParse(myTextBox.Text, out myPos) && int.TryParse(myComboBox.Text, out mySpeedPercent))
            {
                ////實際距離換算成脈波數
                //int myPosPulse = (int)(myPos / mmPerPulse);
                ////將速度百分比換算成實際脈波數
                //int mySpeedPulse = (int)(MaxSpeedPulse * mySpeedPercent / 100);
                //絕對移動
                mySearchAxis.AbsMove(myPos, mySpeedPercent);
            }
            else
            {
                MessageBox.Show("請填入正確數字");
            }

        }

        private void btn_Motor_JogPlus_MouseDown(object sender, MouseEventArgs e)
        {
            Button myEventButton = (Button)sender;
            ComboBox myComboBox = null;
            int myAxisID = -1;

            UserClass_PCI7856_4XMO.Axis mySearchAxis = null; //"軸資訊"(由軸ID進行搜尋  最後由函數輸出命令)

            //判斷第幾個馬達的按鈕
            if (myEventButton.Name == "btn_Motor0_JogPlus")
            {
                myComboBox = (ComboBox)this.groupBox_Command_Motor0.Controls["comboBox_Motor0_Speed"];
                myAxisID = int.Parse(label_Axis0_ID.Text);
            }
            else if (myEventButton.Name == "btn_Motor1_JogPlus")
            {
                myComboBox = (ComboBox)this.groupBox_Command_Motor1.Controls["comboBox_Motor1_Speed"];
                myAxisID = int.Parse(label_Axis1_ID.Text);
            }
            else if (myEventButton.Name == "btn_Motor2_JogPlus")
            {
                myComboBox = (ComboBox)this.groupBox_Command_Motor2.Controls["comboBox_Motor2_Speed"];
                myAxisID = int.Parse(label_Axis2_ID.Text);
            }
            else if (myEventButton.Name == "btn_Motor3_JogPlus")
            {
                myComboBox = (ComboBox)this.groupBox_Command_Motor3.Controls["comboBox_Motor3_Speed"];
                myAxisID = int.Parse(label_Axis3_ID.Text);
            }
            else
            { MessageBox.Show("無此按鈕"); }

            //搜尋指定的Axis
            for (int i = 0; i < G_Table.Axes.Length; i++)
            {
                if (myAxisID == G_Table.Axes[i].ID)
                {
                    mySearchAxis = G_Table.Axes[i];
                }
            }

            //輸出相對位置 (mm)
            int myDistance = 10;
            int mySpeedPercent;

            if (int.TryParse(myComboBox.Text, out mySpeedPercent))
            {
                ////實際距離換算成脈波數
                //int myDistancePulse = (int)(myDistance / mmPerPulse);
                ////將速度百分比換算成實際脈波數
                //int mySpeedPulse = (int)(MaxSpeedPulse * mySpeedPercent / 100);
                //相對移動
                mySearchAxis.RelMove(myDistance, mySpeedPercent);
            }
            else
            {
                MessageBox.Show("請填入正確數字");
            }
        }

        private void btn_Motor_JogMinus_MouseDown(object sender, MouseEventArgs e)
        {
            Button myEventButton = (Button)sender;
            ComboBox myComboBox = null;
            int myAxisID = -1;

            UserClass_PCI7856_4XMO.Axis mySearchAxis = null; //"軸資訊"(由軸ID進行搜尋  最後由函數輸出命令)

            //判斷第幾個馬達的按鈕
            if (myEventButton.Name == "btn_Motor0_JogMinus")
            {
                myComboBox = (ComboBox)this.groupBox_Command_Motor0.Controls["comboBox_Motor0_Speed"];
                myAxisID = int.Parse(label_Axis0_ID.Text);
            }
            else if (myEventButton.Name == "btn_Motor1_JogMinus")
            {
                myComboBox = (ComboBox)this.groupBox_Command_Motor1.Controls["comboBox_Motor1_Speed"];
                myAxisID = int.Parse(label_Axis1_ID.Text);
            }
            else if (myEventButton.Name == "btn_Motor2_JogMinus")
            {
                myComboBox = (ComboBox)this.groupBox_Command_Motor2.Controls["comboBox_Motor2_Speed"];
                myAxisID = int.Parse(label_Axis2_ID.Text);
            }
            else if (myEventButton.Name == "btn_Motor3_JogMinus")
            {
                myComboBox = (ComboBox)this.groupBox_Command_Motor3.Controls["comboBox_Motor3_Speed"];
                myAxisID = int.Parse(label_Axis3_ID.Text);
            }
            else
            { MessageBox.Show("無此按鈕"); }

            //搜尋指定的Axis
            for (int i = 0; i < G_Table.Axes.Length; i++)
            {
                if (myAxisID == G_Table.Axes[i].ID)
                {
                    mySearchAxis = G_Table.Axes[i];
                }
            }

            //輸出相對位置 (mm)
            int myDistance = -10;
            int mySpeedPercent;

            if (int.TryParse(myComboBox.Text, out mySpeedPercent))
            {
                //實際距離換算成脈波數
                //int myDistancePulse = (int)(myDistance / mmPerPulse);
                ////將速度百分比換算成實際脈波數
                //int mySpeedPulse = (int)(MaxSpeedPulse * mySpeedPercent / 100);
                //相對移動
                mySearchAxis.RelMove(myDistance, mySpeedPercent);
            }
            else
            {
                MessageBox.Show("請填入正確數字");
            }
        }
        //全軸一起停
        private void btn_Motor_Stop_Click(object sender, EventArgs e)
        {

            G_Table.Emg_Stop();
        }

        private void btn_Motor_Home_Click(object sender, EventArgs e)
        {
            Button myEventButton = (Button)sender;
            int myAxisID = -1;

            UserClass_PCI7856_4XMO.Axis mySearchAxis = null; //"軸資訊"(由軸ID進行搜尋  最後由函數輸出命令)

            //判斷第幾個馬達的按鈕
            if (myEventButton.Name == "btn_Motor0_Home")
            {
                myAxisID = int.Parse(label_Axis0_ID.Text);
            }
            else if (myEventButton.Name == "btn_Motor1_Home")
            {
                myAxisID = int.Parse(label_Axis1_ID.Text);
            }
            else if (myEventButton.Name == "btn_Motor2_Home")
            {
                myAxisID = int.Parse(label_Axis2_ID.Text);
            }
            else if (myEventButton.Name == "btn_Motor3_Home")
            {
                myAxisID = int.Parse(label_Axis3_ID.Text);
            }
            else
            { MessageBox.Show("無此按鈕"); }

            //搜尋指定的Axis
            for (int i = 0; i < G_Table.Axes.Length; i++)
            {
                if (myAxisID == G_Table.Axes[i].ID)
                {
                    mySearchAxis = G_Table.Axes[i];
                }
            }
            //回Home
            mySearchAxis.GoHome();
        }
        //全軸一起Servo On
        private void btn_ServoOn_Click(object sender, EventArgs e)
        {
            G_Table.ServoOn();
        }

        //全軸一起Servo Off
        private void btn_ServoOff_Click(object sender, EventArgs e)
        {
            G_Table.ServoOff();
        }
        //儲存
        private void btn_Save_Click(object sender, EventArgs e)
        {
            //用目前是否可見判斷
            if (comboBox_H0_Priorty.Visible == true)
            { G_Table.moveSetting.AxesPriority_GoHome[0].Priority = (UserClass_PCI7856_4XMO.AxisPriority.MovePriority)comboBox_H0_Priorty.SelectedIndex; }
            if (comboBox_H1_Priorty.Visible == true)
            { G_Table.moveSetting.AxesPriority_GoHome[1].Priority = (UserClass_PCI7856_4XMO.AxisPriority.MovePriority)comboBox_H1_Priorty.SelectedIndex; }
            if (comboBox_H2_Priorty.Visible == true)
            { G_Table.moveSetting.AxesPriority_GoHome[2].Priority = (UserClass_PCI7856_4XMO.AxisPriority.MovePriority)comboBox_H2_Priorty.SelectedIndex; }
            if (comboBox_H3_Priorty.Visible == true)
            { G_Table.moveSetting.AxesPriority_GoHome[3].Priority = (UserClass_PCI7856_4XMO.AxisPriority.MovePriority)comboBox_H3_Priorty.SelectedIndex; }
            if (comboBox_J0_Priorty.Visible == true)
            { G_Table.moveSetting.AxesPriority_JUMP[0].Priority = (UserClass_PCI7856_4XMO.AxisPriority.MovePriority)comboBox_J0_Priorty.SelectedIndex; }
            if (comboBox_J1_Priorty.Visible == true)
            { G_Table.moveSetting.AxesPriority_JUMP[1].Priority = (UserClass_PCI7856_4XMO.AxisPriority.MovePriority)comboBox_J1_Priorty.SelectedIndex; }
            if (comboBox_J2_Priorty.Visible == true)
            { G_Table.moveSetting.AxesPriority_JUMP[2].Priority = (UserClass_PCI7856_4XMO.AxisPriority.MovePriority)comboBox_J2_Priorty.SelectedIndex; }
            if (comboBox_J3_Priorty.Visible == true)
            { G_Table.moveSetting.AxesPriority_JUMP[3].Priority = (UserClass_PCI7856_4XMO.AxisPriority.MovePriority)comboBox_J3_Priorty.SelectedIndex; }
            
            G_Table.MoveSettingFileSave();

        }
        //回復
        private void btn_Restore_Click(object sender, EventArgs e)
        {
            //用目前是否可見判斷
            if (comboBox_H0_Priorty.Visible == true)
            { comboBox_H0_Priorty.SelectedIndex = (int)G_Table.moveSetting.AxesPriority_GoHome[0].Priority; }
            if (comboBox_H1_Priorty.Visible == true)
            { comboBox_H1_Priorty.SelectedIndex = (int)G_Table.moveSetting.AxesPriority_GoHome[1].Priority; }
            if (comboBox_H2_Priorty.Visible == true)
            { comboBox_H2_Priorty.SelectedIndex = (int)G_Table.moveSetting.AxesPriority_GoHome[2].Priority; }
            if (comboBox_H3_Priorty.Visible == true)
            { comboBox_H3_Priorty.SelectedIndex = (int)G_Table.moveSetting.AxesPriority_GoHome[3].Priority; }
            if (comboBox_J0_Priorty.Visible == true)
            { comboBox_J0_Priorty.SelectedIndex = (int)G_Table.moveSetting.AxesPriority_JUMP[0].Priority; }
            if (comboBox_J1_Priorty.Visible == true)
            { comboBox_J1_Priorty.SelectedIndex = (int)G_Table.moveSetting.AxesPriority_JUMP[1].Priority; }
            if (comboBox_J2_Priorty.Visible == true)
            { comboBox_J2_Priorty.SelectedIndex = (int)G_Table.moveSetting.AxesPriority_JUMP[2].Priority; }
            if (comboBox_J3_Priorty.Visible == true)
            { comboBox_J3_Priorty.SelectedIndex = (int)G_Table.moveSetting.AxesPriority_JUMP[3].Priority; }
        }
        #endregion

        #region "Timer" 
        //顯示馬達狀態
        private void timer_StatusDisplay_Tick(object sender, EventArgs e)
        {
            //顯示FeedBack馬達狀態
            StatusDisplay();
        }
        #endregion

        #endregion

        #region "顯示用"
        //顯示Group(一開始先隱藏)
        private void GroupDisplay_AxisData(int _UserControl_Count, UserClass_PCI7856_4XMO.Axis _Axis, int _AxesNum)
        {
            //判斷前端是否出現錯誤
            if (!G_FunctionFail)
            {
                string myAxisName = _Axis.Name;
                int myAxisID = _Axis.ID;

                //改Group上的字與顯示Group
                if (_UserControl_Count == 0)
                {
                    groupBox_Motor0.Visible = true;
                    groupBox_Motor0.Text = "軸" + myAxisID + "-" + myAxisName;
                    label_Axis0_ID.Text = myAxisID.ToString();

                    //顯示各軸優先權的設定
                    label_H_Axis0_ID.Visible = true;
                    label_J_Axis0_ID.Visible = true;
                    comboBox_H0_Priorty.Visible = true;
                    comboBox_J0_Priorty.Visible = true;
                    label_H_Axis0_ID.Text = myAxisID.ToString();
                    label_J_Axis0_ID.Text = myAxisID.ToString();
                    comboBox_H0_Priorty.SelectedIndex = (int)G_Table.moveSetting.AxesPriority_GoHome[_UserControl_Count].Priority;
                    comboBox_J0_Priorty.SelectedIndex = (int)G_Table.moveSetting.AxesPriority_JUMP[_UserControl_Count].Priority;
                }
                else if (_UserControl_Count == 1)
                {
                    groupBox_Motor1.Visible = true;
                    groupBox_Motor1.Text = "軸" + myAxisID + "-" + myAxisName;
                    label_Axis1_ID.Text = myAxisID.ToString();

                    //顯示各軸優先權的設定
                    label_H_Axis1_ID.Visible = true;
                    label_J_Axis1_ID.Visible = true;
                    comboBox_H1_Priorty.Visible = true;
                    comboBox_J1_Priorty.Visible = true;
                    label_H_Axis1_ID.Text = myAxisID.ToString();
                    label_J_Axis1_ID.Text = myAxisID.ToString();
                    comboBox_H1_Priorty.SelectedIndex = (int)G_Table.moveSetting.AxesPriority_GoHome[_UserControl_Count].Priority;
                    comboBox_J1_Priorty.SelectedIndex = (int)G_Table.moveSetting.AxesPriority_JUMP[_UserControl_Count].Priority;
                }
                else if (_UserControl_Count == 2)
                {
                    groupBox_Motor2.Visible = true;
                    groupBox_Motor2.Text = "軸" + myAxisID + "-" + myAxisName;
                    label_Axis2_ID.Text = myAxisID.ToString();

                    //顯示各軸優先權的設定
                    label_H_Axis2_ID.Visible = true;
                    label_J_Axis2_ID.Visible = true;
                    comboBox_H2_Priorty.Visible = true;
                    comboBox_J2_Priorty.Visible = true;
                    label_H_Axis2_ID.Text = myAxisID.ToString();
                    label_J_Axis2_ID.Text = myAxisID.ToString();
                    comboBox_H2_Priorty.SelectedIndex = (int)G_Table.moveSetting.AxesPriority_GoHome[_UserControl_Count].Priority;
                    comboBox_J2_Priorty.SelectedIndex = (int)G_Table.moveSetting.AxesPriority_JUMP[_UserControl_Count].Priority;
                }
                else if (_UserControl_Count == 3)
                {
                    groupBox_Motor3.Visible = true;
                    groupBox_Motor3.Text = "軸" + myAxisID + "-" + myAxisName;
                    label_Axis3_ID.Text = myAxisID.ToString();

                    //顯示各軸優先權的設定
                    label_H_Axis3_ID.Visible = true;
                    label_J_Axis3_ID.Visible = true;
                    comboBox_H3_Priorty.Visible = true;
                    comboBox_J3_Priorty.Visible = true;
                    label_H_Axis3_ID.Text = myAxisID.ToString();
                    label_J_Axis3_ID.Text = myAxisID.ToString();
                    comboBox_H3_Priorty.SelectedIndex = (int)G_Table.moveSetting.AxesPriority_GoHome[_UserControl_Count].Priority;
                    comboBox_J3_Priorty.SelectedIndex = (int)G_Table.moveSetting.AxesPriority_JUMP[_UserControl_Count].Priority;
                }
                
            }
        }

        //顯示馬達狀態
        private void StatusDisplay()
        {
            for (int i = 0; i < G_Table.Axes.Length; i++)
            {
                Labels_FB_Command[i].Text = (G_Table.Axes[i].FeedBackCommandPos).ToString(); //*mmPerPulse將脈波換算成實際距離
                Labels_FB_Pos[i].Text = (G_Table.Axes[i].FeedBackPos).ToString();//*mmPerPulse將脈波換算成實際距離
                //HMV
                if (G_Table.Axes[i].axis_Status.Mts_HMV == true)
                { PictureBox_FB_HMV[i].BackColor = Color.Lime; }
                else
                { PictureBox_FB_HMV[i].BackColor = Color.Gray; }
                //SMV
                if (G_Table.Axes[i].axis_Status.Mts_SMV == true)
                { PictureBox_FB_SMV[i].BackColor = Color.Lime; }
                else
                { PictureBox_FB_SMV[i].BackColor = Color.Gray; }
                //ALM
                if (G_Table.Axes[i].axis_Status.Mio_ALM == true)
                { PictureBox_FB_ALM[i].BackColor = Color.Red; }
                else
                { PictureBox_FB_ALM[i].BackColor = Color.Gray; }
                //PEL
                if (G_Table.Axes[i].axis_Status.Mio_PEL == true)
                { PictureBox_FB_PEL[i].BackColor = Color.Red; }
                else
                { PictureBox_FB_PEL[i].BackColor = Color.Gray; }
                //MEL
                if (G_Table.Axes[i].axis_Status.Mio_MEL == true)
                { PictureBox_FB_MEL[i].BackColor = Color.Red; }
                else
                { PictureBox_FB_MEL[i].BackColor = Color.Gray; }
                //ORG
                if (G_Table.Axes[i].axis_Status.Mio_ORG == true)
                { PictureBox_FB_ORG[i].BackColor = Color.Red; }
                else
                { PictureBox_FB_ORG[i].BackColor = Color.Gray; }
                //EMG
                if (G_Table.Axes[i].axis_Status.Mio_EMG == true)
                { PictureBox_FB_EMG[i].BackColor = Color.Red; }
                else
                { PictureBox_FB_EMG[i].BackColor = Color.Gray; }
                //SVON
                if (G_Table.Axes[i].axis_Status.Mio_SVON == true)
                { PictureBox_FB_SVON[i].BackColor = Color.Red; }
                else
                { PictureBox_FB_SVON[i].BackColor = Color.Gray; }


            }
            //==================顯示Table狀態==================
            //SMV
            if (G_Table.table_Status.SMV == true)
            { pictureBox_Table_SMV.BackColor = Color.Lime; }
            else
            { pictureBox_Table_SMV.BackColor = Color.Gray; }
            //HMV
            if (G_Table.table_Status.HMV == true)
            { pictureBox_Table_HMV.BackColor = Color.Lime; }
            else
            { pictureBox_Table_HMV.BackColor = Color.Gray; }
            //ALM
            if (G_Table.table_Status.ALM == true)
            { pictureBox_Table_ALM.BackColor = Color.Red; }
            else
            { pictureBox_Table_ALM.BackColor = Color.Gray; }
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            List<int> a = new List<int>();
            a = G_Table.moveSetting.PriorityAxis_JUMP(UserClass_PCI7856_4XMO.AxisPriority.MovePriority.First);
            a = G_Table.moveSetting.PriorityAxis_JUMP(UserClass_PCI7856_4XMO.AxisPriority.MovePriority.Second);
            a = G_Table.moveSetting.PriorityAxis_JUMP(UserClass_PCI7856_4XMO.AxisPriority.MovePriority.Third);
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            G_Table.Jump("shell_8",20);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            G_Table.GoHome();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            G_Table.Jump("shell_1", 20, -5 );

        }

        
        private void btn_Reset_Click(object sender, EventArgs e)
        {
            G_Table.Reset();
        }
        private void btn_Motor0_JogPlus_Click(object sender, EventArgs e)
        {

        }

       

        

       
        

        



        

        

        

    }
}
