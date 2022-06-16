using System.Runtime.Serialization;

namespace GD_STD.Base
{
    /// <summary>
    /// 輸入接點介面
    /// </summary>
    public interface IInput
    {
        /// <summary>
        /// 急停
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X0</para>
        /// </remarks>
        [DataMember]
        bool EMS { get; set; }
        /// <summary>
        /// 油壓,排屑,出料過載
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X1</para>
        /// </remarks>
        [DataMember]
        bool TH_RY { get; set; }
        /// <summary>
        /// 氣壓壓力檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X2</para>
        /// </remarks>
        [DataMember]
        bool AirPSI { get; set; }
        /// <summary>
        /// 油壓壓力檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X3</para>
        /// </remarks>
        [DataMember]
        bool HydraulicPSI { get; set; }
        /// <summary>
        /// 左X原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X4</para>
        /// </remarks>
        [DataMember]
        bool L_X_Origin { get; set; }
        /// <summary>
        /// 左X負極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X5</para>
        /// </remarks>
        [DataMember]
        bool L_X_LimitBack { get; set; }
        /// <summary>
        /// 左Y原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X6</para>
        /// </remarks>
        [DataMember]
        bool L_Z_Origin { get; set; }
        /// <summary>
        /// 左Y負極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X7</para>
        /// </remarks>
        [DataMember]
        bool L_Z_LimitBack { get; set; }
        /// <summary>
        /// 左Y正極限(B)X8
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X8</para>
        /// </remarks>
        [DataMember]
        bool L_Z_LimitFront { get; set; }
        /// <summary>
        /// 左Z原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X9</para>
        /// </remarks>
        [DataMember]
        bool L_Y_Origin { get; set; }
        /// <summary>
        /// 左Z負極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X10</para>
        /// </remarks> 
        [DataMember]
        bool L_Y_LimitBack { get; set; }
        /// <summary>
        /// 左Z正極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X1</para>
        /// </remarks>
        [DataMember]
        bool L_Y_LimitFornt { get; set; }
        /// <summary>
        /// 右X原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X12</para>
        /// </remarks>
        [DataMember]
        bool R_X_Origin { get; set; }
        /// <summary>
        /// 右X負極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X13</para>
        /// </remarks>
        [DataMember]
        bool R_X_LimitBack { get; set; }
        /// <summary>
        /// 右X正極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X14</para>
        /// </remarks>
        [DataMember]
        bool R_X_LimitFornt { get; set; }
        /// <summary>
        /// 右Y原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X15</para>
        /// </remarks>
        [DataMember]
        bool R_Z_Origin { get; set; }
        /// <summary>
        /// 右Y負極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X16</para>
        /// </remarks>
        [DataMember]
        bool R_Z_LimitBack { get; set; }
        /// <summary>
        /// 右Y正極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X17</para>
        /// </remarks>
        [DataMember]
        bool R_Z_LimitFornt { get; set; }
        /// <summary>
        /// 右Z原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X18</para>
        /// </remarks>
        [DataMember]
        bool R_Y_Origin { get; set; }
        /// <summary>
        /// 右Z負極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X19</para>
        /// </remarks>
        [DataMember]
        bool R_Y_LimitBack { get; set; }
        /// <summary>
        /// 右Z正極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X20</para>
        /// </remarks>
        [DataMember]
        bool R_Y_LimitFornt { get; set; }
        /// <summary>
        /// 上X原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X21</para>
        /// </remarks>
        [DataMember]
        bool M_X_Origin { get; set; }
        /// <summary>
        /// 上X負極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X22</para>
        /// </remarks>
        [DataMember]
        bool M_X_LimitBack { get; set; }
        /// <summary>
        /// 上X正極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X23</para>
        /// </remarks>
        [DataMember]
        bool M_X_LimitFornt { get; set; }
        /// <summary>
        /// 上Y原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X24</para>
        /// </remarks>
        [DataMember]
        bool M_Y_Origin { get; set; }
        /// <summary>
        /// 上Y負極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X25</para>
        /// </remarks>
        [DataMember]
        bool M_Y_LimitBack { get; set; }
        /// <summary>
        /// 上Y正極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X26</para>
        /// </remarks>
        [DataMember]
        bool M_Y_LimitFornt { get; set; }
        /// <summary>
        /// 上Z原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X27</para>
        /// </remarks>
        [DataMember]
        bool M_Z_Origin { get; set; }
        /// <summary>
        /// 上Z負極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X28</para>
        /// </remarks>
        [DataMember]
        bool M_Z_LimitBack { get; set; }
        /// <summary>
        /// 上Z正極限(B)X29
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X29</para>
        /// </remarks>
        [DataMember]
        bool M_Z_LimitFornt { get; set; }
        /// <summary>
        /// 左X and上X碰撞
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X30</para>
        /// </remarks>
        [DataMember]
        bool LX_MX_Touch { get; set; }
        /// <summary>
        /// 上X and 右X碰撞
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X31</para>
        /// </remarks>
        [DataMember]
        bool MX_RX_Touch { get; set; }
        /// <summary>
        /// 左主軸相位原點X32
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X32</para>
        /// </remarks>
        [DataMember]
        bool L_Spindle_Origin { get; set; }
        /// <summary>
        /// 右主軸項為原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X33</para>
        /// </remarks>
        [DataMember]
        bool R_Spindle_Origin { get; set; }
        /// <summary>
        /// 左主軸夾刀
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X34</para>
        /// </remarks>
        [DataMember]
        bool L_SpindleClip { get; set; }
        /// <summary>
        /// 左主軸鬆刀
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X35</para>
        /// </remarks>
        [DataMember]
        bool L_SpindleLoosen { get; set; }
        /// <summary>
        /// 右主軸夾刀
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X36</para>
        /// </remarks>
        [DataMember]
        bool R_SpindleClip { get; set; }
        /// <summary>
        /// 右主軸鬆刀X37
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X37</para>
        /// </remarks>
        [DataMember]
        bool R_SpindleLoosen { get; set; }
        /// <summary>
        /// 上主軸夾刀
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X38</para>
        /// </remarks>
        [DataMember]
        bool M_SpindleClip { get; set; }
        /// <summary>
        /// 上主軸鬆刀
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X39</para>
        /// </remarks>
        [DataMember]
        bool M_SpindleLoosen { get; set; }
        /// <summary>
        /// 上軸刀庫原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X40</para>
        /// </remarks>
        [DataMember]
        bool M_DrillHomeOrigin { get; set; }
        /// <summary>
        /// 上軸刀庫換刀點
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X41</para>
        /// </remarks>
        [DataMember]
        bool M_DrillHomeChange { get; set; }
        /// <summary>
        /// 左刀庫(進料邊)原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X42</para>
        /// </remarks>
        [DataMember]
        bool L_IN_DrillHomeOrigin { get; set; }
        /// <summary>
        /// 左刀庫(進料邊)換刀點
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X43</para>
        /// </remarks>
        [DataMember]
        bool L_IN_DrillHomeChange { get; set; }
        /// <summary>
        /// 左刀庫(出料邊)原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X44</para>
        /// </remarks>
        [DataMember]
        bool L_OUT_DrillHomeOrigin { get; set; }
        /// <summary>
        /// 左刀庫(出料邊)換刀點
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X45</para>
        /// </remarks>
        [DataMember]
        bool L_OUT_DrillHomeChange { get; set; }
        /// <summary>
        /// 右刀庫(進料邊)原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X46</para>
        /// </remarks>
        [DataMember]
        bool R_IN_DrillHomeOrigin { get; set; }
        /// <summary>
        /// 右刀庫(進料邊)換刀點
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X47</para>
        /// </remarks>
        [DataMember]
        bool R_IN_DrillHomeChange { get; set; }
        /// <summary>
        /// 右刀庫(出料邊)原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X48</para>
        /// </remarks>
        [DataMember]
        bool R_OUT_DrillHomeOrigin { get; set; }
        /// <summary>
        /// 右刀庫(出料邊)換刀點
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X49</para>
        /// </remarks>
        [DataMember]
        bool R_OUT_DrillHomeChange { get; set; }
        /// <summary>
        /// Z軸側距閘門_關
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X50</para>
        /// </remarks>
        [DataMember]
        bool Z_HighSensorDoor_Close { get; set; }
        /// <summary>
        /// Z軸側距閘門_開
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X51</para>
        /// </remarks>
        [DataMember]
        bool Z_HighSensorDoor_Open { get; set; }
        /// <summary>
        /// 左X潤滑分路檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X52</para>
        /// </remarks>
        [DataMember]
        bool L_X_lubricating { get; set; }
        /// <summary>
        /// 左Y潤滑分路檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X53</para>
        /// </remarks>
        [DataMember]
        bool L_Y_lubricating { get; set; }
        /// <summary>
        /// 左Z潤滑分路檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X54</para>
        /// </remarks>
        [DataMember]
        bool L_Z_lubricating { get; set; }
        /// <summary>
        /// 右X潤滑分路檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X55</para>
        /// </remarks>
        [DataMember]
        bool R_X_lubricating { get; set; }
        /// <summary>
        /// 右Y潤滑分路檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X56</para>
        /// </remarks>
        [DataMember]
        bool R_Y_lubricating { get; set; }
        /// <summary>
        /// 右Z潤滑分路檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X57</para>
        /// </remarks>
        [DataMember]
        bool R_Z_lubricating { get; set; }
        /// <summary>
        /// 上X潤滑分路檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X58</para>
        /// </remarks>
        [DataMember]
        bool M_X_lubricating { get; set; }
        /// <summary>
        /// 上Y潤滑分路檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X59</para>
        /// </remarks>
        [DataMember]
        bool M_Y_lubricating { get; set; }
        /// <summary>
        /// 上Z潤滑分路檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X60</para>
        /// </remarks>
        [DataMember]
        bool M_Z_lubricating { get; set; }
        /// <summary>
        /// 左壓料電阻尺原點(進料端)
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X61</para>
        /// </remarks>
        [DataMember]
        bool L_IN_Clip_Down_Origin { get; set; }
        /// <summary>
        /// 左壓料電阻尺原點(出料端)
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X62</para>
        /// </remarks>
        [DataMember]
        bool L_OUT_Clip_Down_Origin { get; set; }
        /// <summary>
        /// 右壓料電阻尺原點(進料端)
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X63</para>
        /// </remarks>
        [DataMember]
        bool R_IN_Clip_Down_Origin { get; set; }
        /// <summary>
        /// 右壓料電阻尺原點(出料端)
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X64</para>
        /// </remarks>
        [DataMember]
        bool R_OUT_Clip_Down_Origin { get; set; }
        /// <summary>
        /// 側壓料電阻尺原點(進料端)
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X65</para>
        /// </remarks>
        [DataMember]
        bool Side_IN_Clip_Origin { get; set; }
        /// <summary>
        /// 左軸側刀長感測
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X67</para>
        /// </remarks>
        [DataMember]
        bool L_DrillLength { get; set; }
        /// <summary>
        /// 右軸側刀長感測(B)X68
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X68</para>
        /// </remarks>
        [DataMember]
        bool R_DrillLength { get; set; }
        /// <summary>
        /// 上軸側刀長感測
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X69</para>
        /// </remarks>
        [DataMember]
        bool M_DrillLength { get; set; }
        /// <summary>
        /// 左刀庫進料端-刀具1 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X70</para>
        /// </remarks>
        [DataMember]
        bool L_IN_DrillHome_1 { get; set; }
        /// <summary>
        /// 左刀庫進料端-刀具2 X71
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X71</para>
        /// </remarks>
        [DataMember]
        bool L_IN_DrillHome_2 { get; set; }
        /// <summary>
        /// 左刀庫進料端-刀具3 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X72</para>
        /// </remarks>
        [DataMember]
        bool L_IN_DrillHome_3 { get; set; }
        /// <summary>
        /// 左刀庫進料端-刀具4 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X73</para>
        /// </remarks>
        [DataMember]
        bool L_IN_DrillHome_4 { get; set; }
        /// <summary>
        /// 左刀庫出料端-刀具1 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X74</para>
        /// </remarks>
        [DataMember]
        bool L_OUT_DrillHome_1 { get; set; }
        /// <summary>
        /// 左刀庫出料端-刀具2 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X75</para>
        /// </remarks>
        [DataMember]
        bool L_OUT_DrillHome_2 { get; set; }
        /// <summary>
        /// 左刀庫出料端-刀具3 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X76</para>
        /// </remarks>
        [DataMember]
        bool L_OUT_DrillHome_3 { get; set; }
        /// <summary>
        /// 左刀庫出料端-刀具4 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X77</para>
        /// </remarks>
        [DataMember]
        bool L_OUT_DrillHome_4 { get; set; }
        /// <summary>
        /// 右刀庫進料端-刀具1 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X78</para>
        /// </remarks>
        [DataMember]
        bool R_IN_DrillHome_1 { get; set; }
        /// <summary>
        /// 右刀庫進料端-刀具2 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X79</para>
        /// </remarks>
        [DataMember]
        bool R_IN_DrillHome_2 { get; set; }
        /// <summary>
        /// 右刀庫進料端-刀具3 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X80</para>
        /// </remarks>
        [DataMember]
        bool R_IN_DrillHome_3 { get; set; }
        /// <summary>
        /// 右刀庫進料端-刀具4
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X81</para>
        /// </remarks>
        [DataMember]
        bool R_IN_DrillHome_4 { get; set; }
        /// <summary>
        /// 右刀庫出料端-刀具1 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X82</para>
        /// </remarks>
        [DataMember]
        bool R_OUT_DrillHome_1 { get; set; }
        /// <summary>
        /// 右刀庫出料端-刀具2 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X83</para>
        /// </remarks>
        [DataMember]
        bool R_OUT_DrillHome_2 { get; set; }
        /// <summary>
        /// 右刀庫出料端-刀具3 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X84</para>
        /// </remarks>
        [DataMember]
        bool R_OUT_DrillHome_3 { get; set; }
        /// <summary>
        /// 右刀庫出料端-刀具4 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X85</para>
        /// </remarks>
        [DataMember]
        bool R_OUT_DrillHome_4 { get; set; }
        /// <summary>
        /// 上刀庫-刀具1 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X86</para>
        /// </remarks>
        [DataMember]
        bool M_DrillHome_1 { get; set; }
        /// <summary>
        /// 上刀庫-刀具2 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X87</para>
        /// </remarks>
        [DataMember]
        bool M_DrillHome_2 { get; set; }
        /// <summary>
        /// 上刀庫-刀具3 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X88</para>
        /// </remarks>
        [DataMember]
        bool M_DrillHome_3 { get; set; }
        /// <summary>
        /// 上刀庫-刀具4 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X89</para>
        /// </remarks>
        [DataMember]
        bool M_DrillHome_4 { get; set; }
        /// <summary>
        /// 上刀庫-刀具5 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X90</para>
        /// </remarks>
        [DataMember]
        bool M_DrillHome_5 { get; set; }
        /// <summary>
        /// 手臂送料減速點檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X91</para>
        /// </remarks>
        [DataMember]
        bool Feed_Slow_Down_Point { get; set; }
        /// <summary>
        /// 手臂送料原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X92</para>
        /// </remarks>
        [DataMember]
        bool Feed_Origin { get; set; }
        /// <summary>
        /// 出料料件口檢測
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X93</para>
        /// </remarks>
        [DataMember]
        bool FinishOut { get; set; }
        /// <summary>
        /// 潤滑油液位檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X94</para>
        /// </remarks>
        [DataMember]
        bool Total_Lubricatig { get; set; }
        /// <summary>
        /// 強電箱關門檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X95</para>
        /// </remarks>
        [DataMember]
        bool Electrical_BOX_Colse { get; set; }
        /// <summary>
        /// 強電箱LOCK檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X96</para>
        /// </remarks>
        [DataMember]
        bool Electrical_BOX_Lock { get; set; }
        /// <summary>
        /// 外罩門(1)關門檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X97</para>
        /// </remarks>
        [DataMember]
        bool Case_1_Close { get; set; }
        /// <summary>
        /// 外罩門(1)LOCK檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X98</para>
        /// </remarks>
        [DataMember]
        bool Case_1_Lock { get; set; }
        /// <summary>
        /// 外罩門(2)關門檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X99</para>
        /// </remarks>
        [DataMember]
        bool Case_2_Close { get; set; }
        /// <summary>
        /// 外罩門(2)LOCK檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X100</para>
        /// </remarks>
        [DataMember]
        bool Case_2_Lock { get; set; }
        /// <summary>
        /// 外罩門(3)關門檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X101</para>
        /// </remarks>
        [DataMember]
        bool Case_3_Close { get; set; }
        /// <summary>
        /// 外罩門(3)LOCK檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X102</para>
        /// </remarks>
        [DataMember]
        bool Case_3_Lock { get; set; }
        /// <summary>
        /// 面板KEY(2點鐘)_LOCK 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X112</para>
        /// </remarks>
        [DataMember]
        bool HMI_Key_Lcok { get; set; }
        /// <summary>
        /// 面板KEY(10點鐘)_手動
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X113</para>
        /// </remarks>
        [DataMember]
        bool HMI_Key_Manual { get; set; }
        /// <summary>
        /// 面板_Reset
        /// </summary>
        /// <remarks>
        /// <para>類型 : Push Button</para>
        /// <para>地址 : X114</para>
        /// </remarks>
        [DataMember]
        bool HMI_Reset { get; set; }
        /// <summary>
        /// 面板_油壓
        /// </summary>
        /// <remarks>
        /// <para>類型 : Push Button</para>
        /// <para>地址 : X115</para>
        /// </remarks>
        [DataMember]
        bool HMI_Oil_Start { get; set; }
        /// <summary>
        /// 面板_暫停
        /// </summary>
        /// <remarks>
        /// <para>類型 : Push Button</para>
        /// <para>地址 : X116</para>
        /// </remarks>
        [DataMember]
        bool HMI_Pause { get; set; }
        /// <summary>
        /// 面板_啟動
        /// </summary>
        /// <remarks>
        /// <para>類型 : Push Button</para>
        /// <para>地址 : X117</para>
        /// </remarks>
        [DataMember]
        bool HMI_Start_AutoRun { get; set; }
        /// <summary>
        /// 面板_全自動
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X118</para>
        /// </remarks>
        [DataMember]
        bool HMI_Fully_Automatic { get; set; }
        /// <summary>
        /// 面板_半自動
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X119</para>
        /// </remarks>
        [DataMember]
        bool HMI_Semi_Automatic { get; set; }
        /// <summary>
        /// 面板_原點復歸
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X120</para>
        /// </remarks>
        [DataMember]
        bool HMI_Origin_Return { get; set; }
        /// <summary>
        /// 面板_減速
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X122</para>
        /// </remarks>
        [DataMember]
        bool HMI_Arm_Slow_Down { get; set; }
        /// <summary>
        /// 面板_加速
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X123</para>
        /// </remarks>
        [DataMember]
        bool HMI_Arm_Speed_Up { get; set; }
        /// <summary>
        /// 面板_中心出水
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X124</para>
        /// </remarks>
        [DataMember]
        bool HMI_Effluent { get; set; }
        /// <summary>
        /// 面板_主軸鬆刀
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X125</para>
        /// </remarks>
        [DataMember]
        bool HMI_Spindle_Loosen { get; set; }
        /// <summary>
        /// 面板_刀庫模式
        /// </summary>
        /// <remarks>
        /// <para>類型 : 搖桿</para>
        /// <para>地址 : X126</para>
        /// </remarks>
        [DataMember]
        bool HMI_DrillHome_Mode { get; set; }
        /// <summary>
        /// 面板_啟動手搖輪
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X127</para>
        /// </remarks>
        [DataMember]
        bool HMI_HandWheel { get; set; }
        /// <summary>
        /// 面板_警報復位
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X128</para>
        /// </remarks>
        [DataMember]
        bool HMI_Alarm_Return { get; set; }
        /// <summary>
        /// 面板_送料手臂
        /// </summary>
        /// <remarks>
        /// <para>類型 : 搖桿</para>
        /// <para>地址 : X130</para>
        /// </remarks>
        [DataMember]
        bool HMI_Arm_Select { get; set; }
        /// <summary>
        /// 面板_入口料架
        /// </summary>
        /// <remarks>
        /// <para>類型 : 搖桿</para>
        /// <para>地址 : X131</para>
        /// </remarks>
        [DataMember]
        bool HMI_IN_Rack { get; set; }
        /// <summary>
        /// 面板_出口料架
        /// </summary>
        /// <remarks>
        /// <para>類型 : 搖桿</para>
        /// <para>地址 : X132</para>
        /// </remarks>
        [DataMember]
        bool HMI_OUT_Rack { get; set; }
        /// <summary>
        /// 面板_主軸模式
        /// </summary>
        /// <remarks>
        /// <para>類型 : 搖桿</para>
        /// <para>地址 : X133</para>
        /// </remarks>
        [DataMember]
        bool HMI_Spindle_Mode { get; set; }
        /// <summary>
        /// 面板_主軸正轉
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X134</para>
        /// </remarks>
        [DataMember]
        bool HMI_Spindle_CW { get; set; }
        /// <summary>
        /// 面板_主軸停止
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X135</para>
        /// </remarks>
        [DataMember]
        bool HMI_Spindle_Stop { get; set; }
        /// <summary>
        /// 面板_夾具側壓
        /// </summary>
        /// <remarks>
        /// <para>類型 : 搖桿</para>
        /// <para>地址 : X139</para>
        /// </remarks>
        [DataMember]
        bool HMI_Clip_Side_Mode { get; set; }
        /// <summary>
        /// 面板_夾具下壓
        /// </summary>
        /// <remarks>
        /// <para>類型 : 搖桿</para>
        /// <para>地址 : X140</para>
        /// </remarks>
        [DataMember]
        bool HMI_Clip_Down_Mode { get; set; }
        /// <summary>
        /// 面板_捲屑機動作
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X141</para>
        /// </remarks>
        [DataMember]
        bool HMI_Volume_scrap { get; set; }
        /// <summary>
        /// 搖桿_左
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X145</para>
        /// </remarks>
        [DataMember]
        bool JoyStick_Left { get; set; }
        /// <summary>
        /// 搖桿_右
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X146</para>
        /// </remarks>
        [DataMember]
        bool JoyStick_Right { get; set; }
        /// <summary>
        /// 搖桿_前
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X147</para>
        /// </remarks>
        [DataMember]
        bool JoyStick_Front { get; set; }
        /// <summary>
        /// 搖桿_後
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X148</para>
        /// </remarks>
        [DataMember]
        bool JoyStick_Back { get; set; }
        /// <summary>
        /// 搖桿  Push Button 1
        /// </summary>
        /// <remarks>
        /// <para>類型 : Push Button</para>
        /// <para>地址 : X149</para>
        /// </remarks>
        [DataMember]
        bool JoyStick_PB_1 { get; set; }
        /// <summary>
        /// 搖桿 Push Button 2
        /// </summary>
        /// <remarks>
        /// <para>類型 : Push Button</para>
        /// <para>地址 : X150</para>
        /// </remarks>
        [DataMember]
        bool JoyStick_PB_2 { get; set; }
        /// <summary>
        /// 搖桿 Push Button 3
        /// </summary>
        /// <remarks>
        /// <para>類型 : Push Button</para>
        /// <para>地址 : X151</para>
        /// </remarks>
        [DataMember]
        bool JoyStick_PB_3 { get; set; }
        /// <summary>
        /// 搖桿 Push Button 4
        /// </summary>
        /// <remarks>
        /// <para>類型 : Push Button</para>
        /// <para>地址 : X152</para>
        /// </remarks>
        [DataMember]
        bool JoyStick_PB_4 { get; set; }
        /// <summary>
        /// 搖桿 Push Button 5
        /// </summary>
        /// <remarks>
        /// <para>類型 : Push Button</para>
        /// <para>地址 : X153</para>
        /// </remarks>
        [DataMember]
        bool JoyStick_PB_5 { get; set; }
        /// <summary>
        /// 搖桿 Push Button 6
        /// </summary>
        /// <remarks>
        /// <para>類型 : Push Button</para>
        /// <para>地址 : X153</para>
        /// </remarks>
        [DataMember]
        bool JoyStick_PB_6 { get; set; }
    }
}