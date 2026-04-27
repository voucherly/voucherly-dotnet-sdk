using fbognini.Sdk.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Voucherly.Sdk;
using Voucherly.Sdk.Requests;

namespace Voucherly.Checkout.Controllers;

public class PayController : Controller
{
    private readonly ILogger<PayController> _logger;
    private readonly IVoucherlyApiService _voucherlyApiService;

    public PayController(ILogger<PayController> logger, IVoucherlyApiService VoucherlyApiService)
    {
        _logger = logger;
        _voucherlyApiService = VoucherlyApiService;
    }

    [HttpPost]
    public async Task<ActionResult> Init()
    {

        var request = new CreatePaymentRequest
        {
            ReferenceId = Guid.NewGuid().ToString(),
            CustomerFirstName = "Mario",
            CustomerLastName = "Red",
            CustomerEmail = "mario.red@gmail.com",
            Lines = new List<CreatePaymentRequest.PaymentLine>()
            {
                new()
                {
                    Quantity = 1,
                    UnitAmount = 790,
                    Product = new CreatePaymentRequest.PaymentLineProduct()
                    {
                        Name = "Fresh bowl",
                        Image = "https://ucarecdn.com/76a940de-f611-479d-9fc4-bd348dc27f53/-/preview/200x200/",
                        IsFood = true,
                    }
                }

            },
            Mode = Sdk.Models.Payments.PaymentMode.Payment,
            RedirectOkUrl = Url.Action("Success", "Pay", null, HttpContext.Request.Scheme)!,
            RedirectKoUrl = Url.Action("Error", "Pay", null, HttpContext.Request.Scheme)!,
        };

        try
        {
            var payment = await _voucherlyApiService.CreatePayment(request);
            return new RedirectResult(payment.CheckoutUrl!);
        }
        catch (ApiException ex)
        {
            return BadRequest(ex.Content);
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