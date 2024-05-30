using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        private readonly HttpClient _client;
        private readonly string _apiUrl = "http://localhost/api_project/api.php";

        public Form1()
        {
            InitializeComponent();
            _client = new HttpClient();
        }

        private async void btnGetUsers_Click(object sender, EventArgs e)
        {
            try
            {
                var users = await _client.GetFromJsonAsync<User[]>(_apiUrl);
                lstUsers.Items.Clear();
                if (users != null && users.Length > 0)
                {
                    foreach (var user in users)
                    {
                        lstUsers.Items.Add($"{user.userid}: {user.username} - {user.email} - {user.bio}");
                    }
                }
                else
                {
                    MessageBox.Show("No users found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred: {ex.Message}");
            }
        }

        private async void btnAddUser_Click(object sender, EventArgs e)
        {
            try
            {
                var newUser = new User { username = txtUsername.Text, pass = txtPassword.Text, email = txtEmail.Text, bio = txtBio.Text };
                var response = await _client.PostAsJsonAsync(_apiUrl, newUser);
                response.EnsureSuccessStatusCode(); // Throw exception if HTTP response is not successful
                var responseBody = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"POST Response: {responseBody}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred: {ex.Message}");
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            // This event handler is called when the form is loaded.
            // You can add any initialization code here if needed.
        }

        private void lstUsers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

    public class User
    {
        public int userid { get; set; }
        public string username { get; set; }
        public string pass { get; set; }
        public string email { get; set; }
        public string bio { get; set; }
    }
}
