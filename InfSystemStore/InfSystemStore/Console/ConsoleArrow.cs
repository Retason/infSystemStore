using System;

namespace InfSystemStore
{
    class ConsoleArrow
    {
        private const string arrow_symb = "->";
        //Направление движения
        public enum Move
        {
            Up,
            Down
        }

        int _min, _max;

        //Позиция в консоли
        private int _position;
        //Позиция относительно минимума
        public int RelativePosition { get => _position - _min; }

        private ConsoleArrow(int min, int max)
        {
            _min = min;
            _max = max;

            _position = min;
            WriteOnPosition(arrow_symb);
        }

        //Инициализации новой стрелки
        public static ConsoleArrow GetNew(int listLenght)
        {
            int max = Console.CursorTop - 1;
            int min = max - (listLenght - 1);
            return new ConsoleArrow(min, max);
        }

        //Перерисовать стрелку
        public void RePrint(Move direction)
        {
            WriteOnPosition("  ");
            switch (direction)
            {
                case Move.Up:
                    if (_position - 1 >= _min)
                        _position--;
                    break;
                case Move.Down:
                    if (_position + 1 <= _max)
                        _position++;
                    break;
            }
            WriteOnPosition(arrow_symb);
        }

        //Логика движения
        public static void MoveCases(ConsoleArrow arrow, ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    arrow.RePrint(ConsoleArrow.Move.Up);
                    break;
                case ConsoleKey.DownArrow:
                    arrow.RePrint(ConsoleArrow.Move.Down);
                    break;
            }
        }

        //Отобразить строку на позиции
        private void WriteOnPosition(string value)
        {
            int old_left_offset = Console.CursorLeft,
                old_up_offset = Console.CursorTop;

            Console.SetCursorPosition(0, _position);
            Console.Write(value);

            Console.SetCursorPosition(old_left_offset, old_up_offset);
        }
    }
}
