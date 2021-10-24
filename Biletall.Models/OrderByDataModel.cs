using System.Collections.Generic;

namespace Biletall.Models
{
    public class OrderByDataModel
    {
        public string Saat { get; set; }
        public string Fiyat { get; set; }
        public string OnePlus { get; set; }
        public string TwoPlus { get; set; }
        public List<Sefer> Sefers { get; set; }
    }
}
