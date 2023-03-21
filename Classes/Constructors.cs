using DataBridge;
using SAPRFCLib.Targets.Interfaces;

namespace SAPRFC.Classes;

public partial class Functions : IDestinationTargetAware
{
    public RfcDestination Destination { get; set; }
    public Functions()
    {

    }
    public Functions(RfcDestination destination)
    {
        this.Destination = destination;
    }

}