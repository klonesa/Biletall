using Biletall.Core.Domain.Entities;
using Biletall.Entities.Identity;
using System;

namespace Biletall.Entities.Concrete
{
    public class Reservation : EntityBase<int>, IEntity, ISoftDelete
    {
        public String AspNetUserId { get; set; }
        public String Nereden { get; set; }
        public String Nereye { get; set; }
        public String SeyahatTarihi { get; set; }
        public String PnrNo { get; set; }
        public String FirmaNo { get; set; }
        public String YaklasikSeyehatSuresi { get; set; }
        public String Ucret { get; set; }
        public Boolean Durum { get; set; }
        public int KoltukNo { get; set; }
        public String EBiletNo { get; set; }
        public Boolean ServisIstegi { get; set; }
        public String BiletIslemlerim { get; set; }
        public String SeferTipi { get; set; }
        public String SeferNo { get; set; }
        public int Peron { get; set; }
        public Boolean IsDeleted { get; set; }


        public ApplicationUser ApplicationUser { get; set; }
    }
}
