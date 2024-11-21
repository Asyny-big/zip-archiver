using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace MyWindowsFormsApp
{
    public partial class MainForm : Form
    {
        // Поле для хранения пути к текущему ZIP-файлу.
        private string currentZipFilePath = string.Empty;

        // Конструктор формы.
        public MainForm()
        {
            InitializeComponent(); // Инициализация компонентов формы.
            LoadDrives(); // Загрузка списка дисков в TreeView.
        }

        // Загрузка списка дисков в TreeView.
        private void LoadDrives()
        {
            treeView.Nodes.Clear(); // Очистка всех узлов в TreeView.
            foreach (var drive in DriveInfo.GetDrives()) // Перебор всех доступных дисков.
            {
                var node = new TreeNode(drive.Name) { Tag = drive.Name }; // Создание узла для каждого диска.
                node.Nodes.Add("..."); // Добавление дочернего узла-заглушки.
                treeView.Nodes.Add(node); // Добавление узла в TreeView.
            }
        }

        // Обработка события перед раскрытием узла TreeView.
        private void TreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            var node = e.Node; // Получение узла, который будет раскрыт.
            node.Nodes.Clear(); // Очистка всех дочерних узлов.
            var path = node.Tag as string; // Получение пути из тега узла.
            if (path == null) return; // Если путь равен null, выход из метода.

            try
            {
                foreach (var dir in Directory.GetDirectories(path)) // Перебор всех подкаталогов.
                {
                    var dirNode = new TreeNode(Path.GetFileName(dir)) { Tag = dir }; // Создание узла для каждого подкаталога.
                    dirNode.Nodes.Add("..."); // Добавление дочернего узла-заглушки.
                    node.Nodes.Add(dirNode); // Добавление узла в текущий узел.
                }
                foreach (var file in Directory.GetFiles(path)) // Перебор всех файлов.
                {
                    var fileNode = new TreeNode(Path.GetFileName(file)) { Tag = file }; // Создание узла для каждого файла.
                    node.Nodes.Add(fileNode); // Добавление узла в текущий узел.
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Доступ запрещен."); // Показ сообщения об ошибке доступа.
            }
        }

        // Обработка события после выбора узла TreeView.
        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var node = e.Node; // Получение выбранного узла.
            var path = node.Tag as string; // Получение пути из тега узла.
            if (path == null) return; // Если путь равен null, выход из метода.

            listView.Items.Clear(); // Очистка всех элементов в ListView.
            try
            {
                if (Directory.Exists(path)) // Если путь указывает на каталог.
                {
                    foreach (var dir in Directory.GetDirectories(path)) // Перебор всех подкаталогов.
                    {
                        var item = new ListViewItem(Path.GetFileName(dir), 0) { Tag = dir }; // Создание элемента ListView для каждого подкаталога.
                        item.SubItems.Add(""); // Добавление пустого подэлемента.
                        item.SubItems.Add(Directory.GetLastWriteTime(dir).ToString()); // Добавление подэлемента с датой последнего изменения.
                        listView.Items.Add(item); // Добавление элемента в ListView.
                    }
                    foreach (var file in Directory.GetFiles(path)) // Перебор всех файлов.
                    {
                        var item = new ListViewItem(Path.GetFileName(file), 1) { Tag = file }; // Создание элемента ListView для каждого файла.
                        item.SubItems.Add(new FileInfo(file).Length.ToString()); // Добавление подэлемента с размером файла.
                        item.SubItems.Add(File.GetLastWriteTime(file).ToString()); // Добавление подэлемента с датой последнего изменения.
                        listView.Items.Add(item); // Добавление элемента в ListView.
                    }
                }
                else if (File.Exists(path)) // Если путь указывает на файл.
                {
                    try
                    {
                        DisplayFileContents(path); // Отображение содержимого файла.
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Произошла ошибка при открытии файла: {ex.Message}\n{ex.StackTrace}"); // Показ сообщения об ошибке.
                    }
                }
                else
                {
                    MessageBox.Show("Файл не найден или формат не поддерживается."); // Показ сообщения, если файл не найден или формат не поддерживается.
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Доступ запрещен.");
            }
        }

        // Обработка события активации элемента ListView.
        private void ListView_ItemActivate(object sender, EventArgs e)
        {
            var item = listView.SelectedItems[0]; // Получение выбранного элемента ListView.
            var path = item.Tag as string; // Получение пути из тега элемента.
            if (path == null) return; // Если путь равен null, выход из метода.

            if (Directory.Exists(path)) // Если путь указывает на каталог.
            {
                var node = FindNodeByPath(treeView.Nodes, path); // Поиск узла TreeView по пути.
                if (node != null)
                {
                    treeView.SelectedNode = node; // Установка выбранного узла.
                    node.Expand(); // Раскрытие узла.
                }
            }
            else if (File.Exists(path)) // Если путь указывает на файл.
            {
                try
                {
                    Process.Start(new ProcessStartInfo(path) { UseShellExecute = true }); // Открытие файла с использованием ассоциированной программы.
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка при открытии файла: {ex.Message}\n{ex.StackTrace}"); // Показ сообщения об ошибке.
                }
            }
            else if (!string.IsNullOrEmpty(currentZipFilePath)) // Если путь указывает на файл в ZIP-архиве.
            {
                try
                {
                    ExtractAndOpenZipEntry(path); // Извлечение и открытие файла из ZIP-архива.
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошла ошибка при открытии файла из архива: {ex.Message}\n{ex.StackTrace}"); // Показ сообщения об ошибке.
                }
            }
            else
            {
                MessageBox.Show("Файл не найден или формат не поддерживается."); // Показ сообщения, если файл не найден или формат не поддерживается.
            }
        }

        // Поиск узла TreeView по пути.
        private TreeNode? FindNodeByPath(TreeNodeCollection nodes, string path)
        {
            // Перебор всех узлов в коллекции.
            foreach (TreeNode node in nodes)
            {
                // Если тег узла совпадает с заданным путем.
                if (node.Tag as string == path)
                {
                    return node; // Возвращаем найденный узел.
                }
                // Рекурсивный вызов для поиска узла в дочерних узлах.
                var foundNode = FindNodeByPath(node.Nodes, path);
                if (foundNode != null)
                {
                    return foundNode; // Возвращаем найденный узел, если он найден в дочерних узлах.
                }
            }
            return null; // Возвращаем null, если узел не найден.
        }

        // Обработка события нажатия кнопки создания ZIP архива.
        private void CreateZipButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Создание и настройка диалогового окна для сохранения ZIP файла.
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "ZIP files (*.zip)|*.zip"; // Установка фильтра для отображения только ZIP файлов.
                    if (saveFileDialog.ShowDialog() == DialogResult.OK) // Если пользователь выбрал место для сохранения и нажал OK.
                    {
                        // Открытие ZIP архива для создания.
                        using (ZipArchive archive = ZipFile.Open(saveFileDialog.FileName, ZipArchiveMode.Create))
                        {
                            // Перебор всех выбранных элементов в ListView.
                            foreach (ListViewItem item in listView.SelectedItems)
                            {
                                var path = item.Tag as string; // Получение пути из тега элемента.
                                if (path != null)
                                {
                                    if (Directory.Exists(path)) // Если путь указывает на каталог.
                                    {
                                        // Добавление каталога в архив.
                                        AddDirectoryToZip(archive, path, Path.GetFileName(path));
                                    }
                                    else if (File.Exists(path)) // Если путь указывает на файл.
                                    {
                                        // Добавление файла в архив.
                                        archive.CreateEntryFromFile(path, Path.GetFileName(path));
                                    }
                                }
                            }
                            MessageBox.Show("Архив успешно создан."); // Показ сообщения об успешном создании архива.
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Показ сообщения об ошибке при создании архива.
                MessageBox.Show($"Произошла ошибка при создании архива: {ex.Message}\n{ex.StackTrace}");
            }
        }

        // Добавление директории в ZIP архив.
        private void AddDirectoryToZip(ZipArchive archive, string sourceDir, string entryName)
        {
            // Перебор всех файлов в исходной директории.
            foreach (var file in Directory.GetFiles(sourceDir))
            {
                // Создание записи в архиве для каждого файла.
                archive.CreateEntryFromFile(file, Path.Combine(entryName, Path.GetFileName(file)));
            }
            // Перебор всех подкаталогов в исходной директории.
            foreach (var dir in Directory.GetDirectories(sourceDir))
            {
                // Рекурсивное добавление подкаталогов в архив.
                AddDirectoryToZip(archive, dir, Path.Combine(entryName, Path.GetFileName(dir)));
            }
        }

        // Обработка события нажатия кнопки просмотра ZIP архива.
        private void ViewZipButton_Click(object sender, EventArgs e)
        {
            // Создание и настройка диалогового окна для выбора ZIP файла.
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "ZIP files (*.zip)|*.zip"; // Установка фильтра для отображения только ZIP файлов.
                if (openFileDialog.ShowDialog() == DialogResult.OK) // Если пользователь выбрал файл и нажал OK.
                {
                    currentZipFilePath = openFileDialog.FileName; // Сохранение пути к выбранному ZIP файлу.
                    DisplayZipContents(openFileDialog.FileName); // Отображение содержимого выбранного ZIP файла в ListView.
                }
            }
        }

        // Отображение содержимого ZIP архива в ListView.
        private void DisplayZipContents(string zipPath)
        {
            listView.Items.Clear(); // Очистка всех элементов в ListView.
            try
            {
                // Открытие ZIP архива для чтения.
                using (ZipArchive archive = ZipFile.OpenRead(zipPath))
                {
                    // Перебор всех записей (файлов) в архиве.
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        // Создание элемента ListView для каждой записи.
                        var item = new ListViewItem(entry.FullName) { Tag = entry.FullName };
                        // Добавление подэлемента с размером записи.
                        item.SubItems.Add(entry.Length.ToString());
                        // Добавление подэлемента с датой последнего изменения записи.
                        item.SubItems.Add(entry.LastWriteTime.ToString());
                        // Добавление элемента в ListView.
                        listView.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                // Показ сообщения об ошибке при открытии архива.
                MessageBox.Show($"Произошла ошибка при открытии архива: {ex.Message}\n{ex.StackTrace}");
            }
        }

        // Отображение содержимого файла.
        private void DisplayFileContents(string filePath)
        {
            listView.Items.Clear(); // Очистка всех элементов в ListView.
            var extension = Path.GetExtension(filePath).ToLower(); // Получение расширения файла в нижнем регистре.
            if (extension == ".zip")
            {
                DisplayZipContents(filePath); // Если файл является ZIP архивом, отображение его содержимого.
            }
            else
            {
                try
                {
                    // Открытие файла с использованием ассоциированной программы.
                    Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    // Показ сообщения об ошибке при открытии файла.
                    MessageBox.Show($"Произошла ошибка при открытии файла: {ex.Message}\n{ex.StackTrace}");
                }
            }
        }

        // Извлечение и открытие файла из ZIP архива.
        private void ExtractAndOpenZipEntry(string entryName)
        {
            try
            {
                // Открытие ZIP архива для чтения.
                using (ZipArchive archive = ZipFile.OpenRead(currentZipFilePath))
                {
                    // Получение записи (файла) из архива по имени.
                    var entry = archive.GetEntry(entryName);
                    if (entry != null)
                    {
                        // Создание пути для временного файла.
                        var tempPath = Path.Combine(Path.GetTempPath(), entry.FullName);
                        // Получение директории временного файла.
                        var tempDir = Path.GetDirectoryName(tempPath);
                        if (tempDir != null)
                        {
                            // Создание директории, если она не существует.
                            Directory.CreateDirectory(tempDir);
                            // Извлечение файла из архива в временную директорию.
                            entry.ExtractToFile(tempPath, true);
                            // Открытие извлеченного файла с использованием ассоциированной программы.
                            Process.Start(new ProcessStartInfo(tempPath) { UseShellExecute = true });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Показ сообщения об ошибке при извлечении и открытии файла из архива.
                MessageBox.Show($"Произошла ошибка при извлечении и открытии файла из архива: {ex.Message}\n{ex.StackTrace}");
            }
        }

        // Обработка события нажатия кнопки извлечения ZIP архива.
        private void ExtractZipButton_Click(object sender, EventArgs e)
        {
            // Создание и настройка диалогового окна для выбора ZIP файла.
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "ZIP files (*.zip)|*.zip"; // Установка фильтра для отображения только ZIP файлов.
                if (openFileDialog.ShowDialog() == DialogResult.OK) // Если пользователь выбрал файл и нажал OK.
                {
                    // Создание и настройка диалогового окна для выбора папки.
                    using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
                    {
                        if (folderBrowserDialog.ShowDialog() == DialogResult.OK) // Если пользователь выбрал папку и нажал OK.
                        {
                            try
                            {
                                // Извлечение содержимого выбранного ZIP файла в выбранную папку.
                                ZipFile.ExtractToDirectory(openFileDialog.FileName, folderBrowserDialog.SelectedPath);
                                MessageBox.Show("Архив успешно извлечен."); // Показ сообщения об успешном извлечении архива.
                            }
                            catch (Exception ex)
                            {
                                // Показ сообщения об ошибке при извлечении архива.
                                MessageBox.Show($"Произошла ошибка при извлечении архива: {ex.Message}\n{ex.StackTrace}");
                            }
                        }
                    }
                }
            }
        }

        // Обработка события изменения ширины столбца ListView.
        private void ListView_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            // Разрешить пользователю изменять ширину столбца.
            e.Cancel = false;
        }

        // Обработка события нажатия кнопки "О себе".
        private void AboutButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Разработчик: Котов Александр Денисович\nГруппа: О737Б\nEmail: o737b15@voenmeh.ru", "О себе");
        }
    }
}