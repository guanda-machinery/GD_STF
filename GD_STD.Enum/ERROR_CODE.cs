using System;

namespace GD_STD.Enum
{
    /// <summary>
    /// Codesys 錯誤代碼
    /// </summary>
    public enum ERROR_CODE : UInt32
    {
        [ErrorCodeAttribute("初始(無Error)_0", null)]
        //初始(無Error)_0
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Null' 的 XML 註解
        Null,

        [ErrorCodeAttribute("校機參數值缺少", null)]
        //校機參數值缺少
        Machine_Tuning_No_Value,

        [ErrorCodeAttribute("中軸刀庫校機資訊缺少", null)]
        //中軸刀庫校機資訊缺少
        M_Drill_Tuning_No_Value,

        [ErrorCodeAttribute("左軸出口刀庫較校資訊缺少", null)]
        //左軸出口刀庫較校資訊缺少
        L_Out_Drill_Tuning_No_Value,

        [ErrorCodeAttribute("橫移料件無座標", null)]
        //橫移料件無座標
        Traverse_No_Position,

        [ErrorCodeAttribute("左進料刀庫1與設定不符", null)]
        //左進料刀庫1與設定不符
        L_IN_DrillHome1_Error,

        [ErrorCodeAttribute("左進料刀庫2與設定不符", null)]
        //左進料刀庫2與設定不符
        L_IN_DrillHome2_Error,

        [ErrorCodeAttribute("左進料刀庫3與設定不符", null)]
        //左進料刀庫3與設定不符
        L_IN_DrillHome3_Error,

        [ErrorCodeAttribute("左進料刀庫4與設定不符", null)]
        //左進料刀庫4與設定不符
        L_IN_DrillHome4_Error,

        [ErrorCodeAttribute("左出料刀庫1與設定不符", null)]
        //左出料刀庫1與設定不符
        L_Out_DrillHome1_Error,

        [ErrorCodeAttribute("左出料刀庫2與設定不符", null)]
        //左出料刀庫2與設定不符
        L_Out_DrillHome2_Error,

        [ErrorCodeAttribute("左出料刀庫3與設定不符", null)]
        //左出料刀庫3與設定不符
        L_Out_DrillHome3_Error,

        [ErrorCodeAttribute("左出料刀庫4與設定不符", null)]
        //左出料刀庫4與設定不符
        L_Out_DrillHome4_Error,

        [ErrorCodeAttribute("右進料刀庫1與設定不符", null)]
        //右進料刀庫1與設定不符
        R_IN_DrillHome1_Error,

        [ErrorCodeAttribute("右進料刀庫2與設定不符", null)]
        //右進料刀庫2與設定不符
        R_IN_DrillHome2_Error,

        [ErrorCodeAttribute("右進料刀庫3與設定不符", null)]
        //右進料刀庫3與設定不符
        R_IN_DrillHome3_Error,

        [ErrorCodeAttribute("右進料刀庫4與設定不符", null)]
        //右進料刀庫4與設定不符
        R_IN_DrillHome4_Error,

        [ErrorCodeAttribute("右出料刀庫1與設定不符", null)]
        //右出料刀庫1與設定不符
        R_Out_DrillHome1_Error,

        [ErrorCodeAttribute("右出料刀庫2與設定不符", null)]
        //右出料刀庫2與設定不符
        R_Out_DrillHome2_Error,

        [ErrorCodeAttribute("右出料刀庫3與設定不符", null)]
        //右出料刀庫3與設定不符
        R_Out_DrillHome3_Error,

        [ErrorCodeAttribute("右出料刀庫4與設定不符", null)]
        //右出料刀庫4與設定不符
        R_Out_DrillHome4_Error,

        [ErrorCodeAttribute("中軸刀庫1與設定不符", null)]
        //中軸刀庫1與設定不符
        M_DrillHome1_Error,

        [ErrorCodeAttribute("中軸刀庫2與設定不符", null)]
        //中軸刀庫2與設定不符
        M_DrillHome2_Error,

        [ErrorCodeAttribute("中軸刀庫3與設定不符", null)]
        //中軸刀庫3與設定不符
        M_DrillHome3_Error,

        [ErrorCodeAttribute("中軸刀庫4與設定不符", null)]
        //中軸刀庫4與設定不符
        M_DrillHome4_Error,

        [ErrorCodeAttribute("中軸刀庫5與設定不符", null)]
        //中軸刀庫5與設定不符
        M_DrillHome5_Error,

        [ErrorCodeAttribute("Ethercat未啟動", null)]
        //Ethercat未啟動
        Ethercat_NG,

        [ErrorCodeAttribute("自動模式下不在原點", null)]
        //自動模式下不在原點
        AutoNotOnOrigin,

        [ErrorCodeAttribute("左軸刀長量測異常", null)]
        //左軸刀長量測異常
        L_Drill_Length_Error,

        [ErrorCodeAttribute("中軸刀長量測異常", null)]
        //中軸刀長量測異常
        M_Drill_Length_Error,

        [ErrorCodeAttribute("右軸刀長量測異常", null)]
        //右軸刀長量測異常
        R_Drill_Length_Error,

        [ErrorCodeAttribute("加工陣列判斷有干涉", null)]
        //加工陣列判斷有干涉
        Work_Arr_Error,

        [ErrorCodeAttribute("貫穿作業的孔徑與刀庫匹配不到", null)]
        //貫穿作業的孔徑與刀庫匹配不到
        M_BOX_Penetrate_NoMatch,

        [ErrorCodeAttribute("自動模式下完全無加工需求執行", null)]
        //自動模式下完全無加工需求執行
        Auto_Mode_NoNeed_Work,

        [ErrorCodeAttribute("手臂未夾持", null)]
        //手臂未夾持
        Arm_Not_Clip_Error,

        [ErrorCodeAttribute("料件未靠邊", null)]
        //料件未靠邊
        Arm_Not_Close_Side,

        [ErrorCodeAttribute("手臂送料檢測點已感測", null)]
        //手臂送料檢測點已感測
        Arm_Feed_Sensor_On,

        [ErrorCodeAttribute("自動尻料靠邊異常", null)]
        //自動尻料靠邊異常
        Auto_Arm_Close_Side_Error,

        [ErrorCodeAttribute("切換夾持異常", null)]
        //切換夾持異常
        Change_Clip_Error,

        [ErrorCodeAttribute("自動夾持發生異常_T1尺寸異常", null)]
        //自動夾持發生異常_T1尺寸異常
        Auto_Arm_Clip_T1_Error,

        [ErrorCodeAttribute("自動夾持發生異常_T2尺寸異常", null)]
        //自動夾持發生異常_T2尺寸異常
        Auto_Arm_Clip_T2_Error,

        [ErrorCodeAttribute("自動夾持發生異常_H尺寸異常", null)]
        //自動夾持發生異常_H尺寸異常
        Auto_Arm_Clip_H_Error,

        [ErrorCodeAttribute("自動夾持判別靠邊異常", null)]
        //自動夾持判別靠邊異常
        Auto_Arm_Clip_Side_ERROR,

        [ErrorCodeAttribute("自動夾持其他尺寸異常", null)]
        //自動夾持其他尺寸異常
        Auto_Arm_Clip_Other_ERROR,

        [ErrorCodeAttribute("送料長度異常", null)]
        //送料長度異常
        Auto_Arm_Material_Length_Error,

        [ErrorCodeAttribute("側壓夾持料件H尺寸異常", null)]
        //側壓夾持料件H尺寸異常
        Side_Clip_Material_H_Error,

        [ErrorCodeAttribute("下壓夾持料件W尺寸異常", null)]
        //下壓夾持料件W尺寸異常
        Side_Clip_Material_W_Error,

        [ErrorCodeAttribute("下壓夾持料件W尺寸異常_左進", null)]
        //下壓夾持料件W尺寸異常_左進
        Side_Clip_Material_W_Error_L_IN,

        [ErrorCodeAttribute("下壓夾持料件W尺寸異常_左出", null)]
        //下壓夾持料件W尺寸異常_左出
        Side_Clip_Material_W_Error_L_Out,

        [ErrorCodeAttribute("下壓夾持料件W尺寸異常_右進", null)]
        //下壓夾持料件W尺寸異常_右進
        Side_Clip_Material_W_Error_R_IN,

        [ErrorCodeAttribute("下壓夾持料件W尺寸異常_右出", null)]
        //下壓夾持料件W尺寸異常_右出
        Side_Clip_Material_W_Error_R_Out,

        [ErrorCodeAttribute("水平夾爪Sensor異常", null)]
        //水平夾爪Sensor異常
        Level_Clip_Touch_Sensor_Error,

        [ErrorCodeAttribute("垂直夾爪Sensor異常", null)]
        //垂直夾爪Sensor異常
        Vertical_Clip_Touch_Sensor_Error,

        [ErrorCodeAttribute("送料減速點Sensor異常", null)]
        //送料減速點Sensor異常
        Feed_Slow_Down_Sensor_Error,

        [ErrorCodeAttribute("送料原點Sensor異常", null)]
        //送料原點Sensor異常
        Feed_Origin_Sensor_Error,

        [ErrorCodeAttribute("左軸測刀長Sensor異常", null)]
        //左軸測刀長Sensor異常
        L_Measuring_Drill_Sensor_Error,

        [ErrorCodeAttribute("中軸測刀長Sensor異常", null)]
        //中軸測刀長Sensor異常
        M_Measuring_Drill_Sensor_Error,

        [ErrorCodeAttribute("右軸測刀長Sensor異常", null)]
        //右軸測刀長Sensor異常
        R_Measuring_Drill_Sensor_Error,

        [ErrorCodeAttribute("強電箱安全門未關", null)]
        //強電箱安全門鎖定異常
        Electrical_BOX_Error,

        [ErrorCodeAttribute("外罩安全門(1)未關", null)]
        //外罩安全門(1)鎖定異常
        Case_1_Error,

        [ErrorCodeAttribute("外罩安全門(2)未關", null)]
        //外罩安全門(2)鎖定異常
        Case_2_Error,

        [ErrorCodeAttribute("放刀索引錯誤", null)]
        //放刀索引錯誤
        //手動要求放刀,但是與線上刀具索引不符
        Put_Drill_Index_Error,

        //(*Error_Code 10000~15000 開始為在Error中可以使用原點復歸去復位並清除Error*******************************************)

        [ErrorCodeAttribute("軸向左與中即將碰撞", null)]
        //軸向左與中即將碰撞_10000
        Axis_LAndM_Touch = 10000,

        [ErrorCodeAttribute("軸向中與右即將碰撞", null)]
        //軸向中與右即將碰撞
        Axis_MAndR_Touch,

        [ErrorCodeAttribute("軸向左與右即將碰撞", null)]
        //軸向左與右即將碰撞
        Axis_LAndR_Touch,

        [ErrorCodeAttribute("左X負極限觸發", null)]
        //左X負極限觸發
        L_X_LimitBack,

        [ErrorCodeAttribute("左X正極限觸發", null)]
        //左X正極限觸發
        L_X_LimitFornt,

        [ErrorCodeAttribute("左Y負極限觸發", null)]
        //左Y負極限觸發
        L_Y_LimitBack,

        [ErrorCodeAttribute("左Y正極限觸發", null)]
        //左Y正極限觸發
        L_Y_LimitFront,

        [ErrorCodeAttribute("左Z負極限觸發", null)]
        //左Z負極限觸發
        L_Z_LimitBack,

        [ErrorCodeAttribute("右X負極限觸發", null)]
        //右X負極限觸發
        R_X_LimitBack,

        [ErrorCodeAttribute("右X正極限觸發", null)]
        //右X正極限觸發
        R_X_LimitFornt,

        [ErrorCodeAttribute("右Y負極限觸發", null)]
        //右Y負極限觸發
        R_Y_LimitBack,

        [ErrorCodeAttribute("右Y正極限觸發", null)]
        //右Y正極限觸發
        R_Y_LimitFornt,

        [ErrorCodeAttribute("右Z負極限觸發", null)]
        //右Z負極限觸發
        R_Z_LimitBack,

        [ErrorCodeAttribute("右Z正極限觸發", null)]
        //右Z正極限觸發
        R_Z_LimitFornt,

        [ErrorCodeAttribute("上X負極限處發", null)]
        //上X負極限處發
        M_X_LimitBack,

        [ErrorCodeAttribute("上X正極限觸發", null)]
        //上X正極限觸發
        M_X_LimitFornt,

        [ErrorCodeAttribute("上Y負極限觸發", null)]
        //上Y負極限觸發
        M_Y_LimitBack,

        [ErrorCodeAttribute("上Y正極限觸發", null)]
        //上Y正極限觸發
        M_Y_LimitFornt,

        [ErrorCodeAttribute("上Z負極限觸發", null)]
        //上Z負極限觸發
        M_Z_LimitBack,

        [ErrorCodeAttribute("上Z正極限觸發", null)]
        //上Z正極限觸發
        M_Z_LimitFornt,

        [ErrorCodeAttribute("手臂X行程負極限觸發", null)]
        //手臂X行程負極限觸發
        Arm_X_Limit_Back,

        [ErrorCodeAttribute("手臂X行程正極限觸發", null)]
        //手臂X行程正極限觸發
        Arm_X_Limit_Fornt,

        [ErrorCodeAttribute("手臂IN_Z水平夾爪軸負極限觸發", null)]
        //手臂IN_Z水平夾爪軸負極限觸發
        Arm_In_Z_Limit_Back,

        [ErrorCodeAttribute("手臂IN_Z水平夾爪軸正極限觸發", null)]
        //手臂IN_Z水平夾爪軸正極限觸發
        Arm_In_Z_Limit_Front,

        [ErrorCodeAttribute("手臂OUT_Z垂直夾爪軸負極限觸發", null)]
        //手臂OUT_Z垂直夾爪軸負極限觸發
        Arm_Out_Z_Limit_Back,

        [ErrorCodeAttribute("手臂OUT_Z垂直夾爪軸正極限觸發", null)]
        //手臂OUT_Z垂直夾爪軸正極限觸發
        Arm_Out_Z_Limit_Front,

        [ErrorCodeAttribute("L_X_軸向座標超過正負極限", null)]
        //L_X_軸向座標超過正負極限
        L_X_Axis_Position_error,

        [ErrorCodeAttribute("L_Y_軸向座標超過正負極限", null)]
        //L_Y_軸向座標超過正負極限
        L_Y_Axis_Position_error,

        [ErrorCodeAttribute("L_Z_軸向座標超過正負極限", null)]
        //L_Z_軸向座標超過正負極限
        L_Z_Axis_Position_error,

        [ErrorCodeAttribute("M_X_軸向座標超過正負極限", null)]
        //M_X_軸向座標超過正負極限
        M_X_Axis_Position_error,

        [ErrorCodeAttribute("M_Y_軸向座標超過正負極限", null)]
        //M_Y_軸向座標超過正負極限
        M_Y_Axis_Position_error,

        [ErrorCodeAttribute("M_Z_軸向座標超過正負極限", null)]
        //M_Z_軸向座標超過正負極限
        M_Z_Axis_Position_error,

        [ErrorCodeAttribute("R_X_軸向座標超過正負極限", null)]
        //R_X_軸向座標超過正負極限
        R_X_Axis_Position_error,

        [ErrorCodeAttribute("R_Y_軸向座標超過正負極限", null)]
        //R_Y_軸向座標超過正負極限
        R_Y_Axis_Position_error,

        [ErrorCodeAttribute("R_Z_軸向座標超過正負極限", null)]
        //R_Z_軸向座標超過正負極限
        R_Z_Axis_Position_error,

        [ErrorCodeAttribute("Arm_X_軸向座標超過正負極限", null)]
        //Arm_X_軸向座標超過正負極限
        Arm_X_Axis_Position_error,

        [ErrorCodeAttribute("Arm_In_Z_水平夾爪_軸向座標超過正負極限", null)]
        //Arm_In_Z_水平夾爪_軸向座標超過正負極限
        Arm_In_Z_Axis_Position_error,

        [ErrorCodeAttribute("Arm_Out_Z_垂直夾爪_軸向座標超過正負極限", null)]
        //Arm_Out_Z_垂直夾爪_軸向座標超過正負極限
        Arm_Out_Z_Axis_Position_error,

        [ErrorCodeAttribute("中軸相位找尋異常_", null)]
        //中軸相位找尋異常_
        M_Phase_Error,

        [ErrorCodeAttribute("左軸打點異常(接觸不到素材)", null)]
        //左軸打點異常(接觸不到素材)
        L_Point_Abnormal,
        [ErrorCodeAttribute("中軸打點異常(接觸不到素材)", null)]
        //中軸打點異常(接觸不到素材)
        M_Point_Abnormal,
        [ErrorCodeAttribute("右軸打點異常(接觸不到素材)", null)]
        //右軸打點異常(接觸不到素材)
        R_Point_Abnormal,
        [ErrorCodeAttribute("JOB X軸向排序或重複孔位異常", null)]
        //JOB X軸向排序或重複孔位異常
        Sort_Repeat_Error,
        [ErrorCodeAttribute("素材小於5米且無使用T1夾取", null)]
        //素材小於5米且無使用T1夾取
        No_Use_T1_Clip,

        //(*Error_Code20000 開始為使用APP時發生的Error*) //***************************************

        [ErrorCodeAttribute("手機操作KEY不在手動_20000", null)]
        //手機操作KEY不在手動_20000
        Phone_Not_Manual = 20000,

        [ErrorCodeAttribute("目前機器正在執行工作，請等待機器工作執行完畢_20001", null)]
        //目前機器正在執行工作，請等待機器工作執行完畢_20001
        Phone_Machine_Busy,

        [ErrorCodeAttribute("PC端拒絕手機APP連線控制_20002", null)]
        //PC端拒絕手機APP連線控制_20002
        PC_Refuse_Phone_Connect,

        //橫移異常
        //手臂夾持異常
        //下壓複測異常
        //側壓異常
        //高度複測異常	

        //以下是原點無法復歸的異常 30000起 ********************************************

        [ErrorCodeAttribute("總氣壓壓力異常_30000", null)]
        //總氣壓壓力異常_30000
        AirPSI_NG = 30000,

        [ErrorCodeAttribute("油壓系統1_油量異常_30001", null)]
        //油壓系統1_油量異常_30001
        Hydraulic_1_Volume_NG,

        [ErrorCodeAttribute("感應馬達過載_30002", null)]
        //感應馬達過載_30002
        TH_RY_NG,

        [ErrorCodeAttribute("左X未復位_30003", null)]
        //左X未復位_30003
        L_X_NG,

        [ErrorCodeAttribute("左Y未復位_30004", null)]
        //左Y未復位_30004
        L_Y_NG,

        [ErrorCodeAttribute("左Z未復位_30005", null)]
        //左Z未復位_30005
        L_Z_NG,

        [ErrorCodeAttribute("右X未復位_30006", null)]
        //右X未復位_30006
        R_X_NG,

        [ErrorCodeAttribute("右Y未復位_30007", null)]
        //右Y未復位_30007
        R_Y_NG,

        [ErrorCodeAttribute("右Z未復位_30008", null)]
        //右Z未復位_30008
        R_Z_NG,

        [ErrorCodeAttribute("上X未復位_30009", null)]
        //上X未復位_30009
        M_X_NG,

        [ErrorCodeAttribute("上Y未復位_30010", null)]
        //上Y未復位_30010
        M_Y_NG,

        [ErrorCodeAttribute("上Z未復位_30011", null)]
        //上Z未復位_30011
        M_Z_NG,

        [ErrorCodeAttribute("左主軸上未夾刀_30012", null)]
        //左主軸上未夾刀_30012
        L_SpindleClip_NG,

        [ErrorCodeAttribute("右主軸上未夾刀_30013", null)]
        //右主軸上未夾刀_30013
        R_SpindleClip_NG,

        [ErrorCodeAttribute("上主軸上未夾刀_30014", null)]
        //上主軸上未夾刀_30014
        M_SpindleClip_NG,

        [ErrorCodeAttribute("上刀庫未復位_30015", null)]
        //上刀庫未復位_30015
        M_DrillHomeOrigin_NG,

        [ErrorCodeAttribute("左進料刀庫未復位_30016", null)]
        //左進料刀庫未復位_30016
        L_IN_DrillHomeOrigin_NG,

        [ErrorCodeAttribute("左出料刀庫未復位_30017", null)]
        //左出料刀庫未復位_30017
        L_OUT_DrillHomeOrigin_NG,

        [ErrorCodeAttribute("右進料刀庫未復位_30018", null)]
        //右進料刀庫未復位_30018
        R_IN_DrillHomeOrigin_NG,

        [ErrorCodeAttribute("右出料刀庫未復位_30019", null)]
        //右出料刀庫未復位_30019
        R_OUT_DrillHomeOrigin_NG,

        [ErrorCodeAttribute("左進料下壓未復位_30021", null)]
        //左進料下壓未復位_30021
        L_IN_Clip_Down_NG,

        [ErrorCodeAttribute("左出料下壓未復位_30022", null)]
        //左出料下壓未復位_30022
        L_OUT_Clip_Down_NG,

        [ErrorCodeAttribute("右進料下壓未復位_30023", null)]
        //右進料下壓未復位_30023
        R_IN_Clip_Down_NG,

        [ErrorCodeAttribute("右出料下壓未復位_30024", null)]
        //右出料下壓未復位_30024
        R_OUT_Clip_Down_NG,

        [ErrorCodeAttribute("側壓未復位_30025", null)]
        //側壓未復位_30025
        Side_Clip_NG,

        [ErrorCodeAttribute("強電箱門未關閉_30026", null)]
        //強電箱門未關閉_30026
        Electrical_BOX_NO_Close,

        [ErrorCodeAttribute("外罩門一未關閉_30027", null)]
        //外罩門一未關閉_30027
        Case_1_No_Close,

        [ErrorCodeAttribute("外罩門二未關閉_30028", null)]
        //外罩門二未關閉_30028
        Case_2_No_Close,
        
        [ErrorCodeAttribute("研華電池沒電", null)]
        //研華電池沒電
        Advantech_battery_No_Power,
        
        [ErrorCodeAttribute("刀具使用中超過1把", null)]
        //刀具使用中超過1把
        FUN_Double_Use,

        [ErrorCodeAttribute("無刀庫選配但有資料", null)]
        //無刀庫選配但有資料
        DrillDateError,

        [ErrorCodeAttribute("刀庫資訊有問題", null)]
        //刀庫資訊有問題
        DrillHomeDataError,

        [ErrorCodeAttribute("MotionEthercat連線異常", null)]
        //MotionEthercat連線異常
        MotionEthercatError,

        [ErrorCodeAttribute("IO Ethercat連線異常", null)]
        //IO Ethercat連線異常
        IOEthercatError,


        //[ErrorCodeAttribute("外罩門三未關閉_30029", null)]
        //外罩門三未關閉_30029
        //Case_3_NG,

        //*******************************馬達無法Servo_On 4萬起

        [ErrorCodeAttribute("L_X無法Servo_ON", null)]
        //L_X無法Servo_ON
        L_X_Cant_ON = 40000,

        [ErrorCodeAttribute("L_Y無法Servo_ON", null)]
        //L_Y無法Servo_ON
        L_Y_Cant_ON,

        [ErrorCodeAttribute("L_Z無法Servo_ON", null)]
        //L_Z無法Servo_ON
        L_Z_Cant_ON,

        [ErrorCodeAttribute("M_X無法Servo_ON", null)]
        //M_X無法Servo_ON
        M_X_Cant_ON,

        [ErrorCodeAttribute("M_Y無法Servo_ON", null)]
        //M_Y無法Servo_ON
        M_Y_Cant_ON,

        [ErrorCodeAttribute("M_Z無法Servo_ON", null)]
        //M_Z無法Servo_ON
        M_Z_Cant_ON,

        [ErrorCodeAttribute("R_X無法Servo_ON", null)]
        //R_X無法Servo_ON
        R_X_Cant_ON,

        [ErrorCodeAttribute("R_Y無法Servo_ON", null)]
        //R_Y無法Servo_ON
        R_Y_Cant_ON,

        [ErrorCodeAttribute("R_Z無法Servo_ON", null)]
        //R_Z無法Servo_ON
        R_Z_Cant_ON,

        [ErrorCodeAttribute("Arm_X無法Servo_ON", null)]
        //Arm_X無法Servo_ON
        Arm_X_Cant_ON,

        [ErrorCodeAttribute("Arm_In_Z無法Servo_ON", null)]
        //Arm_In_Z無法Servo_ON
        Arm_In_Z_Cant_ON,

        [ErrorCodeAttribute("Arm_Out_Z無法Servo_ON", null)]
        //Arm_Out_Z無法Servo_ON
        Arm_Out_Z_Cant_ON,

        [ErrorCodeAttribute("左主軸無法Servo_ON", null)]
        //左主軸無法Servo_ON
        L_Spindle_Cant_ON,

        [ErrorCodeAttribute("中主軸無法Servo_ON", null)]
        //中主軸無法Servo_ON
        M_Spindle_Cant_ON,

        [ErrorCodeAttribute("右主軸無法Servo_ON", null)]
        //右主軸無法Servo_ON
        R_Spindle_Cant_ON,

        //********************************************Driver警報_10萬起*******************************

        [ErrorCodeAttribute("L_X_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱", null)]
        //L_X_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        L_X_A100 = 100256,

        [ErrorCodeAttribute("L_Y_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱", null)]
        //L_Y_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        L_Y_A100 = 200256,

        [ErrorCodeAttribute("L_Z_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱", null)]
        //L_Z_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        L_Z_A100 = 300256,

        [ErrorCodeAttribute("M_X_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱", null)]
        //M_X_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        M_X_A100 = 400256,

        [ErrorCodeAttribute("M_Y_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱", null)]
        //M_Y_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        M_Y_A100 = 500256,

        [ErrorCodeAttribute("M_Z_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱", null)]
        //M_Z_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        M_Z_A100 = 600256,

        [ErrorCodeAttribute("R_X_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱", null)]
        //R_X_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        R_X_A100 = 700256,

        [ErrorCodeAttribute("R_Y_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱", null)]
        //R_Y_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        R_Y_A100 = 800256,

        [ErrorCodeAttribute("R_Z_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱", null)]
        //R_Z_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        R_Z_A100 = 900256,

        [ErrorCodeAttribute("Arm_X_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱", null)]
        //Arm_X_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        Arm_X_A100 = 1000256,

        [ErrorCodeAttribute("Arm_IN_Z_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱", null)]
        //Arm_IN_Z_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        Arm_IN_Z_A100 = 1100256,

        [ErrorCodeAttribute("Arm_Out_Z_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱", null)]
        //Arm_Out_Z_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        Arm_Out_Z_A100 = 1200256,

        [ErrorCodeAttribute("L_X_再生電阻過載", null)]
        //L_X_再生電阻過載
        L_X_A320 = 100800,

        [ErrorCodeAttribute("L_Y再生電阻過載", null)]
        //L_Y再生電阻過載
        L_Y_A320 = 200800,

        [ErrorCodeAttribute("L_Z_再生電阻過載", null)]
        //L_Z_再生電阻過載
        L_Z_A320 = 300800,

        [ErrorCodeAttribute("M_X_再生電阻過載", null)]
        //M_X_再生電阻過載
        M_X_A320 = 400800,

        [ErrorCodeAttribute("M_Y再生電阻過載", null)]
        //M_Y再生電阻過載
        M_Y_A320 = 500800,

        [ErrorCodeAttribute("M_Z_再生電阻過載", null)]
        //M_Z_再生電阻過載
        M_Z_A320 = 600800,

        [ErrorCodeAttribute("R_X_再生電阻過載", null)]
        //R_X_再生電阻過載
        R_X_A320 = 700800,

        [ErrorCodeAttribute("R_Y再生電阻過載", null)]
        //R_Y再生電阻過載
        R_Y_A320 = 800800,

        [ErrorCodeAttribute("R_Z_再生電阻過載", null)]
        //R_Z_再生電阻過載
        R_Z_A320 = 900800,

        [ErrorCodeAttribute("Arm_X_再生電阻過載", null)]
        //Arm_X_再生電阻過載
        Arm_X_A320 = 1000800,

        [ErrorCodeAttribute("Arm_IN_Z再生電阻過載", null)]
        //Arm_IN_Z再生電阻過載
        Arm_In_Z_A320 = 1100800,

        [ErrorCodeAttribute("Arm_Out_Z_再生電阻過載", null)]
        //Arm_Out_Z_再生電阻過載
        Arm_Out_Z_A320 = 1200800,

        [ErrorCodeAttribute("L_X_Encoder電池警報", null)]
        //L_X_Encoder電池警報
        L_X_A830 = 102096,

        [ErrorCodeAttribute("L_Y_Encoder電池警報", null)]
        //L_Y_Encoder電池警報
        L_Y_A830 = 202096,

        [ErrorCodeAttribute("L_Z_Encoder電池警報", null)]
        //L_Z_Encoder電池警報
        L_Z_A830 = 302096,

        [ErrorCodeAttribute("M_X_Encoder電池警報", null)]
        //M_X_Encoder電池警報
        M_X_A830 = 402096,

        [ErrorCodeAttribute("M_Y_Encoder電池警報", null)]
        //M_Y_Encoder電池警報
        M_Y_A830 = 502096,

        [ErrorCodeAttribute("M_Z_Encoder電池警報", null)]
        //M_Z_Encoder電池警報
        M_Z_A830 = 602096,

        [ErrorCodeAttribute("R_X_Encoder電池警報", null)]
        //R_X_Encoder電池警報
        R_X_A830 = 702096,

        [ErrorCodeAttribute("R_Y_Encoder電池警報", null)]
        //R_Y_Encoder電池警報
        R_Y_A830 = 802096,

        [ErrorCodeAttribute("R_Z_Encoder電池警報", null)]
        //R_Z_Encoder電池警報
        R_Z_A830 = 902096,

        [ErrorCodeAttribute("Arm_X_Encoder電池警報", null)]
        //Arm_X_Encoder電池警報
        Arm_X_A830 = 1002096,

        [ErrorCodeAttribute("Arm_IN_Z_Encoder電池警報", null)]
        //Arm_IN_Z_Encoder電池警報
        Arm_In_Z_A830 = 1102096,

        [ErrorCodeAttribute("Arm_Out_Z_Encoder電池警報", null)]
        //Arm_Out_Z_Encoder電池警報
        Arm_Out_Z_A830 = 1202096,

        [ErrorCodeAttribute("L_X_通訊中斷", null)]
        //L_X_通訊中斷
        L_X_AA12 =102578,
        [ErrorCodeAttribute("L_Y_通訊中斷", null)]
        //L_Y_通訊中斷
        L_Y_AA12 =202578,
        [ErrorCodeAttribute("L_Z_通訊中斷", null)]
        //L_Z_通訊中斷
        L_Z_AA12 =302578,
        [ErrorCodeAttribute("M_X_通訊中斷", null)]
        //M_X_通訊中斷
        M_X_AA12 =402578,
        [ErrorCodeAttribute("M_Y_通訊中斷", null)]
        //M_Y_通訊中斷
        M_Y_AA12 =502578,
        [ErrorCodeAttribute("M_Z_通訊中斷", null)]
        //M_Z_通訊中斷
        M_Z_AA12 =602578,
        [ErrorCodeAttribute("R_X_通訊中斷", null)]
        //R_X_通訊中斷
        R_X_AA12 =702578,
        [ErrorCodeAttribute("R_Y_通訊中斷", null)]
        //R_Y_通訊中斷
        R_Y_AA12 =802578,
        [ErrorCodeAttribute("R_Z_通訊中斷", null)]
        //R_Z_通訊中斷
        R_Z_AA12 =902578,
        [ErrorCodeAttribute("Arm_X_通訊中斷", null)]
        //Arm_X_通訊中斷
        Arm_X_AA12 =1002578,
        [ErrorCodeAttribute("Arm_In_Z_通訊中斷", null)]
        //Arm_In_Z_通訊中斷
        Arm_In_Z_AA12 =1102578,
        [ErrorCodeAttribute("Arm_Out_Z_通訊中斷", null)]
        //Arm_Out_Z_通訊中斷
        Arm_Out_Z_AA12 =1202578,

        //左軸相位找尋異常_3345677
        //L_Phase_Error :=3345677,

        //右軸相位找尋異常_3345679
        //R_Phase_Error :=3345679,
        //Z軸閘門未關_30020
        //Z_HighSensorDoor_NG,
        //有軸向的位置跑掉異常,需跳警報告知回原點自動重新定位
        //Axis_Position_NG,


        //[ErrorCodeAttribute("左X馬達Error_Stop", null)]
        ////左X馬達Error_Stop
        //L_X_Error_Stop,
        //[ErrorCodeAttribute("左Y馬達Error_Stop", null)]
        ////左Y馬達Error_Stop
        //L_Y_Error_Stop,
        //[ErrorCodeAttribute("左Z馬達Error_Stop", null)]
        ////左Z馬達Error_Stop
        //L_Z_Error_Stop,
        //[ErrorCodeAttribute("中X馬達Error_Stop", null)]
        ////中X馬達Error_Stop
        //M_X_Error_Stop,
        //[ErrorCodeAttribute("中Y馬達Error_Stop", null)]
        ////中Y馬達Error_Stop
        //M_Y_Error_Stop,
        //[ErrorCodeAttribute("中Z馬達Error_Stop", null)]
        ////中Z馬達Error_Stop
        //M_Z_Error_Stop,
        //[ErrorCodeAttribute("右X馬達Error_Stop", null)]
        ////右X馬達Error_Stop
        //R_X_Error_Stop,
        //[ErrorCodeAttribute("右Y馬達Error_Stop", null)]
        ////右Y馬達Error_Stop
        //R_Y_Error_Stop,
        //[ErrorCodeAttribute("右Z馬達Error_Stop", null)]
        ////右Z馬達Error_Stop
        //R_Z_Error_Stop,
        //[ErrorCodeAttribute("手臂_X馬達Error_Stop", null)]
        ////手臂_X馬達Error_Stop
        //Arm_X_Error_Stop,
        //[ErrorCodeAttribute("手臂_IN_Z馬達Error_Stop", null)]
        ////手臂_IN_Z馬達Error_Stop
        //Arm_In_Z_Error_Stop,
        //[ErrorCodeAttribute("手臂_Out_Z馬達Error_Stop", null)]
        ////手臂_Out_Z馬達Error_Stop
        //Arm_Out_Z_Error_Stop,
        //[ErrorCodeAttribute("左主軸Error_Stop", null)]
        ////左主軸Error_Stop
        //L_Spindle_Error_Stop,
        //[ErrorCodeAttribute("中主軸Error_Stop", null)]
        ////中主軸Error_Stop
        //M_Spindle_Error_Stop,
        //[ErrorCodeAttribute("右主軸Error_Stop", null)]
        ////右主軸Error_Stop
        //R_Spindle_Error_Stop,

        [ErrorCodeAttribute("無錯誤碼", null)]
        //最大值(無錯誤碼給的值)
        Code_max = 4294967295,

        Unknown = UInt32.MaxValue,

        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Null' 的 XML 註解
        //        //左進料刀庫1與設定不符
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_IN_DrillHome1_Error' 的 XML 註解
        //        L_IN_DrillHome1_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_IN_DrillHome1_Error' 的 XML 註解
        //        //左進料刀庫2與設定不符
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_IN_DrillHome2_Error' 的 XML 註解
        //        L_IN_DrillHome2_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_IN_DrillHome2_Error' 的 XML 註解
        //        //左進料刀庫3與設定不符
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_IN_DrillHome3_Error' 的 XML 註解
        //        L_IN_DrillHome3_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_IN_DrillHome3_Error' 的 XML 註解
        //        //左進料刀庫4與設定不符
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_IN_DrillHome4_Error' 的 XML 註解
        //        L_IN_DrillHome4_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_IN_DrillHome4_Error' 的 XML 註解
        //        //左出料刀庫1與設定不符
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Out_DrillHome1_Error' 的 XML 註解
        //        L_Out_DrillHome1_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Out_DrillHome1_Error' 的 XML 註解
        //        //左出料刀庫2與設定不符
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Out_DrillHome2_Error' 的 XML 註解
        //        L_Out_DrillHome2_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Out_DrillHome2_Error' 的 XML 註解
        //        //左出料刀庫3與設定不符
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Out_DrillHome3_Error' 的 XML 註解
        //        L_Out_DrillHome3_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Out_DrillHome3_Error' 的 XML 註解
        //        //左出料刀庫4與設定不符
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Out_DrillHome4_Error' 的 XML 註解
        //        L_Out_DrillHome4_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Out_DrillHome4_Error' 的 XML 註解
        //        //右進料刀庫1與設定不符
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_IN_DrillHome1_Error' 的 XML 註解
        //        R_IN_DrillHome1_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_IN_DrillHome1_Error' 的 XML 註解
        //        //右進料刀庫1與設定不符
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_IN_DrillHome2_Error' 的 XML 註解
        //        R_IN_DrillHome2_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_IN_DrillHome2_Error' 的 XML 註解
        //        //右進料刀庫1與設定不符
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_IN_DrillHome3_Error' 的 XML 註解
        //        R_IN_DrillHome3_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_IN_DrillHome3_Error' 的 XML 註解
        //        //右進料刀庫1與設定不符
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_IN_DrillHome4_Error' 的 XML 註解
        //        R_IN_DrillHome4_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_IN_DrillHome4_Error' 的 XML 註解
        //        //右出料刀庫1與設定不符
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Out_DrillHome1_Error' 的 XML 註解
        //        R_Out_DrillHome1_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Out_DrillHome1_Error' 的 XML 註解
        //        //右出料刀庫2與設定不符
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Out_DrillHome2_Error' 的 XML 註解
        //        R_Out_DrillHome2_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Out_DrillHome2_Error' 的 XML 註解
        //        //右出料刀庫3與設定不符
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Out_DrillHome3_Error' 的 XML 註解
        //        R_Out_DrillHome3_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Out_DrillHome3_Error' 的 XML 註解
        //        //右出料刀庫4與設定不符
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Out_DrillHome4_Error' 的 XML 註解
        //        R_Out_DrillHome4_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Out_DrillHome4_Error' 的 XML 註解
        //        //中軸刀庫1與設定不符
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_DrillHome1_Error' 的 XML 註解
        //        M_DrillHome1_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_DrillHome1_Error' 的 XML 註解
        //        //中軸刀庫2與設定不符
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_DrillHome2_Error' 的 XML 註解
        //        M_DrillHome2_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_DrillHome2_Error' 的 XML 註解
        //        //中軸刀庫3與設定不符
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_DrillHome3_Error' 的 XML 註解
        //        M_DrillHome3_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_DrillHome3_Error' 的 XML 註解
        //        //中軸刀庫4與設定不符
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_DrillHome4_Error' 的 XML 註解
        //        M_DrillHome4_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_DrillHome4_Error' 的 XML 註解
        //        //中軸刀庫5與設定不符
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_DrillHome5_Error' 的 XML 註解
        //        M_DrillHome5_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_DrillHome5_Error' 的 XML 註解
        //        //Ethercat未啟動
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Ethercat_NG' 的 XML 註解
        //        Ethercat_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Ethercat_NG' 的 XML 註解
        //        //自動模式下不在原點
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.AutoNotOnOrigin' 的 XML 註解
        //        AutoNotOnOrigin,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.AutoNotOnOrigin' 的 XML 註解

        //        //左軸刀長量測異常
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Drill_Length_Error' 的 XML 註解
        //        L_Drill_Length_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Drill_Length_Error' 的 XML 註解
        //        //中軸刀長量測異常
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Drill_Length_Error' 的 XML 註解
        //        M_Drill_Length_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Drill_Length_Error' 的 XML 註解
        //        //右軸刀長量測異常
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Drill_Length_Error' 的 XML 註解
        //        R_Drill_Length_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Drill_Length_Error' 的 XML 註解

        //        //加工陣列判斷有干涉
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Work_Arr_Error' 的 XML 註解
        //        Work_Arr_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Work_Arr_Error' 的 XML 註解
        //        //貫穿作業的孔徑與刀庫匹配不到
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_BOX_Penetrate_NoMatch' 的 XML 註解
        //        M_BOX_Penetrate_NoMatch,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_BOX_Penetrate_NoMatch' 的 XML 註解

        //        //自動模式下完全無加工需求執行
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Auto_Mode_NoNeed_Work' 的 XML 註解
        //        Auto_Mode_NoNeed_Work,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Auto_Mode_NoNeed_Work' 的 XML 註解

        //        //左軸相位原點Sensor異常
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Phase_Sensor_Error' 的 XML 註解
        //        L_Phase_Sensor_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Phase_Sensor_Error' 的 XML 註解
        //        //右軸相位原點Sensor異常
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Phase_Sensor_Error' 的 XML 註解
        //        R_Phase_Sensor_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Phase_Sensor_Error' 的 XML 註解

        //        //手臂夾持失敗
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_Clamp_Failed' 的 XML 註解
        //        Arm_Clamp_Failed,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_Clamp_Failed' 的 XML 註解

        //        //料件尺寸雷射測距異常
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Laser_Material_Error' 的 XML 註解
        //        Laser_Material_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Laser_Material_Error' 的 XML 註解

        //        //手臂未夾持
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_Not_Clip_Error' 的 XML 註解
        //        Arm_Not_Clip_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_Not_Clip_Error' 的 XML 註解
        //        //料件未靠邊
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_Not_Close_Side' 的 XML 註解
        //        Arm_Not_Close_Side,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_Not_Close_Side' 的 XML 註解
        //        //手臂送料檢測點已感測
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_Feed_Sensor_On' 的 XML 註解
        //        Arm_Feed_Sensor_On,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_Feed_Sensor_On' 的 XML 註解
        //        //自動手臂靠邊異常
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Auto_Arm_Close_Side_Error' 的 XML 註解
        //        Auto_Arm_Close_Side_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Auto_Arm_Close_Side_Error' 的 XML 註解
        //        //自動夾持檢測翼板尺寸異常
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Auto_Arm_Clip_W_SIZE_Error' 的 XML 註解
        //        Auto_Arm_Clip_W_SIZE_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Auto_Arm_Clip_W_SIZE_Error' 的 XML 註解
        //        //自動送料長度檢測異常
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Auto_Arm_Material_Length_Error' 的 XML 註解
        //        Auto_Arm_Material_Length_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Auto_Arm_Material_Length_Error' 的 XML 註解

        //        //側壓夾持料件H尺寸異常
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Side_Clip_Material_H_Error' 的 XML 註解
        //        Side_Clip_Material_H_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Side_Clip_Material_H_Error' 的 XML 註解
        //        //下壓夾持料件W尺寸異常
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Side_Clip_Material_W_Error' 的 XML 註解
        //        Side_Clip_Material_W_Error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Side_Clip_Material_W_Error' 的 XML 註解





        //        //軸向左與中即將碰撞_10000
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Axis_LAndM_Touch' 的 XML 註解
        //        Axis_LAndM_Touch = 10000,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Axis_LAndM_Touch' 的 XML 註解
        //        //軸向中與右即將碰撞
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Axis_MAndR_Touch' 的 XML 註解
        //        Axis_MAndR_Touch,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Axis_MAndR_Touch' 的 XML 註解
        //        //軸向左與右即將碰撞
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Axis_LAndR_Touch' 的 XML 註解
        //        Axis_LAndR_Touch,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Axis_LAndR_Touch' 的 XML 註解
        //        //左X負極限觸發
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_X_LimitBack' 的 XML 註解
        //        L_X_LimitBack,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_X_LimitBack' 的 XML 註解
        //        //左X正極限觸發
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_X_LimitFornt' 的 XML 註解
        //        L_X_LimitFornt,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_X_LimitFornt' 的 XML 註解
        //        //左Y負極限觸發
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Y_LimitBack' 的 XML 註解
        //        L_Y_LimitBack,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Y_LimitBack' 的 XML 註解
        //        //左Y正極限觸發
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Y_LimitFront' 的 XML 註解
        //        L_Y_LimitFront,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Y_LimitFront' 的 XML 註解
        //        //左Z負極限觸發
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Z_LimitBack' 的 XML 註解
        //        L_Z_LimitBack,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Z_LimitBack' 的 XML 註解
        //        //右X負極限觸發
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_X_LimitBack' 的 XML 註解
        //        R_X_LimitBack,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_X_LimitBack' 的 XML 註解
        //        //右X正極限觸發
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_X_LimitFornt' 的 XML 註解
        //        R_X_LimitFornt,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_X_LimitFornt' 的 XML 註解
        //        //右Y負極限觸發
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Y_LimitBack' 的 XML 註解
        //        R_Y_LimitBack,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Y_LimitBack' 的 XML 註解
        //        //右Y正極限觸發
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Y_LimitFornt' 的 XML 註解
        //        R_Y_LimitFornt,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Y_LimitFornt' 的 XML 註解
        //        //右Z負極限觸發
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Z_LimitBack' 的 XML 註解
        //        R_Z_LimitBack,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Z_LimitBack' 的 XML 註解
        //        //右Z正極限觸發
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Z_LimitFornt' 的 XML 註解
        //        R_Z_LimitFornt,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Z_LimitFornt' 的 XML 註解
        //        //上X負極限處發
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_X_LimitBack' 的 XML 註解
        //        M_X_LimitBack,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_X_LimitBack' 的 XML 註解
        //        //上X正極限觸發
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_X_LimitFornt' 的 XML 註解
        //        M_X_LimitFornt,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_X_LimitFornt' 的 XML 註解
        //        //上Y負極限觸發
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Y_LimitBack' 的 XML 註解
        //        M_Y_LimitBack,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Y_LimitBack' 的 XML 註解
        //        //上Y正極限觸發
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Y_LimitFornt' 的 XML 註解
        //        M_Y_LimitFornt,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Y_LimitFornt' 的 XML 註解
        //        //上Z負極限觸發
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Z_LimitBack' 的 XML 註解
        //        M_Z_LimitBack,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Z_LimitBack' 的 XML 註解
        //        //上Z正極限觸發
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Z_LimitFornt' 的 XML 註解
        //        M_Z_LimitFornt,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Z_LimitFornt' 的 XML 註解
        //        //手臂X行程負極限觸發
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_X_Limit_Back' 的 XML 註解
        //        Arm_X_Limit_Back,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_X_Limit_Back' 的 XML 註解
        //        //手臂X行程正極限觸發
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_X_Limit_Fornt' 的 XML 註解
        //        Arm_X_Limit_Fornt,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_X_Limit_Fornt' 的 XML 註解
        //        //手臂IN_Z水平夾爪軸負極限觸發
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_In_Z_Limit_Back' 的 XML 註解
        //        Arm_In_Z_Limit_Back,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_In_Z_Limit_Back' 的 XML 註解
        //        //手臂IN_Z水平夾爪軸正極限觸發
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_In_Z_Limit_Front' 的 XML 註解
        //        Arm_In_Z_Limit_Front,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_In_Z_Limit_Front' 的 XML 註解
        //        //手臂OUT_Z垂直夾爪軸負極限觸發
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_Out_Z_Limit_Back' 的 XML 註解
        //        Arm_Out_Z_Limit_Back,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_Out_Z_Limit_Back' 的 XML 註解
        //        //手臂OUT_Z垂直夾爪軸正極限觸發
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_Out_Z_Limit_Front' 的 XML 註解
        //        Arm_Out_Z_Limit_Front,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_Out_Z_Limit_Front' 的 XML 註解

        //        //有軸向的位置跑掉異常,需跳警報告知回原點自動重新定位
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Axis_Position_NG' 的 XML 註解
        //        Axis_Position_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Axis_Position_NG' 的 XML 註解

        //        //左X馬達Error_Stop
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_X_Error_Stop' 的 XML 註解
        //        L_X_Error_Stop,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_X_Error_Stop' 的 XML 註解
        //        //左Y馬達Error_Stop
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Y_Error_Stop' 的 XML 註解
        //        L_Y_Error_Stop,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Y_Error_Stop' 的 XML 註解
        //        //左Z馬達Error_Stop
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Z_Error_Stop' 的 XML 註解
        //        L_Z_Error_Stop,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Z_Error_Stop' 的 XML 註解
        //        //中X馬達Error_Stop
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_X_Error_Stop' 的 XML 註解
        //        M_X_Error_Stop,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_X_Error_Stop' 的 XML 註解
        //        //中Y馬達Error_Stop
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Y_Error_Stop' 的 XML 註解
        //        M_Y_Error_Stop,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Y_Error_Stop' 的 XML 註解
        //        //中Z馬達Error_Stop
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Z_Error_Stop' 的 XML 註解
        //        M_Z_Error_Stop,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Z_Error_Stop' 的 XML 註解
        //        //右X馬達Error_Stop
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_X_Error_Stop' 的 XML 註解
        //        R_X_Error_Stop,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_X_Error_Stop' 的 XML 註解
        //        //右Y馬達Error_Stop
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Y_Error_Stop' 的 XML 註解
        //        R_Y_Error_Stop,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Y_Error_Stop' 的 XML 註解
        //        //右Z馬達Error_Stop
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Z_Error_Stop' 的 XML 註解
        //        R_Z_Error_Stop,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Z_Error_Stop' 的 XML 註解
        //        //手臂_X馬達Error_Stop
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_X_Error_Stop' 的 XML 註解
        //        Arm_X_Error_Stop,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_X_Error_Stop' 的 XML 註解
        //        //手臂_IN_Z馬達Error_Stop
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_In_Z_Error_Stop' 的 XML 註解
        //        Arm_In_Z_Error_Stop,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_In_Z_Error_Stop' 的 XML 註解
        //        //手臂_Out_Z馬達Error_Stop
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_Out_Z_Error_Stop' 的 XML 註解
        //        Arm_Out_Z_Error_Stop,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_Out_Z_Error_Stop' 的 XML 註解
        //        //左主軸Error_Stop
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Spindle_Error_Stop' 的 XML 註解
        //        L_Spindle_Error_Stop,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Spindle_Error_Stop' 的 XML 註解
        //        //中主軸Error_Stop
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Spindle_Error_Stop' 的 XML 註解
        //        M_Spindle_Error_Stop,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Spindle_Error_Stop' 的 XML 註解
        //        //右主軸Error_Stop
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Spindle_Error_Stop' 的 XML 註解
        //        R_Spindle_Error_Stop,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Spindle_Error_Stop' 的 XML 註解

        //        //L_X_軸向座標超過正負極限
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_X_Axis_Position_error' 的 XML 註解
        //        L_X_Axis_Position_error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_X_Axis_Position_error' 的 XML 註解
        //        //L_Y_軸向座標超過正負極限
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Y_Axis_Position_error' 的 XML 註解
        //        L_Y_Axis_Position_error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Y_Axis_Position_error' 的 XML 註解
        //        //L_Z_軸向座標超過正負極限
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Z_Axis_Position_error' 的 XML 註解
        //        L_Z_Axis_Position_error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Z_Axis_Position_error' 的 XML 註解
        //        //M_X_軸向座標超過正負極限
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_X_Axis_Position_error' 的 XML 註解
        //        M_X_Axis_Position_error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_X_Axis_Position_error' 的 XML 註解
        //        //M_Y_軸向座標超過正負極限
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Y_Axis_Position_error' 的 XML 註解
        //        M_Y_Axis_Position_error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Y_Axis_Position_error' 的 XML 註解
        //        //M_Z_軸向座標超過正負極限
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Z_Axis_Position_error' 的 XML 註解
        //        M_Z_Axis_Position_error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Z_Axis_Position_error' 的 XML 註解
        //        //R_X_軸向座標超過正負極限
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_X_Axis_Position_error' 的 XML 註解
        //        R_X_Axis_Position_error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_X_Axis_Position_error' 的 XML 註解
        //        //R_Y_軸向座標超過正負極限
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Y_Axis_Position_error' 的 XML 註解
        //        R_Y_Axis_Position_error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Y_Axis_Position_error' 的 XML 註解
        //        //R_Z_軸向座標超過正負極限
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Z_Axis_Position_error' 的 XML 註解
        //        R_Z_Axis_Position_error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Z_Axis_Position_error' 的 XML 註解
        //        //Arm_X_軸向座標超過正負極限
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_X_Axis_Position_error' 的 XML 註解
        //        Arm_X_Axis_Position_error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_X_Axis_Position_error' 的 XML 註解
        //        //Arm_In_Z_水平夾爪_軸向座標超過正負極限
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_In_Z_Axis_Position_error' 的 XML 註解
        //        Arm_In_Z_Axis_Position_error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_In_Z_Axis_Position_error' 的 XML 註解
        //        //Arm_Out_Z_垂直夾爪_軸向座標超過正負極限
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_Out_Z_Axis_Position_error' 的 XML 註解
        //        Arm_Out_Z_Axis_Position_error,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_Out_Z_Axis_Position_error' 的 XML 註解



        // //手機操作KEY不在手動_20000
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Phone_Not_Manual' 的 XML 註解
        // Phone_Not_Manual=20000,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Phone_Not_Manual' 的 XML 註解
        //        //目前機器正在執行工作，請等待機器工作執行完畢_20001
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Phone_Machine_Busy' 的 XML 註解
        //        Phone_Machine_Busy,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Phone_Machine_Busy' 的 XML 註解
        //        //PC端拒絕手機APP連線控制_20002
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.PC_Refuse_Phone_Connect' 的 XML 註解
        //        PC_Refuse_Phone_Connect,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.PC_Refuse_Phone_Connect' 的 XML 註解




        //        //橫移異常
        //        //手臂夾持異常
        //        //下壓複測異常
        //        //側壓異常
        //        //高度複測異常 



        //        //以下是原點無法復歸的異常

        //        //總氣壓壓力異常_30000
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.AirPSI_NG' 的 XML 註解
        //        AirPSI_NG =30000,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.AirPSI_NG' 的 XML 註解
        //        //油壓系統1_油量異常_30001
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Hydraulic_1_Volume_NG' 的 XML 註解
        //        Hydraulic_1_Volume_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Hydraulic_1_Volume_NG' 的 XML 註解
        //        //感應馬達過載_30002
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.TH_RY_NG' 的 XML 註解
        //        TH_RY_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.TH_RY_NG' 的 XML 註解
        //        //左X未復位_30003
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_X_NG' 的 XML 註解
        //        L_X_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_X_NG' 的 XML 註解
        //        //左Y未復位_30004
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Y_NG' 的 XML 註解
        //        L_Y_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Y_NG' 的 XML 註解
        //        //左Z未復位_30005
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Z_NG' 的 XML 註解
        //        L_Z_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Z_NG' 的 XML 註解
        //        //右X未復位_30006
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_X_NG' 的 XML 註解
        //        R_X_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_X_NG' 的 XML 註解
        //        //右Y未復位_30007
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Y_NG' 的 XML 註解
        //        R_Y_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Y_NG' 的 XML 註解
        //        //右Z未復位_30008
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Z_NG' 的 XML 註解
        //        R_Z_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Z_NG' 的 XML 註解
        //        //上X未復位_30009
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_X_NG' 的 XML 註解
        //        M_X_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_X_NG' 的 XML 註解
        //        //上Y未復位_30010
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Y_NG' 的 XML 註解
        //        M_Y_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Y_NG' 的 XML 註解
        //        //上Z未復位_30011
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Z_NG' 的 XML 註解
        //        M_Z_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Z_NG' 的 XML 註解
        //        //左主軸上未夾刀_30012
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_SpindleClip_NG' 的 XML 註解
        //        L_SpindleClip_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_SpindleClip_NG' 的 XML 註解
        //        //右主軸上未夾刀_30013
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_SpindleClip_NG' 的 XML 註解
        //        R_SpindleClip_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_SpindleClip_NG' 的 XML 註解
        //        //上主軸上未夾刀_30014
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_SpindleClip_NG' 的 XML 註解
        //        M_SpindleClip_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_SpindleClip_NG' 的 XML 註解
        //        //上刀庫未復位_30015
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_DrillHomeOrigin_NG' 的 XML 註解
        //        M_DrillHomeOrigin_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_DrillHomeOrigin_NG' 的 XML 註解
        //        //左進料刀庫未復位_30016
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_IN_DrillHomeOrigin_NG' 的 XML 註解
        //        L_IN_DrillHomeOrigin_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_IN_DrillHomeOrigin_NG' 的 XML 註解
        //        //左出料刀庫未復位_30017
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_OUT_DrillHomeOrigin_NG' 的 XML 註解
        //        L_OUT_DrillHomeOrigin_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_OUT_DrillHomeOrigin_NG' 的 XML 註解
        //        //右進料刀庫未復位_30018
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_IN_DrillHomeOrigin_NG' 的 XML 註解
        //        R_IN_DrillHomeOrigin_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_IN_DrillHomeOrigin_NG' 的 XML 註解
        //        //右出料刀庫未復位_30019
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_OUT_DrillHomeOrigin_NG' 的 XML 註解
        //        R_OUT_DrillHomeOrigin_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_OUT_DrillHomeOrigin_NG' 的 XML 註解
        //        //Z軸閘門未關_30020
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Z_HighSensorDoor_NG' 的 XML 註解
        //        Z_HighSensorDoor_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Z_HighSensorDoor_NG' 的 XML 註解
        //        //左進料下壓未復位_30021
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_IN_Clip_Down_NG' 的 XML 註解
        //        L_IN_Clip_Down_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_IN_Clip_Down_NG' 的 XML 註解
        //        //左出料下壓未復位_30022
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_OUT_Clip_Down_NG' 的 XML 註解
        //        L_OUT_Clip_Down_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_OUT_Clip_Down_NG' 的 XML 註解
        //        //右進料下壓未復位_30023
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_IN_Clip_Down_NG' 的 XML 註解
        //        R_IN_Clip_Down_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_IN_Clip_Down_NG' 的 XML 註解
        //        //右出料下壓未復位_30024
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_OUT_Clip_Down_NG' 的 XML 註解
        //        R_OUT_Clip_Down_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_OUT_Clip_Down_NG' 的 XML 註解
        //        //側壓未復位_30025
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Side_Clip_NG' 的 XML 註解
        //        Side_Clip_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Side_Clip_NG' 的 XML 註解
        //        //強電箱門未關閉_30026
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Electrical_BOX_NG' 的 XML 註解
        //        Electrical_BOX_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Electrical_BOX_NG' 的 XML 註解
        //        //外罩門一未關閉_30027
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Case_1_NG' 的 XML 註解
        //        Case_1_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Case_1_NG' 的 XML 註解
        //        //外罩門二未關閉_30028
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Case_2_NG' 的 XML 註解
        //        Case_2_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Case_2_NG' 的 XML 註解
        //        //外罩門三未關閉_30029
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Case_3_NG' 的 XML 註解
        //        Case_3_NG,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Case_3_NG' 的 XML 註解





        //        //*******************************馬達無法Servo_On 4萬起
        //        //L_X無法Servo_ON
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_X_Cant_ON' 的 XML 註解
        //        L_X_Cant_ON=40000,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_X_Cant_ON' 的 XML 註解
        //        //L_Y無法Servo_ON
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Y_Cant_ON' 的 XML 註解
        //        L_Y_Cant_ON,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Y_Cant_ON' 的 XML 註解
        //        //L_Z無法Servo_ON
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Z_Cant_ON' 的 XML 註解
        //        L_Z_Cant_ON,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Z_Cant_ON' 的 XML 註解
        //        //M_X無法Servo_ON
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_X_Cant_ON' 的 XML 註解
        //        M_X_Cant_ON,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_X_Cant_ON' 的 XML 註解
        //        //M_Y無法Servo_ON
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Y_Cant_ON' 的 XML 註解
        //        M_Y_Cant_ON,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Y_Cant_ON' 的 XML 註解
        //        //M_Z無法Servo_ON
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Z_Cant_ON' 的 XML 註解
        //        M_Z_Cant_ON,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Z_Cant_ON' 的 XML 註解
        //        //R_X無法Servo_ON
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_X_Cant_ON' 的 XML 註解
        //        R_X_Cant_ON,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_X_Cant_ON' 的 XML 註解
        //        //R_Y無法Servo_ON
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Y_Cant_ON' 的 XML 註解
        //        R_Y_Cant_ON,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Y_Cant_ON' 的 XML 註解
        //        //R_Z無法Servo_ON
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Z_Cant_ON' 的 XML 註解
        //        R_Z_Cant_ON,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Z_Cant_ON' 的 XML 註解
        //        //Arm_X無法Servo_ON
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_X_Cant_ON' 的 XML 註解
        //        Arm_X_Cant_ON,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_X_Cant_ON' 的 XML 註解
        //        //Arm_In_Z無法Servo_ON
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_In_Z_Cant_ON' 的 XML 註解
        //        Arm_In_Z_Cant_ON,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_In_Z_Cant_ON' 的 XML 註解
        //        //Arm_Out_Z無法Servo_ON
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_Out_Z_Cant_ON' 的 XML 註解
        //        Arm_Out_Z_Cant_ON,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_Out_Z_Cant_ON' 的 XML 註解
        //        //左主軸無法Servo_ON
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Spindle_Cant_ON' 的 XML 註解
        //        L_Spindle_Cant_ON,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Spindle_Cant_ON' 的 XML 註解
        //        //中主軸無法Servo_ON
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Spindle_Cant_ON' 的 XML 註解
        //        M_Spindle_Cant_ON,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Spindle_Cant_ON' 的 XML 註解
        //        //右主軸無法Servo_ON
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Spindle_Cant_ON' 的 XML 註解
        //        R_Spindle_Cant_ON,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Spindle_Cant_ON' 的 XML 註解




        //        //********************************************Driver警報_10萬起*******************************

        //        //L_X_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_X_A100' 的 XML 註解
        //        L_X_A100=100256,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_X_A100' 的 XML 註解
        //        //L_Y_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Y_A100' 的 XML 註解
        //        L_Y_A100=200256,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Y_A100' 的 XML 註解
        //        //L_Z_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Z_A100' 的 XML 註解
        //        L_Z_A100=300256,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Z_A100' 的 XML 註解
        //        //M_X_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_X_A100' 的 XML 註解
        //        M_X_A100=400256,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_X_A100' 的 XML 註解
        //        //M_Y_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Y_A100' 的 XML 註解
        //        M_Y_A100=500256,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Y_A100' 的 XML 註解
        //        //M_Z_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Z_A100' 的 XML 註解
        //        M_Z_A100=600256,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Z_A100' 的 XML 註解
        //        //R_X_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_X_A100' 的 XML 註解
        //        R_X_A100=700256,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_X_A100' 的 XML 註解
        //        //R_Y_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Y_A100' 的 XML 註解
        //        R_Y_A100=800256,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Y_A100' 的 XML 註解
        //        //R_Z_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Z_A100' 的 XML 註解
        //        R_Z_A100=900256,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Z_A100' 的 XML 註解
        //        //Arm_X_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_X_A100' 的 XML 註解
        //        Arm_X_A100=1000256,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_X_A100' 的 XML 註解
        //        //Arm_IN_Z_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_IN_Z_A100' 的 XML 註解
        //        Arm_IN_Z_A100=1100256,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_IN_Z_A100' 的 XML 註解
        //        //Arm_Out_Z_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_Out_Z_A100' 的 XML 註解
        //        Arm_Out_Z_A100=1200256,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_Out_Z_A100' 的 XML 註解


        //        //L_X_再生電阻過載
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_X_A320' 的 XML 註解
        //        L_X_A320 =100800,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_X_A320' 的 XML 註解
        //        //L_Y再生電阻過載
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Y_A320' 的 XML 註解
        //        L_Y_A320 =200800,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Y_A320' 的 XML 註解
        //        //L_Z_再生電阻過載
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Z_A320' 的 XML 註解
        //        L_Z_A320 =300800,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Z_A320' 的 XML 註解
        //        //M_X_再生電阻過載
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_X_A320' 的 XML 註解
        //        M_X_A320=400800,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_X_A320' 的 XML 註解
        //        //M_Y再生電阻過載
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Y_A320' 的 XML 註解
        //        M_Y_A320 =500800,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Y_A320' 的 XML 註解
        //        //M_Z_再生電阻過載
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Z_A320' 的 XML 註解
        //        M_Z_A320=600800,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Z_A320' 的 XML 註解
        //        //R_X_再生電阻過載
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_X_A320' 的 XML 註解
        //        R_X_A320 =700800,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_X_A320' 的 XML 註解
        //        //R_Y再生電阻過載
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Y_A320' 的 XML 註解
        //        R_Y_A320 =800800,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Y_A320' 的 XML 註解
        //        //R_Z_再生電阻過載
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Z_A320' 的 XML 註解
        //        R_Z_A320 =900800,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Z_A320' 的 XML 註解
        //        //Arm_X_再生電阻過載
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_X_A320' 的 XML 註解
        //        Arm_X_A320 =1000800,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_X_A320' 的 XML 註解
        //        //Arm_IN_Z再生電阻過載
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_In_Z_A320' 的 XML 註解
        //        Arm_In_Z_A320 =1100800,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_In_Z_A320' 的 XML 註解
        //        //Arm_Out_Z_再生電阻過載
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_Out_Z_A320' 的 XML 註解
        //        Arm_Out_Z_A320 =1200800,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_Out_Z_A320' 的 XML 註解


        //        //L_X_Encoder電池警報
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_X_A830' 的 XML 註解
        //        L_X_A830 =102096,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_X_A830' 的 XML 註解
        //        //L_Y_Encoder電池警報
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Y_A830' 的 XML 註解
        //        L_Y_A830 =202096,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Y_A830' 的 XML 註解
        //        //L_Z_Encoder電池警報
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Z_A830' 的 XML 註解
        //        L_Z_A830 =302096,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Z_A830' 的 XML 註解
        //        //M_X_Encoder電池警報
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_X_A830' 的 XML 註解
        //        M_X_A830=402096,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_X_A830' 的 XML 註解
        //        //M_Y_Encoder電池警報
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Y_A830' 的 XML 註解
        //        M_Y_A830 =502096,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Y_A830' 的 XML 註解
        //        //M_Z_Encoder電池警報
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Z_A830' 的 XML 註解
        //        M_Z_A830 =602096,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Z_A830' 的 XML 註解
        //        //R_X_Encoder電池警報
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_X_A830' 的 XML 註解
        //        R_X_A830 =702096,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_X_A830' 的 XML 註解
        //        //R_Y_Encoder電池警報
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Y_A830' 的 XML 註解
        //        R_Y_A830 =802096,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Y_A830' 的 XML 註解
        //        //R_Z_Encoder電池警報
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Z_A830' 的 XML 註解
        //        R_Z_A830 =902096,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Z_A830' 的 XML 註解
        //        //Arm_X_Encoder電池警報
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_X_A830' 的 XML 註解
        //        Arm_X_A830 =1002096,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_X_A830' 的 XML 註解
        //        //Arm_IN_Z_Encoder電池警報
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_In_Z_A830' 的 XML 註解
        //        Arm_In_Z_A830 =1102096,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_In_Z_A830' 的 XML 註解
        //        //Arm_Out_Z_Encoder電池警報
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_Out_Z_A830' 的 XML 註解
        //        Arm_Out_Z_A830 =1202096,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.Arm_Out_Z_A830' 的 XML 註解


        //        //左軸相位找尋異常_3345677
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Phase_Error' 的 XML 註解
        //        L_Phase_Error =3345677,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.L_Phase_Error' 的 XML 註解
        //        //中軸相位找尋異常_3345678
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Phase_Error' 的 XML 註解
        //        M_Phase_Error =3345678,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.M_Phase_Error' 的 XML 註解
        //        //右軸相位找尋異常_3345679
        //#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Phase_Error' 的 XML 註解
        //        R_Phase_Error =3345679,
        //#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'ERROR_CODE.R_Phase_Error' 的 XML 註解

        //[ErrorCodeAttribute("未知的錯誤", null)]
        ////最大值(無錯誤碼給的值)
        //#region 機械
        ///// <summary>
        ///// 沒有錯誤代碼
        ///// </summary>
        ////[Codesys("沒有錯誤代碼")]
        //Null,
        /////// <summary>
        /////// 左進料刀庫1與設定不符
        /////// </summary>
        /////// <remarks>
        /////// 左軸入料口刀庫第 "1" 把軟體設定不相符, 請檢查左軸入料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        /////// </remarks>
        ////[Codesys("左軸入料口刀庫第 \"1\" 把軟體設定不相符", "請檢查左軸入料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        ////L_IN_DrillHome1,
        /////// <summary>
        /////// 左進料刀庫2與設定不符
        /////// </summary>
        /////// <remarks>
        /////// 左軸入料口刀庫第 "2" 把軟體設定不相符, 請檢查左軸入料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        /////// </remarks>
        ////[Codesys("左軸入料口刀庫第 \"2\" 把軟體設定不相符", "請檢查左軸入料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        ////L_IN_DrillHome2,
        /////// <summary>
        /////// 左進料刀庫3與設定不符
        /////// </summary>
        /////// <remarks>
        /////// 左軸入料口刀庫第 "3" 把軟體設定不相符, 請檢查左軸入料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        /////// </remarks>
        ////[Codesys("左軸入料口刀庫第 \"3\" 把軟體設定不相符", "請檢查左軸入料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        ////L_IN_DrillHome3,
        /////// <summary>
        /////// 左進料刀庫4與設定不符
        /////// </summary>
        /////// <remarks>
        /////// 左軸入料口刀庫第 "4" 把軟體設定不相符, 請檢查左軸入料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        /////// </remarks>
        ////[Codesys("左軸入料口刀庫第 \"4\" 把軟體設定不相符", "請檢查左軸入料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        ////L_IN_DrillHome4,
        /////// <summary>
        /////// 左出料刀庫1與設定不符
        /////// </summary>
        /////// <remarks>
        /////// 左軸出料口刀庫第 "1" 把軟體設定不相符, 請檢查左軸出料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        /////// </remarks>
        ////[Codesys("左軸出料口刀庫第 \"1\" 把軟體設定不相符", "請檢查左軸出料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        ////L_Out_DrillHome1,
        /////// <summary>
        /////// 左出料刀庫2與設定不符
        /////// </summary>
        /////// <remarks>
        /////// 左軸出料口刀庫第 "2" 把軟體設定不相符, 請檢查左軸出料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        /////// </remarks>
        ////[Codesys("左軸出料口刀庫第 \"2\" 把軟體設定不相符", "請檢查左軸出料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        ////L_Out_DrillHome2,
        /////// <summary>
        /////// 左軸出料口刀庫第 "3" 把軟體設定不相符, 請檢查左軸出料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        /////// </summary>
        ////[Codesys("左軸出料口刀庫第 \"3\" 把軟體設定不相符", "請檢查左軸出料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        ////L_Out_DrillHome3,
        /////// <summary>
        /////// 左出料刀庫4與設定不符
        /////// </summary>
        /////// <remarks>
        /////// 左軸出料口刀庫第 "4" 把軟體設定不相符, 請檢查左軸出料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        /////// </remarks>
        ////[Codesys("左軸出料口刀庫第 \"4\" 把軟體設定不相符", "請檢查左軸出料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        ////L_Out_DrillHome4,
        /////// <summary>
        /////// 右進料刀庫1與設定不符
        /////// </summary>
        /////// <remarks>
        /////// 右軸入料口刀庫第 "1" 把軟體設定不相符, 請檢查右軸入料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        /////// </remarks>
        ////[Codesys("右軸入料口刀庫第 \"1\" 把軟體設定不相符", "請檢查右軸入料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        ////R_IN_DrillHome1,
        /////// <summary>
        /////// 右進料刀庫1與設定不符
        /////// </summary>
        /////// <remarks>
        /////// 右軸入料口刀庫第 "2" 把軟體設定不相符, 請檢查右軸入料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        /////// </remarks>
        ////[Codesys("右軸入料口刀庫第 \"2\" 把軟體設定不相符", "請檢查右軸入料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        ////R_IN_DrillHome2,
        /////// <summary>
        /////// 右進料刀庫1與設定不符
        /////// </summary>
        /////// <remarks>
        /////// 右軸入料口刀庫第 "3" 把軟體設定不相符, 請檢查右軸入料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        /////// </remarks>
        ////[Codesys("右軸入料口刀庫第 \"3\" 把軟體設定不相符", "請檢查右軸入料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        ////R_IN_DrillHome3,
        /////// <summary>
        /////// 右進料刀庫1與設定不符
        /////// </summary>
        /////// <remarks>
        /////// 右軸入料口刀庫第 "4" 把軟體設定不相符, 請檢查右軸入料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        /////// </remarks>
        ////[Codesys("右軸入料口刀庫第 \"4\" 把軟體設定不相符", "請檢查右軸入料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        ////R_IN_DrillHome4,
        /////// <summary>
        /////// 右出料刀庫1與設定不符
        /////// </summary>
        /////// <remarks>
        /////// 右軸出料口刀庫第 "1" 把軟體設定不相符, 請檢查右軸出料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        /////// </remarks>
        ////[Codesys("右軸出料口刀庫第 \"1\" 把軟體設定不相符", "請檢查右軸出料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        ////R_Out_DrillHome1,
        /////// <summary>
        /////// 右出料刀庫2與設定不符
        /////// </summary>
        ///////<remarks>
        ///////右軸出料口刀庫第 "2" 把軟體設定不相符, 請檢查右軸出料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        ///////</remarks>
        ////[Codesys("右軸出料口刀庫第 \"2\" 把軟體設定不相符", "請檢查右軸出料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        ////R_Out_DrillHome2,
        /////// <summary>
        /////// 右出料刀庫3與設定不符
        /////// </summary>
        /////// <remarks>
        /////// 右軸出料口刀庫第 "3" 把軟體設定不相符, 請檢查右軸出料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        /////// </remarks>
        ////[Codesys("右軸出料口刀庫第 \"3\" 把軟體設定不相符", "請檢查右軸出料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        ////R_Out_DrillHome3,
        /////// <summary>
        /////// 右出料刀庫4與設定不符
        /////// </summary>
        /////// <remarks>
        /////// 右軸出料口刀庫第 "4" 把軟體設定不相符, 請檢查右軸出料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        /////// </remarks>
        ////[Codesys("右軸出料口刀庫第 \"4\" 把軟體設定不相符", "請檢查右軸出料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        ////R_Out_DrillHome4,
        /////// <summary>
        /////// 中軸刀庫1與設定不符
        /////// </summary>
        /////// <remarks>
        /////// 中軸口刀庫第 "1" 把軟體設定不相符, 請檢查中軸口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        /////// </remarks>
        ////[Codesys("中軸口刀庫第 \"1\" 把軟體設定不相符", "請檢查中軸口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        ////M_DrillHome1,
        /////// <summary>
        /////// 中軸刀庫2與設定不符
        /////// </summary>
        /////// <remarks>
        /////// 中軸口刀庫第 "2" 把軟體設定不相符, 請檢查中軸口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        /////// </remarks>
        ////[Codesys("中軸口刀庫第 \"2\" 把軟體設定不相符", "請檢查中軸口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        ////M_DrillHome2,
        /////// <summary>
        /////// 中軸刀庫3與設定不符
        /////// </summary>
        /////// <remarks>
        /////// 中軸口刀庫第 "3" 把軟體設定不相符, 請檢查中軸口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        /////// </remarks>
        ////[Codesys("中軸口刀庫第 \"3\" 把軟體設定不相符", "請檢查中軸口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        ////M_DrillHome3,
        /////// <summary>
        /////// 中軸刀庫4與設定不符
        /////// </summary>
        /////// <remarks>
        /////// 中軸口刀庫第 "4" 把軟體設定不相符, 請檢查中軸口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        /////// </remarks>
        ////[Codesys("中軸口刀庫第 \"4\" 把軟體設定不相符", "請檢查中軸口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        ////M_DrillHome4,
        /////// <summary>
        /////// 中軸刀庫5與設定不符
        /////// </summary>
        /////// <remarks>
        /////// 中軸口刀庫第 "5" 把軟體設定不相符, 請檢查中軸口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        /////// </remarks>
        ////[Codesys("中軸口刀庫第 \"5\" 把軟體設定不相符", "請檢查中軸口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        ////M_DrillHome5,
        /////// <summary>
        /////// EtherCAT 通訊連線錯誤
        /////// </summary>
        /////// <remarks>
        /////// EtherCAT 通訊連線錯誤, 請耐心等候系統 Reset 。如還有問題請聯絡廣達機械國際有限公司
        /////// </remarks>
        ////[Codesys("EtherCAT 通訊連線錯誤", "請耐心等候系統 Reset 。\n如還有問題請聯絡廣達機械國際有限公司")]
        ////EtherCAT,
        /////// <summary>
        /////// 軸向左與中即將碰撞
        /////// </summary>
        /////// <remarks>
        /////// 軸向左與中即將碰撞，請原點復歸，排解完畢請按下警報復位
        /////// </remarks>
        ////[Codesys("軸向左與中即將碰撞", "請原點復歸，\n排解完畢請按下警報復位")]
        ////Axis_LAndM_Touch,
        /////// <summary>
        /////// 軸向中與右即將碰撞
        /////// </summary>
        /////// <remarks>
        /////// 軸向中與右即將碰撞，請原點復歸，排解完畢請按下警報復位
        /////// </remarks>
        ////[Codesys("軸向中與右即將碰撞", "請原點復歸，\n排解完畢請按下警報復位")]
        ////Axis_MAndR_Touch,
        /////// <summary>
        /////// 軸向左與右即將碰撞
        /////// </summary>
        /////// <remarks>
        /////// 軸向左與右即將碰撞, 請原點復歸，排解完畢請按下警報復位
        /////// </remarks>
        ////[Codesys("軸向左與右即將碰撞", "請原點復歸，\n排解完畢請按下警報復位")]
        ////Axis_LAndR_Touch,
        /////// <summary>
        /////// 自動模式下不再原點
        /////// </summary>
        /////// <remarks>
        /////// 自動模式下不再原點, 請先轉到手動模式下，在原點復歸。排解完畢請按下警報復位
        /////// </remarks>
        ////[Codesys("自動模式下不再原點", "請先轉到手動模式下，在原點復歸。\n排解完畢請按下警報復位")]
        ////AutoNotOnOrigin,
        /////// <summary>
        /////// 左X負極限觸發
        /////// </summary>
        /////// <remarks>
        /////// 左X負極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        /////// </remarks>
        ////[Codesys("左X負極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        ////L_X_LimitBack,
        /////// <summary>
        /////// 左Y負極限觸發
        /////// </summary>
        /////// <remarks>
        /////// 左Y負極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        /////// </remarks>
        ////[Codesys("左Y負極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        ////L_Y_LimitBack,
        /////// <summary>
        /////// 左Y正極限觸發
        /////// </summary>
        /////// <remarks>
        /////// 左Y正極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        /////// </remarks>
        ////[Codesys("左Y正極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        ////L_Y_LimitFront,
        /////// <summary>
        /////// 左Z負極限觸發
        /////// </summary>
        /////// <remarks>
        /////// 左Z負極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        /////// </remarks>
        ////[Codesys("左Z負極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        ////L_Z_LimitBack,
        /////// <summary>
        /////// 左Z正極限觸發
        /////// </summary>
        /////// <remarks>
        /////// 左Z正極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        /////// </remarks>
        ////[Codesys("左Z正極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        ////L_Z_LimitFornt,
        /////// <summary>
        /////// 右X負極限觸發
        /////// </summary>
        /////// 右X負極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        ////[Codesys("右X負極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        ////R_X_LimitBack,
        /////// <summary>
        /////// 右X正極限觸發
        /////// </summary>
        /////// <remarks>
        /////// 右X正極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        /////// </remarks>
        ////[Codesys("右X正極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        ////R_X_LimitFornt,
        /////// <summary>
        /////// 右Y負極限觸發
        /////// </summary>
        /////// <remarks>
        /////// 右Y負極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        /////// </remarks>
        ////[Codesys("右Y負極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        ////R_Y_LimitBack,
        /////// <summary>
        /////// 右Y正極限觸發
        /////// </summary>
        /////// <remarks>
        /////// 右Y正極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        /////// </remarks>
        ////[Codesys("右Y正極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        ////R_Y_LimitFornt,
        /////// <summary>
        /////// 右Z負極限觸發
        /////// </summary>
        /////// <remarks>
        /////// 右Z負極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        /////// </remarks>
        ////[Codesys("右Z負極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        ////R_Z_LimitBack,
        /////// <summary>
        /////// 右Z正極限觸發
        /////// </summary>
        /////// <remarks>
        /////// 右Z正極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        /////// </remarks>
        ////[Codesys("右Z正極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        ////R_Z_LimitFornt,
        /////// <summary>
        /////// 上X負極限處發
        /////// </summary>
        /////// <remarks>
        /////// 上X負極限處發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        /////// </remarks>
        ////[Codesys("上X負極限處發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        ////M_X_LimitBack,
        /////// <summary>
        /////// 上X正極限觸發
        /////// </summary>
        /////// <remarks>
        /////// 上X正極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        /////// </remarks>
        ////[Codesys("上X正極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        ////M_X_LimitFornt,
        /////// <summary>
        /////// 上Y負極限觸發
        /////// </summary>
        /////// <remarks>
        /////// 上Y負極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        /////// </remarks>
        ////[Codesys("上Y負極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        ////M_Y_LimitBack,
        /////// <summary>
        /////// 上Y正極限觸發
        /////// </summary>
        /////// <remarks>
        /////// 上Y正極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司
        /////// </remarks>
        ////[Codesys("上Y正極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        ////M_Y_LimitFornt,
        /////// <summary>
        /////// 上Z負極限觸發
        /////// </summary>
        /////// <remarks>
        /////// 上Z負極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        /////// </remarks>
        ////[Codesys("上Z負極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        ////M_Z_LimitBack,
        /////// <summary>
        /////// 上Z正極限觸發
        /////// </summary>
        /////// <remarks>
        /////// 上Z正極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        /////// </remarks>
        ////[Codesys("上Z正極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        ////M_Z_LimitFornt,
        ////#endregion

        ////#region 手機 Error
        /////// <summary>
        /////// 行動裝置手動功能操作失敗
        /////// </summary>
        /////// <remarks>
        /////// 行動裝置手動功能操作失敗，請將鑰匙轉到手動，再重新操作。如還有問題請聯絡廣達機械國際有限公司
        /////// </remarks>
        ////[Codesys("行動裝置手動操作失敗，請將鑰匙轉到手動，再重新操作。\n如還有問題請聯絡廣達機械國際有限公司")]
        ////Phone_Key_Manual = 10000 ,
        /////// <summary>
        /////// 行動裝置手動功能操作失敗
        /////// </summary>
        /////// <remarks>
        /////// 行動裝置手動功能操作失敗，目前機器正在執行工作，請等待機器工作執行完畢，再重新操作此功能。如還有問題請聯絡廣達機械國際有限公司
        /////// </remarks>
        ////[Codesys("行動裝置手動功能操作失敗，目前機器正在執行工作，請等待機器工作執行完畢，再重新操作此功能。\n如還有問題請聯絡廣達機械國際有限公司")]
        ////Phone_Codesys_Run,
        /////// <summary>
        /////// PC 端拒絕手機連線
        /////// </summary>
        ////[Codesys("行動裝置手動功能操作失敗，目前 PC 選取拒絕連線。")]
        ////Phone_Refuse_Connection
        //#endregion
        /// <summary>
        /// 未知錯誤
        /// </summary>
    }
}
