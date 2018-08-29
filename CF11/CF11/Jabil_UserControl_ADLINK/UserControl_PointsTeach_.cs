using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Jabil_UserControl_ADLINK
{
    public partial class UserControl_PointsTeach_ : UserControl
    {
        UserClass_PCI7856_4XMO.Table G_Table;
        Label[] G_AxesName;
        Label[] G_AxesValue;
        Label[] G_AxesUnit;

        #region "初始建構式"
        public UserControl_PointsTeach_()
        {
            InitializeComponent();
        }
        #endregion

        #region "功能"
        //初始化
        public void Initialize(UserClass_PCI7856_4XMO.Table _Table)
        {

            G_Table = _Table;

            //添加新行
            for (int i = 0; i < G_Table.Axes.Length; i++)
            {
                dataGridView_Points.Columns.Add("Column" + G_Table.Axes[i].Name, G_Table.Axes[i].Name);
            }

            //設置DataGridView的欄位填充整個顯示區
            dataGridView_Points.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //按下標題時不排列
            for (int i = 0; i < dataGridView_Points.ColumnCount - 1; i++)
            {
                dataGridView_Points.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            //將Index行唯獨
            dataGridView_Points.Columns["Column_Item"].ReadOnly = true;

            //座標顯示
            G_AxesName = new Label[G_Table.AxesNumber];
            G_AxesValue = new Label[G_Table.AxesNumber];
            G_AxesUnit = new Label[G_Table.AxesNumber];
            for (int i = 0; i < G_Table.AxesNumber; i++)
            {
                G_AxesName[i] = new Label();
                G_AxesName[i].Text = G_Table.Axes[i].Name + ":";
                G_AxesName[i].Size = new Size(30,30);
                G_AxesName[i].Location = new Point(3, 120 + 40 * i);
                this.Controls.Add(G_AxesName[i]);

                G_AxesValue[i] = new Label();
                G_AxesValue[i].Size = new Size(50, 30);
                G_AxesValue[i].Location = new Point(40, 120 + 40 * i);
                this.Controls.Add(G_AxesValue[i]);

                G_AxesUnit[i] = new Label();
                G_AxesUnit[i].Size = new Size(30, 30);
                G_AxesUnit[i].Text = "mm";
                G_AxesUnit[i].Location = new Point(100, 120 + 40 * i);
                this.Controls.Add(G_AxesUnit[i]);
                
            }

         
            //平台名稱
            label_TableName.Text = G_Table.Name;
            //DataGridView刷新
            GridViewRefresh();

            //建立校點
            ComboBoxRefresh();

            comboBox_Points.SelectedIndex = 0;
            comboBox_Points.DropDownHeight = 200;    //設定下拉高度(像素為單位)


            //顯示座標值
            timer_CoordinateDisplay.Enabled = true;

        }

        //ComboBox刷新
        private void ComboBoxRefresh()
        {
            comboBox_Points.Items.Clear();

            for (int i = 0; i < G_Table.teachPoints.PointsNumber; i++)
            {
                //UserClass_PCI7856_4XMO.CoordinatePoint myPoint = G_Table.teachPoints.PointGet(i);
                if (dataGridView_Points[1, i].Value != null)
                //if (myPoint != null)
                {
                    //if (myPoint.Name != null || myPoint.Name.ToString() != "")
                    //{
                    comboBox_Points.Items.Add("P" + i + "- " + dataGridView_Points[1, i].Value.ToString());
                    //}
                }
                else
                {
                    comboBox_Points.Items.Add("P" + i);
                }
            }
        }


        //DataGridView刷新
        public void GridViewRefresh()
        {
            //清除所有數據
            dataGridView_Points.Rows.Clear();
            dataGridView_Points.DataSource = null;

            //增加1000列
            dataGridView_Points.Rows.Add(G_Table.teachPoints.PointsNumber);
            for (int i = 0; i < G_Table.teachPoints.PointsNumber; i++)
            {
                UserClass_PCI7856_4XMO.CoordinatePoint myPoint = G_Table.teachPoints.PointGet(i);
                dataGridView_Points[0, i].Value = "P"+i.ToString();            //編號

                if (myPoint != null)
                {
                    dataGridView_Points[1, i].Value = myPoint.Name; //平台名稱
                    //平台校點數值
                    for (int j = 0; j < G_Table.Axes.Length; j++)
                    {
                        dataGridView_Points[j + 2, i].Value = myPoint.Coordinate[j];
                    }
                }
            }

        }

        //點位確認
        private bool GridViewCheck()
        {
            bool RowNull = true;    //判斷整列是否無值(有值為false)
            bool CheckDoubleFail = false;    //判斷數值是否有誤(不屬於double)
            bool CheckNameFail = false;    //判斷數值是否有誤(不屬於double)
            string[] myNameArray = new string[G_Table.teachPoints.PointsNumber];  //Name的Array 用來判斷Name是否重複
             

            for (int i = 0; i < G_Table.teachPoints.PointsNumber; i++)
            {
                //清除焦點
                dataGridView_Points.ClearSelection();

                RowNull = true;
                //判斷整列是否無值(主要判斷Name與座標)
                for (int j = 1; j < G_Table.Axes.Length + 2; j++)
                {
                    if (dataGridView_Points[j, i].Value != null && dataGridView_Points[j, i].Value.ToString() != "")
                    {
                        RowNull = false;
                    }
                }

                //若有值則將命名規格化
                if (RowNull == false)
                {
                    //Name
                    if (dataGridView_Points[1, i].Value == null || dataGridView_Points[1, i].Value.ToString() == "")
                    {
                        dataGridView_Points[1, i].Value = "P" + i;
                    }


                    //Points Coordinate
                    for (int k = 2; k < G_Table.Axes.Length + 2; k++)
                    {
                        if (dataGridView_Points[k, i].Value == null || dataGridView_Points[k, i].Value.ToString() == "")
                        {
                            dataGridView_Points[k, i].Value = 0;
                            dataGridView_Points[k, i].Style.BackColor = Color.White;
                        }
                        else
                        {
                            double myNumber;
                            if (double.TryParse(dataGridView_Points[k, i].Value.ToString(), out myNumber))
                            {
                                dataGridView_Points[k, i].Style.BackColor = Color.White;
                            }
                            else
                            {
                                dataGridView_Points[k, i].Style.BackColor = Color.Red;
                                CheckDoubleFail = true;
                            }
                        }
                    }

                }

                //將Name放入陣列中，後面用以判斷命名是否重複
                if (dataGridView_Points[1, i].Value != null)
                {
                    myNameArray[i] = dataGridView_Points[1, i].Value.ToString();
                }
                
            }

            if (CheckDoubleFail == true)
            {
                MessageBox.Show("命名錯誤，請檢查紅色區域(點位尚未儲存)");
                return false;
            }


            //確認命名是否重複(從陣列取第i個值跟第j個值做比較，其中i不等於j)
            for (int i = 0; i < myNameArray.Length; i++)
            {
                if (myNameArray[i] != null)
                {
                    //確認第i列的Name是否與其他列的Name重複，用來判斷是否顯示紅色背景
                    bool NameRepeat = false;

                    for (int j = 0; j < myNameArray.Length; j++)
                    {
                        if (myNameArray[j] != null)
                        {
                            if (i != j)
                            {
                                //假如命名重複
                                if (myNameArray[j] == myNameArray[i])
                                {
                                    NameRepeat = true;
                                    CheckNameFail = true;
                                }
                            }
                        }
                    }
                    if (NameRepeat==false)
                    { dataGridView_Points[1, i].Style.BackColor = Color.White; }
                    else
                    { dataGridView_Points[1, i].Style.BackColor = Color.Red; }
                }
            }

            if (CheckNameFail == true)
            {
                MessageBox.Show("命名重複，請檢查紅色區域(點位尚未儲存)");
                return false;
            }
               
            return true;
      
        }

        //點位儲存
        private void PointsSave()
        {

            //儲存到TeachPoints中
            for (int i = 0; i < G_Table.teachPoints.PointsNumber; i++)
            {
                //判斷DataGridView列中的Name有無值(在前段處理已將座標有值)
                if (dataGridView_Points[1, i].Value != null)
                {
                    //將有值列的Name取出
                    string myName = dataGridView_Points[1, i].Value.ToString();
                    //將有值列的座標取出
                    double[] myCoordinate = new double[G_Table.Axes.Length];
                    for (int j = 0; j < G_Table.Axes.Length; j++)
                    {
                        myCoordinate[j] = double.Parse(dataGridView_Points[j + 2, i].Value.ToString());
                    }

                    //新建一個點位
                    G_Table.teachPoints.PointReset(i, new UserClass_PCI7856_4XMO.CoordinatePoint(myName, myCoordinate));
                }
                else
                {
                    G_Table.teachPoints.PointReset(i, null);
                }
            }

            G_Table.TeachPointsFileSave();
            MessageBox.Show("儲存完畢");
        }

        #endregion


        #region "事件函式"
        private void btn_Teach_Click(object sender, EventArgs e)
        {
            string StringName = "";

            Form InputBox = new Form() { Text = "InputBox", Size = new Size(300, 150), StartPosition = FormStartPosition.CenterParent };
            Label Topic = new Label() { Text = "請輸入點位名稱", Location = new Point(35, 20) };
            TextBox textBox_Input = new TextBox() { Size = new Size(223, 22), Location = new Point(37, 44) };
            Button btn_OK = new Button() { Text = "確定", Size = new Size(60, 30), Location = new Point(130, 75) };
            Button btn_NG = new Button() { Text = "取消", Size = new Size(60, 30), Location = new Point(200, 75) };
            btn_OK.Click += (s_OK, e_OK) =>
            {
                InputBox.DialogResult = DialogResult.OK;
                StringName = textBox_Input.Text; ;
            };
            btn_NG.Click += (s_NG, e_NG) =>
            {
                InputBox.DialogResult = DialogResult.Cancel;
            };

            //textBox_Input的匿名事件(要在建立按鈕物件後才可觸發)
            textBox_Input.KeyDown += (s_Key, e_Key) =>
            {
                if (e_Key.KeyCode == Keys.Enter)
                { btn_OK.PerformClick(); }
            };

            InputBox.Controls.Add(Topic);
            InputBox.Controls.Add(textBox_Input);
            InputBox.Controls.Add(btn_OK);
            InputBox.Controls.Add(btn_NG);

            //將原先在DataGridView的名稱放入textbox (不用重新打字)
            int myPointIndex = comboBox_Points.SelectedIndex;
            if (dataGridView_Points[1, myPointIndex].Value != null)
            { textBox_Input.Text = dataGridView_Points[1, myPointIndex].Value.ToString(); }

            //Show出輸入視窗
            InputBox.ShowDialog();

            if (InputBox.DialogResult == DialogResult.OK)
            {
                //將輸入的名稱放入DataGridView中

                if (StringName == "")
                {
                    dataGridView_Points[1, myPointIndex].Value = "P" + myPointIndex.ToString();
                }
                else
                {
                    dataGridView_Points[1, myPointIndex].Value = StringName;
                }

                for (int i = 0; i < G_Table.Axes.Length; i++)
                {
                    dataGridView_Points[i + 2, myPointIndex].Value = G_Table.Axes[i].FeedBackPos;
                }
            }

            //更新CheckBox
            ComboBoxRefresh();
            //讓 comboBox目前顯示的值 更新
            comboBox_Points.SelectedIndex = myPointIndex;
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            //彈跳確定視窗
            DialogResult myResult = MessageBox.Show("確定是否儲存點位", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (myResult == DialogResult.No)
            { return; }

            if (GridViewCheck())
            { PointsSave(); }

        }

        private void btn_Recovery_Click(object sender, EventArgs e)
        {
            //彈跳確定視窗
            DialogResult myResult = MessageBox.Show("確定是否回復成先前的點位", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (myResult == DialogResult.No)
            { return; }

            //讀取原先的點位檔案
            G_Table.TeachPointsFileLoad();
            //DataGridView刷新
            GridViewRefresh();
        }
        //重新設定原點
        private void btn_TableGoHome_Click(object sender, EventArgs e)
        {
            G_Table.GoHome();
        }
        //回到原點位置
        private void btn_JumpHome_Click(object sender, EventArgs e)
        {
            int mySpeedPercent;
            if (int.TryParse(comboBox_Motor_Speed.Text, out mySpeedPercent))
            {
                G_Table.Jump("Home", mySpeedPercent);
            }
        }
        //急停
        private void btn_EmgStop_Click(object sender, EventArgs e)
        {
            G_Table.Emg_Stop();
        }
        //跑點
        private void btn_Jump_Click(object sender, EventArgs e)
        {
            //目標點位
            double[] myJumpCoordinate = new double[G_Table.Axes.Length];

            //從dataGridView取出ComboBox指定的Index
            int myPointIndex = comboBox_Points.SelectedIndex;

            for (int i =0;i<G_Table.Axes.Length; i++)
            { myJumpCoordinate[i] = (double)dataGridView_Points[i + 2, myPointIndex].Value; }

            //設定速度
            int mySpeedPercent;
            if (int.TryParse(comboBox_Motor_Speed.Text, out mySpeedPercent))
            {
                G_Table.Jump(myJumpCoordinate, mySpeedPercent);
            }
        }
        #endregion

        #region "顯示設定"
        private void timer_CoordinateDisplay_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < G_Table.Axes.Length; i++)
            {
                G_AxesValue[i].Text = (G_Table.Axes[i].FeedBackPos).ToString();   //*mmPerPulse將脈波換算成實際距離
            }
        }
        #endregion

        

        
    }
}
