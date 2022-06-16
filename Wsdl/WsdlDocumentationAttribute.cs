using System;
using System.Reflection;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Web.Services.Description;
using System.Xml;

namespace Wsdl
{
    /// <summary>
    /// Wsdl 屬性檔
    /// </summary>
    public class WsdlDocumentationAttribute :
     Attribute,
     IContractBehavior,
     IOperationBehavior,
         IWsdlExportExtension
    {
        ContractDescription contractDescription;
        OperationDescription operationDescription;
        string text;
        XmlElement customWsdlDocElement = null;
        /// <summary>
        /// 文檔說明
        /// </summary>
        /// <param name="text"></param>
        public WsdlDocumentationAttribute(string str)
        {
            this.text = str;
        }

        /// <summary>
        /// This constructor takes an XmlElement if the sample 
        /// were to be modified to import the documentation element
        /// as XML. This sample does not use this constructor.
        /// </summary>
        /// <param name="wsdlDocElement"></param>
        public WsdlDocumentationAttribute(XmlElement wsdlDocElement)
        {
            this.customWsdlDocElement = wsdlDocElement;
        }

        public XmlElement WsdlDocElement
        {
            get { return this.customWsdlDocElement; }
            set { this.customWsdlDocElement = value; }
        }

        public string Text
        {
            get { return this.text; }
            set { this.text = value; }
        }

        #region WSDL Export

        public void ExportContract(WsdlExporter exporter, WsdlContractConversionContext context)
        {
            if (contractDescription != null)
            {
                //在此塊中，是合同級註釋屬性。
                // This.Text返回合同屬性的字符串。
                //設置doc元素； 如果未先完成此操作，則在
                // DocumentElement屬性。
                context.WsdlPortType.Documentation = string.Empty;
                XmlDocument owner = context.WsdlPortType.DocumentationElement.OwnerDocument;
                XmlElement summaryElement = owner.CreateElement("summary");
                summaryElement.InnerText = this.Text;
                context.WsdlPortType.DocumentationElement.AppendChild(summaryElement);
            }
            else
            {
                Operation operation = context.GetOperation(operationDescription);
                if (operation != null)
                {
                    operation.Documentation = String.Empty;

                    //操作C#三重註釋。
                    XmlDocument owner = operation.DocumentationElement.OwnerDocument;
                    XmlElement newSummaryElement = owner.CreateElement("summary");
                    newSummaryElement.InnerText = this.Text;
                    operation.DocumentationElement.AppendChild(newSummaryElement);

                    // 取得回傳訊息
                    ParameterInfo returnValue = operationDescription.SyncMethod.ReturnParameter;
                    object[] returnAttrs = returnValue.GetCustomAttributes(typeof(WsdlParamOrReturnDocumentationAttribute), false);
                    if (returnAttrs.Length != 0)
                    {
                        // <returns>text.</returns>
                        XmlElement returnsElement = owner.CreateElement("returns");
                        returnsElement.InnerText = ((WsdlParamOrReturnDocumentationAttribute)returnAttrs[0]).ParamComment;
                        operation.DocumentationElement.AppendChild(returnsElement);
                    }

                    // 取得參數訊息
                    ParameterInfo[] args = operationDescription.SyncMethod.GetParameters();
                    for (int i = 0; i < args.Length; i++)
                    {
                        object[] docAttrs = args[i].GetCustomAttributes(typeof(WsdlParamOrReturnDocumentationAttribute), false);
                        if (docAttrs.Length == 1)
                        {
                            // <param name="Int1">Text.</param>
                            XmlElement newParamElement = owner.CreateElement("param");
                            XmlAttribute paramName = owner.CreateAttribute("name");
                            paramName.Value = args[i].Name;
                            newParamElement.InnerText = ((WsdlParamOrReturnDocumentationAttribute)docAttrs[0]).ParamComment;
                            newParamElement.Attributes.Append(paramName);
                            operation.DocumentationElement.AppendChild(newParamElement);
                        }
                    }
                }
            }
        }

        public void ExportEndpoint(WsdlExporter exporter, WsdlEndpointConversionContext context)
        {
        }

        #endregion

        #region IContractBehavior Members

        public void AddBindingParameters(ContractDescription description, ServiceEndpoint endpoint, BindingParameterCollection parameters)
        {
            return;
        }

        public void ApplyClientBehavior(ContractDescription description, ServiceEndpoint endpoint, ClientRuntime proxy)
        {
            return;
        }

        public void ApplyDispatchBehavior(ContractDescription description, ServiceEndpoint endpoint, DispatchRuntime dispatch)
        {
            this.contractDescription = description;
        }

        public void Validate(ContractDescription description, ServiceEndpoint endpoint)
        {
            return;
        }

        #endregion

        #region IOperationBehavior Members

        public void AddBindingParameters(OperationDescription description, BindingParameterCollection parameters)
        {
            return;
        }

        public void ApplyClientBehavior(OperationDescription description, ClientOperation proxy)
        {
            return;
        }

        public void ApplyDispatchBehavior(OperationDescription description, DispatchOperation dispatch)
        {
            this.operationDescription = description;
        }

        public void Validate(OperationDescription description)
        {
            return;
        }

        #endregion
    }
}
