
using System.Data;

namespace SAPRFC.Classes
{
    public partial class Functions
    {
        public BaseResponse<DataSet> SearchObjects(string clsname, string clstype, char allvalues = 'X', int maxhits = 100,bool noauth = true, char mafid = 'O', bool externalview = true,DataTable SelectionCriteria = null)
        {
            DataSet res = new DataSet();

            IRfcFunction Function = rfcDestination.Repository.CreateFunction("CLS_IVIEWS_SEARCH_OBJECTS ");
            IRfcTable OptionsTable = Function.GetTable("IT_SELECTION_TABLE");

            if (!(SelectionCriteria is null))
            {
                foreach (DataRow item in SelectionCriteria.Rows)
                {
                    OptionsTable.Append();
                    OptionsTable.SetValue("CHARACTERISTIC",item["CHARACTERISTIC"].ToString());
                    OptionsTable.SetValue("VALUE",item["VALUE"].ToString());

                }
            }
            else
            {
                return new BaseResponse<DataSet>()
                {
                    Data = null,
                    Message = "Busca sem parâmetros não permitida",
                    StatusCode = ResponseStatus.InvalidParameters
                };
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

            DataTable SelectionTable = ConvertRFCTable(Function.GetTable("IT_SELECTION_TABLE"));
            DataTable FoundObj1 = ConvertRFCTable(Function.GetTable("ET_OBJECTS"));
            DataTable FoundCharac1 = ConvertRFCTable(Function.GetTable("ET_VALUES"));
            DataTable FoundObj2 = ConvertRFCTable(Function.GetTable("ET_OBJECTS_EXTERNAL_VIEW"));
            DataTable FoundCharac2 = ConvertRFCTable(Function.GetTable("ET_VALUES_EXTERNAL_VIEW"));

            res.Tables.Add(SelectionTable);
            res.Tables.Add(FoundObj1);
            res.Tables.Add(FoundObj2);
            res.Tables.Add(FoundCharac1);
            res.Tables.Add(FoundCharac2);

            return new BaseResponse<DataSet>()
            {
                Data = res,
                Message = $"Contagem de Filtros : {SelectionTable.Rows.Count}. Resultados Encontrados{FoundObj1.Rows.Count}",
                StatusCode = ResponseStatus.Success
            };
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
            

            DataReturn.Tables.Add(ConvertRFCTable(Function.GetTable("ET_OBJECTS")));
            DataReturn.Tables.Add(ConvertRFCTable(Function.GetTable("ET_VALUES")));
            DataReturn.Tables.Add(ConvertRFCTable(Function.GetTable("ET_OBJECTS_EXTERNAL_VIEW")));
            DataReturn.Tables.Add(ConvertRFCTable(Function.GetTable("ET_VALUES_EXTERNAL_VIEW")));
            DataReturn.Tables.Add(ConvertRFCTable(Function.GetTable("IT_SELECTION_TABLE")));

            return new BaseResponse<DataSet>()
            {
                Data = DataReturn,

            };
        }


        public void FindObjects(string ClassType, string ClassName)
        {
            
        }

    }
}