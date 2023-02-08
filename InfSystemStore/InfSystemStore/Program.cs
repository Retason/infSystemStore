using System;

namespace InfSystemStore
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                User currUser = ConsoleInteraction.Autorization();
                if (currUser == null)
                    return;
                if (currUser.Role == Role.Cashier)
                {
                    Cashier cashier = new Cashier();
                    ConsoleInteraction.CashInteraction(cashier);
                    continue;
                }
                ISupportCRUD workArea = currUser.Role switch
                {
                    Role.Administrator => new Admin(),
                    Role.Accountant => new Accountant(),
                    Role.EmployeeManager => new EmployeeManager(),
                    Role.StorageManager => new StorageManager(),
                    _ => null
                };
                ConsoleInteraction.TableInteraction(workArea, null);
            }
        }
    }
}
