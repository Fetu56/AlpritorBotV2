using AlpritorBotV2.CryptModule;
using AlpritorBotV2.Localization;
using AlpritorBotV2.Localization.Bot;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Interfaces;
using TwitchLib.Communication.Models;

namespace AlpritorBotV2.TwitchModule
{
    public static class BotBase
    {
        private static TwitchClient? Client { get; set; }
        private static readonly string BotUsername;
        private static readonly string AccessToken;

        private static JoinedChannel? Channel { get; set; }

        private static bool isModerator;
        private static bool isJoined;

        static BotBase()
        {
            BotUsername = ConfigurationManager.AppSettings["botUsername"]!;
            AccessToken = ConfigurationManager.AppSettings["accessToken"]!;
        }
        public static void Initialize(string channelName)
        {
            ConnectionCredentials credentials = new ConnectionCredentials(BotUsername, CryptAES.Decrypt(AccessToken));
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            Client = new TwitchClient(customClient);

            Client.OnJoinedChannel += Client_OnJoinedChannel;
            Client.OnMessageReceived += Client_OnMessageReceived;
            Client.OnMessageSent += Client_OnMessageSent;

            Client.Initialize(credentials, channelName);
            Client.Connect();
            Client.JoinChannel(channelName);

            //Отдельная команда !checkMod, незапуск если бот не подключился к каналу = сделать ожидание на прок эвента
        }

        private static void Client_OnMessageSent(object? sender, OnMessageSentArgs e)
        {
            if(e.SentMessage.DisplayName.ToLower().Contains("alpritor_bot"))
            {
                if(e.SentMessage.IsModerator)
                {
                    isModerator = true;
                }
                else
                {
                    Client!.SendMessage(Channel, Resources.GiveMeModeMsg);
                }
            }
        }

        private static void Client_OnMessageReceived(object? sender, OnMessageReceivedArgs e)
        {
            MessageBox.Show(e.ChatMessage.Message);
        }

        private static void Client_OnJoinedChannel(object? sender, OnJoinedChannelArgs e)
        {
            isJoined = true;
            Channel = new JoinedChannel(e.Channel);
            Client!.SendMessage(e.Channel, Resources.HelloMsg);

            Client.GetChannelModerators(e.Channel);
        }

    }



}
