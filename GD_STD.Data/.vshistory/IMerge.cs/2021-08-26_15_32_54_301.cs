using System;

namespace GD_STD.Data
{
    [Serializable]
    public class IMerge
    {
        /// <summary>
        /// 合併物件
        /// </summary>
        /// <param name="steel"></param>
        /// <exception cref="Exception">合併物件失敗，因構件編號不相同。</exception>
        public void Merge(SteelAssembly steel)
        {
            if (steel.Number != this.Number)
            {
                throw new Exception("合併物件失敗，因構件編號不相同。");
            }
            ID.AddRange(steel.ID);
            Position.AddRange(steel.Position);
            TopLevel.AddRange(steel.TopLevel);
            Phase.AddRange(steel.Phase);
            ShippingNumber.AddRange(steel.ShippingNumber);
            ShippingDescription.AddRange(steel.ShippingDescription);
            Count++;
        }
    }
}