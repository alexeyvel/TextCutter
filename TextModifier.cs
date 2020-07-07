using System;

namespace TextEditor
{
    /// <summary>
    /// Класс - контекст, определяет интерфейс необходимый в клиентской части программы 
    /// </summary>
    class TextModifier
    {
        private ITextOperation _operation;
        public TextModifier(){}

        /// <summary>
        /// Перегруженный конструктор, принимает как параметр экземпляр алгаритма (стратегии) поведения 
        /// </summary>
        /// <param name="operation">Экземпляр алгаритма (стратегии) поведения</param>
        public TextModifier(ITextOperation operation)
        {
            _operation = operation;
        }

        /// <summary>
        /// Метод позваляющий менять алгаритмы (стратегии) поведения в рантайме  
        /// </summary>
        /// <param name="operation">Экземпляр алгаритма (стратегии) поведения</param>
        public void SetOperation(ITextOperation operation)
        {
            _operation = operation;
        }

        /// <summary>
        /// Метод выполняющий тот или иной алгаритм согласно выбраной заранее стратегии поведения  
        /// </summary>
        /// <param name="text">Входной текстовый параметр для преобразования</param>
        /// <returns>Текст после модификации</returns>
        public string ExecuteOperation(string text)
        {
            if (_operation == null)
                throw new NotImplementedException();
            return _operation.DoModifyText(text);
        }
    }
}
