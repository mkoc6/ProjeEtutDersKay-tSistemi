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
using System.IO;

namespace _15.ProjeEtutDersKayıtSistemi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-1DQCP20\SQLEXPRESS;Initial Catalog=15.ProjeEtut&DersKayıtSistemi;Integrated Security=True");

        void derslistesi()
        {
            SqlDataAdapter Da = new SqlDataAdapter("select * from TBLDERSLER", baglanti);
            DataTable dt = new DataTable();
            Da.Fill(dt);
            cmbDers.DataSource = dt;

            cmbDers.ValueMember = "DERSID";

            cmbDers.DisplayMember = "DERSAD";
            cmbogretmenders.DataSource = dt;
            cmbogretmenders.DisplayMember = "DERSAD";
            cmbogretmenders.ValueMember = "DERSID";
        }
        //Etütü Listesi
        void etutListesi()
        {
            SqlDataAdapter da3 = new SqlDataAdapter("execute Etut", baglanti);
            DataTable dt3 = new DataTable();
            da3.Fill(dt3);
            bunifuDataGridView1.DataSource = dt3;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbDers.SelectedIndexChanged -= cmbDers_SelectedIndexChanged;
            derslistesi();
            cmbDers.SelectedIndexChanged += cmbDers_SelectedIndexChanged;
            etutListesi();

        }

        private void cmbDers_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            SqlDataAdapter em = new SqlDataAdapter("select * from TBLOGRETMEN WHERE BRANSID =" + cmbDers.SelectedValue, baglanti);
            DataTable SC = new DataTable();
            em.Fill(SC);
            cmbOgretmen.DataSource = SC;
            cmbOgretmen.DisplayMember = "AD";
            cmbOgretmen.ValueMember = "OGRTID";


        }

        private void BtnOlustur_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("insert into TBLETUT (DERSID, OGRETMENID,TARIH, SAAT) VALUES (@P1,@P2,@P3,@P4)", baglanti);
            komut.Parameters.AddWithValue("@P1", cmbDers.SelectedValue);
            komut.Parameters.AddWithValue("@P2", cmbOgretmen.SelectedValue);
            komut.Parameters.AddWithValue("@P3", mskTarih.Text);
            komut.Parameters.AddWithValue("@P4", mskSaat.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Study Added");
        }

        private void bunifuDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int SECİLEN = bunifuDataGridView1.SelectedCells[0].RowIndex;
            txtEtutID.Text = bunifuDataGridView1.Rows[SECİLEN].Cells[0].Value.ToString();
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut5 = new SqlCommand("update TBLETUT SET OGRENCIID=@P1, DURUM=@P2 WHERE ID = "+txtEtutID.Text, baglanti);
            komut5.Parameters.AddWithValue("@P1", textBox2.Text);
            komut5.Parameters.AddWithValue("@P2", "True");
            komut5.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Work Assigned");
            etutListesi();
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void bunifuThinButton23_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            bunifuPictureBox1.ImageLocation = openFileDialog1.FileName;
        }

        private void btnOgrenciEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand KOMUT1 = new SqlCommand("insert into TBLOGRENCI (AD, SOYAD,FOTOGRAF, SINIF,TELEFON,MAIL) VALUES (@P1,@P2,@P3,@P4,@P5,@P6)", baglanti);
            KOMUT1.Parameters.AddWithValue("@P1",TxtAd.Text);
            KOMUT1.Parameters.AddWithValue("@P2", txtSoyad.Text);
            KOMUT1.Parameters.AddWithValue("@P3", bunifuPictureBox1.ImageLocation);
            KOMUT1.Parameters.AddWithValue("@P4", txtSınıf.Text);
            KOMUT1.Parameters.AddWithValue("@P5", mskTelefon.Text);
            KOMUT1.Parameters.AddWithValue("@P6", txtMail.Text);
            KOMUT1.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Student Added");
        }

        private void BtnOgretmenEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand KOMUT8 = new SqlCommand("insert into TBLOGRETMEN (AD, SOYAD, BRANSID) VALUES (@P1,@P2,@P3)", baglanti);
            KOMUT8.Parameters.AddWithValue("@P1",txtOgretmenAd.Text);
            KOMUT8.Parameters.AddWithValue("@P2", TxtOgretmenSoyad.Text);
            KOMUT8.Parameters.AddWithValue("@P3", cmbogretmenders.SelectedValue);
            KOMUT8.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Teacher Added");
        }
    }
}
