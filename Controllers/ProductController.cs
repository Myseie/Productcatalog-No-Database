using Microsoft.AspNetCore.Mvc;
using productcatalog.Models;

namespace productcatalog.Controllers
{
    public class ProductController : Controller
    {
        private static List<Product> Products = new List<Product>();
        private static readonly List<string> Categories = new List<string> {"Elektronik", "Böcker", "Kläder", "Hem & Kök"};

        [HttpGet]
        public IActionResult Index(string sortOrder)
        {
            var sortedProducts = sortOrder switch
            {
                "price" => Products.OrderBy(p => p.Price).ToList(),
                "category" => Products.OrderBy(p => p.Category).ToList(),
                _ => Products.OrderBy(p => p.Name).ToList(),
            };

            ViewBag.TotalProducts = sortedProducts.Count;
            ViewBag.AveragePrice = sortedProducts.Count > 0 ? sortedProducts.Average(p => p.Price) : 0;
            return View(sortedProducts);
        }


        [HttpGet]

        public IActionResult Create()
        {
            ViewBag.Categories = Categories;
            return View();
        }

        [HttpPost]

        public IActionResult Create(string name, string description, decimal price, string category)
        {
            if(string .IsNullOrEmpty(name) || string.IsNullOrEmpty(description) || price <= 0 || string.IsNullOrEmpty(category))
            {
                ViewBag.Error = "Alla fälten måste vara fyllda, och priset måste va högre än 0 ";
            }

            Products.Add(new Product
            {
                Name = name,
                Description = description,
                Price = price,
                Category = category
            });

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id >= 0 && id < Products.Count)
            {
                ViewBag.Categories = Categories;
                var product = Products[id];
                ViewBag.Id = id;
                return View(product);
            }

            return RedirectToAction("Index"); 
        }

        [HttpPost]
        public IActionResult Edit(int id, string name, string description, decimal price, string category)
        {
            if (id >= 0 &&  (id < Products.Count) && !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(description) && price > 0 && !string.IsNullOrEmpty(category) )
            {
                Products[id].Name = name;
                Products[id].Description = description;
                Products[id].Price = price;
                Products[id].Category = category;

            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if(id >= 0 && id < Products.Count)
            {
                Products.RemoveAt(id);
            }
            return RedirectToAction("Index");

        }




    }
}
