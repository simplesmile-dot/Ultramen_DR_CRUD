using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CRUDMahasiswaADO
{
    public partial class Form1 : Form
    {
        private SqlConnection conn;
        private readonly string connectionString =
            "Data Source=DESKTOP-GU8JFSR\\ZIDANE; Initial Catalog=DBAkademikADO; Integrated Security=True"; 
        public Form1()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString); 
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtNIM.Text = row.Cells["NIM"].Value.ToString();
                txtNama.Text = row.Cells["Nama"].Value.ToString();
                cmbJK.SelectedItem = row.Cells["JenisKelamin"].Value.ToString();
                txtAlamat.Text = row.Cells["Alamat"].Value.ToString();
                txtKodeProdi.Text = row.Cells["KodeProdi"].Value.ToString();
                dtpTanggalLahir.Value = Convert.ToDateTime(row.Cells["TanggalLahir"].Value);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            cmbJK.Items.Clear();
            cmbJK.Items.Add("L");
            cmbJK.Items.Add("P");

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView1.CellClick += dataGridView1_CellClick;
        }
       
        private void ConnectDatabase()
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                MessageBox.Show("Koneksi berhasil");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Koneksi gagal: " + ex.Message);
            }
        }
        

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                string query = "SELECT * FROM Mahasiswa";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            string nim = txtNIM.Text;
            string nama = txtNama.Text;
            string jenisKelamin = cmbJK.SelectedItem?.ToString();
            string alamat = txtAlamat.Text;
            string kodeProdi = txtKodeProdi.Text;
            DateTime tanggalLahir = dtpTanggalLahir.Value;
            if (string.IsNullOrEmpty(nim) || string.IsNullOrEmpty(nama) || string.IsNullOrEmpty(jenisKelamin) ||
                string.IsNullOrEmpty(alamat) || string.IsNullOrEmpty(kodeProdi))
            {
                MessageBox.Show("Semua field harus diisi.");
                return;
            }
            try
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Mahasiswa (NIM, Nama, JenisKelamin, Alamat, KodeProdi, TanggalLahir) VALUES (@NIM, @Nama, @JenisKelamin, @Alamat, @KodeProdi, @TanggalLahir)", conn))
                {
                    cmd.Parameters.AddWithValue("@NIM", nim);
                    cmd.Parameters.AddWithValue("@Nama", nama);
                    cmd.Parameters.AddWithValue("@JenisKelamin", jenisKelamin);
                    cmd.Parameters.AddWithValue("@Alamat", alamat);
                    cmd.Parameters.AddWithValue("@KodeProdi", kodeProdi);
                    cmd.Parameters.AddWithValue("@TanggalLahir", tanggalLahir);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Data berhasil disimpan.");
                        ClearForm();
                    }
                    else
                    {
                        MessageBox.Show("Gagal menyimpan data.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }

                string query = @"UPDATE Mahasiswa 
                                 SET Nama = @Nama, 
                                     JenisKelamin = @JK, 
                                     TanggalLahir = @TanggalLahir, 
                                     Alamat = @Alamat, 
                                     KodeProdi = @KodeProdi 
                                 WHERE NIM = @NIM";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@NIM", txtNIM.Text);
                cmd.Parameters.AddWithValue("@Nama", txtNama.Text);
                cmd.Parameters.AddWithValue("@JK", cmbJK.Text);
                cmd.Parameters.AddWithValue("@TanggalLahir", dtpTanggalLahir.Value.Date);
                cmd.Parameters.AddWithValue("@Alamat", txtAlamat.Text);
                cmd.Parameters.AddWithValue("@KodeProdi", txtKodeProdi.Text);

                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Data berhasil diupdate.");
                    ClearForm();
                    btnLoad.PerformClick();
                }
                else
                {
                    MessageBox.Show("Data tidak ditemukan.");
                }
            }

            catch (Exception ex)
            {
                {
                    MessageBox.Show("Terjadi Kesalahan: " + ex.Message);
                }
            }
        }
            
        
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                {
                    conn.Open();
                }
                string query = "DELETE FROM Mahasiswa WHERE NIM = @NIM";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@NIM", txtNIM.Text);
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("Data berhasil dihapus.");
                    ClearForm();
                    btnLoad.PerformClick();
                }
                else
                {
                    MessageBox.Show("Data tidak ditemukan.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi Kesalahan: " + ex.Message);
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {
            
        }
        private void ClearForm()
        {
            txtNIM.Clear();
            txtNama.Clear();
            cmbJK.SelectedIndex = -1;
            txtAlamat.Clear();
            txtKodeProdi.Clear();
            dtpTanggalLahir.Value = DateTime.Now;
            txtNIM.Focus();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            ConnectDatabase();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                txtNIM.Text = row.Cells["NIM"].Value.ToString();
                txtNama.Text = row.Cells["Nama"].Value.ToString();
                cmbJK.SelectedItem = row.Cells["JenisKelamin"].Value.ToString();
                txtAlamat.Text = row.Cells["Alamat"].Value.ToString();
                txtKodeProdi.Text = row.Cells["KodeProdi"].Value.ToString();
                dtpTanggalLahir.Value = Convert.ToDateTime(row.Cells["TanggalLahir"].Value);
            }

        }
    }
}
