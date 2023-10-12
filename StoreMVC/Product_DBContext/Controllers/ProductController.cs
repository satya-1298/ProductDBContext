using CommonLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using RepoLayer.Entity;
using RepoLayer.Interfaces;
using System;
using System.Globalization;
using System.Linq;

namespace Product_DBContext.Controllers
{
    public class ProductController : Controller
    {
        private readonly IRepoProduct repoProduct;
        public ProductController(IRepoProduct repoProduct)
        {
            this.repoProduct = repoProduct;
        }


        /// <summary>
        /// Index Method in which i was implementing  to view the all the products in list formate
        /// in this i implemented searching products by name using lambda function 
        /// and sorting technique to display in our prefered order
        /// </summary>
        /// <param name="search"></param>
        /// <param name="sorting"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index(string search,string sorting)
        {
            var products = repoProduct.GetProducts();

            //Search
            if (!string.IsNullOrEmpty(search))
            {
                if (DateTime.TryParseExact(search, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime searchDate))
                {
                    products = products.Where(p => p.ExpiryDate.Date == searchDate.Date).ToList();
                }
                else
                {
                    products = products.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
                }
            }

            //Sorting date 
            switch (sorting)
            {
                case "name_desc":
                    products = products.OrderByDescending(p => p.ExpiryDate).ToList();
                    break;
                case "name_asc":
                    products = products.OrderBy(p => p.ExpiryDate).ToList();
                    break;
                default:
                    products = products.OrderByDescending(p => p.ExpiryDate).ToList();
                    break;
            }
            // Convert ProductEntity objects to ProductModel objects
            var result = products.Select(ProductEntity => new ProductModel
            {
                ProductId = ProductEntity.ProductId,
                Code = ProductEntity.Code,
                Name = ProductEntity.Name,
                Description = ProductEntity.Description,
                ExpiryDate = ProductEntity.ExpiryDate,
                Category = ProductEntity.Category,
                Image = ProductEntity.Image,
                Status = ProductEntity.Status,
                CreationDate = ProductEntity.CreationDate
            });
           
            return View(result);
        }

      



        /// <summary>
        /// Creates Product and add that to database 
        /// </summary>
        /// <param name="model"></param>
        /// <returns> Go to Index Method to view List of Products</returns>
        [HttpPost]
        public IActionResult Create(ProductModel model) 
        {
            try
            {               
                    repoProduct.AddProduct(model);
                    return RedirectToAction("Index");              
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        /// <summary>
        /// Updating the Product details  here iam calling AddProduct 
        /// because i only want two pages which is Listing and adding  by giving 
        /// AddProduct when we enter same ID it will Update otherwise it will create
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Update/create product into database and go to index to view the list</returns>
        [HttpPost]
        public IActionResult Edit(ProductModel model) 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repoProduct.AddProduct(model);
                    return RedirectToAction("Index");
                }
                return View(model);
            }
            catch
            {
                throw;
            }
        }
        public IActionResult Edit()
        {
            return View();
        }
        public IActionResult Delete(int id)
        {
            try
            {
                repoProduct.DeleteProduct(id);
                if(true)
                {

                    ViewBag.AlertMessage = "Your alert message here.";
                    return View();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                throw;
            }
        }
        
    }
}
