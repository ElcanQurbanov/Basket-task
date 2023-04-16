using Entity_Framework_Slider.Data;
using Entity_Framework_Slider.Models;
using Entity_Framework_Slider.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.ContentModel;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using System.Diagnostics;

namespace Entity_Framework_Slider.Controllers
{
	public class HomeController : Controller
	{
		private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
			_context = context;
        }


		[HttpGet]
        public async Task<IActionResult> Index()
		{

			int basketCount;

			if (Request.Cookies["basket"] != null)
			{
				basketCount = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]).Count;
			}
			else
			{
				basketCount = 0;
			}

			ViewBag.Count = basketCount;



			List<Slider> sliders = await _context.Sliders.ToListAsync();

            SliderInfo? sliderInfo = await _context.SliderInfos.FirstOrDefaultAsync();

			IEnumerable<Blog> blogs = await _context.Blogs.Where(m=> !m.SoftDelete).ToListAsync();

			IEnumerable<Category> categories = await _context.Categories.Where(m => !m.SoftDelete).ToListAsync();

			IEnumerable<Product> products = await _context.Products.Include(m=>m.Images).Where(m => !m.SoftDelete).ToListAsync();

			About? about = await _context.Abouts.Where(m => !m.SoftDelete).FirstOrDefaultAsync();


            IEnumerable<Advantage> advantages = await _context.Advantages.Where(m => !m.SoftDelete).ToListAsync();


            IEnumerable<Instagram> instagram = await _context.instagrams.Where(m => !m.SoftDelete).ToListAsync();

            IEnumerable<Say> says = await _context.says.Where(m => !m.SoftDelete).ToListAsync();




            List<int> nums = new List<int>() { 1, 2, 3, 4, 5, 6 };


			var res = nums.FirstOrDefault();
			ViewBag.num = res;

			HomeVM model = new()
			{
				Sliders = sliders,
				SliderInfo = sliderInfo,
				Blogs = blogs,
				Categories = categories,
				Products = products,
				Advantages = advantages,
				About = about,
				Instagrams = instagram,
				says = says,

			};
			
			return View(model);
		}


		[HttpPost]
		//[ValidateAntiForgeryToken]

		public async Task<IActionResult> AddBasket(int? id)
		{
			if (id is null) return BadRequest();

			Product? dbProduct = await GetProductById((int)id);

			if (dbProduct == null) return NotFound();

			List<BasketVM> basket = GetBasketDatas();

			BasketVM? existProduct = basket?.FirstOrDefault(m => m.Id == dbProduct.Id);

            AddProductToBasket(existProduct, dbProduct, basket);

			//return RedirectToAction(nameof(Index));

			//List <Product> productWm = new List<Product>();

			//List<BasketVM> basketVm = GetBasketDatas();
			//foreach (var item in basketVm)
			//{
			//	Product productModel = await _context.Products.Where(p=> !p.SoftDelete).Include(i=>i.Images).Include(c=>c.Category).FirstOrDefaultAsync(m=>m.Id == item.Id);

			//	productWm.Add(productModel);
			//}

			//return PartialView("_ProductsPartial", productWm);

			return Ok();

		}


		private async Task<Product> GetProductById(int id)
		{
            return await _context.Products.Include(m => m.Images).FirstOrDefaultAsync(m => m.Id == id);
        }

		private List<BasketVM> GetBasketDatas()
		{
            List<BasketVM> basket;

            if (Request.Cookies["basket"] != null)
            {
                basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            }
            else
            {
                basket = new List<BasketVM>();
            }

			return basket;
        }

		private void AddProductToBasket(BasketVM? existProduct, Product product, List<BasketVM> basket)
		{
            if (existProduct == null)
            {
                basket?.Add(new BasketVM
                {
                    Id = product.Id,
                    Count = 1,
                   
                });
            }
            else
            {
                existProduct.Count++;
               
            }



			Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));
		}




		
	}

	
}