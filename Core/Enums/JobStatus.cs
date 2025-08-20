using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enums
{
    public enum JobStatus
    {
        CheckedIn,
        InProgress,
        AwaitingParts,
        TasksCompleted, // All child tasks are done, ready for internal review/billing
        ReadyForBilling, // Or similar, if there's a distinct billing finalization step
        ReadyForPickup,
        Collected,
        OnHold,
        Cancelled
    }
}
