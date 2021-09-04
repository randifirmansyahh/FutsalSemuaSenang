using FutsalSemuaSenang.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FutsalSemuaSenang.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    public class HomeController : Controller
    {
        
        private readonly AppDbContext _context;

        public HomeController(AppDbContext c)
        {
            _context = c;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index([Bind("Tanggal,JamMulai,Durasi")] BookingForm data)
        {
            var cekBooking = _context.Booking.ToList().Where(x => x.Tanggal == data.Tanggal && x.JamMulai == data.JamMulai && x.Status==true);

            int n = cekBooking.Count();

            if(data.JamMulai == "13")
            {
                if (n >= 3)
                {
                    int nSelesai = 0;

                    foreach (var item in cekBooking)
                    {
                        if (item.JamSelesai == "15") nSelesai += 1;
                    }

                    if (nSelesai >= 3)
                    {
                        ViewBag.Message = "Semua lapangan terbooking pada tanggal tersebut, Coba pilih tanggal lain.";
                        return View();
                    }
                    else
                    {
                        ViewBag.Message = "Booking Penuh pada jam tersebut, Coba pilih jam lain";
                        return View();
                    }
                }
                else if (n == 2)
                {
                    return View("Cek");
                }
                else if (n == 1)
                {
                    return View("Cek");
                }
                else
                {
                    return View("Cek");
                }
            }
            else
            {
                int nSelesai = 0;
                
                foreach (var item in cekBooking)
                {
                    if (item.JamSelesai == "15") nSelesai += 1;
                }

                if (nSelesai >= 3)
                {
                    ViewBag.Message = "Semua lapangan terbooking pada tanggal dan jam tersebut";
                    return View();
                }
                else
                {
                    return View("Cek");
                }
            }
        }
    }
}