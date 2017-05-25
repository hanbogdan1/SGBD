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
using System.Threading;

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
        int childNumberOfColumns;
        private TextBox[] textBoxes;
        private Label[] labels;
        List<String> Column_names;

        public Form1()
        {
            Column_names = new List<string>(ConfigurationManager.AppSettings["ChildColumnNamesId"].Split(','));
            childNumberOfColumns = Column_names.Count;
            InitializeComponent();
            GenerateBoxes();
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


        private void GenerateBoxes()
        {

            textBoxes = new TextBox[childNumberOfColumns];
            labels = new Label[childNumberOfColumns];

            for (int i = 0; i < childNumberOfColumns; i++)
            {
                textBoxes[i] = new TextBox();

                labels[i] = new Label { Text = Column_names[i] };



                labels[i].Left = 10;
                labels[i].Top = (i + 1) * 23;

                textBoxes[i].Left = 120;
                textBoxes[i].Top = (i + 1) * 23;

                if (labels[i].Text.StartsWith("id"))
                    textBoxes[i].ReadOnly = true;
                if (labels[i].Text.StartsWith("Id"))
                    textBoxes[i].ReadOnly = true;

                labels[i].AutoSize = true;
                labels[i].Parent = fieldsPanel;
                textBoxes[i].Parent = fieldsPanel;
            }
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
            if (verificare_fields())
                try
                {
                    SqlCommand cmd = new SqlCommand(ConfigurationManager.AppSettings["DeleteQuery"], conn);
                    cmd.Parameters.AddWithValue("@" + labels[0].Text, textBoxes[0].Text);

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
            if (childNumberOfColumns  != dataGridView4.ColumnCount )
                return;
            for (int i = 0; i < childNumberOfColumns; i++)
            {
                textBoxes[i].Text = dataGridView4.Rows[e.RowIndex].Cells[i].Value.ToString();
            }
        }


        bool verificare_fields()
        {
            foreach (TextBox xxxx in textBoxes)
            {
                if (xxxx.Text.Trim() == "")
                {
                    MessageBox.Show("Campurile trebuiesc completate !");
                    return false;
                }

            }
            return true;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            //
            if (verificare_fields())
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(ConfigurationManager.AppSettings["InsertQuery"], conn);

                    for (int i = 0; i < childNumberOfColumns; ++i)
                        cmd.Parameters.AddWithValue("@" + labels[i].Text, textBoxes[i].Text);

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
        {//
            if (verificare_fields())
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(ConfigurationManager.AppSettings["UpdateQuery"], conn);
                    for (int i = 0; i < childNumberOfColumns; ++i)
                        cmd.Parameters.AddWithValue("@" + labels[i].Text, textBoxes[i].Text);

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
            if (childNumberOfColumns  != dataGridView3.ColumnCount-1)
                return;
            for (int i = 0; i < childNumberOfColumns; i++)
            {
                textBoxes[i].Text = dataGridView3.Rows[e.RowIndex].Cells[i].Value.ToString();
            }
        }



        //trans1 sau trans2
        void thread(string nume_proc)
        {
            using (SqlCommand cmd = new SqlCommand(nume_proc, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
        }
        void dead_lock()
        {
            conn.Open();
            Thread t3 = new Thread(() => thread("trans1"));
            t3.Start();
            Thread t4 = new Thread(() => thread("trans2"));
            t4.Start();

            conn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
