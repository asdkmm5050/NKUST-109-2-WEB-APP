using NKUST_109_2_WEB_APP.Models;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace NKUST_109_2_WEB_APP.Data
{
    public static class DbInitializer
    {
        public static void Initialize(HospitalDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Hospitals.Any())
            {
                return;   // DB has been seeded
            }
            var reader = new StreamReader(File.OpenRead("C:\\Code\\.net\\NKUST-109-2-WEB-APP\\NKUST-109-2-WEB-APP\\data.csv"), System.Text.Encoding.Default);
            List<Hospital> hospitals = new();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(",");
                var hospital = new Hospital { Name = values[0], Address = values[1], Telephone = values[2] };
                hospitals.Add(hospital);
            }
            foreach (Hospital i in hospitals)
            {
                context.Hospitals.Add(i);
            }
            context.SaveChanges();
        }
    }
}