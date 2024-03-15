using Microsoft.AspNetCore.Mvc;
using MvcZapatillasExamen.Models;
using MvcZapatillasExamen.Repositories;

namespace MvcZapatillasExamen.Controllers
{
    public class ImagenesZapasController : Controller
    {
        private List<Zapatilla> zapatillas;
        private RepositoryZapatillas repo;

        public ImagenesZapasController(RepositoryZapatillas repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Zapatillas()
        {
            List<Zapatilla> zapatillas = await this.repo.GetZapatillasAsync();
            return View(zapatillas);
        }

        public async Task<IActionResult> InsertarImagen()
        {
            List<Zapatilla> zapatillas = await this.repo.GetZapatillasAsync();
            return View(zapatillas);
        }

        [HttpPost]
        public IActionResult InsertImagen(int idproducto, List<string> imagen)
        {
            foreach (var item in imagen)
            {

                this.repo.InsertarImagenes(idproducto, imagen);
            }
            return RedirectToAction("InsertarImagen");
        }
    }
}
