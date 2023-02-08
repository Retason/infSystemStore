using System;
using System.Reflection;

namespace InfSystemStore
{
    public static class PropertyExtention
    {
        //Присвоить значение свойству из строки
        public static void SetValueFromString<Type>(this PropertyInfo property, Type model, string value) where Type : class
        {
            object obj_value = TypeParse(property, value);
            property.SetValue(model, obj_value);
        }

        //Поиск подстроки
        public static bool IsValid<Type>(this PropertyInfo property, Type model, string value) where Type : ITableElement =>
            property.GetValue(model).ToString().ToLower().Contains(value.ToLower());        

        //Преобразование из строки в примитивные типы
        private static object TypeParse(PropertyInfo property, string value)=> 
            property.PropertyType.Name switch
        {
            "Int32" => int.Parse(value),
            "Double" => double.Parse(value),
            "DateTime" => DateTime.Parse(value),
            "Int64" => long.Parse(value),
            "Role" => (Role)Enum.Parse(typeof(Role),value),
            "Boolean" => bool.Parse(value),
            _ => value
        };
    }
}
