using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices;

namespace APS168_W32
{
	
	//ADLINK Struct++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[StructLayout(LayoutKind.Sequential)]
	public struct STR_SAMP_DATA_4CH 
	{			
		public Int32 tick;
		public Int32 data0; //Total channel = 4
		public Int32 data1;
		public Int32 data2;
		public Int32 data3;

	} 


	[StructLayout(LayoutKind.Sequential)]
	public struct MOVE_PARA
			{
				Int16 i16_accType;	//Axis parameter
				Int16 i16_decType;	//Axis parameter
				Int32 i32_acc;		//Axis parameter
				Int32 i32_dec;		//Axis parameter
				Int32 i32_initSpeed;	//Axis parameter
				Int32 i32_maxSpeed;	//Axis parameter
				Int32 i32_endSpeed; 	//Axis parameter
			} 

	[StructLayout(LayoutKind.Sequential)]
	public struct POINT_DATA
			{
				public Int32 i32_pos;		// Position data (relative or absolute) (pulse)
				public Int16 i16_accType;	// Acceleration pattern 0: T-curve,  1: S-curve
				public Int16 i16_decType;	// Deceleration pattern 0: T-curve,  1: S-curve
				public Int32 i32_acc;		// Acceleration rate ( pulse / ss )
				public Int32 i32_dec;		// Deceleration rate ( pulse / ss )
				public Int32 i32_initSpeed;	// Start velocity	( pulse / s )
				public Int32 i32_maxSpeed;	// Maximum velocity  ( pulse / s )
				public Int32 i32_endSpeed; 	// End velocity		( pulse / s )
				public Int32 i32_angle;		// Arc move angle    ( degree, -360 ~ 360 )
				public Int32 u32_dwell;		// Dwell times       ( unit: ms )
				public Int32 i32_opt;    	// Option //0xABCD , D:0 absolute, 1:relative
			}
 
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT_DATA2
    {
        public Int32 i32_pos_0;	// Position data (relative or absolute) (pulse) , ArraySize =16
        public Int32 i32_pos_1;	// Position data (relative or absolute) (pulse) , ArraySize =16
        public Int32 i32_pos_2;	// Position data (relative or absolute) (pulse) , ArraySize =16
        public Int32 i32_pos_3;	// Position data (relative or absolute) (pulse) , ArraySize =16
        public Int32 i32_pos_4;	// Position data (relative or absolute) (pulse) , ArraySize =16
        public Int32 i32_pos_5;	// Position data (relative or absolute) (pulse) , ArraySize =16
        public Int32 i32_pos_6;	// Position data (relative or absolute) (pulse) , ArraySize =16
        public Int32 i32_pos_7;	// Position data (relative or absolute) (pulse) , ArraySize =16
        public Int32 i32_pos_8;	// Position data (relative or absolute) (pulse) , ArraySize =16
        public Int32 i32_pos_9;	// Position data (relative or absolute) (pulse) , ArraySize =16
        public Int32 i32_pos_10;	// Position data (relative or absolute) (pulse) , ArraySize =16
        public Int32 i32_pos_11;	// Position data (relative or absolute) (pulse) , ArraySize =16
        public Int32 i32_pos_12;	// Position data (relative or absolute) (pulse) , ArraySize =16
        public Int32 i32_pos_13;	// Position data (relative or absolute) (pulse) , ArraySize =16
        public Int32 i32_pos_14;	// Position data (relative or absolute) (pulse) , ArraySize =16
        public Int32 i32_pos_15;	// Position data (relative or absolute) (pulse) , ArraySize =16

        public Int32 i32_initSpeed;	// Start velocity	( pulse / s ) 
        public Int32 i32_maxSpeed;	// Maximum velocity  ( pulse / s ) 
        public Int32 i32_angle;		// Arc move angle    ( degree, -360 ~ 360 ) 
        public Int32 u32_dwell;		// Dwell times       ( unit: ms ) 
        public Int32 i32_opt;    	// Option //0xABCD , D:0 absolute, 1:relative
    }


	[StructLayout(LayoutKind.Sequential)]
	public struct JOG_DATA
			{
				Int16 i16_jogMode;	// Jog mode. 0:Free running mode, 1:Step mode
				Int16 i16_dir;		// Jog direction. 0:positive, 1:negative direction
				Int16 i16_accType;	// Acceleration pattern 0: T-curve,  1: S-curve
				Int32 i32_acc;		// Acceleration rate ( pulse / ss )
				Int32 i32_dec;		// Deceleration rate ( pulse / ss )
				Int32 i32_maxSpeed;	// Positive value, maximum velocity  ( pulse / s )
				Int32 i32_offset;		// Positive value, a step (pulse)
				Int32 i32_delayTime;  // Delay time, ( range: 0 ~ 65535 millisecond, align by cycle time)
			} 

	[StructLayout(LayoutKind.Sequential)]
	public struct HOME_PARA
			{
				ushort u8_homeMode;
				ushort u8_homeDir;
				ushort u8_curveType;
				Int32 i32_orgOffset;
				Int32 i32_acceleration;
				Int32 i32_startVelocity;
				Int32 i32_maxVelocity;
				Int32 i32_OrgVelocity;
			} 
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++			
	public class APS168
	{
		// System & Initialization
		[DllImport("APS168x64.dll")]public static extern Int32  APS_initial( ref System.Int32  BoardID_InBits, System.Int32 Mode );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_close();
		[DllImport("APS168x64.dll")]public static extern Int32  APS_version();
		[DllImport("APS168x64.dll")]public static extern Int32  APS_device_driver_version( System.Int32 Board_ID );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_axis_info( System.Int32 Axis_ID, ref System.Int32 Board_ID, ref System.Int32  Axis_No, ref System.Int32 Port_ID, ref System.Int32  Module_ID );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_set_board_param( System.Int32 Board_ID, System.Int32 BOD_Param_No, System.Int32 BOD_Param );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_board_param( System.Int32 Board_ID, System.Int32 BOD_Param_No, ref System.Int32  BOD_Param );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_set_axis_param( System.Int32 Axis_ID, System.Int32 AXS_Param_No, System.Int32  AXS_Param );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_axis_param( System.Int32 Axis_ID, System.Int32 AXS_Param_No, ref System.Int32  AXS_Param );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_watch_timer( System.Int32 Board_ID, ref System.Int32  Timer );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_device_info( System.Int32 Board_ID, System.Int32 Info_No, ref System.Int32  Info );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_card_name( System.Int32 Board_ID, ref System.Int32 CardName );
        [DllImport("APS168x64.dll")]public static extern Int32  APS_disable_device( System.Int32 DeviceName );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_reset_wdt( System.Int32 Board_ID, System.Int32 WDT_No);
	
		// Flash function [Only for PCI-8253/56, PCI-8392(H)]
		[DllImport("APS168x64.dll")]public static extern Int32  APS_save_parameter_to_flash( System.Int32 Board_ID );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_load_parameter_from_flash( System.Int32 Board_ID );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_load_parameter_from_default( System.Int32 Board_ID );
		
		// SSCNET-3 functions [Only for PCI-8392(H)] 
		[DllImport("APS168x64.dll")]public static extern Int32  APS_start_sscnet( System.Int32 Board_ID, ref System.Int32  AxisFound_InBits );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_stop_sscnet( System.Int32 Board_ID );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_sscnet_servo_param( System.Int32 Axis_ID, System.Int32 Para_No1, ref System.Int32  Para_Dat1, System.Int32 Para_No2, ref System.Int32  Para_Dat2 );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_set_sscnet_servo_param( System.Int32 Axis_ID, System.Int32 Para_No1, System.Int32 Para_Dat1, System.Int32 Para_No2, System.Int32 Para_Dat2 );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_sscnet_servo_alarm( System.Int32 Axis_ID, ref System.Int32  Alarm_No, ref System.Int32  Alarm_Detail );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_reset_sscnet_servo_alarm( System.Int32 Axis_ID );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_save_sscnet_servo_param( System.Int32 Board_ID );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_sscnet_servo_abs_position( System.Int32 Axis_ID, ref System.Int32  Cyc_Cnt, ref System.Int32  Res_Cnt );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_save_sscnet_servo_abs_position( System.Int32 Board_ID );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_load_sscnet_servo_abs_position( System.Int32 Axis_ID, System.Int32 Abs_Option, ref System.Int32  Cyc_Cnt, ref System.Int32  Res_Cnt );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_sscnet_link_status( System.Int32 Board_ID, ref System.Int32  Link_Status );
	  [DllImport("APS168x64.dll")]public static extern Int32  APS_set_sscnet_servo_monitor_src( System.Int32 Axis_ID, System.Int32 Mon_No, System.Int32 Mon_Src );
    [DllImport("APS168x64.dll")]public static extern Int32  APS_get_sscnet_servo_monitor_src( System.Int32 Axis_ID, System.Int32 Mon_No, ref System.Int32 Mon_Src );
    [DllImport("APS168x64.dll")]public static extern Int32  APS_get_sscnet_servo_monitor_data( System.Int32 Axis_ID, System.Int32 Arr_Size, ref System.Int32 Data_Arr );

		// Motion IO & motion status
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_command( System.Int32 Axis_ID, ref System.Int32  Command );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_set_command(System.Int32 Axis_ID, System.Int32 Command);
		[DllImport("APS168x64.dll")]public static extern Int32  APS_motion_status( System.Int32 Axis_ID );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_motion_io_status( System.Int32 Axis_ID );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_set_servo_on( System.Int32 Axis_ID, System.Int32 Servo_On );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_position( System.Int32 Axis_ID, ref System.Int32  Position );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_set_position(System.Int32 Axis_ID, System.Int32 Position);
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_command_velocity(System.Int32 Axis_ID, ref System.Int32  Velocity );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_feedback_velocity(System.Int32 Axis_ID, ref System.Int32  Velocity );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_error_position( System.Int32 Axis_ID, ref System.Int32  Err_Pos );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_target_position( System.Int32 Axis_ID, ref System.Int32 Targ_Pos );
		
		// Single axis motion
		[DllImport("APS168x64.dll")]public static extern Int32  APS_relative_move( System.Int32 Axis_ID, System.Int32 Distance, System.Int32 Max_Speed );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_absolute_move( System.Int32 Axis_ID, System.Int32 Position, System.Int32 Max_Speed );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_velocity_move( System.Int32 Axis_ID, System.Int32 Max_Speed );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_home_move( System.Int32 Axis_ID );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_stop_move( System.Int32 Axis_ID );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_emg_stop( System.Int32 Axis_ID );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_relative_move2( System.Int32 Axis_ID, System.Int32 Distance, System.Int32 Start_Speed, System.Int32 Max_Speed, System.Int32 End_Speed, System.Int32 Acc_Rate, System.Int32 Dec_Rate );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_absolute_move2( System.Int32 Axis_ID, System.Int32 Position, System.Int32 Start_Speed, System.Int32 Max_Speed, System.Int32 End_Speed, System.Int32 Acc_Rate, System.Int32 Dec_Rate );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_home_move2( System.Int32 Axis_ID, System.Int32 Dir, System.Int32 Acc, System.Int32 Start_Speed, System.Int32 Max_Speed, System.Int32 ORG_Speed );
		
		//JOG functions [Only for PCI-8392, PCI-8253/56]
		[DllImport("APS168x64.dll")]public static extern Int32  APS_set_jog_param( System.Int32 Axis_ID, ref JOG_DATA pStr_Jog, System.Int32 Mask );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_jog_param( System.Int32 Axis_ID,ref JOG_DATA pStr_Jog );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_jog_mode_switch( System.Int32 Axis_ID, System.Int32 Turn_No );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_jog_start( System.Int32 Axis_ID, System.Int32 STA_On );
		
		// Interpolation
		[DllImport("APS168x64.dll")]public static extern Int32  APS_absolute_linear_move( System.Int32 Dimension, ref System.Int32 Axis_ID_Array, ref System.Int32 Position_Array, System.Int32 Max_Linear_Speed );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_relative_linear_move( System.Int32 Dimension, ref System.Int32 Axis_ID_Array, ref System.Int32 Distance_Array, System.Int32 Max_Linear_Speed );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_absolute_arc_move( System.Int32 Dimension, ref System.Int32 Axis_ID_Array, ref System.Int32 Center_Pos_Array, System.Int32 Max_Arc_Speed, System.Int32 Angle );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_relative_arc_move( System.Int32 Dimension, ref System.Int32 Axis_ID_Array, ref System.Int32 Center_Offset_Array, System.Int32 Max_Arc_Speed, System.Int32 Angle );
		
		// Interrupt functions
		[DllImport("APS168x64.dll")]public static extern Int32  APS_int_enable( System.Int32 Board_ID, System.Int32 Enable );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_set_int_factor( System.Int32 Board_ID, System.Int32 Item_No, System.Int32 Factor_No, System.Int32 Enable );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_int_factor( System.Int32 Board_ID, System.Int32 Item_No, System.Int32 Factor_No, ref System.Int32 Enable );
			
		[DllImport("APS168x64.dll")]public static extern Int32  APS_wait_single_int( System.Int32 Int_No, System.Int32 Time_Out );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_wait_multiple_int( System.Int32 Int_Count, ref System.Int32 Int_No_Array, System.Int32 Wait_All, System.Int32 Time_Out );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_reset_int( System.Int32 Int_No );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_set_int( System.Int32 Int_No );
		//[Only for PCI-8154/58]
		[DllImport("APS168x64.dll")]public static extern Int32  APS_wait_error_int( System.Int32 Board_ID, System.Int32 Item_No, System.Int32 Time_Out );
		
		// Sampling functions [Only for PCI-8392, PCI-8253/56]
		[DllImport("APS168x64.dll")]public static extern Int32  APS_set_sampling_param( System.Int32 Board_ID, System.Int32 ParaNum, System.Int32 ParaDat );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_sampling_param( System.Int32 Board_ID, System.Int32 ParaNum, ref System.Int32 ParaDat );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_wait_trigger_sampling( System.Int32 Board_ID, System.Int32 Length, System.Int32 PreTrgLen, System.Int32 TimeOutMs, ref STR_SAMP_DATA_4CH DataArr );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_wait_trigger_sampling_async( System.Int32 Board_ID, System.Int32 Length, System.Int32 PreTrgLen, System.Int32 TimeOutMs, ref STR_SAMP_DATA_4CH DataArr );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_sampling_count( System.Int32 Board_ID, ref System.Int32 SampCnt );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_stop_wait_sampling( System.Int32 Board_ID );
		
		
		//DIO & AIO
		[DllImport("APS168x64.dll")]public static extern Int32  APS_write_d_output(System.Int32 Board_ID, System.Int32 DO_Group, System.Int32 DO_Data);
		[DllImport("APS168x64.dll")]public static extern Int32  APS_read_d_output(System.Int32 Board_ID, System.Int32 DO_Group, ref System.Int32 DO_Data);
		[DllImport("APS168x64.dll")]public static extern Int32  APS_read_d_input(System.Int32 Board_ID, System.Int32 DI_Group, ref System.Int32 DI_Data);
		
		[DllImport("APS168x64.dll")]public static extern Int32  APS_read_a_input_value(System.Int32 Board_ID, System.Int32 Channel_No, ref System.Double Convert_Data);
		[DllImport("APS168x64.dll")]public static extern Int32  APS_read_a_input_data(System.Int32 Board_ID, System.Int32 Channel_No, ref System.Int32 Raw_Data);
		[DllImport("APS168x64.dll")]public static extern Int32  APS_write_a_output_value(System.Int32 Board_ID, System.Int32 Channel_No, System.Double  Convert_Data);
		[DllImport("APS168x64.dll")]public static extern Int32  APS_write_a_output_data(System.Int32 Board_ID, System.Int32 Channel_No, System.Int32 Raw_Data);
		
		//Point table move
		[DllImport("APS168x64.dll")]public static extern Int32  APS_set_point_table( System.Int32 Axis_ID, System.Int32 Index, ref POINT_DATA Point );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_point_table( System.Int32 Axis_ID, System.Int32 Index, ref POINT_DATA Point );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_running_point_index( System.Int32 Axis_ID, ref System.Int32 Index );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_start_point_index( System.Int32 Axis_ID, ref System.Int32 Index );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_end_point_index( System.Int32 Axis_ID, ref System.Int32 Index );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_set_table_move_pause( System.Int32 Axis_ID, System.Int32 Pause_en );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_set_table_move_repeat( System.Int32 Axis_ID, System.Int32 Repeat_en );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_point_table_move( System.Int32 Dimension, ref System.Int32 Axis_ID_Array, System.Int32 StartIndex, System.Int32 EndIndex );


        //Point table move2
        [DllImport("APS168x64.dll")]
        public static extern Int32 APS_set_point_table_mode2(Int32 Axis_ID, Int32 Mode);
        [DllImport("APS168x64.dll")]
        public static extern Int32 APS_set_point_table2(Int32 Dimension, ref Int32 Axis_ID_Array, Int32 Index,ref POINT_DATA2 Point);
        [DllImport("APS168x64.dll")]
        public static extern Int32 APS_point_table_continuous_move2(Int32 Dimension,ref Int32 Axis_ID_Array);
[DllImport("APS168x64.dll")]
public static extern Int32 APS_point_table_single_move2(Int32 Axis_ID, Int32 Index);
[DllImport("APS168x64.dll")]
public static extern Int32 APS_get_running_point_index2(Int32 Axis_ID, ref Int32 Index);
[DllImport("APS168x64.dll")]
public static extern Int32 APS_point_table_status2(Int32 Axis_ID,ref  Int32 Status);
		
		// Gantry functions. [Only for PCI-8392, PCI-8253/56]
		[DllImport("APS168x64.dll")]public static extern Int32  APS_set_gantry_param( System.Int32 Board_ID, System.Int32 GroupNum, System.Int32 ParaNum, System.Int32 ParaDat );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_gantry_param( System.Int32 Board_ID, System.Int32 GroupNum, System.Int32 ParaNum, ref System.Int32 ParaDat );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_set_gantry_axis( System.Int32 Board_ID, System.Int32 GroupNum, System.Int32 Master_Axis_ID, System.Int32 Slave_Axis_ID );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_gantry_axis( System.Int32 Board_ID, System.Int32 GroupNum, ref System.Int32 Master_Axis_ID, ref System.Int32 Slave_Axis_ID );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_gantry_error( System.Int32 Board_ID, System.Int32 GroupNum, ref System.Int32 GentryError );
		
		//Field bus master fucntions For PCI-8392(H)
		[DllImport("APS168x64.dll")]public static extern Int32  APS_set_field_bus_param( System.Int32 Board_ID, System.Int32 BUS_No, System.Int32 BUS_Param_No, System.Int32  BUS_Param );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_field_bus_param( System.Int32 Board_ID, System.Int32 BUS_No, System.Int32 BUS_Param_No, ref System.Int32 BUS_Param );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_start_field_bus( System.Int32 Board_ID, System.Int32 BUS_No, System.Int32 Start_Axis_ID );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_stop_field_bus( System.Int32 Board_ID, System.Int32 BUS_No );
		
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_field_bus_last_scan_info( System.Int32 Board_ID, System.Int32 BUS_No, ref System.Int32 Info_Array, System.Int32 Array_Size, ref System.Int32 Info_Count );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_field_bus_master_type( System.Int32 Board_ID, System.Int32 BUS_No, ref System.Int32 BUS_Type );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_field_bus_slave_type( System.Int32 Board_ID, System.Int32 BUS_No, System.Int32 MOD_No, ref System.Int32 MOD_Type );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_field_bus_slave_name( System.Int32 Board_ID, System.Int32 BUS_No, System.Int32 MOD_No, ref System.Int32 MOD_Name );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_field_bus_slave_mapto_AxisID( System.Int32 Board_ID, System.Int32 BUS_No, System.Int32 MOD_No, ref System.Int32 AxisID );
	
		//Field bus slave general functions
		[DllImport("APS168x64.dll")]public static extern Int32  APS_set_field_bus_slave_param( System.Int32 Board_ID, System.Int32 BUS_No, System.Int32 MOD_No, System.Int32 Ch_No, System.Int32 ParaNum, System.Int32 ParaDat  );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_field_bus_slave_param( System.Int32 Board_ID, System.Int32 BUS_No, System.Int32 MOD_No, System.Int32 Ch_No, System.Int32 ParaNum, ref System.Int32 ParaDat );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_slave_connect_quality( System.Int32 Board_ID, System.Int32 BUS_No, System.Int32 MOD_No, ref System.Int32 Sts_data );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_slave_online_status( System.Int32 Board_ID, System.Int32 BUS_No, System.Int32 MOD_No, ref System.Int32 Live );

		//Field bus DIO slave fucntions For PCI-8392(H)
		[DllImport("APS168x64.dll")]public static extern Int32  APS_set_field_bus_d_output( System.Int32 Board_ID, System.Int32 BUS_No, System.Int32 MOD_No, System.Int32 DO_Value );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_field_bus_d_output( System.Int32 Board_ID, System.Int32 BUS_No, System.Int32 MOD_No, ref System.Int32 DO_Value );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_field_bus_d_input( System.Int32 Board_ID, System.Int32 BUS_No, System.Int32 MOD_No, ref System.Int32 DI_Value );

        [DllImport("APS168x64.dll")]
        public static extern Int32 APS_get_field_bus_device_info(Int32 Board_ID, Int32 BUS_No, Int32 MOD_No, Int32 Info_No, ref  Int32 Info);
		
		//Field bus AIO slave function
		[DllImport("APS168x64.dll")]public static extern Int32  APS_set_field_bus_a_output( System.Int32 Board_ID, System.Int32 BUS_No, System.Int32 MOD_No, System.Int32 Ch_No, System.Double   AO_Value );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_field_bus_a_output( System.Int32 Board_ID, System.Int32 BUS_No, System.Int32 MOD_No, System.Int32 Ch_No, ref System.Double AO_Value );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_field_bus_a_input( System.Int32 Board_ID, System.Int32 BUS_No, System.Int32 MOD_No, System.Int32 Ch_No, ref System.Double AI_Value );
		
		// Comparing trigger functions
		[DllImport("APS168x64.dll")]public static extern Int32  APS_set_trigger_param( System.Int32 Board_ID, System.Int32 Param_No, System.Int32 Param_Val );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_trigger_param( System.Int32 Board_ID, System.Int32 Param_No, ref System.Int32 Param_Val );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_set_trigger_linear( System.Int32 Board_ID, System.Int32 LCmpCh, System.Int32 StartPoint, System.Int32 RepeatTimes, System.Int32 Interval );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_set_trigger_table( System.Int32 Board_ID, System.Int32 TCmpCh, ref System.Int32 DataArr, System.Int32 ArraySize ); 
		[DllImport("APS168x64.dll")]public static extern Int32  APS_set_trigger_manual( System.Int32 Board_ID, System.Int32 TrgCh );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_set_trigger_manual_s( System.Int32 Board_ID, System.Int32 TrgChInBit );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_trigger_table_cmp( System.Int32 Board_ID, System.Int32 TCmpCh, ref System.Int32 CmpVal );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_trigger_linear_cmp( System.Int32 Board_ID, System.Int32 LCmpCh, ref System.Int32 CmpVal );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_trigger_count( System.Int32 Board_ID, System.Int32 TrgCh, ref System.Int32 TrgCnt );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_reset_trigger_count( System.Int32 Board_ID, System.Int32 TrgCh );

        //Field bus Comparing trigger functions
        [DllImport("APS168x64.dll")]
        public static extern Int32 APS_set_field_bus_trigger_param(Int32 Board_ID, Int32 BUS_No, Int32 MOD_No, Int32 Param_No, Int32 Param_Val);
        [DllImport("APS168x64.dll")]
        public static extern Int32 APS_get_field_bus_trigger_param(Int32 Board_ID, Int32 BUS_No, Int32 MOD_No, Int32 Param_No, ref Int32 Param_Val);
        [DllImport("APS168x64.dll")]
        public static extern Int32 APS_set_field_bus_trigger_linear(Int32 Board_ID, Int32 BUS_No, Int32 MOD_No, Int32 LCmpCh, Int32 StartPoint, Int32 RepeatTimes, Int32 Interval);
        [DllImport("APS168x64.dll")]
        public static extern Int32 APS_set_field_bus_trigger_table(Int32 Board_ID, Int32 BUS_No, Int32 MOD_No, Int32 TCmpCh,ref Int32 DataArr, Int32 ArraySize);
        [DllImport("APS168x64.dll")]
        public static extern Int32 APS_set_field_bus_trigger_manual(Int32 Board_ID, Int32 BUS_No, Int32 MOD_No, Int32 TrgCh);
        [DllImport("APS168x64.dll")]
        public static extern Int32 APS_set_field_bus_trigger_manual_s(Int32 Board_ID, Int32 BUS_No, Int32 MOD_No, Int32 TrgChInBit);
        [DllImport("APS168x64.dll")]
        public static extern Int32 APS_get_field_bus_trigger_table_cmp(Int32 Board_ID, Int32 BUS_No, Int32 MOD_No, Int32 TCmpCh,ref Int32 CmpVal);
        [DllImport("APS168x64.dll")]
        public static extern Int32 APS_get_field_bus_trigger_linear_cmp(Int32 Board_ID, Int32 BUS_No, Int32 MOD_No, Int32 LCmpCh, ref Int32 CmpVal);
        [DllImport("APS168x64.dll")]
        public static extern Int32 APS_get_field_bus_trigger_count(Int32 Board_ID, Int32 BUS_No, Int32 MOD_No, Int32 TrgCh, ref Int32 TrgCnt);
        [DllImport("APS168x64.dll")]
        public static extern Int32 APS_reset_field_bus_trigger_count(Int32 Board_ID, Int32 BUS_No, Int32 MOD_No, Int32 TrgCh);
[DllImport("APS168x64.dll")]
        public static extern Int32 APS_get_field_bus_linear_cmp_remain_count(Int32 Board_ID, Int32 BUS_No, Int32 MOD_No, Int32 LCmpCh, ref Int32 Cnt);
[DllImport("APS168x64.dll")]
public static extern Int32 APS_get_field_bus_table_cmp_remain_count(Int32 Board_ID, Int32 BUS_No, Int32 MOD_No, Int32 TCmpCh,ref Int32 Cnt);
[DllImport("APS168x64.dll")]
public static extern Int32 APS_get_field_bus_encoder(Int32 Board_ID, Int32 BUS_No, Int32 MOD_No, Int32 EncCh,ref Int32 EncCnt);
[DllImport("APS168x64.dll")]
public static extern Int32 APS_set_field_bus_encoder(Int32 Board_ID, Int32 BUS_No, Int32 MOD_No, Int32 EncCh, Int32 EncCnt);
	
		// Pulser counter function
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_pulser_counter( System.Int32 Board_ID, ref System.Int32 Counter );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_set_pulser_counter( System.Int32 Board_ID, System.Int32 Counter );
		
		// Reserved functions
		[DllImport("APS168x64.dll")]public static extern Int32  APS_field_bus_slave_set_param( System.Int32 Board_ID, System.Int32 BUS_No, System.Int32 MOD_No, System.Int32 Ch_No, System.Int32 ParaNum, System.Int32 ParaDat  );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_field_bus_slave_get_param( System.Int32 Board_ID, System.Int32 BUS_No, System.Int32 MOD_No, System.Int32 Ch_No, System.Int32 ParaNum, ref System.Int32 ParaDat );
	
		[DllImport("APS168x64.dll")]public static extern Int32  APS_field_bus_d_set_output( System.Int32 Board_ID, System.Int32 BUS_No, System.Int32 MOD_No, System.Int32 DO_Value );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_field_bus_d_get_output( System.Int32 Board_ID, System.Int32 BUS_No, System.Int32 MOD_No, ref System.Int32 DO_Value );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_field_bus_d_get_input( System.Int32 Board_ID, System.Int32 BUS_No, System.Int32 MOD_No, ref System.Int32 DI_Value );
		
		[DllImport("APS168x64.dll")]public static extern Int32  APS_field_bus_A_set_output( System.Int32 Board_ID, System.Int32 BUS_No, System.Int32 MOD_No, System.Int32 Ch_No, System.Double   AO_Value );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_field_bus_A_get_output( System.Int32 Board_ID, System.Int32 BUS_No, System.Int32 MOD_No, System.Int32 Ch_No, ref System.Double AO_Value );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_field_bus_A_get_input( System.Int32 Board_ID, System.Int32 BUS_No, System.Int32 MOD_No, System.Int32 Ch_No, ref System.Double AI_Value );
		
		//Dpac Function
		[DllImport("APS168x64.dll")]public static extern Int32  APS_rescan_CF( System.Int32 Board_ID );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_battery_status( System.Int32 Board_ID, ref System.Int32 Battery_status);
		
		//DPAC Display & Display Button
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_display_data( System.Int32 Board_ID, System.Int32 displayDigit, ref System.Int32 displayIndex);
		[DllImport("APS168x64.dll")]public static extern Int32  APS_set_display_data( System.Int32 Board_ID, System.Int32 displayDigit, System.Int32 displayIndex);
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_button_status( System.Int32 Board_ID, ref System.Int32 buttonstatus);
		
		//nv RAM funciton
		[DllImport("APS168x64.dll")]public static extern Int32  APS_set_nv_ram( System.Int32 Board_ID, System.Int32 RamNo, System.Int32 DataWidth, System.Int32 Offset, System.Int32 Data );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_get_nv_ram( System.Int32 Board_ID, System.Int32 RamNo, System.Int32 DataWidth, System.Int32 Offset, ref System.Int32 Data );
		[DllImport("APS168x64.dll")]public static extern Int32  APS_clear_nv_ram( System.Int32 Board_ID, System.Int32 RamNo );
		
	}
}