using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Aero_Test.ViewModel;

namespace Aero_Test
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainWindowVM VM = new MainWindowVM();
            this.DataContext = VM;
            if (File.Exists("Planes.txt") && File.Exists("Flights.txt"))
                VM.LoadInfo("Planes.txt", "Flights.txt");
            else
                MessageBox.Show("Файлы списков самолетов/расписания не найдены. Загрузите вручную");
        }
    }
}
