using GD_STD.Attribute;
using GD_STD.Base;
using GD_STD.Base;
using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using static GD_STD.MemoryHelper;
namespace GD_STD
{
    /// <summary>
    /// 輸入接點
    /// </summary>
    /// <remarks>Codesys Memory 讀取</remarks>
    [DataContract]
    public struct Input : ISharedMemory/*, IInput*/
    {
        /// <summary>
        /// 急停
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X0</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool EMS  ;
        /// <summary>
        /// 油壓,排屑,出料過載
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X1</para>
        /// </remarks>
        [Codesys("感應馬達OverLoad", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool TH_RY  ;
        /// <summary>
        /// 氣壓壓力檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X2</para>
        /// </remarks>
        [Codesys("總氣壓感測器", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool AirPSI  ;
        /// <summary>
        /// 油壓壓力檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X3</para>
        /// </remarks>
        [Codesys("總油壓感測器", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HydraulicPSI  ;
        /// <summary>
        /// 左X原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X4</para>
        /// </remarks>
        [Codesys("X 軸原點", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool L_X_Origin  ;
        /// <summary>
        /// 左X負極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X5</para>
        /// </remarks>
        [Codesys("X 軸負極限", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool L_X_LimitBack  ;
        /// <summary>
        /// 左 Z 原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X6</para>
        /// </remarks>
        [Codesys("Z 軸原點", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool L_Z_Origin  ;
        /// <summary>
        /// 左 Z 負極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X7</para>
        /// </remarks>
        [Codesys("Z 負極限", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool L_Z_LimitBack  ;
        /// <summary>
        /// 左 Z 正極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X8</para>
        /// </remarks>
        [Codesys("Z 正極限", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool L_Z_LimitFront  ;
        /// <summary>
        /// 左 Y 原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X9</para>
        /// </remarks>
        [Codesys("Y 原點", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool L_Y_Origin  ;
        /// <summary>
        /// 左 Y 負極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X10</para>
        /// </remarks> 
        [Codesys("Y 負極限", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool L_Y_LimitBack  ;
        /// <summary>
        /// 左 Y 正極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X11</para>
        /// </remarks>
        [Codesys("Y 正極限", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool L_Y_LimitFornt  ;
        /// <summary>
        /// 右X原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X12</para>
        /// </remarks>
        [Codesys("X 原點", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool R_X_Origin  ;
        /// <summary>
        /// 右X負極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X13</para>
        /// </remarks>
        [Codesys("X 負極限", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool R_X_LimitBack  ;
        /// <summary>
        /// 右X正極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X14</para>
        /// </remarks>
        [Codesys("X 正極限", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool R_X_LimitFornt  ;
        /// <summary>
        /// 右 Z 原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X15</para>
        /// </remarks>
        [Codesys("Z 原點", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool R_Z_Origin  ;
        /// <summary>
        /// 右 Z 負極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X16</para>
        /// </remarks>
        [Codesys("Y 負極限", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool R_Z_LimitBack  ;
        /// <summary>
        /// 右 Z 正極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X17</para>
        /// </remarks>
        [Codesys("Z 正極限", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool R_Z_LimitFornt  ;
        /// <summary>
        /// 右 Y 原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X18</para>
        /// </remarks>
        [Codesys("Y 原點", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool R_Y_Origin  ;
        /// <summary>
        /// 右 Y 負極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X19</para>
        /// </remarks>
        [Codesys("Y 負極限", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool R_Y_LimitBack  ;
        /// <summary>
        /// 右 Y 正極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X20</para>
        /// </remarks>
        [Codesys("Y 正極限", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool R_Y_LimitFornt  ;
        /// <summary>
        /// 上X原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X21</para>
        /// </remarks>
        [Codesys("X 原點", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool M_X_Origin  ;
        /// <summary>
        /// 上X負極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X22</para>
        /// </remarks>
        [Codesys("X 負極限", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool M_X_LimitBack  ;
        /// <summary>
        /// 上X正極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X23</para>
        /// </remarks>
        [Codesys("X 正極限", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool M_X_LimitFornt  ;
        /// <summary>
        /// 上Y原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X24</para>
        /// </remarks>
        [Codesys("Y 原點", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool M_Y_Origin  ;
        /// <summary>
        /// 上Y負極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X25</para>
        /// </remarks>
        [Codesys("Y 負極限", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool M_Y_LimitBack  ;
        /// <summary>
        /// 上Y正極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X26</para>
        /// </remarks>
        [Codesys("Y 正極限", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool M_Y_LimitFornt  ;
        /// <summary>
        /// 上Z原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X27</para>
        /// </remarks>
        [Codesys("Z 原點", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool M_Z_Origin  ;
        /// <summary>
        /// 上Z負極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X28</para>
        /// </remarks>
        [Codesys("Z 負極限", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool M_Z_LimitBack  ;
        /// <summary>
        /// 上Z正極限
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X29</para>
        /// </remarks>
        [Codesys("Z 正極限", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool M_Z_LimitFornt  ;
        /// <summary>
        /// 左X and上X碰撞
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X30</para>
        /// </remarks>
        [Codesys("X & 上 X 碰撞", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool LX_MX_Touch  ;
        /// <summary>
        /// 上X and 右X碰撞
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X31</para>
        /// </remarks>
        [Codesys("X & 右 X 碰撞", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool MX_RX_Touch  ;
        /// <summary>
        /// 左主軸相位原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X32</para>
        /// </remarks>
        [Codesys("主軸相位原點", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool L_Spindle_Origin  ;
        /// <summary>
        /// 右主軸相位原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X33</para>
        /// </remarks>
        [Codesys("主軸相位原點", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool R_Spindle_Origin  ;
        /// <summary>
        /// 左主軸夾刀
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X34</para>
        /// </remarks>
        [Codesys("主軸夾刀", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool L_SpindleClip  ;
        /// <summary>
        /// 左主軸鬆刀
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X35</para>
        /// </remarks>
        [Codesys("主軸鬆刀", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool L_SpindleLoosen  ;
        /// <summary>
        /// 右主軸夾刀
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X36</para>
        /// </remarks>
        [Codesys("主軸夾刀", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool R_SpindleClip  ;
        /// <summary>
        /// 右主軸鬆刀
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X37</para>
        /// </remarks>
        [Codesys("主軸鬆刀", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool R_SpindleLoosen  ;
        /// <summary>
        /// 上主軸夾刀
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X38</para>
        /// </remarks>
        [Codesys("主軸夾刀", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool M_SpindleClip  ;
        /// <summary>
        /// 上主軸鬆刀
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X39</para>
        /// </remarks>
        [Codesys("主軸鬆刀", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool M_SpindleLoosen  ;
        /// <summary>
        /// 上軸刀庫原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X40</para>
        /// </remarks>
        [Codesys("刀庫原點", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool M_DrillHomeOrigin  ;
        /// <summary>
        /// 上軸刀庫換刀檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X41</para>
        /// </remarks>
        [Codesys("換刀檢知", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool M_DrillHomeChange  ;
        /// <summary>
        /// 左刀庫(進料邊)原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X42</para>
        /// </remarks>
        [Codesys("進料邊刀庫原點", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool L_IN_DrillHomeOrigin  ;
        /// <summary>
        /// 左刀庫(進料邊)換刀檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X43</para>
        /// </remarks>
        [Codesys("進料邊換刀檢知", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool L_IN_DrillHomeChange  ;
        /// <summary>
        /// 左刀庫(出料邊)原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X44</para>
        /// </remarks>
        [Codesys("出料邊刀庫原點", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool L_OUT_DrillHomeOrigin  ;
        /// <summary>
        /// 左刀庫(出料邊)換刀檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X45</para>
        /// </remarks>
        [Codesys("出料邊換刀檢知", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool L_OUT_DrillHomeChange  ;
        /// <summary>
        /// 右刀庫(進料邊)原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X46</para>
        /// </remarks>
        [Codesys("進料邊刀庫原點", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool R_IN_DrillHomeOrigin  ;
        /// <summary>
        /// 右刀庫(進料邊)換刀點
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X47</para>
        /// </remarks>
        [Codesys("進料邊換刀檢知", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool R_IN_DrillHomeChange  ;
        /// <summary>
        /// 右刀庫(出料邊)原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X48</para>
        /// </remarks>
        [Codesys("出料邊刀庫原點", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool R_OUT_DrillHomeOrigin  ;
        /// <summary>
        /// 右刀庫(出料邊)換刀點
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X49</para>
        /// </remarks>
        [Codesys("出料邊換刀檢知", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool R_OUT_DrillHomeChange  ;
        /// <summary>
        /// Z軸側距閘門_關
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X50</para>
        /// </remarks>
        [Codesys("測距閘門-關檢知", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Z_HighSensorDoor_Close  ;
        /// <summary>
        /// Z軸側距閘門_開
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X51</para>
        /// </remarks>
        [Codesys("測距閘門-開檢知", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool Z_HighSensorDoor_Open  ;
        /// <summary>
        /// 左X軸潤滑分路檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X52</para>
        /// </remarks>
        [Codesys("X軸潤滑分路檢知", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool L_X_lubricating  ;
        /// <summary>
        /// 左Y軸潤滑分路檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X53</para>
        /// </remarks>
        [Codesys("Y軸潤滑分路檢知", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool L_Y_lubricating  ;
        /// <summary>
        /// 左Z軸潤滑分路檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X54</para>
        /// </remarks>
        [Codesys("Z軸潤滑分路檢知", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool L_Z_lubricating  ;
        /// <summary>
        /// 右X軸潤滑分路檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X55</para>
        /// </remarks>
        [Codesys("X軸潤滑分路檢知", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool R_X_lubricating  ;
        /// <summary>
        /// 右Y軸潤滑分路檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X56</para>
        /// </remarks>
        [Codesys("Y軸潤滑分路檢知", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool R_Y_lubricating  ;
        /// <summary>
        /// 右Z軸潤滑分路檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X57</para>
        /// </remarks>
        [Codesys("Z軸潤滑分路檢知", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool R_Z_lubricating  ;
        /// <summary>
        /// 中X軸潤滑分路檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X58</para>
        /// </remarks>
        [Codesys("X軸潤滑分路檢知", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool M_X_lubricating  ;
        /// <summary>
        /// 上Y軸潤滑分路檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X59</para>
        /// </remarks>
        [Codesys("Y軸潤滑分路檢知", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool M_Y_lubricating  ;
        /// <summary>
        /// 上Z軸潤滑分路檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X60</para>
        /// </remarks>
        [Codesys("Z軸潤滑分路檢知", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool M_Z_lubricating  ;
        /// <summary>
        /// 左壓料電阻尺原點(進料端)
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X61</para>
        /// </remarks>
        [Codesys("左壓料電阻尺原點(進料端)", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool L_IN_Clip_Down_Origin  ;
        /// <summary>
        /// 左壓料電阻尺原點(出料端)
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X62</para>
        /// </remarks>
        [Codesys("左壓料電阻尺原點(出料端)", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool L_OUT_Clip_Down_Origin  ;
        /// <summary>
        /// 右壓料電阻尺原點(進料端)
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X63</para>
        /// </remarks>
        [Codesys("壓料電阻尺原點(進料端)", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool R_IN_Clip_Down_Origin  ;
        /// <summary>
        /// 右壓料電阻尺原點(出料端)
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X64</para>
        /// </remarks>
        [Codesys("壓料電阻尺原點(出料端)", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool R_OUT_Clip_Down_Origin  ;
        /// <summary>
        /// 側壓料電阻尺原點(進料端)
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X65</para>
        /// </remarks>
        [Codesys("側壓料電阻尺原點(進料端)", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool Side_IN_Clip_Origin  ;
        ///// <summary>
        ///// 側壓料電阻尺原點(出料端)
        ///// </summary>
        ///// <remarks>
        ///// <para>類型 : A 接點</para>
        ///// <para>地址 : X66</para>
        ///// </remarks>
        //[Codesys("側壓料電阻尺原點(出料端)", "A")]
        //[DataMember]
        //public bool Side_OUT_Clip_Origin  ;
        /// <summary>
        /// 左軸刀長感測器
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X67</para>
        /// </remarks>
        [Codesys("刀長感測器", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool L_DrillLength  ;
        /// <summary>
        /// 右軸刀長感測
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X68</para>
        /// </remarks>
        [Codesys("刀長感測器", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool R_DrillLength  ;
        /// <summary>
        /// 上軸刀長感測器
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X69</para>
        /// </remarks>
        [Codesys("刀長感測器", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool M_DrillLength  ;
        /// <summary>
        /// 左刀庫進料端-刀具1 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X70</para>
        /// </remarks>
        [Codesys("刀庫進料端-刀具1", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool L_IN_DrillHome_1  ;
        /// <summary>
        /// 左刀庫進料端-刀具2
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X71</para>
        /// </remarks>
        [Codesys("刀庫進料端-刀具2", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool L_IN_DrillHome_2  ;
        /// <summary>
        /// 左刀庫進料端-刀具3 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X72</para>
        /// </remarks>
        [Codesys("刀庫進料端-刀具3", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool L_IN_DrillHome_3  ;
        /// <summary>
        /// 左刀庫進料端-刀具4 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X73</para>
        /// </remarks>
        [Codesys("刀庫進料端-刀具4", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool L_IN_DrillHome_4  ;
        /// <summary>
        /// 左刀庫出料端-刀具1 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X74</para>
        /// </remarks>
        [Codesys("刀庫出料端-刀具1", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool L_OUT_DrillHome_1  ;
        /// <summary>
        /// 左刀庫出料端-刀具2 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X75</para>
        /// </remarks>
        [Codesys("刀庫出料端-刀具2", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool L_OUT_DrillHome_2  ;
        /// <summary>
        /// 左刀庫出料端-刀具3 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X76</para>
        /// </remarks>
        [Codesys("刀庫出料端-刀具3", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool L_OUT_DrillHome_3  ;
        /// <summary>
        /// 左刀庫出料端-刀具4 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X77</para>
        /// </remarks>
        [Codesys("刀庫出料端-刀具4", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool L_OUT_DrillHome_4  ;
        /// <summary>
        /// 右刀庫進料端-刀具1 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X78</para>
        /// </remarks>
        [Codesys("刀庫進料端-刀具1", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool R_IN_DrillHome_1  ;
        /// <summary>
        /// 右刀庫進料端-刀具2 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X79</para>
        /// </remarks>
        [Codesys("刀庫進料端-刀具2", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool R_IN_DrillHome_2  ;
        /// <summary>
        /// 右刀庫進料端-刀具3 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X80</para>
        /// </remarks>
        [Codesys("刀庫進料端-刀具3", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool R_IN_DrillHome_3  ;
        /// <summary>
        /// 右刀庫進料端-刀具4
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X81</para>
        /// </remarks>
        [Codesys("刀庫進料端-刀具4", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool R_IN_DrillHome_4  ;
        /// <summary>
        /// 右刀庫出料端-刀具1 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X82</para>
        /// </remarks>
        [Codesys("刀庫出料端-刀具1", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool R_OUT_DrillHome_1  ;
        /// <summary>
        /// 右刀庫出料端-刀具2 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X83</para>
        /// </remarks>
        [Codesys("刀庫出料端-刀具2", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool R_OUT_DrillHome_2  ;
        /// <summary>
        /// 右刀庫出料端-刀具3 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X84</para>
        /// </remarks>
        [Codesys("刀庫出料端-刀具3", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool R_OUT_DrillHome_3  ;
        /// <summary>
        /// 右刀庫出料端-刀具4 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X85</para>
        /// </remarks>
        [Codesys("刀庫出料端-刀具4", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool R_OUT_DrillHome_4  ;
        /// <summary>
        /// 上刀庫-刀具1 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X86</para>
        /// </remarks>
        [Codesys("刀具1", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool M_DrillHome_1  ;
        /// <summary>
        /// 上刀庫-刀具2 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X87</para>
        /// </remarks>
        [Codesys("刀具2", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool M_DrillHome_2  ;
        /// <summary>
        /// 上刀庫-刀具3 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X88</para>
        /// </remarks>
        [Codesys("刀具3", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool M_DrillHome_3  ;
        /// <summary>
        /// 上刀庫-刀具4 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X89</para>
        /// </remarks>
        [Codesys("刀具4", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool M_DrillHome_4  ;
        /// <summary>
        /// 上刀庫-刀具5 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X90</para>
        /// </remarks>
        [Codesys("刀具5", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool M_DrillHome_5  ;
        /// <summary>
        /// 手臂送料減速點檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X91</para>
        /// </remarks>
        [Codesys("材料送料減速點檢知", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool Feed_Slow_Down_Point  ;
        /// <summary>
        /// 手臂送料原點
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X92</para>
        /// </remarks>
        [Codesys("材料原點檢知", "B")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Feed_Origin  ;
        /// <summary>
        /// 出料料件口檢測
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X93</para>
        /// </remarks>
        [Codesys("鑽孔機-出料口檢知", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool FinishOut  ;
        /// <summary>
        /// 潤滑油液位檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X94</para>
        /// </remarks>
        [Codesys("潤滑油液位檢知", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Total_Lubricatig  ;
        /// <summary>
        /// 強電箱關門檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X95</para>
        /// </remarks>
        [Codesys("強電箱門關門檢知", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Electrical_BOX_Colse  ;
        /// <summary>
        /// 強電箱LOCK檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X96</para>
        /// </remarks>
        [Codesys("強電箱門鎖定檢知", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool Electrical_BOX_Lock  ;
        /// <summary>
        /// 外罩門(1)關門檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X97</para>
        /// </remarks>
        [Codesys("外罩門(1)關門檢知", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool Case_1_Close  ;
        /// <summary>
        /// 外罩門(1)LOCK檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X98</para>
        /// </remarks>
        [Codesys("外罩門(1)鎖定檢知", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool Case_1_Lock  ;
        /// <summary>
        /// 外罩門(2)關門檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X99</para>
        /// </remarks>
        [Codesys("外罩門(2)關門檢知", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool Case_2_Close  ;
        /// <summary>
        /// 外罩門(2)LOCK檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X100</para>
        /// </remarks>
        [Codesys("外罩門(2)鎖定檢知", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool Case_2_Lock  ;
        /// <summary>
        /// 外罩門(3)關門檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X101</para>
        /// </remarks>
        [Codesys("外罩門(3)關門檢知", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool Case_3_Close  ;
        /// <summary>
        /// 外罩門(3)LOCK檢知
        /// </summary>
        /// <remarks>
        /// <para>類型 : B 接點</para>
        /// <para>地址 : X102</para>
        /// </remarks>
        [Codesys("外罩門(3)鎖定檢知", "A")]
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool Case_3_Lock  ;
        /// <summary>
        /// 面板KEY(2點鐘)_LOCK 
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X112</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool HMI_Key_Lcok  ;
        /// <summary>
        /// 面板KEY(10點鐘)_手動
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X113</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool HMI_Key_Manual  ;
        /// <summary>
        /// 面板_Reset
        /// </summary>
        /// <remarks>
        /// <para>類型 : Push Button</para>
        /// <para>地址 : X114</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool HMI_Reset  ;
        /// <summary>
        /// 面板_油壓
        /// </summary>
        /// <remarks>
        /// <para>類型 : Push Button</para>
        /// <para>地址 : X115</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool HMI_Oil_Start  ;
        /// <summary>
        /// 面板_暫停
        /// </summary>
        /// <remarks>
        /// <para>類型 : Push Button</para>
        /// <para>地址 : X116</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_Pause  ;
        /// <summary>
        /// 面板_啟動
        /// </summary>
        /// <remarks>
        /// <para>類型 : Push Button</para>
        /// <para>地址 : X117</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool HMI_Start_AutoRun  ;
        /// <summary>
        /// 面板_全自動
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X118</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool HMI_Fully_Automatic  ;
        /// <summary>
        /// 面板_半自動
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X119</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool HMI_Semi_Automatic  ;
        /// <summary>
        /// 面板_原點復歸
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X120</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool HMI_Origin_Return  ;
        /// <summary>
        /// 面板_減速
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X122</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool HMI_Arm_Slow_Down  ;
        /// <summary>
        /// 面板_加速
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X123</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool HMI_Arm_Speed_Up  ;
        /// <summary>
        /// 面板_中心出水
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X124</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_Effluent  ;
        /// <summary>
        /// 面板_主軸鬆刀
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X125</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_Spindle_Loosen  ;
        /// <summary>
        /// 面板_刀庫模式
        /// </summary>
        /// <remarks>
        /// <para>類型 : 搖桿</para>
        /// <para>地址 : X126</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_DrillHome_Mode  ;
        /// <summary>
        /// 面板_啟動手搖輪
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X127</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool HMI_HandWheel  ;
        /// <summary>
        /// 面板_警報復位
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X128</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool HMI_Alarm_Return  ;
        /// <summary>
        /// 面板_送料手臂
        /// </summary>
        /// <remarks>
        /// <para>類型 : 搖桿</para>
        /// <para>地址 : X130</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool HMI_Arm_Select  ;
        /// <summary>
        /// 面板_入口料架
        /// </summary>
        /// <remarks>
        /// <para>類型 : 搖桿</para>
        /// <para>地址 : X131</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_IN_Rack  ;
        /// <summary>
        /// 面板_出口料架
        /// </summary>
        /// <remarks>
        /// <para>類型 : 搖桿</para>
        /// <para>地址 : X132</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool HMI_OUT_Rack  ;
        /// <summary>
        /// 面板_主軸模式
        /// </summary>
        /// <remarks>
        /// <para>類型 : 搖桿</para>
        /// <para>地址 : X133</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_Spindle_Mode  ;
        /// <summary>
        /// 面板_主軸正轉
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X134</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool HMI_Spindle_CW  ;
        /// <summary>
        /// 面板_主軸停止
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X135</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool HMI_Spindle_Stop  ;
        /// <summary>
        /// 面板_夾具側壓
        /// </summary>
        /// <remarks>
        /// <para>類型 : 搖桿</para>
        /// <para>地址 : X139</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool HMI_Clip_Side_Mode  ;
        /// <summary>
        /// 面板_夾具下壓
        /// </summary>
        /// <remarks>
        /// <para>類型 : 搖桿</para>
        /// <para>地址 : X140</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool HMI_Clip_Down_Mode  ;
        /// <summary>
        /// 面板_捲屑機動作
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X141</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool HMI_Volume_scrap  ;
        /// <summary>
        /// 搖桿_左
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X145</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool JoyStick_Left  ;
        /// <summary>
        /// 搖桿_右
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X146</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool JoyStick_Right  ;
        /// <summary>
        /// 搖桿_前
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X147</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool JoyStick_Front  ;
        /// <summary>
        /// 搖桿_後
        /// </summary>
        /// <remarks>
        /// <para>類型 : A 接點</para>
        /// <para>地址 : X148</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool JoyStick_Back  ;
        /// <summary>
        /// 搖桿  Push Button 1
        /// </summary>
        /// <remarks>
        /// <para>類型 : Push Button</para>
        /// <para>地址 : X149</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool JoyStick_PB_1  ;
        /// <summary>
        /// 搖桿 Push Button 2
        /// </summary>
        /// <remarks>
        /// <para>類型 : Push Button</para>
        /// <para>地址 : X150</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool JoyStick_PB_2  ;
        /// <summary>
        /// 搖桿 Push Button 3
        /// </summary>
        /// <remarks>
        /// <para>類型 : Push Button</para>
        /// <para>地址 : X151</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool JoyStick_PB_3  ;
        /// <summary>
        /// 搖桿 Push Button 4
        /// </summary>
        /// <remarks>
        /// <para>類型 : Push Button</para>
        /// <para>地址 : X152</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)]
        public bool JoyStick_PB_4  ;
        /// <summary>
        /// 搖桿 Push Button 5
        /// </summary>
        /// <remarks>
        /// <para>類型 : Push Button</para>
        /// <para>地址 : X153</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool JoyStick_PB_5  ;
        /// <summary>
        /// 搖桿 Push Button 6
        /// </summary>
        /// <remarks>
        /// <para>類型 : Push Button</para>
        /// <para>地址 : X154</para>
        /// </remarks>
        [DataMember]
        [MarshalAs(UnmanagedType.I1)] 
        public bool JoyStick_PB_6  ;

        void ISharedMemory.ReadMemory()
        {
            using (var input = InPutMemory.CreateViewAccessor(0, Marshal.SizeOf(typeof(Input)), MemoryMappedFileAccess.Read))
            {
                Input result;
                input.Read<Input>(0, out result);
                this = result;
            }
        }
    }
}
