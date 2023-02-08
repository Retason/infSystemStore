using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace InfSystemStore
{

    //строка таблицы
    public interface ITableElement
    {
        int ID { get; set; } //обязательный атрибут

        void FillFromStrings(string[] strs, string[] propertyNames)
        {
            var type = this.GetType();
            for (int i = 0; i < strs.Length; i++)
            {
                PropertyInfo property = type.GetProperty(propertyNames[i]);
                if (!string.IsNullOrEmpty(strs[i]))
                    property.SetValueFromString(this, strs[i]);
            }
        }

        string[] ToStrings(string[] propertyesNames)
        {
            Type type = this.GetType();
            string[] strs = new string[propertyesNames.Length];
            int i = 0;
            foreach (string name in propertyesNames)
            {
                PropertyInfo inf = type.GetProperty(name);
                strs[i++] = inf.GetValue(this).ToString();
            }
            return strs;
        }


        PropertyInfo GetProperty(string name) =>
            this.GetType().GetProperty(name);
    }
}
