
using System.Data;

namespace SAPRFC.Classes
{
    public partial class Functions
    {
        public BaseResponse<DataTable> DocumentsOfMaterial(string Material, string TargetTable = "MARA")
        {
            IRfcFunction Function = rfcDestination.Repository.CreateFunction("BAPI_DOCUMENT_GETOBJECTDOCS ");

            try
            {
                //Setting material number to query for linked documents
                Function.SetValue("OBJECTKEY",$"{Constants.MaterialSeparator}{Material}");
            
                //Specifying type of material to search from. MARA means that function will findo only documents referring to MARA object numbers known as customer materials.
                Function.SetValue("OBJECTTYPE",TargetTable);
            
                Function.Invoke((rfcDestination));
            }
            catch (Exception e)
            {
                return new BaseResponse<DataTable>()
                {
                    Data = null,
                    Message = $"Message : {ResponseStatus.RFCError.Message}. Exception : {e.Message}",
                    StatusCode = ResponseStatus.RFCError

                };
            }

            return new BaseResponse<DataTable>()
            {
                Data = base.ConvertRFCTable(Function.GetTable("DOCUMENTLIST")),
                Message = ResponseStatus.Success.Message,
                StatusCode = ResponseStatus.Success

            };

        }

        // public BaseResponse<DataTable> DocumentInformation(string DocumentNumber)
        // {
        //     IRfcFunction Function = rfcDestination.Repository.CreateFunction("BAPI_DOCUMENT_GETDETAIL2");
        //
        //     try
        //     {
        //         if (!string.IsNullOrEmpty(DocumentNumber))
        //         {
        //             Function.SetValue("DOCUMENTTYPE",);
        //         }
        //     }
        //     catch (Exception e)
        //     {
        //         Console.WriteLine(e);
        //         throw;
        //     }
        // }
        //
        // public BaseResponse<DataTable> DocumentInformation(List<string> DocumentsList)
        // {
        //     
        // }
    }
    
}