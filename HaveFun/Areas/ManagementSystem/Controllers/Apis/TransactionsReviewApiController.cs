using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HaveFun.Models;
using HaveFun.Areas.ManagementSystem.DTOs;




namespace HaveFun.Areas.ManagementSystem.Controllers.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsReviewApiController : ControllerBase
    {
        private readonly HaveFunDbContext _context;

        public TransactionsReviewApiController(HaveFunDbContext context)
        {
            _context = context;
        }

        // GET: api/TransactionsReviewApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RefundReviewDTO>>> GetRefundRequests()
        {
            var refundRequests = await _context.Transactions
                .Where(t => t.Status == 3)
                .Select(t => new RefundReviewDTO
                {
                    Id = t.Id,
                    UserId = t.UserId,
                    Amount = t.Amount,
                    Product = t.Product,
                    Date = t.Date,
                    Status = t.Status,
                    Method = t.Method
                }).ToListAsync();

            return refundRequests;
        }

        // GET: api/TransactionsReviewApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        // PUT: api/TransactionsReviewApi/5/approve
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveRefund(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if(transaction == null)
            {
                return NotFound();
            }

            transaction.Status = 4; // 將狀態改為 4 (已退款)
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/TransactionsReviewApi/5/reject
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}/reject")]
        public async Task<IActionResult> RejectRefund(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            transaction.Status = 0; // 將狀態改為 0 (成功)
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/TransactionsReviewApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransaction", new { id = transaction.Id }, transaction);
        }

        // DELETE: api/TransactionsReviewApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("Count")]
        public async Task<int> Count() { 
        var c= _context.Transactions.Where(t=>t.Status==3).Count();
            return c;
        }


        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }
    }
}
