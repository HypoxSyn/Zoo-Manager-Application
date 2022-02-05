using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;


namespace Zoo_Manager_Application_1._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        SqlConnection sqlConnection;
        public MainWindow()
        {

            InitializeComponent();

            //namespace.properties.settings."whateverstringwassetupforthedb" close the final portion with .ConnectionString
            string connectionString = ConfigurationManager.ConnectionStrings["Zoo_Manager_Application_1._0.Properties.Settings.PanjututorialsDBConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);

            ShowZoos();
            ShowAllAnimals();

        }
        private void ShowZoos()
        {
            try
            {
                string query = "select * from Zoo";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable zooTable = new DataTable();
                    sqlDataAdapter.Fill(zooTable);

                    listZoos.DisplayMemberPath = "Location"; //what I want to see with DisplayMemberPath
                    listZoos.SelectedValuePath = "Id"; //select by ID
                    listZoos.ItemsSource = zooTable.DefaultView;

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void ShowAssociatedAnimals()
        {
            try
            {
                string query = "select * from Animal a inner join ZooAnimal za on a.Id = za.AnimalId where za.ZooId = @ZooId";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {

                    sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);

                    DataTable animalTable = new DataTable();

                    sqlDataAdapter.Fill(animalTable);

                    listAssociatedAnimals.DisplayMemberPath = "Name"; //what I want to see with DisplayMemberPath
                    listAssociatedAnimals.SelectedValuePath = "Id"; //select by ID
                    listAssociatedAnimals.ItemsSource = animalTable.DefaultView;

                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
            }
        }

        private void ShowAllAnimals()
        {

            try
            {
                string query = "select * from Animal";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable animalTable = new DataTable();
                    sqlDataAdapter.Fill(animalTable);

                    listAllAnimals.DisplayMemberPath = "Name";
                    listAllAnimals.SelectedValuePath = "Id";
                    listAllAnimals.ItemsSource = animalTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ListZoos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowAssociatedAnimals(); //SHOW THE ANIMALS AT THE ZOOS
            ShowSelectedZooInTextBox(); //DISPLAYS THE NAME OF THE ZOO IN THE TEXTBOX

        }

        private void ListZoosTextBoxShow_SelectionChange(object sender, SelectionChangedEventArgs e)
        {
            ShowSelectedAnimalInTextBox();
            ShowSelectedZooInTextBox(); //4:37 into Updating Entries in Our Tables
        }



        private void DeleteZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "delete from Zoo where id = @ZooId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlCommand.ExecuteScalar();
                sqlConnection.Close();
                ShowZoos();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();

            }

        }

        private void AddAnimalToZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "insert into ZooAnimal values (@ZooId, @AnimalId)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);
                sqlCommand.ExecuteScalar();


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAssociatedAnimals();

            }
        }

        private void DeleteAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "delete from Animal where id = @AnimalId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);
                sqlCommand.ExecuteScalar();
                sqlConnection.Close();
                ShowAllAnimals();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();
                ShowAllAnimals();
            }
        }

        private void AddAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "insert into Animal values (@Name)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Name", myTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
            finally
            {
                sqlConnection.Close();
                ShowAllAnimals();
            }


        }

        private void RemoveAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "delete from ZooAnimal values (@ZooId, @AnimalId)";
                // string query = "insert into ZooAnimal values (@ZooId, @AnimalId)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", listAssociatedAnimals.SelectedValue);
                sqlCommand.ExecuteScalar();
                sqlConnection.Close();
                ShowAssociatedAnimals();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();

            }
        }

        private void AddZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "insert into Zoo values (@Location)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Location", myTextBox.Text);
                sqlCommand.ExecuteScalar();
                sqlConnection.Close();
                ShowZoos();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();

            }
        }

        private void ShowSelectedZooInTextBox()
        {
            {
                try
                {
                    string query = "select location from Zoo where Id = @zooId";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                    using (sqlDataAdapter)
                    {

                        sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);

                        DataTable zooDataTable = new DataTable();

                        sqlDataAdapter.Fill(zooDataTable);

                        myTextBox.Text = zooDataTable.Rows[0]["Location"].ToString();
                    }
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.ToString());
                }
            }
        }

        private void ShowSelectedAnimalInTextBox()
        {
            {
                try
                {
                    string query = "select name from Animal where Id = @animalId";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                    using (sqlDataAdapter)
                    {

                        sqlCommand.Parameters.AddWithValue("@animalId", listAllAnimals.SelectedValue);

                        DataTable animalDataTable = new DataTable();

                        sqlDataAdapter.Fill(animalDataTable);

                        myTextBox.Text = animalDataTable.Rows[0]["Name"].ToString();
                    }
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.ToString());
                }

            }
        }

        private void UpdateZoo_Click(object sender, RoutedEventArgs e)
        {
            {
                try
                {
                    string query = "update Zoo Set Location = @Location where Id = @ZooId";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                    sqlCommand.Parameters.AddWithValue("@Location", myTextBox.Text);
                    sqlCommand.ExecuteScalar();


                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    sqlConnection.Close();
                    ShowZoos();

                }
            }
        }

        private void listAllAnimals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowSelectedAnimalInTextBox();
        }
    }

}

////Reminder after falling asleep that AddAnimal_Click does not work. It throws an error about columns and this has been the same type of issue when i wrote it as well
////something is off about the columns and code, but when looking at the video everything matches.
//ABOVE problem fixed. SQL in ZooAnimal had Cascade syntax in reverse from instructions