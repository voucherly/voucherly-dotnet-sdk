using FastIDs.TypeId;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Voucherly.Sdk.Models.Payments
{
    public class PaymentLine
    {
        public int Quantity { get; set; }
        public double? TaxRate { get; set; }
        public long UnitAmount { get; set; }
        public long UnitDiscountAmount { get; set; }
        public long FinalAmount { get; set; }
        public TypeId? ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ProductVariant { get; set; }
        public string? ProductImage { get; set; }
        public string? ExternalId1 { get; set; }
        public string? ExternalId2 { get; set; }
        public bool IsFood { get; set; }
        public bool IsGift { get; set; }
    }
}
