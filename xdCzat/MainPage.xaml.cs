

using System.Net.Http.Json;
using System.Web;


namespace xdCzat
{
    public partial class MainPage : ContentPage
    {

        DateTime lastChatRefresh;
    
       IDispatcherTimer timer;


        public MainPage()
        {
            InitializeComponent();
            lastChatRefresh = DateTime.UnixEpoch;
            timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, e) => GetHistory();
            timer.Start();
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
        private void GetHistory()
        {

            string timestamp = HttpUtility.UrlEncode(lastChatRefresh.ToString("o"));
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7094/");
                HttpResponseMessage response = client.GetAsync("chat?timestamp="+timestamp).Result;
                List<ChatMessage> messages = response.Content.ReadFromJsonAsync<List<ChatMessage>>().Result
                                                       ?? new List<ChatMessage>();

                foreach (ChatMessage message in messages)
                {
                    Label messageLabel = new Label();
                    messageLabel.Text = message.Author + ": " + message.Message;
                    ChatHistory.Children.Add(messageLabel);
                    
                }
            }
            ChatScrollView.ScrollToAsync(ChatHistory, ScrollToPosition.End, true);
            lastChatRefresh = DateTime.UtcNow;
        }
    }
}
