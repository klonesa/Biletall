using Biletall.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Biletall.DataAccess.Services.Abstraction
{
    public interface IBiletallService
    {
        Task<List<KaraNokta>> KaraNoktaList();
        Task<List<Sefer>> SeferList(string nereden, string nereye, DateTime tarih);
        Task<List<Guzergah>> GuzergahList(string nereden, string nereye, DateTime tarih, string seferTakipNo);
    }
}
