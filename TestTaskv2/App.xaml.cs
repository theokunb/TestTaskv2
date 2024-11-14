using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TestTaskv2.Repository;
using TestTaskv2.Services;
using TestTaskv2.ViewModel;

namespace TestTaskv2
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;
        private const string connectionString = "Host=localhost;Database=postgres;Username=postgres;Password=12345;Port=5432";

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddScoped(provider => new MainWindow()
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            });
            services.AddScoped<MainViewModel>();

            services.AddScoped<IDataRepository, DataRepository>(provider => new DataRepository(connectionString));
            services.AddScoped<IDataSourceVisitor, DataSourceVisitor>(provider =>
            {
                var xmlParser = _serviceProvider.GetRequiredService<XmlParser>();
                var htmlParser = _serviceProvider.GetRequiredService<HtmlParser>();
                return new DataSourceVisitor(xmlParser, htmlParser);
            });
            services.AddScoped<XmlParser>();
            services.AddScoped<HtmlParser>();

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _serviceProvider.GetRequiredService<MainWindow>().Show();
        }
    }
}
