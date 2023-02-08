using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfSystemStore
{
    class Accountant : ISupportCRUD
    {
        public Accountant()
        {
            _dataStorageFolder = AccountingRecord.StrorageName;
            var data = DataStorage.AccountingRecords;
            foreach (var element in data)
                Data.Add(element);
            Sizes = new uint[] { 5, 40, 10, 18, 10 };
            Names = typeof(AccountingRecord).GetProperties().Select(x => x.Name).ToArray();
            Constructor = AccountingRecord.GetNew;
        }

        public override string GetAmount() =>
            "Sum = " + DataStorage.AccountingRecords.Sum(x => x.Amount).ToString();
    }
}
