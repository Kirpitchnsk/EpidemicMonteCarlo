using MonteCarloCommon;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace MonteCarloWinForms
{
    /// <summary>
    /// Управляет настройкой графика для виизуализации результатов
    /// </summary>
    public class ChartManager
    {
        private readonly Chart chart;

        /// <summary>
        /// Инициализирует новый экземпляр класса ChartManager
        /// </summary>
        /// <param name="chart">Элемент chart для построения диграммы</param>
        public ChartManager(Chart chart)
        {
            this.chart = chart;
        }

        /// <summary>
        /// Задает настройки для графика с учетом предоставленных статистических данных
        /// </summary>
        /// <param name="statisticsList">Статистика для отображения на графике</param>
        public void SetupChart(List<CsvRow> statisticsList)
        {
            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.ChartAreas.Add(new ChartArea("Main"));

            var seriesMeanS = CreateSeries("Воспримчивые S (Susceptible)");
            var seriesMeanE = CreateSeries("Подверженные E (Exposed)");
            var seriesMeanI = CreateSeries("Инфицированные I (Infected)");
            var seriesMeanR = CreateSeries("Выздоровевшие R (Recovered)");
            var seriesMeanD = CreateSeries("Умершие D (Deceased)");

            foreach (var stat in statisticsList)
            {
                seriesMeanS.Points.AddXY(stat.Time, stat.Susceptible);
                seriesMeanE.Points.AddXY(stat.Time, stat.Exposed);
                seriesMeanI.Points.AddXY(stat.Time, stat.Infected);
                seriesMeanR.Points.AddXY(stat.Time, stat.Recovered);
                seriesMeanD.Points.AddXY(stat.Time, stat.Dead);
            }

            chart.Series.Add(seriesMeanS);
            chart.Series.Add(seriesMeanE);
            chart.Series.Add(seriesMeanI);
            chart.Series.Add(seriesMeanR);
            chart.Series.Add(seriesMeanD);

            chart.ChartAreas[0].AxisX.Title = "Промежуток времени";
            chart.ChartAreas[0].AxisY.Title = "Популяция";

            chart.Titles.Clear();
            chart.Titles.Add("Моделирование эпидемии методом Монте-Карло, модель SEIRD");

            chart.Legends.Clear();
            var legend = new Legend
            {
                Docking = Docking.Top,
                Alignment = StringAlignment.Center
            };
            chart.Legends.Add(legend);
        }

        /// <summary>
        /// Инициализация рада данных
        /// </summary>
        /// <param name="seriesName">Название ряда</param>
        /// <returns>Создание ряда с указанным именем</returns>
        private Series CreateSeries(string seriesName)
        {
            return new Series(seriesName)
            {
                ChartType = SeriesChartType.Line
            };
        }
    }
}
