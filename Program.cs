using System;
using System.Windows.Forms;

namespace MyWindowsFormsApp
{
    static class Program
    {
        // Точка входа для приложения.
        [STAThread]
        static void Main()
        {
            // Включение визуальных стилей для приложения.
            Application.EnableVisualStyles();
            // Установка совместимости с рендерингом текста.
            Application.SetCompatibleTextRenderingDefault(false);
            
            Application.Run(new MainForm());
        }
    }
}