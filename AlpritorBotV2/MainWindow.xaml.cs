using AlpritorBotV2.CryptModule;
using AlpritorBotV2.ListenerModule;
using AlpritorBotV2.TwitchModule;
using AlpritorBotV2.Windows.Culture;
using AlpritorBotV2.Windows.InputBox;
using System.Globalization;
using System.Printing;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace AlpritorBotV2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //var ts = CryptAES.Encrypt("");
            var pairCultureWindow = new ChooseCulture().ShowDialog();

            if (pairCultureWindow!.First != null)
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(pairCultureWindow!.First.CultureName);
            }

            if (string.IsNullOrWhiteSpace(pairCultureWindow!.Second))
            {
                BotBase.Initialize(pairCultureWindow!.First?.CultureName);
            }
            else
            {
                BotBase.Initialize(pairCultureWindow!.First?.CultureName);
                BotBase.JoinChannel(pairCultureWindow!.Second);
            }

            InitializeComponent();
        }

        private async void ButtonGetToken_Click(object sender, RoutedEventArgs e)
        {
            ButtonGetToken.IsEnabled = false;
            try
            {
                BotBase.SetUserAccessToken(await TokenListener.GetAccessToken());
                if (BotBase.IsUserAccessTokenSet)
                {
                    ButtonGetToken.Foreground = Brushes.Green;
                }
                else
                {
                    ButtonGetToken.Foreground = Brushes.Red;
                    ButtonGetToken.IsEnabled = true;
                }
            }
            catch (System.Exception ex)
            {

                ButtonGetToken.IsEnabled = true;
                MessageBox.Show(ex.Message, "ERROR");//locale AND BUTTON TEXT TOO
            }
        }

        private void ButtonUptime_Click(object sender, RoutedEventArgs e)
        {
            if (BotBase.isJoined)
            {
                BotBase.Uptime(null);
            }
        }

        private void ButtonSendMsg_Click(object sender, RoutedEventArgs e)
        {
            if (BotBase.isJoined)
            {
                BotBase.SendMsg(new InputBoxWindow("Enter text:").ShowDialog());//locale
            }
        }

        private void ButtonChangeChannel_Click(object sender, RoutedEventArgs e)
        {
            BotBase.JoinChannel(new InputBoxWindow("Channel name:").ShowDialog());//locale
        }

        private void ButtonChangeLanguage_Click(object sender, RoutedEventArgs e)
        {
            BotBase.ChangeCulture(new InputBoxWindow("New culture:").ShowDialog());//locale
        }
    }
}
