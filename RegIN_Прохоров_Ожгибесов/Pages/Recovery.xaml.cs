using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace RegIN_Прохоров_Ожгибесов.Pages
{
    /// <summary>
    /// Логика взаимодействия для Recovery.xaml
    /// </summary>
    public partial class Recovery : Page
    {
        /// <summary>
        /// Логин введенный пользователем
        /// </summary>
        string OldLogin;
        /// <summary>
        /// Переменная отвечающая за ввод капчи
        /// </summary>
        bool IsCapture = false;
        public Recovery()
        {
            InitializeComponent();

            MainWindow.mainWindow.UserLogIn.HandelCorrectLogin += CorrectLogin;
            MainWindow.mainWindow.UserLogIn.HandlerInCorrectLogin += InCorrectLogin;

            Capture.HandlerCorrectCapture += CorrectCapture;
        }

        private void CorrectLogin()
        {
            if (OldLogin != TbLogin.Text)
            {
                OldLogin = TbLogin.Text;

                SetNotification("Hi, " + MainWindow.mainWindow.UserLogIn.Name, Brushes.Black);
                SetAnim();
                SendNewPassword();
            }
        }

        private void SetAnim()
        {
            try
            {
                BitmapImage biImg = new BitmapImage();
                MemoryStream ms = new MemoryStream(MainWindow.mainWindow.UserLogIn.Image);

                biImg.BeginInit();
                biImg.StreamSource = ms;
                biImg.EndInit();

                DoubleAnimation StartAnimation = new DoubleAnimation();
                StartAnimation.From = 1;
                StartAnimation.To = 0;
                StartAnimation.Duration = TimeSpan.FromSeconds(0.6);
                StartAnimation.Completed += delegate
                {
                    SetImage(biImg);

                    DoubleAnimation EndAnimation = new DoubleAnimation();
                    EndAnimation.From = 0;
                    EndAnimation.To = 1;
                    EndAnimation.Duration = TimeSpan.FromSeconds(1.2);

                    IUser.BeginAnimation(Image.OpacityProperty, EndAnimation);
                };

                IUser.BeginAnimation(Image.OpacityProperty, StartAnimation);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void SetImage(ImageSource imgSrc)
        {
            IUser.Source = imgSrc;
        }

        private void ResetImage()
        {
            DoubleAnimation StartAnimation = new DoubleAnimation();
            StartAnimation.From = 1;
            StartAnimation.To = 0;
            StartAnimation.Duration = TimeSpan.FromSeconds(0.6);
            StartAnimation.Completed += delegate
            {
                SetImage(new BitmapImage(new Uri("pack://application:,,,/Images/users.jpg")));

                DoubleAnimation EndAnimation = new DoubleAnimation();
                EndAnimation.From = 0;
                EndAnimation.To = 1;
                EndAnimation.Duration = TimeSpan.FromSeconds(1.2);

                IUser.BeginAnimation(Image.OpacityProperty, EndAnimation);
            };

            IUser.BeginAnimation(Image.OpacityProperty, StartAnimation);
        }

        private void InCorrectLogin()
        {
            if (LNameUser.Content.ToString() != "")
            {
               LNameUser.Content = string.Empty;

                ResetImage();
            }

            if (TbLogin.Text.Length > 0)
                SetNotification("Login is incorrect", Brushes.Red);
        }

        private void CorrectCapture()
        {
            Capture.IsEnabled = false;
            IsCapture = true;

            SendNewPassword();
        }

        private void SetLogin(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                MainWindow.mainWindow.UserLogIn.GetUserLogin(TbLogin.Text);
        }

        private void SetLogin(object sender, RoutedEventArgs e) =>
            MainWindow.mainWindow.UserLogIn.GetUserLogin(TbLogin.Text);

        private void SendNewPassword()
        {
            if (IsCapture)
            {
                if (MainWindow.mainWindow.UserLogIn.Password != String.Empty)
                {
                    DoubleAnimation StartAnimation = new DoubleAnimation(); 
                    StartAnimation.From = 1;
                    StartAnimation.To = 0;
                    StartAnimation.Duration = TimeSpan.FromSeconds(0.6);
                    StartAnimation.Completed += delegate
                    {
                        IUser.Source = new BitmapImage(new Uri("pack://application:,,,/Images/gmail.png"));

                        DoubleAnimation EndAnimation = new DoubleAnimation();
                        EndAnimation.From = 0;
                        EndAnimation.To = 1;
                        EndAnimation.Duration = TimeSpan.FromSeconds(1.2);

                        IUser.BeginAnimation(Image.OpacityProperty, EndAnimation);
                    };

                    IUser.BeginAnimation(Image.OpacityProperty, StartAnimation);

                    SetNotification("An email has been sent to your email.", Brushes.Black);

                    MainWindow.mainWindow.UserLogIn.CrateNewPassword();
                }
            }
        }

        public void SetNotification(string Message, SolidColorBrush _Color)
        {
            LNameUser.Content = Message;
            LNameUser.Foreground = _Color;
        }

        private void OpenLogin(object sender, MouseButtonEventArgs e)
        {
            MainWindow.mainWindow.OpenPage(new Login());
        }
    }
}
