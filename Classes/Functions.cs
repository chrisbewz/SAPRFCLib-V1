
using System.Data;
using System.Runtime.CompilerServices;
using DataBridge;


namespace SAPRFC.Classes
{
    public partial class Functions
    {
        private RfcDestination rfcDestination;
        private JObject Parameters;

        public Functions()
        {
             var bridge = new Middleware();
             var data = new Bindings();
             this.rfcDestination = bridge._rfcDestination;
             this.Parameters = data.Parameters();
        }
        public BaseResponse<Dictionary<string,string>> GetMatData(string MaterialNumber,string SearchOption = "DEFAULT")
        {
            

            string QueryType = "MATERIAL";
            Parameters[QueryType]["OPTIONS"]["QUERY_STR"] = $"MATNR = '{Constants.MaterialSeparator}{MaterialNumber}'";

            return ReadTable(Parameters, QueryType, ParametersType:SearchOption);
        }

        public BaseResponse<Dictionary<string,string>> GetMaterialDescription(string MaterialNumber,string SearchOption = "DEFAULT")
        {
            string QueryType = "MATERIALDESC";
            Parameters[QueryType]["OPTIONS"]["QUERY_STR"] = $"MATNR = '{Constants.MaterialSeparator}{MaterialNumber}'";

            return ReadTable(Parameters, QueryType, ParametersType: SearchOption);
        }

        public BaseResponse<Dictionary<string, string>> POFromMaterial(string MaterialNumber, string SearchOption = "DEFAULT")
        {

            string QueryType = "POFROMMAT";
            Parameters[QueryType]["OPTIONS"]["QUERY_STR"] = string.Format("MATNR = '{0}{1}'", Constants.MaterialSeparator, MaterialNumber);

            return ReadTable(Parameters, QueryType, ParametersType: SearchOption);
        }

        public BaseResponse<Dictionary<string, string>> ClientNumberFromPO(string PurchaseOrder, string SearchOption = "DEFAULT")
        {
            string QueryType = "CLFROMPO";
            Parameters[QueryType]["OPTIONS"]["QUERY_STR"] = $"VBELN = '{PurchaseOrder}'";

            return ReadTable(Parameters, QueryType, ParametersType: SearchOption);
        }

        public BaseResponse<Dictionary<string, string>> ClientName(string ClientNumber, string SearchOption = "DEFAULT")
        {
            string QueryType = "CLNAMEFROMCL";
            Parameters[QueryType]["OPTIONS"]["QUERY_STR"] = string.Format("KUNNR = '{0}'", ClientNumber);

            return ReadTable(Parameters, QueryType, ParametersType: SearchOption);
        }

        public BaseResponse<Dictionary<string, DataTable>> ReadMaterial(string Material)
        {
            Dictionary<string, DataTable> DataReturn = new Dictionary<string, DataTable>();

            IRfcFunction Function = rfcDestination.Repository.CreateFunction("RFC_GET_MATERIAL_DATA");

            Function.SetValue("I_MATERIAL", Material);
            Function.Invoke(rfcDestination);

            try
            {
                //Getting Material Tables

                IRfcTable MaterialDescriptionTable = Function.GetTable("SAP_FIELD_DATA");
                IRfcTable MaterialCharacteristicsTable = Function.GetTable("DMS_CLASS_DATA");

                //Parsing Tables
                DataReturn["MARA-DATA"] = TableParsing.ConvertRFCTable(MaterialDescriptionTable);
                DataReturn["MARK-DATA"] = TableParsing.ConvertRFCTable(MaterialCharacteristicsTable);
            }
            catch (Exception ex)
            {

                return new BaseResponse<Dictionary<string, DataTable>>
                {
                    Data = null,
                    Message = $"Message:{ResponseStatus.RFCError.Message}",
                    StatusCode = ResponseStatus.Success
                };
            }


            return new BaseResponse<Dictionary<string, DataTable>>
            {
                Data = DataReturn,
                Message = ResponseStatus.Success.Message,
                StatusCode = ResponseStatus.Success
            };


        }
        
        
    }
}
