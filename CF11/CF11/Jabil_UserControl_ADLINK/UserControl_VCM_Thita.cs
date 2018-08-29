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
    public partial class UserControl_VCM_Thita : UserControl
    {
        public UserClass_PCI7856_4XMO.Axis G_Axis;

        Thread Thread_StatusGet;

        public UserControl_VCM_Thita()
        {
            InitializeComponent();

            

        }

        public void Initialize(UserClass_PCI7856_4XMO.Axis _Axis)
        {
            G_Axis = _Axis;

            int ErrorCodeCount = 0;

            //IO and Pulse Mode
            ErrorCodeCount += APS168.APS_set_axis_param(G_Axis.ID, (Int32)APS_Define.PRA_ALM_LOGIC, 0);       //Set PRA_ALM_LOGIC       1: Active high
            ErrorCodeCount += APS168.APS_set_axis_param(G_Axis.ID, (Int32)APS_Define.PRA_PLS_IPT_MODE, 2);    //Set PRA_PLS_IPT_MODE    2: A/B X4
            ErrorCodeCount += APS168.APS_set_axis_param(G_Axis.ID, (Int32)APS_Define.PRA_PLS_OPT_MODE, 2);    //Set PRA_PLS_OPT_MODE    4: CW/CCW (AH)
            ErrorCodeCount += APS168.APS_set_axis_param(G_Axis.ID, (Int32)APS_Define.PRA_PLS_IPT_LOGIC, 0);   //Set PRA_PLS_IPT_LOGIC   0: don not reverse counting direction
            //
            ErrorCodeCount += APS168.APS_set_axis_param(G_Axis.ID, (Int32)APS_Define.PRA_FEEDBACK_SRC, 1);    //Select feedback conter  0: Ext. Encoder mode  1: Stepper mode 

            ErrorCodeCount += APS168.APS_set_axis_param(G_Axis.ID, (Int32)APS_Define.PRA_SERVO_LOGIC, 0);     //Set SERVO output logic  0: Active low

            //Single Move Parameter
            ErrorCodeCount += APS168.APS_set_axis_param(G_Axis.ID, (Int32)APS_Define.PRA_CURVE, 1);			  //Set PRA_CURVE  0:T-Curve 1:S-Curve
            ErrorCodeCount += APS168.APS_set_axis_param(G_Axis.ID, (Int32)APS_Define.PRA_ACC, 300000);		  //Set PRA_ACC 
            ErrorCodeCount += APS168.APS_set_axis_param(G_Axis.ID, (Int32)APS_Define.PRA_DEC, 300000);		  //Set PRA_DEC
            ErrorCodeCount += APS168.APS_set_axis_param(G_Axis.ID, (Int32)APS_Define.PRA_VS, 0);			  //Set PRA_VS (Set homing start velocity)

            //Home Move Parameter
            ErrorCodeCount += APS168.APS_set_axis_param(G_Axis.ID, (Int32)APS_Define.PRA_ORG_LOGIC, 1);       //1: Inverse
            //ErrorCodeCount += APS168.APS_set_axis_param(_AxisID, (Int32)APS_Define.PRA_HOME_MODE, 0);		  //Set PRA_HOME_MODE
            ErrorCodeCount += APS168.APS_set_axis_param(G_Axis.ID, (Int32)APS_Define.PRA_HOME_MODE, 1);		  //Set PRA_HOME_MODE
            ErrorCodeCount += APS168.APS_set_axis_param(G_Axis.ID, (Int32)APS_Define.PRA_HOME_DIR, 1);		  //Set PRA_HOME_DIR    0:Positive 1:Negative
            ErrorCodeCount += APS168.APS_set_axis_param(G_Axis.ID, (Int32)APS_Define.PRA_HOME_VM, 50000);	  //Set PRA_HOME_VM     (Set homing maximum velocity. )
            ErrorCodeCount += APS168.APS_set_axis_param(G_Axis.ID, (Int32)APS_Define.PRA_HOME_EZA, 0);		  //Set PRA_HOME_EZA    (0: Not enabl)
            ErrorCodeCount += APS168.APS_set_axis_param(G_Axis.ID, (Int32)APS_Define.PRA_HOME_VO, 10);		  //Set PRA_HOME_VO     (Homing leave home velocity)
            ErrorCodeCount += APS168.APS_set_axis_param(G_Axis.ID, (Int32)APS_Define.PRA_HOME_VM, 50000);       //Set PRA_HOME_VM     Homing speed
            ErrorCodeCount += APS168.APS_set_axis_param(G_Axis.ID, (Int32)APS_Define.PRA_HOME_OFFSET, 500);	  //Set PRA_HOME_OFFSET (Homing leave home distance – Specify ORG offset)
            ErrorCodeCount += APS168.APS_set_axis_param(G_Axis.ID, (Int32)APS_Define.PRA_PLS_IPT_LOGIC, 1);   //Set PRA_PLS_IPT_LOGIC   1: reverse counting direction

            if (ErrorCodeCount == 0)
            {
                //取得FeedBack值
                Thread_StatusGet = new Thread(Status_Get);
                Thread_StatusGet.Start();
                //顯示FeedBack值
                timer_StatusDisplay.Enabled = true;

                label_Axis0_ID.Text = G_Axis.ID.ToString() ;
            }
        }

        private void btn_Motor0_Move_Click(object sender, EventArgs e)
        {
                //輸出絕對角度
                double myAngle;
                int mySpeedPercent;

                if (double.TryParse(textBox_Angle.Text, out myAngle) && int.TryParse(comboBox_Speed.Text, out mySpeedPercent))
                {
                    //絕對移動
                    G_Axis.AbsMove(myAngle, mySpeedPercent);
                }
                else
                {
                    MessageBox.Show("請填入正確數字");
                }
               
            
        }

        private void btn_Motor0_JogPlus_Click(object sender, EventArgs e)
        {

            int mySpeedPercent;

            if ( int.TryParse(comboBox_Speed.Text, out mySpeedPercent))
            {
                //絕對移動
                G_Axis.RelMove(90, mySpeedPercent);
            }
            else
            {
                MessageBox.Show("請填入正確數字");
            }
        }

        private void btn_Motor0_JogMinus_Click(object sender, EventArgs e)
        {
            int mySpeedPercent;

            if (int.TryParse(comboBox_Speed.Text, out mySpeedPercent))
            {
                //絕對移動
                G_Axis.RelMove(-90, mySpeedPercent);
            }
            else
            {
                MessageBox.Show("請填入正確數字");
            }
        }

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

        
        //取得馬達回饋資訊
        private void AxisData_Get()
        {
            //取得Axis狀態值
            AxisStatus_Get();
           
        }
    
        //取軸卡的狀態
        public void AxisStatus_Get()
        {
            Int32 CommandPulse = 0;
            Int32 PositionPulse = 0;
            Int32 MotionSts = 0;
            Int32 IoSts = 0;

            //取值(FeedBack Data)
            APS168.APS_get_command(G_Axis.ID, ref CommandPulse);
            APS168.APS_get_position(G_Axis.ID, ref PositionPulse);

            IoSts = APS168.APS_motion_io_status(G_Axis.ID);
            MotionSts = APS168.APS_motion_status(G_Axis.ID);

            //放值
            G_Axis.FeedBackCommandPulse = CommandPulse;
            G_Axis.FeedBackPulse = PositionPulse;
            G_Axis.FeedBackMotionStatus = MotionSts;
            G_Axis.FeedBackIO_Status = IoSts;
        }

        //關閉
        public void Close()
        {
            if (Thread_StatusGet != null)
            {
                if (Thread_StatusGet.IsAlive)
                {
                    Thread_StatusGet.Abort();
                }
            }
        }
        private void timer_StatusDisplay_Tick(object sender, EventArgs e)
        {
            label_Motor0_FB_Command.Text = (G_Axis.FeedBackCommandPos).ToString(); 
            label_Motor0_FB_Pos.Text = (G_Axis.FeedBackPos).ToString();
            //HMV
            if (G_Axis.axis_Status.Mts_HMV == true)
            { pictureBox_Motor0_HMV.BackColor = Color.Lime; }
            else
            { pictureBox_Motor0_HMV.BackColor = Color.Gray; }
            //SMV
            if (G_Axis.axis_Status.Mts_SMV == true)
            { pictureBox_Motor0_SMV.BackColor = Color.Lime; }
            else
            { pictureBox_Motor0_SMV.BackColor = Color.Gray; }
            //ALM
            if (G_Axis.axis_Status.Mio_ALM == true)
            { pictureBox_Motor0_ALM.BackColor = Color.Red; }
            else
            { pictureBox_Motor0_ALM.BackColor = Color.Gray; }
            //PEL
            if (G_Axis.axis_Status.Mio_PEL == true)
            { pictureBox_Motor0_PEL.BackColor = Color.Red; }
            else
            { pictureBox_Motor0_PEL.BackColor = Color.Gray; }
            //MEL
            if (G_Axis.axis_Status.Mio_MEL == true)
            { pictureBox_Motor0_MEL.BackColor = Color.Red; }
            else
            { pictureBox_Motor0_MEL.BackColor = Color.Gray; }
            //ORG
            if (G_Axis.axis_Status.Mio_ORG == true)
            { pictureBox_Motor0_ORG.BackColor = Color.Red; }
            else
            { pictureBox_Motor0_ORG.BackColor = Color.Gray; }
            //EMG
            if (G_Axis.axis_Status.Mio_EMG == true)
            { pictureBox_Motor0_EMG.BackColor = Color.Red; }
            else
            { pictureBox_Motor0_EMG.BackColor = Color.Gray; }
            //SVON
            if (G_Axis.axis_Status.Mio_SVON == true)
            { pictureBox_Motor0_SVON.BackColor = Color.Red; }
            else
            { pictureBox_Motor0_SVON.BackColor = Color.Gray; }
        }

        private void btn_Motor0_Reset_Click(object sender, EventArgs e)
        {
            G_Axis.Reset();
        }
    }
}