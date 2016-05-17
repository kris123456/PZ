﻿using SkyCrab.Common_classes.Chats;
using SkyCrab.Common_classes.Games.Pouches;
using SkyCrab.Common_classes.Games.Racks;
using SkyCrab.Common_classes.Players;
using SkyCrab.SkyCrabClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyCrab
{
    static class SkyCrabGlobalVariables
    {
       public static Player player;
       public static SkyCrabRoom room; // pokój z uzupełnionymi danymi od serwera , wykorzystywany m.in. przez lobby
        public static ChatMessage chatMessages;
        public static String MessagesLog = "";
        public static bool isGame = false;
        public static bool isGetNewTile = false; // zmienna pomocnicza przy otrzymaniu od serwera komunikatu NEW_TILE
        public static bool anotherPlayersGetNewTile = false;
        public static uint anotherPlayersGetNewTileCount = 0;
        public static DrawedLetters newTile; // nowe płytki zwrócone przez serwer
        public static uint GameId;
        public static bool isMyRound = false;
        public static LostLetters lostLetters = new LostLetters(); // zmienna do której są dopisywane płytki 
        public static readonly object roomLock = new object();
    }
}
