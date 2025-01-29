using StockControl.Data;
using StockControl.Models;

namespace StockControl.Services
{
    public class SaleService
    {
        private readonly SaleRepository _repository = new SaleRepository();

        public void AddSale(Sale sale)
        {
            _repository.AddSale(sale);
        }

        public List<SalesDTO> ListSales(int? productId)
        {
            return _repository.ListSales(productId);    
        }
    }
}
