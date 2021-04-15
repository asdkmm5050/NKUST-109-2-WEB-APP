using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NKUST_109_2_WEB_APP.Data;
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
        public async Task<IActionResult> Index()
        {
            return View(await _context.Hospitals.ToListAsync());
        }
    }
}
