using System.Drawing.Drawing2D;

namespace asasa
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click_2(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click_3(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // Basit kontrol �rne�i (ger�ek uygulamada veritaban� kullan�n)
            if (username == "admin" && password == "12345")
            {
                MessageBox.Show("Giri� ba�ar�l�!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Ana formu a�
                MainForm mainForm = new MainForm();
                mainForm.Show();
                this.Hide(); // Login formunu gizle
            }
            else
            {
                MessageBox.Show("Ge�ersiz kullan�c� ad� veya �ifre!", "Hata",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Text = ""; // �ifre alan�n� temizle
                txtPassword.Focus(); // �ifre alan�na odaklan
            }


        }
    }
}
    