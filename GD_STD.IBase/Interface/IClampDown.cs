namespace GD_STD.Base
{
    public interface IClampDown
    {
        /// <summary>
        /// 入口右側
        /// </summary>
        ///  <remarks>面對加工機出料右邊向</remarks>
        double EntranceR { get; set; }
        /// <summary>
        /// 出口右側
        /// </summary>
        ///  <remarks>面對加工機出料右邊向</remarks>
        double ExportR { get; set; }
    }
}