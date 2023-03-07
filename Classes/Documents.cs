using GenericTypes.Documents;


namespace SAPRFC.Classes
{
    public partial class Functions
    {
        public BaseRFCResponse<DataTable> DocumentsOfMaterial(string Material, string TargetTable = "MARA")
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
                return new BaseRFCResponse<DataTable>()
                {
                    Data = null,
                    Message = $"Message : {ResponseStatus.RFCError.Message}. Exception : {e.Message}",
                    StatusCode = ResponseStatus.RFCError

                };
            }

            return new BaseRFCResponse<DataTable>()
            {
                Data = TableParsing.ConvertRFCTable(Function.GetTable("DOCUMENTLIST")),
                Message = ResponseStatus.Success.Message,
                StatusCode = ResponseStatus.Success

            };

        }
        //Considering just basic data passed as arguments
        public BaseRFCResponse<DataSet> DocumentInformation(Document DocData)
        {
            IRfcFunction Function = rfcDestination.Repository.CreateFunction("BAPI_DOCUMENT_GETDETAIL2");
            //Setting Parameters
            DataSet Response = new DataSet();
            DataTable ResponseTable = new DataTable();
            
            try
            {
                if(!string.IsNullOrEmpty(DocData.SapName))
                {
                    //TODO: Adicionar a em alguma classe de suporte os padding zeros para inserir o numero de documento corretamente na BAPI_DOCUMENT_GET_DETAIL2
                    Function.SetValue("DOCUMENTNUMBER",$"00000000000000{DocData.SapName}");
                }
                else
                {
                    
                    return new BaseRFCResponse<DataSet>()
                    {
                        Data = null,Message = "Document number with null assignment is not allowed",StatusCode = ResponseStatus.InvalidParameters
                    };
                }

                if(!string.IsNullOrEmpty(DocData.Version))
                {

                    Function.SetValue("DOCUMENTVERSION",DocData.Version);    

                }
                else
                {

                    return new BaseRFCResponse<DataSet>()
                    {
                        Data = null,
                        Message = "Document without version assignment is not allowed",
                        StatusCode = ResponseStatus.InvalidParameters
                    };

                }

                if (!(string.IsNullOrEmpty(DocData.PartialVersion)))
                {
                    Function.SetValue("DOCUMENTPART",DocData.PartialVersion);
                }
                else
                {
                    DocData.PartialVersion = "000";
                    Function.SetValue("DOCUMENTPART",DocData.PartialVersion);
                }    


                
                if (!string.IsNullOrEmpty(DocData.Name))
                {
                    //considering that partial version of document is commom set in SAP as 000 for almost all docs
                    Function.SetValue("DOCUMENTTYPE",DocData.Name);
                }
                else
                {
                    return new BaseRFCResponse<DataSet>()
                    {
                        Data = null,
                        Message = "Document without type assignment is not allowed",
                        StatusCode = ResponseStatus.InvalidParameters
                    };
                }
                
                //Setting optional parameters
                
                Function.SetParameterActive("GETOBJECTLINKS",true);
                Function.SetParameterActive("GETDOCDESCRIPTIONS",true);
                Function.SetParameterActive("GETDOCFILES",true);
                Function.SetParameterActive("GETSTRUCTURE",true);
                Function.SetParameterActive("GETWHEREUSED",true);
                
                Function.Invoke(rfcDestination);
                
                //Fetching all needed response tables

                DataTable Files = TableParsing.ConvertRFCTable(Function.GetTable("DOCUMENTFILES"));

                var filesData = Files.AsEnumerable().Select(x => new
                {
                    DOCTYPE = x.Field<string>("WSAPPLICATION"),
                    ORIGINAL = x.Field<string>("DOCFILE"),
                    LAST_MODIFIER = x.Field<string>("CHANGED_BY"),
                    CREATOR = x.Field<string>("CREATED_BY"),
                    CREATE_DATA = x.Field<string>("CREATED_AT")
                }).ToList();
                
                DataTable TableData = RuntimeHelpers.Transformations.DataTables.LINQResultToDataTable(filesData);
                Response.Tables.Add(TableData);
            }
            
            catch (Exception ex)
            {
                return new BaseRFCResponse<DataSet>()
                {
                    Data = null,
                    Message = $"Message :{ResponseStatus.RFCError.Message}. Exception thrown : {ex.Message}",
                    StatusCode = ResponseStatus.RFCError
                };
            }

            return new BaseRFCResponse<DataSet>()
            {
                Data = Response,
                Message =
                    $"Message :{ResponseStatus.Success}. DataSet fetch:({Response.Tables.Count.ToString()} [Individual Tables] )"
            };

        }
    }
    
}