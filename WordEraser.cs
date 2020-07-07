using System.Text;
using System.Text.RegularExpressions;

namespace TextEditor
{
    /// <summary>
    /// Класс реализующий удаление всех слов длинной от 1 до N символов 
    /// </summary>
    class WordEraser : ITextOperation
    {
        private byte _number;
        /// <summary>
        /// Шаблон регулярного выражения, обеспечивающий выбор всех слов 
        /// в тексте с заданным колличеством букв
        /// </summary>
        private string pattern = @"\b[\p{L}\d]{1,NUMBER}\b";

        /// <summary>
        /// Конструктор класса. Производит инициализацию поля _number,
        /// отвечающего за верхнее колличество символов в удаляемом слове 
        /// </summary>
        /// <param name="number">Максимальное число букв в удаляемом слове</param>
        public WordEraser(byte number)
        {
            _number = number;
        }
        /// <summary>
        /// Метод производящий модификации с текстом: удаление всех слов
        /// длинной от 1 до N символов 
        /// </summary>
        /// <param name="text">Текст который необходимо модифицировать</param>
        /// <returns>Текст после модификации</returns>
        public string DoModifyText(string text)
        {
            string newPattern = pattern.Replace("NUMBER", _number.ToString());
            Regex regex = new Regex(newPattern);
            StringBuilder stringBuilder = new StringBuilder(text);
            string tempText = regex.Replace(stringBuilder.ToString(), "");
            return tempText;
        }
    }
}
