using System;

namespace InfSystemStore
{
    //Запись в бугалтерии
    public class AccountingRecord : ITableElement
    {
        internal static string StrorageName = "AccountingRecords";

        public int ID { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public DateTime RecordingDate { get; set; }
        public bool IsReceipt { get; set; }

        public static AccountingRecord GetNew() =>
            new AccountingRecord();


    }
}
