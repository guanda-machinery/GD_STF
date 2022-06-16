using WPFSTD105.Listening;

namespace WPFSTD105
{
#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'IAbsListening' 的 XML 註解
    public interface IAbsListening
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'IAbsListening' 的 XML 註解
    {
        /// <summary>
        /// 啟用聆聽模式
        /// <para>如果是 true 執行緒會持續聆聽。如果是 false 執行緒會解除掉持續聆聽狀態</para>
        /// </summary>
        bool Mode { get; set; }

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'IAbsListening.Sleep' 的 XML 註解
        int Sleep { get; }
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'IAbsListening.Sleep' 的 XML 註解

#pragma warning disable CS1591 // 遺漏公用可見類型或成員 'IAbsListening.ChangeLevel(LEVEL)' 的 XML 註解
        void ChangeLevel(LEVEL level);
#pragma warning restore CS1591 // 遺漏公用可見類型或成員 'IAbsListening.ChangeLevel(LEVEL)' 的 XML 註解
    }
}