using Common;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        private readonly MessageServiceClient _messageServiceClient = new();

        private string MessageContent
        {
            get => textBox1.Text;
            set => textBox1.Text = value;
        }

        public Form1()
        {
            Load += Form1_Load;
            InitializeComponent();
            textBox1.KeyDown += TextBox1_KeyDown;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

            object lockObj = new();
            _messageServiceClient.Subscribe()
                .ForEachAsync(message =>
                {
                    lock (lockObj)
                    {
                        dispatcher.Invoke(() => listView1.Items.Add(message.Content));
                    }
                });
        }

        private async Task Send()
        {
            if (string.IsNullOrWhiteSpace(MessageContent))
            {
                return;
            }

            await _messageServiceClient.SendAsync(MessageContent);
            MessageContent = string.Empty;
        }

        private async void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                await Send();
            }
        }
    }
}