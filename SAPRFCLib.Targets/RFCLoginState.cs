using System;
using SAP.Middleware.Connector;
using SAPRFCLib.Targets.Interfaces;

namespace SAPRFCLib.Targets;

public sealed class RFCLoginState : IParameterAware,IDestinationTargetAware,IDisposable
{
    public RfcConfigParameters Parameters { get; set; }
    public RfcDestination Destination { get; set; }
    
    public IDestinationConfiguration Configuration { get; set; }
    public bool IsDestinationActive { get; set; } = false;

    public RFCLoginState()
    {
        this.Parameters = (RfcConfigParameters)null;
        this.Destination = (RfcDestination)null;
        this.IsDestinationActive = false;
    }

    public RFCLoginState(RfcConfigParameters parameters,RfcDestination destination)
    {
        this.Destination = destination;
        this.Parameters = parameters;
    }

    public void Dispose()
    {
        if (!(Destination.Equals(null)))
        {
            this.Parameters = (RfcConfigParameters)null;
            RfcDestinationManager.UnregisterDestinationConfiguration(this.Configuration);

        }
    }
}