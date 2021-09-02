using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FutsalSemuaSenang.Services
{
    public class OTPService
    {
        public string Name;
        public string Email;
        public string Password;

        public void SetUser(string Name, string Email, string Password)
        {
            this.Name = Name;
            this.Email = Email;
            this.Password = Password;
        }

        public void GetUser(ref string Name, ref string Email, ref string Password)
        {
            Name = this.Name;
            Password = this.Password;
            Email = this.Email;
        }
    }
}