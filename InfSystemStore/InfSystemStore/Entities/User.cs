

using System;
using System.Linq;
using System.Xml.Serialization;

namespace InfSystemStore
{
    //Роли
    public enum Role
    {
        Administrator = 1, //админ
        Cashier, //кассир
        EmployeeManager, //менеджер персонала
        StorageManager, //склад менеджер
        Accountant //бухгалтер
    }
    //Пользователь
    [Serializable]
    public class User : ITableElement
    {
        
        public const string StrorageName = "Users";

        
        public string GetTrueName()
        {
            Employee employee = DataStorage.Employees.FirstOrDefault(x => x.UserID == ID);
            if (employee == null)
                return Login;
            return employee.Name + ' ' + employee.LastName + ' ' + employee.Patronymic;
        }
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

        public static User GetNew() =>
            new User();
    }
}
