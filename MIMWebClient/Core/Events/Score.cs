﻿using System.Linq;
using MIMWebClient.Core.Player;

namespace MIMWebClient.Core.Events
{
    using System.Text;
    using System.Web.Helpers;

    using Microsoft.ApplicationInsights.Extensibility.Implementation;

    using MIMWebClient.Core.Room;

    using MongoDB.Bson;

    using PlayerSetup;

    class Score
    {

        public static void ReturnScore(Player playerData)
        {
            string scoreTest = "Score:\r\n Name: " + playerData.Name + " Race: " + playerData.Race;

            var context = HubContext.getHubContext;
            context.Clients.Client(playerData.HubGuid).addNewMessageToPage(scoreTest);
        }

        public static void ReturnScoreUI(Player playerData)
        {

            if (playerData.HubGuid != null)
            {
                var context = HubContext.getHubContext;
                context.Clients.Client(playerData.HubGuid).updateScore(playerData);

            }
        }

        public static void UpdateUiPrompt(Player playerData)
        {

            if (playerData.HubGuid != null)
            {

                var context = HubContext.getHubContext;
                context.Clients.Client(playerData.HubGuid)
                    .updateStat(playerData.HitPoints, playerData.MaxHitPoints, "hp");
                context.Clients.Client(playerData.HubGuid)
                    .updateStat(playerData.ManaPoints, playerData.MaxManaPoints, "mana");
                context.Clients.Client(playerData.HubGuid)
                    .updateStat(playerData.MovePoints, playerData.MaxMovePoints, "endurance");
                context.Clients.Client(playerData.HubGuid)
                    .updateStat(playerData.Experience, playerData.ExperienceToNextLevel, "tnl");

            }
        }

        public static void UpdateUiInventory(Player playerData)
        {

            if (playerData.HubGuid != null)
            {
                var context = HubContext.getHubContext;


                context.Clients.Client(playerData.HubGuid)
                    .updateInventory(playerData.Inventory.Where(x => x.location == Item.Item.ItemLocation.Inventory && x.type != Item.Item.ItemType.Gold));

            }

        }

        public static void UpdateUiEquipment(Player playerData)
        {

            if (playerData.HubGuid != null)
            {
                var context = HubContext.getHubContext;


                context.Clients.Client(playerData.HubGuid)
                    .updateEquipment( Equipment.DisplayEq(playerData, playerData.Equipment));

            }
             
        }

        public static void UpdateUiAffects(Player playerData)
        {

            if (playerData.HubGuid != null)
            {
                var context = HubContext.getHubContext;


                if (playerData.Affects != null)
                {

                    if (playerData.Affects.Count > 0)
                    {
                        var aff = new StringBuilder();

                        aff.Append("<li><p>You are affected by the following affects:</p></li>");
                       

                        foreach (var affect in playerData.Affects)
                        {
                            aff.Append("<li>" + affect.Name + " (" + affect.Duration + ") ticks</li> ");
                            
                        }

                        context.Clients.Client(playerData.HubGuid)
                            .updateAffects(aff);
                    }
                    else
                    {
                        context.Clients.Client(playerData.HubGuid)
                            .updateAffects("You are not affected by anything.");
                    }
                }
                else
                {
                    context.Clients.Client(playerData.HubGuid)
                        .updateAffects("You are not affected by anything.");
                }

              

            }

        }

        public static void UpdateUIChannels(Player playerData, string text, string className)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }
            var context = HubContext.getHubContext;
            context.Clients.Client(playerData.HubGuid).UpdateUiChannels(text, className);

        }

        public static void UpdateUiRoom(Player playerData, string room)
        {

            if (playerData.HubGuid != null)
            {
                //var room = new Room.Room();
                //var currentRoom = p
                var context = HubContext.getHubContext;
                context.Clients.Client(playerData.HubGuid).UpdateUiRoom(room);
                //context.Clients.Client(playerData.HubGuid).updateRoom(playerData.Inventory);
            }
        }

        public static void UpdateUiMap(Player playerData, int roomId, string area, string region, int zindex)
        {

            if (playerData.HubGuid != null)
            {
                //var room = new Room.Room();
                //var currentRoom = p
                var context = HubContext.getHubContext;
                context.Clients.Client(playerData.HubGuid).UpdateUiMap(roomId, area, region, zindex);
                //context.Clients.Client(playerData.HubGuid).updateRoom(playerData.Inventory);
            }
        }
    }
}
