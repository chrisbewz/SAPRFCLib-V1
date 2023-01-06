using System;
using SAP.Middleware.Connector;
using System.Configuration;

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
        public RfcDestination rfcDestination;
        private readonly string _destinationName;

        public Middleware()
        {
            this._destinationName = ConfigurationManager.AppSettings["SAP_SYSTEM_NAME"];
        }

        protected string DestinationName(Middleware instance)
        {
            return instance._destinationName; 
        }
    }

    public class Destination : Middleware
    {
        private readonly string Name;
        public Destination() : base()
        {
            this.Name = base.DestinationName(this);
            base.rfcDestination = this.GetDestination();
        }
        public RfcDestination GetDestination()
        {
            try
            {
                return RfcDestinationManager.GetDestination(this.Name);
            }
            catch (Exception)
            {

                throw new Exception();
            }
            
        }
    }
}
