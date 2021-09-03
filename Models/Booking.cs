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
        public DateTime Tanggal { get; set; }
        public DateTime JamMulai { get; set; }
        public DateTime JamSelesai { get; set; }
        public int Harga { get; set; }
        public bool Status { get; set; }
    }
    public class BookingForm
    {
        public int IdUser { get; set; }
        public string NamaLapangan { get; set; }
        public DateTime Tanggal { get; set; }
        public DateTime JamMulai { get; set; }
        public DateTime JamSelesai { get; set; }
        public int Harga { get; set; }
        public bool Status { get; set; }
    }
}