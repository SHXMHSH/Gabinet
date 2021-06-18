using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gabinet.Extension;
using Gabinet.IRepository;
using Gabinet.Models;
using Gabinet.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gabinet.Controllers
{
    public class AppointmentController : Controller
    {
        private IUowRepository _uowRepo;
        private readonly UserManager<IdentityModel> _userManager;
        public AppointmentController(IUowRepository uowRepo, UserManager<IdentityModel> userManager) {
            _uowRepo = uowRepo;
            _userManager = userManager;
        }

        [TempData]
        public string ResultAction { get; set; }

        // GET: /<controller>/
        public IActionResult Index(int page = 1, int itemOnPage = 15, int place = 0, string date = null)
        {
            ViewBag.MaxItem = itemOnPage;

            if (date is null)
            {
                date = DateTime.Now.ToShortDateString();
            }

            //IdentityModel currentUser = await _userManager.GetUserAsync(this.User);
            //int placeUser = currentUser.User.PlaceId;


            IEnumerable<AppointmentModel> appointmentList = place == 0 ?
                _uowRepo.Repository<AppointmentModel>().GetAll(x => x.User, null,x=>x.Patient).Paging(page, itemOnPage):
                _uowRepo.Repository<AppointmentModel>().GetAll(x => x.User, x => x.PlaceId == place && x.Date.Date.ToShortDateString() == date, x => x.Patient ).Paging(page, itemOnPage);

            return View(new PagingVModel<AppointmentModel>
            {
                GetCollection = appointmentList,
                GetPaging = new PagingModel
                {
                    CountItem = appointmentList.Count(),
                    CurrentPage = page,
                    ItemOnPage = itemOnPage
                }
            }
             );

        }
        public ViewResult Date(int userId = 0)
        {
            ViewBag.userId = userId;
         return   View();
        }

        public ViewResult Register(int userId, string registerDate)
        {
            string newdate = registerDate.ToString();
            return View();
        }

        public IActionResult Create(int placeId = 0 ,string returnUrl = null)
        {
            
           PlaceModel place = placeId == 0 ? null : _uowRepo.Repository<PlaceModel>().FindById(placeId);

            if(place is null)
            {
                return RedirectToAction(nameof(Index));

            }
            place = null;
            ViewBag.ReturnUrl = returnUrl;
            return View(new AppointmentVModel { GetAppointment = new AppointmentModel { },
                                                GetUser = _uowRepo.Repository<UserModel>().GetAll(null, x => x.PlaceId == placeId),
                                                GetPatient = null
            });

        }
        

    }
}
