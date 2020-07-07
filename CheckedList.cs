using System;

namespace TextEditor
{
    /// <summary>
    /// Класс предоставляющий массив алгаритмов (стратегий) поведения
    /// в том же порядке, что и их описание в checkedListBox1.Items
    /// </summary>
    class CheckedList
    {
        /// <summary>
        /// Свойство позволяющее получить массив алгаритмов (стратегий) поведения
        /// в том же порядке, что и их описание в checkedListBox1.Items
        /// </summary>
        public ITextOperation[] OperationsList { get; set; }
        private byte _number;

        /// <summary>
        /// Конструктор класса, служит для инициализации массива
        /// </summary>
        /// /// <param name="number">Максимальное число букв в удаляемом слове</param>
        public CheckedList(byte number)
        {
            _number = number;
            OperationsList = new ITextOperation[]
           {
                new PunctuationMarkEraser(),
                new WordEraser(_number)
           };
        }
    }
}
