using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FastReport;
using FastReport.Data;
using FastReport.Export.PdfSimple;
using FastReport.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProyectoDPWA_Citas.Data;
using ProyectoDPWA_Citas.Models;

namespace ProyectoDPWA_Citas.Controllers
{
    [Authorize(Roles = "secretaria")]
    public class PacienteController : Controller
    {
        private readonly ClinicaModContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        public PacienteController(ClinicaModContext context, IWebHostEnvironment env, IConfiguration configuration)
        {
            _context = context;
            _env = env;
            _configuration = configuration;
        }


        // GET: Pacientes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pacientes.ToListAsync());
        }

        // GET: Pacientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(m => m.IdPaciente == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // GET: Pacientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pacientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPaciente,Nombres,Apellidos,FechaNacimiento,Edad,Direccion,Telefono")] Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paciente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(paciente);
        }

        // GET: Pacientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }
            return View(paciente);
        }

        // POST: Pacientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPaciente,Nombres,Apellidos,FechaNacimiento,Edad,Direccion,Telefono")] Paciente paciente)
        {
            if (id != paciente.IdPaciente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paciente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacienteExists(paciente.IdPaciente))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(paciente);
        }

        // GET: Pacientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(m => m.IdPaciente == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // POST: Pacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            _context.Pacientes.Remove(paciente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PacienteExists(int id)
        {
            return _context.Pacientes.Any(e => e.IdPaciente == id);
        }
        public IActionResult DetallesPacientes(string idPaciente)
        {
            var detallesPacientes = _context.Pacientes.Where(c => c.IdPaciente.Equals(int.Parse(idPaciente)));
            TempData["idPaciente"] = idPaciente;
            return PartialView("DetallesPacientes", detallesPacientes.ToList());
        }

        [HttpGet("idPaciente")]
        public IActionResult Generate()
        {
            var webReport = new WebReport();
            webReport.Report.Load(Path.Combine(_env.ContentRootPath, "Reports", "ReportePaciente.frx"));
            webReport.Report.Dictionary.Connections[0].ConnectionString = _configuration.GetConnectionString("Connection");
            var paciente = GetTable<Paciente>(_context.Pacientes, "Pacientes");
            webReport.Report.RegisterData(paciente, "Pacientes");
            webReport.Report.Prepare();
            Stream stream = new MemoryStream();
            webReport.Report.Export(new PDFSimpleExport(), stream);
            stream.Position = 0;
            // return stream in browser
            return File(stream, "application/zip", "reportePacientes.pdf");
        }
        static DataTable GetTable<TEntity>(IEnumerable<TEntity> table, string name) where TEntity : class
        {
            var offset = 78;
            DataTable result = new DataTable(name);
            PropertyInfo[] infos = typeof(TEntity).GetProperties();
            foreach (PropertyInfo info in infos)
            {
                if (info.PropertyType.IsGenericType
                && info.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    result.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType)));
                }
                else
                {
                    result.Columns.Add(new DataColumn(info.Name, info.PropertyType));
                }
            }
            

            return result;
        }

    }
}
