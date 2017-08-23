﻿using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMWebClient.Core.Player;

namespace MIMWebClient.Core
{
    using System.Security.Cryptography.X509Certificates;

    using MIMWebClient.Core.Events;
    using MIMWebClient.Core.Player;
    using MIMWebClient.Hubs;
    using Room;

    public static class HubContext
    {
        private static IHubContext _getHubContext;
        /// <summary>
        /// gets the SignalR Hub connection
        /// </summary>
        public static IHubContext getHubContext
        {
            get
            {
                if (_getHubContext == null)
                {
                    _getHubContext = GlobalHost.ConnectionManager.GetHubContext<MIMHub>();
                }

                return _getHubContext;
            }
        }

        /// <summary>
        /// Send a message to connected clients
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="player">The active player</param>
        /// <param name="sendToAll">toggle to send to all or to caller</param>
        /// <param name="excludeCaller">toggle to send to all but exclude caller</param>
        public static void SendToClient(string message, string playerId, string recipientId = null, bool sendToAll = false, bool excludeCaller = false, bool excludeRecipient = false)
        {
            if (message != null)
            {

                //Send a message to all users
                if (sendToAll && excludeCaller == false)
                {
                    HubContext.getHubContext.Clients.All.addNewMessageToPage(message);
                }

                if (playerId != null)
                {
                    //send a message to all but caller
                    // x hits you - Fight message is being sent to all instead of person who's being hit
                    if (excludeCaller && sendToAll == false && excludeRecipient == false)
                    {
                        HubContext.getHubContext.Clients.AllExcept(playerId).addNewMessageToPage(message);
                    }

                    //send a message to all but recipient
                    if (excludeRecipient && recipientId != null)
                    {
                        HubContext.getHubContext.Clients.Client(recipientId).addNewMessageToPage(message);
                    }

                    //send only to caller
                    if (sendToAll == false && excludeCaller == false)
                    {
                        HubContext.getHubContext.Clients.Client(playerId).addNewMessageToPage(message);
                    }


                }
            }
        }

        /// <summary>
        /// Sends a message to all except those specified in the exclude list 
        /// which contains the hubguid of the player
        /// </summary>
        /// <param name="message">message to send to all</param>
        /// <param name="excludeThesePlayers">list of HubGuid's to exclude</param>
        /// <param name="players">players to send message to</param>
        [System.Obsolete("SendToAllExcept is deprecated, please use SendToClient in a forEach instead and exclude the players there.")]
        public static void SendToAllExcept(string message, List<string> excludeThesePlayers, List<PlayerSetup.Player> players)
        {
            if (message == null)
            {
                return;
            }

            foreach (var player in players)
            {

                if (player != null && player.HubGuid != excludeThesePlayers?.FirstOrDefault(x => x.Equals(player.HubGuid)))
                {
                    HubContext.getHubContext.Clients.Client(player.HubGuid).addNewMessageToPage(message);
                }

            }

        }
        [System.Obsolete("broadcastToRoom is deprecated, please use SendToClient in a forEach instead and exclude the player there.")]
        public static void broadcastToRoom(string message, List<PlayerSetup.Player> players, string playerId, bool excludeCaller = false)
        {
            int playerCount = players.Count;

            if (excludeCaller)
            {
                for (int i = 0; i < playerCount; i++)
                {
                    if (playerId != players[i].HubGuid)
                    {
                        HubContext.getHubContext.Clients.Client(players[i].HubGuid).addNewMessageToPage(message);
                    }
                }
            }
            else
            {
                for (int i = 0; i < playerCount; i++)
                {
                    HubContext.getHubContext.Clients.Client(players[i].HubGuid).addNewMessageToPage(message);
                }
            }


        }

        public static void Quit(string playerId, Room.Room room)
        {


            //remove player from room and player cache

            try
            {

                var Player = Cache.getPlayer(playerId);

                if (Player?.Target != null)
                {
                    SendToClient("You can't quit during combat.", playerId);
                    return;
                }     
              
                if (Player != null)
                {

                     PlayerManager.RemovePlayerFromRoom(room, Player);



                    Save.SavePlayer(Player);

                    PlayerSetup.Player removedChar = null;

                    MIMHub._PlayerCache.TryRemove(Player.HubGuid, out removedChar);

                    SendToClient("Gods take note of your progress", playerId);
                    SendToClient("See you soon!", playerId);
                    broadcastToRoom(Player.Name + " has left the realm", room.players, playerId, true);

                    try
                    {


                        HubContext.getHubContext.Clients.Client(playerId).quit();
                    }
                    catch (Exception ex)
                    {
                        var log = new Error.Error
                        {
                            Date = DateTime.Now,
                            ErrorMessage = ex.InnerException.ToString(),
                            MethodName = "Quit"
                        };

                        Save.LogError(log);
                    }

                }

            }
            catch (Exception ex)
            {
                var log = new Error.Error
                {
                    Date = DateTime.Now,
                    ErrorMessage = ex.InnerException.ToString(),
                    MethodName = "Quit"
                };

                Save.LogError(log);
            }





        }
    }
}
