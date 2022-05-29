using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoDPWA_Citas.Controllers
{
    public class ErroresController : Controller
    {
        public IActionResult Http(int statusCode)
        {  
            if (statusCode == 404)
                return View("Error404");
            return View();
        }
    }
}
