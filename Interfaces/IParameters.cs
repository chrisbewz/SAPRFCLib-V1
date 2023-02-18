using System.Threading.Tasks;

namespace SAPRFCLib.Interfaces
{
    public interface IFunctions
    {
        // void SetDestination(RfcConfigParameters parameters);
        RfcDestination rfcDestination { get; set; }
        BaseResponse<DataTable> DocumentsOfMaterial(string Material, string TargetTable);
        BaseResponse<DataSet> GetCharacteristics(string CharacteristicName);
        public BaseResponse<DataSet> GetMaterialInformation(string material, string classType, string className, string objTable = "MARA");
        public Task<DataSet> SearchObjectsAsync(int maxhits, string clsname, string clstype, char allvalues = 'X', bool noauth = true, char mafid = 'O', bool externalview = true, DataTable SelectionCriteria = null);
        public DataSet SearchObjects(int maxhits, string clsname, string clstype, char allvalues = 'X', bool noauth = true, char mafid = 'O', bool externalview = true, DataTable SelectionCriteria = null);
        public string MaterialName(string MaterialNumber, SAPLanguages language, string SearchOption = "DEFAULT");
        public BaseResponse<DataTable> MaterialsInformation(List<string> MaterialNumbers,string SearchOption = "DEFAULT");

    }
}

