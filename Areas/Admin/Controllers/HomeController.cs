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
    }
}