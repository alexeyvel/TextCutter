namespace TextEditor
{
    /// <summary>
    /// Интерфейс, объявляющий метод, общий для всех
    /// поддерживаемых версий некоторого алгоритма
    /// </summary>
    interface ITextOperation
    {
        string DoModifyText(string text);
    }
}
