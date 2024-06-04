using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HaveFun.Models;

namespace HaveFun.Controllers
{
    public class MemberProfilesController : Controller
    {
        private readonly HaveFunDbContext _context;

        public MemberProfilesController(HaveFunDbContext context)
        {
            _context = context;
        }

        // GET: MemberProfiles
        public async Task<IActionResult> Index()
        {
            return View(await _context.MemberProfile.ToListAsync());
        }

        // GET: MemberProfiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberProfile = await _context.MemberProfile
                .FirstOrDefaultAsync(m => m.MemberProfileId == id);
            if (memberProfile == null)
            {
                return NotFound();
            }

            return View(memberProfile);
        }

        // GET: MemberProfiles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MemberProfiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberProfileId,Nickname,Location,Birthday,Occupation,Interests,Introduction")] MemberProfile memberProfile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(memberProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(memberProfile);
        }
        //POST: MemberProfiles/Image/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MemberProfile Images, IFormFile myimg)
        {
            if (ModelState.IsValid)
            {
                if (myimg != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        myimg.CopyTo(ms);
                        Images.Image = ms.ToArray();
                    }
                }
                _context.Add(Images);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Images"] = new SelectList(
                _context.Set<MemberProfile>(), "MemberProfileId", "Nickname", Images.Image);
            return View(Images);
        }

        // GET: MemberProfiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberProfile = await _context.MemberProfile.FindAsync(id);
            if (memberProfile == null)
            {
                return NotFound();
            }
            return View(memberProfile);
        }

        // POST: MemberProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemberProfileId,Nickname,Location,Birthday,Occupation,Interests,Introduction")] MemberProfile memberProfile)
        {
            if (id != memberProfile.MemberProfileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(memberProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberProfileExists(memberProfile.MemberProfileId))
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
            return View(memberProfile);
        }

        // GET: MemberProfiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberProfile = await _context.MemberProfile
                .FirstOrDefaultAsync(m => m.MemberProfileId == id);
            if (memberProfile == null)
            {
                return NotFound();
            }

            return View(memberProfile);
        }

        // POST: MemberProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var memberProfile = await _context.MemberProfile.FindAsync(id);
            if (memberProfile != null)
            {
                _context.MemberProfile.Remove(memberProfile);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberProfileExists(int id)
        {
            return _context.MemberProfile.Any(e => e.MemberProfileId == id);
        }
    }
}
