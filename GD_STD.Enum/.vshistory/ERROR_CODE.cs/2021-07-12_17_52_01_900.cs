using System;

namespace GD_STD.Enum
{
    /// <summary>
    /// Codesys 錯誤代碼
    /// </summary>
    public enum ERROR_CODE : UInt32
    {
        //初始(無Error)_0
        Null,
        //左進料刀庫1與設定不符_1
        L_IN_DrillHome1_Error,
        //左進料刀庫2與設定不符_2
        L_IN_DrillHome2_Error,
        //左進料刀庫3與設定不符_3
        L_IN_DrillHome3_Error,
        //左進料刀庫4與設定不符_4
        L_IN_DrillHome4_Error,
        //左出料刀庫1與設定不符_5
        L_Out_DrillHome1_Error,
        //左出料刀庫2與設定不符_6
        L_Out_DrillHome2_Error,
        //左出料刀庫3與設定不符_7
        L_Out_DrillHome3_Error,
        //左出料刀庫4與設定不符_8
        L_Out_DrillHome4_Error,
        //右進料刀庫1與設定不符_9
        R_IN_DrillHome1_Error,
        //右進料刀庫1與設定不符_10
        R_IN_DrillHome2_Error,
        //右進料刀庫1與設定不符_11
        R_IN_DrillHome3_Error,
        //右進料刀庫1與設定不符_12
        R_IN_DrillHome4_Error,
        //右出料刀庫1與設定不符_13
        R_Out_DrillHome1_Error,
        //右出料刀庫2與設定不符_14
        R_Out_DrillHome2_Error,
        //右出料刀庫3與設定不符_15
        R_Out_DrillHome3_Error,
        //右出料刀庫4與設定不符_16
        R_Out_DrillHome4_Error,
        //中軸刀庫1與設定不符_17
        M_DrillHome1_Error,
        //中軸刀庫2與設定不符_18
        M_DrillHome2_Error,
        //中軸刀庫3與設定不符_19
        M_DrillHome3_Error,
        //中軸刀庫4與設定不符_20
        M_DrillHome4_Error,
        //中軸刀庫5與設定不符_21
        M_DrillHome5_Error,
        //Ethercat未啟動_22
        Ethercat_NG,
        //自動模式下不在原點_23
        AutoNotOnOrigin,

        //料件橫移未靠邊_24
        Traverse_Error,
        //手臂夾持失敗_25
        Arm_Clamp_Failed,

        //料件寬度與資料參數不符_26
        Material_Width_Error,
        //料件高度與資料參數不符_27
        Material_Height_Error,
        //料件尺寸雷射測距異常_28
        Laser_Material_Error,

        //左軸刀常量測異常_29
        L_Drill_Length_Error,
        //中軸刀常量測異常_30
        M_Drill_Length_Error,
        //右軸刀常量測異常_31
        R_Drill_Length_Error,

        //加工陣列判斷有干涉_32
        Work_Arr_Error,
        //貫穿作業的孔徑與刀庫匹配不到_33
        M_BOX_Penetrate_NoMatch,

        //自動模式下完全無加工需求執行_34
        Auto_Mode_NoNeed_Work,

        //軸向左與中即將碰撞_10000
        Axis_LAndM_Touch = 10000,
        //軸向中與右即將碰撞_10001
        Axis_MAndR_Touch,
        //軸向左與右即將碰撞_10002
        Axis_LAndR_Touch,
        //左X負極限觸發_10003
        L_X_LimitBack,
        //左X正極限觸發_10004
        L_X_LimitFornt,
        //左Y負極限觸發_10005
        L_Y_LimitBack,
        //左Y正極限觸發_10006
        L_Y_LimitFront,
        //左Z負極限觸發_10007
        L_Z_LimitBack,
        //右X負極限觸發_10008
        R_X_LimitBack,
        //右X正極限觸發_10009
        R_X_LimitFornt,
        //右Y負極限觸發_10010
        R_Y_LimitBack,
        //右Y正極限觸發_10011
        R_Y_LimitFornt,
        //右Z負極限觸發_10012
        R_Z_LimitBack,
        //右Z正極限觸發_10013
        R_Z_LimitFornt,
        //上X負極限處發_10014
        M_X_LimitBack,
        //上X正極限觸發_10015
        M_X_LimitFornt,
        //上Y負極限觸發_10016
        M_Y_LimitBack,
        //上Y正極限觸發_10017
        M_Y_LimitFornt,
        //上Z負極限觸發_10018
        M_Z_LimitBack,
        //上Z正極限觸發_10019
        M_Z_LimitFornt,
        //手臂行程負極限觸發_10020
        Arm_X_Limit_Back,
        //手臂行程正極限觸發_10021
        Arm_X_Limit_Fornt,
        //手臂Z軸負極限觸發_10022
        Arm_Z_Limit_Back,
        //手臂Z軸正極限觸發_10023
        Arm_Z_Limit_Front,
        //有軸向的位置跑掉異常,需跳警報告知回原點自動重新定位_10024
        Axis_Position_NG,


        //左X馬達Error_Stop_10025
        L_X_Error_Stop,
        //左Y馬達Error_Stop_10026
        L_Y_Error_Stop,
        //左Z馬達Error_Stop_10027
        L_Z_Error_Stop,
        //中X馬達Error_Stop_10028
        M_X_Error_Stop,
        //中Y馬達Error_Stop_10029
        M_Y_Error_Stop,
        //中Z馬達Error_Stop_10030
        M_Z_Error_Stop,
        //右X馬達Error_Stop_10031
        R_X_Error_Stop,
        //右Y馬達Error_Stop_10032
        R_Y_Error_Stop,
        //右Z馬達Error_Stop_10033
        R_Z_Error_Stop,
        //手臂_X馬達Error_Stop_10034
        Arm_X_Error_Stop,
        //手臂_IN_Z馬達Error_Stop_10035
        Arm_In_Z_Error_Stop,
        //手臂_Out_Z馬達Error_Stop_1003
        Arm_Out_Z_Error_Stop,
        //左主軸Error_Stop_10037
        L_Spindle_Error_Stop,
        //中主軸Error_Stop_10038
        M_Spindle_Error_Stop,
        //右主軸Error_Stop_10039
        R_Spindle_Error_Stop,

        //L_X_軸向座標超過正負極限_10040
        L_X_Axis_Position_error,
        //L_Y_軸向座標超過正負極限_10041
        L_Y_Axis_Position_error,
        //L_Z_軸向座標超過正負極限_10042
        L_Z_Axis_Position_error,
        //M_X_軸向座標超過正負極限_10043
        M_X_Axis_Position_error,
        //M_Y_軸向座標超過正負極限_10044
        M_Y_Axis_Position_error,
        //M_Z_軸向座標超過正負極限_10045
        M_Z_Axis_Position_error,
        //R_X_軸向座標超過正負極限_10046
        R_X_Axis_Position_error,
        //R_Y_軸向座標超過正負極限_10047
        R_Y_Axis_Position_error,
        //R_Z_軸向座標超過正負極限_10048
        R_Z_Axis_Position_error,





        //手機操作KEY不在手動_20000
        Phone_Not_Manual = 20000,
        //目前機器正在執行工作，請等待機器工作執行完畢_20001
        Phone_Machine_Busy,
        //PC端拒絕手機APP連線控制_20002
        PC_Refuse_Phone_Connect,




        //橫移異常
        //手臂夾持異常
        //下壓複測異常
        //側壓異常
        //高度複測異常 



        //以下是原點無法復歸的異常

        //總氣壓壓力異常_30000
        AirPSI_NG = 30000,
        //總油壓壓力異常_30001
        HydraulicPSI_NG,
        //感應馬達過載_30002
        TH_RY_NG,
        //左X未復位_30003
        L_X_NG,
        //左Y未復位_30004
        L_Y_NG,
        //左Z未復位_30005
        L_Z_NG,
        //右X未復位_30006
        R_X_NG,
        //右Y未復位_30007
        R_Y_NG,
        //右Z未復位_30008
        R_Z_NG,
        //上X未復位_30009
        M_X_NG,
        //上Y未復位_30010
        M_Y_NG,
        //上Z未復位_30011
        M_Z_NG,
        //左主軸上未夾刀_30012
        L_SpindleClip_NG,
        //右主軸上未夾刀_30013
        R_SpindleClip_NG,
        //上主軸上未夾刀_30014
        M_SpindleClip_NG,
        //上刀庫未復位_30015
        M_DrillHomeOrigin_NG,
        //左進料刀庫未復位_30016
        L_IN_DrillHomeOrigin_NG,
        //左出料刀庫未復位_30017
        L_OUT_DrillHomeOrigin_NG,
        //右進料刀庫未復位_30018
        R_IN_DrillHomeOrigin_NG,
        //右出料刀庫未復位_30019
        R_OUT_DrillHomeOrigin_NG,
        //Z軸閘門未關_30020
        Z_HighSensorDoor_NG,
        //左進料下壓未復位_30021
        L_IN_Clip_Down_NG,
        //左出料下壓未復位_30022
        L_OUT_Clip_Down_NG,
        //右進料下壓未復位_30023
        R_IN_Clip_Down_NG,
        //右出料下壓未復位_30024
        R_OUT_Clip_Down_NG,
        //側壓未復位_30025
        Side_Clip_NG,
        //強電箱門未關閉_30026
        Electrical_BOX_NG,
        //外罩門一未關閉_30027
        Case_1_NG,
        //外罩門二未關閉_30028
        Case_2_NG,
        //外罩門三未關閉_30029
        Case_3_NG,





        //*******************************馬達無法Servo_On 4萬起
        //L_X無法Servo_ON
        L_X_Cant_ON = 40000,
        //L_Y無法Servo_ON
        L_Y_Cant_ON,
        //L_Z無法Servo_ON
        L_Z_Cant_ON,
        //M_X無法Servo_ON
        M_X_Cant_ON,
        //M_Y無法Servo_ON
        M_Y_Cant_ON,
        //M_Z無法Servo_ON
        M_Z_Cant_ON,
        //R_X無法Servo_ON
        R_X_Cant_ON,
        //R_Y無法Servo_ON
        R_Y_Cant_ON,
        //R_Z無法Servo_ON
        R_Z_Cant_ON,
        //Arm_X無法Servo_ON
        Arm_X_Cant_ON,
        //Arm_In_Z無法Servo_ON
        Arm_In_Z_Cant_ON,
        //Arm_Out_Z無法Servo_ON
        Arm_Out_Z_Cant_ON,
        //左主軸無法Servo_ON
        L_Spindle_Cant_ON,
        //中主軸無法Servo_ON
        M_Spindle_Cant_ON,
        //右主軸無法Servo_ON
        R_Spindle_Cant_ON,




        //********************************************Driver警報_10萬起*******************************

        //L_X_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        L_X_A100 = 100256,
        //L_Y_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        L_Y_A100 = 200256,
        //L_Z_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        L_Z_A100 = 300256,
        //M_X_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        M_X_A100 = 400256,
        //M_Y_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        M_Y_A100 = 500256,
        //M_Z_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        M_Z_A100 = 600256,
        //R_X_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        R_X_A100 = 700256,
        //R_Y_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        R_Y_A100 = 800256,
        //R_Z_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        R_Z_A100 = 900256,
        //Arm_X_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        Arm_X_A100 = 1000256,
        //Arm_IN_Z_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        Arm_IN_Z_A100 = 1100256,
        //Arm_Out_Z_過電流或散熱片過熱 IGBT 產生過熱電流或者伺服單元的散熱片過熱
        Arm_Out_Z_A100 = 1200256,


        //L_X_再生電阻過載
        L_X_A320 = 100800,
        //L_Y再生電阻過載
        L_Y_A320 = 200800,
        //L_Z_再生電阻過載
        L_Z_A320 = 300800,
        //M_X_再生電阻過載
        M_X_A320 = 400800,
        //M_Y再生電阻過載
        M_Y_A320 = 500800,
        //M_Z_再生電阻過載
        M_Z_A320 = 600800,
        //R_X_再生電阻過載
        R_X_A320 = 700800,
        //R_Y再生電阻過載
        R_Y_A320 = 800800,
        //R_Z_再生電阻過載
        R_Z_A320 = 900800,
        //Arm_X_再生電阻過載
        Arm_X_A320 = 1000800,
        //Arm_IN_Z再生電阻過載
        Arm_In_Z_A320 = 1100800,
        //Arm_Out_Z_再生電阻過載
        Arm_Out_Z_A320 = 1200800,


        //L_X_Encoder電池警報
        L_X_A830 = 102096,
        //L_Y_Encoder電池警報
        L_Y_A830 = 202096,
        //L_Z_Encoder電池警報
        L_Z_A830 = 302096,
        //M_X_Encoder電池警報
        M_X_A830 = 402096,
        //M_Y_Encoder電池警報
        M_Y_A830 = 502096,
        //M_Z_Encoder電池警報
        M_Z_A830 = 602096,
        //R_X_Encoder電池警報
        R_X_A830 = 702096,
        //R_Y_Encoder電池警報
        R_Y_A830 = 802096,
        //R_Z_Encoder電池警報
        R_Z_A830 = 902096,
        //Arm_X_Encoder電池警報
        Arm_X_A830 = 1002096,
        //Arm_IN_Z_Encoder電池警報
        Arm_In_Z_A830 = 1102096,
        //Arm_Out_Z_Encoder電池警報
        Arm_Out_Z_A830 = 1202096,








        //最大值(無錯誤碼給的值)
        #region 機械
        ///// <summary>
        ///// 沒有錯誤代碼
        ///// </summary>
        //[Codesys("沒有錯誤代碼")]
        //Null,
        ///// <summary>
        ///// 左進料刀庫1與設定不符
        ///// </summary>
        ///// <remarks>
        ///// 左軸入料口刀庫第 "1" 把軟體設定不相符, 請檢查左軸入料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        ///// </remarks>
        //[Codesys("左軸入料口刀庫第 \"1\" 把軟體設定不相符", "請檢查左軸入料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        //L_IN_DrillHome1,
        ///// <summary>
        ///// 左進料刀庫2與設定不符
        ///// </summary>
        ///// <remarks>
        ///// 左軸入料口刀庫第 "2" 把軟體設定不相符, 請檢查左軸入料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        ///// </remarks>
        //[Codesys("左軸入料口刀庫第 \"2\" 把軟體設定不相符", "請檢查左軸入料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        //L_IN_DrillHome2,
        ///// <summary>
        ///// 左進料刀庫3與設定不符
        ///// </summary>
        ///// <remarks>
        ///// 左軸入料口刀庫第 "3" 把軟體設定不相符, 請檢查左軸入料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        ///// </remarks>
        //[Codesys("左軸入料口刀庫第 \"3\" 把軟體設定不相符", "請檢查左軸入料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        //L_IN_DrillHome3,
        ///// <summary>
        ///// 左進料刀庫4與設定不符
        ///// </summary>
        ///// <remarks>
        ///// 左軸入料口刀庫第 "4" 把軟體設定不相符, 請檢查左軸入料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        ///// </remarks>
        //[Codesys("左軸入料口刀庫第 \"4\" 把軟體設定不相符", "請檢查左軸入料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        //L_IN_DrillHome4,
        ///// <summary>
        ///// 左出料刀庫1與設定不符
        ///// </summary>
        ///// <remarks>
        ///// 左軸出料口刀庫第 "1" 把軟體設定不相符, 請檢查左軸出料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        ///// </remarks>
        //[Codesys("左軸出料口刀庫第 \"1\" 把軟體設定不相符", "請檢查左軸出料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        //L_Out_DrillHome1,
        ///// <summary>
        ///// 左出料刀庫2與設定不符
        ///// </summary>
        ///// <remarks>
        ///// 左軸出料口刀庫第 "2" 把軟體設定不相符, 請檢查左軸出料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        ///// </remarks>
        //[Codesys("左軸出料口刀庫第 \"2\" 把軟體設定不相符", "請檢查左軸出料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        //L_Out_DrillHome2,
        ///// <summary>
        ///// 左軸出料口刀庫第 "3" 把軟體設定不相符, 請檢查左軸出料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        ///// </summary>
        //[Codesys("左軸出料口刀庫第 \"3\" 把軟體設定不相符", "請檢查左軸出料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        //L_Out_DrillHome3,
        ///// <summary>
        ///// 左出料刀庫4與設定不符
        ///// </summary>
        ///// <remarks>
        ///// 左軸出料口刀庫第 "4" 把軟體設定不相符, 請檢查左軸出料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        ///// </remarks>
        //[Codesys("左軸出料口刀庫第 \"4\" 把軟體設定不相符", "請檢查左軸出料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        //L_Out_DrillHome4,
        ///// <summary>
        ///// 右進料刀庫1與設定不符
        ///// </summary>
        ///// <remarks>
        ///// 右軸入料口刀庫第 "1" 把軟體設定不相符, 請檢查右軸入料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        ///// </remarks>
        //[Codesys("右軸入料口刀庫第 \"1\" 把軟體設定不相符", "請檢查右軸入料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        //R_IN_DrillHome1,
        ///// <summary>
        ///// 右進料刀庫1與設定不符
        ///// </summary>
        ///// <remarks>
        ///// 右軸入料口刀庫第 "2" 把軟體設定不相符, 請檢查右軸入料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        ///// </remarks>
        //[Codesys("右軸入料口刀庫第 \"2\" 把軟體設定不相符", "請檢查右軸入料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        //R_IN_DrillHome2,
        ///// <summary>
        ///// 右進料刀庫1與設定不符
        ///// </summary>
        ///// <remarks>
        ///// 右軸入料口刀庫第 "3" 把軟體設定不相符, 請檢查右軸入料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        ///// </remarks>
        //[Codesys("右軸入料口刀庫第 \"3\" 把軟體設定不相符", "請檢查右軸入料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        //R_IN_DrillHome3,
        ///// <summary>
        ///// 右進料刀庫1與設定不符
        ///// </summary>
        ///// <remarks>
        ///// 右軸入料口刀庫第 "4" 把軟體設定不相符, 請檢查右軸入料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        ///// </remarks>
        //[Codesys("右軸入料口刀庫第 \"4\" 把軟體設定不相符", "請檢查右軸入料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        //R_IN_DrillHome4,
        ///// <summary>
        ///// 右出料刀庫1與設定不符
        ///// </summary>
        ///// <remarks>
        ///// 右軸出料口刀庫第 "1" 把軟體設定不相符, 請檢查右軸出料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        ///// </remarks>
        //[Codesys("右軸出料口刀庫第 \"1\" 把軟體設定不相符", "請檢查右軸出料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        //R_Out_DrillHome1,
        ///// <summary>
        ///// 右出料刀庫2與設定不符
        ///// </summary>
        /////<remarks>
        /////右軸出料口刀庫第 "2" 把軟體設定不相符, 請檢查右軸出料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        /////</remarks>
        //[Codesys("右軸出料口刀庫第 \"2\" 把軟體設定不相符", "請檢查右軸出料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        //R_Out_DrillHome2,
        ///// <summary>
        ///// 右出料刀庫3與設定不符
        ///// </summary>
        ///// <remarks>
        ///// 右軸出料口刀庫第 "3" 把軟體設定不相符, 請檢查右軸出料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        ///// </remarks>
        //[Codesys("右軸出料口刀庫第 \"3\" 把軟體設定不相符", "請檢查右軸出料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        //R_Out_DrillHome3,
        ///// <summary>
        ///// 右出料刀庫4與設定不符
        ///// </summary>
        ///// <remarks>
        ///// 右軸出料口刀庫第 "4" 把軟體設定不相符, 請檢查右軸出料口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        ///// </remarks>
        //[Codesys("右軸出料口刀庫第 \"4\" 把軟體設定不相符", "請檢查右軸出料口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        //R_Out_DrillHome4,
        ///// <summary>
        ///// 中軸刀庫1與設定不符
        ///// </summary>
        ///// <remarks>
        ///// 中軸口刀庫第 "1" 把軟體設定不相符, 請檢查中軸口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        ///// </remarks>
        //[Codesys("中軸口刀庫第 \"1\" 把軟體設定不相符", "請檢查中軸口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        //M_DrillHome1,
        ///// <summary>
        ///// 中軸刀庫2與設定不符
        ///// </summary>
        ///// <remarks>
        ///// 中軸口刀庫第 "2" 把軟體設定不相符, 請檢查中軸口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        ///// </remarks>
        //[Codesys("中軸口刀庫第 \"2\" 把軟體設定不相符", "請檢查中軸口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        //M_DrillHome2,
        ///// <summary>
        ///// 中軸刀庫3與設定不符
        ///// </summary>
        ///// <remarks>
        ///// 中軸口刀庫第 "3" 把軟體設定不相符, 請檢查中軸口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        ///// </remarks>
        //[Codesys("中軸口刀庫第 \"3\" 把軟體設定不相符", "請檢查中軸口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        //M_DrillHome3,
        ///// <summary>
        ///// 中軸刀庫4與設定不符
        ///// </summary>
        ///// <remarks>
        ///// 中軸口刀庫第 "4" 把軟體設定不相符, 請檢查中軸口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        ///// </remarks>
        //[Codesys("中軸口刀庫第 \"4\" 把軟體設定不相符", "請檢查中軸口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        //M_DrillHome4,
        ///// <summary>
        ///// 中軸刀庫5與設定不符
        ///// </summary>
        ///// <remarks>
        ///// 中軸口刀庫第 "5" 把軟體設定不相符, 請檢查中軸口刀庫，是否與應用程式之設定相符。排解完畢請按下警報復位
        ///// </remarks>
        //[Codesys("中軸口刀庫第 \"5\" 把軟體設定不相符", "請檢查中軸口刀庫，是否與應用程式之設定相符。\n排解完畢請按下警報復位")]
        //M_DrillHome5,
        ///// <summary>
        ///// EtherCAT 通訊連線錯誤
        ///// </summary>
        ///// <remarks>
        ///// EtherCAT 通訊連線錯誤, 請耐心等候系統 Reset 。如還有問題請聯絡廣達機械國際有限公司
        ///// </remarks>
        //[Codesys("EtherCAT 通訊連線錯誤", "請耐心等候系統 Reset 。\n如還有問題請聯絡廣達機械國際有限公司")]
        //EtherCAT,
        ///// <summary>
        ///// 軸向左與中即將碰撞
        ///// </summary>
        ///// <remarks>
        ///// 軸向左與中即將碰撞，請原點復歸，排解完畢請按下警報復位
        ///// </remarks>
        //[Codesys("軸向左與中即將碰撞", "請原點復歸，\n排解完畢請按下警報復位")]
        //Axis_LAndM_Touch,
        ///// <summary>
        ///// 軸向中與右即將碰撞
        ///// </summary>
        ///// <remarks>
        ///// 軸向中與右即將碰撞，請原點復歸，排解完畢請按下警報復位
        ///// </remarks>
        //[Codesys("軸向中與右即將碰撞", "請原點復歸，\n排解完畢請按下警報復位")]
        //Axis_MAndR_Touch,
        ///// <summary>
        ///// 軸向左與右即將碰撞
        ///// </summary>
        ///// <remarks>
        ///// 軸向左與右即將碰撞, 請原點復歸，排解完畢請按下警報復位
        ///// </remarks>
        //[Codesys("軸向左與右即將碰撞", "請原點復歸，\n排解完畢請按下警報復位")]
        //Axis_LAndR_Touch,
        ///// <summary>
        ///// 自動模式下不再原點
        ///// </summary>
        ///// <remarks>
        ///// 自動模式下不再原點, 請先轉到手動模式下，在原點復歸。排解完畢請按下警報復位
        ///// </remarks>
        //[Codesys("自動模式下不再原點", "請先轉到手動模式下，在原點復歸。\n排解完畢請按下警報復位")]
        //AutoNotOnOrigin,
        ///// <summary>
        ///// 左X負極限觸發
        ///// </summary>
        ///// <remarks>
        ///// 左X負極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        ///// </remarks>
        //[Codesys("左X負極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        //L_X_LimitBack,
        ///// <summary>
        ///// 左Y負極限觸發
        ///// </summary>
        ///// <remarks>
        ///// 左Y負極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        ///// </remarks>
        //[Codesys("左Y負極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        //L_Y_LimitBack,
        ///// <summary>
        ///// 左Y正極限觸發
        ///// </summary>
        ///// <remarks>
        ///// 左Y正極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        ///// </remarks>
        //[Codesys("左Y正極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        //L_Y_LimitFront,
        ///// <summary>
        ///// 左Z負極限觸發
        ///// </summary>
        ///// <remarks>
        ///// 左Z負極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        ///// </remarks>
        //[Codesys("左Z負極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        //L_Z_LimitBack,
        ///// <summary>
        ///// 左Z正極限觸發
        ///// </summary>
        ///// <remarks>
        ///// 左Z正極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        ///// </remarks>
        //[Codesys("左Z正極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        //L_Z_LimitFornt,
        ///// <summary>
        ///// 右X負極限觸發
        ///// </summary>
        ///// 右X負極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        //[Codesys("右X負極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        //R_X_LimitBack,
        ///// <summary>
        ///// 右X正極限觸發
        ///// </summary>
        ///// <remarks>
        ///// 右X正極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        ///// </remarks>
        //[Codesys("右X正極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        //R_X_LimitFornt,
        ///// <summary>
        ///// 右Y負極限觸發
        ///// </summary>
        ///// <remarks>
        ///// 右Y負極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        ///// </remarks>
        //[Codesys("右Y負極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        //R_Y_LimitBack,
        ///// <summary>
        ///// 右Y正極限觸發
        ///// </summary>
        ///// <remarks>
        ///// 右Y正極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        ///// </remarks>
        //[Codesys("右Y正極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        //R_Y_LimitFornt,
        ///// <summary>
        ///// 右Z負極限觸發
        ///// </summary>
        ///// <remarks>
        ///// 右Z負極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        ///// </remarks>
        //[Codesys("右Z負極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        //R_Z_LimitBack,
        ///// <summary>
        ///// 右Z正極限觸發
        ///// </summary>
        ///// <remarks>
        ///// 右Z正極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        ///// </remarks>
        //[Codesys("右Z正極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        //R_Z_LimitFornt,
        ///// <summary>
        ///// 上X負極限處發
        ///// </summary>
        ///// <remarks>
        ///// 上X負極限處發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        ///// </remarks>
        //[Codesys("上X負極限處發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        //M_X_LimitBack,
        ///// <summary>
        ///// 上X正極限觸發
        ///// </summary>
        ///// <remarks>
        ///// 上X正極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        ///// </remarks>
        //[Codesys("上X正極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        //M_X_LimitFornt,
        ///// <summary>
        ///// 上Y負極限觸發
        ///// </summary>
        ///// <remarks>
        ///// 上Y負極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        ///// </remarks>
        //[Codesys("上Y負極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        //M_Y_LimitBack,
        ///// <summary>
        ///// 上Y正極限觸發
        ///// </summary>
        ///// <remarks>
        ///// 上Y正極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司
        ///// </remarks>
        //[Codesys("上Y正極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        //M_Y_LimitFornt,
        ///// <summary>
        ///// 上Z負極限觸發
        ///// </summary>
        ///// <remarks>
        ///// 上Z負極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        ///// </remarks>
        //[Codesys("上Z負極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        //M_Z_LimitBack,
        ///// <summary>
        ///// 上Z正極限觸發
        ///// </summary>
        ///// <remarks>
        ///// 上Z正極限觸發，請原點復歸，排解完畢請按下警報復位。如還有問題請聯絡廣達機械國際有限公司
        ///// </remarks>
        //[Codesys("上Z正極限觸發，請原點復歸，\n排解完畢請按下警報復位。\n如還有問題請聯絡廣達機械國際有限公司")]
        //M_Z_LimitFornt,
        //#endregion

        //#region 手機 Error
        ///// <summary>
        ///// 行動裝置手動功能操作失敗
        ///// </summary>
        ///// <remarks>
        ///// 行動裝置手動功能操作失敗，請將鑰匙轉到手動，再重新操作。如還有問題請聯絡廣達機械國際有限公司
        ///// </remarks>
        //[Codesys("行動裝置手動操作失敗，請將鑰匙轉到手動，再重新操作。\n如還有問題請聯絡廣達機械國際有限公司")]
        //Phone_Key_Manual = 10000 ,
        ///// <summary>
        ///// 行動裝置手動功能操作失敗
        ///// </summary>
        ///// <remarks>
        ///// 行動裝置手動功能操作失敗，目前機器正在執行工作，請等待機器工作執行完畢，再重新操作此功能。如還有問題請聯絡廣達機械國際有限公司
        ///// </remarks>
        //[Codesys("行動裝置手動功能操作失敗，目前機器正在執行工作，請等待機器工作執行完畢，再重新操作此功能。\n如還有問題請聯絡廣達機械國際有限公司")]
        //Phone_Codesys_Run,
        ///// <summary>
        ///// PC 端拒絕手機連線
        ///// </summary>
        //[Codesys("行動裝置手動功能操作失敗，目前 PC 選取拒絕連線。")]
        //Phone_Refuse_Connection
        #endregion
        /// <summary>
        /// 未知錯誤
        /// </summary>
        Unknown = UInt32.MaxValue,
    }
}
