using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Jabil_UserControl_ADLINK
{
    public partial class UserControl_PCI7230_ : UserControl
    {
        Label[] G_Labels_DI = new Label[16];
        Label[] G_Labels_DO = new Label[16];
        PictureBox[] G_PictureBoxs_DI = new PictureBox[16];
        PictureBox[] G_PictureBoxs_DO = new PictureBox[16];

        ushort G_PCI_7230Card;

        #region "初始建構式"
        //輸入參數為第幾張卡
        public UserControl_PCI7230_()
        {
            InitializeComponent();

            
            for (int i = 0; i < 16; i++)
            {
                string label_DI = "label_DI"+i;
                string label_DO = "label_DO"+i;
                string pictureBox_DI = "pictureBox_DI"+i;
                string pictureBox_DO = "pictureBox_DO"+i;

                G_Labels_DI[i] = (Label)this.groupBox_DI.Controls[label_DI];
                G_Labels_DO[i] = (Label)this.groupBox_DO.Controls[label_DO];
                G_PictureBoxs_DI[i] = (PictureBox)this.groupBox_DI.Controls[pictureBox_DI];
                G_PictureBoxs_DO[i] = (PictureBox)this.groupBox_DO.Controls[pictureBox_DO];
            }
  
        }
        #endregion

        #region "功能"

        //初始化
        public void Initialize(int _CardNum)
        {
            //配給資源
            if (Register(_CardNum))
            {
                label_CardNum.Text = _CardNum.ToString();
              
                timer_ScanDisp.Enabled  = true  ;
            }
            else
            { MessageBox.Show("未讀取到PCI-7230"); }
        }

        public enum IO  //命名用
        { Input, Output }

        //DI DO命名
        public void Naming(IO _IO,int _Index,string _Text)
        {
            if (_IO == IO.Output)
            {
                G_Labels_DO[_Index].Text = G_Labels_DO[_Index].Text + "-" + _Text;
            }
            else
            {
                G_Labels_DI[_Index].Text = G_Labels_DI[_Index].Text + "-" + _Text;
            }
        }
       

        //DI與DO的Port皆為0~15

        //PCI-7230 註冊
        private bool Register(int _CardNum)
        {
            G_PCI_7230Card = (ushort)DASK.Register_Card(DASK.PCI_7230, (ushort)_CardNum); //Register_Card (CardType,同類型第1張卡)

            if (G_PCI_7230Card == 65523)
            { return false; }

            //Register後，輸出會全關，但實際訊號不變
            int SW15 = SW_O(15);
            if (SW15 == 0)
            { Off(15); }
            else
            { On(15); }

            return true;
        }

        //PCI-7230 讀取指定bit的DI
        public ushort SW_I(int _ReadBit)
        {
            ushort BitResult;
            ushort ReturnValue;

            ReturnValue = 1;
            DASK.DI_ReadLine(G_PCI_7230Card, 0, (ushort)_ReadBit, out BitResult);
            
            if (BitResult==0)
            {ReturnValue = 0;}

            return ReturnValue; //回傳結果
        }

        //PCI-7230 讀取指定名稱的DI
        public ushort SW_I(string _ReadName)
        {
            int _ReadBit=0;
            ushort BitResult;
            ushort ReturnValue;

            ReturnValue = 1;

            for (int i = 0; i < 16; i++)
            {
                //判斷內部是否含- (有命名才會產生-)，不含則跑下一次迴圈
                if (G_Labels_DI[i].Text.Contains("-")==false)
                { continue; }

                //以-拆解字串
                string[] myDI_Name = G_Labels_DI[i].Text.Split('-');
                if (myDI_Name[1] == _ReadName)
                { _ReadBit = i; break; }
                //查無此名稱
                if (i == 15)
                { MessageBox.Show("DI查無此名稱，請重新確認"); return (99); }
            }

            DASK.DI_ReadLine(G_PCI_7230Card, 0, (ushort)_ReadBit, out BitResult);

            if (BitResult == 0)
            { ReturnValue = 0; }

            return ReturnValue; //回傳結果
        }

        //PCI-7230 讀取指定bit的DO
        public ushort SW_O(int _ReadBit)
        {
            ushort BitResult;
            DASK.DO_ReadLine(G_PCI_7230Card, 0, (ushort)_ReadBit, out BitResult);
            return BitResult;        //回傳結果
        }

        //PCI-7230 讀取指定名稱的DO
        public ushort SW_O(string _ReadName)
        {
            int _ReadBit = 0;
            for (int i = 0; i < 16; i++)
            {
                //判斷內部是否含- (有命名才會產生-)，不含則跑下一次迴圈
                if (G_Labels_DO[i].Text.Contains("-") == false)
                { continue; }

                //以-拆解字串
                string[] myDO_Name = G_Labels_DO[i].Text.Split('-');
                if (myDO_Name[1] == _ReadName)
                { _ReadBit = i; break; }
                //查無此名稱
                if (i == 15)
                { MessageBox.Show("DO查無此名稱，請重新確認"); return (99); }
            }

            ushort BitResult;
            DASK.DO_ReadLine(G_PCI_7230Card, 0, (ushort)_ReadBit, out BitResult);
            return BitResult;        //回傳結果
        }

        //PCI-7230 打開指定bit的DO
        public void On(int _WriteBit)
        {
            DASK.DO_WriteLine(G_PCI_7230Card, 0, (ushort)_WriteBit, 1);
        }

        //PCI-7230 打開指定名稱的DO
        public void On(string _WriteName)
        {
            int _WriteBit = 0;

            for (int i = 0; i < 16; i++)
            {
                //判斷內部是否含- (有命名才會產生-)，不含則跑下一次迴圈
                if (G_Labels_DO[i].Text.Contains("-") == false)
                { continue; }

                //以-拆解字串
                string[] myDO_Name = G_Labels_DO[i].Text.Split('-');
                if (myDO_Name[1] == _WriteName)
                { _WriteBit = i; break; }
                //查無此名稱
                if (i == 15)
                { MessageBox.Show("DO查無此名稱，請重新確認"); return ; }
            }

            DASK.DO_WriteLine(G_PCI_7230Card, 0, (ushort)_WriteBit, 1);
        }

        //PCI-7230 關閉指定bit的DO
        public void Off(int _WriteBit)
        {
            DASK.DO_WriteLine(G_PCI_7230Card, 0, (ushort)_WriteBit, 0);
        }

        //PCI-7230 關閉指定名稱的DO
        public void Off(string _WriteName)
        {
            int _WriteBit = 0;

            for (int i = 0; i < 16; i++)
            {
                //判斷內部是否含- (有命名才會產生-)，不含則跑下一次迴圈
                if (G_Labels_DO[i].Text.Contains("-") == false)
                { continue; }

                //以-拆解字串
                string[] myDO_Name = G_Labels_DO[i].Text.Split('-');
                if (myDO_Name[1] == _WriteName)
                { _WriteBit = i; break; }
                //查無此名稱
                if (i == 15)
                { MessageBox.Show("DO查無此名稱，請重新確認"); return; }
            }

            DASK.DO_WriteLine(G_PCI_7230Card, 0, (ushort)_WriteBit, 0);
        }

        //PCI-7230 重置所有的DO為off
        public void Off_All()
        {
            DASK.DO_WritePort(G_PCI_7230Card, 0, 0);
        }

        //關閉(釋放資源)
        public void Close()
        {

            //PCI-7230 釋放資源
            DASK.Release_Card(G_PCI_7230Card);

        }
        //PictureBox顯示DIO狀態
        private void PictureBox_Show()
        {
            for (int i = 0; i < 16; i++)
            {
                ushort DI_Status;
                ushort DO_Status;
                DI_Status = SW_I(i);
                DO_Status = SW_O(i);

                if (DI_Status == 1)
                { G_PictureBoxs_DI[i].BackColor = Color.Green; }
                else
                { G_PictureBoxs_DI[i].BackColor = Color.Red; }

                if (DO_Status == 1)
                { G_PictureBoxs_DO[i].BackColor = Color.Green; }
                else
                { G_PictureBoxs_DO[i].BackColor = Color.Red; }
            }
        }

        
        #endregion

        #region "事件函式"
        //DO 的Click觸發
        private void pictureBox_DO_DoubleClick(object sender, EventArgs e)
        {
            int myIndex = 0;

            //查詢觸發
            for (int i = 0; i < 16; i++)
            {
                if (G_PictureBoxs_DO[i] == (PictureBox)sender)
                {
                    myIndex = i; break;
                }
            }

            //改變狀態
            if (SW_O(myIndex) == 0)
            { On(myIndex); }
            else
            { Off(myIndex); }

        }

        //Timer(顯示狀態)
        private void timer_ScanDisp_Tick(object sender, EventArgs e)
        {
            PictureBox_Show();
        }
        #endregion
    }
    
}
