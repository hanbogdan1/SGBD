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
        public Form1()
        {
            InitializeComponent();

            SqlConnection conn = new SqlConnection("Data Source = BOGDAN-PC\\SQLEXPRESS; Initial Catalog = Vending company ; Integrated Security = True");
            SqlDataAdapter dataAdapDist = new SqlDataAdapter("select * from distribuitori", conn);
            SqlDataAdapter dataAdaptFact = new SqlDataAdapter("select * from factura", conn);
            Console.WriteLine(conn.State);
            Console.WriteLine(conn.ConnectionTimeout);
            Console.WriteLine("asdasdasdasdasd");
            DataSet ds = new DataSet();
            BindingSource bsd = new BindingSource();
            BindingSource bsf = new BindingSource();
            dataAdapDist.Fill(ds, "Distribuitori");
            dataAdaptFact.Fill(ds, "Factura");
            

            SqlCommandBuilder ComBuildDist = new SqlCommandBuilder(dataAdapDist);

            DataRelation dr = new DataRelation("FK_DISTRIBUITOR_FACTURA",  ds.Tables["Distribuitori"].Columns["Id"], ds.Tables["Factura"].Columns["IdDistribuitor"]);
            ds.Relations.Add(dr);
            bsd.DataSource = ds;
            bsd.DataMember = "Distribuitori";

            bsf.DataSource = bsd;
            bsf.DataMember = "FK_DISTRIBUITOR_FACTURA";

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            

        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
