using System.Threading.Tasks;

namespace SAPRFCLib.Interfaces
{
    public interface IFunctions
    {
        RfcDestination rfcDestination { get; set; }
        BaseResponse<DataTable> DocumentsOfMaterial(string Material, string TargetTable);
        BaseResponse<DataSet> GetCharacteristics(string CharacteristicName);
        BaseResponse<DataSet> GetMaterialInformation(string material, string classType, string className, string objTable = "MARA");
        Task<DataSet> SearchObjectsAsync(int maxhits, string clsname, string clstype, char allvalues = 'X', bool noauth = true, char mafid = 'O', bool externalview = true, DataTable SelectionCriteria = null);
        DataSet SearchObjects(int maxhits, string clsname, string clstype, char allvalues = 'X', bool noauth = true, char mafid = 'O', bool externalview = true, DataTable SelectionCriteria = null);
        string MaterialName(string MaterialNumber, SAPLanguages language, string SearchOption = "DEFAULT");
        BaseResponse<DataTable> MaterialsInformation(List<string> MaterialNumbers,string SearchOption = "DEFAULT");
        BaseResponse<DataSet> ClassGetDetail(string classType, string classNumber, SAPLanguages languageIsoCode = null, SAPLanguages languageSAPCode = null);
        string CharacteristicDescriptionName(string characteristic, SAPLanguages language = null);
        string CharacteristicDescriptionValue(string characteristic, string searchValue,SAPLanguages language = null);
    }
}

