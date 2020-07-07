using System.Text;
using System.Text.RegularExpressions;

namespace TextEditor
{
    /// <summary>
    /// Класс реализующий удаление всех знаков препинания 
    /// </summary>
    class PunctuationMarkEraser : ITextOperation
    {
        /// <summary>
        /// Шаблон регулярного выражения, обеспечивающий выбор 
        /// всех знаков препинания в тексте
        /// </summary>
        private string pattern = @"\p{P}";

        /// <summary>
        /// Метод производящий модификации с текстом: удаление всех знаков препинания 
        /// </summary>
        /// <param name="text">Текст который необходимо модифицировать</param>
        /// <returns>Текст после модификации</returns>
        public string DoModifyText(string text)
        {
            Regex regex = new Regex(pattern);
            StringBuilder stringBuilder = new StringBuilder(text);
            for (int i = 0; i < stringBuilder.Length; i++)
            {
                if (regex.IsMatch(stringBuilder[i].ToString()))
                {
                    stringBuilder.Remove(i, 1);
                    i--;
                }       
            }           
            return stringBuilder.ToString();
        }
    }
}
