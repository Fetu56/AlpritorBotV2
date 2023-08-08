using AlpritorBotV2.TwitchModule;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace AlpritorBotV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            Windows.Culture.ChooseCulture culture = new();
            culture.ShowDialog();

            if(culture.SelectedCulture!=null && !string.IsNullOrEmpty(culture.ChannelName))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(culture.SelectedCulture.CultureName);
                //MessageBox.Show(Localization.Bot.Resources.CurrentLocale);

                BotBase.Initialize(culture.ChannelName);

                //MessageBox.Show(Localization.Bot.Resources.GiveMeModeMsg);
                //MessageBox.Show(Localization.Bot.Resources.HelloMsg);
            }





            InitializeComponent();
        }
    }
}
