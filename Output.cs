using GD_STD.Attribute;
using GD_STD.Base;
using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using static GD_STD.MemoryHelper;
namespace GD_STD
{
    /// <summary>
    /// 輸出接點
    /// </summary>
    /// <remarks>Codesys Memory 讀取</remarks>
    [DataContract]
    public struct Output : ISharedMemory/*, IOutput*/
    {
        /// <summary>
        /// 油壓啟動
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y0</para>
        /// </remarks>
        [Codesys("油壓起動")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Hydraulic_On;
        /// <summary>
        /// 排屑啟動
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y1</para>
        /// </remarks>
        [Codesys("排屑啟動")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Volume_Scrap_On;
        /// <summary>
        /// 完工出料馬達啟動
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y2</para>
        /// </remarks>
        [Codesys("完工出料馬達啟動")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Finish_Motor_On;
        /// <summary>
        /// 進料橫移正轉
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y3</para>
        /// </remarks>
        [Codesys("進料橫移正轉")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Materials_Side_Move_CCW;
        /// <summary>
        /// 進料橫移逆轉
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y4</para>
        /// </remarks>
        [Codesys("進料橫移逆轉")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Materials_Side_Move_CW;
        /// <summary>
        /// 進料橫移速度(一)
        /// <remarks>
        /// <para>地址 : Y5</para>
        /// </remarks>
        /// </summary>
        [Codesys("進料橫移速度(一)")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Materials_Side_Move_Speed_1;
        /// <summary>
        /// 進料橫移速度(二)
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y6</para>
        /// </remarks>
        [Codesys("進料橫移速度(二)")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Materials_Side_Move_Speed_2;
        /// <summary>
        /// 左主軸噴油霧
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y7</para>
        /// </remarks>
        [Codesys("主軸油霧")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool L_Spray_OilFog;
        /// <summary>
        /// 右主軸噴油霧
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y8</para>
        /// </remarks>
        [Codesys("主軸油霧")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool R_Spray_OilFog;
        /// <summary>
        /// 上主軸噴油霧
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y9</para>
        /// </remarks>
        [Codesys("主軸油霧")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool M_Spray_OilFog;
        /// <summary>
        /// 左主軸外管出風
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y10</para>
        /// </remarks>
        [Codesys("主軸外管出風")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool L_Spary_Air;
        /// <summary>
        /// 右主軸外管出風
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y11</para>
        /// </remarks>
        [Codesys("主軸外管出風")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool R_Spary_Air;
        /// <summary>
        /// 上主軸外管出風
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y12</para>
        /// </remarks>
        [Codesys("主軸外管出風")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool M_Spary_Air;
        /// <summary>
        /// 左主軸換刀出風
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y13</para>
        /// </remarks>
        [Codesys("主軸換刀出風")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool L_Spindle_Change_Air;
        /// <summary>
        /// 右主軸換刀出風
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y14</para>
        /// </remarks>
        [Codesys("主軸換刀出風")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool R_Spindle_Change_Air;
        /// <summary>
        /// 上軸換刀吹氣
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y15</para>
        /// </remarks>
        [Codesys("主軸換刀出風")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool M_Spindle_Change_Air;
        /// <summary>
        /// 左主軸鬆刀
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y16</para>
        /// </remarks>
        [Codesys("主軸鬆刀動力")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool L_Spindle_Loosen;
        /// <summary>
        /// 右主軸鬆刀
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y17</para>
        /// </remarks>
        [Codesys("主軸鬆刀動力")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool R_Spindle_Loosen;
        /// <summary>
        /// 上主軸鬆刀
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y18</para>
        /// </remarks>
        [Codesys("主軸鬆刀動力")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool M_Spindle_Loosen;
        /// <summary>
        /// 上刀庫原點
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y19</para>
        /// </remarks>
        [Codesys("刀庫原點")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool M_DirllHome_Origin;
        /// <summary>
        /// 上刀庫到換刀點
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y20</para>
        /// </remarks>
        [Codesys("刀庫氣壓缸動作")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool M_DrillHome_Change;
        /// <summary>
        /// 左刀庫復歸(進料端)
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y21</para>
        /// </remarks>
        [Codesys("刀庫復歸(進料端)")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool L_IN_DrillHome_Origin;
        /// <summary>
        /// 左刀庫推出(進料端)
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y22</para>
        /// </remarks>
        [Codesys("刀庫復歸(進料端)")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool L_IN_DrillHome_RollOut;
        /// <summary>
        /// 左刀庫復歸(出料端)
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y23</para>
        /// </remarks>
        [Codesys("刀庫復歸(出料端)")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool L_OUT_DrillHome_Origin;
        /// <summary>
        /// 左刀庫推出(出料端)
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y24</para>
        /// </remarks>
        [Codesys("左刀庫推出(出料端)")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool L_OUT_DrillHome_RollOut;
        /// <summary>
        /// 右刀庫復歸(進料端)
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y25</para>
        /// </remarks>
        [Codesys("刀庫復歸(進料端)")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool R_IN_DrillHome_Origin;
        /// <summary>
        /// 右刀庫推出(進料端)
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y26</para>
        /// </remarks>
        [Codesys("刀庫推出(進料端)")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool R_IN_DrillHome_RollOut;
        /// <summary>
        /// 右刀庫復歸(出料端)
        /// </summary>
        ///  <remarks>
        /// <para>地址 : Y27</para>
        /// </remarks>
        [Codesys("刀庫推出(進料端)")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool R_OUT_DrillHome_Origin;
        /// <summary>
        /// 右刀庫推出(出料端)
        /// </summary>
        ///  <remarks>
        /// <para>地址 : Y28</para>
        /// </remarks>
        [Codesys("刀庫推出(出料端)")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool R_OUT_DrillHome_RollOut;
        /// <summary>
        /// 左下壓復歸(進料端)
        /// </summary>
        ///  <remarks>
        /// <para>地址 : Y29</para>
        /// </remarks>
        [Codesys("壓料復歸(進料端)")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool L_IN_Clip_Down_Origin;
        /// <summary>
        /// 左下壓動作(進料端)
        /// </summary>
        ///  <remarks>
        /// <para>地址 : Y30</para>
        /// </remarks>
        [Codesys("壓料動作(進料端)")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool L_IN_Clip_Down;
        /// <summary>
        /// 左下壓復歸(出料端)
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y31</para>
        /// </remarks>
        [Codesys("壓料復歸(出料端)")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool L_OUT_Clip_Down_Origin;
        /// <summary>
        /// 左下壓動作(出料端)
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y32</para>
        /// </remarks>
        [Codesys("壓料動作(出料端)")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool L_OUT_Clip_Down;
        /// <summary>
        /// 右下壓復歸(進料端)
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y33</para>
        /// </remarks>
        [Codesys("下壓復歸(進料端)")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool R_IN_Clip_Down_Origin;
        /// <summary>
        /// 右下壓動作(進料端)
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y34</para>
        /// </remarks>
        [Codesys("壓料動作(進料端)")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool R_IN_Clip_Down;
        /// <summary>
        /// 右下壓復歸(出料端)
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y35</para>
        /// </remarks>
        [Codesys("下壓復歸(出料端)")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool R_OUT_Clip_Down_Origin;
        /// <summary> 
        /// 右下壓動作(出料端)
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y36</para>
        /// </remarks>
        [Codesys("下壓動作(出料端)")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool R_OUT_Clip_Down;
        /// <summary>
        /// 側壓復歸
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y37</para>
        /// </remarks>
        [Codesys("側壓原點")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool SIDE_Clip_Origin;
        /// <summary> 
        /// 側壓動作
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y38</para>
        /// </remarks>
        [Codesys("側壓動作")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool SIDE_Clip;
        /// <summary> 
        /// Z軸側距閘門 Open
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y39</para>
        /// </remarks>
        [Codesys("高度感測器閘門(開)")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Z_High_Sensor_Door_Open;
        /// <summary> 
        /// 強電箱門LOCK 
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y40</para>
        /// </remarks>
        [Codesys("強電箱門 Lock")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Electrical_BOX_Lock;
        /// <summary> 
        /// 外罩門(1)LOCK 
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y41</para>
        /// </remarks>
        [Codesys(" 外罩門(1) Lock ")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Case_1_Lock;
        /// <summary> 
        /// 外罩門(2)LOCK 
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y42</para>
        /// </remarks>
        [Codesys(" 外罩門(2) Lock ")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Case_2_Lock;
        /// <summary> 
        /// 外罩門(3)LOCK 
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y43</para>
        /// </remarks>
        [Codesys(" 外罩門(3) Lock ")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Case_3_Lock;
        /// <summary>
        /// 左主軸風扇開啟
        /// </summary>
        /// <remarks>
        /// 地址 : Y44
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool L_Spindle_Fan;
        /// <summary>
        /// 右主軸風扇開啟
        /// </summary>
        /// <remarks>
        /// 地址 : Y45
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool R_Spindle_Fan;
        /// <summary>
        /// 中主軸風扇開啟
        /// </summary>
        /// <remarks>
        /// 地址 : Y46
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool M_Spindle_Fan;
        /// <summary> 
        /// 面板_總電源燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y48</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_Power_Light;
        /// <summary> 
        /// 面板_油壓啟動燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y49</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_Hydraulic_Light;
        /// <summary> 
        /// 面板_加工直行燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y50</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_AutoRun_Light;
        /// <summary> 
        /// 面板_暫停燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y51</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_Pause_Light;
        /// <summary>
        /// EtherCAT 燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y52</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_EtherCAT_Light;
        /// <summary> 
        /// 面板_OverLoad燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y53</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_OverLoad_Light;
        /// <summary>
        /// 面板_Alarm燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y54</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_Alarm_Light;
        /// <summary> 
        /// 面板_全自動燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y57</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_Fully_Automatic_Light;
        /// <summary> 
        /// 面板_半自動燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y58</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_Semi_Automatic_Light;
        /// <summary>
        /// 面板_原點復歸燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y59</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_Origin_Return_Light;
        /// <summary> 
        /// 面板_減速燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y61</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_Arm_Slow_Down_Light;
        /// <summary> 
        /// 面板_加速燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y62</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_Arm_Speed_Up_Light;
        /// <summary> 
        /// 面板_中心出水燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y63</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_Effluent_Light;
        /// <summary> 
        /// 面板鬆刀燈Y64
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y64</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_Spindle_Loosen_Light;
        /// <summary> 
        /// 面板_刀庫模式燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y65</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_DrillHome_Mode_Light;
        /// <summary> 
        /// 面板_啟動手搖輪燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y66</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_HandWheel_Light;
        /// <summary> 
        /// 面板_警報復位燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y67</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_Alarm_Return_Light;
        /// <summary> 
        /// 面板_手臂選擇燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y69</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_Arm_Select_Light;
        /// <summary> 
        /// 面板_入口料架燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y70</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_IN_Rack_Light;
        /// <summary> 
        /// 面板_出口料架燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y71</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_OUT_Rack_Light;
        /// <summary> 
        /// 面板_主軸模式燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y72</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_Spindle_Mode_Light;
        /// <summary> 
        /// 面板_主軸正轉燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y73</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_Spindle_CW_Light;
        /// <summary> 
        /// 面板_主軸停止燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y74</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_Spindle_Stop_Light;
        /// <summary> 
        /// 面板_夾具側壓模式燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y78</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_Clip_Side_Mode_Light;
        /// <summary> 
        /// 面板_夾具下壓模式(搖桿)
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y79</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_Clip_Down_Mode_Light;
        /// <summary> 
        /// 面板_捲屑機動作燈
        /// </summary> 
        /// <remarks>
        /// <para>地址 : Y80</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_Volume_scrap_Light;
        /// <summary> 
        /// 警示燈_R
        /// </summary> 
        /// <remarks>
        /// <para>地址 :  Y84</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Red_light;
        /// <summary> 
        /// 警示燈_Y
        /// </summary> 
        /// <remarks>
        /// <para>地址 :  Y85</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Yellow_Light;
        /// <summary> 
        /// 警示燈_G
        /// </summary> 
        /// <remarks>
        /// <para>地址 :  Y86</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Green_Light;
        /// <summary> 
        /// 蜂鳴器
        /// </summary> 
        /// <remarks>
        /// <para>地址 :  Y87</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool BZ;
        /// <summary>
        /// 幫 PC 主機開機
        /// </summary>
        /// <remarks>
        /// <para>地址 : Y88</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Boot_Open;
        /// <summary>
        /// PC介面開啟燈(Reset燈)
        /// </summary>
        /// <remarks>
        /// 地址 :  Y89
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool PC_HMI_Open_Light;
        /// <summary>
        /// 排屑(逆轉)啟動
        /// </summary>
        /// <remarks>
        /// 地址 :  Y47
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Volume_Scrap_CCW_On;
        /// <summary>
        /// 完工出料(逆轉)
        /// </summary>
        /// <remarks>
        /// 地址 : Y96
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Finish_Motor_Back;
        /// <summary>
        /// Z軸雷射開啟(ON)
        /// </summary>
        /// <remarks>
        /// 地址 : Y97
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool High_Sensor_Laser_ON;
        /// <summary>
        /// //主軸正壓開啟(開機即啟動(吹氣確保無鐵屑)
        /// </summary>
        /// <remarks>
        /// 地址 : Y98
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool All_Spindle_Air_On;
        /// <summary>
        /// 潤滑油機啟動(ON=軌道潤滑供油)
        /// </summary>
        /// <remarks>
        /// 地址 : Y99
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Lubricating_On;
        /// <summary>
        /// 總油霧開啟(加工油霧總閥開啟,需開啟才能噴油霧)
        /// </summary>
        /// <remarks>
        /// 地址 : Y100
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Total_Spray_OilFog_On;
        /// <summary>
        /// 雷射測距(高度)歸零
        /// </summary>
        /// <remarks>
        /// 地址 : Y101
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool High_Sensor_Return_0;

        void ISharedMemory.ReadMemory()
        {
            using (var output = OutputMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(Output)), MemoryMappedFileAccess.Read))
            {
                Output result;

                output.Read<Output>(0, out result);

                this = result;
            }
        }
    }
}
