using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace MSSQLforCS
{
    public partial class Form1 : Form
    {
        private SqlConnection northwndConnection = null;
        private List<string[]> rows = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            northwndConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Northwind"].ConnectionString);
            northwndConnection.Open();
            SqlDataAdapter da = new SqlDataAdapter("select * from Products", northwndConnection);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView2.DataSource = ds.Tables[0];



            SqlDataReader dataReader = null;
            rows =new List<string[]>();
            string[] row = null; ;
            try
            {
                SqlCommand sqlcommand = new SqlCommand("select ProductName,QuantityPerUnit,UnitPrice " +
                    "from Products", northwndConnection);
                dataReader = sqlcommand.ExecuteReader();
                while (dataReader.Read())
                {
                    row = new string[]
                    {
                        Convert.ToString(dataReader["ProductName"]),
                        Convert.ToString(dataReader["QuantityPerUnit"]),
                        Convert.ToString(dataReader["UnitPrice"])
                    };
                    rows.Add(row);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
            }
            RefreshList(rows);
        }
        private void RefreshList(List<string[]> list)
        {
            listView2.Clear();
            foreach (string[] s in list)
            {
                listView2.Items.Add(new ListViewItem(s));
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter(tbRequest.Text, northwndConnection);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            dataGridView1.DataSource = dataSet.Tables[0];

        }

        private void btnSelect2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            SqlDataReader dataReader = null;
            try
            {
                SqlCommand sqlcommand = new SqlCommand("select ProductName, QuantityPerUnit, UnitPrice from Products", northwndConnection);
                dataReader = sqlcommand.ExecuteReader();
                ListViewItem item = null;
                while (dataReader.Read())
                {
                    item = new ListViewItem(new string[] { Convert.ToString(dataReader["ProductName"]),
                        Convert.ToString(dataReader["QuantityPerUnit"]) ,Convert.ToString(dataReader["UnitPrice"]) });
                    listView1.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
            }
        }

        private void tbNameProduct_TextChanged(object sender, EventArgs e)
        {
            (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"ProductName like '%{tbNameProduct.Text}%'  ";
        }

        private void cbfilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbfilter.SelectedIndex)
            {
                case 0:
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"UnitsInStock <=10 ";
                    break;
                case 1:
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"UnitsInStock >= 10 and UnitsInStock <= 50 ";
                    break;
                case 2:
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = $"UnitsInStock >= 50";
                    break;
                case 3:
                    (dataGridView2.DataSource as DataTable).DefaultView.RowFilter = "";
                    break;

            }
        }
    }
}
