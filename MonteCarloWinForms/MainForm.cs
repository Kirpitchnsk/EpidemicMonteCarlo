using MonteCarloCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MonteCarloWinForms
{
    public partial class MainForm : Form
    {
        private List<CsvRow> statisticsList;
        private readonly ChartManager chartManager;

        public MainForm()
        {
            InitializeComponent();
            chartManager = new ChartManager(graphField);
        }

        /// <summary>
        /// Событие, которое срабатывает при нажатии на элемент открыть файл
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFileMenuItem_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                Title = "Открыть CSV таблицу"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    LoadStatistics(openFileDialog.FileName);
                    chartManager.SetupChart(statisticsList);
                }
                catch (InvalidDataException ex)
                {
                    MessageBox.Show($"Произошла ошибка при чтении файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Событие, которое срабатывает при нажатии на элемент сохранить график
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveChartMenuItem_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "PNG Image|*.png",
                Title = "Сохранить график в формате png"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                graphField.SaveImage(saveFileDialog.FileName, ChartImageFormat.Png);
            }
        }

        /// <summary>
        /// Отображение справки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void helpMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Загрузите CSV файл и отобразится график\n" +
                "Created by MrZiegel,2024");
        }

        /// <summary>
        /// Загрузка CSV файла со статистикой
        /// </summary>
        /// <param name="filePath"></param>
        private void LoadStatistics(string filePath)
        {
            var csvReader = new CsvReader();
            statisticsList = csvReader.ReadStatistics(filePath);
        }

    }
}
