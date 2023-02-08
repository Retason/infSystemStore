using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace InfSystemStore
{
    //Хранилище данных
    static class DataStorage
    {
        //Поля с данными
        private static readonly string SavePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Store system");
        private static List<AccountingRecord> _accountingRecords;
        private static List<User> _users;
        private static List<Employee> _employees;
        private static List<Product> _products;

        //Свойства с данными (если поле равно null - загружают данные из файлов)
        public static List<User> Users
        {
            get
            {
                if (_users != null)
                    return _users;
                _users = Get<List<User>>(User.StrorageName);
                if (_users.Count == 0)
                    _users.Add(
                        new User()
                        {
                            ID = 0,
                            Login = "admin",
                            Password = "admin",
                            Role = Role.Administrator
                        });
                return _users;
            }
        }
        public static List<AccountingRecord> AccountingRecords { get => _accountingRecords != null ? _accountingRecords : _accountingRecords = Get<List<AccountingRecord>>(AccountingRecord.StrorageName); }
        public static List<Employee> Employees { get => _employees != null ? _employees : _employees = Get<List<Employee>>(Employee.StrorageName); }
        public static List<Product> Products { get => _products != null ? _products : _products = Get<List<Product>>(Product.StrorageName); }

        //Создание нужной директории
        static DataStorage()
        {
            if (!Directory.Exists(SavePath))
                Directory.CreateDirectory(SavePath);
        }
        //Перезагрузка данных
        public static void ReloadData()
        {
            _accountingRecords = null;
            _users = null;
            _employees = null;
            _products = null;
        }
        //Отправить данные
        public static void Send<Type>(Type obj, string fName) where Type : class
        {
            fName = Path.HasExtension(fName) ? fName : fName + ".json";
            string js_str = JsonConvert.SerializeObject(obj);
            File.WriteAllText(Path.Combine(SavePath, fName),js_str);           
        }
        //Получить данные
        private static Type Get<Type>(string fName) where Type : class, new()
        {
            fName = Path.HasExtension(fName) ? fName : fName + ".json";
            try
            {
                string js_str = File.ReadAllText(Path.Combine(SavePath, fName));
                return JsonConvert.DeserializeObject<Type>(js_str);
            }
            catch
            {
                return new Type();
            }

        }
    }
}
