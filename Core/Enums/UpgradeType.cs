using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enums
{
    public enum UpgradeType
    {
        RAMUpgrade,
        SSDUpgrade,
        CompletePcBuild,    
        GPUUpgrade,
        CPUUpgrade,
        MotherboardUpgrade,
        PowerSupplyUpgrade,
        CoolingSystemUpgrade,
        StorageUpgrade,
        NetworkCardUpgrade,
        NewPurchase, // New purchase of a component or system
        Other
    }
}
