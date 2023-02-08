using System;
using System.Collections.Generic;
using System.Linq;

namespace InfSystemStore
{
    //Взаимодействие с консолью
    static class ConsoleInteraction
    {
        private const string interval = "   ";
        private const string pls_enter = ": ";
        private const int _up_offset = 2;
        private const int _left_offset = 100;
        private static int additional_line_len = pls_enter.Length + interval.Length;
        private static User _currentUser;

        //Кассир
        public static void CashInteraction(Cashier cashier)
        {
            PrintHeader("Esc - back","S - save");
            PrintTable(cashier.GetStringTable(), cashier.Sizes, cashier.Names);
            ConsoleArrow arrow = ConsoleArrow.GetNew(cashier.Data.Count);
            while (true)
            {
                var key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.OemPlus: goto case ConsoleKey.Add;
                    case ConsoleKey.OemMinus: goto case ConsoleKey.Subtract;

                    case ConsoleKey.Add:
                        CeckoutProduct product = cashier.Data[arrow.RelativePosition] as CeckoutProduct;
                        //Пишет количество на нужной позиции (если оно изменилось)
                        if (cashier.Add(product))
                            PrintMsgOnPosition(
                                product.SelectedQuantity.ToString(), 
                                (3, arrow.RelativePosition + _up_offset + 1), 
                                10);
                        break;
                    case ConsoleKey.Subtract:
                        CeckoutProduct product2 = cashier.Data[arrow.RelativePosition] as CeckoutProduct;
                        //Пишет количество на нужной позиции (если оно изменилось)
                        if (cashier.Subtract(product2))
                            PrintMsgOnPosition(
                                    product2.SelectedQuantity.ToString(),
                                    (3, arrow.RelativePosition + _up_offset + 1),
                                    10);
                        break;
                    case ConsoleKey.S:
                        cashier.SaveChanges();
                        PrintHeader("Esc - back", "S - save");
                        PrintTable(cashier.GetStringTable(), cashier.Sizes, cashier.Names);
                        break;
                    case ConsoleKey.Escape:
                        _currentUser = null;
                        return;
                    default:
                        ConsoleArrow.MoveCases(arrow, key);
                        break;
                }
            }
        }

        //Взаимодействие интерфейса с таблицей
        public static void TableInteraction(ISupportCRUD workArea, List<ITableElement> data = null)
        {
            if (data == null)
                data = workArea.Data;
            ConsoleArrow arrow = null;
            PrintTableMenue(workArea, data, ref arrow);
            while (true)
            {
                var key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.F1:
                        ITableElement element = FillForm(workArea.Constructor(), workArea.Names);
                        if (element != null)
                            workArea.Add(element);
                        PrintTableMenue(workArea, data, ref arrow);
                        break;
                    case ConsoleKey.F2:
                        TableSearch(workArea);
                        PrintTableMenue(workArea, data, ref arrow);
                        break;
                    case ConsoleKey.Enter:
                        MoreInformationInteraction(data[arrow.RelativePosition], workArea);
                        PrintTableMenue(workArea, data, ref arrow);
                        break;
                    case ConsoleKey.Escape:
                        DataStorage.ReloadData();
                        return;
                    default:
                        ConsoleArrow.MoveCases(arrow, key);
                        break;
                }
            }
        }
        //Вывод меню и таблицы
        private static void PrintTableMenue(ISupportCRUD workArea, List<ITableElement> data, ref ConsoleArrow arrow)
        {
            PrintHeader(
                "Enter - more inf",
                "F1 - add",
                "F2 - search",
                "Esc - back");
            PrintTable(workArea.GetStringTable(data), workArea.Sizes, workArea.Names);
            arrow = ConsoleArrow.GetNew(data.Count);
            string sum = workArea.GetAmount();
            if (sum != null)
                Console.WriteLine(new string('-', 100) + '\n' + sum);
        }
        //Окно подробной инфы
        public static void MoreInformationInteraction(ITableElement element, ISupportCRUD workArea)
        {
            PrintHeader("F1 - edit","del - delete", "Esc - back");
            int i = 0;
            string[] answers_old = element.ToStrings(workArea.Names);
            foreach (string point in workArea.Names)
                Console.WriteLine(interval + point + pls_enter + answers_old[i++]);
            while (true)
            {
                var key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.F1:
                        //Изменение элемента и перерисовка формы
                        workArea.Edit(element);
                        PrintHeader("F1 - edit", "del - delete", "Esc - back");
                        int j = 0;
                        string[] answers_new = element.ToStrings(workArea.Names);
                        foreach (string point in workArea.Names)
                            Console.WriteLine(interval + point + pls_enter + answers_new[j++]);
                        break;
                    case ConsoleKey.Delete:
                        workArea.Delete(element);
                        return;
                    case ConsoleKey.Escape:
                        return;
                }
            }

        }
        //Поиск по таблице
        private static void TableSearch(ISupportCRUD workArea)
        {
            PrintHeader(
                "Esc - back",
                "Enter - search");

            foreach (string name in workArea.Names)
                Console.WriteLine(interval + name);
            ConsoleArrow arrow = ConsoleArrow.GetNew(workArea.Names.Length);

            while (true)
            {
                var key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.Escape:
                        return;
                    case ConsoleKey.Enter:
                        string keyWord = ScanfInput("Key word: ");
                        if (!string.IsNullOrEmpty(keyWord))
                        {
                            TableInteraction(workArea, workArea.Search(workArea.Names[arrow.RelativePosition], keyWord));
                            return;
                        }
                        break;
                    default:
                        ConsoleArrow.MoveCases(arrow, key);
                        break;
                }
            }
        }
        //Отрисовать таблицу
        private static void PrintTable(string[][] data, uint[] sizes, string[] names)
        {
            Console.WriteLine(interval+GetFormatRow(names, sizes));
            foreach (string[] row in data)
                Console.WriteLine(interval + GetFormatRow(row, sizes));
        }
        //Получить выровненную строку таблицы
        private static string GetFormatRow(string[] data, uint[] sizes)
        {
            string formatRow = "";
            for (int i = 0; i < sizes.Length; i++)
                formatRow += string.Format("{" + 0.ToString() + ',' + (sizes[i]).ToString() + "}|", Shorten(data[i], (int)sizes[i]));
            return formatRow;
        }
        //Укоротить содержание, если оно не влазит в колонку
        private static string Shorten(string value, int maxLen) =>
            value.Length <= maxLen ? value : value.Remove(maxLen - 3) + "...";

        //Заполнение формы 
        public static ITableElement FillForm(ITableElement model, string[] fillPoints, bool needPrintOldValues = false)
        {
            string[] mainCommands = {
                "Esc - back",
                "S - save",
                "Enter - fill" };
            //Добавляем подписи ролей
            if (model.GetType().Equals(typeof(User)))
            {
                string[] roleNamess = Enum.GetNames(typeof(Role));
                string[] RoleValueNames = new string[roleNamess.Length];
                for (int j = 0; j < roleNamess.Length; j++)
                    RoleValueNames[j] = ((int)Enum.Parse(typeof(Role), roleNamess[j])).ToString() + " - " + roleNamess[j];
                mainCommands = RoleValueNames.Concat(mainCommands).ToArray();
            }
            PrintHeader(mainCommands);
            
            //Печатаем форму и, если нужно, старые данные
            string[] answers = new string[fillPoints.Length];
            int i = 0;
            if (needPrintOldValues)
            {
                string[] old_answers = model.ToStrings(fillPoints);
                foreach (string point in fillPoints)
                    Console.WriteLine(interval + point + pls_enter + old_answers[i++]);
            }
            else
                foreach (string point in fillPoints)
                    Console.WriteLine(interval + point + pls_enter);
            ConsoleArrow arrow = ConsoleArrow.GetNew(fillPoints.Length);
            

            while (true)
            {
                var key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.S:
                        if (!needPrintOldValues && answers.Contains(null))
                        {
                            PrintMsg("To save, all fields must be filled in");
                            break;
                        }
                        try
                        {
                            model.FillFromStrings(answers, fillPoints);
                            return model;
                        }
                        catch (Exception ex)
                        {
                            PrintMsg("Error filling\nOut the form the fields are filled in incorrectly\n" + ex.Message);
                            break;
                        }
                    case ConsoleKey.Escape:
                        return null;
                    case ConsoleKey.Enter:
                        string input = ScanfInput(
                            (fillPoints[arrow.RelativePosition].Length + additional_line_len,
                            _up_offset + arrow.RelativePosition));
                        if (input != null)
                            answers[arrow.RelativePosition] = input;
                        break;
                    default:
                        ConsoleArrow.MoveCases(arrow, key);
                        break;
                }
            }
        }
        //Авторизация
        public static User Autorization()
        {
            PrintHeader(
                "F1 - enter",
                "Enter - fill",
                "Esc - back");

            string[] autorizationPoints = { "Login", "Password"};
            string[] answers = new string[autorizationPoints.Length];
            User user;
            Authorization aut = new Authorization();

            foreach (string point in autorizationPoints)
                Console.WriteLine(interval + point + pls_enter);
            ConsoleArrow arrow = ConsoleArrow.GetNew(autorizationPoints.Length);

            while (true)
            {
                var key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.Enter:
                        string input;
                        (int, int) scanfPoint = (autorizationPoints[arrow.RelativePosition].Length + additional_line_len, _up_offset + arrow.RelativePosition);

                        input = (arrow.RelativePosition == 0) ? 
                            ScanfInput(scanfPoint) : 
                            ScanfPassword(scanfPoint);

                        if (input != null)
                            answers[arrow.RelativePosition] = input;
                        break;
                    case ConsoleKey.F1:
                        if (answers.Contains(null))
                        {
                            PrintMsg("All fields must be filled in");
                            break;
                        }
                        user = aut.Autorize(answers[0], answers[1]);
                        if (user == null)
                        {
                            PrintMsg("User not found");
                            break;
                        }
                        _currentUser = user;
                        return user;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                    default:
                        ConsoleArrow.MoveCases(arrow, key);
                        break;
                }
            }
        }

        //Печатает заголовок
        private static void PrintHeader()
        {
            Console.Clear();
            if (_currentUser != null)
                Console.WriteLine("{0,-60} {1,30}", "Welcome, " + _currentUser.GetTrueName(), "Role: " + _currentUser.Role.ToString());
            else
                Console.WriteLine("6 Shop");
            Console.WriteLine(new string('=',100));
        }
        //Печатает заголовок и доступ комманды
        private static void PrintHeader(params string[] commands)
        {
            PrintHeader();
            PrintAvailableCommands(commands, _left_offset);
        }
        //Печатает доступ комманды
        private static void PrintAvailableCommands(string[] commands, int left_offset, int up_offset = 0)
        {
            int old_left_offset = Console.CursorLeft,
                old_up_offset = Console.CursorTop;

            for (int i = 0; i < commands.Length; i++)
            {
                Console.SetCursorPosition(left_offset, i + up_offset);
                Console.Write($"|{commands[i]}");
            }

            Console.SetCursorPosition(old_left_offset, old_up_offset);
        }

        //Считать строку в определённом месте на экране
        private static string ScanfInput((int, int) readAreaPoint)
        {
            int old_left_offset = Console.CursorLeft,
                old_up_offset = Console.CursorTop;

            Console.SetCursorPosition(readAreaPoint.Item1, readAreaPoint.Item2);
            Console.Write(new string(' ', 20));
            Console.SetCursorPosition(readAreaPoint.Item1, readAreaPoint.Item2);

            string toRet = Console.ReadLine();

            Console.SetCursorPosition(old_left_offset, old_up_offset);

            if (string.IsNullOrEmpty(toRet))
                toRet = null;

            return toRet;
        }
        //Вывести предлог и считать строку 
        private static string ScanfInput(string preposition)
        {
            int old_left_offset = Console.CursorLeft,
                old_up_offset = Console.CursorTop;

            Console.SetCursorPosition(old_left_offset, old_up_offset + 2);

            Console.Write(preposition);
            string toRet = Console.ReadLine();

            Console.SetCursorPosition(old_left_offset, old_up_offset);

            if (string.IsNullOrEmpty(toRet))
                toRet = null;

            return toRet;
        }
        //Считать пароль
        private static string ScanfPassword((int, int) readAreaPoint)
        {
            int old_left_offset = Console.CursorLeft,
                old_up_offset = Console.CursorTop;

            Console.SetCursorPosition(readAreaPoint.Item1, readAreaPoint.Item2);
            Console.Write(new string(' ', 20));
            Console.SetCursorPosition(readAreaPoint.Item1, readAreaPoint.Item2);

            string toRet = null;

            while (true)
            {
                ConsoleKeyInfo keyInf = Console.ReadKey(true);
                if (keyInf.Key == ConsoleKey.Enter)
                    break;
                else
                {
                    if (char.IsLetterOrDigit(keyInf.KeyChar) || char.IsSymbol(keyInf.KeyChar))
                    {
                        Console.Write('*');
                        toRet += keyInf.KeyChar;
                    }
                }
            }

            Console.SetCursorPosition(old_left_offset, old_up_offset);

            if (string.IsNullOrEmpty(toRet))
                toRet = null;

            return toRet;
        }
        //Вывести сообщение
        public static void PrintMsg(string msg)
        {
            int old_left_offset = Console.CursorLeft,
                old_up_offset = Console.CursorTop;

            Console.WriteLine("\n\n\n\n" + msg);

            Console.SetCursorPosition(old_left_offset, old_up_offset);
        }
        //Вывести сообщение в определённом месте на экране
        public static void PrintMsgOnPosition(string msg, (int,int) point, int lenght)
        {
            int old_left_offset = Console.CursorLeft,
    old_up_offset = Console.CursorTop;
            Console.SetCursorPosition(point.Item1, point.Item2);

            Console.Write("{0,"+ lenght + "}", msg);

            Console.SetCursorPosition(old_left_offset, old_up_offset);
        }
    }
}
