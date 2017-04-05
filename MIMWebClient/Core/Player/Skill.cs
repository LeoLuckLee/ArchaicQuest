﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIMWebClient.Core.Events;

namespace MIMWebClient.Core.Player
{
    using MIMWebClient.Core.Player.Skills;

    using MongoDB.Bson.Serialization.Attributes;

    public class Skill
    {
        public enum Type
        {
            Skill,
            Spell
        };

        /// <summary>
        /// Notes taking from here https://en.wikipedia.org/wiki/Magic_of_Dungeons_%26_Dragons
        /// </summary>
        public enum SpellGroupType
        {
            [Description("This school is focused on protective spells, as well as spells which cancel or interfere with other spells, magical effects or supernatural abilities, such as Break Enchantment, Dimensional Anchor, Dispel Magic or Remove Curse. Wizards who specialize in this school are known as Abjurers.")]
            Abjuration,
            [Description("This school is focused on instantaneous transportation, conjuring manifestations of creatures, energy or objects, and object creation. Mages who specialize in this school are known as Conjurers.")]
            Conjuration,
            [Description("This school is focused on acquiring information, spells such as Detect Magic, Identify and Read Magic, Wizards who specialize in this school are known as Diviners.")]
            Divination,
            [Description("Holy spells for Cleric, druid types")]
            Divine,
            [Description("Enchanment, buffs and charm spells, Wizards who specialize in this school are known as Enchanters ")]
            Enchantment,
            [Description("Invocation is focused on damaging energy-based spells such as Fireball, Lightning Bolt, and Cone of Cold. It also includes conjurations of magical energy, such as Wall of Force, Darkness, Light Wizards who specialize in this school are known as Invokers.")]
            Invocation,
            [Description("This school is divided into five subschools: figment, glamer, pattern, phantasm, and shadow. Figment spells create artificial sensations with no physical substance.  Glamer spells alter the target's sensory properties, and can cause invisibility. Pattern spells create insubstantial images which affect the minds of the viewers, and can inflict harm. Phantasm spells create hallucinations which can be harmful. Shadow spells use magical shadows to create things with physical substance. Wizards who specialize in this school are known as Illusionists.")]
            Illusion,
            [Description("Necromancy spells involve death, undeath, and the manipulation of life energy. Necromancy can usually be divided into three or four categories: spells that help or create the Undead, like Animate Dead; spells that hurt the Undead, like Disrupt Undead; spells that hurt other people, like Enervation or Vampiric Touch; and spells that manipulate life in order to heal Wizards who specialize in this school are known as Necromancers.")]
            Necromancy,
            [Description(" Spells in this school alter the properties of their targets. Examples include Bull's Strength, Fabricate, Polymorph, Plant Growth, Move Earth, and Water Breathing Wizards who specialize in this school are known as Transmuters.")]
            Transmutation,
            [Description("Universal spells have effects too broad to place into one class, or too useful for any specialist to consider forsaking. They often can perform multiple effects, or perform a very specific effect that does not fit into another category.")]
            Universal,
            None
        };

        public enum ElementalType
        {
            Fire,
            Frost,
            Holy,
            Poison,
            Shocking,
            Vampric,
            None
        };

        public string Name { get; set; }

        public int Proficiency { get; set; }

        public bool Passive { get; set; }

        public int Delay { get; set; }

        public int CoolDown { get; set; }

        //Says what status a skill can be used from
        // Fighting means in a fight only
        //Resting means everything
        //standing means all but resting
        //can't use skills while asleep
        public string UsableFromStatus { get; set; }
        public int LevelObtained { get; set; }
        public int ManaCost { get; set; }
        public int MaxProficiency { get; set; }
        [BsonIgnore]
        public Help HelpText { get; set; }
        [BsonIgnore]
        public string Syntax { get; set; }


        public static bool CheckPlayerHasSkill(PlayerSetup.Player player, string skillName)
        {

            //Check if player has spell
            var hasSpell =
               player.Skills.FirstOrDefault(
                   x =>
                       x.Name.Equals(skillName) &&
                       x.LevelObtained <= player.Level);

            if (hasSpell != null)
            {
                return true;
            }


            return false;
        }


        public static string GetSkillTarget(string target)
        {
            if (target == "")
            {
                return String.Empty;
            }

            // check to see if user typed "C magic target" or just "c magic"
            // if an actual target there wil lbe a space
            var targetLength = target.Count(x => x.Equals(' '));
            if (targetLength >= 1)
            {
                string theTarget;
                var hasQuotes = target.Contains("'\"");

                if (hasQuotes)
                {
                    theTarget = target.Substring(target.LastIndexOf('"') + 1);

                    if (string.IsNullOrEmpty(theTarget))
                    {
                        theTarget = target.Substring(target.LastIndexOf('\'') + 1);
                    }
                }
                else
                {
                    theTarget = target.Substring(target.LastIndexOf(' ') + 1);
                }


                return theTarget;

            }

            return String.Empty;
        }

        public static PlayerSetup.Player FindTarget(string target, Room.Room room)
        {
            //Find target if it's specified
            if (target == "")
            {
                return null;
            }
               
            var getTargetName = Skill.GetSkillTarget(target);

            if (getTargetName != string.Empty)
            {
                var foundTarget = Fight2.FindTarget(room, getTargetName);

                return foundTarget;
            }

            return null;
        }

    }
}
