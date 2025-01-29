using ConsoleApp1.Data;
using ConsoleApp1.Models;

namespace ConsoleApp1.Services
{
    public class ProductService
    {
        private readonly ProductRepository _repository = new ProductRepository();

        public void CheckTableExists()
        {
            _repository.CheckTableExists();
        }

        public void InsertProduct(Product product)
        {
            _repository.InsertProduct(product);
        }

        public List<Product> GetAll()
        {
            return _repository.GetAll();
        }

        public List<Product> GetProductByName(string name)
        {
            return _repository.GetByName(name);
        }

        public void UpdateProduct(Product product)
        {
            _repository.UpdateProduct(product);
        }
        public void DeleteProduct(int id)
        {
            _repository.DeleteProduct(id);
        }

        public void UpdateProductStock(int stockToDeduce, int productId)
        {
            _repository.UpdateProductStock(stockToDeduce, productId);
        }
    }
}
