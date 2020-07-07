using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditor
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Свойство принимающее поток из считанного файла возвращающее данные после преобразования.
        /// </summary>
        private string tempTextBuffer;
        private List<Stream> streams;
        private List<string> textBuffer;
        private List<string> fileNames;
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "Откройте меню File и выбирете файлы для редактирования" + Environment.NewLine;
        }
        private void UIControlElementsEnabled(bool flag)
        {
            checkedListBox1.Enabled = flag;
            menuStrip1.Enabled = flag;
            startButton.Enabled = flag;
            refreshButton.Enabled = flag;
            trackBar1.Enabled = flag;
        }
        //Метод UI обеспечивающий выбор и открытие файлов в стандартном диалоговом окне выбора
        //При успешном выборе файла выводит имя выбранного файла в текстовое поле, делает доступным фильтры 
        //выбора операций для редактирования и меню сохранения, 
        //считывает открытый файл в специальную переменную TextBuffer
        private async void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                checkedListBox1.Enabled = true;
                streams = new List<Stream>();
                textBuffer = new List<string>();
                fileNames = new List<string>();
                foreach (string file in openFileDialog1.FileNames)
                {
                    openFileDialog1.FileName = file;
                    streams.Add(openFileDialog1.OpenFile());
                    fileNames.Add(file);
                }
                for (int i = 0; i < streams.Count; i++)
                {
                    try
                    {
                        using (StreamReader reader = new StreamReader(streams[i], Encoding.Default))
                        {
                            tempTextBuffer = await reader.ReadToEndAsync();
                            textBuffer.Add(tempTextBuffer);
                        }
                        textBox1.Text += $"Файл: {fileNames[i]} - успешно загружен" + Environment.NewLine;
                    }
                    catch (Exception ex)
                    {
                        textBox1.BackColor = Color.Red;
                        textBox1.Text = ex.Message;
                    }
                }
                saveToolStripMenuItem.Enabled = true;
                textBox1.Text += $"Выберите из фильтра список необходимых операций и нажмите кнопку \"{startButton.Text}\""
                    + Environment.NewLine; 
            }
        }

        //Метод UI обеспечивающий сохранение данных из переменной TextBuffer в файл на диске.
        private async void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < textBuffer.Count; i++)
            {
                saveFileDialog1.FileName = fileNames[i].Replace(".txt", "_modifed.txt");
                string filename = saveFileDialog1.FileName;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(filename, false))
                        {
                            await writer.WriteAsync(textBuffer[i]);
                            textBox1.Text = $"Файл {filename} успешно сохранен" + Environment.NewLine;
                        }
                    }
                    catch (Exception ex)
                    {
                        textBox1.BackColor = Color.Red;
                        textBox1.Text = ex.Message;
                    }
                }
            }
        }

        //Метод UI, обеспечивающий выбор пользователем списка алгоритмов обработки данных.
        //дополнительно, в зависимости от выбора делает доступными/заблокированными некторые другие элементы UI (например трекбар, кнопку запуска)
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var indicesList = checkedListBox1.CheckedIndices;
            if (indicesList.Contains(0) && indicesList.Count == 1 || indicesList.Count == 0)
            {
                trackBar1.Enabled = false;
                label2.Text = "Выбрано букв:";
            }
            else
            {
                trackBar1.Enabled = true;
                label2.Text = $"Выбрано букв: {trackBar1.Value}";
            }
            if (indicesList.Count > 0)
                startButton.Enabled = true;
            else startButton.Enabled = false;
        }

        //Метод UI, отрабатывающий нажатие на  кнопуц "Выполнить" и запускающий выполнение операций, которые были выбраны в фильтре выбора операций.
        //Обеспечивает выбор списка алгоритмов обработки данных на основе данных от пользователя.
        //дополнительно делает доступными/заблокированными некторые другие элементы UI (например трекбар, кнопку обнавления, кнопку работы с файлами, фильтр выбора операций)
        private async void startButton_Click(object sender, EventArgs e)
        {
            var indicesList = checkedListBox1.CheckedIndices;
            textBox1.Clear();
            textBox1.Text = "Идет орбаботка, ожидайте..." + Environment.NewLine;
            UIControlElementsEnabled(false);
            var listModifer = new CheckedList((byte)trackBar1.Value);
            for (int j = 0; j < textBuffer.Count; j++)
            {
                for (int i = 0; i < indicesList.Count; i++)
                {
                    var textModifer = new TextModifier(listModifer.OperationsList[i]);
                    await Task.Run(() => { textBuffer[j] = textModifer.ExecuteOperation(textBuffer[j]); });
                }
                textBox1.Text += $"Операция над {fileNames[j]} выполнена." + Environment.NewLine;
            }
            UIControlElementsEnabled(true);
            textBox1.Text += "Результат операции можно сохранить в файл";
        }

        //Метод UI, определяющий число букв для операции удаления слова.
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label2.Text = $"Выбрано букв: {trackBar1.Value}";
        }

        //Метод UI, отрабатывающий нажатие на  кнопуц "Обновить", и возвращающий программу в начальное состояние.
        //дополнительно делает доступными/заблокированными некторые другие элементы UI (например трекбар, кнопку выполнения, кнопку работы с файлами, фильтр выбора операций)
        private void refreshButton_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            trackBar1.Value = trackBar1.Minimum;
            checkedListBox1.Enabled = false;
            startButton.Enabled = false;
            trackBar1.Enabled = false;                    
            saveToolStripMenuItem.Enabled = false;
            var indicesList = checkedListBox1.Items;
            for (int i = 0; i < indicesList.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }
            tempTextBuffer = null;
            if (streams != null)
            {
                foreach(var stream in streams)
                    stream.Dispose();
            }
            if (fileNames != null)
            {
                for (int i = 0; i < fileNames.Count; i++)
                {
                    fileNames.RemoveAt(i);
                    textBuffer.RemoveAt(i);
                }
            }
            label2.Text = "Выбрано букв:";
            textBox1.Text = "Откройте меню File и выбирете файлы для редактирования" + Environment.NewLine;
        }
    }
}
