using Microsoft.AspNetCore.Mvc;
using ProyectoDPWA_Citas.Models;
using ProyectoDPWA_Citas.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ProyectoDPWA_Citas.Controllers
{
    public partial class DoctorController : Controller
    {
        #region"Funciones para cumplir cita"
        private bool isCitaEjecutada(CIta cita)
        {
            return cita.Estado.Equals("Completada") ||
                cita.Estado.Equals("Finalizada") ? false: true;
        }
        public async Task<IActionResult> Cita_Execution_Oppenning(int citaId)
        {
            Cita_Execution viewmodel = new Cita_Execution();
            CIta cita = await _context.Cita.Where(c=> c.IdCita == citaId).FirstOrDefaultAsync();
            //si se intenta volver a ejecutar una cita completada o finalizada
            if (!isCitaEjecutada(cita))
            {
                return NotFound();
            }

            //este codigo no es necesario aca (solo de referencia)
            /*IEnumerable<SelectListItem> estadosCita = new List<SelectListItem>() {
                new SelectListItem("Pendiente", "Pendiente"),
                new SelectListItem("Confirmada", "Confirmada"),
                new SelectListItem("Completada", "Completada"),
                new SelectListItem("Finalizada", "Finalizada")
            };*/

            viewmodel.cita = cita;
            viewmodel.diagnostico = cita.DiagnosticoCurrent;
            viewmodel.receta = cita.DiagnosticoCurrent.RecetaCurrent;

            return View("~/Views/Doctor/Cita_Execution.cshtml", viewmodel);
        }

        public async Task<IActionResult> Cita_Execution_Save(Cita_Execution citaExecution)
        {
            string retorno;

            if(citaExecution == null
                || citaExecution.cita == null
                || citaExecution.cita.IdCita < 0)
            {
                retorno = "No es posible guardar los cambios";
                return new JsonResult(retorno);
            }

            try
            {
                await _context.Database.BeginTransactionAsync();

                citaExecution.diagnostico.IdCita = citaExecution.cita.IdCita;
                citaExecution.cita = await _context.Cita.Include(c => c.IdPacienteNavigation)
                    .Where(c => c.IdCita == citaExecution.cita.IdCita).FirstOrDefaultAsync();
                if (!isCitaEjecutada(citaExecution.cita))
                {
                    return NotFound();
                }
                citaExecution.cita.Estado = "Completada";
                _context.Add(citaExecution.diagnostico);
                await _context.SaveChangesAsync();

                Receta receta = new Receta();
                receta.IdDiagnostico = citaExecution.diagnostico.IdDiagnostico;
                receta.FechaPrescripcion = DateTime.Now;
                _context.Add(receta);
                await _context.SaveChangesAsync();

                foreach (DetallesReceta dr in citaExecution.detallesReceta)
                {
                    dr.IdReceta = receta.IdReceta;
                    _context.Add(dr);
                }
                await _context.SaveChangesAsync();

                await _context.Database.CommitTransactionAsync();
                retorno = "Exito";
            }
            catch (Exception e)
            {
                await _context.Database.RollbackTransactionAsync();
                retorno = "ha ocurrido un error!";
            }

            return new JsonResult(retorno);
        }
        #endregion "Funciones para cumplir cita"
    }
}
