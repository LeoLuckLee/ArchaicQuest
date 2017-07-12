﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Components.DictionaryAdapter;
using MIMWebClient.Core.Item;
using MIMWebClient.Core.Mob;
using MIMWebClient.Core.Player;
using MIMWebClient.Core.Room;
using MIMWebClient.Core.World.Anker.Mobs.Easy;
using Action = MIMWebClient.Core.Item.Action;

namespace MIMWebClient.Core.World.Tutorial
{
    public class Ambush
    {
        public static Room.Room TutorialRoom1()
        {

    
            var room = new Room.Room
            {
                region = "Tutorial",
                area = "Tutorial",
                areaId = 0,
                title = "Deep in the forest",
                description = "<p>Large trees tower high above all around you and Wilhelm as you tread lightly through the thick woods. No end to the forest is in sight, every way you look is endless amounts of bushes and trees with wide branches reaching out in everything direction blocking the sun.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<PlayerSetup.Player>(),
                terrain = Room.Room.Terrain.Field,
                keywords = new List<RoomObject>(),
                corpses = new List<PlayerSetup.Player>(),
                players = new List<PlayerSetup.Player>(),
                fighting = new List<string>(),
                clean = true,
                EventLook = "tutorial",


            };

        
            room.Emotes.Add("You hear the sound of bushes and trees rustling all around you");
            room.Emotes.Add("*SNAP* You hear the distinctive sound of a stick snapping");
            // Create Exits
            var north = new Exit
            {
                name = "North",
                area = "Tutorial",
                region = "Tutorial",
                areaId = 1,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
                canLock = true,
                canOpen = true,
                open = true,
                doorName = null

            };
           


            //Create Mobs
            var wilhelm = new PlayerSetup.Player
            {
                NPCId = Guid.NewGuid(),
                Name = "Wilhelm",
                KnownByName = true,
                Type = PlayerSetup.Player.PlayerTypes.Mob,
                Description = "Wilhelm has matted black hair, sharp hazel eyes and a cropped beard. He wears chain mail and wields a short sword.",
                Strength = 12,
                Dexterity = 12,
                Constitution = 12,
                Intelligence = 12,
                Wisdom = 12,
                Charisma = 12,
                MaxHitPoints = 150,
                HitPoints = 150,
                Level = 10,
                Status = PlayerSetup.Player.PlayerStatus.Standing,
                Skills = new List<Skill>(),
                Inventory = new List<Item.Item>(),
                DialogueTree = new List<DialogTree>(),
                Greet = false,
                GreetMessage = "I don't think we have much further to go, ",
                Emotes = new List<string>(),
                EventOnComunicate = new Dictionary<string, string>(),
                  EventOnEnter = "tutorial"

            };

            wilhelm.Emotes.Add("looks around nervously");
        
            var dagger = new Item.Item
            {
                actions = new Item.Action(),
                name = "Blunt dagger",
                eqSlot = Item.Item.EqSlot.Wielded,
                weaponType = Item.Item.WeaponType.ShortBlades,
                stats = new Item.Stats { damMin = 2, damMax = 4, minUsageLevel = 1 },
                type = Item.Item.ItemType.Weapon,
                equipable = true,
                attackType = Item.Item.AttackType.Pierce,
                slot = Item.Item.EqSlot.Wielded,
                location = Item.Item.ItemLocation.Inventory,
                description = new Description(),

            };

            dagger.description.look = "This is just a blunt dagger";
            dagger.description.exam = "This blunt dagger is better suited to buttering bread than killing";
          
            wilhelm.Inventory.Add(dagger);
           
            var recall = new Recall
            {
                Area = room.area,
                AreaId = room.areaId,
                Region = room.region
            };

            wilhelm.Recall = recall;

            var didYouHearThat = new DialogTree()
            {
                Id = "tut1",
                Message = "Did you hear that?",
                PossibleResponse = new List<Responses>(),
                
            };

            var tut1a = new Responses()
            {
                QuestionId = "tut1",
                Response = "It sounded like a twig"
            };

            var tut1b = new Responses()
            {
                QuestionId = "tut1",
                Response = "No you're just hearing things"
            };


            didYouHearThat.PossibleResponse.Add(tut1a);
            didYouHearThat.PossibleResponse.Add(tut1b);

          //  wilhelm.DialogueTree.Add(didYouHearThat);Fw


         
            wilhelm.EventOnComunicate.Add("tutorial", "yes");

            var attack = new Responses()
            {
                QuestionId = "tut1",
                MatchPhrase = tut1a.Response,               
                Response = "AAAHG-ATACK!!!!",
                Keyword = new List<string>()
               
            };

            attack.Keyword.Add(tut1a.Response);

            var attackb = new Responses()
            {
                QuestionId = "tut1",
                MatchPhrase = tut1b.Response,
                Response = "AAAHG-ATACK!!!!",
                Keyword = new List<string>()

            };

            attackb.Keyword.Add(tut1b.Response);

         
            room.exits.Add(north);
          
            room.mobs.Add(wilhelm);         

            return room;
        }

        public static Room.Room TutorialLostInTheWoods()
        {

            var room = new Room.Room
            {
                region = "Tutorial",
                area = "Tutorial",
                areaId = 1,
                title = "Deep in the forest",
                description = "<p>Behind you the sounds of swords clashing fill the night air along with never ending goblin screams echoing through the tall thick trees all around you. " +
                              "Between the trees to the north west you see what looks to be a small camp with fire, Maybe you can get some help there.</p> <p class='RoomExits'>[Hint] You move by typing North, East, South, West or n/e/s/w for short. The Exit list will tell you which directions are possible.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<PlayerSetup.Player>(),
                terrain = Room.Room.Terrain.Field,
                keywords = new List<RoomObject>(),
                corpses = new List<PlayerSetup.Player>(),
                players = new List<PlayerSetup.Player>(),
                fighting = new List<string>(),
                clean = true

            };


            var north = new Exit
            {
                name = "North",
                area = "Tutorial",
                region = "Tutorial",
                areaId = 2,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
                canLock = true,
                canOpen = true,
                open = true,
                doorName = null

            };


            room.exits.Add(north);

            return room;
        }

        public static Room.Room TutorialLostInTheWoods2()
        {

            var room = new Room.Room
            {
                region = "Tutorial",
                area = "Tutorial",
                areaId = 2,
                title = "Deep in the forest",
                description = "<p>Shadows flicker off the trees and shrubs caused by the glow of the camp fire to the east. All around you are tall thick tree, and impassable bushes, the fire is your only beceon of hope in this dense woodland.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<PlayerSetup.Player>(),
                terrain = Room.Room.Terrain.Field,
                keywords = new List<RoomObject>(),
                corpses = new List<PlayerSetup.Player>(),
                players = new List<PlayerSetup.Player>(),
                fighting = new List<string>(),
                clean = true

            };


            var east = new Exit
            {
                name = "East",
                area = "Tutorial",
                region = "Tutorial",
                areaId = 3,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
                canLock = true,
                canOpen = true,
                open = true,
                doorName = null

            };

            var south = new Exit
            {
                name = "South",
                area = "Tutorial",
                region = "Tutorial",
                areaId = 1,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
                canLock = true,
                canOpen = true,
                open = true,
                doorName = null

            };



            room.exits.Add(east);
            room.exits.Add(south);

            return room;
        }

        public static Room.Room TutorialGoblinCamp()
        {

            var room = new Room.Room
            {
                region = "Tutorial",
                area = "Tutorial",
                areaId = 3,
                title = "A goblin camp",
                description = "<p>Thick logs used for sitting face each other with a large fire between them at the centre of the camp. " +
                              "A few bed rolls and discarded pieces of meat lay haphazardly across the ground. To the north and south are two small identical tents.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<PlayerSetup.Player>(),
                terrain = Room.Room.Terrain.Field,
                keywords = new List<RoomObject>(),
                corpses = new List<PlayerSetup.Player>(),
                players = new List<PlayerSetup.Player>(),
                fighting = new List<string>(),
                clean = true

            };


            var west = new Exit
            {
                name = "West",
                area = "Tutorial",
                region = "Tutorial",
                areaId = 2,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
                canLock = true,
                canOpen = true,
                open = true,
                doorName = null

            };

            var east = new Exit
            {
                name = "East",
                area = "Tutorial",
                region = "Tutorial",
                areaId = 10,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
                canLock = true,
                canOpen = true,
                open = true,
                doorName = null

            };


            var north = new Exit
            {
                name = "North",
                area = "Tutorial",
                region = "Tutorial",
                areaId = 4,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
                canLock = true,
                canOpen = true,
                open = true,
                doorName = null

            };

            var south = new Exit
            {
                name = "South",
                area = "Tutorial",
                region = "Tutorial",
                areaId = 5,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
                canLock = true,
                canOpen = true,
                open = true,
                doorName = null

            };



            room.mobs.Add(Goblin.WeakGoblin());

            room.Emotes.Add("The fire crackles and spits.");
            room.Emotes.Add("You hear a howl in the distance.");
            room.exits.Add(north);
            room.exits.Add(east);
            room.exits.Add(south);
            room.exits.Add(west);

            return room;
        }

        public static Room.Room TutorialGoblinCampTentNorth()
        {

            var room = new Room.Room
            {
                region = "Tutorial",
                area = "Tutorial",
                areaId = 4,
                title = "A tent in the goblin camp",
                description = "<p>A small bed and wooden chest has been set up against the side of the tent, filling most of the space. A small cluttered desk sits on the other side leaving just about enough room to enter and leave.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<PlayerSetup.Player>(),
                terrain = Room.Room.Terrain.Field,
                keywords = new List<RoomObject>(),
                corpses = new List<PlayerSetup.Player>(),
                players = new List<PlayerSetup.Player>(),
                fighting = new List<string>(),
                clean = true

            };

            var bed = new RoomObject
            {
                name = "Bed",
                look = "The bed is unmade and the sheets are covered in stains.",
                examine = "Under the pillow you see what looks like a pair of leather gloves poking out slightly."
            };

       
            room.keywords.Add(bed);

            var west = new Exit
            {
                name = "South",
                area = "Tutorial",
                region = "Tutorial",
                areaId = 3,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
                canLock = true,
                canOpen = true,
                open = true,
                doorName = null

            };

            var leather = new Item.Item
            {
                armourType = Item.Item.ArmourType.Leather,
                eqSlot = Item.Item.EqSlot.Body,
                description = new Description()
                {
                    look =  "A basic looking leather vest.",
                    exam =
                        "A basic looking leather vest.",
       
                },
                location = Item.Item.ItemLocation.Room,
                slot = Item.Item.EqSlot.Body,
                type = Item.Item.ItemType.Armour,
                name = "A simple leather vest",
                ArmorRating = new ArmourRating()
                {
                    Armour = 2,
                    Magic = 0
                },
                itemFlags = new EditableList<Item.Item.ItemFlags>()
                {
                    Item.Item.ItemFlags.equipable
                },
                Weight = 0.2,
                equipable = true,
                keywords = new List<string>()
                {
                    "leather",
                    "vest",
                    "leather vest"
                }

            };

            var leatherGloves = new Item.Item
            {
                armourType = Item.Item.ArmourType.Leather,
                eqSlot = Item.Item.EqSlot.Hands,
                description = new Description()
                {
                    look = "A basic pair leather gloves.",
                    exam =
                        "A basic pair leather gloves.",

                },
                location = Item.Item.ItemLocation.Room,
                slot = Item.Item.EqSlot.Hands,
                type = Item.Item.ItemType.Armour,
                name = "A basic pair leather gloves",
                ArmorRating = new ArmourRating()
                {
                    Armour = 2,
                    Magic = 0
                },
                itemFlags = new EditableList<Item.Item.ItemFlags>()
                {
                    Item.Item.ItemFlags.equipable
                },
                Weight = 0.2,
                equipable = true,
                keywords = new List<string>()
                {
                    "leather",
                    "gloves",
                    "leather gloves"
                }

            };

            var chest = new Item.Item
            {
                name = "Basic looking wooden chest",
                stuck = true,
                type = Item.Item.ItemType.Container,
                open = false,
                canOpen = true,
                container = true,
                containerSize = 5,
                location = Item.Item.ItemLocation.Room,
                description = new Description()
                {
                    exam =
            "A small poorly crafted wooden chest sits on the floor at the foot of the bed. There is no visible lock.",
                    look = "A small poorly crafted wooden chest sits on the floor at the foot of the bed."
                },
                keywords = new List<string>()
                {
                    "Chest",
                    "Wooden",
                    "Wooden Chest"
                },
                containerItems = new List<Item.Item>()
                {
                    leather
                }
            };

            var bedContainer = new Item.Item
            {
                name = "bed",
                stuck = true,
                type = Item.Item.ItemType.Container,
                open = true,
                canOpen = false,
                container = true,
                containerSize = 5,
                location = Item.Item.ItemLocation.Room,
                description = new Description(),
                keywords = new List<string>(),
               isHiddenInRoom = true,
                containerItems = new List<Item.Item>()
                {
                    leatherGloves
                }
            };
 
            room.items.Add(chest);
            room.items.Add(bedContainer);

            room.mobs.Add(Goblin.WeakGoblin());

 
            room.Emotes.Add("You hear a howl in the distance.");

            room.exits.Add(west);

            return room;
        }

        public static Room.Room TutorialGoblinCampTentSouth()
        {

            var room = new Room.Room
            {
                region = "Tutorial",
                area = "Tutorial",
                areaId = 5,
                title = "A tent in the goblin camp",
                description = "<p>A small bed and wooden chest has been set up against the side of the tent, filling most of the space. A small cluttered desk sits on the other side leaving just about enough room to enter and leave.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<PlayerSetup.Player>(),
                terrain = Room.Room.Terrain.Field,
                keywords = new List<RoomObject>(),
                corpses = new List<PlayerSetup.Player>(),
                players = new List<PlayerSetup.Player>(),
                fighting = new List<string>(),
                clean = true

            };


            var west = new Exit
            {
                name = "North",
                area = "Tutorial",
                region = "Tutorial",
                areaId = 3,
                keywords = new List<string>(),
                hidden = false,
                locked = false,
                canLock = true,
                canOpen = true,
                open = true,
                doorName = null

            };

            var leather = new Item.Item
            {
                armourType = Item.Item.ArmourType.Leather,
                eqSlot = Item.Item.EqSlot.Legs,
                description = new Description()
                {
                    look = "A pair of worn leather trousers.",
                    exam =
                        "A pair of worn leather trousers.",

                },
                location = Item.Item.ItemLocation.Room,
                slot = Item.Item.EqSlot.Legs,
                type = Item.Item.ItemType.Armour,
                name = "A pair of worn leather trousers",
                ArmorRating = new ArmourRating()
                {
                    Armour = 2,
                    Magic = 0
                },
                itemFlags = new EditableList<Item.Item.ItemFlags>()
                {
                    Item.Item.ItemFlags.equipable
                },
                Weight = 0.2,
                equipable = true,
                keywords = new List<string>()
                {
                    "leather",
                    "trousers",
                    "leather trousers",
                    "pants"
                }

            };

            var chest = new Item.Item
            {
                name = "Basic looking wooden chest",
                stuck = true,
                type = Item.Item.ItemType.Container,
                open = false,
                canOpen = true,
                container = true,
                containerSize = 5,
                location = Item.Item.ItemLocation.Room,
                description = new Description()
                {
                    exam =
                        "A small poorly crafted wooden chest sits on the floor at the foot of the bed. There is no visible lock.",
                    look = "A small poorly crafted wooden chest sits on the floor at the foot of the bed."
                },
                keywords = new List<string>()
                {
                    "Chest",
                    "Wooden",
                    "Wooden Chest"
                },
                containerItems = new List<Item.Item>()
                {
                    leather
                }
            };

            room.items.Add(chest);

            var goblin = Goblin.WeakGoblin();
            goblin.Gold = 10;
            

            room.mobs.Add(goblin);


            room.Emotes.Add("You hear a howl in the distance.");

            room.exits.Add(west);

            return room;
        }


        public static Room.Room TutorialRoom2()
        {

            var room = new Room.Room
            {
                region = "Tutorial",
                area = "Tutorial",
                areaId = 10,
                title = "Deep in the forest",
                description = "<p>You run forward, fleeing the ambush. You look back for one last time to see Wilhelm let out an ear piercing scream as he is over run by the goblins. " +
                              "Turning back around you greet a club that whacks you in the face sending you sprawling across the floor, your ears are ringing then everything goes black.</p>",

                //Defaults
                exits = new List<Exit>(),
                items = new List<Item.Item>(),
                mobs = new List<PlayerSetup.Player>(),
                terrain = Room.Room.Terrain.Field,
                keywords = new List<RoomObject>(),
                corpses = new List<PlayerSetup.Player>(),
                players = new List<PlayerSetup.Player>(),
                fighting = new List<string>(),
                clean = true,
                EventOnEnter = "rescue"


            };
 
     

            return room;
        }
    }
}