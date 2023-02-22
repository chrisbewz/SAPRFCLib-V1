using ParsingUtils.RFC;

namespace SAPRFC.Classes
{
    public partial class Functions
    {
        public string CharacteristicDescriptionName(string characteristic,SAPLanguages language = null)
        {
            IRfcFunction _function = rfcDestination.Repository.CreateFunction("BAPI_CHARACT_GETDETAIL");
            if(language != null) _function.SetValue("LANGUAGE",$"{language.SPRAS_CODE}");
            _function.SetValue("CHARACTNAME",$"{characteristic}");
            ;
            _function.Invoke(rfcDestination);

            string result = (from x in TableParsing.ConvertRFCTable(_function.GetTable("CHARACTDESCR")).AsEnumerable()
                select x.Field<string>("DESCRIPTION")).First<string>();
            
            return result;

        }

        public string CharacteristicDescriptionValue(string characteristic,string searchValue,SAPLanguages language = null)
        {
            IRfcFunction _function = rfcDestination.Repository.CreateFunction("BAPI_CHARACT_GETDETAIL");
            if(language != null) _function.SetValue("LANGUAGE",$"{language.SPRAS_CODE}");
            _function.SetValue("CHARACTNAME",$"{characteristic}");
            _function.Invoke(rfcDestination);

            string characteristicType = Structures.ConvertStructure(_function.GetStructure("CHARACTDETAIL")).Rows[0][1].ToString();

            DataTable valuePool = (characteristicType == "CHAR")
                ? TableParsing.ConvertRFCTable(_function.GetTable("CHARACTVALUESDESCR"))
                : (characteristicType == "NUM")
                    ? TableParsing.ConvertRFCTable(_function.GetTable("CHARACTVALUESNUM"))
                    : null;

            string result = (characteristicType == "NUM")
                ? valuePool.AsEnumerable()
                    .Select(x=> x.Field<string>("VALUE_FROM")).FirstOrDefault()
                : valuePool?.AsEnumerable()
                    .Where(x=>x.Field<string>("VALUE_CHAR") == searchValue)
                    .Select(x=>x.Field<string>("DESCRIPTION")).FirstOrDefault();

            return result ??= "Unknown";

        }
    }
}

