using StoreConsoleApp;

const string connectionString = "Host=localhost;Port=5432;Database=luboychi;Username=postgres;Password=09092009";

var categoriesService = new CategoriesService(connectionString);
var productsService = new ProductsService(connectionString);
var warehousesService = new WarehousesService(connectionString);
var stocksService = new StocksService(connectionString);
var customersService = new CustomersService(connectionString);
var salesService = new SalesService(connectionString);
var saleItemsService = new SaleItemsService(connectionString);

while (true)
{
    System.Console.WriteLine("============================================================");
    System.Console.WriteLine(@"1-Categories
2-Products
3-Warehouses
4-Stocks
5-Customers
6-Sales
7-Sale Items
0-Exit");
    System.Console.WriteLine("============================================================");

    var action = Console.ReadLine();

    switch (action)
    {
        case "1":
            while (true)
            {
                System.Console.WriteLine("============================================================");
                System.Console.WriteLine(@"1-Show all categories
2-Add new category
3-Update category description
4-Delete category
0-Back");
                System.Console.WriteLine("============================================================");

                var catAction = Console.ReadLine();

                switch (catAction)
                {
                    case "1":
                        categoriesService.ShowAllCategories();
                        break;

                    case "2":
                        System.Console.Write("Enter new category: ");
                        var nc = Console.ReadLine();
                        categoriesService.AddNewCategory(nc, null, null);
                        break;

                    case "3":
                        System.Console.WriteLine("Enter category id: ");
                        var id = Convert.ToInt32(Console.ReadLine());
                        System.Console.Write("Enter new category description: ");
                        var nd = Console.ReadLine();
                        categoriesService.UpdateDescriptionOfCategory(id, nd);
                        break;

                    case "4":
                        System.Console.WriteLine("Enter category id: ");
                        var idd = Convert.ToInt32(Console.ReadLine());
                        categoriesService.DeleteCategory(idd);
                        break;

                    case "0":
                        goto endCategories;
                }
            }
        endCategories:
            break;

        case "2":
            while (true)
            {
                System.Console.WriteLine("============================================================");
                System.Console.WriteLine(@"1-Show all products
2-Add new product
3-Update product price
4-Delete product
0-Back");
                System.Console.WriteLine("============================================================");

                var prodAction = Console.ReadLine();

                switch (prodAction)
                {
                    case "1":
                        productsService.ShowAllProducts();
                        break;

                    case "2":
                        System.Console.Write("Enter product name: ");
                        var pn = Console.ReadLine();
                        System.Console.Write("Enter price: ");
                        var price = Convert.ToDecimal(Console.ReadLine());
                        productsService.AddNewProduct(pn, null, price, null, null);
                        break;

                    case "3":
                        System.Console.WriteLine("Enter product id: ");
                        var pid = Convert.ToInt32(Console.ReadLine());
                        System.Console.Write("Enter new price: ");
                        var np = Convert.ToDecimal(Console.ReadLine());
                        productsService.UpdatePriceOfProduct(pid, np);
                        break;

                    case "4":
                        System.Console.WriteLine("Enter product id: ");
                        var pidd = Convert.ToInt32(Console.ReadLine());
                        productsService.DeleteProduct(pidd);
                        break;

                    case "0":
                        goto endProducts;
                }
            }
        endProducts:
            break;

        case "3":
            while (true)
            {
                System.Console.WriteLine("============================================================");
                System.Console.WriteLine(@"1-Show all warehouses
2-Add new warehouse
3-Update warehouse address
4-Delete warehouse
0-Back");
                System.Console.WriteLine("============================================================");

                var whAction = Console.ReadLine();

                switch (whAction)
                {
                    case "1":
                        warehousesService.ShowAllWarehouses();
                        break;

                    case "2":
                        System.Console.Write("Enter warehouse name: ");
                        var wn = Console.ReadLine();
                        System.Console.Write("Enter address: ");
                        var wa = Console.ReadLine();
                        warehousesService.AddNewWarehouse(wn, wa);
                        break;

                    case "3":
                        System.Console.WriteLine("Enter warehouse id: ");
                        var wid = Convert.ToInt32(Console.ReadLine());
                        System.Console.Write("Enter new address: ");
                        var nwa = Console.ReadLine();
                        warehousesService.UpdateAddressOfWarehouse(wid, nwa);
                        break;

                    case "4":
                        System.Console.WriteLine("Enter warehouse id: ");
                        var widd = Convert.ToInt32(Console.ReadLine());
                        warehousesService.DeleteWarehouse(widd);
                        break;

                    case "0":
                        goto endWarehouses;
                }
            }
        endWarehouses:
            break;

        case "4":
            while (true)
            {
                System.Console.WriteLine("============================================================");
                System.Console.WriteLine(@"1-Show all stocks
2-Add new stock record
3-Update stock quantity
4-Delete stock record
0-Back");
                System.Console.WriteLine("============================================================");

                var stAction = Console.ReadLine();

                switch (stAction)
                {
                    case "1":
                        stocksService.ShowAllStocks();
                        break;

                    case "2":
                        System.Console.Write("Enter product id: ");
                        var spid = Convert.ToInt32(Console.ReadLine());
                        System.Console.Write("Enter warehouse id: ");
                        var swid = Convert.ToInt32(Console.ReadLine());
                        System.Console.Write("Enter quantity: ");
                        var sq = Convert.ToInt32(Console.ReadLine());
                        stocksService.AddNewStock(spid, swid, sq);
                        break;

                    case "3":
                        System.Console.WriteLine("Enter stock id: ");
                        var sid = Convert.ToInt32(Console.ReadLine());
                        System.Console.Write("Enter new quantity: ");
                        var nsq = Convert.ToInt32(Console.ReadLine());
                        stocksService.UpdateQuantityOfStock(sid, nsq);
                        break;

                    case "4":
                        System.Console.WriteLine("Enter stock id: ");
                        var sidd = Convert.ToInt32(Console.ReadLine());
                        stocksService.DeleteStock(sidd);
                        break;

                    case "0":
                        goto endStocks;
                }
            }
        endStocks:
            break;

        case "5":
            while (true)
            {
                System.Console.WriteLine("============================================================");
                System.Console.WriteLine(@"1-Show all customers
2-Add new customer
3-Update customer phone
4-Delete customer
0-Back");
                System.Console.WriteLine("============================================================");

                var custAction = Console.ReadLine();

                switch (custAction)
                {
                    case "1":
                        customersService.ShowAllCustomers();
                        break;

                    case "2":
                        System.Console.Write("Enter full name: ");
                        var cn = Console.ReadLine();
                        System.Console.Write("Enter email: ");
                        var ce = Console.ReadLine();
                        System.Console.Write("Enter phone number: ");
                        var cp = Console.ReadLine();
                        System.Console.Write("Enter address: ");
                        var ca = Console.ReadLine();
                        customersService.AddNewCustomer(cn, ce, cp, ca);
                        break;

                    case "3":
                        System.Console.WriteLine("Enter customer id: ");
                        var cid = Convert.ToInt32(Console.ReadLine());
                        System.Console.Write("Enter new phone number: ");
                        var ncp = Console.ReadLine();
                        customersService.UpdatePhoneOfCustomer(cid, ncp);
                        break;

                    case "4":
                        System.Console.WriteLine("Enter customer id: ");
                        var cidd = Convert.ToInt32(Console.ReadLine());
                        customersService.DeleteCustomer(cidd);
                        break;

                    case "0":
                        goto endCustomers;
                }
            }
        endCustomers:
            break;

        case "6":
            while (true)
            {
                System.Console.WriteLine("============================================================");
                System.Console.WriteLine(@"1-Show all sales
2-Add new sale
3-Update sale status
4-Delete sale
0-Back");
                System.Console.WriteLine("============================================================");

                var saleAction = Console.ReadLine();

                switch (saleAction)
                {
                    case "1":
                        salesService.ShowAllSales();
                        break;

                    case "2":
                        System.Console.Write("Enter warehouse id: ");
                        var swid2 = Convert.ToInt32(Console.ReadLine());
                        System.Console.Write("Enter customer id: ");
                        var scid = Convert.ToInt32(Console.ReadLine());
                        System.Console.Write("Enter sale date (yyyy-MM-dd): ");
                        var sdate = Convert.ToDateTime(Console.ReadLine());
                        System.Console.Write("Enter status: ");
                        var sstatus = Console.ReadLine();
                        System.Console.Write("Enter total price: ");
                        var stotal = Convert.ToDecimal(Console.ReadLine());
                        System.Console.Write("Enter discount price (0 if none): ");
                        var sdiscount = Convert.ToDecimal(Console.ReadLine());
                        salesService.AddNewSale(swid2, scid, sdate, sstatus, stotal, sdiscount);
                        break;

                    case "3":
                        System.Console.WriteLine("Enter sale id: ");
                        var said = Convert.ToInt32(Console.ReadLine());
                        System.Console.Write("Enter new status: ");
                        var nstatus = Console.ReadLine();
                        salesService.UpdateStatusOfSale(said, nstatus);
                        break;

                    case "4":
                        System.Console.WriteLine("Enter sale id: ");
                        var saidd = Convert.ToInt32(Console.ReadLine());
                        salesService.DeleteSale(saidd);
                        break;

                    case "0":
                        goto endSales;
                }
            }
        endSales:
            break;

        case "7":
            while (true)
            {
                System.Console.WriteLine("============================================================");
                System.Console.WriteLine(@"1-Show all sale items
2-Add new sale item
3-Update sale item quantity
4-Delete sale item
0-Back");
                System.Console.WriteLine("============================================================");

                var siAction = Console.ReadLine();

                switch (siAction)
                {
                    case "1":
                        saleItemsService.ShowAllSaleItems();
                        break;

                    case "2":
                        System.Console.Write("Enter sale id: ");
                        var sisid = Convert.ToInt32(Console.ReadLine());
                        System.Console.Write("Enter product id: ");
                        var sipid = Convert.ToInt32(Console.ReadLine());
                        System.Console.Write("Enter quantity: ");
                        var siq = Convert.ToInt32(Console.ReadLine());
                        System.Console.Write("Enter total price: ");
                        var sitotal = Convert.ToDecimal(Console.ReadLine());
                        saleItemsService.AddNewSaleItem(sisid, sipid, siq, sitotal);
                        break;

                    case "3":
                        System.Console.WriteLine("Enter sale item id: ");
                        var siid = Convert.ToInt32(Console.ReadLine());
                        System.Console.Write("Enter new quantity: ");
                        var nsiq = Convert.ToInt32(Console.ReadLine());
                        saleItemsService.UpdateQuantityOfSaleItem(siid, nsiq);
                        break;

                    case "4":
                        System.Console.WriteLine("Enter sale item id: ");
                        var siidd = Convert.ToInt32(Console.ReadLine());
                        saleItemsService.DeleteSaleItem(siidd);
                        break;

                    case "0":
                        goto endSaleItems;
                }
            }
        endSaleItems:
            break;

        case "0":
            return;
    }
}
