using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Web.Services.Description;
using System.Xml;
using System.Xml.Schema;

namespace Wsdl
{
    /// <summary>
    /// Wsdl文檔導入器
    /// </summary>
    public class WsdlDocumentationImporter : IWsdlImportExtension, IServiceContractGenerationExtension, IOperationContractGenerationExtension, IContractBehavior, IOperationBehavior
    {
        readonly string text;

        #region WSDL Import
        /// <summary>
        /// 初始化
        /// </summary>
        public WsdlDocumentationImporter()
        {
        }
        /// <summary>
        /// 將註解導入 Wsdl 文檔
        /// </summary>
        /// <param name="comment">註解</param>
        public WsdlDocumentationImporter(string comment)
        {
            this.text = comment;
        }

        public string Text
        {
            get { return text; }
        }

        public void ImportContract(WsdlImporter importer, WsdlContractConversionContext context)
        {
            //合約文件
            if (context.WsdlPortType.Documentation != null)
            {
                //系統檢查合約行為，以查看是否有任何實現IWsdlImportExtension。
                context.Contract.Behaviors.Add(new WsdlDocumentationImporter(context.WsdlPortType.Documentation));
            }
            // 操作文件
            foreach (Operation operation in context.WsdlPortType.Operations)
            {
                if (operation.Documentation != null)
                {
                    OperationDescription operationDescription = context.Contract.Operations.Find(operation.Name);
                    if (operationDescription != null)
                    {
                        //系統檢查操作行為，以查看是否有任何實現IWsdlImportExtension。
                        operationDescription.Behaviors.Add(new WsdlDocumentationImporter(operation.Documentation));
                    }
                }
            }
        }

        public void BeforeImport(ServiceDescriptionCollection wsdlDocuments, XmlSchemaSet xmlSchemas, ICollection<XmlElement> policy)
        {
        }

        public void ImportEndpoint(WsdlImporter importer, WsdlEndpointConversionContext context) { }

        #endregion

        #region Code Generation

        public void GenerateContract(ServiceContractGenerationContext context)
        {
            context.ContractType.Comments.AddRange(FormatComments(text));
        }

        public void GenerateOperation(OperationContractGenerationContext context)
        {
            context.SyncMethod.Comments.AddRange(FormatComments(text));
        }

        #endregion

        #region Utility Functions

        CodeCommentStatementCollection FormatComments(string text)
        {
            /*
             *請注意，在Visual C＃和Visual Basic中，XML註釋格式吸收了
             * 文檔元素，中間有一個換行符。 這個樣本
             * 可以使用XmlElement並在其中創建代碼註釋
             * 元素從來沒有換行。
             */

            CodeCommentStatementCollection collection = new CodeCommentStatementCollection();
            collection.Add(new CodeCommentStatement("從WSDL文檔：", true));
            collection.Add(new CodeCommentStatement(String.Empty, true));

            foreach (string line in WordWrap(this.Text, 80))
            {
                collection.Add(new CodeCommentStatement(line, true));
            }

            collection.Add(new CodeCommentStatement(String.Empty, true));
            return collection;
        }

        Collection<string> WordWrap(string text, int columnWidth)
        {
            Collection<string> lines = new Collection<string>();
            System.Text.StringBuilder builder = new System.Text.StringBuilder();

            string[] words = text.Split(' ');
            foreach (string word in words)
            {
                if ((builder.Length > 0) && ((builder.Length + word.Length + 1) > columnWidth))
                {
                    lines.Add(builder.ToString());
                    builder = new System.Text.StringBuilder();
                }
                builder.Append(word);
                builder.Append(' ');
            }
            lines.Add(builder.ToString());

            return lines;
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
            return;//this.contractDescription = description;
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
            // this.operationDescription = description;
        }

        public void Validate(OperationDescription description)
        {
            return;
        }

        #endregion
    }
}
