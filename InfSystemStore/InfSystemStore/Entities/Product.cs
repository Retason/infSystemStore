namespace InfSystemStore
{
    //Продукт
    public class Product : ITableElement
    {
        public const string StrorageName = "Products";
        public int ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }
        public static Product GetNew() =>
            new Product();
    }
}
