using AlpritorBotV2.CryptModule;
using AlpritorBotV2.Resources.Localization.Bot;
using AlpritorBotV2.LogModule;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Channels.ModifyChannelInformation;
using TwitchLib.Api.Helix.Models.Games;
using TwitchLib.Api.Helix.Models.Streams.GetStreams;
using TwitchLib.Api.Helix.Models.Users.GetUsers;
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
        public static TwitchClient? Client { get; private set; }
        public static JoinedChannel? Channel { get; set; }
        public static bool isJoined;
        private static bool _isModerator;
        private static readonly TwitchAPI _api;
        private static readonly string _botUsername;
        private static readonly string _accessToken;
        private static readonly Dictionary<string, Func<OnChatCommandReceivedArgs, Task>> _commands = new() { { "checkMod", new(async (arg) => CheckMod(arg)) }, { "uptime", new (async (arg) => await Uptime(arg)) }, { "updateInfo", new(async (arg) => await UpdateInfo(arg)) } };

        public static CultureInfo? CurrentCulture { get; private set; }
        private static string? _userAccessToken;
        public static bool IsUserAccessTokenSet { get { return !string.IsNullOrWhiteSpace(_userAccessToken); }}
        static BotBase()
        {
            _botUsername = ConfigurationManager.AppSettings["botUsername"]!;
            _accessToken = CryptAES.Decrypt(ConfigurationManager.AppSettings["accessToken"]!);
            _api = new TwitchAPI();
            _api.Settings.ClientId = ConfigurationManager.AppSettings["clientId"]!;
            _api.Settings.AccessToken = _accessToken;
        }

        public static void Initialize(string? culture)
        {
            if(culture == null)
            {
                CurrentCulture = new CultureInfo("en-GB");
            }
            else
            {
                CurrentCulture = new CultureInfo(culture);
            }

            ConnectionCredentials credentials = new(_botUsername, _accessToken);
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };

            WebSocketClient customClient = new(clientOptions);
            Client = new TwitchClient(customClient);

            Client.OnJoinedChannel += Client_OnJoinedChannel;
            Client.OnMessageReceived += Client_OnMessageReceived;
            Client.OnMessageSent += Client_OnMessageSent;
            Client.OnChatCommandReceived += Client_OnChatCommandReceived;

            Client.Initialize(credentials);

            Client.Connect();
            //незапуск если бот не подключился к каналу = сделать ожидание на прок эвента
        }

        private async static void Client_OnChatCommandReceived(object? sender, OnChatCommandReceivedArgs e)
        {
            try
            {
                if (_commands.ContainsKey(e.Command.CommandText))
                {
                    await _commands.GetValueOrDefault(e.Command.CommandText)!(e);
                }
                else
                {
                    Client!.SendReply(Channel, e.Command.ChatMessage.Id, "Unknown command");//CHANGE TO LOCALE
                }
            }
            catch (Exception ex)
            {
                SimpleLogger.WriteToLog($"Error: {ex.Message}", "BotBase Commands");
            }

        }

        private static void Client_OnMessageSent(object? sender, OnMessageSentArgs e)
        {
            if (e.SentMessage.Message.Contains(".mods"))
            {
                //mod give pls msg
                //if (e.SentMessage.DisplayName.ToLower().Contains("alpritor_bot"))
                //{
                //    if (e.SentMessage.IsModerator)
                //    {
                //        _isModerator = true;
                //    }
                //    else
                //    {
                //        Client!.SendMessage(Channel, ResourcesLocal.ResourceManager.GetString("GiveMeModeMsg", CurrentCulture));
                //    }
                //}
            }
            else
            {
                SimpleLogger.WriteToLog($"Sent message: from - \"{e.SentMessage.DisplayName}\", user role - \"{e.SentMessage.UserType}\"", "BotBase");
            }
        }

        private static void Client_OnMessageReceived(object? sender, OnMessageReceivedArgs e)
        {
            //MessageBox.Show(e.ChatMessage.Message);
            ResourcesLocal.ResourceManager.GetString("", CurrentCulture);
        }

        private static void Client_OnJoinedChannel(object? sender, OnJoinedChannelArgs e)
        {
            isJoined = true;
            Channel = new JoinedChannel(e.Channel);
            Client!.SendMessage(e.Channel, ResourcesLocal.ResourceManager.GetString("HelloMsg", CurrentCulture));

            Client.GetChannelModerators(e.Channel);
        }

        public static void SetUserAccessToken(string token)
        {
            _userAccessToken = token;
        }

        public static void JoinChannel(string newChannel)
        {
            if(string.IsNullOrWhiteSpace(newChannel))
            {
                return;
            }
            if(Channel != null)
            {
                Client!.LeaveChannel(Channel);
                Channel = null;
                isJoined = false;
            }
            Client!.JoinChannel(newChannel);
        }

        private async static Task<Stream> GetCurrentStream()
        {
            Stream? stream = null;
            var streams = await _api.Helix.Streams.GetStreamsAsync(userLogins: new List<string> { Channel!.Channel });
            if (streams.Streams.Length > 0)
            {
                stream = streams.Streams.First();
            }
            return stream!;
        }

        private async static Task<User> GetCurrentUser()
        {
            User? user = null;
            var users = await _api.Helix.Users.GetUsersAsync(logins: new List<string> { Channel!.Channel });
            if (users.Users.Length > 0)
            {
                user = users.Users.First();
            }
            return user!;
        }

        public static void CheckMod(string[] args)
        {
            if (!_isModerator)
            {
                Client!.SendMessage(Channel, ".mods");
            }
        }
        public static void ChangeBotLocale(string[] args)
        {
            if (!_isModerator)
            {
                Client!.SendMessage(Channel, ".mods");
            }
        }

        public async static Task Uptime(OnChatCommandReceivedArgs e)
        {
            var stream = await GetCurrentStream();
            if (stream != null)
            {
                var tmpTime = (DateTime.UtcNow - stream.StartedAt).ToString("hh\\:mm\\:ss");
                Client!.SendMessage(Channel, $"{ResourcesLocal.ResourceManager.GetString("Uptime", CurrentCulture)}: {tmpTime}");
            }
            else
            {
                Client!.SendMessage(Channel, $"{ResourcesLocal.ResourceManager.GetString("UptimeFail", CurrentCulture)}");
            }
        }

        public async static Task UpdateInfo(OnChatCommandReceivedArgs e)
        {
            if (_userAccessToken == null)
            {
                Client!.SendMessage(Channel, $"Access token didnt assigned to bot :(");//CHANGE TO LOCALES
                return;
            }
            string[] args = e.Command.ArgumentsAsList.ToArray();
            string gameId = string.Empty;
            string gameName = string.Empty;
            if (args.Length > 1)
            {
                gameName = args[1].Replace('_', ' ');
                GetGamesResponse games = await _api.Helix.Games.GetGamesAsync(gameNames: new List<string>() { gameName });
                if (games.Games.Length > 0)
                {
                    gameId = games.Games.First().Id;
                }
            }

            try
            {
                string newTitle = args[0].Replace('_', ' ');
                ModifyChannelInformationRequest request = new() { GameId = gameId, Title = newTitle };
                _api.Helix.Channels.ModifyChannelInformationAsync(GetCurrentUser().Result.Id.ToString(), request, _userAccessToken).Wait();
                Client!.SendMessage(Channel, $"New stream name: {newTitle}" + (gameId == string.Empty? "" : $", New game: {gameName}"));//CHANGE TO LOCALES
            }
            catch (TwitchLib.Api.Core.Exceptions.BadScopeException)
            {
                Client!.SendMessage(Channel, $"Error with access token :(");//CHANGE TO LOCALES
                throw;
            }


        }
        public static void SendMsg(string message)
        {
            Client!.SendMessage(Channel, message);
        }

        public static void ChangeCulture(string culture)
        {
            if(string.IsNullOrWhiteSpace(culture))
            {
                return;
            }

            try
            {
                CurrentCulture = new CultureInfo(culture);
            }
            catch (Exception) { }
        }

        public static void BanUser(OnChatCommandReceivedArgs e)
        {
            if(e.Command.ChatMessage.IsModerator || e.Command.ChatMessage.IsBroadcaster)
            {
                string[] args = e.Command.ArgumentsAsList.ToArray();
                Client.BanUser(Channel, args[0], args[1] == null ? $"Banned from bot by {e.Command.ChatMessage.Username}." : args[1]);//locale
            }
           
        }
    }



}
