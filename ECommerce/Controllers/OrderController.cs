using Azure;
using BusinessLayer.Repository;
using DataAcessLayer.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using System.Drawing.Printing;
using System.Security.Claims;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrders _order;

        public OrderController(IOrders orders)
        {
            _order = orders;
        }

        [HttpPost("order-products"), Authorize]
        public async Task<IActionResult> OrderProduct(OrderDTO obj)
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = Convert.ToInt32(id);

            var res = _order.order(obj, userId);
            return Ok(res.Result);
        }

        [HttpGet("view-orders"), Authorize]
        public async Task<IActionResult> ViewOrders()
        {
            string id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = Convert.ToInt32(id);
            var order = await _order.ViewOrders(userId);
            return Ok(order);
        }

        [HttpGet("Invoice")]
        public async Task<ActionResult> GenratePdf(int OrderId)
        {
            var pdf = await _order.Invoice(OrderId);
            string fileName = "Innvoice.pdf";
            return File(pdf, "application/pdf", fileName);
            //return pdf;
        }

        [HttpGet("View-suppilier-order")]
        public async Task<IActionResult> viewSuppilarOrder(int id)
        {
            var order = await _order.ViewSuppilerOrders(id);
            return Ok(order);
        }
    }
}