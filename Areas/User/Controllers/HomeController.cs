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
            int nSelesaiAll = 0;
            var cekAll = _context.Booking.ToList().Where(x => x.Tanggal == data.Tanggal && x.Status == true);

            //cek all
            foreach (var item in cekAll)
            {
                if (item.JamSelesai == "15") nSelesaiAll += 1;
            }

            var cekBooking = _context.Booking.ToList().Where(x => x.Tanggal == data.Tanggal && x.JamMulai == data.JamMulai && x.Status==true);

            int n = cekBooking.Count();

            int nSelesai = 0;

            //cek jam selesai 15
            foreach (var item in cekBooking)
            {
                if (item.JamSelesai == "15") nSelesai += 1;
            }

            //cek jam 13 saja
            if (data.JamMulai == "13" || nSelesaiAll >= 3)
            {
                if (n >= 3 || nSelesaiAll >= 3)
                {
                    if (nSelesai >= 3 || nSelesaiAll >=3)
                    {
                        ViewBag.Message = "Semua lapangan terbooking pada tanggal tersebut, Silahkan pilih tanggal lain.";
                        return View();
                    }
                    else
                    {
                        ViewBag.Message = "Booking Penuh pada jam tersebut, Silahkan pilih jam lain jika tersedia";
                        return View();
                    }
                }
                else if (n == 2)
                {
                    // create booking + kirim email
                    return View("Cek");
                }
                else if (n == 1)
                {
                    // create booking + kirim email
                    return View("Cek");
                }
                else
                {
                    // create booking + kirim email
                    return View("Cek");
                }
            }

            //cek jam 14.00
            else
            {
                if (n >= 3)
                {
                    ViewBag.Message = "Semua lapangan terbooking pada jam tersebut, Silahkan pilih jam lain.";
                    return View();
                }
                else
                {
                    //crate booking + kirim email
                    return View("Cek");
                }
            }
        }
    }
}