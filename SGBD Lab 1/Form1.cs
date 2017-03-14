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

namespace SGBD_Lab_1
{
    public partial class Form1 : Form
    {
        SqlConnection conn = new SqlConnection("Data Source = BOGDAN-PC\\SQLEXPRESS; Initial Catalog = Vending company ; Integrated Security = True");
        SqlDataAdapter dataAdapDist;
        SqlDataAdapter dataAdaptFact;
        DataSet ds;
        BindingSource bsd;
        BindingSource bsf;
        public Form1()
        {
            InitializeComponent();
            dataGridView3.ReadOnly = true;
            dataGridView4.ReadOnly = true;
            
            
                conn.Open();
                dataAdapDist = new SqlDataAdapter("select * from distribuitori", conn);
                dataAdaptFact = new SqlDataAdapter("select * from factura", conn);
                Console.WriteLine(conn.State);
                Console.WriteLine(conn.ConnectionTimeout);
                Console.WriteLine("asdasdasdasdasd");
                ds = new DataSet();
                bsd = new BindingSource();
                bsf = new BindingSource();
                dataAdapDist.Fill(ds, "Distribuitori");
                dataAdaptFact.Fill(ds, "Factura");


                SqlCommandBuilder ComBuildDist = new SqlCommandBuilder(dataAdapDist);

                DataRelation dr = new DataRelation("FK_DISTRIBUITOR_FACTURA", ds.Tables["Distribuitori"].Columns["Id"], ds.Tables["Factura"].Columns["IdDistribuitor"]);
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

            dataAdaptFact.Update(ds);
        }
       

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                dataAdaptFact.DeleteCommand = new SqlCommand("delete from Factura where id =@id ", conn);
        
                dataAdaptFact.DeleteCommand.Parameters.Add("@id", SqlDbType.Int).Value = textId.Text;
                conn.Open();

                if (dataAdaptFact.DeleteCommand.ExecuteNonQuery() >= 1)
                    MessageBox.Show("Delete Succesfull !");

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


                    dataAdaptFact.InsertCommand = new SqlCommand("insert into Factura (Pret,Data,idPiesa,idDistribuitor) Values( @pret, @data, @idPiesa, @idDist) ", conn);
                    dataAdaptFact.InsertCommand.Parameters.Add("@pret", SqlDbType.Real).Value = textPret.Text;
                    dataAdaptFact.InsertCommand.Parameters.Add("@data", SqlDbType.Date).Value = textData.Text;
                    dataAdaptFact.InsertCommand.Parameters.Add("@idPiesa", SqlDbType.Int).Value = textIdPiesa.Text;
                    dataAdaptFact.InsertCommand.Parameters.Add("@idDist", SqlDbType.Int).Value = textIdDIstribuitor.Text;
                    conn.Open();
                    dataAdaptFact.InsertCommand.ExecuteNonQuery();
                    MessageBox.Show("Insert Succesfull !");
    
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

         

        }

      

        private void ButtonUpdate_Click(object sender, EventArgs e)
        {
            if (textPret.Text != "" && textData.Text != "" && textIdPiesa.Text != "")
            {
                try
                {
                    dataAdaptFact.UpdateCommand = new SqlCommand("update Factura set Pret =@pret ,Data = @data, idPiesa =@idPiesa where id =@id ", conn);
                    dataAdaptFact.UpdateCommand.Parameters.Add("@pret", SqlDbType.Real).Value = textPret.Text;
                    dataAdaptFact.UpdateCommand.Parameters.Add("@data", SqlDbType.Date).Value = textData.Text;
                    dataAdaptFact.UpdateCommand.Parameters.Add("@idPiesa", SqlDbType.Int).Value = textIdPiesa.Text;
                    dataAdaptFact.UpdateCommand.Parameters.Add("@id", SqlDbType.Int).Value = textId.Text;
                    conn.Open();

                    if (dataAdaptFact.UpdateCommand.ExecuteNonQuery() >= 1)
                    {
                        MessageBox.Show("Update Succesfull !");
                        refresh_data();
                    }
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
        }
    }
}
