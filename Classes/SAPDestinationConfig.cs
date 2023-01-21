
using System.Runtime.CompilerServices;

namespace SAPRFC.Classes
{
    public class SAPDestinationConfig:IDestinationConfiguration
    {
        public bool ChangeEventsSupported()
        {
            //throw new NotImplementedException();
            return true;
        }

        public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;

        public RfcConfigParameters GetParameters(string destinationName)
        {
            return new RfcConfigParameters()
            {
                {RfcConfigParameters.Name,ConfigurationManager.AppSettings["SAP_SYSTEM_NAME"] },
                {RfcConfigParameters.AppServerHost, ConfigurationManager.AppSettings["SAP_SERVERHOST"] },
                {RfcConfigParameters.SystemNumber, ConfigurationManager.AppSettings["SAP_SYSTEM_NUMBER"] },
                {RfcConfigParameters.SystemID, ConfigurationManager.AppSettings["SAP_SYSTEM_ID"] },
                {RfcConfigParameters.User, ConfigurationManager.AppSettings["SAP_USERNAME"] },
                {RfcConfigParameters.Password, ConfigurationManager.AppSettings["SAP_PASSWORD"]},
                {RfcConfigParameters.Client, ConfigurationManager.AppSettings["SAP_CLIENT"] },
                {RfcConfigParameters.Language, ConfigurationManager.AppSettings["SAP_LANGUAGE"]},
                {RfcConfigParameters.PoolSize, ConfigurationManager.AppSettings["SAP_POOLSIZE"]}
            };
        }
    }

    public class Middleware
    {
        
        private readonly string _destinationName;
        public RfcDestination _rfcDestination { get; set; }

        public Middleware()
        {
            this._destinationName = ConfigurationManager.AppSettings["SAP_SYSTEM_NAME"];
            this._rfcDestination = GetDestination();
        }
    
        private string DestinationName(Middleware instance)
        {
            return instance._destinationName; 
        }
        
        private RfcDestination GetDestination()
        {
            try
            {
                return RfcDestinationManager.GetDestination(DestinationName(this));
            }
            catch (Exception)
            {
                throw new Exception();
            }
            
        }
    }

    // public static class Middleware
    // {
    //     public static RfcDestination _rfcDestination = GetDestination();
    //     private static string _destinationName = ConfigurationManager.AppSettings["SAP_SYSTEM_NAME"];
    //     
    //     private static RfcDestination GetDestination()
    //     {
    //         try
    //         {
    //             return RfcDestinationManager.GetDestination(_destinationName);
    //         }
    //         catch (Exception ex)
    //         {
    //             throw new Exception($"{ex.Message}");
    //         }
    //         
    //     }
    //     
    // }

}
