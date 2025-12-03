using System.Windows;
using System.Windows.Controls;

namespace RegIN_Прохоров_Ожгибесов.Pages
{
    /// <summary>
    /// Логика взаимодействия для Pin.xaml
    /// </summary>
    public partial class Pin : Page
    {
        bool isHasPin;
        public Pin(bool isHasPin)
        {
            InitializeComponent();

            this.isHasPin = isHasPin;
        }

        private void SetPin(object sender, RoutedEventArgs e)
        {
            if (isHasPin == true)
            {
                if (TbPin.Text == MainWindow.mainWindow.UserLogIn.Pincode)
                    MessageBox.Show("Успешная авторизация");
                else
                    MessageBox.Show("Не правильный пин-код");
            }
            else
            {
                if (!string.IsNullOrEmpty(TbPin.Text) && TbPin.Text.Length == 4)
                {
                    MainWindow.mainWindow.UserLogIn.SetPin(TbPin.Text);

                    MessageBox.Show("Пин-код успешно установлен");
                }
                else MessageBox.Show("Неккоректный пин код");
            }
        }

        private void SetCode(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }

        private void Miss(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MainWindow.mainWindow.frame.GoBack();
        }
    }
}
