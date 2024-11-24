using DeliveryClient.Dto;
using DeliveryClient.ViewModels;
using System.Windows;

namespace DeliveryClient
{
    /// <summary>
    /// Логика взаимодействия для UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        public UpdateWindow(UpdateOrderDto updateOrderDto)
        {
            InitializeComponent();

            DataContext = new UpdateOrderDtoViewModel() { UpdateOrderDto = updateOrderDto };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
