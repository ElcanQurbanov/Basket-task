using Entity_Framework_Slider.Data;
using Entity_Framework_Slider.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Entity_Framework_Slider.Areas.Admin.Controllers
{
    [Area("Admin")]
     public class SliderController : Controller
    {
        private readonly AppDbContext _context;

        public SliderController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Slider> sliders = await _context.Sliders.Where(m=>!m.SoftDelete).ToListAsync();
            return View(sliders);
        }
    }
}
