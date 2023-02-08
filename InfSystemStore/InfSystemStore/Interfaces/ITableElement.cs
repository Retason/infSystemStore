using System.Reflection;

namespace InfSystemStore
{
    public interface ITableElement
    {
        int ID { get; set; }
        void FillFromStrings(string[] strs)
        {
            int i= 0;
            foreach (PropertyInfo property in this.GetType().GetProperties())
            {
                if (!string.IsNullOrEmpty(strs[i]))
                    property.SetValueFromString(this, strs[i]);
                i++;
            }
        }
        string[] ToStrings()
        {
            var properties = this.GetType().GetProperties();
            string[] strs = new string[properties.Length];
            int i = 0;
            foreach (PropertyInfo propertyInfo in properties)
                strs[i++] = propertyInfo.GetValue(this).ToString();
            return strs;
        }

        PropertyInfo GetProperty(int index) =>
            this.GetType().GetProperties()[index];
    }
}
