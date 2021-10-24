using Biletall.Business.Abstract;
using Biletall.Core.Enums;
using Biletall.Core.Middleware.Caching;
using Biletall.Core.Utilities.Results;
using Biletall.DataAccess.EntityFramework.UnityOfWork;
using Biletall.DataAccess.Services.Abstraction;
using Biletall.Entities.Concrete;
using Biletall.Entities.Identity;
using Biletall.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biletall.Business.Concrete
{
    public class BusinessService : IBusinessService
    {
        private readonly IUoWBiletall _repository;
        private readonly IBiletallService _service;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICacheManager _cacheManager;

        public BusinessService(
            IUoWBiletall repository,
            IBiletallService service,
            UserManager<ApplicationUser> userManager,
            ICacheManager cacheManager)
        {
            _repository = repository;
            _service = service;
            _userManager = userManager;
            _cacheManager = cacheManager;
        }

        public IResult<VM_PnrIndex> Index()
        {
            SeedData();
            var model=new VM_PnrIndex();
            var data=(from user in _repository.Users
                      join res in _repository.Reservation.GetAll() on user.Id equals res.AspNetUserId
                      where !res.IsDeleted
                      select new {user,res })
                      .Select(x=>new PnrModel
                      {
                          PnrNo=x.res.PnrNo,
                          FullName=x.user.FullName,
                          Nereden=x.res.Nereden,
                          Nereye=x.res.Nereye,
                          Tarih=x.res.SeyahatTarihi,
                      }).ToList();
            model.PnrModels = data;

            return new Result<VM_PnrIndex>(model);
        }

        public IResult<object> KaranoktaList()
        {
            var isAlreadyExit=_cacheManager.IsAdd(CacheKeys.CityList);
            if (isAlreadyExit)
                return new Result<object>(_cacheManager.Get(CacheKeys.CityList));
            else
            {
                var data = new List<object>() { new { text = "Şehir Seçiniz", id = "0" } };
                var result=_service.KaraNoktaList().Result
                .Select(x=>new
                {
                    text = x.Ad,
                    id = x.ID
                }).Distinct().OrderBy(x => x.text).ToList();

                data.AddRange(result);
                _cacheManager.Add(CacheKeys.CityList, data, 60);
                return new Result<object>(data);
            }
        }

        public async Task<IResult<List<Guzergah>>> GuzergahList(string nereden, string nereye, DateTime tarih, string seferTakipNo)
        {
            var isAlreadyExit=_cacheManager.IsAdd(nereden+nereye+tarih+seferTakipNo);
            if (isAlreadyExit)
                return new Result<List<Guzergah>>(_cacheManager.Get<List<Guzergah>>(nereden + nereye + tarih + seferTakipNo));
            else
            {
                var data=await _service.GuzergahList(nereden,nereye,tarih,seferTakipNo);
                _cacheManager.Add(nereden + nereye + tarih + seferTakipNo, data, 60);
                return new Result<List<Guzergah>>(data);
            }
        }

        public async Task<IResult<List<Sefer>>> SeferList(string nereden, string nereye, DateTime tarih)
        {
            var key=nereden+nereye+tarih.ToString("mm/dd/yyyy");
            var isAlreadyExit=_cacheManager.IsAdd(key);
            if (isAlreadyExit)
                return new Result<List<Sefer>>(_cacheManager.Get<List<Sefer>>(key));
            else
            {
                var data=await _service.SeferList(nereden,nereye,tarih);
                _cacheManager.Add(key, data, 60);
                return new Result<List<Sefer>>(data);
            }
        }

        public async Task<IResult<List<Sefer>>> OrderByData(OrderByDataModel model)
        {
            var data =new List<Sefer>();
            if (model.Saat == "1")
                data = model.Sefers.OrderByDescending(x => x.KalkisSaati).ToList();
            else if (model.Fiyat == "1")
                data = model.Sefers.OrderByDescending(x => x.BiletFiyatiInternet).ToList();
            else if (model.OnePlus == "1")
                data = model.Sefers.Where(x => x.OtobusKoltukYerlesimTipi == "1+1").ToList();
            else
                data = model.Sefers.Where(x => x.OtobusKoltukYerlesimTipi == "2+1").ToList();
            return new Result<List<Sefer>>(data);
        }

        public IResult<ReservationModel> Search(String pnrNo)
        {

            var data=(from user in _repository.Users
                      join res in _repository.Reservation.GetAll() on user.Id equals res.AspNetUserId
                      where !res.IsDeleted &&
                      res.PnrNo==pnrNo
                      select new {user,res })
                      .Select(x=>new ReservationModel
                      {
                          Cinsiyet=((GenderType)x.user.GenderId).GetDescription(),
                          BiletIslemlerim="",
                          Durum=x.res.Durum==true ? "Rezervasyon" : "Rezervasyon İptal",
                          EBiletNo=x.res.EBiletNo,
                          Email=x.user.Email,
                          FirmaNo=x.res.FirmaNo,
                          KoltukNo=x.res.KoltukNo,
                          Nereden=x.res.Nereden,
                          Nereye=x.res.Nereye,
                          Peron=x.res.Peron,
                          PhoneNumber=x.user.PhoneNumber,
                          PnrNo=x.res.PnrNo,
                          SeferNo=x.res.SeferNo,
                          SeferTipi=x.res.SeferTipi,
                          ServisIstegi=x.res.ServisIstegi==true ? "Evet" : "Hayır",
                          SeyahatTarihi=x.res.SeyahatTarihi,
                          Ucret=x.res.Ucret,
                          YaklasikSeyehatSuresi=x.res.YaklasikSeyehatSuresi,
                          FullName=x.user.FullName
                      }).FirstOrDefault();

            return new Result<ReservationModel>(data);
        }
        #region PRIVATE OPERATIONS

        public void SeedData()
        {
            ApplicationUser user;

            if (_repository.Users.Count() == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    user = new ApplicationUser
                    {
                        UserName = FakeData.NameData.GetCompanyName(),
                        Email = FakeData.NetworkData.GetEmail(),
                        GenderId = FakeData.NumberData.GetNumber(1, 3),
                        FullName = FakeData.NameData.GetFullName(),
                        PhoneNumber = FakeData.PhoneNumberData.GetPhoneNumber()
                    };
                    _userManager.CreateAsync(user).GetAwaiter().GetResult();
                }
                _repository.Commit();
            }

            if (_repository.Reservation.Count() == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    var userId=_repository.Users.FirstOrDefault();
                    var model=new Reservation
                    {
                        AspNetUserId=userId.Id,
                        BiletIslemlerim="",
                        Durum=FakeData.BooleanData.GetBoolean(),
                        EBiletNo="",
                        FirmaNo="37",
                        IsDeleted=false,
                        KoltukNo=FakeData.NumberData.GetNumber(1,40),
                        Nereden=FakeData.PlaceData.GetCity(),
                        Nereye=FakeData.PlaceData.GetCity(),
                        Peron=FakeData.NumberData.GetNumber(1,40),
                        PnrNo=FakeData.NumberData.GetNumber(1000,9999).ToString(),
                        SeferNo=FakeData.NumberData.GetNumber(1000,9999).ToString(),
                        SeferTipi="MOLALI",
                        ServisIstegi=FakeData.BooleanData.GetBoolean(),
                        SeyahatTarihi=CustomTime(FakeData.DateTimeData.GetDatetime(DateTime.Now,DateTime.Now.AddDays(10)).ToString()),
                        Ucret=FakeData.NumberData.GetNumber(30,60).ToString(),
                        YaklasikSeyehatSuresi=FakeData.NumberData.GetNumber(1,5)+"-"+FakeData.NumberData.GetNumber(1,5),
                    };

                    _repository.Reservation.Insert(model);
                }
                _repository.Commit();
            }
        }

        public string CustomTime(string date)
        {
            var dateTime=Convert.ToDateTime(date);
            return dateTime.Day + " " + dateTime.ToString("MMM") + " " + dateTime.Year + " " + dateTime.DayOfWeek;
        }

        #endregion
    }
}
