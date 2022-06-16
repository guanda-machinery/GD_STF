using System.Runtime.Serialization;

namespace GD_STD.IBase
{
    /// <summary>
    /// 輸出接點介面
    /// </summary>
    public interface IOutput
    {
        /// <summary>
        /// 油壓啟動
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y0</para>
        /// </remarks>
        [DataMember]
        bool Hydraulic_On { get; set; }
        /// <summary>
        /// 排屑啟動
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y1</para>
        /// </remarks>
        [DataMember]
        bool Volume_Scrap_On { get; set; }
        /// <summary>
        /// 完工出料馬達啟動
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y2</para>
        /// </remarks>
        [DataMember]
        bool Finish_Motor_On { get; set; }
        /// <summary>
        /// 進料橫移正轉
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y3</para>
        /// </remarks>
        [DataMember]
        bool Materials_Side_Move_CCW { get; set; }
        /// <summary>
        /// 進料橫移逆轉
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y4</para>
        /// </remarks>
        [DataMember]
        bool Materials_Side_Move_CW { get; set; }
        /// <summary>
        /// 進料橫移速度(一)
        /// <remarks>
        /// <para>地址 : Y5</para>
        /// </remarks>
        /// </summary>
        [DataMember]
        bool Materials_Side_Move_Speed_1 { get; set; }
        /// <summary>
        /// 進料橫移速度(二)
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y6</para>
        /// </remarks>
        [DataMember]
        bool Materials_Side_Move_Speed_2 { get; set; }
        /// <summary>
        /// 左主軸噴油霧Y7
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y7</para>
        /// </remarks>
        [DataMember]
        bool L_Spray_OilFog { get; set; }
        /// <summary>
        /// 右主軸噴油霧
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y8</para>
        /// </remarks>
        [DataMember]
        bool R_Spray_OilFog { get; set; }
        /// <summary>
        /// 上主軸噴油霧
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y9</para>
        /// </remarks>
        [DataMember]
        bool M_Spray_OilFog { get; set; }
        /// <summary>
        /// 左主軸外管出風
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y10</para>
        /// </remarks>
        [DataMember]
        bool L_Spary_Air { get; set; }
        /// <summary>
        /// 右主軸外管出風Y11
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y11</para>
        /// </remarks>
        [DataMember]
        bool R_Spary_Air { get; set; }
        /// <summary>
        /// 上主軸外管出風
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y12</para>
        /// </remarks>
        [DataMember]
        bool M_Spary_Air { get; set; }
        /// <summary>
        /// 左軸換刀吹氣
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y13</para>
        /// </remarks>
        [DataMember]
        bool L_Spindle_Change_Air { get; set; }
        /// <summary>
        /// 右軸換刀吹氣
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y14</para>
        /// </remarks>
        [DataMember]
        bool R_Spindle_Change_Air { get; set; }
        /// <summary>
        /// 上軸換刀吹氣
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y15</para>
        /// </remarks>
        [DataMember]
        bool M_Spindle_Change_Air { get; set; }
        /// <summary>
        /// 左主軸鬆刀
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y16</para>
        /// </remarks>
        [DataMember]
        bool L_Spindle_Loosen { get; set; }
        /// <summary>
        /// 右主軸鬆刀
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y17</para>
        /// </remarks>
        [DataMember]
        bool R_Spindle_Loosen { get; set; }
        /// <summary>
        /// 上主軸鬆刀
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y18</para>
        /// </remarks>
        [DataMember]
        bool M_Spindle_Loosen { get; set; }
        /// <summary>
        /// 上刀庫回原點
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y19</para>
        /// </remarks>
        [DataMember]
        bool M_DirllHome_Origin { get; set; }
        /// <summary>
        /// 上刀庫到換刀點
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y20</para>
        /// </remarks>
        [DataMember]
        bool M_DrillHome_Change { get; set; }
        /// <summary>
        /// 左刀庫復歸(進料端)
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y21</para>
        /// </remarks>
        [DataMember]
        bool L_IN_DrillHome_Origin { get; set; }
        /// <summary>
        /// 左刀庫推出(進料端)
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y22</para>
        /// </remarks>
        [DataMember]
        bool L_IN_DrillHome_RollOut { get; set; }
        /// <summary>
        /// 左刀庫復歸(出料端)
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y23</para>
        /// </remarks>
        [DataMember]
        bool L_OUT_DrillHome_Origin { get; set; }
        /// <summary>
        /// 左刀庫推出(出料端)
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y24</para>
        /// </remarks>
        [DataMember]
        bool L_OUT_DrillHome_RollOut { get; set; }
        /// <summary>
        /// 右刀庫復歸(進料端)
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y25</para>
        /// </remarks>
        [DataMember]
        bool R_IN_DrillHome_Origin { get; set; }
        /// <summary>
        /// 右刀庫推出(進料端)
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y26</para>
        /// </remarks>
        [DataMember]
        bool R_IN_DrillHome_RollOut { get; set; }
        /// <summary>
        /// 右刀庫復歸(出料端)
        /// </summary>
        ///  <remarks>
        /// <para>地址 : Y27</para>
        /// </remarks>
        [DataMember]
        bool R_OUT_DrillHome_Origin { get; set; }
        /// <summary>
        /// 右刀庫推出(出料端)
        /// </summary>
        ///  <remarks>
        /// <para>地址 : Y28</para>
        /// </remarks>
        [DataMember]
        bool R_OUT_DrillHome_RollOut { get; set; }
        /// <summary>
        /// 左下壓復歸(進料端)
        /// </summary>
        ///  <remarks>
        /// <para>地址 : Y29</para>
        /// </remarks>
        [DataMember]
        bool L_IN_Clip_Down_Origin { get; set; }
        /// <summary>
        /// 左下壓動作(進料端)
        /// </summary>
        ///  <remarks>
        /// <para>地址 : Y30</para>
        /// </remarks>
        [DataMember]
        bool L_IN_Clip_Down { get; set; }
        /// <summary>
        /// 左下壓復歸(出料端)
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y31</para>
        /// </remarks>
        [DataMember]
        bool L_OUT_Clip_Down_Origin { get; set; }
        /// <summary>
        /// 左下壓動作(出料端)
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y32</para>
        /// </remarks>
        [DataMember]
        bool L_OUT_Clip_Down { get; set; }
        /// <summary>
        /// 右下壓復歸(進料端)
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y33</para>
        /// </remarks>
        [DataMember]
        bool R_IN_Clip_Down_Origin { get; set; }
        /// <summary>
        /// 右下壓動作(進料端)
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y34</para>
        /// </remarks>
        [DataMember]
        bool R_IN_Clip_Down { get; set; }
        /// <summary>
        /// 右下壓復歸(出料端)
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y35</para>
        /// </remarks>
        [DataMember]
        bool R_OUT_Clip_Down_Origin { get; set; }
        /// <summary> 
        /// 右下壓動作(出料端)
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y36</para>
        /// </remarks>
        [DataMember]
        bool R_OUT_Clip_Down { get; set; }
        /// <summary>
        /// 側壓復歸
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y37</para>
        /// </remarks>
        [DataMember]
        bool SIDE_Clip_Origin { get; set; }
        /// <summary> 
        /// 側壓動作
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y38</para>
        /// </remarks>
        [DataMember]
        bool SIDE_Clip { get; set; }
        /// <summary> 
        /// Z軸側距閘門 Open
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y39</para>
        /// </remarks>
        [DataMember]
        bool Z_High_Sensor_Door_Open { get; set; }
        /// <summary> 
        /// 強電箱門LOCK 
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y40</para>
        /// </remarks>
        [DataMember]
        bool Electrical_BOX_Lock { get; set; }
        /// <summary> 
        /// 外罩門(1)LOCK 
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y41</para>
        /// </remarks>
        [DataMember]
        bool Case_1_Lock { get; set; }
        /// <summary> 
        /// 外罩門(2)LOCK 
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y42</para>
        /// </remarks>
        [DataMember]
        bool Case_2_Lock { get; set; }
        /// <summary> 
        /// 外罩門(3)LOCK 
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y43</para>
        /// </remarks>
        [DataMember]
        bool Case_3_Lock { get; set; }
        /// <summary> 
        /// 左軸Z軸剎車 Open
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y44</para>
        /// </remarks>
        [DataMember]
        bool L_Z_Break_Open { get; set; }
        /// <summary> 
        /// 右軸Z軸剎車 Open Y45
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y45</para>
        /// </remarks>
        [DataMember]
        bool R_Z_Break_Open { get; set; }
        /// <summary> 
        /// 上軸Z軸剎車OPEN
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y46</para>
        /// </remarks>
        [DataMember]
        bool M_Z_Break_Open { get; set; }
        /// <summary> 
        /// 面板_總電源燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y48</para>
        /// </remarks>
        [DataMember]
        bool HMI_Power_Light { get; set; }
        /// <summary> 
        /// 面板_油壓啟動燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y49</para>
        /// </remarks>
        [DataMember]
        bool HMI_Hydraulic_Light { get; set; }
        /// <summary> 
        /// 面板_加工直行燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y50</para>
        /// </remarks>
        [DataMember]
        bool HMI_AutoRun_Light { get; set; }
        /// <summary> 
        /// 面板_暫停燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y51</para>
        /// </remarks>
        [DataMember]
        bool HMI_Pause_Light { get; set; }
        /// <summary>
        /// EtherCAT 燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y52</para>
        /// </remarks>
        [DataMember]
        bool HMI_EtherCAT_Light { get; set; }
        /// <summary> 
        /// 面板_OverLoad燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y53</para>
        /// </remarks>
        [DataMember]
        bool HMI_OverLoad_Light { get; set; }
        /// <summary>
        /// 面板_Alarm燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y54</para>
        /// </remarks>
        [DataMember]
        bool HMI_Alarm_Light { get; set; }
        /// <summary> 
        /// 面板_全自動燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y57</para>
        /// </remarks>
        [DataMember]
        bool HMI_Fully_Automatic_Light { get; set; }
        /// <summary> 
        /// 面板_半自動燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y58</para>
        /// </remarks>
        [DataMember]
        bool HMI_Semi_Automatic_Light { get; set; }
        /// <summary>
        /// 面板_原點復歸燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y59</para>
        /// </remarks>
        [DataMember]
        bool HMI_Origin_Return_Light { get; set; }
        /// <summary> 
        /// 面板_減速燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y61</para>
        /// </remarks>
        [DataMember]
        bool HMI_Arm_Slow_Down_Light { get; set; }
        /// <summary> 
        /// 面板_加速燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y61</para>
        /// </remarks>
        [DataMember]
        bool HMI_Arm_Speed_Up_Light { get; set; }
        /// <summary> 
        /// 面板_中心出水燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y63</para>
        /// </remarks>
        [DataMember]
        bool HMI_Effluent_Light { get; set; }
        /// <summary> 
        /// 面板鬆刀燈Y64
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y64</para>
        /// </remarks>
        [DataMember]
        bool HMI_Spindle_Loosen_Light { get; set; }
        /// <summary> 
        /// 面板_刀庫模式燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y65</para>
        /// </remarks>
        [DataMember]
        bool HMI_DrillHome_Mode_Light { get; set; }
        /// <summary> 
        /// 面板_啟動手搖輪燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y66</para>
        /// </remarks>
        [DataMember]
        bool HMI_HandWheel_Light { get; set; }
        /// <summary> 
        /// 面板_警報復位燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y67</para>
        /// </remarks>
        [DataMember]
        bool HMI_Alarm_Return_Light { get; set; }
        /// <summary> 
        /// 面板_手臂選擇燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y69</para>
        /// </remarks>
        [DataMember]
        bool HMI_Arm_Select_Light { get; set; }
        /// <summary> 
        /// 面板_入口料架燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y70</para>
        /// </remarks>
        [DataMember]
        bool HMI_IN_Rack_Light { get; set; }
        /// <summary> 
        /// 面板_出口料架燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y71</para>
        /// </remarks>
        [DataMember]
        bool HMI_OUT_Rack_Light { get; set; }
        /// <summary> 
        /// 面板_主軸模式燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y72</para>
        /// </remarks>
        [DataMember]
        bool HMI_Spindle_Mode_Light { get; set; }
        /// <summary> 
        /// 面板_主軸正轉燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y73</para>
        /// </remarks>
        [DataMember]
        bool HMI_Spindle_CW_Light { get; set; }
        /// <summary> 
        /// 面板_主軸停止燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y74</para>
        /// </remarks>
        [DataMember]
        bool HMI_Spindle_Stop_Light { get; set; }
        /// <summary> 
        /// 面板_夾具側壓模式燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y78</para>
        /// </remarks>
        [DataMember]
        bool HMI_Clip_Side_Mode_Light { get; set; }
        /// <summary> 
        /// 面板_夾具下壓模式(搖桿)
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y79</para>
        /// </remarks>
        [DataMember]
        bool HMI_Clip_Down_Mode_Light { get; set; }
        /// <summary> 
        /// 面板_捲屑機動作燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y80</para>
        /// </remarks>
        [DataMember]
        bool HMI_Volume_scrap_Light { get; set; }
        /// <summary>
        /// 幫 PC 主機開機
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y88</para>
        /// </remarks>
        [DataMember]
        bool Boot_Open { get; set; }
    }
}