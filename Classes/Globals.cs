using System;
using SAP.Middleware.Connector;
using System.Configuration;

namespace SAPRFC.Classes
{
    public static class Globals
    {
        public static void AppStart()
        {
            string destinationCongurationName = ConfigurationManager.AppSettings["SAP_SYSTEM_NAME"];

            IDestinationConfiguration destinationConfiguration = null;
            bool destinationIsInitialized = false;

            if (!destinationIsInitialized)
            {
                destinationConfiguration = new SAPDestinationConfig();
                destinationConfiguration.GetParameters(destinationCongurationName);

                if (RfcDestinationManager.TryGetDestination(destinationCongurationName) == null)
                {
                    RfcDestinationManager.RegisterDestinationConfiguration(destinationConfiguration);
                    destinationIsInitialized = true;
                }
            }
        }

    }

    public class SAPConnectorInterface
    {
        private RfcDestination rfcDestination;


        public bool TestConnection(string destinationName)
        {
            bool result = false;

            try
            {
                rfcDestination = RfcDestinationManager.GetDestination(destinationName);
                if (rfcDestination != null)
                {
                    rfcDestination.Ping();
                    result = true;
                }

            }
            catch (Exception ex)
            {
                //result = false;
                throw new Exception("Connection Error: " + ex.Message);
            }
            return result;
        }

    }

}