using System;

namespace Wsdl
{
    /// <summary>
    /// Wsdl 標記參數或回傳的屬性檔
    /// </summary>
    [AttributeUsage(Targets.ParamReturnTargets)]
    public class WsdlParamOrReturnDocumentationAttribute : Attribute
    {
        /// <summary>
        ///  Wsdl 標記參數或回傳的屬性檔
        /// </summary>
        /// <param name="docComment">要標記的文字</param>
        public WsdlParamOrReturnDocumentationAttribute(string docComment)
        {
            this.docValue = docComment;
        }

        string docValue;

        public string ParamComment
        {
            get { return docValue; }
            set { docValue = value; }
        }
    }
}
