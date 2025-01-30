using ConsoleApp1.Models;
using ConsoleApp1.Services;
using StockControl.Models;
using StockControl.Services;
using System.Globalization;

class StockControlMain
{
    #region Constructor
    private readonly static DatabaseChecksService _databaseChecksService = new DatabaseChecksService();
    private readonly static ProductService _productService = new ProductService();
    private readonly static SaleService _saleService = new SaleService();   
    private readonly static EmployeeService _employeeService = new EmployeeService();

    #endregion

    static void Main()
    {
        ProductService _service = new ProductService();

        _databaseChecksService.CheckTableExists();

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
        Console.WriteLine("[9] - Desvincular um funcionário");
        Console.WriteLine("[10] - Listar funcionários");
        Console.WriteLine("[11] - Listar funcionários desvinculados");
        Console.WriteLine("[12] - Limpar a tela");

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
                RemoveEmployee();
                break;
            case 10:
                GetEmployees();
                break;
            case 11:
                GetExEmployees();
                break;
            case 12:
                Console.Clear();
                Main();
                break;
        }

        CloseSystem();
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

        if(products.Count == 0)
        {
            //Linebreak
            Console.WriteLine();

            Console.WriteLine("Nenhum produto encontrado.");

            CloseSystem();
        }

        Console.WriteLine("Esses foram todos os produtos encontrados: ");

        //Linebreak
        Console.WriteLine();

        foreach(Product product in products)
        {
            product.Description = product.Description == null || string.IsNullOrEmpty(product.Description) ? "Nenhuma descrição adicionada" : product.Description;
            Console.WriteLine($"Nome: {product.Name} - Descrição: {product.Description} - Valor: R$ {product.Price} - Quantidade em estoque: {product.StockAmount}");
        }

        return products;
    }

    static List<Product> GetByName()
    {
        //Linebreak
        Console.WriteLine();

        Console.WriteLine("Digite o nome do produto que quer procurar");
        string name = Console.ReadLine();

        List<Product> products = _productService.GetProductByName(name);

        if (products.Count == 0)
        {
            //Linebreak
            Console.WriteLine();

            Console.WriteLine("Nenhum produto encontrado.");

            CloseSystem();
        }

        //Linebreak
        Console.WriteLine();

        Console.WriteLine($"Encontramos os seguintes produtos com o nome {name}: ");

        //Linebreak
        Console.WriteLine();

        foreach (Product product in products)
        {
            Console.WriteLine($"Nome: {product.Name} - Descrição: {product.Description ?? "Nenhuma descrição adicionada"} - Valor: R$ {product.Price} - Quantidade em estoque: {product.StockAmount}");
        }

        return products;
    }

    static void UpdateProduct()
    {
        //Linebreak
        Console.WriteLine();

        Product product = new Product();

        Console.WriteLine("*Primeiro precisamos encontrar o produto que deseja atualizar. Por favor digite o nome do produto e iremos verificar se ele existe: ");
        string productName = Console.ReadLine();

        List<Product> foundProducts = _productService.GetProductByName(productName);

        if (foundProducts.Count == 0)
        {
            //Linebreak
            Console.WriteLine();

            Console.WriteLine("Nenhum produto encontrado.");
            
            CloseSystem();
        }

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

        Console.WriteLine("*Digite a quantidade que será adicionada ao estoque: ");
        Console.WriteLine("Obs: A quantidade digitada será somada à quantidade original.");
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

        CloseSystem();
    }

    #endregion

    #region Sales

    static void RegisterSale()
    {
        //Linebreak
        Console.WriteLine();

        Console.WriteLine("Digite o nome do produto que foi vendido e vamos verificar se ele existe. Obs: Para listar todos basta não inserir nada.");
        string name = Console.ReadLine();

        List<Product> foundProducts = _productService.GetProductByName(name);

        if (foundProducts.Count == 0)
        {
            //Linebreak
            Console.WriteLine();

            Console.WriteLine("Nenhum produto encontrado.");

            CloseSystem();
        }

        //Linebreak
        Console.WriteLine();

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

        if(employee.Id == 0)
        {
            //Linebreak
            Console.WriteLine();

            Console.WriteLine("Nenhum CPF encontrado com o CPF informado. Gostaria de reiniciar o processo?");
            string userAnswer = Console.ReadLine().ToLower();

            if (userAnswer == "sim" || userAnswer == "si" || userAnswer == "s")
                RegisterSale();
            else
                CloseSystem();
        }

        sale.EmployeeId = employee.Id;

        Console.WriteLine("Digite quantos produtos foram vendidos: ");
        sale.AmountSold = Convert.ToInt32(Console.ReadLine());

        Product chosenProduct = foundProducts.Where(e => e.Id == sale.ProductId).FirstOrDefault();
        if (chosenProduct.StockAmount < 1 || chosenProduct.StockAmount - sale.AmountSold < 0)
        {
            //Linebreak
            Console.WriteLine();

            Console.WriteLine("Não é possível registrar essa quantidade de vendas pois não há a quantidade em estoque suficiente para essa venda. Gostaria de reiniciar a operação?");
            string userAnswer = Console.ReadLine().ToLower();

            if(userAnswer == "sim" || userAnswer == "si" || userAnswer == "s")
            {
                Console.Clear();
                RegisterSale();
            }
        }

            sale.SaleTime = DateTime.Now;
        _saleService.AddSale(sale);
        _productService.UpdateProductStock(sale.ProductId, sale.AmountSold);
    }

    static void ListSales()
    {
        List<SalesDTO> sales = _saleService.ListSales(null);

        if (sales.Count == 0)
        {
            Console.WriteLine("Nenhuma venda encontrada");

            CloseSystem();
        }

        //Linebreak
        Console.WriteLine();

        Console.WriteLine("Essas foram todas as vendas encontradas: ");

        //Linebreak
        Console.WriteLine();

        foreach (SalesDTO sale in sales)
        {
            Console.WriteLine($"Funcionário: {sale.EmployeeName} - Produto Vendido: {sale.ProductName} - Total de vendas: {sale.SoldAmount}");
        }

        CloseSystem();
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
        employee.Cpf = Console.ReadLine().Trim().Replace("-", "").Replace(".","");

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

            Console.WriteLine("Digite a data de contratação:");
            string datetime = Convert.ToDateTime(Console.ReadLine()).ToString("yyyy-MM-dd hh:mm:ss.ss");

            employee.HiringDate = Convert.ToDateTime(datetime);
        }

        _employeeService.AddEmployee(employee);

        CloseSystem();
    }

    static List<Employee> GetEmployees()
    {
        List<Employee> employees = _employeeService.GetAllEmployees();

        if (employees.Count == 0)
        {
            //Linebreak
            Console.WriteLine();

            Console.WriteLine("Nenhum empregado encontrado.");

            CloseSystem();
        }

        //Linebreak
        Console.WriteLine(); 

        Console.WriteLine("Esses foram todos os funcionários encontrados: ");

        //Linebreak
        Console.WriteLine();

        foreach (Employee employee in employees)
        {
            string status = employee.IsEmployed == true ? "Empregado" : "Vínculo Rompido";
            Console.WriteLine($"Nome: {employee.Name} - CPF: {employee.Cpf} - {status}");
        }

        return employees;
    }

    static void RemoveEmployee()
    {
        List<Employee> employees = _employeeService.GetAllEmployees();

        if (employees.Count == 0)
        {
            //Linebreak
            Console.WriteLine();

            Console.WriteLine("Nenhum empregado encontrado.");

            CloseSystem();
        }

        //Linebreak
        Console.WriteLine();

        Console.WriteLine("Esses foram todos os funcionários encontrados: ");

        //Linebreak
        Console.WriteLine();

        foreach (Employee employee in employees)
        {
            string status = employee.IsEmployed == true ? "Empregado" : "Vínculo Rompido";
            Console.WriteLine($"Nome: {employee.Name} - CPF: {employee.Cpf} - {status}");
        }

        //Linebreak
        Console.WriteLine();

        Console.WriteLine("Digite o ID de qual destes funcionários você deseja romper o vínculo:");
        int usersChoice = Convert.ToInt32( Console.ReadLine());

        Employee chosenEmployee = employees.Where(e => e.Id == usersChoice).FirstOrDefault();

        if (chosenEmployee == null)
        {
            //Linebreak
            Console.WriteLine();

            Console.WriteLine("Nenhum funcionário encontrado com o ID digitado. Gostaria de reiniciar a operação?");
            string userAnswer = Console.ReadLine().ToLower();

            if (userAnswer == "sim" || userAnswer == "si" || userAnswer == "s")
            {
                Console.Clear();
                RemoveEmployee();
            }
        }

        _employeeService.RemoveEmployee(usersChoice);

        //Linebreak
        Console.WriteLine();

        Console.WriteLine("O processo foi concluído com sucesso.");

        CloseSystem();
    }

    static List<Employee> GetExEmployees()
    {
        List<Employee> employees = _employeeService.GetAllExEmployees();

        if (employees.Count == 0)
        {
            //Linebreak
            Console.WriteLine();

            Console.WriteLine("Nenhum empregado encontrado.");

            CloseSystem();
        }

        //Linebreak
        Console.WriteLine();

        Console.WriteLine("Esses foram todos os funcionários encontrados: ");

        //Linebreak
        Console.WriteLine();

        foreach (Employee employee in employees)
        {
            string status = employee.IsEmployed == true ? "Empregado" : "Vínculo Rompido";
            Console.WriteLine($"Nome: {employee.Name} - CPF: {employee.Cpf} - {status} - Data do desvínculo: dd/MM/yyyy HH:MM:SS", employee.UnemploymentDate);
        }

        return employees;
    }

    #endregion

    #region Private

    private static void CloseSystem()
    {
        //Linebreak
        Console.WriteLine();

        Console.WriteLine("Deseja encerrar o sistema? Sim ou Não.");
        string userAnswer = Console.ReadLine().ToLower();

        if (userAnswer == "não" || userAnswer == "nao" || userAnswer == "na" || userAnswer == "n")
            Main();
        else
            Environment.Exit(0);
    }

    #endregion
}