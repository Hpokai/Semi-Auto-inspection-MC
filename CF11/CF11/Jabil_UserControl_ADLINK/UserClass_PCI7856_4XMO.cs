using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using APS168_W32;
using System.Threading;
using APS_Define_W32;

namespace Jabil_UserControl_ADLINK
{
    public class UserClass_PCI7856_4XMO
    {
        
        //==================平台==================
        public class Table
        {
            private string Name_;   //平台名稱
            private Axis[] Axes_;    //平台的各軸
            private int AxesNumber_;   //平台的軸數量

            private TeachPoints teachPoints_ ;
            private MoveSetting moveSetting_ ;

            //Table狀態(SMV, HMV, ALM)
            private Table_Status table_Status_;

            Thread Thread_Go;
            public bool GoOK = true;   //避免連按兩次

            //Jump的執行緒，避免鎖住主執行緒
            Thread Thread_Jump;
            public bool JumpOK =true;   //避免連按兩次

            //GoHome的執行緒，避免鎖住主執行緒
            Thread Thread_GoHome;
            public bool GoHomeOK = true;   //避免連按兩次
            
            private string G_FolderPath_Pts = System.IO.Directory.GetCurrentDirectory() + @"\TeachPoints";
            private string G_FilePath_Pts = null;
            private string G_FileExtension_Pts = ".jab_pts";

            private string G_FolderPath_Setting = System.IO.Directory.GetCurrentDirectory() + @"\Setting";
            private string G_FilePath_Setting = null;
            private string G_FileExtension_Setting = ".jab_setting";
            
            public Table(string _Name, Axis[] _Axes)
            {
                //判斷是否超過四軸
                if (_Axes.Length <= 4)
                { AxesNumber_ = _Axes.Length; }
                else
                { MessageBox.Show("Table初始化超過四軸"); return; }

                if (AxisNameID_Check(_Axes))
                {
                    Name_ = _Name;
                    Axes_ = _Axes;
                }
                else
                { MessageBox.Show("ID或名稱重複"); return; }

                //教點檔案路徑(TeachPoints)
                G_FilePath_Pts = G_FolderPath_Pts + @"\" + _Name + G_FileExtension_Pts;
                //讀取教點檔案，若無檔案則另存一空白檔案
                TeachPointsFileLoad();

                //移動設定檔案路徑(MoveSetting)
                G_FilePath_Setting = G_FolderPath_Setting + @"\" + _Name + G_FileExtension_Setting;
                //讀取移動設定檔案，若無檔案則另存一空白檔案
                SettingFileLoad();
                
            }

            #region 屬性
            public string Name
            {
                get { return Name_; }
            }

            public Axis[] Axes
            {
                get { return Axes_; }
            }

            public int AxesNumber
            {
                get { return AxesNumber_; }
            }

            public TeachPoints teachPoints
            {
                get { return teachPoints_; }
            }

            public MoveSetting moveSetting
            {
                get { return moveSetting_; }
            }

            public Table_Status table_Status
            {
                get
                {
                    return table_Status_;
                }
            }

            #endregion
            
            //判斷AxisID與名稱是否重複(Table內比較)
            private bool AxisNameID_Check(Axis[] _Axes)
            {

                //判斷AxisID是否重複
                for (int i = 0; i < _Axes.Length; i++)
                {
                    for (int j = 0; j < _Axes.Length; j++)
                    {
                        if (i != j)
                        {
                            if (_Axes[i].ID == _Axes[j].ID || _Axes[i].Name == _Axes[j].Name)
                            { return false; }
                        }
                    }
                }
                return true;
            }

            #region 儲存與讀取檔案相關

            #region 校點
            //儲存教點檔案
            public void TeachPointsFileSave()
            {
                TeachPointsSerialize();
            }

            //讀取教點檔案，若無檔案則另存一空白檔案
            public void TeachPointsFileLoad()
            {
                //判斷資料夾是否存在
                if (Directory.Exists(G_FolderPath_Pts) == false)
                {
                    System.IO.Directory.CreateDirectory(G_FolderPath_Pts);
                }

                //確認是否有檔案
                if (System.IO.File.Exists(G_FilePath_Pts))
                { teachPoints_ = TeachPointsDeserialize(); }
                else
                {
                    teachPoints_ = new TeachPoints();
                    TeachPointsSerialize();
                }


                //foreach (string file_Name in Directory.GetFileSystemEntries(G_FolderPath))   //尋找目錄中的檔案(全名)
                //{
                //    string myExt = System.IO.Path.GetExtension(file_Name); //副檔名
                //    if (myExt == ".jab")
                //    {
                //        string myFileName = System.IO.Path.GetFileNameWithoutExtension(file_Name); //主檔名

                //        if (myFileName == _FileName)
                //        {
                //            TeachPoints_ = TeachPointsDeserialize();
                //            return;
                //        }
                //    }

                //}
                //TeachPoints_ = new TeachPoints(PointNumber_);
                //TeachPointsSerialize();
            }

            //教點序列化
            private void TeachPointsSerialize()
            {

                FileStream myFileStream = new FileStream(G_FilePath_Pts, FileMode.Create);  //建立與path的資料流以便binaryFormatter寫入序列化資訊
                BinaryFormatter myBF = new BinaryFormatter(); //BinaryFormatter為跟記憶體溝通的必備物件

                myBF.Serialize(myFileStream, (object)teachPoints_);
                myFileStream.Close();
            }

            //教點反序列化
            private TeachPoints TeachPointsDeserialize()
            {
                FileStream myFileStream = new FileStream(G_FilePath_Pts, FileMode.Open);
                BinaryFormatter myBF = new BinaryFormatter();
                Object myTeachPoints = myBF.Deserialize(myFileStream);
                myFileStream.Close();
                return (TeachPoints)myTeachPoints;
            }
            #endregion

            #region 移動設定
            //儲存移動設定檔案
            public void MoveSettingFileSave()
            {
                MoveSettingSerialize();
            }

            //讀取移動設定檔案，若無檔案則另存一空白檔案
            public void SettingFileLoad()
            {
                //判斷資料夾是否存在
                if (Directory.Exists(G_FolderPath_Setting) == false)
                {
                    System.IO.Directory.CreateDirectory(G_FolderPath_Setting);
                }

                //確認是否有檔案
                if (System.IO.File.Exists(G_FilePath_Setting))
                { moveSetting_ = MoveSettingDeserialize(); }
                else
                {
                    moveSetting_ = new MoveSetting(Axes_);
                    MoveSettingSerialize();
                }
            }

            //移動設定序列化
            private void MoveSettingSerialize()
            {

                FileStream myFileStream = new FileStream(G_FilePath_Setting, FileMode.Create);  //建立與path的資料流以便binaryFormatter寫入序列化資訊
                BinaryFormatter myBF = new BinaryFormatter(); //BinaryFormatter為跟記憶體溝通的必備物件

                myBF.Serialize(myFileStream, (object)moveSetting_);
                myFileStream.Close();
            }

            //移動設定反序列化
            private MoveSetting MoveSettingDeserialize()
            {
                FileStream myFileStream = new FileStream(G_FilePath_Setting, FileMode.Open);
                BinaryFormatter myBF = new BinaryFormatter();
                Object myMoveSetting = myBF.Deserialize(myFileStream);
                myFileStream.Close();
                return (MoveSetting)myMoveSetting;
            }
            #endregion
            #endregion

            #region 平台移動相關方法
            //各軸絕對移動(同動) 引數: 座標值
            public void Go(double[] _Coordinate, int _SpeedPercent)
            {
                if (GoOK == false || JumpOK == false || GoHomeOK == false)
                { return; }

                GoOK = false;
                Thread_Go = new Thread(delegate()
                {
                    for (int i = 0; i < Axes_.Length; i++)
                    {
                        Axes_[i].AbsMove(_Coordinate[i], _SpeedPercent);
                    }
                    AxisStatus_Get();
                    TableStatus_Get();
                    do
                    { Thread.Sleep(50); }
                    while (table_Status_.SMV);

                    GoOK = true;
                });
                Thread_Go.Start();
                
            }

            //各軸絕對移動(同動) 引數: 點位名稱
            public void Go(string _PointName, int _SpeedPercent)
            {
                if (GoOK == false || JumpOK == false || GoHomeOK == false)
                { return; }

                //取得點位
                double[] myCoordinate = teachPoints.PointGet(_PointName).Coordinate;
                if (myCoordinate == null)
                { return; }

                GoOK = false;
                Thread_Go = new Thread(delegate()
                {
                    for (int i = 0; i < Axes_.Length; i++)
                    {
                        Axes_[i].AbsMove(myCoordinate[i], _SpeedPercent);
                    }
                    AxisStatus_Get();
                    TableStatus_Get();
                    do
                    { Thread.Sleep(50); }
                    while (table_Status_.SMV);

                    GoOK = true;
                });

                Thread_Go.Start();
            }

            //各軸絕對移動(同動) 引數: 點位index
            public void Go(int _PointIndex, int _SpeedPercent)
            {
                if (GoOK == false || JumpOK == false || GoHomeOK == false)
                { return; }

                //取得點位
                double[] myCoordinate = teachPoints.PointGet(_PointIndex).Coordinate;
                if (myCoordinate == null)
                { return; }

                GoOK = false;
                Thread_Go = new Thread(delegate()
                {
                    for (int i = 0; i < Axes_.Length; i++)
                    {
                        Axes_[i].AbsMove(myCoordinate[i], _SpeedPercent);
                    }
                    AxisStatus_Get();
                    TableStatus_Get();
                    do
                    { Thread.Sleep(50); }
                    while (table_Status_.SMV);

                    GoOK = true;
                });

                Thread_Go.Start();
            }

            #region Jump
            //各軸順序動(避免撞機)   (Z軸不會全拉，只會拉相對於目前位置的指定高度)
            public void Jump(string _PointName, int _SpeedPercent, double _RelZ)
            {
                //避免連按兩次Jump(不跳警告，因為會鎖住視窗不能按急停)
                if (GoOK == false || JumpOK == false || GoHomeOK == false)
                { return; }

                //取得點位
                double[] myCoordinate = teachPoints.PointGet(_PointName).Coordinate;
                if (myCoordinate == null)
                { return; }

                JumpOK = false;
                //(先照順序收，再照順序動)
                Thread_Jump = new Thread(delegate()
                {
                    //避免鎖住主執行緒
                    List<int> myPriorityAxesID = new List<int>();

                    //==============================================先照順序收(但不包含第一順位)  避免撞機請複製
                    for (int i = 4; i > 0; i--)   //順序最多為4
                    {
                        myPriorityAxesID = null;
                        //最多
                        myPriorityAxesID = moveSetting.PriorityAxis_JUMP((UserClass_PCI7856_4XMO.AxisPriority.MovePriority)i);

                        if (myPriorityAxesID != null)
                        {
                            //比較ID是否相同
                            foreach (Axis mySearchAxis in Axes_)
                            {
                                foreach (int myPriorityAxisID in myPriorityAxesID)
                                {
                                    if (mySearchAxis.ID == myPriorityAxisID)
                                    {

                                        mySearchAxis.RelMove(_RelZ, _SpeedPercent);

                                    } //===========收Z軸
                                }
                            }
                            //避免掃描的時序尚未更新狀態
                            AxisStatus_Get();
                            TableStatus_Get();
                            do
                            { Thread.Sleep(50); }
                            while (table_Status_.SMV);
                        }

                    }
                    //==============================================


                    //照順序動
                    for (int i = 0; i < 4; i++)   //順序最多為4
                    {
                        myPriorityAxesID = null;
                        //最多
                        myPriorityAxesID = moveSetting.PriorityAxis_JUMP((UserClass_PCI7856_4XMO.AxisPriority.MovePriority)i);

                        if (myPriorityAxesID != null)
                        {
                            //比較ID是否相同
                            foreach (Axis mySearchAxis in Axes_)
                            {
                                foreach (int myPriorityAxisID in myPriorityAxesID)
                                {
                                    if (mySearchAxis.ID == myPriorityAxisID)
                                    {
                                        mySearchAxis.AbsMove(myCoordinate[myPriorityAxisID], _SpeedPercent);
                                    }
                                }
                            }
                            //避免掃描的時序尚未更新狀態
                            AxisStatus_Get();
                            TableStatus_Get();
                            do
                            { Thread.Sleep(50); }
                            while (table_Status_.SMV);
                        }

                    }

                    JumpOK = true;
                });
                Thread_Jump.Start();
            }

            //各軸絕對移動(照順序動)  (全拉)
            public void Jump(string _PointName, int _SpeedPercent)
            {
                //避免連按兩次Jump(不跳警告，因為會鎖住視窗不能按急停)
                if (GoOK == false || JumpOK == false || GoHomeOK == false)
                { return; }

                //取得點位
                double[] myCoordinate = teachPoints.PointGet(_PointName).Coordinate;
                if (myCoordinate == null)
                { return; }

                JumpOK = false;
                Thread_Jump = new Thread(delegate()
                {
                    //避免鎖住主執行緒
                    List<int> myPriorityAxesID = new List<int>();

                    //先照逆順序收(但不包含第一順位)  
                    for (int i = 4; i > 0; i--)   //順序最多為4
                    {
                        myPriorityAxesID = null;
                        //最多
                        myPriorityAxesID = moveSetting.PriorityAxis_JUMP((UserClass_PCI7856_4XMO.AxisPriority.MovePriority)i);

                        if (myPriorityAxesID != null)
                        {
                            //比較ID是否相同
                            foreach (Axis mySearchAxis in Axes_)
                            {
                                foreach (int myPriorityAxisID in myPriorityAxesID)
                                {
                                    if (mySearchAxis.ID == myPriorityAxisID)
                                    { mySearchAxis.AbsMove(0, _SpeedPercent); }  //收Z軸
                                }
                            }
                            //避免掃描的時序尚未更新狀態
                            AxisStatus_Get();
                            TableStatus_Get();
                            do
                            { Thread.Sleep(50); }
                            while (table_Status_.SMV);
                        }

                    }

                    //照順順序動
                    for (int i = 0; i < 4; i++)   //順序最多為4
                    {
                        myPriorityAxesID = null;
                        //最多
                        myPriorityAxesID = moveSetting.PriorityAxis_JUMP((UserClass_PCI7856_4XMO.AxisPriority.MovePriority)i);

                        if (myPriorityAxesID != null)
                        {
                            //比較ID是否相同
                            foreach (Axis mySearchAxis in Axes_)
                            {
                                foreach (int myPriorityAxisID in myPriorityAxesID)
                                {
                                    if (mySearchAxis.ID == myPriorityAxisID)
                                    { mySearchAxis.AbsMove(myCoordinate[myPriorityAxisID], _SpeedPercent); }
                                }
                            }
                            //避免掃描的時序尚未更新狀態
                            AxisStatus_Get();
                            TableStatus_Get();
                            do
                            { Thread.Sleep(50); }
                            while (table_Status_.SMV);
                        }

                    }

                    JumpOK = true;

                });
                Thread_Jump.Start();
            }

            //各軸絕對移動(照順序動)
            public void Jump(double[] _Coordinate, int _SpeedPercent)
            {
                //避免連按兩次Jump(不跳警告，因為會鎖住視窗不能按急停)
                if (GoOK == false || JumpOK == false || GoHomeOK == false)
                { return; }

                JumpOK = false;
                Thread_Jump = new Thread(delegate()
                {
                    //避免鎖住主執行緒
                    List<int> myPriorityAxesID = new List<int>();

                    //先照逆順序收(但不包含第一順位)  
                    for (int i = 4; i > 0; i--)   //順序最多為4
                    {
                        myPriorityAxesID = null;
                        //最多
                        myPriorityAxesID = moveSetting.PriorityAxis_JUMP((UserClass_PCI7856_4XMO.AxisPriority.MovePriority)i);

                        if (myPriorityAxesID != null)
                        {
                            //比較ID是否相同
                            foreach (Axis mySearchAxis in Axes_)
                            {
                                foreach (int myPriorityAxisID in myPriorityAxesID)
                                {
                                    if (mySearchAxis.ID == myPriorityAxisID)
                                    { mySearchAxis.AbsMove(0, _SpeedPercent); }  //收Z軸
                                }
                            }
                            //避免掃描的時序尚未更新狀態
                            AxisStatus_Get();
                            TableStatus_Get();
                            do
                            { Thread.Sleep(50); }
                            while (table_Status_.SMV);
                        }

                    }

                    //照順順序動
                    for (int i = 0; i < 4; i++)   //順序最多為4
                    {
                        myPriorityAxesID = null;
                        //最多
                        myPriorityAxesID = moveSetting.PriorityAxis_JUMP((UserClass_PCI7856_4XMO.AxisPriority.MovePriority)i);

                        if (myPriorityAxesID != null)
                        {
                            //比較ID是否相同
                            foreach (Axis mySearchAxis in Axes_)
                            {
                                foreach (int myPriorityAxisID in myPriorityAxesID)
                                {
                                    if (mySearchAxis.ID == myPriorityAxisID)
                                    { mySearchAxis.AbsMove(_Coordinate[myPriorityAxisID], _SpeedPercent); }
                                }
                            }
                            //避免掃描的時序尚未更新狀態
                            AxisStatus_Get();
                            TableStatus_Get();
                            do
                            { Thread.Sleep(50); }
                            while (table_Status_.SMV);
                        }

                    }

                    JumpOK = true;

                });
                Thread_Jump.Start();
            }
            #endregion
            

            //平台的軸全部急停
            public void Emg_Stop()
            {
                GoOK = true;
                JumpOK = true;
                GoHomeOK = true;

                //若還在進行Go動作中
                if (Thread_Go != null)
                {
                    if (Thread_Go.IsAlive)
                    { Thread_Go.Abort(); }
                }

                //若還在進行Jump動作中
                if (Thread_Jump != null)
                {
                    if (Thread_Jump.IsAlive)
                    { Thread_Jump.Abort(); }
                }

                //若還在進行Home動作中
                if (Thread_GoHome!=null)
                {
                    if (Thread_GoHome.IsAlive)
                    { Thread_GoHome.Abort(); }
                }
                //急停命令
                foreach(Axis myAxis in Axes_)
                {
                    myAxis.Emg_Stop();
                }
            }

            //平台的軸全部歸原點(照Home 設定順序)
            public void GoHome()
            {
                GoHomeOK = false;
                //避免鎖住主執行緒
                Thread_GoHome = new Thread(delegate()
                {
                    List<int> myPriorityAxesID = new List<int>();

                    //快速不精準歸原點，主要避免撞機
                    foreach (Axis myAxis in Axes_)
                    {
                        APS168.APS_set_axis_param(myAxis.ID, (Int32)APS_Define.PRA_HOME_VM, 50000);	  //Set PRA_HOME_VM     (Set homing maximum velocity. )
                        APS168.APS_set_axis_param(myAxis.ID, (Int32)APS_Define.PRA_HOME_MODE, 0);		  //Set PRA_HOME_MODE
                    }

                    for (int i = 0; i < 4; i++)   //順序最多為4
                    {
                        myPriorityAxesID = null;
                        //最多
                        myPriorityAxesID = moveSetting.PriorityAxis_GoHome((UserClass_PCI7856_4XMO.AxisPriority.MovePriority)i);

                        if (myPriorityAxesID != null)
                        {
                            //比較ID是否相同
                            foreach (Axis mySearchAxis in Axes_)
                            {
                                foreach (int myPriorityAxisID in myPriorityAxesID)
                                {
                                    if (mySearchAxis.ID == myPriorityAxisID)
                                    { mySearchAxis.GoHome(); }
                                }
                            }
                            //避免掃描的時序尚未更新狀態
                            AxisStatus_Get();
                            TableStatus_Get();
                            do
                            { }
                            while (table_Status_.HMV);
                        }

                    }

                    //精準歸原點，速度較慢
                    foreach (Axis myAxis in Axes_)
                    {
                        APS168.APS_set_axis_param(myAxis.ID, (Int32)APS_Define.PRA_HOME_VM, 5000);	  //Set PRA_HOME_VM     (Set homing maximum velocity. )
                        APS168.APS_set_axis_param(myAxis.ID, (Int32)APS_Define.PRA_HOME_MODE, 1);		  //Set PRA_HOME_MODE
                    }

                    for (int i = 0; i < Axes_.Length; i++)
                    {
                        Axes_[i].GoHome();
                    }

                    //避免掃描的時序尚未更新狀態
                    AxisStatus_Get();
                    TableStatus_Get();
                    do
                    { }
                    while (table_Status_.HMV);

                    GoHomeOK = true;
                });
                Thread_GoHome.Start();
            }


            //平台的軸全部Servo On
            public void ServoOn()
            {
                foreach (Axis myAxis in Axes_)
                {
                    myAxis.ServoOn();
                }
            }

            //平台的軸全部Servo Off
            public void ServoOff()
            {
                foreach (Axis myAxis in Axes_)
                {
                    myAxis.ServoOff();
                }
            }

            //平台的軸全部Reset
            public void Reset()
            {
                foreach (Axis myAxis in Axes_)
                {
                    myAxis.Reset();
                }
            }

            //取軸卡的狀態
            public void AxisStatus_Get()
            {
                foreach (UserClass_PCI7856_4XMO.Axis myAxis in Axes_)
                {
                    Int32 CommandPulse = 0;
                    Int32 PositionPulse = 0;
                    Int32 MotionSts = 0;
                    Int32 IoSts = 0;

                    //取值(FeedBack Data)
                    APS168.APS_get_command(myAxis.ID, ref CommandPulse);
                    APS168.APS_get_position(myAxis.ID, ref PositionPulse);

                    IoSts = APS168.APS_motion_io_status(myAxis.ID);
                    MotionSts = APS168.APS_motion_status(myAxis.ID);

                    //放值
                    myAxis.FeedBackCommandPulse = CommandPulse;
                    myAxis.FeedBackPulse = PositionPulse;
                    myAxis.FeedBackMotionStatus = MotionSts;
                    myAxis.FeedBackIO_Status = IoSts;
                }
            }
            //更新Table狀態
            public void TableStatus_Get()
            {
                
                //table_Status_.SMV = false;

                //if (myAxis.axis_Status.Mts_SMV == true)
                //{ table_Status_.SMV = true; }

                //確定完再改狀態，否則用以上寫法會因為時序導致Jump出錯
                bool mySMV = false;
                bool myHMV = false;
                bool myALM = false;

                foreach (Axis myAxis in Axes_)
                {
                    //判斷是否有軸在移動
                    if (myAxis.axis_Status.Mts_SMV == true)
                    { mySMV = true; }
                    //判斷是否有軸在Homing
                    if (myAxis.axis_Status.Mts_HMV == true)
                    { myHMV = true; }
                    //判斷是否有軸Alarm
                    if (myAxis.axis_Status.Mio_ALM == true)
                    { myALM = true; }
                }


                if (mySMV == true)
                { table_Status_.SMV = true; }
                else
                { table_Status_.SMV = false; }
                if (myHMV == true)
                { table_Status_.HMV = true; }
                else
                { table_Status_.HMV = false; }
                if (myALM == true)
                { table_Status_.ALM = true; }
                else
                { table_Status_.ALM = false; }

            }
            #endregion
        }

        //==================軸==================
        public class Axis
        {
            //Initial
            private string Name_;   //平台軸的名稱
            private int ID_;    //ID 從0開始編，不可重複
            
            //FeedBack
            private double FeedBackCommandPos_;
            private double FeedBackPos_;
            private int FeedBackCommandPulse_;
            private int FeedBackPulse_;

            private int FeedBackMotionStatus_;
            private int FeedBackIO_Status_;
            //Home
            private int HomeMode_;
            private int HomeSpeed_;
            //Status
            private Axis_Status axis_Status_;

            double mmPerPulse;     //每個脈波的實際距離(計算方式: 螺桿Pitch/一圈的脈波數量)
            double MaxSpeedPulse;           //預設100%速度，每秒  (目前設定為1萬pulse1=1轉)

            //初始化
            public Axis(string _Name, int _ID, double _mmPerPulse, int _MaxSpeedPulse)
            {
                Name_ = _Name;
                ID_ = _ID;
                mmPerPulse = _mmPerPulse;
                MaxSpeedPulse = _MaxSpeedPulse;
            }

            #region 軸控相關方法
            //單軸絕對位置移動(mm, speed percent)
            public void AbsMove(double _Pos, int _SpeedPercent)
            {
                //將mm轉換成實際脈波數
                int myPosPulse = (int)(_Pos / mmPerPulse);
                //將速度百分比轉換成實際速度
                int mySpeedPulse = (int)(MaxSpeedPulse *  _SpeedPercent/ 100 );

                if (APS168.APS_absolute_move(ID_, myPosPulse, mySpeedPulse) != 0)
                {
                    MessageBox.Show("絕對位置移動錯誤");
                }
            }

            //單軸相對位置移動(mm)
            public void RelMove(double _Distance, int _SpeedPercent)
            {
                //將mm轉換成實際脈波數
                int myDistPulse = (int)(_Distance / mmPerPulse);
                //將速度百分比轉換成實際速度
                int mySpeedPulse = (int)(MaxSpeedPulse * _SpeedPercent / 100);

                if (APS168.APS_relative_move(ID_, myDistPulse, mySpeedPulse) != 0)
                {
                    MessageBox.Show("相對位置移動錯誤");
                }
            }

            //重置command與position
            public void Reset()
            {
                APS168.APS_set_command(ID_, 0);
                APS168.APS_set_position(ID_, 0);
            }

            //急停
            public void Emg_Stop()
            {
                if (APS168.APS_emg_stop(ID_) != 0)
                {
                    MessageBox.Show("急停錯誤");
                }
            }

            //Servo On
            public void ServoOn()
            {
                if (APS168.APS_set_servo_on(ID_, 1) != 0)    //1為On
                {
                    MessageBox.Show("Servo On錯誤");
                }
            }

            //ServoOff
            public void ServoOff()
            {
                if (APS168.APS_set_servo_on(ID_, 0) != 0)    //0為Off
                {
                    MessageBox.Show("Servo Off錯誤");
                }
            }

            //歸原點
            public void GoHome()
            {
                // APS168.APS_set_axis_param(_AxisID, (Int32)APS_Define.PRA_HOME_MODE, 0);		  //Set PRA_HOME_DIR   0:Positive 1:Negative

                if (APS168.APS_home_move(ID_) != 0)
                {
                    MessageBox.Show("歸Home錯誤");
                }
            }

            #endregion

            #region 屬性
            //Name
            public string Name
            {
                get { return Name_; }
            }
            //ID
            public int ID
            {
                get { return ID_; }
            }

            //FeedBack===============================================
            //回饋的命令脈波位置  CommandPulse
            public int FeedBackCommandPulse
            {
                get { return FeedBackCommandPulse_; }
                set { FeedBackCommandPos_ = FeedBackCommandPulse_* mmPerPulse; FeedBackCommandPulse_ = value; }
            }
            //脈波位置  Pulse
            public int FeedBackPulse
            {
                get { return FeedBackPulse_; }
                set { FeedBackPos_ = FeedBackPulse_ * mmPerPulse; FeedBackPulse_ = value; }
            }

            //回饋的命令實際位置  CommandPos
            public double FeedBackCommandPos
            {
                get { return FeedBackCommandPos_; }
            }
            //實際位置  Pos
            public double FeedBackPos
            {
                get { return FeedBackPos_; }
            }

            //移動狀態 Motion Status
            public int FeedBackMotionStatus
            {
                get { return FeedBackMotionStatus_; }
                set { FeedBackMotionStatus_ = value; AxisMoStatus_Convert(); }  //將MotionStatus轉換成布林陣列，並判斷移動狀態
            }
            //IO狀態 I/O Status
            public int FeedBackIO_Status
            {
                get { return FeedBackIO_Status_; }
                set { FeedBackIO_Status_ = value; AxisIoStatus_Convert(); }  //將IO Status轉換成布林陣列，並判斷IO狀態
            }

            //Home===============================================
            //Home Mode
            public int HomeMode
            {
                get { return HomeMode_; }
                set { HomeMode_ = value; }
            }
            //Home Speed
            public int HomeSpeed
            {
                get { return HomeSpeed_; }
                set { HomeSpeed_ = value; }
            }

            //回傳馬達狀態的布林陣列
            public Axis_Status axis_Status 
            {
                get
                {
                    return axis_Status_;
                }
            }
            #endregion

            #region MotionStatus與IoStatus功能
            //將MotionStatus從整數轉換成bit，判斷是否"移動中"
            private void AxisMoStatus_Convert()
            {
                //==========================MotionStatus=========================
                //將整數轉換成位元組陣列
                byte[] myBytesArray_MST = System.BitConverter.GetBytes(FeedBackMotionStatus_);
                //將位元組陣列轉換成
                System.Collections.BitArray myTemp_MST = new System.Collections.BitArray(myBytesArray_MST);

                bool[] myBitArray_MST = new bool[32];
                for (int i = 0; i < myTemp_MST.Length; i++)
                {
                    myBitArray_MST[i] = myTemp_MST[i];
                }

                //HMV  歸原點是否移動中
                if (myBitArray_MST[6] == true)
                { axis_Status_.Mts_HMV = true; }
                else
                { axis_Status_.Mts_HMV = false; }
                //SMV  單軸是否移動中
                if (myBitArray_MST[7] == true)
                { axis_Status_.Mts_SMV = true; }
                else
                { axis_Status_.Mts_SMV = false; }

            }

            //將IoStatus從整數轉換成bit，判斷各IO點位
            private void AxisIoStatus_Convert()
            {
                //==========================Motion IO Status=========================
                //將整數轉換成位元組陣列
                byte[] myBytesArray_MIO = System.BitConverter.GetBytes(FeedBackIO_Status_);
                //將位元組陣列轉換成位元陣列
                System.Collections.BitArray myTemp_MIO = new System.Collections.BitArray(myBytesArray_MIO);

                bool[] myBitArray_MIO = new bool[32];
                for (int i = 0; i < myTemp_MIO.Length; i++)
                {
                    myBitArray_MIO[i] = myTemp_MIO[i];
                }

                //ALM  異常是否發生
                if (myBitArray_MIO[0] == true)
                { axis_Status_.Mio_ALM = true; }
                else
                { axis_Status_.Mio_ALM = false; }
                //PEL  正極限是否觸發
                if (myBitArray_MIO[1] == true)
                { axis_Status_.Mio_PEL = true; }
                else
                { axis_Status_.Mio_PEL = false; }
                //MEL  負極限是否觸發
                if (myBitArray_MIO[2] == true)
                { axis_Status_.Mio_MEL = true; }
                else
                { axis_Status_.Mio_MEL = false; }
                //ORG  原點是否觸發
                if (myBitArray_MIO[3] == true)
                { axis_Status_.Mio_ORG = true; }
                else
                { axis_Status_.Mio_ORG = false; }
                //EMG  急停是否觸發
                if (myBitArray_MIO[4] == true)
                { axis_Status_.Mio_EMG = true; }
                else
                { axis_Status_.Mio_EMG = false; }
                //SVON  是否ServoOn
                if (myBitArray_MIO[7] == true)
                { axis_Status_.Mio_SVON = true; }
                else
                { axis_Status_.Mio_SVON = false; }


            }
            #endregion
        }
        //==================校點==================(內部容納1000個點位座標)
        [Serializable]
        public class TeachPoints
        {
            CoordinatePoint[] Points_ ;
            int PointsNumber_ = 1000;

            public TeachPoints()
            {
                //PointsNumber_;
                Points_ = new CoordinatePoint[PointsNumber_];
            }

            public int PointsNumber
            {
                get { return PointsNumber_; }
            }
            //點位設定(加入新點位到舊點位檔中，需進行命名判斷)
            public bool PointSet(int _Index, CoordinatePoint _NewPoint)
            {
                //判斷相同的Index下，名字是否相同
                if (Points_[_Index].Name == _NewPoint.Name)
                {
                    Points_[_Index] = _NewPoint;
                    return true;
                }

                //判斷不同Index名字是否重複
                if (Name_Check(_NewPoint))
                {
                    Points_[_Index] = _NewPoint;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //點位重新設定(重組點位檔)
            public void PointReset(int _Index, CoordinatePoint _NewPoint)
            {
                Points_[_Index] = _NewPoint;
            }
            //點位讀取
            public CoordinatePoint PointGet(int _Index)
            {
                if (_Index >= 0 && _Index < PointsNumber_)
                {
                    return Points_[_Index];
                }
                else
                {
                    MessageBox.Show("無此點位");
                    return null;
                }
            }
            //點位讀取
            public CoordinatePoint PointGet(string _Name)
            {
                for (int i = 0; i < 1000; i++)
                {
                    if (Points_[i] != null)
                    {
                        if (Points_[i].Name == _Name)
                        {
                            return Points_[i];
                        }
                    }
                }
                MessageBox.Show("無此點位");
                return null;
            }

            //確定點位名稱是否重複
            private bool Name_Check(CoordinatePoint _NewPoint)
            {
                foreach (CoordinatePoint myOldPoint in Points_)
                {
                    if (myOldPoint != null && _NewPoint.Name == myOldPoint.Name)
                    {
                        return false;
                    }
                }
                return true;
            }

            
        }

        //==================點位座標==================(根據軸數產生對應數量的點位座標)
        [Serializable]
        public class CoordinatePoint
        {
            string Name_;  //點位名稱 
            double[] Coordinate_;  //點位座標

            //初始化
            public CoordinatePoint(string _Name, double[] _Coordinate)
            {

                Name_ = _Name;
                Coordinate_ = _Coordinate;
            }


            public string Name
            {
                get { return Name_; }
            }

            public double[] Coordinate
            {
                get { return Coordinate_; }
            }
        }

        public struct Table_Status
        {
            public bool HMV;
            public bool SMV;
            public bool ALM;
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

        [Serializable]
        public class MoveSetting
        {
            AxisPriority[] AxesPriority_GoHome_;
            AxisPriority[] AxesPriority_JUMP_;

            //當無檔案需建立檔案時
            public MoveSetting(Axis[] _Axes)
            {
                AxesPriority_GoHome_ = new AxisPriority[_Axes.Length];  //定義Homing的各軸優先權
                AxesPriority_JUMP_ = new AxisPriority[_Axes.Length];    //定義JUMP的各軸優先權

                for (int i = 0; i < _Axes.Length; i++)
                {
                    AxesPriority_GoHome_[i] = new AxisPriority(_Axes[i].ID,_Axes[i].Name, AxisPriority.MovePriority.First);

                    AxesPriority_JUMP_[i] = new AxisPriority(_Axes[i].ID, _Axes[i].Name, AxisPriority.MovePriority.First);
                   
                }
            }

            //Home的各軸優先權
            public AxisPriority[] AxesPriority_GoHome
            {
                get { return AxesPriority_GoHome_; }
                set { AxesPriority_GoHome_ = value; }
            }

            //屬於何種優先權的軸(Home)
            public List<int> PriorityAxis_GoHome(AxisPriority.MovePriority _ObjectPriority)
            {
                //將符合優先權的軸名稱放入
                List<int> myAxisName = new List<int>();
                for (int i = 0; i < AxesPriority_GoHome_.Length; i++)
                {
                    if ((AxesPriority_GoHome_[i].Priority) == _ObjectPriority)
                    {
                        myAxisName.Add(AxesPriority_GoHome_[i].ID);
                    }
                }
                return myAxisName;
            }

            //Jump的各軸優先權資訊
            public AxisPriority[] AxesPriority_JUMP
            {
                get { return AxesPriority_JUMP_; }
                set { AxesPriority_JUMP_ = value; }
            }

            //屬於何種優先權的軸(Jump)
            public List<int> PriorityAxis_JUMP(AxisPriority.MovePriority _ObjectPriority)
            {
                //將符合優先權的軸名稱放入
                List<int> myAxisName = new List<int>();
                for (int i=0;i<AxesPriority_JUMP_.Length;i++)
                {
                    if ((AxesPriority_JUMP_[i].Priority) == _ObjectPriority)
                    {
                        myAxisName.Add(AxesPriority_JUMP_[i].ID);
                    }
                }
                return myAxisName;
            }

            //回Home的軸動作(如:先快Home 再慢Home)
           

        }

        [Serializable]
        public class AxisPriority
        {
            public enum MovePriority
            {
                First,  //第一順位
                Second, //第二順位
                Third,  //第三順位
                Fourth  //第四順位 
            }

            string Name_;  //軸名稱 
            int ID_;  //軸ID 
            MovePriority Priority_;  //優先權

            public AxisPriority(int _ID, string _Name, MovePriority _Priority)
            {
                ID_ = _ID;
                Name_ = _Name;
                Priority_ = _Priority;
            }

            public int ID
            {
                get { return ID_; }
            }

            public string Name
            {
                get { return Name_; }
            }

            public MovePriority Priority
            {
                get { return Priority_; }
                set { Priority_ = value; }
            }
        }
    }
}
