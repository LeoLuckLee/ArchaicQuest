﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MIMWebClient.Core.World.Crafting;

namespace MIMWebClient.Core.Events
{
    public class CraftMaterials
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }


    public class Craft
    {
        public string Name { get; set; }

        /// <summary>
        /// required Materials
        /// </summary>
        public List<CraftMaterials> Materials { get; set; }

        public List<string> CraftingEmotes { get; set; }

        public string StartMessage { get; set; }

        public string Description { get; set; }

        public string SuccessMessage { get; set; }

        public string FaliureMessage { get; set; }

        public Item.Item CreatesItem { get; set; }

        /// <summary>
        /// otherwise player inv
        /// </summary>
        public bool CraftAppearsInRoom { get; set; }

        /// <summary>
        /// duration in ms
        /// </summary>
        public int Duration { get; set; }


        public static void CraftItem(PlayerSetup.Player player, Room.Room room, string craftItem)
        {
            CanCraft(player, room, craftItem);
        }

        public static void CraftList(PlayerSetup.Player player)
        {
            if (player.CraftingRecipes == null || player.CraftingRecipes.Count == 0)
            {
                HubContext.Instance.SendToClient("You don't know how to craft anything.", player.HubGuid);
                return;
            }

            HubContext.Instance.SendToClient("<p>You can craft:</p>", player.HubGuid);

            foreach (var craft in player.CraftingRecipes)
            {
                var canCraft = Crafting.CraftList().FirstOrDefault(x => x.Name.Equals(craft));
                var required = string.Empty;

                if (canCraft != null)
                {
                    foreach (var materials in canCraft.Materials)
                    {
                        required += materials + " ";
                    }        

                    HubContext.Instance.SendToClient("<p>" + canCraft.Name + ", Required: " + required+"</p>", player.HubGuid);
                }
            }
        }

        public static Boolean HasAllMaterials(PlayerSetup.Player player, List<CraftMaterials> materialList)
        {
            foreach (var material in materialList)
            {
                if (player.Inventory.Count(x => x.name.Equals(material.Name)) != material.Count)
                {
                    HubContext.Instance.SendToClient("You don't have all the materials required to craft " + Helpers.ReturnName(null, null, material.Name) + ".", player.HubGuid);
                    return false;
                }
            }

            return true;
        }


        public static void CanCraft(PlayerSetup.Player player, Room.Room room, string craftItem)
        {
            if (string.IsNullOrEmpty(craftItem))
            {
                HubContext.Instance.SendToClient("What do you want to craft?", player.HubGuid);
                return;
            }

            var findCraft = Crafting.CraftList().FirstOrDefault(x => x.Name.ToLower().Contains(craftItem.ToLower()));
 
            var hasCraft = player.CraftingRecipes.FirstOrDefault(x => x.ToLower().Contains(craftItem.ToLower()));

            if (hasCraft == null)
            {
                HubContext.Instance.SendToClient("You don't know how to craft that.", player.HubGuid);
                return;
            }

            var hasMaterials = findCraft != null && HasAllMaterials(player, findCraft.Materials);

            if (!hasMaterials)
            { 
                return;
            }

          CraftItem(player, room, findCraft);
        }

        public static void CraftItem(PlayerSetup.Player player, Room.Room room, Craft craftItem)
        {
            HubContext.Instance.SendToClient(craftItem.StartMessage, player.HubGuid);

            Task.Delay(1500);

            foreach (var emote in craftItem.CraftingEmotes)
            {
                HubContext.Instance.SendToClient(emote, player.HubGuid);


                Task.Delay(1000);
            }

            HubContext.Instance.SendToClient(craftItem.SuccessMessage, player.HubGuid);


            foreach (var materials in craftItem.Materials)
            {

                for (int i = 0; i < materials.Count; i++)
                {
                    var item = player.Inventory.FirstOrDefault(x => x.name.Equals(materials.Name));

                    player.Inventory.Remove(item);
                }
            }

            if (craftItem.CraftAppearsInRoom)
            {
                room.items.Add(craftItem.CreatesItem);
            }

        }
    }
}