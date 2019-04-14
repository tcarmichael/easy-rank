using System;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace EasyRank__
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MessageSender keySender;

        private Timer timer;

        public MainWindow()
        {
            InitializeComponent();

            // 20 key presses per second
            double keyDelay_s = 1000.0 / 20.0;

            timer = new Timer(keyDelay_s);
            timer.Elapsed += RepeatKey;
            timer.AutoReset = true;
        }

        private void StartButton_Checked(object sender, RoutedEventArgs e)
        {
            // Retrieve the StarCraft II processes.
            var processes = Process.GetProcessesByName("SC2_x64");

            var hWnd = processes.First().MainWindowHandle;

            // Make sure the user entered a key to press.
            if (selectedKey.Text.Length != 1)
                return;

            char keyCode = selectedKey.Text.Single();

            keySender = new MessageSender(hWnd, keyCode);

            keySender.SendDown(false);

            timer.Start();
        }

        private void StartButton_Unchecked(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            keySender.SendUp();
        }

        private void RepeatKey(object sender, ElapsedEventArgs e)
        {
            keySender.SendDown(true);
        }
    }
}
