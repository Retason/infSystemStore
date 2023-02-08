using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfSystemStore
{
    class Cashier
    {
        public List<ITableElement> Data { get; } = new List<ITableElement>();
        public uint[] Sizes { get;}
        public string[] Names { get; }
        public string _productsDataStorage;
        public string _accountingRecordDataStorage;
        public uint QuantityPosition { get; }

        public Cashier()
        {
            _productsDataStorage = Product.StrorageName;
            _accountingRecordDataStorage = AccountingRecord.StrorageName;
            var data = DataStorage.Products;
            foreach (var element in data)
                Data.Add(new CeckoutProduct(element));
            Sizes = new uint[] { 15, 30, 20, 10 };
            QuantityPosition = 0;
            for (int i = 0; i < Sizes.Length - 1; i++)
                QuantityPosition += Sizes[i];
            Names = typeof(CeckoutProduct).GetProperties().Select(x => x.Name).Except(new string[] { "StockQuantity" }).ToArray();
        }

        public string GetAmount() =>
            "Sum = " + Data.Select(x=>x as CeckoutProduct).Sum(x => x.Price).ToString();

        public bool Add(CeckoutProduct product)
        {
            if (product.SelectedQuantity + 1 <= product.StockQuantity)
            {
                product.SelectedQuantity++;
                return true;
            }
            return false;
        }

        public bool Subtract(CeckoutProduct product)
        {
            if (product.SelectedQuantity - 1 >= 0)
            {
                product.SelectedQuantity--;
                return true;
            }
            return false;
        }

        public string[][] GetStringTable()
        {
            string[][] table = new string[Data.Count][];
            for (int i = 0; i < table.Length; i++)
                table[i] = Data[i].ToStrings(Names);
            return table;
        }

        public void SaveChanges()
        {
            IEnumerable<CeckoutProduct> allProducts = Data.Select(x => x as CeckoutProduct);
            List<CeckoutProduct> selectedProducts = allProducts.Where(x=>x.SelectedQuantity > 0).ToList();
            foreach (var product in selectedProducts)
            {
                DataStorage.Products.FirstOrDefault(x => x.ID == product.ID).StockQuantity -= product.SelectedQuantity;

                var last = DataStorage.AccountingRecords.LastOrDefault();
                DataStorage.AccountingRecords.Add(new AccountingRecord()
                {
                    ID = last == null ? 0 : last.ID + 1,
                    Name = product.Name,
                    RecordingDate = DateTime.UtcNow,
                    Amount = product.Price * product.SelectedQuantity,
                    IsReceipt = true
                });
            }
            DataStorage.Send(DataStorage.AccountingRecords, AccountingRecord.StrorageName);
            DataStorage.Send(DataStorage.Products, Product.StrorageName);
            foreach (var product in allProducts)
                product.SelectedQuantity = 0;
        }
            
    }
}
