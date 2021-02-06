using LibMiScsi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace MiScsi
{
    public static class MainWindowCommands
    {
        public static readonly ICommand Refresh = new RoutedUICommand(
            "Refresh Connections",
            "MainWindowRefresh",
            typeof(MainWindowCommands),
            new InputGestureCollection() { new KeyGesture(Key.F5) }
            );

        public static readonly ICommand ToggleConnected = new RoutedUICommand(
            "Toggle Connected",
            "MainWindowToggleConnect",
            typeof(MainWindowCommands)
            );
    }


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IscsiManager iscsiManager = new IscsiManager();
        private int working = 0;
        public MainWindow()
        {
            InitializeComponent();
            Refresh();

        }

        private void Refresh()
        {
            new Thread(() =>
            {
                working += 1;
                try
                {


                    var connections = iscsiManager.ListConnections();
                    Debug.WriteLine(connections);
                    Dispatcher.Invoke(() => { connectionsGrid.ItemsSource = connections; });
                    //Thread.Sleep(3000);
                }
                finally
                {
                    working -= 1;
                }
            }).Start();
        }


        private void Iscsi_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            Debug.WriteLine("working {0}", working);
            e.CanExecute = working == 0;
        }

        private void Refresh_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Refresh();
        }

        private void ToggleConnection_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var connection = (e.OriginalSource as Button).DataContext as IscsiConnection;

            new Thread(() =>
            {
                working += 1;
                try
                {
                    if (connection.IsConnected)
                    {
                        iscsiManager.Disconnect(connection);
                    }
                    else
                    {
                        iscsiManager.Connect(connection);
                    }
                    Refresh();
                }
                finally
                {
                    working -= 1;
                }
            }).Start();

        }
    }
}
