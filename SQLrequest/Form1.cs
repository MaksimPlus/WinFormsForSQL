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

namespace SQLrequest
{
    public partial class Form1 : Form
    {
        private SqlConnection northwndconnection = null;
        private List<string[]> rows=new List<string[]>();
        private List<string[]> filteredlist = null;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            northwndconnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Northwind"].
                ConnectionString);
            northwndconnection.Open();
            SqlDataReader dataReader = null;
            string[] row = null;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT ProductName,QuantityPerUnit,UnitPrice " +
                    "FROM Products", northwndconnection);
                dataReader=sqlCommand.ExecuteReader();
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
            listView1.Items.Clear();
            foreach(string[] s in list)
            {
                listView1.Items.Add(new ListViewItem(s));
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {

                 case 0:

            filteredlist = rows.Where((x) => Double.Parse(x[2])<=10).ToList();
                    RefreshList(filteredlist);
            break;

                 case 1:

                    filteredlist = rows.Where((x) => Double.Parse(x[2]) >10 && Double.Parse(x[2]) <=100).ToList();
                    RefreshList(filteredlist);
                    break;

                case 2:

                    filteredlist = rows.Where((x) => Double.Parse(x[2]) > 100).ToList();
                    RefreshList(filteredlist);
                    break;

                case 3:

                    RefreshList(rows);

                    break;




        }


    }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            filteredlist = rows.Where((x) => x[0].ToLower().Contains(textBox1.Text.ToLower())).ToList();
            RefreshList(filteredlist);
        }
    }
}
