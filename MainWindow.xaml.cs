using System.Windows;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnRequestButtonClick(object sender, RoutedEventArgs e)
        {
            string userRole = "user"; // Здесь можно динамически задавать роль
            ISubject proxy = new Proxy(userRole);
            string request = RequestTextBox.Text;

            // Выполнение запроса через прокси
            string result = proxy.Request(request);

            // Отображение результата
            ResultTextBlock.Text = result;
        }
    }
}
