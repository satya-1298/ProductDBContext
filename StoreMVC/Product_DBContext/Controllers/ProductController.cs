using CommonLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Product_DBContext.Models;
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
        public IActionResult Index(string search,string sorting, int page=1)
        {
            try
            {
                var products = repoProduct.GetProducts();
                ViewBag.NameSortParm = String.IsNullOrEmpty(sorting) ? "name_desc" : "";
                ViewBag.EDateSortParm = sorting == "Edate_asc" ? "Edate_desc" : "Edate_asc";
                ViewBag.CodeSort = sorting == "code_asc" ? "code_desc" : "code_asc";
                ViewBag.DescriptionSort = sorting == "description_asc" ? "description_desc" : "description_asc";
                ViewBag.CategorySort = sorting == "category_asc" ? "category_desc" : "category_asc";
                ViewBag.StatusSort = sorting == "status_asc" ? "status_desc" : "status_asc";
                ViewBag.CDateSort = sorting == "CDate_asc" ? "CDate_desc" : "CDate_asc";
                ViewBag.ImageSort = sorting == "Image_asc" ? "Image_desc" : "Image_asc";
                ViewBag.Search = search;
                //Search
                if (!string.IsNullOrEmpty(search))
                {
                    if (DateTime.TryParse(search,out DateTime searchdate))
                    {
                        products = products.Where(p => p.ExpiryDate.Date == searchdate.Date || p.CreationDate.Date == searchdate.Date).ToList();
                    }
                    else
                    {
                        products = products.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase) || p.Code.Contains(search, StringComparison.OrdinalIgnoreCase)
                        || p.Category.Contains(search, StringComparison.OrdinalIgnoreCase) || p.Status.Equals(search, StringComparison.OrdinalIgnoreCase)
                        || p.Description.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
                    }

                }

                switch (sorting)
                {
                    case "name_desc":
                        products = products.OrderByDescending(s => s.Name);
                        break;
                    case "code_desc":
                        products = products.OrderByDescending(s => s.Code);
                        break;
                     
                    case "code_asc":
                        products = products.OrderBy(s => s.Code);
                        break;
                    case "Edate_asc":
                        products = products.OrderBy(s => s.ExpiryDate);
                        break;
                    case "Edate_desc":
                        products = products.OrderByDescending(s => s.ExpiryDate);
                        break;
                    case "description_desc":
                        products = products.OrderByDescending(s => s.Description);
                        break;

                    case "description_asc":
                        products = products.OrderBy(s => s.Description);
                        break;
                    case "category_desc":
                        products = products.OrderByDescending(s => s.Category);
                        break;

                    case "category_asc":
                        products = products.OrderBy(s => s.Category);
                        break;
                    case "status_desc":
                        products = products.OrderByDescending(s => s.Status);
                        break;
                        
                    case "status_asc":
                        products = products.OrderBy(s => s.Status);
                        break;
                    case "Image_desc":
                        products = products.OrderByDescending(s => s.Image);
                        break;

                    case "Image_asc":
                        products = products.OrderBy(s => s.Image);
                        break;

                    case "CDate_desc":
                        products = products.OrderByDescending(s => s.CreationDate);
                        break;

                    case "CDate_asc":
                        products = products.OrderBy(s => s.CreationDate);
                        break;

                    default:
                        products = products.OrderBy(s => s.Name);
                        break;
                }

               

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

                //pagination
                const int pSize = 3;
                if (page < 1)
                    page = 1;
                int count = result.Count();
                var pager = new Pagination(count, page, pSize);
                int skip = (page - 1) * pSize;
                var data = result.Skip(skip).Take(pager.PageSize).ToList();
                this.ViewBag.Pagination = pager;
                //return View(result);
                return View(data);
            }
            catch
            {
                return View("Error", "An error occurred while retrieving products. Please try again later  ");
            }
        
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
            try
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
            catch
            {
                return View("Error", "An error occurred while Upset products. Please try again later ");
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
              

                return RedirectToAction("Index", new { success = 1 });
            }
            catch 
            {
                return View("Error", "Product Not found. Please try again later ");
            }
        }


    }
}

//Search
//if (!string.IsNullOrEmpty(search))
//{
//    if (DateTime.TryParseExact(search, "dd-MM-yyyy", CultureInfo
//    .InvariantCulture, DateTimeStyles.None, out DateTime searchDate))
//    {
//        products = products.Where(p => p.ExpiryDate.Date == searchDate.Date || p.CreationDate.Date == searchDate.Date).ToList();
//    }
//    else
//    {
//        products = products.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase) || p.Code.Contains(search, StringComparison.OrdinalIgnoreCase)
//        || p.Category.Contains(search, StringComparison.OrdinalIgnoreCase) || p.Status.Equals(search, StringComparison.OrdinalIgnoreCase)
//        || p.Description.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
//    }

//}

////Sorting date 
//switch (sorting)
//{
//    case "desc":
//        products = products.OrderByDescending(p => p.CreationDate).ToList();
//        break;
//    case "asc":
//        products = products.OrderBy(p => p.CreationDate).ToList();
//        break;
//    default:
//        products = products.OrderByDescending(p => p.CreationDate).ToList();
//        break;
//}
// Convert ProductEntity objects to ProductModel objects
