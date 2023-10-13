using CommonLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using RepoLayer.Entity;
using RepoLayer.Interfaces;
using System;
using System.Collections.Generic;
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
                if (DateTime.TryParseExact(search, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime searchDate))
                {
                    products = products.Where(p => p.ExpiryDate.Date == searchDate.Date||p.CreationDate.Date==searchDate.Date).ToList();
                }
                else
                {
                    products = products.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase) || p.Code.Contains(search, StringComparison.OrdinalIgnoreCase)
                    ||p.Category.Contains(search, StringComparison.OrdinalIgnoreCase) ||p.Status.Equals(search, StringComparison.OrdinalIgnoreCase)
                    ||p.Description.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
                }

            }

            //Sorting date 
            switch (sorting)
            {
                case "desc":
                    products = products.OrderByDescending(p => p.CreationDate).ToList();
                    break;
                case "asc":
                    products = products.OrderBy(p => p.CreationDate).ToList();
                    break;
                default:
                    products = products.OrderByDescending(p => p.CreationDate).ToList();
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
        /// creating and  Updating the Product details  here iam calling AddProduct 
        /// because i only want two pages which is Listing and adding  by giving 
        /// AddProduct when we enter same ID it will Update otherwise it will create
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Update/create product into database and go to index to view the list</returns>
       
        public IActionResult  AddEdit(string code)
        {
            List<ProductEntity> list = repoProduct.GetProducts().ToList();
            ProductEntity entity = list.SingleOrDefault(e => e.Code == code);

            if (entity != null)
            {
                ProductModel model = new ProductModel
                {
                    ProductId = entity.ProductId,
                    Code = entity.Code,
                    Name = entity.Name,
                    Description = entity.Description,
                    ExpiryDate = entity.ExpiryDate,
                    Category = entity.Category,
                    Image = entity.Image,
                    Status = entity.Status,
                    CreationDate = entity.CreationDate
                };

                return View(model);
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public IActionResult AddEdit(ProductModel model)
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

        /// <summary>
        /// Deleting the Product by ProductId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Delete(int id)
        {
            try
            {
                var productToDelete = repoProduct.ProductByID(id).SingleOrDefault();

                if (productToDelete != null)
                {
                    repoProduct.DeleteProduct(productToDelete.ProductId);

                }
              

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
