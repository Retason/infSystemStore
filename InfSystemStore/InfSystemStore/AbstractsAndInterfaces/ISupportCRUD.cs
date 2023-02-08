using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InfSystemStore
{
    
    abstract class ISupportCRUD
    {
        //Данные
        public List<ITableElement> Data { get; protected set; } = new List<ITableElement>();
        //Размеры колонок с данными
        public uint[] Sizes { get; protected set; }
        //Имена колонок
        public string[] Names { get; protected set; }
        //Конструктор настоящего типа элементов
        public Func<ITableElement> Constructor { get; protected set; }
        public string _dataStorageFolder;

        //Преобразовать в таблицу
        public string[][] GetStringTable(List<ITableElement> data)
        {
            string[][] table = new string[data.Count][];
            for (int i = 0; i < table.Length; i++)
                table[i] = data[i].ToStrings(Names);
            return table;            
        }


        public virtual string GetAmount() => null;

        //Сохранение изменений
        private void SaveChanges() =>
            DataStorage.Send(Data, _dataStorageFolder);//.Select(x=>(object)x).ToList()

        //CRUD операции
        public void Add(ITableElement element)
        {
            Data.Add(element);
            SaveChanges();
        }
        public void Delete(ITableElement element)
        {
            Data.Remove(element);
            SaveChanges();
        }
        public void Edit(ITableElement element)
        {
            ITableElement copy = Constructor();
            copy.FillFromStrings(element.ToStrings(Names), Names);

            ITableElement result = ConsoleInteraction.FillForm(copy, Names, true);
            if (result == null)
                return;
            element.FillFromStrings(result.ToStrings(Names), Names);
            SaveChanges();
        }
        public List<ITableElement> Search(string propertyName, string keyWord)
        {
            PropertyInfo property = Data.FirstOrDefault()?.GetProperty(propertyName);
            return Data.Where(x => property.IsValid(x, keyWord)).ToList();
        }

    }
}
