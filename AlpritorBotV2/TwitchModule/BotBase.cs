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
        private static bool _isModerator;
        private static bool _isJoined;
        private static readonly TwitchAPI _api;
        private static readonly string _botUsername;
        private static readonly string _accessToken;
        private static readonly Dictionary<string, Func<string[], Task>> _commands = new() { { "checkMod", new Func<string[], Task>(async (arg) => CheckMod(arg)) }, { "uptime", new Func<string[], Task>(async (arg) => await Uptime(arg)) }, { "updateInfo", new Func<string[], Task>(async (arg) => await UpdateInfo(arg)) } };

        private static string? _userAccessToken;
        static BotBase()
        {
            _botUsername = ConfigurationManager.AppSettings["botUsername"]!;
            _accessToken = CryptAES.Decrypt(ConfigurationManager.AppSettings["accessToken"]!);
            _api = new TwitchAPI();
            _api.Settings.ClientId = ConfigurationManager.AppSettings["clientId"]!;
            _api.Settings.AccessToken = _accessToken;
        }

        public static void Initialize(string channelName)
        {
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

            Client.Initialize(credentials, channelName);

            if (!Client.IsConnected)
            {
                Client.Connect();

            }

            Client.JoinChannel(channelName);


            //незапуск если бот не подключился к каналу = сделать ожидание на прок эвента
        }

        private async static void Client_OnChatCommandReceived(object? sender, OnChatCommandReceivedArgs e)
        {
            try
            {
                if (_commands.ContainsKey(e.Command.CommandText))
                {
                    await _commands.GetValueOrDefault(e.Command.CommandText)!(e.Command.ArgumentsAsList.ToArray());
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
                if (e.SentMessage.DisplayName.ToLower().Contains("alpritor_bot"))
                {
                    if (e.SentMessage.IsModerator)
                    {
                        _isModerator = true;
                    }
                    else
                    {
                        Client!.SendMessage(Channel, ResourcesLocal.GiveMeModeMsg);
                    }
                }
            }
            else
            {
                SimpleLogger.WriteToLog($"Sent message: from - \"{e.SentMessage.DisplayName}\", user role - \"{e.SentMessage.UserType}\"", "BotBase");
            }
        }

        private static void Client_OnMessageReceived(object? sender, OnMessageReceivedArgs e)
        {
            //MessageBox.Show(e.ChatMessage.Message);
        }

        private static void Client_OnJoinedChannel(object? sender, OnJoinedChannelArgs e)
        {
            _isJoined = true;
            Channel = new JoinedChannel(e.Channel);
            Client!.SendMessage(e.Channel, ResourcesLocal.HelloMsg);

            Client.GetChannelModerators(e.Channel);
        }

        public static void SetUserAccessToken(string token)
        {
            _userAccessToken = token;
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

        private static void CheckMod(string[] args)
        {
            if (!_isModerator)
            {
                Client!.SendMessage(Channel, ".mods");
            }
        }

        private async static Task Uptime(string[] args)
        {
            var stream = await GetCurrentStream();
            if (stream != null)
            {
                var tmpTime = (DateTime.UtcNow - stream.StartedAt).ToString("hh\\:mm\\:ss");
                Client!.SendMessage(Channel, $"{ResourcesLocal.Uptime}: {tmpTime}");
            }
            else
            {
                Client!.SendMessage(Channel, $"{ResourcesLocal.UptimeFail}");
            }
        }

        private async static Task UpdateInfo(string[] args)
        {
            if (_userAccessToken == null)
            {
                Client!.SendMessage(Channel, $"Access token didnt assigned to bot :(");//CHANGE TO LOCALES
                return;
            }
            string gameId = string.Empty;
            string gameName = string.Empty;
            if (args.Length > 1)
            {
                gameName = args[1].Replace('_', ' ');
                GetGamesResponse games = await _api.Helix.Games.GetGamesAsync(gameNames: new List<string>() { gameName });
                if (games.Games.Count() > 0)
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
    }



}
