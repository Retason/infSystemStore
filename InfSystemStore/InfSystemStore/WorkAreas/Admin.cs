using System.Collections.Generic;
using System.Linq;

namespace InfSystemStore
{
    class Admin : ISupportCRUD
    {
        public Admin()
        {
            _dataStorageFolder = User.StrorageName;
            var data = DataStorage.Users;
            foreach (var element in data)
                Data.Add(element);
            Sizes = new uint[] { 10, 20,20,20};
            Names = typeof(User).GetProperties().Select(x => x.Name).ToArray();
            Constructor = User.GetNew;
        }

    }
}
