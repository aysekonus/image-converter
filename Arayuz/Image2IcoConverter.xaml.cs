using System.IO;
using System.Windows;
using Microsoft.Win32;
using Image2ICO_Converter.Sınıflar;

namespace Image2ICO_Converter
{
    /// <summary>
    /// Interaction logic for Image2IcoConverter.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PNG Files (*.png)|*.png"
            };

            if (true == openFileDialog.ShowDialog())
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter      = "ICO Files (*.ico)|*.ico",
                    FileName    = Path.GetFileNameWithoutExtension(openFileDialog.FileName) + "_icon"
                };

                if (true == saveFileDialog.ShowDialog())
                {
                    if (true == Donusturucu.GetInstance.PNG2ICO(openFileDialog.FileName, saveFileDialog.FileName, 64))
                    {
                        MessageBox.Show("Dönüştürme tamamlandı.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }
    }
}