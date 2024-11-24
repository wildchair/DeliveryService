using CommunityToolkit.Mvvm.Input;
using DeliveryClient.Dto;
using DeliveryClient.Utils;
using DeliveryService.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Xml.Serialization;
using static DeliveryClient.Utils.Configurator;

namespace DeliveryClient.ViewModels
{
    public class OrdersViewModel : INotifyPropertyChanged
    {
        private readonly string _serverAddress;

        private ObservableCollection<Order> _orders;
        private Order _selectedOrder;

        private RelayCommand _addCommand;
        private RelayCommand<string> _refreshCommand;
        private RelayCommand<Order> _deleteCommand;
        private RelayCommand<Order> _updateCommand;

        private string _searchQuery;
        private bool _inProgress = false;

        private List<IRelayCommand> _commands = new();

        public bool InProgress
        {
            get => _inProgress;
            set
            {
                _inProgress = value;
                OnPropertyChanged(nameof(InProgress));
            }
        }

        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set
            {
                _orders = value;
                OnPropertyChanged(nameof(Orders));
            }
        }

        public Order SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                OnPropertyChanged(nameof(SelectedOrder));
            }
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
            }
        }

        public RelayCommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                {
                    _addCommand = new RelayCommand(async () =>
                    {
                        InProgress = true;
                        var orderDto = new NewOrderDto()
                        {
                            Id = 1,
                            Address = "ул. Ленина",
                            DeliveryTime = DateTime.Now.AddDays(1),
                            Cargo = new() { Id = 0, Name = "Груз", Weight = 0, SizeClass = CargoSizeClass.None }
                        };
                        var wind = new AddWindow(orderDto);
                        if (!(bool)wind.ShowDialog())
                        {
                            InProgress = false;
                            return;
                        }

                        HttpClient httpClient = new HttpClient();
                        var response = await httpClient.PostAsJsonAsync($"http://{_serverAddress}/orders", orderDto);
                        var responseString = await response.Content.ReadAsStringAsync();
                        MessageBox.Show(response.StatusCode.ToString() + "\r\n" + responseString);
                        InProgress = false;
                    }, () => !InProgress);

                    _commands.Add(_addCommand);
                }
                return _addCommand;
            }
        }

        public RelayCommand<Order> DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new RelayCommand<Order>(async order =>
                    {
                        InProgress = true;
                        HttpClient httpClient = new HttpClient();
                        var response = await httpClient.DeleteAsync($"http://{_serverAddress}/orders/{order.Id}");

                        var responseString = await response.Content.ReadAsStringAsync();
                        MessageBox.Show(response.StatusCode.ToString() + "\r\n" + responseString);
                        InProgress = false;
                    }, (id) => SelectedOrder != null && !InProgress);

                    _commands.Add(_deleteCommand);
                }
                return _deleteCommand;
            }
        }

        public RelayCommand<Order> UpdateCommand
        {
            get
            {
                if (_updateCommand == null)
                {
                    _updateCommand = new RelayCommand<Order>(async order =>
                    {
                        InProgress = true;
                        var updateOrderDto = new UpdateOrderDto()
                        {
                            Address = order.Address,
                            DeliveryTime = order.DeliveryTime,
                            Description = order.Description,
                            State = order.State,
                            Cargo = order.Cargo,
                            CourierId = order.Courier == null ? 0 : order.Courier.Id
                        };

                        var wind = new UpdateWindow(updateOrderDto);
                        if (!(bool)wind.ShowDialog())
                        {
                            InProgress = false;
                            return;
                        }

                        HttpClient httpClient = new HttpClient();
                        var response = await httpClient.PutAsJsonAsync($"http://{_serverAddress}/orders/{order.Id}", updateOrderDto);

                        var responseString = await response.Content.ReadAsStringAsync();
                        MessageBox.Show(response.StatusCode.ToString() + "\r\n" + responseString);
                        InProgress = false;
                    }, (id) => SelectedOrder != null && !InProgress);

                    _commands.Add(_updateCommand);
                }
                return _updateCommand;
            }
        }

        public RelayCommand<string> RefreshCommand
        {
            get
            {
                if (_refreshCommand == null)
                {
                    _refreshCommand = new RelayCommand<string>(async query =>
                    {
                        InProgress = true;
                        query = String.IsNullOrEmpty(query) ? String.Empty : "/search/" + query;

                        HttpClient httpClient = new HttpClient();
                        var response = await httpClient.GetAsync($"http://{_serverAddress}/orders{query}");
                        var orders = await response.Content.ReadFromJsonAsync<ObservableCollection<Order>>();

                        Orders = orders;

                        var responseString = await response.Content.ReadAsStringAsync();
                        MessageBox.Show(response.StatusCode.ToString() + "\r\n" + responseString);
                        InProgress = false;
                    }, (query) => !InProgress);

                    _commands.Add(_refreshCommand);
                }
                return _refreshCommand;
            }
        }

        public OrdersViewModel(Configurator.Config config)
        {
            _serverAddress = config.ServerAddress + ":" + config.ServerPort;

            InitializeDB(config.Orders);
        }

        private async void InitializeDB(Order[] orders)
        {
            InProgress = true;

            HttpClient httpClient = new HttpClient();
            var response = await httpClient.PostAsJsonAsync($"http://{_serverAddress}/orders/initialize", orders);
            var responseString = await response.Content.ReadAsStringAsync();
            MessageBox.Show(response.StatusCode.ToString() + "\r\n" + responseString);
            InProgress = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            foreach (var command in _commands) 
                command.NotifyCanExecuteChanged();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
