namespace SAPRFC.Classes
{
    
    public partial class Functions
    {
        public BaseRFCResponse<Dictionary<string, string>> ReadTable(JObject Parameters, string QueryID, List<string> ClauseArgs = null, string TableReader = "RFC_READ_TABLE", string ParametersType = "DEFAULT",string Clauses = "Single")
        {
            Dictionary<string, string> DataTableReturn = new Dictionary<string, string>();

            IRfcFunction Function = Destination.Repository.CreateFunction(TableReader);

            IRfcTable SAPFieldsTable = Function.GetTable("FIELDS");
            SAPFieldsTable.Append();

            IRfcTable OptionsTable = Function.GetTable("OPTIONS");
            OptionsTable.Append();

            try
            {
                {

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
                                    OptionsTable.SetValue("TEXT", conditions);
                                }
                                
                            }
                        }
                        
                    }
                    else if (Parameters[QueryID]["FIELDS"] is null)
                    {
                        return new BaseRFCResponse<Dictionary<string, string>>
                        {
                            Data = null,
                            StatusCode = ResponseStatus.InvalidParameters,
                            Message = ResponseStatus.InvalidParameters.Message

                        };
                    }
                    else if (Parameters[QueryID]["TABLE"] is null)
                    {
                        return new BaseRFCResponse<Dictionary<string, string>>
                        {
                            Data = null,
                            StatusCode = ResponseStatus.TableNotExist,
                            Message = ResponseStatus.TableNotExist.Message

                        };
                    }
                    else if (Parameters[QueryID]["OPTIONS"] is null)
                    {
                        return new BaseRFCResponse<Dictionary<string, string>>
                        {
                            Data = null,
                            StatusCode = ResponseStatus.InvalidParameters,
                            Message = ResponseStatus.InvalidParameters.Code

                        };
                    }
                    Function.Invoke(Destination);

                    IRfcTable DataReadTable = Function.GetTable("DATA");

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
                }
            }
            catch (Exception ex)
            {

                return new BaseRFCResponse<Dictionary<string, string>>
                {
                    Data = DataTableReturn,
                    StatusCode = ResponseStatus.RFCError,
                    Message = ex.Message
                };
            }

            return new BaseRFCResponse<Dictionary<string, string>>
            {
                Data = DataTableReturn,
                StatusCode = ResponseStatus.Success,
                Message = ResponseStatus.Success.Message

            };
        }

        public BaseRFCResponse<DataTable> ReadingTable(RFCReadParameters Parameters, string QueryString = null, string FieldSelection = "DEFAULT",  string TableReader = "RFC_READ_TABLE", string ReadMode = "Single")
        {
            DataTable ReturnData = new DataTable();

            try
            {
                IRfcFunction Function = Destination.Repository.CreateFunction(TableReader);

                IRfcTable SAPFieldsTable = Function.GetTable("FIELDS");
                SAPFieldsTable.Append();

                IRfcTable OptionsTable = Function.GetTable("OPTIONS");
                OptionsTable.Append();

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
                                OptionsTable.SetValue("TEXT", Parameters.Options[0]);
                            }
                            catch
                            {
                                return new BaseRFCResponse<DataTable>()
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
                        return new BaseRFCResponse<DataTable>()
                        {
                            Data = null,
                            Message = $"{ResponseStatus.InvalidParameters.Message}. Empty parameters are not allowed for security.",
                            StatusCode = ResponseStatus.InvalidParameters
                        };
                    }
                }

                Function.SetValue("QUERY_TABLE", Parameters.Table);
                Function.SetValue("ROWCOUNT", Parameters.RowCount.ToString());
                Function.SetValue("ROWSKIPS", Parameters.RowSkip.ToString());
                Function.SetValue("DELIMITER", Parameters.GetDelimiter());
                
                Function.Invoke(Destination);

                IRfcTable DataReadTable = Function.GetTable("DATA");

                
                int FieldsLenght = Parameters.Fields[FieldSelection].Count;
                foreach (string ColumnAlias in Parameters.GetFields(FieldSelection))
                {
                    ReturnData.Columns.Add(ColumnAlias, typeof(string));
                }

                foreach (var DataRowContent in DataReadTable)
                {
                    string RowContent = DataRowContent.GetValue("WA").ToString();
                    var LineChunks = RowContent.Split(Parameters.GetDelimiter());
                    
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
                return new BaseRFCResponse<DataTable>()
                {
                    Data = null,
                    Message = $"Message : {ResponseStatus.RFCError.Message}. Exception description: {ex.Message}",
                    StatusCode = ResponseStatus.RFCError
                };
            }

            return new BaseRFCResponse<DataTable>()
            {
                Data = ReturnData,
                Message = $"Status : {ResponseStatus.Success.Message}. {ReturnData.Rows.Count} rows fetched.",
                StatusCode = ResponseStatus.RFCError
            };
        }
        public BaseRFCResponse<DataTable> TableFromReadTable(JObject Parameters, string QueryID, List<string> ClauseArgs = null, string TableReader = "RFC_READ_TABLE", string ParametersType = "DEFAULT", string Clauses = "Single")
        {
            DataTable ReturnData = new DataTable();

            IRfcFunction Function = Destination.Repository.CreateFunction(TableReader);

            IRfcTable SAPFieldsTable = Function.GetTable("FIELDS");
            SAPFieldsTable.Append();

            IRfcTable OptionsTable = Function.GetTable("OPTIONS");
            OptionsTable.Append();

            try
            {
                if (Function != null)
                {
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
                    else if (Parameters[QueryID]["FIELDS"] is null)
                    {
                        return new BaseRFCResponse<DataTable>
                        {
                            Data = null,
                            StatusCode = ResponseStatus.InvalidParameters,
                            Message = ResponseStatus.InvalidParameters.Message

                        };
                    }
                    else if (Parameters[QueryID]["TABLE"] is null)
                    {
                        return new BaseRFCResponse<DataTable>
                        {
                            Data = null,
                            StatusCode = ResponseStatus.TableNotExist,
                            Message = ResponseStatus.TableNotExist.Message

                        };
                    }
                    else if (Parameters[QueryID]["OPTIONS"] is null)
                    {
                        return new BaseRFCResponse<DataTable>
                        {
                            Data = null,
                            StatusCode = ResponseStatus.InvalidParameters,
                            Message = ResponseStatus.InvalidParameters.Code

                        };
                    }
                    Function.Invoke(Destination);

                    IRfcTable DataReadTable = Function.GetTable("DATA");
                    Dictionary<string, string> ResponseData = new Dictionary<string, string>();

                    int FieldsLenght = Parameters[QueryID]["FIELDS"][ParametersType].ToArray().Length;

                    foreach (string ColumnAlias in Parameters[QueryID]["FIELDS"][ParametersType].ToArray())
                    {
                        ReturnData.Columns.Add(ColumnAlias,typeof(string));
                    }

                    foreach (var DataRowContent in DataReadTable)
                    {
                        string RowContent = DataRowContent.GetValue("WA").ToString();
                        char Delimiter = (char)Parameters[QueryID]["DELIMITER"];
                        var LineChunks = RowContent.Split(Delimiter);
                        
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

                return new BaseRFCResponse<DataTable>
                {
                    Data = null,
                    StatusCode = ResponseStatus.RFCError,
                    Message = ex.Message
                };
            }

            return new BaseRFCResponse<DataTable>
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
        
    }
}
