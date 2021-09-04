using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FutsalSemuaSenang.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public string NamaLapangan { get; set; }
        public string Tanggal { get; set; }
        public string JamMulai { get; set; }
        public string JamSelesai { get; set; }
        public int Harga { get; set; }
        public bool Status { get; set; }
    }
    public class BookingForm
    {
        public string EmailUser { get; set; }
        public int IdUser { get; set; }
        public string NamaLapangan { get; set; }
        public string Tanggal { get; set; }
        public string JamMulai { get; set; }
        public string JamSelesai { get; set; }
        public int Durasi { get; set; }
        public int Harga { get; set; }
        public bool Status { get; set; }
    }
}