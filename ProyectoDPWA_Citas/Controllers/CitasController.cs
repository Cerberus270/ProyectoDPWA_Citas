using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoDPWA_Citas.Data;
using ProyectoDPWA_Citas.Models;

namespace ProyectoDPWA_Citas.Controllers
{
    [Authorize(Roles = "secretaria")]
    public class CitasController : Controller
    {
        private readonly ClinicaModContext _context;

        public CitasController(ClinicaModContext context)
        {
            _context = context;
        }

        // GET: CItas
        public async Task<IActionResult> Index()
        {
            var clinicaModContext = _context.Cita.Include(c => c.IdPacienteNavigation);
            return View(await clinicaModContext.ToListAsync());
        }

        
        // GET: CItas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cIta = await _context.Cita
                .Include(c => c.IdPacienteNavigation)
                .FirstOrDefaultAsync(m => m.IdCita == id);
            if (cIta == null)
            {
                return NotFound();
            }

            return View(cIta);
        }

        // GET: CItas/Create
        public IActionResult Create()
        {
            List<SelectListItem> nombresCompletos = RetornarNombreCompleto();
            ViewData["nombres"] = nombresCompletos;
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "IdPaciente", "Apellidos");

            return View();
        }

        // POST: CItas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCita,IdPaciente,Fecha,Hora,Estado")] CIta cIta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cIta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "IdPaciente", "Apellidos", cIta.IdPaciente);
            return View(cIta);
        }

        // GET: CItas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cIta = await _context.Cita.FindAsync(id);
            if (cIta == null)
            {
                return NotFound();
            }
            List<SelectListItem> nombresCompletos = RetornarNombreCompleto();
            ViewData["nombres"] = nombresCompletos;
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "IdPaciente", "Apellidos", cIta.IdPaciente);
            return View(cIta);
        }

        // POST: CItas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCita,IdPaciente,Fecha,Hora,Estado")] CIta cIta)
        {
            if (id != cIta.IdCita)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cIta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CItaExists(cIta.IdCita))
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
            List<SelectListItem> nombresCompletos = RetornarNombreCompleto();
            ViewData["nombres"] = nombresCompletos;
            ViewData["IdPaciente"] = new SelectList(_context.Pacientes, "IdPaciente", "Apellidos", cIta.IdPaciente);
            return View(cIta);
        }

        public async Task<IActionResult> Finalizar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cita = await _context.Cita
                .Include(c => c.IdPacienteNavigation)
                .FirstOrDefaultAsync(m => m.IdCita == id);
            if (cita == null)
            {
                return NotFound();
            }

            return View(cita);
        }

        [HttpPost, ActionName("Finalizar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Finalizar(int id)
        {
            var cita = await _context.Cita.FindAsync(id);
            cita.Estado = "Finalizada";
            _context.Cita.Update(cita);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: CItas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cIta = await _context.Cita
                .Include(c => c.IdPacienteNavigation)
                .FirstOrDefaultAsync(m => m.IdCita == id);
            if (cIta == null)
            {
                return NotFound();
            }

            return View(cIta);
        }

        // POST: CItas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cita = await _context.Cita.FindAsync(id);
            cita.Estado = "Cancelada";
            _context.Cita.Update(cita);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CItaExists(int id)
        {
            return _context.Cita.Any(e => e.IdCita == id);
        }

        private List<SelectListItem> RetornarNombreCompleto()
        {

            List<SelectListItem> items = new List<SelectListItem>();
            //Loop and add the Parent Nodes.
   
                //Loop and add the Items.
                foreach (Paciente subType in _context.Pacientes)
                {
                    string fullName = subType.Nombres + " " + subType.Apellidos;
                    items.Add(new SelectListItem
                    {
                        Value = subType.IdPaciente.ToString(),
                        Text = fullName,
                    });
                }
            return items;
        }
    }
}
