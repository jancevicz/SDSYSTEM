using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SDSYSTEM.Data;
using SDSYSTEM.Models;

namespace SDSYSTEM.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Tickets.Include(t => t.AssignedTo).Include(t => t.CreatedBy).Include(t => t.Status);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.AssignedTo)
                .Include(t => t.CreatedBy)
                .Include(t => t.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewData["AssignedToId"] = new SelectList(_context.Users, "Id", "Email");
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Email");
            ViewData["StatusId"] = new SelectList(_context.TicketStatuses, "Id", "Id");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Department,FullName,AssignedToId,StatusId,CreatedAt,ResolvedAt")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.CreatedAt = DateTime.Now;
                ticket.StatusId = 1; // Ustaw status na "Nieprzydzielone"
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Populate ViewData with necessary data for dropdowns
            ViewData["AssignedToId"] = new SelectList(_context.Users, "Id", "Email");
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Email");
            ViewData["StatusId"] = new SelectList(_context.TicketStatuses, "Id", "Id", ticket.StatusId);

            return View(ticket);
        }


        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["AssignedToId"] = new SelectList(_context.Users, "Id", "Email", ticket.AssignedToId);
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Email", ticket.CreatedById);
            ViewData["StatusId"] = new SelectList(_context.TicketStatuses, "Id", "Id", ticket.StatusId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Department,FullName,StatusId,AssignedToId,CreatedById,CreatedAt,ResolvedAt")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ticket.AssignedToId.HasValue && ticket.StatusId == 1)
                    {
                        ticket.StatusId = 2; // Ustaw status na "Przydzielone"
                    }

                    if (ticket.ResolvedAt.HasValue)
                    {
                        ticket.StatusId = 3; // Ustaw status na "Wykonane"
                    }

                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssignedToId"] = new SelectList(_context.Users, "Id", "Email", ticket.AssignedToId);
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Email", ticket.CreatedById);
            ViewData["StatusId"] = new SelectList(_context.TicketStatuses, "Id", "Id", ticket.StatusId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.AssignedTo)
                .Include(t => t.CreatedBy)
                .Include(t => t.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}
