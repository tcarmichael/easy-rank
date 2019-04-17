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
            // Make sure the user entered a key to press.
            if (selectedKey.Text.Length != 1)
                return;

            // Retrieve the StarCraft II processes.
            var process32Bit = Process.GetProcessesByName("SC2").FirstOrDefault();
            var process64Bit = Process.GetProcessesByName("SC2_x64").FirstOrDefault();

            // Prefer the 64-bit process, falling back to the 32-bit process if not found.
            var hWnd = process64Bit?.MainWindowHandle
                ?? process32Bit?.MainWindowHandle
                ?? IntPtr.Zero;

            // Could not find StarCraft process.
            if (hWnd == IntPtr.Zero)
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
