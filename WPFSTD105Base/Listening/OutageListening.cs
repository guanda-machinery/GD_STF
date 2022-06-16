using GD_STD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WPFSTD105.CodesysIIS;
using static WPFSTD105.Properties.Power;
namespace WPFSTD105.Listening
{
    /// <summary>
    /// 斷電保持聆聽端
    /// </summary>
    public class OutageListening : AbsListening
    {
        /// <inheritdoc/>
        protected override void ReadCodeSysMemory()
        {
            Outage result = ReadCodesysMemor.GetOutage();

            Default.ArmAxisX = result.Arm.X;
            Default.ArmAxisY = result.Arm.Y;
            Default.ArmAxisZ = result.Arm.Z;

            Default.LeftAxisX = result.Left.X;
            Default.LeftAxisY = result.Left.Y;
            Default.LeftAxisZ = result.Left.Z;

            Default.RightAxisX = result.Right.X;
            Default.RightAxisY = result.Right.Y;
            Default.RightAxisZ = result.Right.Z;

            Default.MiddleAxisX = result.Middle.X;
            Default.MiddleAxisY = result.Middle.Y;
            Default.MiddleAxisZ = result.Middle.Z;

            Default.Save();
        }
    }
}
