using System.Collections.Generic;
using System.Linq;

namespace InfSystemStore
{
    class EmployeeManager : ISupportCRUD
    {
        public EmployeeManager()
        {
            _dataStorageFolder = Employee.StrorageName;
            var data = DataStorage.Employees;
            foreach (var element in data)
                Data.Add(element);
            Sizes = new uint[] { 6, 5, 14, 13, 12, 12 }; //, 10, 8, 7 };
            Names = typeof(Employee).GetProperties().Select(x => x.Name).Except(new string[]{ "UserID" }).ToArray();
            Constructor = Employee.GetNew;
        }
    }
}
