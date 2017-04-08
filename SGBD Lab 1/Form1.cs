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
using System.Configuration;
using System.Collections.Specialized;

namespace SGBD_Lab_1
{
    public partial class Form1 : Form
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString);
        SqlDataAdapter dataAdapDist;
        SqlDataAdapter dataAdaptFact;
        DataSet ds;
        BindingSource bsd;
        BindingSource bsf;
        DataRelation dr;
        public Form1()
        {
            InitializeComponent();
            dataGridView3.ReadOnly = true;
            dataGridView4.ReadOnly = true;


            conn.Open();
            dataAdapDist = new SqlDataAdapter(ConfigurationSettings.AppSettings["select_dist"], conn);
            dataAdaptFact = new SqlDataAdapter(ConfigurationSettings.AppSettings["select_fact"], conn);
            Console.WriteLine(conn.State);
            Console.WriteLine(conn.ConnectionTimeout);
            Console.WriteLine("asdasdasdasdasd");
            ds = new DataSet();
            bsd = new BindingSource();
            bsf = new BindingSource();
            dataAdapDist.Fill(ds, "Distribuitori");
            dataAdaptFact.Fill(ds, "Factura");


            //SqlCommandBuilder ComBuildDist = new SqlCommandBuilder(dataAdapDist);

            dr = new DataRelation("FK_DISTRIBUITOR_FACTURA", ds.Tables["Distribuitori"].Columns["Id"], ds.Tables["Factura"].Columns["IdDistribuitor"]);
            ds.Relations.Add(dr);
            bsd.DataSource = ds;
            bsd.DataMember = "Distribuitori";

            bsf.DataSource = bsd;
            bsf.DataMember = "FK_DISTRIBUITOR_FACTURA";
            dataGridView4.DataSource = bsf;
            dataGridView3.DataSource = bsd;
            Console.WriteLine(conn.State);
            conn.Close();


        }

        private void Form1_Load(object sender, EventArgs e)
        {


        }

        private void refresh_data()
        {
            try
            {

                ds.Clear();
                dataAdapDist.Fill(ds, "Distribuitori");
                dataAdaptFact.Fill(ds, "Factura");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                List<String> Column_names = new List<string>(ConfigurationManager.AppSettings["ChildColumnNamesJustId"].Split(','));
                SqlCommand cmd = new SqlCommand(ConfigurationManager.AppSettings["DeleteQuery"], conn);

                foreach (string column in Column_names)
                {
                    TextBox textBox = (TextBox)this.Controls["text" + column];
                    cmd.Parameters.AddWithValue("@" + column, textBox.Text);
                }

                conn.Open();
                if (cmd.ExecuteNonQuery() != 0)
                    MessageBox.Show("Delete Succesfull !");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                refresh_data();
            }
        }



        private void dataGridView4_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            textPret.Text = dataGridView4.Rows[e.RowIndex].Cells[1].Value.ToString();
            textData.Text = dataGridView4.Rows[e.RowIndex].Cells[2].Value.ToString();
            textIdPiesa.Text = dataGridView4.Rows[e.RowIndex].Cells[3].Value.ToString();
            textId.Text = dataGridView4.Rows[e.RowIndex].Cells[0].Value.ToString();
            textIdDIstribuitor.Text = dataGridView4.Rows[e.RowIndex].Cells[4].Value.ToString();

        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {

            if (textPret.Text != "" && textData.Text != "" && textIdPiesa.Text != "")
            {
                try
                {
                    List<String> Column_names = new List<string>(ConfigurationManager.AppSettings["ChildColumnNamesId"].Split(','));
                    SqlCommand cmd = new SqlCommand(ConfigurationManager.AppSettings["InsertQuery"], conn);

                    foreach (string column in Column_names)
                    {
                        TextBox textBox = (TextBox)this.Controls["text" + column];
                        cmd.Parameters.AddWithValue("@" + column, textBox.Text);
                    }

                    conn.Open();
                    if (cmd.ExecuteNonQuery() != 0)
                        MessageBox.Show("Insert Succesfull !");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    refresh_data();
                }
            }
        }

        private void ButtonUpdate_Click(object sender, EventArgs e)
        {
            if (textBoxAdresa.Text != "" && textBoxLocalitatea.Text != "" && textBoxTimpLivrare.Text != "")
            {
                try
                {
                    List<String> Column_names = new List<string>(ConfigurationManager.AppSettings["ChildColumnNamesId"].Split(','));
                    SqlCommand cmd = new SqlCommand(ConfigurationManager.AppSettings["UpdateQuery"], conn);
                    foreach (string column in Column_names)
                    {
                        TextBox textBox = (TextBox)this.Controls["text" + column];
                        cmd.Parameters.AddWithValue("@" + column, textBox.Text);
                    }



                    conn.Open();
                    if (cmd.ExecuteNonQuery() != 0)
                        MessageBox.Show("Update Succesfull !");
                }

                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    refresh_data();
                }

            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (textBoxAdresa.Text != "" && textBoxLocalitatea.Text != "" && textBoxTimpLivrare.Text != "")
            {
                try
                {
                    List<String> Column_names = new List<string>(ConfigurationManager.AppSettings["ParentColumnNamesId"].Split(','));
                    SqlCommand cmd = new SqlCommand(ConfigurationManager.AppSettings["ParentInsertQuery"], conn);

                    foreach (string column in Column_names)
                    {
                        TextBox textBox = (TextBox)this.Controls["textBox" + column];
                        cmd.Parameters.AddWithValue("@" + column, textBox.Text);
                    }

                    conn.Open();
                    if (cmd.ExecuteNonQuery() != 0)
                        MessageBox.Show("Insert Succesfull !");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    refresh_data();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBoxId.Text != "")
            {
                try
                {
                    List<String> Column_names = new List<string>(ConfigurationManager.AppSettings["ParentColumnNamesJustId"].Split(','));
                    SqlCommand cmd = new SqlCommand(ConfigurationManager.AppSettings["ParentDeleteQuery"], conn);

                    foreach (string column in Column_names)
                    {
                        TextBox textBox = (TextBox)this.Controls["textBox" + column];
                        cmd.Parameters.AddWithValue("@" + column, textBox.Text);
                    }

                    conn.Open();
                    if (cmd.ExecuteNonQuery() != 0)
                        MessageBox.Show("Delete Succesfull !");

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    refresh_data();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBoxAdresa.Text != "" && textBoxLocalitatea.Text != "" && textBoxTimpLivrare.Text != "")
            {
                try
                {
                    List<String> Column_names = new List<string>(ConfigurationManager.AppSettings["ParentColumnNamesId"].Split(','));
                    SqlCommand cmd = new SqlCommand(ConfigurationManager.AppSettings["ParentUpdateQuery"], conn);
                    foreach (string column in Column_names)
                    {
                        TextBox textBox = (TextBox)this.Controls["textBox" + column];
                        cmd.Parameters.AddWithValue("@" + column, textBox.Text);
                    }



                    conn.Open();
                    if (cmd.ExecuteNonQuery() != 0)
                        MessageBox.Show("Update Succesfull !");
                }

                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    refresh_data();
                }

            }
        }

        private void dataGridView3_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            textBoxId.Text = dataGridView3.Rows[e.RowIndex].Cells[0].Value.ToString();
            textBoxAdresa.Text = dataGridView3.Rows[e.RowIndex].Cells[1].Value.ToString();
            textBoxLocalitatea.Text = dataGridView3.Rows[e.RowIndex].Cells[2].Value.ToString();
            textBoxTimpLivrare.Text = dataGridView3.Rows[e.RowIndex].Cells[3].Value.ToString();
        }
    }
}
