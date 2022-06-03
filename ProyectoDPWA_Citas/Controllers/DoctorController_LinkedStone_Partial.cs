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
        public async Task<IActionResult> Cita_Execution_Oppenning(int citaId)
        {
            Cita_Execution viewmodel = new Cita_Execution();
            CIta cita = await _context.Cita.Where(c=> c.IdCita == citaId).FirstOrDefaultAsync();

            //este codigo no es necesario aca
            /*IEnumerable<SelectListItem> estadosCita = new List<SelectListItem>() {
                new SelectListItem("Pendiente", "Pendiente"),
                new SelectListItem("Confirmada", "Confirmada"),
                new SelectListItem("Completada", "Completada"),
                new SelectListItem("Finalizada", "Finalizada")
            };*/

            viewmodel.cita = cita;
            viewmodel.diagnostico = cita.DiagnosticoCurrent;
            viewmodel.receta = cita.DiagnosticoCurrent.RecetaCurrent;

            //ViewBag.estadosCita = estadosCita;
            return View("~/Views/Doctor/Cita_Execution.cshtml", viewmodel);
        }

        public async Task<IActionResult> Cita_Execution_Save(Cita_Execution citaExecution)
        {
            string retorno;

            try
            {
                await _context.Database.BeginTransactionAsync();

                citaExecution.diagnostico.IdCita = citaExecution.cita.IdCita;
                _context.Add(citaExecution.diagnostico);
                await _context.SaveChangesAsync();

                Receta receta = new Receta();
                receta.IdDiagnostico = citaExecution.diagnostico.IdDiagnostico;
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
