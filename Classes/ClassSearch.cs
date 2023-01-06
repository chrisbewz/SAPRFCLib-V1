
using System.Data;

namespace SAPRFC.Classes
{
    public partial class Functions
    {
                public BaseResponse<Dictionary<string, DataTable>> VCSearch(Dictionary<string, string> SelectionCriteria)
        {
            Dictionary<string, DataTable> DataReturn = new Dictionary<string, DataTable>();

            IRfcFunction Function = rfcDestination.Repository.CreateFunction("BAPI_CLASS_SELECT_OBJECTS");


            IRfcTable Criteria = Function.GetTable("SELECTIONCRITERIONS");
            Criteria.Append();

            Dictionary<string, string> Criterions = new Dictionary<string, string>()
            {
                {"NAME_CHAR" ,"System.String"},
                {"CHAR_VALUE","System.String" },
                {"NUM_VAL_FM","System.Int32" },
                {"NUM_VAL_TO","System.Int32"}
            };

            Function.SetValue("LANGUINT", Constants.PTLang.ToString());
            foreach (KeyValuePair<string, string> c in SelectionCriteria)
            {
                try
                {


                    if (c.Key.Equals("CLASS_CODE"))
                    {
                        Function.SetValue("CLASSTYPE", c.Value.ToString());

                    }
                    else if (c.Key.Equals("CLASS_NAME"))
                    {
                        Function.SetValue("CLASSNUM", c.Value.ToString());
                    }
                    else if (c.Key.Equals("MAX_LIMIT"))
                    {
                        if ((c.Key == "MAX_LIMIT"))
                        {
                            Function.SetValue("MAXHITS", c.Value.ToString());
                        }
                        else
                        {
                            //Limita a Quantidade de dados retornados
                            Function.SetValue("MAXHITS", Constants.VCSearchChunkSize.ToString());
                        }
                    }
                    else
                    {
                        try
                        {
                            Criteria.SetValue("NAME_CHAR", c.Key.ToString());
                            Criteria.SetValue("CHAR_VALUE", c.Value.ToString());
                            Criteria.Append();
                        }
                        catch
                        {
                            continue;
                        }
                        
                    }
                }
                catch
                {
                    return new BaseResponse<Dictionary<string, DataTable>>
                    {
                        Data = null,
                        Message = ResponseStatus.InvalidParameters.Message,
                        StatusCode = ResponseStatus.InvalidParameters

                    };

                }
            }

            Function.Invoke(rfcDestination);

            IRfcTable ReturnTable = Function.GetTable("SELECTEDOBJECTS");

            DataReturn.Add("Response", ConvertRFCTable(ReturnTable));

            return new BaseResponse<Dictionary<string, DataTable>>
            {
                Data = DataReturn,
                Message = ResponseStatus.Success.Message,
                StatusCode = ResponseStatus.Success
            };
        }

        public BaseResponse<Dictionary<string, DataTable>> GetClassChararteristics(string ClassName,string ClassNumber)
        {
            Dictionary<string, DataTable> ClassValues = new Dictionary<string, DataTable>();

            IRfcFunction Function = rfcDestination.Repository.CreateFunction("BAPI_CLASS_GET_CHARACTERISTICS");

            Function.SetValue("CLASSTYPE",ClassNumber);
            Function.SetValue("CLASSNUM", ClassName);

            Function.Invoke(rfcDestination);

            try
            {

                //Getting Material Tables

                IRfcTable ClassCharacteristics = Function.GetTable("CHARACTERISTICS");
                IRfcTable ClassAllowedValues= Function.GetTable("CHAR_VALUES");

                //Parsing Tables
                ClassValues["CLS-CHARAC"] = ConvertRFCTable(ClassCharacteristics);
                ClassValues["CLS-ALLOWED"] = ConvertRFCTable(ClassAllowedValues);
            }
            catch (Exception ex)
            {

                return new BaseResponse<Dictionary<string, DataTable>>
                {
                    Data = null,
                    Message = string.Format("Message:{0}", ResponseStatus.RFCError.Message),
                    StatusCode = ResponseStatus.RFCError
                };
            }

            return new BaseResponse<Dictionary<string, DataTable>>
            {
                Data = ClassValues,
                Message = ResponseStatus.Success.Message,
                StatusCode = ResponseStatus.Success
            };


        }
    }
}