using FutsalSemuaSenang.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FutsalSemuaSenang.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext c)
        {
            _context = c;
            var role = _context.Roles.FirstOrDefault(x => x.Id == 1);
        }

        public IActionResult Index()
        {
            var data = _context.Booking.ToList();
            return View(data);
        }

        public IActionResult Terkonfirmasi()
        {
            var data = _context.Booking.ToList().Where(x => x.Status == true);
            return View(data);
        }

        public IActionResult BelumTerkonfirmasi()
        {
            var data = _context.Booking.ToList().Where(x => x.Status == false);
            return View(data);
        }

        public IActionResult Konfirmasi(Booking data)
        {
            if (ModelState.IsValid)
            {
                var findBooking = _context.Booking.Find(data.Id);

                var findUser = _context.User.Find(findBooking.IdUser);

                if (findBooking == null)
                {
                    return NotFound();
                }

                findBooking.Status = true;

                _context.Update(findBooking);

                _context.SaveChanges();

                Random nilai = new Random();
                int KodeBooking = nilai.Next(10000000, 99999999);

                MailMessage email = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                email.From = new MailAddress("futsalsemuasenang@gmail.com");
                email.To.Add(findUser.Email);
                email.Subject = "Order Booking Berhasil !";
                email.Body =
                    "<h1 style='color:green;'>Selamat, Order booking anda telah berhasil !</h1><br><br>" +
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
                                "ID Booking" +
                            "</td>" +
                            "<td><h3>" +
                                findBooking.Id +
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
                                findBooking.NamaLapangan +
                            "</h3></td>" +
                        "<tr>" +
                        "<tr>" +
                            "<td>" +
                                "Jam Mulai" +
                            "</h3>" +
                            "<td><h3>" +
                                findBooking.JamMulai +
                            "</h3></td>" +
                        "<tr>" +
                        "<tr>" +
                            "<td>" +
                                "Jam Selesai" +
                            "</td>" +
                            "<td><h3>" +
                                findBooking.JamSelesai +
                            "</h3></td>" +
                        "<tr>" +
                        "<tr>" +
                            "<td>" +
                                "Harga" +
                            "</td>" +
                            "<td><h3>" +
                                findBooking.Harga +
                            "</h3></td>" +
                        "<tr>" +
                        "<tr>" +
                            "<td>" +
                                "Status" +
                            "</td>" +
                            "<td>" +
                                "<h2 style='color:green;'>Berhasil</h2>" +
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

                //cari booking yang sama dengan konfirmasi
                var findSame = _context.Booking.ToList().Where(x => x.Tanggal==findBooking.Tanggal && x.JamMulai == findBooking.JamMulai && x.Id != findBooking.Id);

                //membatalkan semua order yang sama dengan konfirmasi
                foreach(var item in findSame)
                {
                    //hapus booking yang sama dengan konfirmasi
                    var hapus = _context.Booking.Find(item.Id);
                    _context.Remove(hapus);
                    _context.SaveChanges();

                    //cari user
                    var cariUser = _context.User.Find(item.IdUser);

                    //kirim email pembatalan
                    email.To.Add(cariUser.Email);
                    email.Subject = "Order Booking Ditolak oleh Admin !";
                    email.Body =
                        "<h1 style='color:red;'>Mohon maaf, Order booking anda telah ditolak oleh admin !</h1><br><br>" +
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
                                    "ID Booking" +
                                "</td>" +
                                "<td><h3>" +
                                    hapus.Id +
                                "</h3></td>" +
                            "<tr>" +
                            "<tr>" +
                                "<td>" +
                                    "Nama Pembooking" +
                                "</td>" +
                                "<td><h3>" +
                                    cariUser.Name +
                                "</h3></td>" +
                            "<tr>" +
                            "<tr>" +
                                "<td>" +
                                    "Nama Lapangan" +
                                "</td>" +
                                "<td><h3>" +
                                    hapus.NamaLapangan +
                                "</h3></td>" +
                            "<tr>" +
                            "<tr>" +
                                "<td>" +
                                    "Jam Mulai" +
                                "</h3>" +
                                "<td><h3>" +
                                    hapus.JamMulai +
                                "</h3></td>" +
                            "<tr>" +
                            "<tr>" +
                                "<td>" +
                                    "Jam Selesai" +
                                "</td>" +
                                "<td><h3>" +
                                    hapus.JamSelesai +
                                "</h3></td>" +
                            "<tr>" +
                            "<tr>" +
                                "<td>" +
                                    "Harga" +
                                "</td>" +
                                "<td><h3>" +
                                    hapus.Harga +
                                "</h3></td>" +
                            "<tr>" +
                            "<tr>" +
                                "<td>" +
                                    "Status" +
                                "</td>" +
                                "<td>" +
                                    "<h2 style='color:red;'>Ditolak</h2>" +
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
                }

                return RedirectToAction("BelumTerkonfirmasi");
            }

            return View();
        }

        public IActionResult BatalkanKonfirmasi(Booking data)
        {
            if (ModelState.IsValid)
            {
                var findBooking = _context.Booking.Find(data.Id);

                var findUser = _context.User.Find(findBooking.IdUser);

                if (findBooking == null)
                {
                    return NotFound();
                }

                findBooking.Status = false;

                _context.Update(findBooking);

                _context.SaveChanges();

                Random nilai = new Random();
                int KodeBooking = nilai.Next(10000000, 99999999);

                MailMessage email = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                email.From = new MailAddress("futsalsemuasenang@gmail.com");
                email.To.Add(findUser.Email);
                email.Subject = "Order Booking Dibatalkan !";
                email.Body =
                    "<h1 style='color:red;'>Mohon maaf, Order booking anda telah dibatalkan oleh admin, Silahkan untuk order kembali !</h1><br><br>" +
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
                                "ID Booking" +
                            "</td>" +
                            "<td><h3>" +
                                findBooking.Id +
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
                                findBooking.NamaLapangan +
                            "</h3></td>" +
                        "<tr>" +
                        "<tr>" +
                            "<td>" +
                                "Jam Mulai" +
                            "</h3>" +
                            "<td><h3>" +
                                findBooking.JamMulai +
                            "</h3></td>" +
                        "<tr>" +
                        "<tr>" +
                            "<td>" +
                                "Jam Selesai" +
                            "</td>" +
                            "<td><h3>" +
                                findBooking.JamSelesai +
                            "</h3></td>" +
                        "<tr>" +
                        "<tr>" +
                            "<td>" +
                                "Harga" +
                            "</td>" +
                            "<td><h3>" +
                                findBooking.Harga +
                            "</h3></td>" +
                        "<tr>" +
                        "<tr>" +
                            "<td>" +
                                "Status" +
                            "</td>" +
                            "<td>" +
                                "<h2 style='color:red;'>Dibatalkan</h2>" +
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

                return RedirectToAction("Terkonfirmasi");
            }

            return View();
        }

        //select Roles supaya hanya user yang tampil
        private Roles IdAdmin()
        {
            var role = _context.Roles.FirstOrDefault(x => x.Id == 1);
            return role;
        }

        public IActionResult AllUser()
        {
            var data = _context.User.ToList().Where(x => x.Role != IdAdmin());
            return View(data);
        }

        public IActionResult Aktif()
        {
            var data = _context.User.ToList().Where(x => x.Status == "1" && x.Role != IdAdmin());
            return View(data);
        }

        public IActionResult NonAktif()
        {
            var role = _context.Roles.FirstOrDefault(x => x.Id == 1);
            var data = _context.User.ToList().Where(x => x.Status == "0" && x.Role != IdAdmin());
            return View(data);
        }

        public IActionResult Aktifkan(UserId data)
        {
            if (ModelState.IsValid)
            {
                var finduser = _context.User.Find(data.Id);

                if (finduser == null)
                {
                    return NotFound();
                }

                finduser.Status = "1";

                _context.Update(finduser);

                _context.SaveChanges();

                return RedirectToAction("NonAktif");
            }

            return View();
        }

        public IActionResult NonAktifkan(UserId data)
        {
            if (ModelState.IsValid)
            {
                var finduser = _context.User.Find(data.Id);

                if (finduser == null)
                {
                    return NotFound();
                }

                finduser.Status = "0";

                _context.Update(finduser);

                _context.SaveChanges();

                return RedirectToAction("Aktif");
            }

            return View();
        }

        public IActionResult InputBooking()
        {
            return View();
        }

        [HttpPost]
        public IActionResult InputBooking([Bind("EmailUser,Tanggal,JamMulai,Durasi")] BookingForm data)
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
            else if (JM13 >= 3 && JS15 >= 3)
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

            //set Id User dari Admin
            Random IdRandom = new Random();
            int Id = IdRandom.Next(10000000, 99999999);
            
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
                Status = true,
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
            email.To.Add(data.EmailUser);
            email.Subject = "Order Booking Berhasil !";
            email.Body =
                "<h1 style='color:green;'>Selamat, Order booking anda telah berhasil !\n</h1><br><br>" +
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
                            "Email Pembooking" +
                        "</td>" +
                        "<td><h3>" +
                            data.EmailUser +
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
                            "<h2 style='color:green;'>Berhasil</h2>" +
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

            return Redirect("Terkonfirmasi");
        }
    }
}