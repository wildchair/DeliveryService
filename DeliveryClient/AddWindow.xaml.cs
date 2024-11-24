using DeliveryClient.Dto;
using DeliveryClient.ViewModels;
using System.Windows;

namespace DeliveryClient
{
    /// <summary>
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public AddWindow(NewOrderDto newOrderDto)
        {
            InitializeComponent();

            DataContext = new NewOrderDtoViewModel() { NewOrderDto = newOrderDto };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
