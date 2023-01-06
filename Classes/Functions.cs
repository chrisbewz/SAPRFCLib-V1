
using System.Data;

namespace SAPRFC.Classes
{
    public partial class Functions : Tables
    {
        public Functions() :base()
        {

        }
        
        public BaseResponse<Dictionary<string,string>> GetMatData(string MaterialNumber,string SearchOption = "DEFAULT")
        {
            JObject MaterialParams = GetParameters();

            string QueryType = "MATERIAL";
            MaterialParams[QueryType]["OPTIONS"]["QUERY_STR"] = string.Format("MATNR = '{0}{1}'",Constants.MaterialSeparator,MaterialNumber);

            return ReadTable(MaterialParams, QueryType, ParametersType:SearchOption);
        }

        public BaseResponse<Dictionary<string,string>> GetMaterialDescription(string MaterialNumber,string SearchOption = "DEFAULT")
        {
            JObject MaterialParams = GetParameters();

            string QueryType = "MATERIALDESC";
            MaterialParams[QueryType]["OPTIONS"]["QUERY_STR"] = string.Format("MATNR = '{0}{1}'", Constants.MaterialSeparator, MaterialNumber);

            return ReadTable(MaterialParams, QueryType, ParametersType: SearchOption);
        }

        public BaseResponse<Dictionary<string, string>> POFromMaterial(string MaterialNumber, string SearchOption = "DEFAULT")
        {
            JObject MaterialParams = GetParameters();


            string QueryType = "POFROMMAT";
            MaterialParams[QueryType]["OPTIONS"]["QUERY_STR"] = string.Format("MATNR = '{0}{1}'", Constants.MaterialSeparator, MaterialNumber);

            return ReadTable(MaterialParams, QueryType, ParametersType: SearchOption);
        }

        public BaseResponse<Dictionary<string, string>> ClientNumberFromPO(string PurchaseOrder, string SearchOption = "DEFAULT")
        {
            JObject MaterialParams = GetParameters();

            string QueryType = "CLFROMPO";
            MaterialParams[QueryType]["OPTIONS"]["QUERY_STR"] = string.Format("VBELN = '{0}'", PurchaseOrder);

            return ReadTable(MaterialParams, QueryType, ParametersType: SearchOption);
        }

        public BaseResponse<Dictionary<string, string>> ClientName(string ClientNumber, string SearchOption = "DEFAULT")
        {
            JObject MaterialParams = GetParameters();

            string QueryType = "CLNAMEFROMCL";
            MaterialParams[QueryType]["OPTIONS"]["QUERY_STR"] = string.Format("KUNNR = '{0}'", ClientNumber);

            return ReadTable(MaterialParams, QueryType, ParametersType: SearchOption);
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
                DataReturn["MARA-DATA"] = ConvertRFCTable(MaterialDescriptionTable);
                DataReturn["MARK-DATA"] = ConvertRFCTable(MaterialCharacteristicsTable);
            }
            catch (Exception ex)
            {

                return new BaseResponse<Dictionary<string, DataTable>>
                {
                    Data = null,
                    Message = string.Format("Message:{0}",ResponseStatus.RFCError.Message),
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
