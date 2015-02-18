using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BGR32ToBGR24
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = MainWindowVM = new MainWindowViewModel();
            VerificationTextBox.TextVerifier = new Func<string, bool?>((path) => System.IO.Directory.Exists(path));
            VerificationTextBox.TextIsValidChanged += VerificationTextBox_TextIsValidChanged;

            var assemblyName = Assembly.GetExecutingAssembly().GetName();
            var version = assemblyName.Version;
            if (version.Build != 0)
            {
                Title = string.Format("{0} {1}.{2}.{3}", assemblyName.Name, version.Major, version.Minor, version.Build);
            }
            else
            {
                Title = string.Format("{0} {1}.{2}", assemblyName.Name, version.Major, version.Minor);
            }
        }

        void VerificationTextBox_TextIsValidChanged(object sender, RoutedEventArgs e)
        {
            MainWindowVM.ScoutFromRoot();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindowVM.OpenFileDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //実行
            Task.Factory.StartNew(() =>
                {
                    MainWindowVM.DescendFromRoot();
                    MainWindowVM.ScoutFromRoot();
                });
        }

        internal MainWindowViewModel MainWindowVM { get; set; }
    }
}
