using CommonLayer.Models;
using Microsoft.Data.SqlClient;
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
        public IEnumerable<ProductEntity> ProductByID(int productId)
        {
        
             var result=_context.Store.FromSqlRaw("EXEC SPRetrieveByID @ProductId", new SqlParameter("@ProductId", productId))
                .ToList();
            return result;
        }
        public bool AddProduct(ProductModel model)
        {
            try
            {
                 _context.AddDetails(model);
                _context.SaveChanges();
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
                var parameters = new SqlParameter("@ProductId", productId);

                _context.Database.ExecuteSqlRaw("SPDelete @ProductId", parameters);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
      
    }

}

