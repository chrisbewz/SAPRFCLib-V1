

using System.Data;

namespace SAPRFC.Classes
{
    public class FetchData : Destination
    {
        
        private JObject _queryParameters;
        private JObject _tooltips;
        private JObject _sqlQueries;
        private JObject _matches;
        public FetchData()
        {
            this._queryParameters = JSONParse(ConfigurationManager.AppSettings["QUERIES_JSON"]);
            this._tooltips = JSONParse(ConfigurationManager.AppSettings["TIPS_JSON"]);
            this._sqlQueries = JSONParse(ConfigurationManager.AppSettings["SQL_CHAR_QUERIES"]);
            this._matches = JSONParse(ConfigurationManager.AppSettings["SQL_CHAR_MATCHES"]);
        }
        public DataTable ConvertRFCTable(IRfcTable SAPTable)
        {
            DataTable DotNetTable = new DataTable();

            for (int RfcTableItem = 0; RfcTableItem < SAPTable.ElementCount; RfcTableItem++)
            {
                RfcElementMetadata TableElementData = SAPTable.GetElementMetadata(RfcTableItem);
                DotNetTable.Columns.Add(TableElementData.Name);

            }

            foreach (IRfcStructure RfcTableRow in SAPTable)
            {
                DataRow Tablerow = DotNetTable.NewRow();

                for (int RfcTableItem = 0; RfcTableItem < SAPTable.ElementCount; RfcTableItem++)
                {
                    RfcElementMetadata TableElementData = SAPTable.GetElementMetadata(RfcTableItem);
                    if (TableElementData.DataType == RfcDataType.BCD && TableElementData.Name == "ABC")
                    {
                        Tablerow[RfcTableItem] = RfcTableRow.GetInt(TableElementData.Name);
                    }
                    else
                    {
                        Tablerow[RfcTableItem] = RfcTableRow.GetString(TableElementData.Name);

                    }

                }
                DotNetTable.Rows.Add(Tablerow);
            }
            return DotNetTable;
        }
        public JObject JSONParse(string fileName)
        {
            JObject o2;
            // read SON directly from a file
            StreamReader file = File.OpenText(String.Format(fileName));
            
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                 o2 = (JObject)JToken.ReadFrom(reader);
            }

            return o2;
        }

        protected JObject Parameters(FetchData instance)
        {
            return instance._queryParameters;
        }

        public JObject GetTips(FetchData instance)
        {
            return instance._tooltips;
        }

        public JObject GetQueries(FetchData instance)
        {
            return instance._sqlQueries;
        }

        public JObject GetMatches(FetchData instance)
        {
            return instance._matches;
        }

    }

    public class Tables : FetchData
    {
        private JObject _parameters;
        public Tables():base()
        {
            this._parameters = base.Parameters(this);
        }

        public JObject GetParameters()
        {
            try
            {
                return this._parameters;
            }
            catch (Exception)
            {
                throw new Exception();
            }

        }
        public BaseResponse<Dictionary<string, string>> ReadTable(JObject Parameters, string QueryID, List<string> ClauseArgs = null, string TableReader = "RFC_READ_TABLE", string ParametersType = "DEFAULT",string Clauses = "Single")
        {
            Dictionary<string, string> DataTableReturn = new Dictionary<string, string>();

            IRfcFunction Function = rfcDestination.Repository.CreateFunction(TableReader);

            IRfcTable SAPFieldsTable = Function.GetTable("FIELDS");
            SAPFieldsTable.Append();

            IRfcTable OptionsTable = Function.GetTable("OPTIONS");
            OptionsTable.Append();

            try
            {
                if (Function != null)
                {
                    //Setting SAPImport Tables Parameters


                    if (!(Parameters[QueryID]["TABLE"] is null))
                    {
                        Function.SetValue("QUERY_TABLE", Parameters[QueryID]["TABLE"].ToString());
                    }
                    if (!(Parameters[QueryID]["DELIMITER"] is null))
                    {
                        Function.SetValue("DELIMITER", Parameters[QueryID]["DELIMITER"].ToString());
                    }

                    if (!(Parameters[QueryID]["FIELDS"] is null))
                    {
                        foreach (var key in Parameters[QueryID]["FIELDS"][ParametersType])
                        {
                            if (key != Parameters[QueryID]["FIELDS"][ParametersType].Last())
                            {
                                SAPFieldsTable.SetValue("FIELDNAME", key.ToString());
                                SAPFieldsTable.Append();
                            }
                            else
                            {
                                SAPFieldsTable.SetValue("FIELDNAME", key.ToString());
                            }

                        }
                    }
                    if (!(Parameters[QueryID]["ROWCOUNT"] is null))
                    {
                        Function.SetValue("ROWCOUNT", Parameters[QueryID]["ROWCOUNT"].ToString());
                    }

                    if (!(Parameters[QueryID]["ROWSKIPS"] is null))
                    {
                        Function.SetValue("ROWSKIPS", Parameters[QueryID]["ROWSKIPS"].ToString());
                    }

                    if (!(Parameters[QueryID]["OPTIONS"] is null))
                    {
                        if(Clauses.Equals("Single"))
                        {
                            var QueryString = Parameters[QueryID]["OPTIONS"]["QUERY_STR"];
                            OptionsTable.SetValue("TEXT", QueryString.ToString());
                        }
                        else if (Clauses.Equals("Multiple"))
                        {
                            foreach(string conditions in ClauseArgs)
                            {
                                if (!(conditions.Equals(ClauseArgs.Last())))
                                {
                                    OptionsTable.SetValue("TEXT", $"{conditions}");
                                    OptionsTable.Append();
                                }
                                else
                                {
                                    OptionsTable.SetValue("TEXT", conditions.ToString());
                                }
                                
                            }
                        }
                        
                    }

                    //Case Fields are Null
                    else if (Parameters[QueryID]["FIELDS"] is null)
                    {
                        return new BaseResponse<Dictionary<string, string>>
                        {
                            Data = null,
                            StatusCode = ResponseStatus.InvalidParameters,
                            Message = ResponseStatus.InvalidParameters.Message

                        };
                    }

                    //Case Fields are Null
                    else if (Parameters[QueryID]["TABLE"] is null)
                    {
                        return new BaseResponse<Dictionary<string, string>>
                        {
                            Data = null,
                            StatusCode = ResponseStatus.TableNotExist,
                            Message = ResponseStatus.TableNotExist.Message

                        };
                    }

                    //Case Options are Null
                    else if (Parameters[QueryID]["OPTIONS"] is null)
                    {
                        return new BaseResponse<Dictionary<string, string>>
                        {
                            Data = null,
                            StatusCode = ResponseStatus.InvalidParameters,
                            Message = ResponseStatus.InvalidParameters.Code

                        };
                    }
                    Function.Invoke(rfcDestination);

                    //try
                    //{

                    //Fetching RFC Messages and Exceptions
                    //IRfcTable MessageTable = Function.GetTable("");
                    IRfcTable DataReadTable = Function.GetTable("DATA");
                    Dictionary<string, string> ResponseData = new Dictionary<string, string>();

                    foreach (var DataRowContent in DataReadTable)
                    {
                        string RowContent = DataRowContent.GetValue("WA").ToString();

                        char Delimiter = (char)Parameters[QueryID]["DELIMITER"];
                        string[] columns = RowContent.Split(Delimiter);

                        foreach (string ColumnItem in columns)
                        {
                            int index = Array.IndexOf(columns, ColumnItem);
                            if(!(DataTableReturn.ContainsKey(Parameters[QueryID]["FIELDS"][ParametersType][index].ToString())))
                            {
                                if (!(string.IsNullOrWhiteSpace(ColumnItem)))
                                {
                                    DataTableReturn.Add(Parameters[QueryID]["FIELDS"][ParametersType][index].ToString(), ColumnItem);
                                }
                                else
                                {
                                    DataTableReturn.Add(Parameters[QueryID]["FIELDS"][ParametersType][index].ToString(), Constants.NotValidField);
                                }
                            }

                        }
                    }
                    //}
                    //catch (Exception ex)
                    //{

                    //    return new BaseResponse<Dictionary<string, string>>
                    //    {
                    //        Data = null,
                    //        StatusCode = ResponseStatus.Empty,
                    //        Message = ResponseStatus.Empty.Message
                    //    };
                    //}

                }
            }
            catch (Exception ex)
            {

                return new BaseResponse<Dictionary<string, string>>
                {
                    Data = DataTableReturn,
                    StatusCode = ResponseStatus.RFCError,
                    Message = ex.Message
                };
            }

            return new BaseResponse<Dictionary<string, string>>
            {
                Data = DataTableReturn,
                StatusCode = ResponseStatus.Success,
                Message = ResponseStatus.Success.Message

            };
        }

        public BaseResponse<DataTable> ReadingTable(RFCReadParameters Parameters, string QueryString = null, string FieldSelection = "DEFAULT",  string TableReader = "RFC_READ_TABLE", string ReadMode = "Single")
        {
            Dictionary<string, DataTable> DataTableReturn = new Dictionary<string, DataTable>();
            DataTable ReturnData = new DataTable();

            try
            {
                IRfcFunction Function = rfcDestination.Repository.CreateFunction(TableReader);

                IRfcTable SAPFieldsTable = Function.GetTable("FIELDS");
                SAPFieldsTable.Append();

                IRfcTable OptionsTable = Function.GetTable("OPTIONS");
                OptionsTable.Append();

                //Setting function params

                if (Parameters.Fields[FieldSelection].Any())
                {
                    foreach (string FieldName in Parameters.Fields[FieldSelection])
                    {
                        if (!(FieldName.Equals(Parameters.Fields[FieldSelection].Last())))
                        {
                            SAPFieldsTable.SetValue("FIELDNAME", FieldName);
                            SAPFieldsTable.Append();
                        }
                        else
                        {
                            SAPFieldsTable.SetValue("FIELDNAME", FieldName);
                        }
                    }
                }

                if (Parameters.Options.Any())
                {
                    if (ReadMode.ToLower().Equals("single"))
                    {
                        if (QueryString is null)
                        {
                            try 
                            {
                                //Assuming that exist at least one Clause on Options list, or if it's set at runtime in code
                                //Then only the first element is used
                                OptionsTable.SetValue("TEXT", Parameters.Options[0]);

                            }
                            catch
                            {
                                return new BaseResponse<DataTable>()
                                {
                                    Data = null,
                                    Message = ResponseStatus.InvalidParameters.Message,
                                    StatusCode = ResponseStatus.InvalidParameters
                                };
                            }
                                
                        }
                        else
                        {
                            OptionsTable.SetValue("TEXT", QueryString);
                        }
                    }
                    else if (ReadMode.ToLower().Equals("multiple"))
                    {
                        foreach (string Condition in Parameters.Options)
                        {
                            if (!Condition.Equals(Parameters.Options.Last()))
                            {
                                OptionsTable.SetValue("TEXT", Condition);
                                OptionsTable.Append();
                            }
                            else
                            {
                                OptionsTable.SetValue("TEXT", Condition);
                            }
                        }
                    }
                    else
                    {
                        return new BaseResponse<DataTable>()
                        {
                            Data = null,
                            Message = $"{ResponseStatus.InvalidParameters.Message}. Empty parameters are not allowed for security.",
                            StatusCode = ResponseStatus.InvalidParameters
                        };
                    }
                }

                //Setting commom import parameters for RFC_READ_TABLE

                Function.SetValue("QUERY_TABLE", Parameters.Table);
                Function.SetValue("ROWCOUNT", Parameters.RowCount.ToString());
                Function.SetValue("ROWSKIPS", Parameters.RowSkip.ToString());
                Function.SetValue("DELIMITER", Parameters.GetDelimiter());

                //Invoking RFC_READ_TABLE from RFC SAP destination repository
                Function.Invoke(rfcDestination);

                //Fetching response

                IRfcTable DataReadTable = Function.GetTable("DATA");
                Dictionary<string, string> ResponseData = new Dictionary<string, string>();

                //Instantiating the return data structure
                int FieldsLenght = Parameters.Fields[FieldSelection].Count;
                foreach (string ColumnAlias in Parameters.GetFields(FieldSelection))
                {
                    ReturnData.Columns.Add(ColumnAlias, typeof(string));
                }

                foreach (var DataRowContent in DataReadTable)
                {
                    //Fetch Individual Lines os WA return structure
                    string RowContent = DataRowContent.GetValue("WA").ToString();
                    var LineChunks = RowContent.Split(Parameters.GetDelimiter());

                    //Passing Line Contents to Datatable
                    DataRow Row;
                    Row = ReturnData.NewRow();

                    int i = 0;
                    foreach (string Chunk in LineChunks)
                    {
                        string ColumnName = Parameters.Fields[FieldSelection].ElementAt(i);
                        if (!(string.IsNullOrWhiteSpace(Chunk) || string.IsNullOrEmpty(Chunk)))
                        {

                            Row[ColumnName] = Chunk;
                        }
                        else
                        {
                            Row[ColumnName] = Constants.NotValidField;
                        }
                        i++;
                    }
                    ReturnData.Rows.Add(Row);
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<DataTable>()
                {
                    Data = null,
                    Message = $"Message : {ResponseStatus.RFCError.Message}. Exception description: {ex.Message}",
                    StatusCode = ResponseStatus.RFCError
                };
            }

            return new BaseResponse<DataTable>()
            {
                Data = ReturnData,
                Message = $"Status : {ResponseStatus.Success.Message}. {ReturnData.Rows.Count} rows fetched.",
                StatusCode = ResponseStatus.RFCError
            };
        }
        public BaseResponse<DataTable> TableFromReadTable(JObject Parameters, string QueryID, List<string> ClauseArgs = null, string TableReader = "RFC_READ_TABLE", string ParametersType = "DEFAULT", string Clauses = "Single")
        {
            Dictionary<string, DataTable> DataTableReturn = new Dictionary<string, DataTable>();
            DataTable ReturnData = new DataTable();

            IRfcFunction Function = rfcDestination.Repository.CreateFunction(TableReader);

            IRfcTable SAPFieldsTable = Function.GetTable("FIELDS");
            SAPFieldsTable.Append();

            IRfcTable OptionsTable = Function.GetTable("OPTIONS");
            OptionsTable.Append();

            try
            {
                if (Function != null)
                {
                    //Setting SAPImport Tables Parameters


                    if (!(Parameters[QueryID]["TABLE"] is null))
                    {
                        Function.SetValue("QUERY_TABLE", Parameters[QueryID]["TABLE"].ToString());
                    }
                    if (!(Parameters[QueryID]["DELIMITER"] is null))
                    {
                        Function.SetValue("DELIMITER", Parameters[QueryID]["DELIMITER"].ToString());
                    }

                    if (!(Parameters[QueryID]["FIELDS"] is null))
                    {
                        foreach (var key in Parameters[QueryID]["FIELDS"][ParametersType])
                        {
                            if (key != Parameters[QueryID]["FIELDS"][ParametersType].Last())
                            {
                                SAPFieldsTable.SetValue("FIELDNAME", key.ToString());
                                SAPFieldsTable.Append();
                            }
                            else
                            {
                                SAPFieldsTable.SetValue("FIELDNAME", key.ToString());
                            }

                        }
                    }
                    if (!(Parameters[QueryID]["ROWCOUNT"] is null))
                    {
                        Function.SetValue("ROWCOUNT", Parameters[QueryID]["ROWCOUNT"].ToString());
                    }

                    if (!(Parameters[QueryID]["ROWSKIPS"] is null))
                    {
                        Function.SetValue("ROWSKIPS", Parameters[QueryID]["ROWSKIPS"].ToString());
                    }

                    if (!(Parameters[QueryID]["OPTIONS"] is null))
                    {
                        if (Clauses.Equals("Single"))
                        {
                            var QueryString = Parameters[QueryID]["OPTIONS"]["QUERY_STR"];
                            OptionsTable.SetValue("TEXT", QueryString.ToString());
                        }
                        else if (Clauses.Equals("Multiple"))
                        {
                            foreach (string conditions in ClauseArgs)
                            {
                                if (!(conditions.Equals(ClauseArgs.Last())))
                                {
                                    OptionsTable.SetValue("TEXT", $"{conditions}");
                                    OptionsTable.Append();
                                }
                                else
                                {
                                    OptionsTable.SetValue("TEXT", conditions.ToString());
                                }

                            }
                        }

                    }

                    //Case Fields are Null
                    else if (Parameters[QueryID]["FIELDS"] is null)
                    {
                        return new BaseResponse<DataTable>
                        {
                            Data = null,
                            StatusCode = ResponseStatus.InvalidParameters,
                            Message = ResponseStatus.InvalidParameters.Message

                        };
                    }

                    //Case Fields are Null
                    else if (Parameters[QueryID]["TABLE"] is null)
                    {
                        return new BaseResponse<DataTable>
                        {
                            Data = null,
                            StatusCode = ResponseStatus.TableNotExist,
                            Message = ResponseStatus.TableNotExist.Message

                        };
                    }

                    //Case Options are Null
                    else if (Parameters[QueryID]["OPTIONS"] is null)
                    {
                        return new BaseResponse<DataTable>
                        {
                            Data = null,
                            StatusCode = ResponseStatus.InvalidParameters,
                            Message = ResponseStatus.InvalidParameters.Code

                        };
                    }
                    Function.Invoke(rfcDestination);

                    IRfcTable DataReadTable = Function.GetTable("DATA");
                    Dictionary<string, string> ResponseData = new Dictionary<string, string>();

                    //Instantiating the return data structure

                    int FieldsLenght = Parameters[QueryID]["FIELDS"][ParametersType].ToArray().Length;

                    foreach (string ColumnAlias in Parameters[QueryID]["FIELDS"][ParametersType].ToArray())
                    {
                        ReturnData.Columns.Add(ColumnAlias,typeof(string));
                    }

                    foreach (var DataRowContent in DataReadTable)
                    {
                        //Fetch Individual Lines os WA return structure
                        string RowContent = DataRowContent.GetValue("WA").ToString();
                        char Delimiter = (char)Parameters[QueryID]["DELIMITER"];
                        var LineChunks = RowContent.Split(Delimiter);

                        //Passing Line Contents to Datatable
                        DataRow Row; 
                        Row = ReturnData.NewRow();

                        int i = 0;
                        foreach (string Chunk in LineChunks)
                        {
                            string ColumnName = Parameters[QueryID]["FIELDS"][ParametersType].ToArray().ElementAt(i).ToString();
                            if (!(string.IsNullOrWhiteSpace(Chunk) || string.IsNullOrEmpty(Chunk)))
                            {

                                Row[ColumnName] = Chunk;
                            }
                            else
                            {
                                Row[ColumnName] = Constants.NotValidField;
                            }
                            
                            i++;
                        }
                        ReturnData.Rows.Add(Row);
                    }
                }
            }
            catch (Exception ex)
            {

                return new BaseResponse<DataTable>
                {
                    Data = null,
                    StatusCode = ResponseStatus.RFCError,
                    Message = ex.Message
                };
            }

            //DataTableReturn.Add("Response", ReturnData);

            return new BaseResponse<DataTable>
            {
                Data = ReturnData,
                StatusCode = ResponseStatus.Success,
                Message = ResponseStatus.Success.Message

            };
        }

        public Dictionary<string, Dictionary<string, object>> DataTableToDictionary(DataTable Table, string RootKey)
        {
            var columns = Table.Columns.Cast<DataColumn>().Where(c => c.ColumnName != RootKey);

            Dictionary<string,Dictionary<string,object>> DictionaryFromTable = Table.Rows.Cast<DataRow>()
                .ToDictionary(r => r[RootKey].ToString(), r => columns.ToDictionary(c => c.ColumnName, c => r[c.ColumnName]));

            return DictionaryFromTable;
        }

        public Dictionary<string, object> GetDict(DataTable dt)
        {
            return dt.AsEnumerable()
              .ToDictionary<DataRow, string, object>(row => row.Field<string>(0),
                                        row => row.Field<object>(1));
        }


        public List<Dictionary<string, object>> DataTableToDictionaries(DataTable dt)
        {
            var dictionaries = new List<Dictionary<string, object>>();
            foreach (DataRow row in dt.Rows)
            {
                Dictionary<string, object> dictionary = Enumerable.Range(0, dt.Columns.Count).ToDictionary(i => dt.Columns[i].ColumnName, i => row.ItemArray[i]);
                dictionaries.Add(dictionary);
            }

            return dictionaries;
        }

        public DataTable RemoveDuplicatesRecords(DataTable dt)
        {
            //Returns just 5 unique rows
            var UniqueRows = dt.AsEnumerable().Distinct(DataRowComparer.Default);
            DataTable dt2 = UniqueRows.CopyToDataTable();
            return dt2;
        }

    }
}
