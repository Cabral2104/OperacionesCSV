using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OperacionesCSV
{
    public partial class Form1 : Form
    {
        private string currentFilePath;
        private DataTable dataTable;
        private DataTable originalDataTable;

        public Form1()
        {
            InitializeComponent();
        }


        private void SaveAsFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Archivos CSV (*.csv)|*.csv|Todos los archivos (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                currentFilePath = saveFileDialog.FileName;
                SaveCsvFile(currentFilePath);
            }
        }

        private void LoadCsvFile(string filePath)
        {
            try
            {
                dataTable = new DataTable();
                originalDataTable = new DataTable();
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string[] headers = sr.ReadLine().Split(',');
                    foreach (string header in headers)
                    {
                        dataTable.Columns.Add(header);
                        originalDataTable.Columns.Add(header);
                    }

                    while (!sr.EndOfStream)
                    {
                        string[] rows = sr.ReadLine().Split(',');
                        DataRow dataRow = dataTable.NewRow();
                        DataRow originalDataRow = originalDataTable.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dataRow[i] = rows[i];
                            originalDataRow[i] = rows[i];
                        }
                        dataTable.Rows.Add(dataRow);
                        originalDataTable.Rows.Add(originalDataRow);
                    }
                }
                dataGridView.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el archivo CSV: " + ex.Message);
            }
        }

        private void SaveCsvFile(string filePath)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string rowData = "";
                        foreach (var item in row.ItemArray)
                        {
                            rowData += item.ToString() + ",";
                        }
                        sw.WriteLine(rowData.TrimEnd(','));
                    }
                }
                MessageBox.Show("Archivo CSV guardado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el archivo CSV: " + ex.Message);
            }
        }

        //private void EditButton_Click(object sender, EventArgs e)
        //{
        //    dataGridView.ReadOnly = false;
        //    dataGridView.AllowUserToAddRows = true;
        //}

        //private void AddRowButton_Click(object sender, EventArgs e)
        //{
        //    DataRow newRow = dataTable.NewRow();
        //    dataTable.Rows.Add(newRow);
        //}

        //private void UndoButton_Click(object sender, EventArgs e)
        //{
        //    dataTable = originalDataTable.Copy();
        //    dataGridView.DataSource = dataTable;
        //}

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos CSV (*.csv)|*.csv|Todos los archivos (*.*)|*.*";
            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                currentFilePath = openFileDialog.FileName;
                LoadCsvFile(currentFilePath);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (currentFilePath != null)
            {
                SaveCsvFile(currentFilePath);
            }
            else
            {
                SaveAsFile();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            dataGridView.ReadOnly = false;
            dataGridView.AllowUserToAddRows = true;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            DataRow newRow = dataTable.NewRow();
            dataTable.Rows.Add(newRow);
        }

        private void btnDeshacer_Click(object sender, EventArgs e)
        {
            dataTable = originalDataTable.Copy();
            dataGridView.DataSource = dataTable;
        }
    }
}
