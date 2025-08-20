using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enums
{
    public enum UpgradeStatus
    {
        Requested,   // Upgrade has been requested but not yet started
        InProgress, // Upgrade is currently being performed
        PendingParts, // Waiting for parts to arrive
        UpgradeComplete, // Upgrade has been completed successfully
        Cancelled // Upgrade has been cancelled
    }
}
