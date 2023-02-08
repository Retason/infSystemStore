namespace InfSystemStore
{
    //Товар на кассе
    public class CeckoutProduct : Product, ITableElement
    {
        public int SelectedQuantity { get; set; }
        public CeckoutProduct(Product product)
        {
            ID = product.ID;
            Name = product.Name;
            Price = product.Price;
            StockQuantity = product.StockQuantity;
            SelectedQuantity = 0;
        }
    }
}
