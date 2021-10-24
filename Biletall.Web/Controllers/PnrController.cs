using Biletall.Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Biletall.Web.Controllers
{
    public class PnrController : Controller
    {
        private readonly IBusinessService _businessService;

        public PnrController(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        public IActionResult Index()
        {
            var data=_businessService.Index();

            return View(data.Data);
        }

        public IActionResult Search(string pnrNo)
        {
            var data=_businessService.Search(pnrNo).Data;

            return Json(data);
        }
    }
}
