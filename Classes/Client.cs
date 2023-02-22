namespace SAPRFC.Classes
{
    public partial class Functions
    {
        public BaseResponse<Dictionary<string, string>> POFromMaterial(string MaterialNumber, string SearchOption = "DEFAULT")
        {

            string QueryType = "POFROMMAT";
            Parameters[QueryType]["OPTIONS"]["QUERY_STR"] = string.Format("MATNR = '{0}{1}'", Constants.MaterialSeparator, MaterialNumber);

            return ReadTable(Parameters, QueryType, ParametersType: SearchOption);
        }

        public BaseResponse<Dictionary<string, string>> ClientNumberFromPO(string PurchaseOrder, string SearchOption = "DEFAULT")
        {
            string QueryType = "CLFROMPO";
            Parameters[QueryType]["OPTIONS"]["QUERY_STR"] = $"VBELN = '{PurchaseOrder}'";

            return ReadTable(Parameters, QueryType, ParametersType: SearchOption);
        }

        public BaseResponse<Dictionary<string, string>> ClientName(string ClientNumber, string SearchOption = "DEFAULT")
        {
            string QueryType = "CLNAMEFROMCL";
            Parameters[QueryType]["OPTIONS"]["QUERY_STR"] = string.Format("KUNNR = '{0}'", ClientNumber);

            return ReadTable(Parameters, QueryType, ParametersType: SearchOption);
        }
    }
}

