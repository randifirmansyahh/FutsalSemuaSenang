using FutsalSemuaSenang.Models;
using FutsalSemuaSenang.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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

            int Alpha = 0;
            int Beta = 0;
            int Charlie = 0;

            //cek all by date
            var cekAll = _context.Booking.ToList().Where(x => x.Tanggal == data.Tanggal && x.Status == true);
            foreach (var item in cekAll)
            {
                //cek jam mulai dan jam selesai
                if (item.JamMulai == "13" && item.JamSelesai == "15") cekFull += 1;

                if (item.JamMulai == "13") JM13 += 1;

                if (item.JamMulai == "14") JM14 += 1;

                if (item.JamSelesai == "14") JS14 += 1;

                if (item.JamSelesai == "15") JS15 += 1;

                //cek nama lapangan
                if (item.NamaLapangan == "Alpha" && item.JamMulai == "13" && item.JamSelesai == "15") Alpha += 2;
                else if (item.NamaLapangan == "Beta" && item.JamMulai == "13" && item.JamSelesai == "15") Beta += 2;
                else if (item.NamaLapangan == "Charlie" && item.JamMulai == "13" && item.JamSelesai == "15") Charlie += 2;

                if (item.NamaLapangan == "Alpha") Alpha += 1;
                else if (item.NamaLapangan == "Beta") Beta += 1;
                else if (item.NamaLapangan == "Charlie") Charlie += 1;
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

            //set Nama lapangan
            string NamaLapangKosong = "";

            //cari nama lapangan yang kosong
            if (Alpha < 2) NamaLapangKosong = "Alpha";
            else if (Beta < 2) NamaLapangKosong = "Beta";
            else if (Charlie < 2) NamaLapangKosong = "Charlie";

            //jam selesai
            int jamSelesainya = Int16.Parse(data.JamMulai) + data.Durasi;

            //harga
            int harga = 100000 * data.Durasi;

            //disini push db
            var booking = new Booking()
            {
                IdUser = Id,
                NamaLapangan = NamaLapangKosong,
                Tanggal = data.Tanggal,
                JamMulai = data.JamMulai,
                JamSelesai = jamSelesainya.ToString(),
                Harga = harga,
                Status = false,
            };

            _context.Add(booking);

            _context.SaveChanges();

            //disini kirim email
            Random nilai = new Random();
            int KodeBooking = nilai.Next(10000000, 99999999);

            //cari dulu usernya
            var findUser = _context.User.Find(Id);

            MailMessage email = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            email.From = new MailAddress("futsalsemuasenang@gmail.com");
            email.To.Add(findUser.Email);
            email.Subject = "Order Booking Berhasil !";
            email.Body =
                "<h1 style='color:blue;'>Selamat, Order booking anda telah berhasil !\nTunggu email selanjutnya untuk konfirmasi dari kami.</h1><br><br>" +
                "<table>" +
                    "<tr>" +
                        "<td>" +
                            "Kode Booking" +
                        "</td>" +
                        "<td><h3>" +
                            KodeBooking +
                        "</h3></td>" +
                    "<tr>" +
                    "<tr>" +
                        "<td>" +
                            "Nama Pembooking" +
                        "</td>" +
                        "<td><h3>" +
                            findUser.Name +
                        "</h3></td>" +
                    "<tr>" +
                    "<tr>" +
                        "<td>" +
                            "Nama Lapangan" +
                        "</td>" +
                        "<td><h3>" +
                            NamaLapangKosong +
                        "</h3></td>" +
                    "<tr>" +
                    "<tr>" +
                        "<td>" +
                            "Jam Mulai" +
                        "</h3>" +
                        "<td><h3>" +
                            data.JamMulai +
                        "</h3></td>" +
                    "<tr>" +
                    "<tr>" +
                        "<td>" +
                            "Jam Selesai" +
                        "</td>" +
                        "<td><h3>" +
                            jamSelesainya +
                        "</h3></td>" +
                    "<tr>" +
                    "<tr>" +
                        "<td>" +
                            "Harga" +
                        "</td>" +
                        "<td><h3>" +
                            harga +
                        "</h3></td>" +
                    "<tr>" +
                    "<tr>" +
                        "<td>" +
                            "Status" +
                        "</td>" +
                        "<td>" +
                            "<h2 style='color:blue;'>Berhasil</h2>" +
                        "</td>" +
                    "<tr>" +
                "<table><br><br>" +
                "<a href='mailto:futsalsemuasenang@gmail.com?subject=Bantuan&body=Halo'>Membutuhkan bantuan dari kami ?</a><br><br>" +
                "<a href='https://localhost:5001'>Kunjungi website ?</a><br>";

            email.IsBodyHtml = true;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("futsalsemuasenang@gmail.com", "FutsalSemuaSenang!");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(email);

            ViewBag.Message = NamaLapangKosong;
            return View();
        }
    }
}