using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace RegIN_Прохоров_Ожгибесов.Pages
{
    /// <summary>
    /// Логика взаимодействия для Regin.xaml
    /// </summary>
    public partial class Regin : Page
    {

        OpenFileDialog FileDialogImage = new OpenFileDialog();
        bool BCorrectLogin = false;
        bool BCorrectPassword = false;  
        bool BcorrectConfirmPassword = false;
        bool BSetImage = false;
        public Regin()
        {
            InitializeComponent();
            MainWindow.mainWindow.UserLogIn.HandlerInCorrectLogin += CorrectLogin;
            MainWindow.mainWindow.UserLogIn.HandlerInCorrectLogin += InCorrectLogin;
            FileDialogImage.Filter = "PNG (*.png)|*.png|JPG (*.jpg)|*.jpg";
            FileDialogImage.RestoreDirectory = true;
            FileDialogImage.Title = "Choose a phoro for your avatar";
        }
        private void CorrectLogin()
        {
            SetNotification("lOGIN ALREADY IN USER", Brushes.Red);
            BCorrectLogin = false ;

        }
        private void InCorrectLogin() =>
            SetNotification ("",Brushes.Black);
        private void SetLogin(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                SetLogin();
            }
        }

        private void SetLogin(object sender, RoutedEventArgs e)
        {
            SetLogin();
        }
        public void SetLogin()
        {
            Regex regex = new Regex(@"^([a-zA-Z0-9_-]{4,8}@{2,}.[a-zA-Z0-9_-]{2,})$");
            BCorrectLogin = regex.IsMatch(TbLogin.Text);
            if (regex.IsMatch(TbLogin.Text) == true)
            {
                SetNotification("", Brushes.Black);
                MainWindow.mainWindow.UserLogIn.GetUserLogin(TbLogin.Text);
            }
            else
            {
                SetNotification("Invalid login", Brushes.Red);
            }

            OnRegIn();
        }

        private void SetName(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsLetter(e.Text, 0));
        }

        private void SetPassword(object sender, KeyEventArgs e)
        {
           if(e.Key == Key.Enter)
            {
                SetPassword();
            }
        }

        private void SetPassword(object sender, RoutedEventArgs e)
        {
            SetPassword();
        }
        public void SetPassword()
        {
            Regex regex = new Regex(@"^(?=.[0-9])(?=.[!@#%^&*\-_]{10,}$");
            BCorrectPassword = regex.IsMatch(TbPassword.Password);
            if (regex.IsMatch(TbPassword.Password) == true)
            {
                SetNotification("", Brushes.Black);
                if (tbConfirmPassword.Password.Length > 0)
                    ConfirmPassword(true);
                OnRegIn();
            }
            else
            {
                SetNotification("Invalid password", Brushes.Red);
            }
        }

        private void ConfirmPassword(object sender, KeyEventArgs e)
        {
           if(e.Key == Key.Enter)
            {
                ConfirmPassword();
            }
        }

        private void ConfirmPassword(object sender, RoutedEventArgs e)
        {
            ConfirmPassword();
        }
        public void ConfirmPassword(bool Pass = false)
        {
            BcorrectConfirmPassword = tbConfirmPassword.Password == TbPassword.Password;
            if(tbConfirmPassword.Password != TbPassword.Password)
            {
                SetNotification("Passwords do not match", Brushes.Black);
                if(!Pass) 
                    SetPassword();
            }
        }
        private void OpenLogin(object sender, MouseButtonEventArgs e)
        {

        }
        void OnRegIn()
        {
            if (!BCorrectLogin)
                return;
            if (TbName.Text.Length == 0)
                return;
            if (!BCorrectPassword)
                return;
            if (!BcorrectConfirmPassword)
                return;
            MainWindow.mainWindow.UserLogIn.Login = TbLogin.Text;
            MainWindow.mainWindow.UserLogIn.Password = TbPassword.Password;
            MainWindow.mainWindow.UserLogIn.Name = TbName.Text;

            if (BSetImage)
                MainWindow.mainWindow.UserLogIn.Image = File.ReadAllBytes(Directory.GetCurrentDirectory() + @"\User.jpg");

            MainWindow.mainWindow.UserLogIn.DateUpdate = DateTime.Now;
            MainWindow.mainWindow.UserLogIn.DateCreate = DateTime.Now;

            MainWindow.mainWindow.OpenPage(new Confirmation(Confirmation.TypeConfirmation.Regin));
        }
        private void OpenRegin(object sender, MouseButtonEventArgs e)
        {

        }


        public void SetNotification(string Message, SolidColorBrush _Color)
        {
            LNameUser.Content = Message;
            LNameUser.Foreground = _Color;
        }
    }
}
