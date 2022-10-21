using DevExpress.CodeParser;
using DevExpress.Diagram.Core.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFSTD105
{
    public class MyCs
    {
        public MyCs()
        {

        }

      

        /// <summary>
        /// 字串除法轉換
        /// </summary>
        /// <param name="instr"></param>
        /// <returns></returns>
        public double DivSymbolConvert(string instr)
        {
            
            double GetValue = 0.0;

            string[] inputStrings = instr.Split('/');
            if (Convert.ToDouble(inputStrings[0]) == 0 || instr == "0")
            {
                return 0;
            }
            GetValue = Convert.ToDouble(inputStrings[0]) / Convert.ToDouble(inputStrings[1]);

            return GetValue; 
        }





        public class assemblyPos
        {
           public double Start { set; get; }
           public double End { set; get; }
           public bool IsCut { set; get; }
           public string Number { set; get; }
        }



    }





}
