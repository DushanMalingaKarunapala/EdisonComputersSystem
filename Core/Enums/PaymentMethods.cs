using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enums
{
    public enum PaymentMethods
    {
        Cash,
        CreditCard,
        DebitCard,
        BankTransfer,
        Check,
        MobilePayment,
        Other // For any other payment methods not listed
    }
}
