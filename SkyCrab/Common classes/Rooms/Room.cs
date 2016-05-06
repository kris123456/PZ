﻿using SkyCrab.Common_classes.Players;
using SkyCrab.Common_classes.Rooms.Players;
using SkyCrab.Common_classes.Rooms.Rules;
using System.Collections.Generic;

namespace SkyCrab.Common_classes.Rooms
{
    public class TooManyPlayersInRoomException : SkyCrabException
    {

        public TooManyPlayersInRoomException() :
            base()
        {
        }

    }

    public class PlayerAlreadyInRoomException : SkyCrabException
    {

        public PlayerAlreadyInRoomException() :
            base()
        {
        }

    }

    public class NoSuchPlayerInRoomException : SkyCrabException
    {

        public NoSuchPlayerInRoomException() :
            base()
        {
        }

    }

    public class Room
    {

        public uint MAX_PLAYERS = 4;

        private uint id;
        private Player owner;
        private string name;
        private RoomType roomType;
        private readonly RuleSet rules = new RuleSet();
        private LinkedList<PlayerInRoom> players = new LinkedList<PlayerInRoom>();


        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public Player Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        public string Name
        {
            get { return name; }
            set
            {
                LengthLimit.RoomName.CheckAndThrow(value);
                name = value;
            }
        }


        public RoomType RoomType
        {
            get { return roomType; }
            set { roomType = value; }
        }

        public RuleSet Rules
        {
            get { return rules; }
        }



        public string MaxPlayersLimit
        {
            get
            {
                return rules.maxPlayerCount.value.ToString();
            }
        }

        public string MaxTimeLimit
        {
            get
            {
                return rules.maxRoundTime.value.ToString();
            }
        }

        public string IsRulesFive
        {
            get
            {
                if(rules.fivesFirst.value == true)
                {
                    return "✓";
                }
                else if(rules.fivesFirst.value == false)
                {
                    return "-";
                }
                return "-";
            }
        }

        public string IsRulesExchange
        {
            get
            {
                if (rules.restrictedExchange.value == true)
                {
                    return "✓";
                }
                else if (rules.restrictedExchange.value == false)
                {
                    return "-";
                }
                return "-";
            }
        }

        public LinkedList<PlayerInRoom> Players
        {
            get { return players; }
        }

        public bool AllPlayersReady
        {
            get
            {
                foreach (PlayerInRoom player in players)
                    if (!player.IsReady)
                        return false;
                return true;
            }
        }


        public Room()
        {
        }

        public Room(uint id, Player owner, string name, RoomType roomType, RuleSet rules)
        {
            this.id = id;
            this.owner = owner;
            LengthLimit.RoomName.CheckAndThrow(name);
            this.name = name;
            this.roomType = roomType;
            this.rules = rules;
        }

        public void AddPlayer(Player player)
        {
            if (players.Count >= MAX_PLAYERS)
                throw new TooManyPlayersInRoomException();
            if (hasPlayer(player.Id))
                throw new PlayerAlreadyInRoomException();
            PlayerInRoom playerInRoom = new PlayerInRoom(player);
            players.AddLast(playerInRoom);
        }

        public void RemovePlayer(uint playerId)
        {
            for (var i = players.First; i != null; i = i.Next)
                if (i.Value.Player.Id == playerId)
                {
                    players.Remove(i);
                    return;
                }
            throw new NoSuchPlayerInRoomException();
        }

        public bool hasPlayer(uint playerId)
        {
            foreach (PlayerInRoom playerInRoom in players)
                if (playerInRoom.Player.Id == playerId)
                    return true;
            return false;
        }
        
        public void SetPlayerReady(uint playerId, bool ready)
        {
            foreach (PlayerInRoom playerInRoom in players)
                if (playerInRoom.Player.Id == playerId)
                {
                    playerInRoom.IsReady = ready;
                    return;
                }
        }

    }
}
