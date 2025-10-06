

namespace xdCzat
{
    public partial class MainPage : ContentPage
    {
    

        public MainPage()
        {
            InitializeComponent();
        }

        private void Send(object sender, EventArgs e)
        {
            string userName = UsernameEntry.Text;
            string message = ChatEntry.Text;

            ChatEntry.Text = string.Empty;

            Label messageLabel = new Label();
            messageLabel.Text = userName + ": " + message;
            ChatHistory.Children.Add(messageLabel);
        }
    }
}
