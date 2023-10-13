using CommonLayer.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepoLayer.Context
{
    public class ProductContext:DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> option) : base(option)
        {

        }
        public DbSet<ProductEntity> Store { get; set; }
        public void AddDetails(ProductModel model )
        {
            Database.ExecuteSqlRaw("EXEC SPProduct   @Code, @Name, @Description, @ExpiryDate, @Category, @Image,@Status,@CreationDate",
                new SqlParameter("@Code",model.Code),
                new SqlParameter("@Name", model.Name),
                new SqlParameter("@Description", model.Description),
                new SqlParameter("@ExpiryDate", model.ExpiryDate),
                new SqlParameter("@Category", model.Category),
                new SqlParameter("@Image", model.Image),
                new SqlParameter("@Status", model.Status),
                new SqlParameter("@CreationDate", model.CreationDate));
        }
      
    }
}
