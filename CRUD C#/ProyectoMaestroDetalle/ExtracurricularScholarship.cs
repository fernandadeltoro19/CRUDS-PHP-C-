﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ProyectoMaestroDetalle
{
    public partial class ExtracurricularScholarship : Form
    {
        private string connectionString = "Data Source=AXEL\\SQLEXPRESS;Initial Catalog=Extracurricular;Integrated Security=True;Connect Timeout=60";

        public ExtracurricularScholarship()
        {
            InitializeComponent();
        }

        private void ExtracurricularScholarship_Load(object sender, EventArgs e)
        {
            txtDescription.MaxLength = 20;
            txtElegebilityRequeriments.MaxLength = 20;
            txtScholarshipAmount.MaxLength = 20;
            txtScholarshipDuration.MaxLength = 20;


            dataGridView1.CellClick += dataGridView1_CellClick;

            CargarDatosAccreditationControl();

            btnModificar.Enabled = false;
        }
        private void CargarDatosAccreditationControl()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "EXEC GetScholarships";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;
                }

                connection.Close();
            }
        }
            private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                    txtDescription.Text = selectedRow.Cells["description"].Value.ToString();
                txtScholarshipDuration.Text = selectedRow.Cells["scholarshipDuration"].Value.ToString();
                txtElegebilityRequeriments.Text = selectedRow.Cells["eligibilityRequirements"].Value.ToString();
                txtScholarshipAmount.Text = selectedRow.Cells["scholarshipAmount"].Value.ToString();

                btnModificar.Enabled = true;
                }
            }

            private int GetselectedAccreditationControl()
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                    return (int)selectedRow.Cells["idScholarShip"].Value;
                }

                return -1;
            }

        private void btninsertar_Click(object sender, EventArgs e)
        {
            string description = label123.Text;
            string scholarshipduration = label22.Text;
            string elegibilityRequeriments = txtElegebilityRequeriments.Text;
            decimal scholarshipAmount =decimal.Parse( txtScholarshipAmount.Text);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("CreateScholarship", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@description", description);
                    command.Parameters.AddWithValue("@scholarshipAmount", scholarshipAmount);
                    command.Parameters.AddWithValue("@eligibilityRequirements", elegibilityRequeriments);
                    command.Parameters.AddWithValue("@scholarshipduration", scholarshipduration);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Insertado correctamente.");
                        CargarDatosAccreditationControl();
                    }
                    else
                    {
                        MessageBox.Show("Insertado correctamente.");
                    }
                }

                connection.Close();
            }
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            MenuOpciones me = new MenuOpciones();
            me.Show();
            this.Hide();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            int Scholarshipid = GetselectedAccreditationControl();
            string description = txtDescription.Text;
            string scholarshipduration = txtScholarshipDuration.Text;
            string elegibilityRequeriments = txtElegebilityRequeriments.Text;
            decimal scholarshipAmount = decimal.Parse(txtScholarshipAmount.Text);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("UpdateScholarship", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@idScholarship", Scholarshipid);
                        command.Parameters.AddWithValue("@description", description);
                        command.Parameters.AddWithValue("@scholarshipAmount", scholarshipAmount);
                        command.Parameters.AddWithValue("@eligibilityRequirements", elegibilityRequeriments);
                        command.Parameters.AddWithValue("@scholarshipduration", scholarshipduration);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Acreditación modificada correctamente.");
                            CargarDatosAccreditationControl();
                        }
                        else
                        {
                            MessageBox.Show("Error al modificar la acreditación.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int accreditationID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["idScholarship"].Value);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("DeleteScholarship", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@idScholarship", accreditationID);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
                            MessageBox.Show("Registro eliminado correctamente.");
                        }
                        else
                        {
                            MessageBox.Show("Error al eliminar el registro.");
                        }
                    }

                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Seleccione una fila para eliminar.");
            }
        }
    }
}
