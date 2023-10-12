using CommonLayer.Models;
using Microsoft.EntityFrameworkCore;
using RepoLayer.Context;
using RepoLayer.Entity;
using RepoLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepoLayer.Services
{
    public class RepoProduct:IRepoProduct
    {
        private readonly ProductContext _context;
        public RepoProduct(ProductContext context)
        {
            this._context = context;
        }
        public IEnumerable<ProductEntity> GetProducts()
        {
            //var products = new List<ProductEntity>();
            var result = _context.Store.FromSqlRaw("SPRetrieveAllData").ToList();
            return result;
        }
        public bool AddProduct(ProductModel model)
        {
            try
            {
                 _context.AddDetails(model);
                
                return true; 
            }
            catch (Exception ex)
            {
               
                return false;
            }
        }
        public bool DeleteProduct(int productId)
        {
            try
            {
                // Find the product in the database by its ProductId.
                var productToDelete = _context.Store.SingleOrDefault(p => p.ProductId == productId);

                if (productToDelete != null)
                {
                    // Remove the product from the context and mark it for deletion.
                    _context.Store.Remove(productToDelete);

                    // Save the changes to the database.
                    _context.SaveChanges();

                    return true;
                }

                return false; // Product not found or couldn't be deleted.
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during the deletion process.
                return false;
            }
        }

    }
}
