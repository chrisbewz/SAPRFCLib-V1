
namespace SAPRFC.Classes
{
    // public class SAPDestinationConfig:IDestinationConfiguration
    // {
    //     public bool ChangeEventsSupported()
    //     {
    //         //throw new NotImplementedException();
    //         return true;
    //     }
    //
    //     public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;
    //
    //     public RfcConfigParameters GetParameters(string destinationName)
    //     {
    //         return new RfcConfigParameters()
    //         {
    //             {RfcConfigParameters.Name,ConfigurationManager.AppSettings["SAP_SYSTEM_NAME"] },
    //             {RfcConfigParameters.AppServerHost, ConfigurationManager.AppSettings["SAP_SERVERHOST"] },
    //             {RfcConfigParameters.SystemNumber, ConfigurationManager.AppSettings["SAP_SYSTEM_NUMBER"] },
    //             {RfcConfigParameters.SystemID, ConfigurationManager.AppSettings["SAP_SYSTEM_ID"] },
    //             {RfcConfigParameters.User, ConfigurationManager.AppSettings["SAP_USERNAME"] },
    //             {RfcConfigParameters.Password, ConfigurationManager.AppSettings["SAP_PASSWORD"]},
    //             {RfcConfigParameters.Client, ConfigurationManager.AppSettings["SAP_CLIENT"] },
    //             {RfcConfigParameters.Language, ConfigurationManager.AppSettings["SAP_LANGUAGE"]},
    //             {RfcConfigParameters.PoolSize, ConfigurationManager.AppSettings["SAP_POOLSIZE"]}
    //         };
    //     }
    // }

    // public class Middleware
    // {
    //     
    //     private readonly string _destinationName;
    //     public RfcDestination _rfcDestination { get; set; }
    //
    //     public Middleware()
    //     {
    //         this._destinationName = ConfigurationManager.AppSettings["SAP_SYSTEM_NAME"];
    //         this._rfcDestination = GetDestination();
    //     }
    //
    //     public Middleware(object DestinationParameters)
    //     {
    //         
    //     }
    //
    //     private string DestinationName(Middleware instance)
    //     {
    //         return instance._destinationName; 
    //     }
    //     
    //     private RfcDestination GetDestination()
    //     {
    //         try
    //         {
    //             return RfcDestinationManager.GetDestination(DestinationName(this));
    //         }
    //         catch (Exception)
    //         {
    //             throw new Exception();
    //         }
    //         
    //     }
    // }

    public partial class Functions
    {
        // public void SetDestination(RfcConfigParameters parameters)
        // {
        //     if (parameters is null) throw new ArgumentNullException($"Destination configuration parameters must not be null.");
        //     this.rfcDestination ??= RfcDestinationManager.GetDestination(parameters);
        // }
        public void DisposeConnection(RfcDestination destination)
        {
            if (!(destination == null)) RfcSessionManager.EndContext(destination);
            throw new ArgumentNullException("Destination context is already disposed");
        }
        public void UnregisterDestination(IDestinationConfiguration destinationConfiguration)
        {
            if (!(destinationConfiguration == null)) RfcDestinationManager.UnregisterDestinationConfiguration(destinationConfiguration);
            throw new ArgumentNullException("RFC Destination is already unregistered.");
        }
    }
}
