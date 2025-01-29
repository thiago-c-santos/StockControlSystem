using ConsoleApp1.Models;
using ConsoleApp1.Services;
using StockControl.Models;
using StockControl.Services;
using System.Globalization;

class StockControlMain
{
    private readonly static ProductService _productService = new ProductService();
    private readonly static SaleService _saleService = new SaleService();   
    private readonly static EmployeeService _employeeService = new EmployeeService();   

    static void Main()
    {
        ProductService _service = new ProductService();
        
        _service.CheckTableExists();

        Console.WriteLine("------------------ Gerenciador de estoque ------------------");
        Console.WriteLine("Esse sistema é responsável por realizar o gerenciamento do estoque. Para iniciar escolha uma das funções abaixo: ");
        Console.WriteLine();
        Console.WriteLine("[1] - Adicionar um novo produto.");
        Console.WriteLine("[2] - Listar todos os produtos.");
        Console.WriteLine("[3] - Buscar um produto pelo nome.");
        Console.WriteLine("[4] - Atualizar um produto.");
        Console.WriteLine("[5] - Remover um produto.");
        Console.WriteLine("[6] - Registrar uma venda.");
        Console.WriteLine("[7] - Listar as vendas.");
        Console.WriteLine("[8] - Cadastrar um funcionário");
        Console.WriteLine("[9] - Listar funcionários");

        int usersChoice = Convert.ToInt32(Console.ReadLine());

        switch (usersChoice)
        {
            case 1:
                AddProduct();
                break;
            case 2:
                GetAll();
                break;
            case 3:
                GetByName();
                break;
            case 4:
                UpdateProduct();
                break;
            case 5:
                DeleteProduct();
                break;
            case 6:
                RegisterSale();
                break;
            case 7: 
                ListSales();
                break;
            case 8:
                RegisterEmployee();
                break;
            case 9:
                GetEmployees();
                break;
        }
    }

    #region Products

    static void AddProduct()
    {
        Product product = new Product();

        Console.WriteLine("*Digite o nome do produto");
        product.Name = Console.ReadLine();

        //Linebreak
        Console.WriteLine();

        Console.WriteLine("Digite uma descrição do produto (opcional): ");
        product.Description = Console.ReadLine();

        //Linebreak
        Console.WriteLine();

        Console.WriteLine("*Digite o valor do produto: ");
        product.Price = Convert.ToDouble(Console.ReadLine().ToString(CultureInfo.InvariantCulture));

        //Linebreak
        Console.WriteLine();

        Console.WriteLine("*Digite a quantidade disponível em estoque: ");
        product.StockAmount = Convert.ToInt32(Console.ReadLine());

        _productService.InsertProduct(product);
    }

    static List<Product> GetAll()
    {
        List<Product> products = _productService.GetAll();

        Console.WriteLine("Esses foram todos os produtos encontrados: ");

        //Linebreak
        Console.WriteLine();

        foreach(Product product in products)
        {
            Console.WriteLine($"Nome: {product.Name} - Descrição = {product.Description ?? "Nenhuma descrição adicionada"} Valor: R$ {product.Price} - Quantidade em estoque: {product.StockAmount}");
        }

        return products;
    }

    static List<Product> GetByName()
    {
        Console.WriteLine("Digite o nome do produto que quer procurar");
        string name = Console.ReadLine();

        List<Product> products = _productService.GetProductByName(name);

        //Linebreak
        Console.WriteLine();

        Console.WriteLine($"Encontramos os seguintes produtos com o nome {name}: ");

        //Linebreak
        Console.WriteLine();

        foreach (Product product in products)
        {
            Console.WriteLine($"Nome: {product.Name} - Descrição = {product.Description ?? "Nenhuma descrição adicionada"} Valor: R$ {product.Price} - Quantidade em estoque: {product.StockAmount}");
        }

        return products;
    }

    static void UpdateProduct()
    {
        Product product = new Product();

        Console.WriteLine("*Digite o nome do produto que deseja atualizar");
        product.Name = Console.ReadLine();

        List<Product> foundProducts = GetByName();

        Console.WriteLine("Esses foram os produtos encontrados. Por favor digite o ID do produto que deseja atualizar.");
        Console.WriteLine();

        foreach (Product foundProduct in foundProducts)
        {
            Console.WriteLine($"[{foundProduct.Id}] - {foundProduct.Name}");
        }

        product.Id = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Digite a nova descrição do produto (opcional): ");
        product.Description = Console.ReadLine();

        //Linebreak
        Console.WriteLine();

        Console.WriteLine("*Digite o novo valor do produto: ");
        product.Price = Convert.ToDouble(Console.ReadLine().ToString(CultureInfo.InvariantCulture));

        //Linebreak
        Console.WriteLine();

        Console.WriteLine("A quantidade digitada será somada à quantidade original.");
        Console.WriteLine("*Digite a quantidade que será adicionada ao estoque: ");
        product.StockAmount = Convert.ToInt32(Console.ReadLine());

        _productService.UpdateProduct(product);
    }

    static void DeleteProduct()
    {
        List<Product> allProducts = GetAll();

        Console.WriteLine("Digite o ID do produto que deseja deletar: ");
        Console.WriteLine();

        foreach (Product product in allProducts)
        {
            Console.WriteLine($"[{product.Id}] - {product.Name}");
        }

        int chosenProduct = Convert.ToInt32(Console.ReadLine());

        _productService.DeleteProduct(chosenProduct);
    }

    #endregion

    #region Sales

    static void RegisterSale()
    {
        Console.WriteLine("Digite o nome do produto que foi vendido: ");
        string name = Console.ReadLine();

        //Linebreak
        Console.WriteLine();

        List<Product> foundProducts = _productService.GetProductByName(name);

        foreach (Product foundProduct in foundProducts)
        {
            Console.WriteLine($"[{foundProduct.Id}] - {foundProduct.Name}");
        }

        Console.WriteLine("Digite o ID do produto que foi vendido.");

        Sale sale = new Sale();
        sale.ProductId = Convert.ToInt32(Console.ReadLine());

        //Linebreak
        Console.WriteLine();

        Console.WriteLine("Digite o CPF do funcionário responsável pela venda: ");
        string cpf = Console.ReadLine();

        //Get information about the employee using CPF.
        Employee employee = _employeeService.GetEmployeeByCpf(cpf);
        sale.EmployeeId = employee.Id;

        Console.WriteLine("Digite quantos produtos foram vendidos: ");
        sale.AmountSold = Convert.ToInt32(Console.ReadLine());

        sale.SaleTime = DateTime.Now;
        _saleService.AddSale(sale);
        _productService.UpdateProductStock(sale.ProductId, sale.AmountSold);
    }

    static List<SalesDTO> ListSales()
    {
        Console.WriteLine("Essas foram todas as vendas encontradas: ");

        //Linebreak
        Console.WriteLine();

        List<SalesDTO> sales = _saleService.ListSales(null);

        foreach (SalesDTO sale in sales)
        {
            Console.WriteLine($"Funcionário: {sale.EmployeeName} - Produto Vendido: {sale.ProductName} - Total de vendas: {sale.SoldAmount}");
        }

        return sales;
    }

    #endregion

    #region Employee

    static void RegisterEmployee()
    {
        Employee employee = new Employee();

        Console.WriteLine("Digite o nome do novo funcionário: ");
        employee.Name = Console.ReadLine();

        //Linebreak
        Console.WriteLine();

        Console.WriteLine("Digite o CPF do novo funcionário");
        employee.Cpf = Console.ReadLine();

        //Linebreak
        Console.WriteLine();

        Console.WriteLine("A data de contratação é a mesma da data de hoje?");
        string answer = Console.ReadLine();

        if (answer.ToLower() == "sim" || answer.ToLower() == "s" || answer.ToLower() == "si")
        {
            employee.HiringDate = DateTime.Now;
        }
        else
        {
            //Linebreak
            Console.WriteLine();

            Console.WriteLine("Digite a data de contratação.");
            string datetime = Convert.ToDateTime(Console.ReadLine()).ToString("yyyy-MM-dd hh:mm:ss.ss");

            employee.HiringDate = Convert.ToDateTime(datetime);
        }

        _employeeService.AddEmployee(employee);
    }

    static List<Employee> GetEmployees()
    {
        List<Employee> employees = _employeeService.GetAllEmployees();

        Console.WriteLine("Esses foram todos os funcionários encontrados: ");

        //Linebreak
        Console.WriteLine();

        foreach (Employee employee in employees)
        {
            Console.WriteLine($"Nome: {employee.Name} - CPF: {employee.Cpf}");
        }

        return employees;
    }

    #endregion
}