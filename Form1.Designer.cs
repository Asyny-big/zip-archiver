namespace MyWindowsFormsApp
{
    partial class MainForm
    {
        // Контейнер для компонентов формы.
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button aboutButton;

        // Метод для освобождения ресурсов, занятых формой.
        protected override void Dispose(bool disposing)
        {
            // Если disposing равно true и components не равно null,
            // освобождаем все компоненты, управляемые контейнером components.
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            // Вызов базового метода Dispose для выполнения стандартного освобождения ресурсов.
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        private void InitializeComponent()
        {
            // Создание и настройка элементов интерфейса.
            this.treeView = new System.Windows.Forms.TreeView();
            this.listView = new System.Windows.Forms.ListView();
            this.createZipButton = new System.Windows.Forms.Button();
            this.viewZipButton = new System.Windows.Forms.Button();
            this.extractZipButton = new System.Windows.Forms.Button();
            this.aboutButton = new System.Windows.Forms.Button();
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.buttonPanel.SuspendLayout(); // Приостановка макета панели для предотвращения обновлений до завершения настройки.
            this.SuspendLayout(); // Приостановка макета формы
            //
            // treeView
            // Настройка TreeView для отображения структуры каталогов.
            this.treeView.Location = new System.Drawing.Point(12, 50);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(200, 400);
            this.treeView.TabIndex = 0;
            this.treeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeView_BeforeExpand); // Добавление обработчика события BeforeExpand.
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_AfterSelect);
            this.treeView.Dock = System.Windows.Forms.DockStyle.Left; // Закрепление TreeView слева на форме.
            // 
            // listView
            // Настройка ListView для отображения содержимого каталогов и файлов.
            this.listView.Location = new System.Drawing.Point(218, 50); 
            this.listView.Name = "listView"; 
            this.listView.Size = new System.Drawing.Size(570, 400);
            this.listView.TabIndex = 1; 
            this.listView.View = System.Windows.Forms.View.Details; // Установка режима отображения ListView в виде деталей.
            this.listView.FullRowSelect = true; // Включение выбора всей строки в ListView.
            this.listView.MultiSelect = true; // Включение множественного выбора в ListView.
            this.listView.ItemActivate += new System.EventHandler(this.ListView_ItemActivate); // Добавление обработчика события ItemActivate.
            this.listView.Columns.Add("Имя", 200); 
            this.listView.Columns.Add("Размер", 100); 
            this.listView.Columns.Add("Дата изменения", 100); 
            this.listView.HeaderStyle = ColumnHeaderStyle.Clickable; // Установка стиля заголовков колонок как кликабельных.
            this.listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize); // Автоматическое изменение размера колонок по размеру заголовков.
            this.listView.AllowColumnReorder = true; // Разрешение перетаскивания колонок в ListView.
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill; // Закрепление ListView, чтобы он занимал оставшееся пространство формы.
            // 
            // createZipButton
            // Кнопка для создания ZIP архива.
            this.createZipButton.Location = new System.Drawing.Point(12, 12);
            this.createZipButton.Name = "createZipButton";
            this.createZipButton.Size = new System.Drawing.Size(75, 23);
            this.createZipButton.TabIndex = 2;
            this.createZipButton.Text = "Создать Zip";
            this.createZipButton.UseVisualStyleBackColor = true; // Включение использования стиля кнопки по умолчанию.
            this.createZipButton.Click += new System.EventHandler(this.CreateZipButton_Click); // Добавление обработчика события Click для кнопки.
            // 
            // viewZipButton
            // Кнопка для просмотра содержимого ZIP архива.
            this.viewZipButton.Location = new System.Drawing.Point(93, 12);
            this.viewZipButton.Name = "viewZipButton";
            this.viewZipButton.Size = new System.Drawing.Size(75, 23);
            this.viewZipButton.TabIndex = 3;
            this.viewZipButton.Text = "Просмотр Zip";
            this.viewZipButton.UseVisualStyleBackColor = true;
            this.viewZipButton.Click += new System.EventHandler(this.ViewZipButton_Click);
            // 
            // extractZipButton
            // Кнопка для извлечения содержимого ZIP архива.
            this.extractZipButton.Location = new System.Drawing.Point(174, 12);
            this.extractZipButton.Name = "extractZipButton";
            this.extractZipButton.Size = new System.Drawing.Size(75, 23);
            this.extractZipButton.TabIndex = 4;
            this.extractZipButton.Text = "Извлечь Zip";
            this.extractZipButton.UseVisualStyleBackColor = true;
            this.extractZipButton.Click += new System.EventHandler(this.ExtractZipButton_Click);
            // 
            // aboutButton
            // Кнопка для отображения информации о разработчике.
            this.aboutButton.Location = new System.Drawing.Point(255, 12);
            this.aboutButton.Name = "aboutButton";
            this.aboutButton.Size = new System.Drawing.Size(75, 23);
            this.aboutButton.TabIndex = 5;
            this.aboutButton.Text = "О себе";
            this.aboutButton.UseVisualStyleBackColor = true;
            this.aboutButton.Click += new System.EventHandler(this.AboutButton_Click);
            // 
            // buttonPanel
            // Панель для размещения кнопок управления.
            this.buttonPanel.Controls.Add(this.createZipButton); // Добавление кнопки createZipButton на панель.
            this.buttonPanel.Controls.Add(this.viewZipButton); // Добавление кнопки viewZipButton на панель.
            this.buttonPanel.Controls.Add(this.extractZipButton); // Добавление кнопки extractZipButton на панель.
            this.buttonPanel.Controls.Add(this.aboutButton); // Добавление кнопки aboutButton на панель.
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Top; // Закрепление панели сверху формы.
            this.buttonPanel.Location = new System.Drawing.Point(0, 0); // Установка местоположения панели на форме.
            this.buttonPanel.Name = "buttonPanel"; 
            this.buttonPanel.Size = new System.Drawing.Size(800, 50); 
            this.buttonPanel.TabIndex = 5; // Установка табуляционного индекса панели.
            // 
            // MainForm
            // Настройка главной формы приложения.
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.listView); // Добавление элемента управления listView на форму.
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.buttonPanel);
            this.Name = "MainForm";
            this.Text = "ZIP-архиватор";
            this.MinimumSize = new System.Drawing.Size(650, 500);
            this.ResumeLayout(false); // Возобновление макета формы.
            this.PerformLayout(); // Принудительное выполнение макета для всех дочерних элементов.
        }

        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.Button createZipButton;
        private System.Windows.Forms.Button viewZipButton;
        private System.Windows.Forms.Button extractZipButton;
        private System.Windows.Forms.Panel buttonPanel;

        #endregion
    }
}