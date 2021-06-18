using Gabinet.Data;
using Gabinet.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gabinet.Controllers
{
    public class AcceptVisitController : Controller
    {
        private GabinetDbContext _dbContext;
        private readonly UserManager<IdentityModel> _userManager;
        public AcceptVisitController(UserManager<IdentityModel> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            IdentityModel currentUser = await _userManager.GetUserAsync(this.User);
            int userId = (int)currentUser?.UserId;
            IEnumerable<AppointmentModel> appointments = _dbContext.GetAppointment.Where(x => x.PatientId == userId).Include(x => x.User).Include(x => x.Patient);
            return appointments is null ? RedirectToAction(nameof(Index)) : View(appointments);
        }
    }
}
