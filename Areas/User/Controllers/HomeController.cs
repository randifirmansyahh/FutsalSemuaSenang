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
        public IActionResult Cek([Bind("Tanggal,Jam,Durasi")] Booking data)
        {
            var cekBooking = _context.Booking.ToList().Where(x => x.Tanggal == data.Tanggal && x.Status==true);
            if (cekBooking == null)
            {
                return View();
            }

            ViewBag.Message = "Sudah ada pembooking pada tanggal tersebut";
            return RedirectToAction("Index");
        }
    }
}