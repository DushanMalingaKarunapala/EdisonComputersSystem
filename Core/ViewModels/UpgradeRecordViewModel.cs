using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    //view model for UpgradeRecord, which is used to represent upgrade records in the application
    public class UpgradeRecordViewModel
    {
        public int UpgradeRecordId { get; set; }

        public string UpgradeDescription { get; set; }
     

        public string UpgradeType { get; set; } // Assuming UpgradeType is an enum, you can convert it to string for display
        public string UpgradeStatus { get; set; } // Assuming Status is a string, you can change it to an enum if needed
    }
}
