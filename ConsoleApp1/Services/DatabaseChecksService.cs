using StockControl.Data;

namespace StockControl.Services
{
    public class DatabaseChecksService
    {
        public DatabaseChecks _dataBaseChecks = new DatabaseChecks();

        public void CheckTableExists()
        {
            _dataBaseChecks.CheckTableExists();
        }
    }
}
