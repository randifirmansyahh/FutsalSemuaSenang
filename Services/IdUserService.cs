using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FutsalSemuaSenang.Services
{
    public class IdUserService
    {
        public int Id;
        
        public void SetIdUser(int Id)
        {
            this.Id= Id;
        }

        public void GetIdUser(ref int Id)
        {
            Id = this.Id;
        }
    }
}