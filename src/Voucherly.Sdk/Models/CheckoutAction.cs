using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voucherly.Sdk.Models
{
    public enum CheckoutAction
    {
        DIRECT,
        OTP,
        REDIRECT,
        DROPIN,
    }
}
