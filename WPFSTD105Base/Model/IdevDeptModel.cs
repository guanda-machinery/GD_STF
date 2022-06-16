
namespace WPFSTD105
{
    /// <summary>
    /// 一個 VM 3D模型的介面
    /// </summary>
    public interface IdevDeptModel
    {
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'IdevDeptModel.Model' 的 XML 註解
        devDept.Eyeshot.Model Model { get; set; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'IdevDeptModel.Model' 的 XML 註解
        /// <summary>
        ///  當我添加或刪除一系列實體時，我只想在最後刷新模型。
        /// </summary>
        bool stopInvalidate { get; set; }
    }
}