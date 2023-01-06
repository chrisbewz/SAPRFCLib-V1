using System.Data;

namespace SAPRFC.Classes
{
    public partial class Functions
    {
        public BaseResponse<DataTable> GetWBSData(string WBSelements)
        {
            DataTable Response = new DataTable();
            Response.TableName = "WBS_INFOS";

            try
            {
                //Casting RFC Table Response to DataSet Instance
                if (rfcDestination != null)
                {

                    RfcRepository RFCRepo = rfcDestination.Repository;

                    IRfcFunction Function = RFCRepo.CreateFunction("BAPI_PROJECT_GETINFO");
                    Function.SetValue("PROJECT_DEFINITION", WBSelements);
                    Function.Invoke(rfcDestination);
                    Response.Merge(base.ConvertRFCTable(Function.GetTable("E_WBS_ELEMENT_TABLE")),true);
                }

            }
            
            catch (Exception ex)
            {
                return new BaseResponse<DataTable>
                {
                    Data = null,
                    StatusCode = ResponseStatus.Empty,
                    Message = ResponseStatus.Empty.Message
                };
            }
            return new BaseResponse<DataTable>
            {
                Data = Response,
                StatusCode = ResponseStatus.Success,
                Message = string.Format("{0}. Table contains [ Rows {1} : Columns {2} ]", ResponseStatus.Success.Message,Response.Rows.Count, Response.Columns.Count)
            };
        }

        public BaseResponse<Dictionary<string, string>> GetPEP(string MaterialNumber, string SearchOption = "DEFAULT")
        {
            JObject MaterialParams = GetParameters();

            string QueryType = "WBS_ELEM";
            MaterialParams[QueryType]["OPTIONS"]["QUERY_STR"] = string.Format("KUNNR EQ '{0}'", MaterialNumber);

            return ReadTable(MaterialParams, QueryType, ParametersType: SearchOption);
        }
    }
}