using System.Data;


namespace SAPRFC.Classes
{
    public partial class Functions
    {
        public BaseRFCResponse<DataTable> GetWBSData(string WBSelements)
        {
            DataTable Response = new DataTable();
            Response.TableName = "WBS_INFOS";

            try
            {
                //Casting RFC Table Response to DataSet Instance
                if (Destination != null)
                {

                    RfcRepository RFCRepo = Destination.Repository;

                    IRfcFunction Function = RFCRepo.CreateFunction("BAPI_PROJECT_GETINFO");
                    Function.SetValue("PROJECT_DEFINITION", WBSelements);
                    Function.Invoke(Destination);
                    Response.Merge(TableParsing.ConvertRFCTable(Function.GetTable("E_WBS_ELEMENT_TABLE")),true);
                }

            }
            
            catch (Exception ex)
            {
                return new BaseRFCResponse<DataTable>
                {
                    Data = null,
                    StatusCode = ResponseStatus.Empty,
                    Message = ResponseStatus.Empty.Message
                };
            }
            return new BaseRFCResponse<DataTable>
            {
                Data = Response,
                StatusCode = ResponseStatus.Success,
                Message = $"{ResponseStatus.Success.Message}. Table contains [ Rows {Response.Rows.Count} : Columns {Response.Columns.Count} ]"
            };
        }

        public BaseRFCResponse<Dictionary<string, string>> GetPEP(string MaterialNumber, string SearchOption = "DEFAULT")
        {
            

            string QueryType = "WBS_ELEM";
            Parameters[QueryType]["OPTIONS"]["QUERY_STR"] = $"KUNNR EQ '{MaterialNumber}'";

            return ReadTable(Parameters, QueryType, ParametersType: SearchOption);
        }
    }
}