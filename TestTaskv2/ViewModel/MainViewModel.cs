using Microsoft.Win32;
using System.Windows.Input;
using TestTaskv2.Core;
using TestTaskv2.Entity;
using TestTaskv2.Repository;
using TestTaskv2.Services;

namespace TestTaskv2.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IDataRepository _dataRepository;
        private readonly IDataSourceVisitor _visitor;

        public MainViewModel()
        {
        }

        public MainViewModel(IDataRepository dataRepository, IDataSourceVisitor visitor)
        {
            _dataRepository = dataRepository;
            _visitor = visitor;

            CommandHrefExecute = new RelayCommand(param => OnHrefExecute(param));
            CommandXmlExecute = new RelayCommand(param => OnXmlExecute(param));
            CommandAttach = new RelayCommand(param => OnAttach(param));

            FileAttached = false;

            Link = "https://tenmon.ru/1/0123200000319002908";
        }

        private string _link = "";
        public string Link
        {
            get => _link;
            set
            {
                _link = value;
                OnPropertyChanged(nameof(Link));
                OnPropertyChanged(nameof(LinkFiled));
            }
        }
        public bool LinkFiled => Link.Length > 0;
        private bool _fileAttached;
        public bool FileAttached
        {
            get => _fileAttached;
            set
            {
                _fileAttached = value;
                OnPropertyChanged(nameof(FileAttached));
            }
        }
        private string _statusText;
        public string StatusText
        {
            get => _statusText;
            set
            {
                _statusText = value;
                OnPropertyChanged(nameof(StatusText));
            }
        }

        public ICommand CommandAttach { get; private set; }
        public ICommand CommandHrefExecute { get; private set; }
        public ICommand CommandXmlExecute { get; private set; }

        private DataSource _datasource;

        private void OnAttach(object param)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Xml документ(*.xml)|*.xml";
            var showdialog = openFileDialog.ShowDialog();

            if (showdialog != true)
            {
                return;
            }

            _datasource = new XmlSource
            {
                FilePath = openFileDialog.FileName
            };

            FileAttached = true;
            StatusText = openFileDialog.SafeFileName;
        }

        private void OnHrefExecute(object parameter)
        {
            _datasource = new HrefSource()
            {
                Link = parameter.ToString()
            };

            var data = _datasource?.Accept(_visitor);
            InsertDataToDb(data);
        }

        private void OnXmlExecute(object parameter)
        {
            var data = _datasource?.Accept(_visitor);
            InsertDataToDb(data);
        }

        private async void InsertDataToDb(PurchaseData data)
        {
            if(data == null)
            {
                return;
            }

            int purchaseDataId = await _dataRepository.CreateAsync(data);
            StatusText = $"created purchaseData {purchaseDataId}";
        }
    }
}
