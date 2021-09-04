using FutsalSemuaSenang.Models;
using FutsalSemuaSenang.Services;
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

        private readonly IdUserService idUserSvc;

        public HomeController(AppDbContext c, IdUserService idUserSvc)
        {
            _context = c;
            this.idUserSvc = idUserSvc;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index([Bind("Tanggal,JamMulai,Durasi")] BookingForm data)
        {
            string pesanFull = "Semua lapangan terbooking pada tanggal tersebut, Silahkan pilih tanggal lain.";
            string pesanJamFull = "Semua lapangan terbooking pada Jam tersebut, Silahkan pilih Jam lain.";

            int JM13 = 0;
            int JM14 = 0;
            int JS14 = 0;
            int JS15 = 0;
            int cekFull = 0;

            //cek all by date
            var cekAll = _context.Booking.ToList().Where(x => x.Tanggal == data.Tanggal && x.Status == true);
            foreach (var item in cekAll)
            {
                if (item.JamMulai == "13" && item.JamSelesai == "15") cekFull += 1;

                if (item.JamMulai == "13") JM13 += 1;

                if (item.JamMulai == "14") JM14 += 1;

                if (item.JamSelesai == "14") JS14 += 1;

                if (item.JamSelesai == "15") JS15 += 1;
            }

            //Is Full ?
            if (cekFull >= 3)
            {
                ViewBag.Message = pesanFull;
                return View();
            }
            else if(JM13 >=3 && JS15 >= 3)
            {
                //pesan full
                ViewBag.Message = pesanFull;
                
                if (JM14 >= 3 && JS14 >= 3)
                {
                    return View();
                }
                else if (JM14 >= 2 && JS14 >= 2)
                {
                    return View();
                }
                else if (JM14 >= 1 && JS14 >= 1)
                {
                    return View();
                }
            }

            //cek jam full
            ViewBag.Message = pesanJamFull;
            if (data.JamMulai == "13" && JM13 >= 3)
            {
                return View();
            }
            else if (data.JamMulai == "14" && JM14 >= 3)
            {
                return View();
            }

            //cek jangan 2 jam kalo penuh
            if (data.Durasi >= 2 && JM14 >= 3) 
            {
                ViewBag.Message = "Anda hanya dapat memesan 1 Jam, karna di jam berikutnya telah di booking oleh orang lain";
                return View();
            }

            //set Id User
            int Id = 0;
            idUserSvc.GetIdUser(ref Id);
            
            //disini push db

            //disini kirim email

            return View("Cek");
        }
    }
}