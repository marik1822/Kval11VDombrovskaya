using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Kval11VDombrovskaya
{
    /// <summary>
    /// Логика взаимодействия для AddNews.xaml
    /// </summary>
    public partial class AddNewsDombrovskaya : Window
    {
        public string id { get; set; }
        static string connectionString;
        SqlDataAdapter adapter;
        static DataTable News;
        static DataTable Groupp;
        static DataTable Agents;
        public AddNewsDombrovskaya()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();

            }
            catch (SqlException)
            {
                MessageBox.Show("Ошибка подключения БД");
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            News = new DataTable();
            SqlConnection connection = null;
            string sql1 = "Select * from IdMax;";
            if ((Name.Text=="")&&(Content.Text=="")&&(Teg.Text==""))
            {
                MessageBox.Show("Вы ничего не ввели");
            }
            if ((Name.Text != "") && (Content.Text != "") && (Teg.Text != ""))
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql1, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    id = reader[0].ToString();
                    int idN = int.Parse(id) + 1;
                    id = idN.ToString();
                    string sql = "INSERT INTO News(IdNews,NameNews,СontentNews,DateNews,IdAgent,Teg,IdGroup) VALUES("+id+",'"+Name.Text+"','"+Content.Text+"','"+DateTime.Now.ToString()+"',"+Agent.SelectedItem.ToString()+" ,'"+Teg.Text+"','"+Agent.SelectedItem.ToString()+"')";
                    SqlCommand command1 = new SqlCommand(sql, connection);
                    reader.Close();
                    int num = command1.ExecuteNonQuery();
                    if (num != 0)
                    {
                        MessageBox.Show("Новость успешно добавлена");
                        

                    }
                    else
                        MessageBox.Show("Ошибка добавления новости!!");
                    return;
                }
                reader.Close();
                MessageBox.Show("Ошибка добавления новости!!");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string sql;
            string sql2;
            SqlConnection connection = null;
            Agents = new DataTable();
            sql = "SELECT NameAgent,IdAgent from Agent;";
            connection = new SqlConnection(connectionString);
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
            connection.Open();

            adapter.Fill(Agents);
            for (int i = 0; i < Agents.Rows.Count; i++)
            {
                Agent.Items.Add(Agents.Rows[i]["IdAgent"].ToString());
            }

            connection.Close();
            
            SqlConnection connection2 = null;
            Groupp = new DataTable();
            sql2 = "SELECT NameGroup,IdGroup from GrouppNews ;";
            connection2 = new SqlConnection(connectionString);
            SqlDataAdapter adapter2 = new SqlDataAdapter(sql2, connection2);
            connection2.Open();
            adapter2.Fill(Groupp);
            for (int i = 0; i < Groupp.Rows.Count; i++)
            {
                Group.Items.Add(Groupp.Rows[i]["IdGroup"].ToString());
            }

            connection2.Close();

        }
    }
}
