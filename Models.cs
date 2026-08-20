namespace StoreConsoleApp
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentCategoryId { get; set; }
        public bool IsActive { get; set; }

        public override string ToString()
            => $"[{Id}] {Name} — {Description} (Parent: {(ParentCategoryId.HasValue ? ParentCategoryId.ToString() : "none")}, Active: {IsActive})";
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? Weight { get; set; }
        public int? CategoryId { get; set; }

        public override string ToString()
            => $"[{Id}] {Name} — {Price:0.00} (Category: {(CategoryId.HasValue ? CategoryId.ToString() : "none")})";
    }

    public class Warehouse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }

        public override string ToString()
            => $"[{Id}] {Name} — {Address} (Active: {IsActive})";
    }

    public class Stock
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public int Quantity { get; set; }

        public override string ToString()
            => $"[{Id}] Product #{ProductId} @ Warehouse #{WarehouseId} — Qty: {Quantity}";
    }

    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public override string ToString()
            => $"[{Id}] {FullName} — {Email} — {PhoneNumber}";
    }

    public class Sale
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public int CustomerId { get; set; }
        public DateTime SaleDate { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal DiscountPrice { get; set; }

        public override string ToString()
            => $"[{Id}] {SaleDate:yyyy-MM-dd} — Warehouse #{WarehouseId} — Customer #{CustomerId} — {Status} — {TotalPrice:0.00} (discount {DiscountPrice:0.00})";
    }

    public class SaleItem
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }

        public override string ToString()
            => $"[{Id}] Sale #{SaleId} — Product #{ProductId} — Qty: {Quantity} — {TotalPrice:0.00}";
    }
}
