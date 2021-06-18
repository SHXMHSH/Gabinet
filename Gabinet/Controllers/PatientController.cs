using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gabinet.Data;
using Gabinet.Extension;
using Gabinet.IRepository;
using Gabinet.Models;
using Gabinet.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gabinet.Controllers
{
    public class PatientController : Controller
    {
        private IUowRepository _uowRepo;
        private GabinetDbContext _dbContext;
        public PatientController(IUowRepository uowRepo, GabinetDbContext dbContext)
        {
            _uowRepo = uowRepo;
            _dbContext = dbContext;
        }

        [TempData]
        public string ResultAction { get; set; }

        //Get AllPatient
        // GET: /<controller>/
        public IActionResult Index(int page = 1, int itemOnPage = 10)
        {
            ViewBag.MaxItem = itemOnPage;
            return View(new PagingVModel<PatientModel>
            {
                GetCollection = _uowRepo.Repository<PatientModel>().GetAll().Paging(page, itemOnPage),
                GetPaging = new PagingModel
                {
                    CountItem = _uowRepo.Repository<PatientModel>().GetAll().Count(),
                    ItemOnPage = itemOnPage,
                    CurrentPage = page
                }
            });
        }

        public IActionResult PatientVisits(int id = 0)
        {
            if (id == 0)
            {
                return RedirectToAction(nameof(Index));
            }
            IEnumerable<AppointmentModel> appointments = _dbContext.GetAppointment.Where(x => x.PatientId == id).Include(x => x.User).Include(x => x.Patient);
            return appointments is null ? RedirectToAction(nameof(Index)) : View(appointments);
        }

        // View for Create and Edit Patient
        // GET /CreatePatient?id={item}/
        public async Task<IActionResult> CreatePatient(int id = 0)
        {
            if (id == 0)
            {
                return View(new PatientModel { });
            }
            PatientModel item = await _uowRepo.Repository<PatientModel>().FindById(id);

            return item is null ? RedirectToAction(nameof(Index)) : View(item);
        }

        //Create or Edit Patient
        //POST /Createpatient/
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> CreatePatient(PatientModel patientModel, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                if (patientModel.PatientId == 0)
                {
                    PatientModel item = await _uowRepo.Repository<PatientModel>().FindById(patientModel.PatientId);
                    if (item is null)
                    {
                        await _uowRepo.Repository<PatientModel>().Create(patientModel);
                        ResultAction = "Pacjent został dodany poprawnie";
                        await _uowRepo.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Pacjent istnieje w bazie danych");
                    }
                }
                else
                {
                    _uowRepo.Repository<PatientModel>().Update(patientModel);
                    ResultAction = "Dane pacjenta zostały zakutalizowane poprawnie!";
                    _uowRepo.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }

            }

            return View(patientModel);
        }

        //Remove Patient
        //Post /RemovePlace/
        public async Task<IActionResult> RemovePatient(int patientId)
        {
            PatientModel item = await _uowRepo.Repository<PatientModel>().FindById(patientId);
            if (item is null)
            {
                ResultAction = "Wystąpił błąd podczas wykonywania operacji!";
                return RedirectToAction(nameof(Index));
            }
            IEnumerable<AppointmentModel> appointment = _uowRepo.Repository<AppointmentModel>()?.GetAll(null, x => x.PatientId == patientId);
            if (appointment.Count() == 0)
            {
                _uowRepo.Repository<PatientModel>().Delete(item);
                _uowRepo.SaveChanges();
                ResultAction = "Operacja przebiegła prawidłowo";
                return RedirectToAction(nameof(Index));
            }
            ResultAction = "Wystąpił błąd podczas wykonywania operacji! Skontaktuj się z administratorem";
            return RedirectToAction(nameof(Index));
        }

    }
}
