using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Vsto.Sample.Client.Commands;
using Vsto.Sample.Client.Models;

namespace Vsto.Sample.Client.ViewModels
{
    public class ContentViewModel : INotifyPropertyChanged
    {
        private string _output;
        private readonly IServiceClient _serviceClient;

        public ContentViewModel(IServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
            _serviceClient.ActionLogged += logOutput => Output += logOutput;
            LoadContent = new DelegateCommand(_ => _serviceClient.LoadData());
        }

        public string ServiceEndpoint
        {
            get { return _serviceClient.Endpoint; }
            set
            {
                if (string.Equals(_serviceClient.Endpoint, value)) return;
                _serviceClient.Endpoint = value;
                OnPropertyChanged(@"ServiceEndpoint");
            }
        }

        public string Output
        {
            get { return _output; }
            set
            {
                if (string.Equals(_output, value)) return;
                _output = value;
                OnPropertyChanged(@"Output");
            }
        }

        public ICommand LoadContent { get; private set; }

        protected void OnPropertyChanged(string propertyName)
        {
            var subject = new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, subject);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
