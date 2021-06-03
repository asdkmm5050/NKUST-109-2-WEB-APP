using Microsoft.AspNetCore.Http;
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
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly HospitalDbContext dbContext;

        public ValuesController(HospitalDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("{name?}")]
        public Task<List<Hospital>> GetVolumeDatas(string? name)
        {
            IQueryable<Hospital> query = dbContext.Hospitals.AsQueryable();
            if (name != null)
            {
                query = query.Where(vd => vd.Name == name);
            }
            return query.ToListAsync();
        }
    }
}
