using BankingApp.Main.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.Main.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransferController : ControllerBase
    {
        private readonly TransferService _service;

        public TransferController(TransferService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Transfer(int fromId, int toId, decimal amount)
        {
            bool success = await _service.TransferAsync(fromId, toId, amount);
            return success ? Ok("Transfer berhasil") : BadRequest("Transfer gagal");
        }
    }

}
