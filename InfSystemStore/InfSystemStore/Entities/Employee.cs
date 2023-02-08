using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace InfSystemStore
{
    //Сотрудник
    [DataContract]
    public class Employee :ITableElement
    {
        public const string StrorageName = "Employeers";

        [JsonIgnore]
        public int UserId
        { get => ID; 
            set {
                if (!DataStorage.Users.Any(x => x.ID == value))
                    throw new Exception($"User with id = {value} not found");
                if (DataStorage.Employees.Any(x=>x.UserID == value && x.ID != this.ID))
                    throw new Exception($"User with id = {value} already linked to another employee");
                ID = value;
            } 
        }
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Patronymic { get; set; }
        [DataMember]
        public string Position { get; set; }
        [DataMember]
        public DateTime Birth { get; set; }
        [DataMember]
        public long Passport { get; set; }
        
        [DataMember]
        public double Salary { get; set; }
        [DataMember]
        public int UserID { get; set; }

        public static Employee GetNew() =>
            new Employee();
    }
}
