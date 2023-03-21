using System.Threading.Tasks;

namespace SAPRFCLib.Interfaces
{
    public interface IFunctions
    {
        RfcDestination Destination { get; set; }
        BaseRFCResponse<DataTable> DocumentsOfMaterial(string Material, string TargetTable);
        BaseRFCResponse<DataSet> GetCharacteristics(string CharacteristicName);
        BaseRFCResponse<DataSet> GetMaterialInformation(string material, string classType, string className, string objTable = "MARA");
        public BaseRFCResponse<DataSet> SearchObjectsCompact(int maxhits, string clsname, string clstype, char allvalues = 'X', bool noauth = true, char mafid = 'O', bool externalview = true, DataTable SelectionCriteria = null);
        Task<BaseRFCResponse<DataSet>> SearchObjectsAsync(int maxhits, string clsname, string clstype, char allvalues = 'X', bool noauth = true, char mafid = 'O', bool externalview = true, DataTable SelectionCriteria = null);
        BaseRFCResponse<DataSet> SearchObjects(int maxhits, string clsname, string clstype, char allvalues = 'X', bool noauth = true, char mafid = 'O', bool externalview = true, DataTable SelectionCriteria = null);
        string MaterialName(string MaterialNumber, SAPLanguages language, string SearchOption = "DEFAULT");
        BaseRFCResponse<DataTable> MaterialsInformation(List<string> MaterialNumbers,string SearchOption = "DEFAULT");
        BaseRFCResponse<DataSet> ClassGetDetail(string classType, string classNumber, SAPLanguages languageIsoCode = null, SAPLanguages languageSAPCode = null);
        string CharacteristicDescriptionName(string characteristic, SAPLanguages language = null);
        string CharacteristicDescriptionValue(string characteristic, string searchValue,SAPLanguages language = null);
        BaseRFCResponse<DataTable> ReadingTable(RFCReadParameters Parameters, string QueryString = null, string FieldSelection = "DEFAULT", string TableReader = "RFC_READ_TABLE", string ReadMode = "Single");
    }
}

