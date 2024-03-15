using Microsoft.AspNetCore.Mvc;
using MvcZapatillasExamen.Models;
using MvcZapatillasExamen.Repositories;

namespace MvcZapatillasExamen.Controllers
{
    public class PaginacionController : Controller
    {
        private RepositoryZapatillas repo;

        public PaginacionController(RepositoryZapatillas repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Zapatillas()
        {
            List<Zapatilla> zapatillas = await this.repo.GetZapatillasAsync();
            return View(zapatillas);
        }

        public async Task<IActionResult> ImagenesZapatilla(int? posicion, int idproducto)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            ModelImagenesZapasPaginacion model = await this.repo.GetImagenZapatillasAsync(posicion.Value, idproducto);
            Zapatilla zapatilla = await this.repo.FindZapatillaAsync(idproducto);
            ViewData["ZAPATILLASELECCIONADA"] = zapatilla;
            ViewData["ZAPATILLA"] = idproducto;
            ViewData["POSICION"] = posicion;
            return View(model.ImagenZapatilla);
        }

        public async Task<IActionResult> ImagenesDetailsPartial(int? posicion, int idproducto)
        {
            if (posicion == null)
            {
                return PartialView("_ImagenesZapasPartial");
            }
            ModelImagenesZapasPaginacion model = await this.repo.GetImagenZapatillasAsync(posicion.Value, idproducto);
            ViewData["REGISTROS"] = model.Registros;
            int siguiente = posicion.Value + 1;
            if (siguiente > model.Registros)
            {
                siguiente = model.Registros;
            }
            int anterior = posicion.Value - 1;
            if (anterior < 1)
            {
                anterior = 1;
            }

            ViewData["ULTIMO"] = model.Registros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            ViewData["POSICION"] = posicion;
            return PartialView("_ImagenesZapasPartial", model.ImagenZapatilla);
        }
    }
}
