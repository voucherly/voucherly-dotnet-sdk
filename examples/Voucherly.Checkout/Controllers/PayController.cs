using Microsoft.AspNetCore.Mvc;
using Voucherly.Sdk;
using Voucherly.Sdk.Enums;
using Voucherly.Sdk.Exceptions;
using Voucherly.Sdk.Requests;

namespace Voucherly.Checkout.Controllers;

public class PayController(IVoucherlyClient voucherly) : Controller
{
    [HttpPost]
    public async Task<ActionResult> Init(CancellationToken cancellationToken)
    {
        var request = new CreatePaymentRequest
        {
            Mode = PaymentMode.Payment,
            ReferenceId = Guid.NewGuid().ToString(),
            CustomerFirstName = "Mario",
            CustomerLastName = "Rossi",
            CustomerEmail = "mario.rossi@example.com",
            Lines =
            [
                new PaymentLineRequest
                {
                    Quantity = 1,
                    UnitAmount = 790,
                    Product = new PaymentLineRequestProduct
                    {
                        Name = "Fresh bowl",
                        Image = "https://ucarecdn.com/76a940de-f611-479d-9fc4-bd348dc27f53/-/preview/200x200/",
                        LineType = LineType.Food,
                    },
                },
            ],
            RedirectOkUrl = Url.Action("Success", "Pay", null, HttpContext.Request.Scheme)!,
            RedirectKoUrl = Url.Action("Error", "Pay", null, HttpContext.Request.Scheme)!,
        };

        try
        {
            var payment = await voucherly.Payments.CreateAsync(request, cancellationToken);
            return Redirect(payment.CheckoutUrl!);
        }
        catch (ApiException exception)
        {
            return BadRequest(exception.RawBody);
        }
    }

    public IActionResult Success()
    {
        return View();
    }

    public IActionResult Error()
    {
        return View();
    }
}
