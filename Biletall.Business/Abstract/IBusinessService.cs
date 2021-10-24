using Biletall.Core.Utilities.Results;
using Biletall.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Biletall.Business.Abstract
{
    public interface IBusinessService
    {
        IResult<VM_PnrIndex> Index();
        void SeedData();
        IResult<object> KaranoktaList();
        Task<IResult<List<Sefer>>> SeferList(string nereden, string nereye, DateTime tarih);
        Task<IResult<List<Guzergah>>> GuzergahList(string nereden, string nereye, DateTime tarih, string seferTakipNo);
        Task<IResult<List<Sefer>>> OrderByData(OrderByDataModel model);
        IResult<ReservationModel> Search(string pnrNo);
    }
}
