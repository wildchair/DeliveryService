using DeliveryClient.Dto;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DeliveryClient.ViewModels
{
    public class UpdateOrderDtoViewModel : INotifyPropertyChanged
    {
        private UpdateOrderDto _updateOrderDto;

        public UpdateOrderDto UpdateOrderDto
        {
            get => _updateOrderDto;
            set
            {
                _updateOrderDto = value;
                OnPropertyChanged(nameof(UpdateOrderDto));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}