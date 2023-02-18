using System.Data;
using System.Threading.Tasks;


namespace SAPRFC.Classes
{
    public partial class Functions
    {
        public async Task<DataSet> SearchObjectsAsync( int maxhits,string clsname, string clstype, char allvalues = 'X',bool noauth = true, char mafid = 'O', bool externalview = true,DataTable SelectionCriteria = null)
        {
            DataSet res = new DataSet();

            IRfcFunction Function = rfcDestination.Repository.CreateFunction("CLS_IVIEWS_SEARCH_OBJECTS ");
            IRfcTable OptionsTable = Function.GetTable("IT_SELECTION_TABLE");

            await Task.Run((() =>
            {
                if (!(SelectionCriteria is null || SelectionCriteria.Rows.Count.Equals(0)))
                {
                    foreach (DataRow item in SelectionCriteria.Rows)
                    {
                        if (item.Field<string>("VALUE") is null || item.Field<string>("VALUE").Equals(string.Empty)) continue;

                        OptionsTable.Append();
                        OptionsTable.SetValue("CHARACTERISTIC", item["CHARACTERISTIC"].ToString());
                        OptionsTable.SetValue("VALUE", item["VALUE"].ToString());
                    }
                }

                Function.SetValue("I_CLASS", clsname);
                Function.SetValue("I_CLASSTYPE", clstype);
                Function.SetValue("I_MAXIMUM_NUMBER_OF_HITS", maxhits);

                if (externalview)
                {
                    Function.SetValue("I_EXTERNAL_VIEW", 'X');
                }

                if (noauth)
                {
                    Function.SetValue("I_NO_AUTH_CHECK", 'X');
                }

                Function.SetValue("I_MAFID", mafid);
                Function.SetValue("I_ALL_VALUES", allvalues);

                Function.Invoke(rfcDestination);
            }));


            DataTable SelectionTable = TableParsing.ConvertRFCTable(Function.GetTable("IT_SELECTION_TABLE"));
            DataTable FoundObj1 = TableParsing.ConvertRFCTable(Function.GetTable("ET_OBJECTS"));
            DataTable FoundCharac1 = TableParsing.ConvertRFCTable(Function.GetTable("ET_VALUES"));
            DataTable FoundObj2 = TableParsing.ConvertRFCTable(Function.GetTable("ET_OBJECTS_EXTERNAL_VIEW"));
            DataTable FoundCharac2 = TableParsing.ConvertRFCTable(Function.GetTable("ET_VALUES_EXTERNAL_VIEW"));

            res.Tables.Add(SelectionTable);
            res.Tables.Add(FoundObj1);
            res.Tables.Add(FoundObj2);
            res.Tables.Add(FoundCharac1);
            res.Tables.Add(FoundCharac2);

            return res;
        }
        public DataSet SearchObjects( int maxhits,string clsname, string clstype, char allvalues = 'X',bool noauth = true, char mafid = 'O', bool externalview = true,DataTable SelectionCriteria = null)
        {
            DataSet res = new DataSet();

            IRfcFunction Function = rfcDestination.Repository.CreateFunction("CLS_IVIEWS_SEARCH_OBJECTS ");
            IRfcTable OptionsTable = Function.GetTable("IT_SELECTION_TABLE");

            if (!(SelectionCriteria is null || SelectionCriteria.Rows.Count.Equals(0)))
            {
                foreach (DataRow item in SelectionCriteria.Rows)
                {
                    if (item.Field<string>("VALUE") is null || item.Field<string>("VALUE").Equals(string.Empty)) continue;

                    OptionsTable.Append();
                    OptionsTable.SetValue("CHARACTERISTIC", item["CHARACTERISTIC"].ToString());
                    OptionsTable.SetValue("VALUE", item["VALUE"].ToString());
                }
            }

            Function.SetValue("I_CLASS", clsname);
            Function.SetValue("I_CLASSTYPE", clstype);
            Function.SetValue("I_MAXIMUM_NUMBER_OF_HITS", maxhits);

            if (externalview)
            {
                Function.SetValue("I_EXTERNAL_VIEW", 'X');
            }

            if (noauth)
            {
                Function.SetValue("I_NO_AUTH_CHECK", 'X');
            }

            Function.SetValue("I_MAFID", mafid);
            Function.SetValue("I_ALL_VALUES", allvalues);

            Function.Invoke(rfcDestination);

            DataTable SelectionTable = TableParsing.ConvertRFCTable(Function.GetTable("IT_SELECTION_TABLE"));
            DataTable FoundObj1 = TableParsing.ConvertRFCTable(Function.GetTable("ET_OBJECTS"));
            DataTable FoundCharac1 = TableParsing.ConvertRFCTable(Function.GetTable("ET_VALUES"));
            DataTable FoundObj2 = TableParsing.ConvertRFCTable(Function.GetTable("ET_OBJECTS_EXTERNAL_VIEW"));
            DataTable FoundCharac2 = TableParsing.ConvertRFCTable(Function.GetTable("ET_VALUES_EXTERNAL_VIEW"));

            res.Tables.Add(SelectionTable);
            res.Tables.Add(FoundObj1);
            res.Tables.Add(FoundObj2);
            res.Tables.Add(FoundCharac1);
            res.Tables.Add(FoundCharac2);

            return res;
        }
        public BaseResponse<DataSet> ClassObjects(string ClassType, string ClassName,Dictionary<string,string> SelectionParameters = null,string MaxHits = "999",bool IgnoreOptions = true)
        { 
            DataSet DataReturn = new DataSet();

            IRfcFunction Function = rfcDestination.Repository.CreateFunction("CLSX_SEARCH_OBJECTS");
            
            //Commom Parameters
            Function.SetValue("I_CLASS",ClassName);
            Function.SetValue("I_CLASSTYPE",ClassType);
            Function.SetValue("I_MAXIMUM_NUMBER_OF_HITS",MaxHits);
            
            Function.SetValue("I_EXTERNAL_VIEW",'X');
            Function.SetValue("I_ALL_VALUES",'X');
            Function.SetValue("I_NO_AUTH_CHECK",'X');
            Function.SetValue("I_SUBCLASSES",'X');

            if (!(IgnoreOptions))
            {
                IRfcTable OptionsCriteria = Function.GetTable("IT_SELECTION_TABLE");
                OptionsCriteria.Append();

                if (SelectionParameters is null)
                {
                    throw new NullReferenceException();
                }
                else
                {
                    foreach (string item in SelectionParameters.Keys.ToList())
                    {
                        if (!(item.Equals(SelectionParameters.Keys.ToList().Last())))
                        {
                            OptionsCriteria.SetValue("CHARACTERISTIC",item.ToString());
                            OptionsCriteria.Append();
                            OptionsCriteria.SetValue("VALUE",SelectionParameters[item].ToString());
                            OptionsCriteria.Append();
                        }
                        else
                        {
                            OptionsCriteria.SetValue("CHARACTERISTIC","item");
                            OptionsCriteria.SetValue("VALUE",SelectionParameters[item].ToString());
                        }
                    }
                }
            }
            
            Function.Invoke(rfcDestination);
            

            DataReturn.Tables.Add(TableParsing.ConvertRFCTable(Function.GetTable("ET_OBJECTS")));
            DataReturn.Tables.Add(TableParsing.ConvertRFCTable(Function.GetTable("ET_VALUES")));
            DataReturn.Tables.Add(TableParsing.ConvertRFCTable(Function.GetTable("ET_OBJECTS_EXTERNAL_VIEW")));
            DataReturn.Tables.Add(TableParsing.ConvertRFCTable(Function.GetTable("ET_VALUES_EXTERNAL_VIEW")));
            DataReturn.Tables.Add(TableParsing.ConvertRFCTable(Function.GetTable("IT_SELECTION_TABLE")));

            return new BaseResponse<DataSet>()
            {
                Data = DataReturn,

            };
        }
        public BaseResponse<DataSet> GetMaterialInformation(string material, string classType, string className, string objTable = "MARA")
        {
            IRfcFunction Function = rfcDestination.Repository.CreateFunction("BAPI_OBJCL_GETDETAIL");

            // RFCReadParameters readParameters = RFCReadParameters.AUSP;
            // readParameters.InsertFields(new List<string> { "CUOBF", "MATNR" }, "TEMP");
            // readParameters.SetWhereClauses($"MATNR = '0000000000{material}'", true);
            // var responseSearch = ReadingTable(readParameters, null, "TEMP");
            // var objKey = responseSearch.Data.Rows[0].Field<string>("CUOBF");

            Function.SetValue("OBJECTKEY", material);
            Function.SetValue("OBJECTTABLE", objTable);
            Function.SetValue("CLASSTYPE", classType);
            Function.SetValue("CLASSNUM", className);

            Function.Invoke(rfcDestination);

            DataSet result = new DataSet();

            result.Tables.Add(TableParsing.ConvertRFCTable(Function.GetTable("ALLOCVALUESNUM")));
            result.Tables.Add(TableParsing.ConvertRFCTable(Function.GetTable("ALLOCVALUESCHAR")));
            result.Tables.Add(TableParsing.ConvertRFCTable(Function.GetTable("ALLOCVALUESCURR")));

            return new BaseResponse<DataSet>()
            {
                Data = result
            };

        }

        public void FindObjects(string ClassType, string ClassName)
        {
            
        }

    }
}