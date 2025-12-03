using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RegIN_Прохоров_Ожгибесов.Pages
{
    /// <summary>
    /// Логика взаимодействия для Confirmation.xaml
    /// </summary>
    public partial class Confirmation : Page
    {
        public enum TypeConfirmation
        {
            Login,
            Regin
        }

        TypeConfirmation ThisTypeConfirmation;

        public int Code = 0;
        public Confirmation(TypeConfirmation typeConfirmation)
        {
            InitializeComponent();

            ThisTypeConfirmation = typeConfirmation;

            SendMailCode();
        }

        public void SendMailCode()
        {
            Code = new Random().Next(100000, 999999);
            Classes.SendMail.SendMessage($"Login code: {Code}", MainWindow.mainWindow.UserLogIn.Login);

            Thread TSendMailCode = new Thread(TimerSendMailCode);
            TSendMailCode.Start();
        }

        public void TimerSendMailCode()
        {
            for (int i = 0; i < 60; i++)
            {
                Dispatcher.Invoke(() =>
                {
                    LTimer.Content = $"A second message can be sent after {(60 - i)} seconds";
                });
                Thread.Sleep(1000);
            }

            Dispatcher.Invoke(() =>
            {
                BSendMessage.IsEnabled = true;
                LTimer.Content = "";
            });
        }

        private void OpenLogin(object sender, MouseButtonEventArgs e)
        {
            MainWindow.mainWindow.OpenPage(new Login());
        }

        private void SendMail(object sender, RoutedEventArgs e)
        {
            SendMailCode();
        }

        private void SetCode(object sender, RoutedEventArgs e) =>
            SetCode();

        private void SetCode(object sender, KeyEventArgs e)
        {
            if (TbCode.Text.Length == 6)
                SetCode();
        }

        private void SetCode()
        {
            if (TbCode.Text == Code.ToString() && TbCode.IsEnabled == true)
            {
                TbCode.IsEnabled = false;

                if (ThisTypeConfirmation == TypeConfirmation.Login)
                    MessageBox.Show("Авторизация пользователя успешно подтверждена.");
                else
                {
                    MainWindow.mainWindow.UserLogIn.SetUser();
                    MessageBox.Show("Регистрация пользователя успешно подтверждена.");
                }
            }
        }
    }
}
