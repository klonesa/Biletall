using Biletall.Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Biletall.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Reservations = new HashSet<Reservation>();
        }

        public string FullName { get; set; }
        public int GenderId { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}
