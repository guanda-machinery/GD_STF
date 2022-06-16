using System;

namespace Wsdl
{
    /// <summary>
    /// 有效套用屬性的應用程式項目
    /// </summary>
    public static class Targets
    {
        public const AttributeTargets ParamReturnTargets
          = AttributeTargets.ReturnValue | AttributeTargets.Parameter | AttributeTargets.GenericParameter;
    }
}
