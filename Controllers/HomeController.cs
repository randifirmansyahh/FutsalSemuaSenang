using FutsalSemuaSenang.Models;
using FutsalSemuaSenang.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FutsalSemuaSenang.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult Login() => View();

        public IActionResult Daftar() => View();

        private readonly AppDbContext _context;
        private readonly OTPService otpSvc;

        public HomeController(AppDbContext c, OTPService otpSvc)
        {
            _context = c;
            this.otpSvc = otpSvc;
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Email,Password")] UserLogin data)
        {
            var user = _context.User
                .Where(x => x.Email == data.Email
                    && x.Password == data.Password)
                .Include(x => x.Role)
                .FirstOrDefault();

            if (user != null)
            {
                var claims = new List<Claim> {
                    new Claim("email", user.Email),
                    new Claim("role", user.Role.Name)
                };

                await HttpContext.SignInAsync(new ClaimsPrincipal(
                    new ClaimsIdentity(claims, "Cookies", "email", "role")
                    ));

                if (user.Role.Id == 1)
                {
                    return Redirect("/Admin");
                }
                else if (user.Role.Id == 2)
                {
                    return Redirect("/User");
                }

                return Redirect("/Home");
            }
            return View();
        }

        private int UserOtp;

        [HttpPost]
        public IActionResult KonfirmasiEmail([Bind("Name,Password,Email,Role")] UserForm data)
        {
            try
            {
                otpSvc.SetUser(data.Name, data.Email, data.Password);

                Random nilai = new Random();
                int otp = nilai.Next(1000, 9999);
                this.UserOtp = otp;

                MailMessage email = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                email.From = new MailAddress("futsalsemuasenang@gmail.com");
                email.To.Add(data.Email);
                email.Subject = "Konfirmasi Akun";
                email.Body = "Masukan kode " + otp + " untuk mengkonfirmasi akun pendaftaran anda di Website Futsal Semua Senang!";

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("futsalsemuasenang@gmail.com", "FutsalSemuaSenang!");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(email);
                ViewBag.Message = "Kode OTP telah dikirimkan ke email " + data.Email; 
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                ViewBag.Message = "Gagal mengirimkan OTP, tolong isi email anda dengan benar";
                return View("Daftar");
            }
            return View();
        }

        private void TambahUser()
        {
            string nama = "", email = "", password = "";
            otpSvc.GetUser(ref nama, ref email, ref password);
            var user = new User()
            {
                Name = nama,
                Password = password,
                Email = email,
                Status = true,
            };

            var role = _context.Roles
                    .FirstOrDefault(x => x.Id == 2);

            if (role != null)
            {
                user.Role = role;
            }

            _context.Add(user);

            _context.SaveChanges();
        }

        [HttpPost]
        public IActionResult Cek([Bind("otp")] Otp data)
        {
            if (data.KodeOtp == this.UserOtp)
            {
                TambahUser();
                return View("Login");
            }
            else
            {
                return View("KonfirmasiEmail");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}