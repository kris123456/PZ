﻿using SkyCrab.Classes.Menu.LoggedPlayer;
using SkyCrab.Common_classes.Rooms;
using SkyCrab.Connection.PresentationLayer.Messages;
using SkyCrab.Connection.PresentationLayer.Messages.Menu.Rooms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SkyCrab.Classes.Menu
{
    /// <summary>
    /// Interaction logic for PlayAsLoggedPlayer.xaml
    /// </summary>
    public partial class PlayAsLoggedPlayer : UserControl
    {

        ManageRooms manageRooms = null;

        ObservableCollection<String> typeOfRoomLabels;
        ObservableCollection<String> minTimeLimitLabels;
        ObservableCollection<String> maxTimeLimitLabels;
        ObservableCollection<String> minCountPlayersLabels;
        ObservableCollection<String> maxCountPlayersLabels;

        public PlayAsLoggedPlayer()
        {
            InitializeComponent();
            manageRooms = new ManageRooms();
            Room filterRoom = new Room();

            filterRoom.Name = "";
            filterRoom.RoomType = RoomType.PUBLIC;
            filterRoom.Rules.fivesFirst.indifferently = true;
           // filterRoom.Rules.fivesFirst.value = true;
           filterRoom.Rules.restrictedExchange.indifferently = true;
            //filterRoom.Rules.restrictedExchange.value = true;
            filterRoom.Rules.maxPlayerCount.indifferently = true;
            filterRoom.Rules.maxRoundTime.indifferently = true;

            var getListOfRooms = FindRoomsMsg.SyncPostFindRooms(App.clientConn, filterRoom, 1000);
 
            if (!getListOfRooms.HasValue)
            {
                MessageBox.Show("Brak odpowiedzi od serwera!");
                return;
            }

            var answerValue = getListOfRooms.Value;

            if (answerValue.messageId == MessageId.ROOM_LIST)
            {
                manageRooms.ListRoomsFromServer = (List<Room>)answerValue.message;
                for (int i = 0; i < manageRooms.ListRoomsFromServer.Count; i++)
                {
                    MessageBox.Show("Pokój " + manageRooms.ListRoomsFromServer[i].Name);
                    manageRooms.ListOfRooms.Add(manageRooms.ListRoomsFromServer[i]);
                }
            }

            DataContext = manageRooms;
    
        }

        private void textBoxSearchRoom_TextChanged(object sender, TextChangedEventArgs e)
        {
            // czyszczenie starej zawartości listy pokoi
            manageRooms.ClearListRooms();

            Room filterRoom = new Room();

            filterRoom.Name = textBoxSearchRoom.Text;
            //publiczny
            if(typeOfRoom.Text == "publiczny")
            {
                filterRoom.RoomType = RoomType.PUBLIC;
            }
            // znajomi
            else if (typeOfRoom.Text == "znajomi")
            {
                filterRoom.RoomType = RoomType.FRIENDS;
            }

            if(fivesFirst.IsChecked.Value == true)
            {
                filterRoom.Rules.fivesFirst.value = true;
            }
            else if(fivesFirst.IsChecked.Value == false)
            {
                filterRoom.Rules.fivesFirst.value = false;
            }

            if(restrictedExchange.IsChecked.Value == true)
            {
                filterRoom.Rules.restrictedExchange.value = true;
            }
            else if(restrictedExchange.IsChecked.Value == false)
            {
                filterRoom.Rules.restrictedExchange.value = false;
            }
            // stany pośrednie checkboxów - reguły gry
            if(fivesFirst.IsChecked == null)
            {
                filterRoom.Rules.fivesFirst.indifferently = true;
            }
            if(restrictedExchange.IsChecked == null)
            {
                filterRoom.Rules.restrictedExchange.indifferently = true;
            }
            // min i max czas gry

            if(int.Parse(minTimeLimit.Text) >= 0)
            {
                filterRoom.Rules.maxRoundTime.min = uint.Parse(minTimeLimit.Text);
            }

            if(int.Parse(maxTimeLimit.Text) >= 0)
            {
                filterRoom.Rules.maxRoundTime.max = uint.Parse(maxTimeLimit.Text);
            }

            // min i max liczba graczy
            
            if(int.Parse(minCountPlayers.Text) > 0)
            {
                filterRoom.Rules.maxPlayerCount.min = byte.Parse(minCountPlayers.Text);
            }

            if(int.Parse(maxCountPlayers.Text) > 0)
            {
                filterRoom.Rules.maxPlayerCount.max = byte.Parse(maxCountPlayers.Text);
            }

            var getListOfRooms = FindRoomsMsg.SyncPostFindRooms(App.clientConn, filterRoom, 1000);

            if (!getListOfRooms.HasValue)
            {
                MessageBox.Show("Brak odpowiedzi od serwera!");
                return;
            }

            var answerValue = getListOfRooms.Value;

            if (answerValue.messageId == MessageId.ROOM_LIST)
            {
                manageRooms.ListRoomsFromServer = (List<Room>)answerValue.message;
                for (int i = 0; i < manageRooms.ListRoomsFromServer.Count; i++)
                {
                    MessageBox.Show("Pokój " + manageRooms.ListRoomsFromServer[i].Name);
                    manageRooms.ListOfRooms.Add(manageRooms.ListRoomsFromServer[i]);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new MainMenuLoggedPlayer());
        }

        private void RoomCreateButton_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new CreateRoomForLoggedPlayers());
        }

        private void typeOfRoom_Loaded(object sender, RoutedEventArgs e)
        {
            typeOfRoomLabels = new ObservableCollection<String>();

            typeOfRoomLabels.Add("publiczny");
            typeOfRoomLabels.Add("znajomi");

            typeOfRoom.ItemsSource = typeOfRoomLabels;
            typeOfRoom.SelectedIndex = 0;
        }

        private void minTimeLimit_Loaded(object sender, RoutedEventArgs e)
        {
            minTimeLimitLabels = new ObservableCollection<String>();

            for(int i=5; i <=15; i+=5)
            {
                minTimeLimitLabels.Add(i.ToString());
            }
            for(int i=30; i <=60; i+=15)
            {
                minTimeLimitLabels.Add(i.ToString());
            }
            minTimeLimitLabels.Add("Brak limitu");

            minTimeLimit.ItemsSource = minTimeLimitLabels;
            minTimeLimit.SelectedIndex = 0;
        }

        private void maxTimeLimit_Loaded(object sender, RoutedEventArgs e)
        {
            maxTimeLimitLabels = new ObservableCollection<String>();

            for (int i = 5; i <= 15; i += 5)
            {
                maxTimeLimitLabels.Add(i.ToString());
            }
            for (int i = 30; i <= 60; i += 15)
            {
                maxTimeLimitLabels.Add(i.ToString());
            }
            maxTimeLimitLabels.Add("Brak limitu");
            maxTimeLimit.ItemsSource = minTimeLimitLabels;
            maxTimeLimit.SelectedIndex = 6;

        }

        private void minCountPlayers_Loaded(object sender, RoutedEventArgs e)
        {
            minCountPlayersLabels = new ObservableCollection<String>();
            for(int i=1; i <=4; i++)
            {
                minCountPlayersLabels.Add(i.ToString());
            }
            minCountPlayers.ItemsSource = minCountPlayersLabels;
            minCountPlayers.SelectedIndex = 0;
        }

        private void maxCountPlayers_Loaded(object sender, RoutedEventArgs e)
        {
            maxCountPlayersLabels = new ObservableCollection<String>();
            for (int i = 1; i <= 4; i++)
            {
                maxCountPlayersLabels.Add(i.ToString());
            }
            maxCountPlayers.ItemsSource = maxCountPlayersLabels;
            maxCountPlayers.SelectedIndex = 3;
        }
    }
}
