using DeliveryClient.Dto;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DeliveryClient.ViewModels
{
    public class NewOrderDtoViewModel : INotifyPropertyChanged
    {
        private NewOrderDto _newOrderDto;

        public NewOrderDto NewOrderDto 
        {
            get => _newOrderDto;
            set
            {
                _newOrderDto = value;
                OnPropertyChanged(nameof(NewOrderDto));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
