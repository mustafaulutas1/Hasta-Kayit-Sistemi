using Npgsql;
using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing; 
using System.Linq; 

namespace WindowsFormsApp14
{
    public partial class Form1 : Form
    {
        private NpgsqlConnection baglanti;
        public Form1()
        {
            InitializeComponent();
            Baglan(); 
            HastaBilgileriniGoster(); // Hastaları göster
        }
        private void CustomizeUI()
        {
            
            this.BackColor = Color.LightGray;

            
            var buttons = this.Controls.OfType<Button>();
            foreach (var button in buttons)
            {
                button.BackColor = Color.Teal;
                button.ForeColor = Color.White;
            }

            dataGridView1.BackgroundColor = Color.White;

            this.Text = "Hasta Kayıt Uygulaması";
            
        }
        private void Baglan()
        {
            baglanti = new NpgsqlConnection("server=localhost; port=5432; database=hastane;User Id=postgres;password=123456;");
            try
            {
                baglanti.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bağlantı hatası: " + ex.Message);
            }
        }
        // Hastaları gösterme 
        private void HastaBilgileriniGoster()
        {
            using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter("SELECT * FROM hasta", baglanti))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }
        // Hasta ekle
        private void HastaEkle(string ad, string soyad, DateTime dogumTarihi, string dogumYeri, string tcKimlik, string telefon, string hastalik, string ilac, string adres)
        {
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO hasta (ad, soyad, dogum_tarihi, dogum_yeri, tc_kimlik, telefon, hastalik, ilac, adres) VALUES (@ad, @soyad, @dogum_tarihi, @dogum_yeri, @tc_kimlik, @telefon, @hastalik, @ilac, @adres)", baglanti))
                {
                    cmd.Parameters.AddWithValue("@ad", ad);
                    cmd.Parameters.AddWithValue("@soyad", soyad);
                    cmd.Parameters.AddWithValue("@dogum_tarihi", dogumTarihi);
                    cmd.Parameters.AddWithValue("@dogum_yeri", dogumYeri);
                    cmd.Parameters.AddWithValue("@tc_kimlik", tcKimlik);
                    cmd.Parameters.AddWithValue("@telefon", telefon);
                    cmd.Parameters.AddWithValue("@hastalik", hastalik);
                    cmd.Parameters.AddWithValue("@ilac", ilac);
                    cmd.Parameters.AddWithValue("@adres", adres);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Hasta kaydı başarıyla eklendi.");
            }
            catch (Exception ex)//Hatalı ekleme
            {
                MessageBox.Show("Hasta kaydı eklenemedi HATA!!!: " + ex.Message);
            }
            finally
            {
                HastaBilgileriniGoster(); // Hasta güncelle
            }
        }

        // Hasta sil
        private void HastaSil(string tc_kimlik)
        {
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM hasta WHERE tc_kimlik = @tc_kimlik", baglanti))
                {
                    cmd.Parameters.AddWithValue("@tc_kimlik", tc_kimlik);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Hasta kaydı silindi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hasta kaydı silinemedi HATA!!!: " + ex.Message);
            }
            finally
            {
                HastaBilgileriniGoster(); 
            }
        }


        // Hasta güncelle
        private void HastaGuncelle(int hastaID, string ad, string soyad, DateTime dogumTarihi, string dogumYeri, string tcKimlik, string telefon, string hastalik, string ilac, string adres)
        {
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE hasta SET ad = @ad, soyad = @soyad, dogum_tarihi = @dogum_tarihi, dogum_yeri = @dogum_yeri, telefon = @telefon, hastalik = @hastalik, ilac = @ilac, adres = @adres WHERE tc_kimlik= @tc_kimlik", baglanti))
                {

                    cmd.Parameters.AddWithValue("@ad", ad);
                    cmd.Parameters.AddWithValue("@soyad", soyad);
                    cmd.Parameters.AddWithValue("@dogum_tarihi", dogumTarihi);
                    cmd.Parameters.AddWithValue("@dogum_yeri", dogumYeri);
                    cmd.Parameters.AddWithValue("@tc_kimlik", tcKimlik);
                    cmd.Parameters.AddWithValue("@telefon", telefon);
                    cmd.Parameters.AddWithValue("@hastalik", hastalik);
                    cmd.Parameters.AddWithValue("@ilac", ilac);
                    cmd.Parameters.AddWithValue("@adres", adres);

                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Hasta kaydı güncellendi.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hasta kaydı güncellenemedi HATA!!!: " + ex.Message);
            }
            finally
            {
                HastaBilgileriniGoster(); 
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string ad = textBox1.Text;
            string soyad = textBox2.Text;
            DateTime dogumTarihi = dateTimePicker1.Value;
            string dogumYeri = textBox5.Text;
            string tc_kimlik = textBox3.Text;
            string telefon = textBox6.Text;
            string hastalik = textBox7.Text;
            string ilac = textBox8.Text;
            string adres = textBox4.Text;

            HastaEkle(ad, soyad, dogumTarihi, dogumYeri, tc_kimlik, telefon, hastalik, ilac, adres);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                if (dataGridView1.SelectedCells.Count > 0)
                {
                    int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
                    string tc_kimlik = dataGridView1.Rows[selectedRowIndex].Cells["tc_kimlik"].Value.ToString();

                    HastaSil(tc_kimlik);
                }
                else
                {
                    MessageBox.Show("Hasta.");
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            
            int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
            int hastaID = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells["tc_kimlik"].Value);

            // Güncellenecek hasta bilgilerini
            string ad = textBox1.Text;
            string soyad = textBox2.Text;
            DateTime dogumTarihi = dateTimePicker1.Value;
            string dogumYeri = textBox5.Text;
            string tc_kimlik = textBox3.Text;
            string telefon = textBox6.Text;
            string hastalik = textBox7.Text;
            string ilac = textBox8.Text;
            string adres = textBox4.Text;

            HastaGuncelle(hastaID, ad, soyad, dogumTarihi, dogumYeri, tc_kimlik, telefon, hastalik, ilac, adres);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            DataGridViewRow selectedRow = dataGridView1.Rows[rowIndex];

            string ad = selectedRow.Cells["ad"].Value.ToString();
            string soyad = selectedRow.Cells["soyad"].Value.ToString();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string yeniAd = textBox1.Text;

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string yeniSoyad = textBox2.Text;
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string tc_kimlik = textBox3.Text;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string Adres = textBox4.Text;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            string Dogum_Yeri = textBox5.Text;
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            string hastalik = textBox7.Text;
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            string ilac = textBox8.Text;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime seciliTarih = dateTimePicker1.Value;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            string yeniTelefon = textBox6.Text;
        }
        

        private void button1_Click_1(object sender, EventArgs e)
        {
            string ad = textBox1.Text;
            string soyad = textBox2.Text;
            string Adres = textBox4.Text;
            DateTime dogumTarihi = dateTimePicker1.Value;
            string dogumYeri = textBox5.Text;
            string tc_kimlik = textBox3.Text;
            string telefon = textBox6.Text;
            string hastalik = textBox7.Text;
            string ilac = textBox8.Text;

            HastaEkle(ad, soyad, dogumTarihi, dogumYeri, tc_kimlik, telefon, hastalik, ilac, Adres);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
 
            MessageBox.Show("Hasta Kayıt Sistemine Hoş Geldiniz!");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Tc no bilgisi alma
            string tcKimlikNo = textBox9.Text;

            // Tc kimlik no su girilen hastanın bilgilerini textBox lara ekleme
            using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter("SELECT * FROM hasta WHERE tc_kimlik = @tc_kimlik", baglanti))
            {
                adapter.SelectCommand.Parameters.AddWithValue("@tc_kimlik", tcKimlikNo);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    
                    DataRow row = dt.Rows[0];
                    textBox1.Text = row["ad"].ToString();
                    textBox2.Text = row["soyad"].ToString();
                    dateTimePicker1.Value = Convert.ToDateTime(row["dogum_tarihi"]);
                    textBox5.Text = row["dogum_yeri"].ToString();
                    textBox3.Text = row["tc_kimlik"].ToString();
                    textBox6.Text = row["telefon"].ToString();
                    textBox7.Text = row["hastalik"].ToString();
                    textBox8.Text = row["ilac"].ToString();
                    textBox4.Text = row["adres"].ToString();
                }
                else
                {
                    MessageBox.Show("Hasta bulunamadı.");
                }
            }
        }

        
    }
}

