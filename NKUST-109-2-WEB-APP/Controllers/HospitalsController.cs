using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NKUST_109_2_WEB_APP.Data;
using NKUST_109_2_WEB_APP.Models;
using Microsoft.EntityFrameworkCore;

namespace NKUST_109_2_WEB_APP.Controllers
{
    public class HospitalsController : Controller
    {
        private readonly HospitalDbContext _context;

        public HospitalsController(HospitalDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["AddressSortParm"] = String.IsNullOrEmpty(sortOrder) ? "address_desc" : "";
            ViewData["TelephoneSortParm"] = String.IsNullOrEmpty(sortOrder) ? "telephone_desc" : "";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var hospitals = from i in _context.Hospitals
                            select i;
            if (!String.IsNullOrEmpty(searchString))
            {
                hospitals = hospitals.Where(i => i.Name.Contains(searchString)
                                       || i.Address.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    hospitals = hospitals.OrderBy(i => i.Name.Length);
                    break;
                case "address_desc":
                    hospitals = hospitals.OrderBy(i => i.Address.Length);
                    break;
                case "telephone_desc":
                    hospitals = hospitals.OrderBy(i => i.Telephone);
                    break;
                default:
                    hospitals = hospitals.OrderBy(i => i.Name);
                    break;
            }
            int pageSize = 6;
            return View(await PaginatedList<Hospital>.CreateAsync(hospitals.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hospital = await _context.Hospitals
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (hospital == null)
            {
                return NotFound();
            }

            return View(hospital);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Address,Telephone")] Hospital hospital)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(hospital);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(hospital);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            var hospital = await _context.Hospitals
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            return View(hospital);
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var hospitalToUpdate = await _context.Hospitals.FirstOrDefaultAsync(s => s.ID == id);
#pragma warning disable HAA0101 // Array allocation for params parameter
            if (await TryUpdateModelAsync(
                hospitalToUpdate,
                "",
                s => s.Name, s => s.Address, s => s.Telephone))
#pragma warning restore HAA0101 // Array allocation for params parameter
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(hospitalToUpdate);
        }
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Hospitals
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(student);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
#pragma warning disable HAA0101 // Array allocation for params parameter
            var student = await _context.Hospitals.FindAsync(id);
#pragma warning restore HAA0101 // Array allocation for params parameter
            if (student == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Hospitals.Remove(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }
    }
}
