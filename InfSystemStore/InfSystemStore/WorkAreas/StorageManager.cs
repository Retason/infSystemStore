using System.Collections.Generic;
using System.Linq;

namespace InfSystemStore
{
    class StorageManager : ISupportCRUD
    {
        public StorageManager()
        {
            _dataStorageFolder = Product.StrorageName;
            var data = DataStorage.Products;
            foreach (var element in data)
                Data.Add(element);
            Sizes = new uint[] { 10, 40, 10, 15 };
            Names = typeof(Product).GetProperties().Select(x => x.Name).ToArray();
            Constructor = Product.GetNew;
        }
    }
}
