using GD_STD.MS;

namespace STDTOMS
{
    /// <summary>
    /// Codesys 資料傳送到 MSSQL
    /// </summary>
    public class CodesysToMSSQL : ICodesysToMSSQL
    {
        /// <inheritdoc/>
        public void InsertMSAxisMain(MSAxisMain ms)
        {

            MssqlHelper<MSAxisMain>.Insert(ms);
        }
        /// <inheritdoc/>
        public void InsertMSServoAxis(MSServoAxis ms)
        {
            MssqlHelper<MSServoAxis>.Insert(ms);
        }
        /// <inheritdoc/>
        public void InsertMSIO(MSIO ms)
        {
            MssqlHelper<MSIO>.Insert(ms);
        }
        /// <inheritdoc/>
        public void InsertMSRuler(MSRuler ms)
        {
            MssqlHelper<MSRuler>.Insert(ms);
        }
    }
}
