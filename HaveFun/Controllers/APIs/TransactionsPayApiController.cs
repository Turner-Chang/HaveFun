using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HaveFun.Models;
using HaveFun.DTOs;

namespace HaveFun.Controllers.APIs
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsPayApiController : ControllerBase
    {
        private readonly HaveFunDbContext _context;

        public TransactionsPayApiController(HaveFunDbContext context)
        {
            _context = context;
        }

        // GET: api/TransactionsPayApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentHistoryDTO>>> GetTransactions()
        {
            var transactions  = await _context.Transactions
                .Select(t => new PaymentHistoryDTO{ 
                    Date = t.Date,	
                    Product = t.Product, 
                    Amount = t.Amount
                }).ToListAsync();

            return transactions;
        }

        // GET: api/TransactionsPayApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentHistoryDTO>> GetTransaction(int id)
        {
            var transaction = await _context.Transactions
                .Where(t => t.Id == id)
                .Select(t => new PaymentHistoryDTO
                {
                    Date = t.Date,
                    Product = t.Product,
                    Amount = t.Amount
                }).FirstOrDefaultAsync();
            if (transaction == null)
            {
                return NotFound();
            }
            return transaction;
        }

		// GET: api/TransactionsPayApi/user/{userId}
		[HttpGet("user/{userId}")]
		public async Task<ActionResult<IEnumerable<PaymentHistoryDTO>>> GetTransactionsForUser(int userId)
		{
			// 根據 UserId 過濾資料
			var transactions = await _context.Transactions
				.Where(t => t.UserId == userId)
				.Select(t => new PaymentHistoryDTO
				{
					Date = t.Date,
					Product = t.Product, 
					Amount = t.Amount
					// 根據需求添加其他屬性
				})
				.ToListAsync();

			return transactions;
		}

		// PUT: api/TransactionsPayApi/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(int id, Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return BadRequest();
            }

            _context.Entry(transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TransactionsPayApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransaction", new { id = transaction.Id }, transaction);
        }

        // DELETE: api/TransactionsPayApi/5
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

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }
    }
}
