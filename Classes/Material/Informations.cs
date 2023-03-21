
using System.ComponentModel;
using SAPRFCLib.Interfaces;


namespace SAPRFC.Classes
{
    public partial class Functions: IFunctions
    {
        private JObject Parameters;
        
        public BaseRFCResponse<Dictionary<string,string>> GetMatData(string MaterialNumber,string SearchOption = "DEFAULT")
        {
            string QueryType = "MATERIAL";
            Parameters[QueryType]["OPTIONS"]["QUERY_STR"] = $"MATNR = '{Constants.MaterialSeparator}{MaterialNumber}'";

            return ReadTable(Parameters, QueryType, ParametersType:SearchOption);
        }
        
        [Obsolete("Obsolete method.Call MaterialsInformation instead")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public BaseRFCResponse<Dictionary<string,string>> GetMaterialDescription(string MaterialNumber,string SearchOption = "DEFAULT")
        {
            string QueryType = "MATERIALDESC";
            Parameters[QueryType]["OPTIONS"]["QUERY_STR"] = $"MATNR = '{Constants.MaterialSeparator}{MaterialNumber}'";

            return ReadTable(Parameters, QueryType, ParametersType: SearchOption);
        }
        public string MaterialName(string MaterialNumber,SAPLanguages language,string SearchOption = "ALL")
        {
            language ??= SAPLanguages.Portuguese;
            var MAKT_PARAMS = RFCReadParameters.MAKT;
            MAKT_PARAMS.SetWhereClauses(new List<string>()
            {
                $"MATNR = '{Constants.MaterialSeparator}{MaterialNumber}' AND",
                $"SPRAS = '{language.SPRAS_CODE}'"
            }, true);
            
            var returnTable = ReadingTable(MAKT_PARAMS,
                null,
                SearchOption,
                "RFC_READ_TABLE","Multiple").Data;
            var materialDesc = returnTable
                .AsEnumerable()
                .Select(x => x.Field<string>("MAKTX"))
                .First()
                .ToString();

            return materialDesc;

        }
        
        public BaseRFCResponse<DataTable> MaterialsInformation(List<string> MaterialNumbers,string SearchOption="DEFAULT")
        {
            var MAKT_PARAMS = RFCReadParameters.MAKT;
            
            MAKT_PARAMS.SetWhereClauses(MaterialNumbers);
            return ReadingTable(MAKT_PARAMS,
                null,
                SearchOption,
                "RFC_READ_TABLE","Multiple");
        }
        
        public BaseRFCResponse<Dictionary<string, DataTable>> ReadMaterial(string Material)
        {
            Dictionary<string, DataTable> DataReturn = new Dictionary<string, DataTable>();

            IRfcFunction Function = Destination.Repository.CreateFunction("RFC_GET_MATERIAL_DATA");

            Function.SetValue("I_MATERIAL", Material);
            Function.Invoke(Destination);

            try
            {
                //Getting Material Tables

                IRfcTable MaterialDescriptionTable = Function.GetTable("SAP_FIELD_DATA");
                IRfcTable MaterialCharacteristicsTable = Function.GetTable("DMS_CLASS_DATA");

                //Parsing Tables
                DataReturn["MARA-DATA"] = TableParsing.ConvertRFCTable(MaterialDescriptionTable);
                DataReturn["MARK-DATA"] = TableParsing.ConvertRFCTable(MaterialCharacteristicsTable);
            }
            catch (Exception ex)
            {

                return new BaseRFCResponse<Dictionary<string, DataTable>>
                {
                    Data = null,
                    Message = $"Message:{ResponseStatus.RFCError.Message}",
                    StatusCode = ResponseStatus.Success
                };
            }


            return new BaseRFCResponse<Dictionary<string, DataTable>>
            {
                Data = DataReturn,
                Message = ResponseStatus.Success.Message,
                StatusCode = ResponseStatus.Success
            };


        }
        
    }
}
