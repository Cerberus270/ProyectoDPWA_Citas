using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoDPWA_Citas.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoDPWA_Citas.Controllers
{
    public class DoctorController : Controller
    {
        private readonly ClinicaModContext _context;

        public DoctorController(ClinicaModContext context)
        {
            _context = context;
        }

        // GET: CItas
        public async Task<IActionResult> Index()
        {
            var clinicaModContext = _context.Cita.Where(c => c.Estado.Equals("Confirmada")).Include(c => c.IdPacienteNavigation);
            return View(await clinicaModContext.ToListAsync());
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

        private bool CItaExists(int id)
        {
            return _context.Cita.Any(e => e.IdCita == id);
        }
    }
}
