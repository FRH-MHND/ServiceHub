using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceHub.Data;
using ServiceHub.DTOs;
using ServiceHub.Models;
using ServiceHub.Services.Implementation;

namespace ServiceHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _paymentService;
        private readonly PdfService _pdfService;
        private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;

		public PaymentController(PaymentService paymentService, PdfService pdfService, ApplicationDbContext context)
        {
            _paymentService = paymentService;
            _pdfService = pdfService;
            _context = context;
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetPaymentHistory([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] string serviceType)
        {
            var userId = GetUserIdFromToken();
            var paymentHistory = await _paymentService.GetPaymentHistoryAsync(userId, startDate, endDate, serviceType);
            return Ok(paymentHistory);
        }

        [HttpGet("receipt/{id}")]
        public async Task<IActionResult> GetReceipt(int id)
        {
            var userId = GetUserIdFromToken();
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);
            if (payment == null)
            {
                return NotFound();
            }

            var paymentDto = new PaymentHistoryDto
            {
                ServiceType = payment.ServiceType,
                ProviderName = payment.ProviderName,
                Date = payment.Date,
                AmountPaid = payment.AmountPaid,
                PaymentMethod = payment.PaymentMethod
            };

            var pdf = _pdfService.GenerateReceipt(paymentDto);
            return File(pdf, "application/pdf", "receipt.pdf");
        }

        private int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User ID not found in token.");
            }

            return int.Parse(userIdClaim.Value);
        }
    }
}
