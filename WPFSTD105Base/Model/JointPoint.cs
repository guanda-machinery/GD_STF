using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSTD105.Model
{
    /// <summary>
    /// 接頭打點
    /// </summary>
    public class JointPoint
    {

        private double? xPosition = null;
        public double? X_Position
        {
            get
            {
                return xPosition;
            } 
            set
            {
                xPosition = value;
                if (xPosition.HasValue)
                {
                    xPosition =Math.Abs(value.Value);
                }
            } 
        }

        private double? yPosition = null;
        public double? Y_Position
        { 
            get 
            { 
                return yPosition;
            }
            set
            {
                yPosition = value;
                if (yPosition.HasValue)
                {
                    yPosition = Math.Abs(value.Value);
                }
            }
        }
    }
}
