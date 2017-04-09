﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMWebClient.Core.Player;

namespace MIMWebClient.Core.Events
{
    using System.Collections.Concurrent;

    using MIMWebClient.Core.PlayerSetup;
    using MIMWebClient.Core.Room;

    using MongoDB.Bson;
    using MongoDB.Driver;

    public class LoadRoom
    {
        public string Region { get; set; }
        public string Area { get; set; }
        public int id { get; set; }


        public Room LoadRoomFile()
        {
            const string ConnectionString = "mongodb://testuser:password@ds052968.mlab.com:52968/mimdb";

            // Create a MongoClient object by using the connection string
            var client = new MongoClient(ConnectionString);

            //Use the MongoClient to access the server
            var database = client.GetDatabase("mimdb");

            var collection = database.GetCollection<Room>("Room");



            Room room = collection.Find(x => x.areaId == this.id && x.area == Area && x.region == Region).FirstOrDefault();


            if (room != null)
            {
                return room;
            }

            throw new Exception("No Room found in the Database for areaId: " + id);
        }


        /// <summary>
        /// Displays room desc, players, mobs, items and exits
        /// </summary>
        /// <param name="room"></param>
        /// <param name="playerName"></param>
        /// <returns></returns>
        public static string DisplayRoom(Room room, string playerName)
        {

            string roomTitle = room.title;
            string roomDescription = room.description;

            var exitList = string.Empty;
            foreach (var exit in room.exits)
            {
                exitList += exit.name + " ";
            }

            var player = room.players.FirstOrDefault(x => x.Name.Equals(playerName));


            var itemList = string.Empty;
            foreach (var item in room.items)
            {
                if (item != null)
                {

                    if (item.itemFlags?.Contains(Item.Item.ItemFlags.invis) == true && player.DetectInvis == false || item.itemFlags?.Contains(Item.Item.ItemFlags.hidden) == true && player.DetectHidden == false)
                    {
                        continue;
                    }

                    if (!item.isHiddenInRoom)
                    {
                        var result = AvsAnLib.AvsAn.Query(item.name);
                        string article = result.Article;

                        if (item.description?.room != null)
                        {
                            itemList += $"<p class='roomItems'>{item.description.room}<p>";
                        }
                        else
                        {
                            itemList += $"<p class='roomItems'>{ article} {item.name} is on the floor here.<p>";
                        }

                        
                    }
                }
            }

           
            var playerList = string.Empty;
            if (room.players != null)
            {
               

                foreach (var item in room.players)
                {
                    if (item.invis == true && player.DetectInvis == false || item.hidden == true && player.DetectHidden == false)
                    {
                        continue;
                    }

                    if (item.nonDectect == true)
                    {
                        continue;
                    }

                    if (item.Name != playerName)
                    {
                        if (item.Status == Player.PlayerStatus.Standing)
                        {
                            playerList += item.Name + " is here\r\n";
                        }
                        else if (item.Status == Player.PlayerStatus.Fighting)
                        {
                            playerList += item.Name + " is fighting " + item.Target.Name + "\r\n";
                        }
                        else if (item.Status == PlayerSetup.Player.PlayerStatus.Resting)
                        {
                            playerList += item.Name + " is resting.";
                        }
                        else if (item.Status == PlayerSetup.Player.PlayerStatus.Sleeping)
                        {
                            playerList += item.Name + " is sleeping.";
                        }
   
                    }

                }
            }

            var mobList = string.Empty;
            if (room.mobs != null)
            {
                foreach (var item in room.mobs)
                {

                    if (item.invis == true && player.DetectInvis == false || item.hidden == true && player.DetectHidden == false)
                    {
                        continue;
                    }

                    if (item.nonDectect == true)
                    {
                        continue;
                    }


                    var result = AvsAnLib.AvsAn.Query(item.Name);
                    string article = result.Article;

                    if (item.KnownByName)
                    {
                        article = string.Empty;
                    }

                    if (item.Status == Player.PlayerStatus.Standing)
                    {
                        mobList += "<p class='roomItems'>" + article + " " + item.Name + " is here.<p>";
                    }
                    else if (item.Status == Player.PlayerStatus.Fighting)
                    {
                        mobList += "<p class='roomItems'>" + article + " " + item.Name + " is fighting " + item.Target.Name + "</p>";
                    }
                    else if (item.Status == PlayerSetup.Player.PlayerStatus.Resting)
                    {
                        mobList += "<p class='roomItems'>" + article + " " + item.Name + " is resting.<p>";
                    }
                    else if (item.Status == PlayerSetup.Player.PlayerStatus.Sleeping)
                    {
                        mobList += "<p class='roomItems'>" + article + " " + item.Name + " is sleeping.<p>";
                    }
                }
            }


            string displayRoom = "<p class='roomTitle'>" + roomTitle + "<p><p class='roomDescription'>" + roomDescription + "</p> <p class='RoomExits'>[ Exits: " + exitList.ToLower() + " ]</p>" + itemList + "\r\n" + playerList + "\r\n" + mobList;

            //  Score.UpdateUiRoom(room.players.FirstOrDefault(x => x.Name.Equals(playerName)), displayRoom);
            return displayRoom;

        }

        public static void ReturnRoom(Player player, Room room, string commandOptions = "", string keyword = "")
        {

            Room roomData = room;

            if (string.IsNullOrEmpty(commandOptions) && keyword == "look")
            {
                var roomInfo = DisplayRoom(roomData, player.Name);
                HubContext.SendToClient(roomInfo, player.HubGuid);

            }
            else
            {

                int n = -1;
                string item = string.Empty;

                if (commandOptions.IndexOf('.') != -1)
                {
                    n = Convert.ToInt32(commandOptions.Substring(0, commandOptions.IndexOf('.')));
                    item = commandOptions.Substring(commandOptions.LastIndexOf('.') + 1);
                }

                if (roomData.keywords == null)
                {
                    roomData.keywords = new List<RoomObject>();
                }

                var roomDescription = roomData.keywords.Find(x => x.name.ToLower().Contains(commandOptions));

                if (roomData.items == null)
                {
                    roomData.items = new List<Item.Item>();
                }

                var itemDescription = (n == -1)
                                          ? roomData.items.Find(x => x.name.ToLower().Contains(commandOptions))
                                          : roomData.items.FindAll(x => x.name.ToLower().Contains(item))
                                                .Skip(n - 1)
                                                .FirstOrDefault();

                if (roomData.mobs == null)
                {
                    roomData.mobs = new List<Player>();
                }

                var mobDescription = roomData.mobs.Find(x => x.Name.ToLower().Contains(commandOptions.ToLower()));

                if (roomData.players == null)
                {
                    roomData.players = new List<Player>();
                }

                var playerDescription = roomData.players.Find(x => x.Name.ToLower().Contains(commandOptions.ToLower()));

                var targetPlayerId = string.Empty;
                if (playerDescription != null)
                {
                    var isPlayer = playerDescription.Type.Equals("Player");

                    if (isPlayer)
                    {
                        targetPlayerId = playerDescription.HubGuid;
                    }
                }

                var roomExitDescription = roomData.exits.Find(x => x.name.ToLower().Contains(commandOptions));


                //Returns descriptions for important objects in the room
                if (roomDescription != null && keyword != "look in" && !string.IsNullOrWhiteSpace(commandOptions))
                {
                    string descriptionText = string.Empty;
                    string broadcastAction = string.Empty;
                    if (keyword.Equals("look"))
                    {
                        descriptionText = roomDescription.look;
                        broadcastAction = " looks at a " + roomDescription.name;
                    }
                    else if (keyword.StartsWith("examine"))
                    {
                        descriptionText = roomDescription.examine;
                        broadcastAction = " looks closely at a " + roomDescription.name;
                    }
                    else if (keyword.StartsWith("touch"))
                    {
                        descriptionText = roomDescription.touch;
                        broadcastAction = " touches closely a " + roomDescription.name;
                    }
                    else if (keyword.StartsWith("smell"))
                    {
                        descriptionText = roomDescription.smell;
                        broadcastAction = " sniffs a " + roomDescription.name;
                    }
                    else if (keyword.StartsWith("taste"))
                    {
                        descriptionText = roomDescription.taste;
                        broadcastAction = " licks a " + roomDescription.name;
                    }

                    if (!string.IsNullOrEmpty(descriptionText))
                    {
                        HubContext.SendToClient(descriptionText, player.HubGuid);

                        foreach (var players in room.players)
                        {
                            if (player.Name != players.Name)
                            {
                                HubContext.SendToClient(player.Name + broadcastAction, players.HubGuid);
                            }
                        }
                    }
                    else
                    {
                        HubContext.SendToClient("You can't do that to a " + roomDescription.name, player.HubGuid);
                    }

                }
                else if (itemDescription != null && !string.IsNullOrWhiteSpace(commandOptions))
                {
                    string descriptionText = string.Empty;
                    string broadcastAction = string.Empty;

                    if (keyword.Equals("look in", StringComparison.InvariantCultureIgnoreCase))
                    {

                        if (itemDescription.open == false)
                        {
                            HubContext.SendToClient("You to to open the " + itemDescription.name + " before you can look inside", player.HubGuid);
                            return;
                        }

                        if (itemDescription.container == true)
                        {
                            if (itemDescription.containerItems.Count > 0)
                            {
                                HubContext.SendToClient("You look into the " + itemDescription.name + " and see:", player.HubGuid);

                                foreach (var containerItem in itemDescription.containerItems)
                                {
                                    HubContext.SendToClient(containerItem.name, player.HubGuid);
                                }
                            }
                            else
                            {
                                HubContext.SendToClient("You look into the " + itemDescription.name + " but it is empty", player.HubGuid);
                            }

                            
                            foreach (var character in room.players)
                            {
                                if (player != character)
                                {
                                    
                                    var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} looks in a {itemDescription.name}";

                                    HubContext.SendToClient(roomMessage, character.HubGuid);
                                }
                            }
                        }
                        else
                        {
                            HubContext.SendToClient(itemDescription.name + " is not a container", player.HubGuid);
                        }
                    }
                    else if (keyword.StartsWith("look"))
                    {
                        descriptionText = itemDescription.description.look;
                        broadcastAction = " looks at a " + itemDescription.name;
                    }
                    else if (keyword.StartsWith("examine"))
                    {
                        descriptionText = itemDescription.description.exam;
                        broadcastAction = " looks closely at a " + itemDescription.name;
                    }

                    if (!keyword.Equals("look in", StringComparison.InvariantCultureIgnoreCase))
                    {

                        if (!string.IsNullOrEmpty(descriptionText))
                        {
                            HubContext.SendToClient(descriptionText, player.HubGuid);

                            foreach (var players in room.players)
                            {
                                if (player.Name != players.Name)
                                {
                                    HubContext.SendToClient(player.Name + broadcastAction,
                                        players.HubGuid);
                                }
                            }
                        }
                        else
                        {
                            HubContext.SendToClient("You can't do that to a " + itemDescription.name, player.HubGuid);
                        }
                    }
                }
                else if (mobDescription != null && !string.IsNullOrWhiteSpace(commandOptions))
                {
                    string descriptionText = string.Empty;

                    if (keyword.StartsWith("look") || keyword.StartsWith("examine"))
                    {
                        descriptionText = mobDescription.Description;
                    }


                    if (!string.IsNullOrEmpty(descriptionText))
                    {
                        HubContext.SendToClient(descriptionText, player.HubGuid);
                     
                        Equipment.ShowEquipmentLook(mobDescription, player);
                  

                        foreach (var character in room.players)
                        {
                            if (player != character)
                            {

                                var roomMessage = $"{ Helpers.ReturnName(player, character, string.Empty)} looks at { Helpers.ReturnName(mobDescription, character, string.Empty)}";

                                HubContext.SendToClient(roomMessage, character.HubGuid);
                            }
                        }
                    }
                    else
                    {
                        HubContext.SendToClient("You can't do that to a " + mobDescription.Name, player.HubGuid);
                    }
                }
                else if (playerDescription != null && !string.IsNullOrWhiteSpace(commandOptions))
                {
                    string descriptionText = string.Empty;


                    if (keyword.StartsWith("look") || keyword.StartsWith("examine"))
                    {
                        descriptionText = playerDescription.Description;
                    }


                    if (!string.IsNullOrEmpty(descriptionText))
                    {
                        HubContext.SendToClient(descriptionText, player.HubGuid);
                        Equipment.ShowEquipmentLook(playerDescription, player);
                        HubContext.SendToClient(player.Name + " looks at you", playerDescription.HubGuid);
                    }
                    else
                    {
                        HubContext.SendToClient("You can't do that to a " + playerDescription.Name, player.HubGuid);
                    }
                }
                else if (roomExitDescription != null)
                {
                    HubContext.SendToClient("You look " + roomExitDescription.name, player.HubGuid);

                    var currentAreaId = player.AreaId;
                    player.AreaId = roomExitDescription.areaId;


                    var adjacentRoom = Cache.getRoom(player);
                    if (adjacentRoom == null)
                    {
                        var newRoom = new LoadRoom();

                        newRoom.Area = roomExitDescription.area;
                        newRoom.id = roomExitDescription.areaId;
                        newRoom.Region = roomExitDescription.region;

                         adjacentRoom = newRoom.LoadRoomFile();

                        //add to cache?

                    }

                  var showNextRoom = LoadRoom.DisplayRoom(adjacentRoom, player.Name);



                     HubContext.SendToClient(showNextRoom, player.HubGuid);

                    player.AreaId = currentAreaId;

                    foreach (var players in room.players)
                    {
                        if (player.Name != players.Name)
                        {
                            HubContext.SendToClient(player.Name + " looks " + roomExitDescription.name, players.HubGuid);
                        }
                    }
                }
                else
                {
                    HubContext.SendToClient("You don't see that here.", player.HubGuid);
                }

            }
        }
    }
}
