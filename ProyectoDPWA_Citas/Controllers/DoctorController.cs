﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoDPWA_Citas.Data;
using ProyectoDPWA_Citas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoDPWA_Citas.Controllers
{
    [Authorize(Roles = "doctor")]
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
            var clinicaModContext = _context.Cita.Where(c => c.Estado.Equals("Confirmada") || c.Estado.Equals("Finalizada")).Include(c => c.IdPacienteNavigation);
            return View(await clinicaModContext.ToListAsync());
        }

        public async Task<IActionResult> Expediente(int? id)
        {
            ViewModelDiagnosticoRecetaCita viewModel = new ViewModelDiagnosticoRecetaCita();
            var pac = await _context.Pacientes
                .FirstOrDefaultAsync(m => m.IdPaciente == id);
            //var diagnosticos = _context.Diagnosticos.Where(c => c.IdCitaNavigation.IdPaciente == id).ToListAsync();
            /*
             * select C.idCita, P.nombres, P.apellidos, D.idDiagnostico, D.descripcion, R.idReceta, R.fechaPrescripcion from Receta R
               INNER JOIN Diagnostico D on R.idDiagnostico = D.idDiagnostico
               INNER JOIN Cita C on D.idCita = C.idCita
               INNER JOIN Paciente P on C.idPaciente = P.idPaciente
               where C.idPaciente = 1;
            */
            //var diagnosticos = (from r in _context.Receta
            //                    join d in _context.Diagnosticos on r.IdDiagnostico equals d.IdDiagnostico
            //                    join c in _context.Cita on d.IdCita equals c.IdCita
            //                    where c.IdPaciente == id
            //                    select d);

            var diagnosticos = (from d in _context.Diagnosticos
                                join c in _context.Cita on d.IdCita equals c.IdCita
                                join p in _context.Pacientes on c.IdPaciente equals p.IdPaciente
                                where c.IdPaciente == id
                                select d).ToList();
            var recetas = (from r in _context.Receta
                                join d in _context.Diagnosticos on r.IdDiagnostico equals d.IdDiagnostico
                                join c in _context.Cita on d.IdCita equals c.IdCita
                                join p in _context.Pacientes on c.IdPaciente equals p.IdPaciente
                                where c.IdPaciente == id
                                select r).ToList();
            List<ViewModelDiagnosticoReceta> listaDiagnosticos = new List<ViewModelDiagnosticoReceta>();
            for(int i = 0; i < diagnosticos.Count; i++)
            {
                ViewModelDiagnosticoReceta modelDiagnosticoReceta = new ViewModelDiagnosticoReceta();
                modelDiagnosticoReceta.diagnostico = diagnosticos[i];
                modelDiagnosticoReceta.receta = recetas[i];
                listaDiagnosticos.Add(modelDiagnosticoReceta);
            }
            viewModel.Paciente = pac;
            viewModel.modelDiagnosticoRecetas = listaDiagnosticos;
            //viewModel.diagnosticos = await diagnosticos.ToListAsync();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult DetallesRecetas(string idReceta)
        {
            var detallesRecetas = _context.DetallesReceta.Where(c => c.IdReceta.Equals(int.Parse(idReceta)));
            TempData["idReceta"] = idReceta;
            return PartialView("DetallesRecetas", detallesRecetas.ToList());
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
