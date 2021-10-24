using Biletall.Business.Abstract;
using Biletall.Models;
using Biletall.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace Biletall.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBusinessService _businessService;

        public HomeController(ILogger<HomeController> logger,
            IBusinessService businessService)
        {
            _logger = logger;
            _businessService = businessService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult OtobusSefer(string nereden, string nereye, string tarih)
        {
            var data=_businessService.SeferList(nereden,nereye,Convert.ToDateTime(tarih)).GetAwaiter().GetResult().Data;

            return Json(data);
        }

        [HttpGet]
        public IActionResult CityList()
        {
            var data=_businessService.KaranoktaList().Data;

            return Json(data);
        }

        [HttpPost]
        public IActionResult OrderByData(OrderByDataModel model)
        {
            var data= _businessService.OrderByData(model).GetAwaiter().GetResult();
            return Json(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
