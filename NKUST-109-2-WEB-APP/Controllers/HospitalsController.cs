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
    }
}
