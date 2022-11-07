using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Newtonsoft.Json;
using Tutoring_Platform.Models;
using Tutoring_Platform.CustomModels;
using Microsoft.AspNetCore.Authorization;

namespace Tutoring_Platform.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private tutoringContext db;
        public AdminController(tutoringContext dbModel)
        {
            db = dbModel;
        }
    }
}
