using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InfSystemStore
{
    //Интерфейс
    interface ISupportCRUD<DataType> where DataType : ITableElement, new()
    {
        List<DataType> Data { get; }
        uint[] Sizes { get; }
        string[] Names { get; }

        string[][] GetStringTable()
        {
            string[][] table = new string[Data.Count][];
            for (int i = 0; i < table.Length; i++)
                table[i] = Data[i].ToStrings();
            return table;            
        }

        void Add(string[] strs)
        {
            DataType newElement = new DataType();
            newElement.FillFromStrings(strs);
            Data.Add(newElement);
        }
        void Delete(int id) =>
            Data.Remove(Data.Find(x=>x.ID == id));
        void Edit(int id, string[] strs) =>
            Data.Find(x => x.ID.Equals(id)).FillFromStrings(strs);
        List<ITableElement> Search(int propertyIndex, string keyWord)
        {
            PropertyInfo property = Data.First().GetProperty(propertyIndex);
            return Data.Where(x => property.IsValid(x, keyWord)).ToList();
        }

    }
}
