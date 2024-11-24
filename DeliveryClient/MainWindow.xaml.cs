using DeliveryClient.Utils;
using DeliveryClient.ViewModels;
using DeliveryService.Models;
using System.Windows;

namespace DeliveryClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var ordersViewModel = new OrdersViewModel(Configurator.LoadXmlConfig());
            DataContext = ordersViewModel;
        }
    }
}