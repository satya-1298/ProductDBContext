using CommonLayer.Models;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepoLayer.Interfaces
{
    public interface IRepoProduct
    {
        public IEnumerable<ProductEntity> GetProducts();
        public bool AddProduct(ProductModel model);
        public bool DeleteProduct(int productId);


    }
}
