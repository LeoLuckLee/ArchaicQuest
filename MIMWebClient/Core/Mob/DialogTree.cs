﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MIMWebClient.Core.Player;

namespace MIMWebClient.Core.Mob
{
    public class DialogTree
    {
        /// <summary>
        /// ID = MobName + int e.g Modo1
        /// </summary>
        public string Id { get; set; }
        public int? QuestId { get; set; }
        public string Message { get; set; }
        public string MatchPhrase { get; set; }
        public bool? GiveQuest { get; set; } = false;
        public bool? GivePrerequisiteItem { get; set; } = false;
        public Item.Item GiveItem { get; set; }
        public string GiveItemEmote { get; set; }
        public Skill GiveSkill { get; set; }
        public List<Responses> PossibleResponse { get; set; }
        public bool? DoAction { get; set; } = false;
        public string ShowIfOnQuest { get; set; } = String.Empty;
        public int ShowIfLevelUpTo { get; set; } = 0;
        public bool ShowIfEvil { get; set; } = false;
        public bool ShowIfGood { get; set; } = false;
        public bool ShowIfNeutral { get; set; } = false;
        public string DontShowIfOnQuest { get; set; } = String.Empty;

        public Action CallScript { get; set; }
}
}