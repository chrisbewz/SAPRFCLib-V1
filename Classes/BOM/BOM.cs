using System.Data;



namespace SAPRFC.Classes
{
    public partial class Functions
    {
        public BaseRFCResponse<bool> BOMExists(string Material)
        {
            DataTable check = new DataTable();
            try
            {
                RFCReadParameters.MAST.SetWhereClauses($"MATNR = '{Material}'",true);
                check.Merge(ReadingTable(RFCReadParameters.MAST).Data);

                if(check.Rows.Count is (0))
                {
                    return new BaseRFCResponse<bool>()
                    {
                        Data = false,
                        Message = $"Message : {ResponseStatus.Empty.Message}. No material match at MAST table.",
                        StatusCode = ResponseStatus.Empty
                    };
                }

            }
            catch (Exception ex)
            {

                return new BaseRFCResponse<bool>()
                {
                    Data = false,
                    Message = $"Message : {ResponseStatus.RFCError.Message}. Exception : {ex.Message}",
                    StatusCode = ResponseStatus.RFCError
                };
            }
            return new BaseRFCResponse<bool>()
            {
                Data = true,
                Message = $"Message : {ResponseStatus.Success.Message}. Material has been found as BOM mantain. Infos: Plant :{check.AsEnumerable().Select(S=> S.Field<string>("WERKS")).ElementAt(0)} / Alternative BOM : {check.AsEnumerable().Select(S => S.Field<string>("STLAL")).ElementAt(0)} ",
                StatusCode = ResponseStatus.RFCError
            };
        }
        public BaseRFCResponse<DataSet> ReadBOM(string Material, DateTime ValidFrom, DateTime ValidTo, string Plant = null, string AlternativeBOM = null,string ChangeNumber = null,string RevisionLevel = null, string BOMUsage = "1")
        {
            DataSet BOMResponse = new DataSet();
            try
            {
                //Casting RFC Table Response to DataSet Instance
                if (rfcDestination != null)
                {

                    RfcRepository RFCRepo = rfcDestination.Repository;

                    IRfcFunction Function = RFCRepo.CreateFunction("CSAP_MAT_BOM_READ");
                    if(string.IsNullOrEmpty(Material))
                    {
                        return new BaseRFCResponse<DataSet>()
                        {
                            Data = null,
                            Message = ResponseStatus.InvalidParameters.Message,
                            StatusCode = ResponseStatus.InvalidParameters
                        };
                    }
                    else
                    {
                        Function.SetValue("MATERIAL", Material);
                    }
                    if(!(Plant is null))
                    {
                        Function.SetValue("PLANT", Plant);
                    }

                    if (!(string.IsNullOrEmpty(BOMUsage)))
                    {
                        Function.SetValue("BOM_USAGE", BOMUsage);
                    }

                    //Setting Dates
                    Function.SetValue("VALID_FROM", ValidFrom.ToString("dd.MM.yyyy"));
                    Function.SetValue("VALID_TO", ValidTo.ToString("dd.MM.yyyy"));

                    if(!(string.IsNullOrEmpty(AlternativeBOM)))
                    {
                        Function.SetValue("ALTERNATIVE", AlternativeBOM);
                    }

                    if(!(string.IsNullOrEmpty(ChangeNumber)))
                    {
                        Function.SetValue("CHANGE_NO", ChangeNumber);
                    }
                    
                    Function.Invoke(rfcDestination);

                    //Fetching BOM Names
                    DataTable bomItems = TableParsing.ConvertRFCTable(Function.GetTable("T_STPO"));
                    List<string> Materials = bomItems.AsEnumerable().Select(S => S.Field<string>("COMPONENT")).ToList();
                    DataTable MaterialNames = new DataTable();

                    foreach (string item in Materials)
                    {
                        //GETTING MATERIAL NAMES FROM NUMBERS QUERIED
                        try
                        {
                            RFCReadParameters.MAKT.SetWhereClauses($"MATNR =  '{Constants.MaterialSeparator}{item}' AND SPRAS = '{SAPLanguages.Portuguese.SPRAS_CODE}' ", true);
                            BaseRFCResponse<DataTable> maraDescr = ReadingTable(RFCReadParameters.MAKT);
                            MaterialNames.Merge(maraDescr.Data);
                        }
                        catch
                        {
                            continue;
                        }

                    }

                    foreach(DataRow Row in MaterialNames.Rows)
                    {
                        Row.SetField("MATNR", Row.Field<string>("MATNR").Replace(Constants.MaterialSeparator, ""));
                    }

                    var TableJoin = (from x in bomItems.AsEnumerable()
                                     join y in MaterialNames.AsEnumerable()
                                     on x.Field<string>("COMPONENT") equals y.Field<string>("MATNR")
                                     select new
                                     {
                                         Position = x.Field<string>("ITEM_NO"),
                                         Name = y.Field<string>("MAKTG"),
                                         Material = x.Field<string>("COMPONENT"),
                                         Unity_Of_Measure = x.Field<string>("COMP_UNIT"),
                                         QTD = x.Field<string>("COMP_QTY"),
                                         ECM_Until = x.Field<string>("CHG_NO_TO"),
                                         ECM_From = x.Field<string>("CHANGE_NO"),
                                         Node_Number = x.Field<string>("ITEM_NODE"),
                                         Item_Id = x.Field<string>("ITM_IDENT"),
                                         Responsible = x.Field<string>("CHANGED_BY")
                                     }).ToList();


                    DataTable FinalItems = RuntimeHelpers.Transformations.DataTables.LINQResultToDataTable<dynamic>(TableJoin);

                    FinalItems.Columns.Add("Internal_BOM",typeof(bool));

                    foreach(DataRow Row in FinalItems.Rows)
                    {
                        string CurrentMaterial = $"{Constants.MaterialSeparator}{Row.Field<string>("Material")}";
                        if (BOMExists(CurrentMaterial).Data is true)
                        {
                            Row.SetField<bool>("Internal_BOM", true);
                        }
                        else
                        {
                            Row.SetField<bool>("Internal_BOM", false);
                        }
                    }

                    //Fetching all data relative to BOM Material
                    BOMResponse.Tables.Add(FinalItems);
                    //BOMResponse.Tables.Add(base.ConvertRFCTable(Function.GetTable("T_STPO")));
                    BOMResponse.Tables.Add(TableParsing.ConvertRFCTable(Function.GetTable("T_STKO")));
                    BOMResponse.Tables.Add(TableParsing.ConvertRFCTable(Function.GetTable("T_DEP_DESCR")));
                    BOMResponse.Tables.Add(TableParsing.ConvertRFCTable(Function.GetTable("T_DEP_ORDER")));
                    BOMResponse.Tables.Add(TableParsing.ConvertRFCTable(Function.GetTable("T_DEP_DATA")));
                    BOMResponse.Tables.Add(TableParsing.ConvertRFCTable(Function.GetTable("T_DEP_DOC")));
                    BOMResponse.Tables.Add(TableParsing.ConvertRFCTable(Function.GetTable("T_DEP_SOURCE")));

                    //Fetching document assignment
                    BOMResponse.Tables.Add(TableParsing.ConvertRFCTable(Function.GetTable("T_DOC_LINK")));
                }
            }
            catch (Exception ex)
            {

                return new BaseRFCResponse<DataSet>()
                {
                    Data = BOMResponse,
                    Message = $"Message : {ResponseStatus.RFCError.Message}. Exception : {ex.Message}",
                    StatusCode = ResponseStatus.RFCError

                };
            }

            return new BaseRFCResponse<DataSet>()
            {
                Data = BOMResponse,
                Message = ResponseStatus.Success.Message,
                StatusCode = ResponseStatus.Success

            };

        }

    }
}