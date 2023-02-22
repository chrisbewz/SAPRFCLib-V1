namespace SAPRFC.Classes;
using TableParsing = ParsingUtils.DataTables.Parsing;
using StructParsing = ParsingUtils.RFC.Structures;

public partial class Functions
{
    public BaseResponse<DataSet> GetCharacteristics(string CharacteristicName)
    {
        DataSet ReturnValues = new DataSet();
        IRfcFunction Function = rfcDestination.Repository.CreateFunction("BAPI_CHARACT_GETDETAIL");
        
        Function.SetValue("CHARACTNAME",CharacteristicName);
        Function.SetValue("LANGUAGE",SAPLanguages.Portuguese.SPRAS_CODE);
        
        Function.Invoke(rfcDestination);

        try
        {
            ReturnValues.Tables.Add(TableParsing.ConvertRFCTable(Function.GetTable("CHARACTVALUESNUM")));
            ReturnValues.Tables.Add(TableParsing.ConvertRFCTable(Function.GetTable("CHARACTVALUESCHAR")));
            ReturnValues.Tables.Add(TableParsing.ConvertRFCTable(Function.GetTable("CHARACTVALUESCURR")));
            ReturnValues.Tables.Add(TableParsing.ConvertRFCTable(Function.GetTable("CHARACTVALUESDESCR")));
            ReturnValues.Tables.Add(TableParsing.ConvertRFCTable(Function.GetTable("CHARACTREFERENCES")));
            ReturnValues.Tables.Add(TableParsing.ConvertRFCTable(Function.GetTable("CHARACTRESTRICTIONS")));
            ReturnValues.Tables.Add(TableParsing.ConvertRFCTable(Function.GetTable("CHARACTDESCR")));
            ReturnValues.Tables.Add(StructParsing.ConvertStructure(Function.GetStructure("CHARACTDETAIL")));

            ReturnValues.Tables[0].TableName = "CHARACTVALUESNUM";
            ReturnValues.Tables[1].TableName = "CHARACTVALUESCHAR";
            ReturnValues.Tables[2].TableName = "CHARACTVALUESCURR";
            ReturnValues.Tables[3].TableName = "CHARACTVALUESDESCR";
            ReturnValues.Tables[4].TableName = "CHARACTREFERENCES";
            ReturnValues.Tables[5].TableName = "CHARACTRESTRICTIONS";
            ReturnValues.Tables[6].TableName = "CHARACTDESCR";
            ReturnValues.Tables[7].TableName = "CHARACTDETAIL";
            

            return new BaseResponse<DataSet>()
            {
                Data = ReturnValues,
                StatusCode = ResponseStatus.Success,
                Message = $"Success : {ReturnValues.Tables.Count} Tables created."
            };
        }
        catch (Exception e)
        {
            return new BaseResponse<DataSet>()
            {
                Data = null,
                StatusCode = ResponseStatus.RFCError,
                Message = $"RFC Exception occurred: {e.Message}."
            };
        }
    }
}