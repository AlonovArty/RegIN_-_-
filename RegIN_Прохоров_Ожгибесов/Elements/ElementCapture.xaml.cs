using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace RegIN_Прохоров_Ожгибесов.Elements
{
    /// <summary>
    /// Логика взаимодействия для ElementCapture.xaml
    /// </summary>
    public partial class ElementCapture : UserControl
    {
        public CorrectCapture HandlerCorrectCapture;
        public delegate void CorrectCapture();

        string StrCapture;
        int ElementWidth = 280;
        int ElementHeight = 50;

        private static readonly Random rnd = new Random();  

        public ElementCapture()
        {
            InitializeComponent();

            InputCapture.MaxLength = 4;

            GenerateCaptcha();
        }

        public void GenerateCaptcha()
        {
            InputCapture.Text = "";
            Capture.Children.Clear();
            StrCapture = "";

            for (int i = 0; i < 80; i++)
            {
                Label Lbackground = new Label()
                {
                   Content = rnd.Next(0, 10),
                   FontSize = rnd.Next(10, 16),
                   FontWeight = FontWeights.Bold,
                   Foreground = new SolidColorBrush(Color.FromArgb(100, (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255))),
                   Margin = new Thickness(rnd.Next(0,ElementWidth-20), rnd.Next(0, ElementHeight - 20), 0,0)
                };
                Capture.Children.Add(Lbackground);
            }

            for (int i = 0; i < 4; i++)
            {
                char c = (char)('0' + rnd.Next(0, 10));
                StrCapture += c;

                Label lCode = new Label()
                {
                    Content = c,
                    FontSize = 30,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(Color.FromArgb(255, (byte)rnd.Next(0, 255),
                    (byte)rnd.Next(0, 255), (byte)rnd.Next(0, 255))),
                    Margin = new Thickness(ElementWidth / 2 - 60 + i * 30, rnd.Next(-10, 10), 0, 0)
                };

                Capture.Children.Add(lCode);
            }

            InputCapture.Focus();
        }

        public bool OnCapture()
        {
            return StrCapture == InputCapture.Text;
        }

        private void EnterCapture(object sender, KeyEventArgs e)
        {
            if (InputCapture.Text.Length == 4)
                if (!OnCapture())
                    GenerateCaptcha();
                else if (HandlerCorrectCapture != null)
                    HandlerCorrectCapture.Invoke();
        }
    }
}
