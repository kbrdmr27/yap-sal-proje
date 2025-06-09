using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace asasa
{
    public partial class MainForm : Form
    {
        private System.Windows.Forms.Timer lessonTimer;
        private int seconds = 0;
        private int minutes = 0;
        private int hours = 0;

        // Ruh hali ve hedef bilgileri korunsun diye
        private string selectedMood = "😊 Harika";
        private string hedefText = "";

        public MainForm()
        {
            this.Text = "Öğrenci Takip Sistemi";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            lessonTimer = new System.Windows.Forms.Timer();
            lessonTimer.Interval = 1000;
            lessonTimer.Tick += LessonTimer_Tick;

            CreateSidebar();
            CreateContentPanel();
        }

        private void LessonTimer_Tick(object sender, EventArgs e)
        {
            seconds++;
            if (seconds == 60)
            {
                seconds = 0;
                minutes++;
                if (minutes == 60)
                {
                    minutes = 0;
                    hours++;
                }
            }

            Label timerLabel = GetTimerLabel();
            if (timerLabel != null)
                timerLabel.Text = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
        }

        private Label GetTimerLabel()
        {
            Panel contentPanel = (Panel)this.Controls["contentPanel"];
            foreach (Control ctrl in contentPanel.Controls)
            {
                if (ctrl is Panel p1)
                {
                    foreach (Control ctrl2 in p1.Controls)
                    {
                        if (ctrl2 is Panel p2)
                        {
                            foreach (Control ctrl3 in p2.Controls)
                            {
                                if (ctrl3 is Label lbl && lbl.Font.Size == 48)
                                {
                                    return lbl;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        private void CreateSidebar()
        {
            Panel sidebar = new Panel();
            sidebar.BackColor = Color.FromArgb(51, 51, 76);
            sidebar.Dock = DockStyle.Left;
            sidebar.Width = 200;

            Button exitButton = CreateSidebarButton("Çıkış");
            exitButton.Dock = DockStyle.Bottom;
            sidebar.Controls.Add(exitButton);

            string[] topMenuItems = { "Profil", "Derslerim", "Ana Sayfa" };

            foreach (string item in topMenuItems)
            {
                Button btn = CreateSidebarButton(item);
                sidebar.Controls.Add(btn);
            }

            this.Controls.Add(sidebar);
        }

        private Button CreateSidebarButton(string text)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Font = new Font("Segoe UI", 10);
            btn.ForeColor = Color.White;
            btn.BackColor = Color.FromArgb(51, 51, 76);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Height = 50;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(20, 0, 0, 0);
            btn.Margin = new Padding(0);
            btn.Dock = DockStyle.Top;

            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(70, 70, 100);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(51, 51, 76);

            if (text == "Çıkış")
                btn.Click += (s, e) => Application.Exit();
            else
                btn.Click += (s, e) => ShowContent(btn.Text);

            return btn;
        }

        private void CreateContentPanel()
        {
            Panel contentPanel = new Panel();
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.BackColor = Color.White;
            contentPanel.Name = "contentPanel";

            Label defaultContent = new Label();
            defaultContent.Text = "Hoşgeldiniz!";
            defaultContent.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            defaultContent.ForeColor = Color.FromArgb(51, 51, 76);
            defaultContent.Dock = DockStyle.Fill;
            defaultContent.TextAlign = ContentAlignment.MiddleCenter;

            contentPanel.Controls.Add(defaultContent);
            this.Controls.Add(contentPanel);
        }

        private void ShowContent(string menuItem)
        {
            Panel contentPanel = (Panel)this.Controls["contentPanel"];
            contentPanel.Controls.Clear();

            switch (menuItem)
            {
                case "Ana Sayfa":
                    {
                        Panel anaPanel = new Panel();
                        anaPanel.Dock = DockStyle.Fill;
                        anaPanel.BackColor = Color.White;

                        int sidebarWidth = 200;
                        int contentBoxWidth = this.ClientSize.Width - sidebarWidth - 40;

                        Panel contentBox = new Panel();
                        contentBox.Size = new Size(contentBoxWidth, 400);
                        contentBox.Location = new Point(sidebarWidth + 10, 30);
                        contentBox.BackColor = Color.White;

                        Label title = new Label();
                        title.Text = "Bugünkü Ruh Hâlin:";
                        title.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                        title.Size = new Size(contentBoxWidth - 20, 20);
                        title.Location = new Point(10, 0);

                        ComboBox moodCombo = new ComboBox();
                        moodCombo.Items.AddRange(new string[] { "😊 Harika", "🙂 İyi", "😐 Orta", "😟 Kötü", "😞 Çok Kötü" });
                        moodCombo.DropDownStyle = ComboBoxStyle.DropDownList;
                        moodCombo.Font = new Font("Segoe UI", 8);
                        moodCombo.Size = new Size(contentBoxWidth - 20, 22);
                        moodCombo.Location = new Point(10, 25);
                        moodCombo.SelectedItem = selectedMood;
                        moodCombo.SelectedIndexChanged += (s, e) => selectedMood = moodCombo.SelectedItem.ToString();

                        Label goalLabel = new Label();
                        goalLabel.Text = "Hedefin:";
                        goalLabel.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                        goalLabel.Size = new Size(contentBoxWidth - 20, 20);
                        goalLabel.Location = new Point(10, 55);

                        TextBox goalTextBox = new TextBox();
                        goalTextBox.Multiline = true;
                        goalTextBox.Font = new Font("Segoe UI", 8);
                        goalTextBox.Size = new Size(contentBoxWidth - 20, 60);
                        goalTextBox.Location = new Point(10, 80);
                        goalTextBox.ScrollBars = ScrollBars.Vertical;
                        goalTextBox.Text = hedefText;
                        goalTextBox.TextChanged += (s, e) => hedefText = goalTextBox.Text;

                        Button saveButton = new Button();
                        saveButton.Text = "Kaydet";
                        saveButton.Font = new Font("Segoe UI", 8, FontStyle.Bold);
                        saveButton.Size = new Size(80, 30);
                        saveButton.Location = new Point(contentBoxWidth - 90, 150);
                        saveButton.BackColor = Color.FromArgb(0, 120, 215);
                        saveButton.ForeColor = Color.White;
                        saveButton.FlatStyle = FlatStyle.Flat;

                        Button startLessonButton = new Button();
                        startLessonButton.Text = "Dersi Başlat";
                        startLessonButton.Font = new Font("Segoe UI", 8, FontStyle.Bold);
                        startLessonButton.Size = new Size(100, 30);
                        startLessonButton.Location = new Point(10, 160);

                        Button stopLessonButton = new Button();
                        stopLessonButton.Text = "Dersi Durdur";
                        stopLessonButton.Font = new Font("Segoe UI", 8, FontStyle.Bold);
                        stopLessonButton.Size = new Size(100, 30);
                        stopLessonButton.Location = new Point(120, 160);
                        stopLessonButton.Enabled = lessonTimer.Enabled;

                        Button resetButton = new Button();
                        resetButton.Text = "Sıfırla";
                        resetButton.Font = new Font("Segoe UI", 8, FontStyle.Bold);
                        resetButton.Size = new Size(100, 30);
                        resetButton.Location = new Point(230, 160);
                        resetButton.BackColor = Color.FromArgb(255, 69, 0);
                        resetButton.ForeColor = Color.White;
                        resetButton.FlatStyle = FlatStyle.Flat;

                        Label timerLabel = new Label();
                        timerLabel.Font = new Font("Segoe UI", 48, FontStyle.Bold);
                        timerLabel.Size = new Size(contentBoxWidth - 20, 100);
                        timerLabel.Location = new Point(10, 230);
                        timerLabel.TextAlign = ContentAlignment.MiddleCenter;
                        timerLabel.Text = $"{hours:D2}:{minutes:D2}:{seconds:D2}";

                        startLessonButton.Click += (s, e) =>
                        {
                            lessonTimer.Start();
                            startLessonButton.Enabled = false;
                            stopLessonButton.Enabled = true;
                        };

                        stopLessonButton.Click += (s, e) =>
                        {
                            lessonTimer.Stop();
                            startLessonButton.Enabled = true;
                            stopLessonButton.Enabled = false;
                        };

                        resetButton.Click += (s, e) =>
                        {
                            lessonTimer.Stop();
                            seconds = 0;
                            minutes = 0;
                            hours = 0;
                            timerLabel.Text = "00:00:00";
                            startLessonButton.Enabled = true;
                            stopLessonButton.Enabled = false;
                        };

                        saveButton.Click += (s, e) =>
                        {
                            string tarih = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                            string ruhHali = moodCombo.SelectedItem.ToString();
                            string hedef = goalTextBox.Text;
                            string calismaSuresi = timerLabel.Text;

                            MessageBox.Show(
                                $"Ruh hâlin: {ruhHali}\nHedefin: {hedef}\nÇalışma Süresi: {calismaSuresi}",
                                "Bugünün Özeti",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );

                            var yeniKayit = new GunlukDers
                            {
                                Tarih = tarih,
                                RuhHali = ruhHali,
                                Hedef = hedef,
                                CalismaSuresi = calismaSuresi
                            };

                            string filePath = "data.json";
                            List<GunlukDers> kayitlar = new List<GunlukDers>();

                            if (File.Exists(filePath))
                            {
                                string json = File.ReadAllText(filePath);
                                kayitlar = JsonSerializer.Deserialize<List<GunlukDers>>(json);
                            }

                            kayitlar.Add(yeniKayit);
                            File.WriteAllText(filePath, JsonSerializer.Serialize(kayitlar, new JsonSerializerOptions { WriteIndented = true }));

                            // sayaç sıfırlansın
                            lessonTimer.Stop();
                            seconds = 0;
                            minutes = 0;
                            hours = 0;
                            timerLabel.Text = "00:00:00";
                            startLessonButton.Enabled = true;
                            stopLessonButton.Enabled = false;
                        };

                        contentBox.Controls.Add(title);
                        contentBox.Controls.Add(moodCombo);
                        contentBox.Controls.Add(goalLabel);
                        contentBox.Controls.Add(goalTextBox);
                        contentBox.Controls.Add(saveButton);
                        contentBox.Controls.Add(startLessonButton);
                        contentBox.Controls.Add(stopLessonButton);
                        contentBox.Controls.Add(resetButton);
                        contentBox.Controls.Add(timerLabel);

                        anaPanel.Controls.Add(contentBox);
                        contentPanel.Controls.Add(anaPanel);
                    }
                    break;

                case "Derslerim":
                    {
                        Panel dersPanel = new Panel();
                        dersPanel.Dock = DockStyle.Fill;
                        dersPanel.BackColor = Color.White;
                        dersPanel.Padding = new Padding(200, 10, 10, 10);

                        string filePath = "data.json";
                        List<GunlukDers> kayitlar = new List<GunlukDers>();

                        if (File.Exists(filePath))
                        {
                            string json = File.ReadAllText(filePath);
                            kayitlar = JsonSerializer.Deserialize<List<GunlukDers>>(json);
                        }

                        ListView listView = new ListView();
                        listView.View = View.Details;
                        listView.FullRowSelect = true;
                        listView.GridLines = true;
                        listView.Dock = DockStyle.Fill;
                        listView.MultiSelect = false;

                        // Sütunlar ekleniyor
                        listView.Columns.Add("Tarih", 175);
                        listView.Columns.Add("Ruh Hali", 100);
                        listView.Columns.Add("Hedef", 250);
                        listView.Columns.Add("Çalışma Süresi", 120);
                        listView.Columns.Add("Hedef Durumu", 100);

                        foreach (var ders in kayitlar)
                        {
                            ListViewItem item = new ListViewItem(ders.Tarih);
                            item.SubItems.Add(ders.RuhHali);
                            item.SubItems.Add(ders.Hedef);
                            item.SubItems.Add(ders.CalismaSuresi);

                            // Hedef durumu (✓ veya ✗ ile)
                            string durum = ders.HedefDurumu == "Başarılı" ? "✓ Başarılı" : "✗ Başarısız";
                            item.SubItems.Add(durum);

                            // Duruma göre renk ayarla
                            if (ders.HedefDurumu == "Başarılı")
                                item.ForeColor = Color.Green;
                            else
                                item.ForeColor = Color.Red;

                            listView.Items.Add(item);
                        }

                        // ListView item tıklama olayı
                        listView.MouseClick += (s, e) =>
                        {
                            var hitTest = listView.HitTest(e.Location);
                            if (hitTest.Item != null && hitTest.SubItem != null)
                            {
                                // Sadece "Hedef Durumu" sütununa tıklandığında
                                if (hitTest.Item.SubItems.IndexOf(hitTest.SubItem) == 4)
                                {
                                    int selectedIndex = hitTest.Item.Index;
                                    var selectedDers = kayitlar[selectedIndex];

                                    if (selectedDers.HedefDurumu == "Başarılı")
                                    {
                                        selectedDers.HedefDurumu = "Başarısız";
                                        hitTest.Item.SubItems[4].Text = "✗ Başarısız";
                                        hitTest.Item.ForeColor = Color.Red;
                                    }
                                    else
                                    {
                                        selectedDers.HedefDurumu = "Başarılı";
                                        hitTest.Item.SubItems[4].Text = "✓ Başarılı";
                                        hitTest.Item.ForeColor = Color.Green;
                                    }

                                    // JSON dosyasını güncelle
                                    File.WriteAllText(filePath, JsonSerializer.Serialize(kayitlar, new JsonSerializerOptions { WriteIndented = true }));
                                }
                            }
                        };

                        // Sil butonu
                        Button deleteButton = new Button();
                        deleteButton.Text = "Seçili Kaydı Sil";
                        deleteButton.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                        deleteButton.BackColor = Color.FromArgb(192, 0, 0);
                        deleteButton.ForeColor = Color.White;
                        deleteButton.FlatStyle = FlatStyle.Flat;
                        deleteButton.Height = 35;
                        deleteButton.Width = 150;
                        deleteButton.Dock = DockStyle.Bottom;
                        deleteButton.Margin = new Padding(0, 10, 0, 0);

                        deleteButton.Click += (s, e) =>
                        {
                            if (listView.SelectedItems.Count == 0)
                            {
                                MessageBox.Show("Lütfen silmek istediğiniz kaydı seçiniz.", "Uyarı",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            var result = MessageBox.Show("Seçili kaydı silmek istediğinize emin misiniz?", "Onay",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                            if (result == DialogResult.Yes)
                            {
                                int selectedIndex = listView.SelectedIndices[0];
                                kayitlar.RemoveAt(selectedIndex);
                                listView.Items.RemoveAt(selectedIndex);

                                File.WriteAllText(filePath, JsonSerializer.Serialize(kayitlar, new JsonSerializerOptions { WriteIndented = true }));
                                MessageBox.Show("Kayıt başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        };

                        Panel buttonPanel = new Panel();
                        buttonPanel.Dock = DockStyle.Bottom;
                        buttonPanel.Height = 50;
                        buttonPanel.Controls.Add(deleteButton);

                        dersPanel.Controls.Add(listView);
                        dersPanel.Controls.Add(buttonPanel);
                        contentPanel.Controls.Add(dersPanel);
                    }
                    break;
                    
                case "Profil":
                    {
                        Panel profilePanel = new Panel();
                        profilePanel.Dock = DockStyle.Fill;
                        profilePanel.BackColor = Color.White;
                        profilePanel.Padding = new Padding(200, 30, 30, 30);

                        GroupBox profileGroup = new GroupBox();
                        profileGroup.Text = "Öğrenci Profil Bilgileri";
                        profileGroup.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                        profileGroup.Dock = DockStyle.Top;
                        profileGroup.Height = 350;
                        profileGroup.Padding = new Padding(20);

                        // Profil bilgilerini saklamak için dosya yolu
                        string profileFilePath = "profile.json";

                        // Kontroller oluşturuluyor
                        TableLayoutPanel tableLayout = new TableLayoutPanel();
                        tableLayout.Dock = DockStyle.Fill;
                        tableLayout.ColumnCount = 2;
                        tableLayout.RowCount = 6;
                        tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
                        tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
                        tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
                        tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
                        tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
                        tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
                        tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
                        tableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));

                        // Ad
                        Label lblName = new Label();
                        lblName.Text = "Ad:";
                        lblName.TextAlign = ContentAlignment.MiddleRight;
                        lblName.Dock = DockStyle.Fill;
                        TextBox txtName = new TextBox();
                        txtName.Dock = DockStyle.Fill;
                        txtName.Margin = new Padding(0, 5, 0, 5);

                        // Soyad
                        Label lblSurname = new Label();
                        lblSurname.Text = "Soyad:";
                        lblSurname.TextAlign = ContentAlignment.MiddleRight;
                        lblSurname.Dock = DockStyle.Fill;
                        TextBox txtSurname = new TextBox();
                        txtSurname.Dock = DockStyle.Fill;
                        txtSurname.Margin = new Padding(0, 5, 0, 5);

                        // Yaş
                        Label lblAge = new Label();
                        lblAge.Text = "Yaş:";
                        lblAge.TextAlign = ContentAlignment.MiddleRight;
                        lblAge.Dock = DockStyle.Fill;
                        TextBox txtAge = new TextBox();
                        txtAge.Dock = DockStyle.Fill;
                        txtAge.Margin = new Padding(0, 5, 0, 5);

                        // Okul
                        Label lblSchool = new Label();
                        lblSchool.Text = "Okul:";
                        lblSchool.TextAlign = ContentAlignment.MiddleRight;
                        lblSchool.Dock = DockStyle.Fill;
                        TextBox txtSchool = new TextBox();
                        txtSchool.Dock = DockStyle.Fill;
                        txtSchool.Margin = new Padding(0, 5, 0, 5);

                        // Bölüm
                        Label lblDepartment = new Label();
                        lblDepartment.Text = "Bölüm:";
                        lblDepartment.TextAlign = ContentAlignment.MiddleRight;
                        lblDepartment.Dock = DockStyle.Fill;
                        TextBox txtDepartment = new TextBox();
                        txtDepartment.Dock = DockStyle.Fill;
                        txtDepartment.Margin = new Padding(0, 5, 0, 5);

                        // Kaydet butonu
                        Button btnSave = new Button();
                        btnSave.Text = "Bilgileri Kaydet";
                        btnSave.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                        btnSave.BackColor = Color.FromArgb(0, 120, 215);
                        btnSave.ForeColor = Color.White;
                        btnSave.FlatStyle = FlatStyle.Flat;
                        btnSave.Height = 35;
                        btnSave.Dock = DockStyle.Fill;
                        btnSave.Margin = new Padding(100, 10, 100, 0);

                        // Kontroller tabloya ekleniyor
                        tableLayout.Controls.Add(lblName, 0, 0);
                        tableLayout.Controls.Add(txtName, 1, 0);
                        tableLayout.Controls.Add(lblSurname, 0, 1);
                        tableLayout.Controls.Add(txtSurname, 1, 1);
                        tableLayout.Controls.Add(lblAge, 0, 2);
                        tableLayout.Controls.Add(txtAge, 1, 2);
                        tableLayout.Controls.Add(lblSchool, 0, 3);
                        tableLayout.Controls.Add(txtSchool, 1, 3);
                        tableLayout.Controls.Add(lblDepartment, 0, 4);
                        tableLayout.Controls.Add(txtDepartment, 1, 4);
                        tableLayout.Controls.Add(btnSave, 0, 5);
                        tableLayout.SetColumnSpan(btnSave, 2);

                        // Daha önce kaydedilmiş profili yükle
                        if (File.Exists(profileFilePath))
                        {
                            string json = File.ReadAllText(profileFilePath);
                            var profile = JsonSerializer.Deserialize<StudentProfile>(json);

                            txtName.Text = profile.Name;
                            txtSurname.Text = profile.Surname;
                            txtAge.Text = profile.Age.ToString();
                            txtSchool.Text = profile.School;
                            txtDepartment.Text = profile.Department;
                        }

                        // Kaydet butonu işlevi
                        btnSave.Click += (s, e) =>
                        {
                            if (int.TryParse(txtAge.Text, out int age))
                            {
                                var profile = new StudentProfile
                                {
                                    Name = txtName.Text,
                                    Surname = txtSurname.Text,
                                    Age = age,
                                    School = txtSchool.Text,
                                    Department = txtDepartment.Text
                                };

                                File.WriteAllText(profileFilePath, JsonSerializer.Serialize(profile, new JsonSerializerOptions { WriteIndented = true }));
                                MessageBox.Show("Profil bilgileri kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Lütfen geçerli bir yaş giriniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        };

                        profileGroup.Controls.Add(tableLayout);
                        profilePanel.Controls.Add(profileGroup);
                        contentPanel.Controls.Add(profilePanel);
                    }
                    break;
            }
        }

        // Öğrenci profili için yeni sınıf
        public class StudentProfile
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public int Age { get; set; }
            public string School { get; set; }
            public string Department { get; set; }
        }
            }
        }

        public class GunlukDers
        {
            public string Tarih { get; set; }
            public string RuhHali { get; set; }
            public string Hedef { get; set; }
            public string CalismaSuresi { get; set; }
            public string HedefDurumu { get; set; } // Yeni property
        }


