using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WindowsAutomatedSimplifier.NetworkSettings
{

    /// <summary>
    /// Interaktionslogik für Network.xaml
    /// </summary>
    public partial class Network
    {
        public ObservableCollection<KeyValuePair<string, object>> SelectedInterface { get; set; } = new ObservableCollection<KeyValuePair<string, object>>();
        public List<object> List { get; }
        public Network()
        {
            Task createTask = Task.Factory.StartNew(NetworkLogic.CreateNetInformation);
            InitializeComponent();
            createTask.Wait();
            List = NetworkLogic.GetNetInformationNames();
            Interfaces.SelectionChanged += (sender, args) =>
            {
                SelectedInterface.Clear();

                foreach (var pair in NetworkLogic.GetNetInformationByName(Interfaces.SelectedItem.ToString()))
                    SelectedInterface.Add(pair);
            };
            Interfaces.SelectedIndex = 0;

            PrintIP();
        }

        public void ListNetCards()
        {
            foreach (var cardsProperty in NetworkLogic.GetNetworkCardsProperties())
            {
                DataGrid data = new DataGrid { ItemsSource = cardsProperty };
            }
        }

        private void RefreshIP_OnClick(object sender, RoutedEventArgs e)
        {
            NetworkLogic.RefreshIP();
            PrintIP();
        }

        private void PrintIP()
        {
            IPAddress ip = NetworkLogic.GetLocalIPAddress();
            CurrIP.Content = ip + " / " + NetworkLogic.GetSubnetMask(ip);
        }
    }
}
