using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gabinet.IRepository;
using Gabinet.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gabinet.Controllers
{
    [Route("Gabinet/{action}")]
    public class PlaceController : Controller
    {
        private  IUowRepository _uowRepo;
        public PlaceController(IUowRepository uowRepository)
        {
            _uowRepo = uowRepository;
        }

        [TempData]
        public string ResultAction { get; set; }


        // Get All Places
        // GET: /<controller>/
        public IActionResult Index() => View(_uowRepo.Repository<PlaceModel>().GetAll(null,x => x.Active == true));

        // View for Create and Edit Place
        // GET /CreatePlace?id={item}/
        public async Task<IActionResult> CreatePlace(int id = 0, string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (id == 0)
            {
                return View(new PlaceModel { });
            }
            PlaceModel item = await _uowRepo.Repository<PlaceModel>().FindById(id);

            if (item.Active == false)
            {
                return RedirectToAction(nameof(Index));
            }
            return item is null ? RedirectToAction(nameof(Index)) : View(item);
        }

        //Create or Edit place
        //POST /CreatePlace/
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> CreatePlace(PlaceModel createModel, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                if (createModel.PlaceId == 0)
                {
                    await _uowRepo.Repository<PlaceModel>().Create(createModel);
                    ResultAction = string.Format("Gabinet {0} został dodany poprawnie!", createModel.Name);
                    await _uowRepo.SaveChangesAsync();
                }
                else
                {
                    _uowRepo.Repository<PlaceModel>().Update(createModel);
                    ResultAction = "Dane gabinetu zostały zakutalizowane poprawnie!";
                    _uowRepo.SaveChanges();

                }
                
                return RedirectToAction(nameof(Index));

            }
            ViewBag.ReturnUrl = returnUrl;
            return View(createModel);
        }


        
        //Remove Place
        //Post /RemovePlace/
        public async Task<IActionResult> RemovePlace(int removeId)
        {
            PlaceModel item = await _uowRepo.Repository<PlaceModel>().FindById(removeId);
            if(item is null)
            {
                ResultAction = "Wystąpił błąd podczas wykonywania operacji!";
                return RedirectToAction(nameof(Index));
            }
            item.Name = string.Format("{0} - Gabinet nieaktywny", item.Name);
            item.Active = false;
            _uowRepo.Repository<PlaceModel>().Update(item);
           //_uowRepo.Repository<PlaceModel>().Delete(item);
            _uowRepo.SaveChanges();
            ResultAction = "Operacja przebiegła prawidłowo";
            return RedirectToAction(nameof(Index));
        }
    }


}
