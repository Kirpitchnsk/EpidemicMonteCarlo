using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MonteCarloCommon
{
    public class Statistics
    {
        public List<CsvRow> Data { get; private set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса Statistics
        /// </summary>
        /// <param name="days">Количество дней моделирования</param>
        public Statistics()
        {
            Data = new List<CsvRow>();
        }

        /// <summary>
        /// Вычисляет среднее значение и стандартное отклонение за каждый день на основе полученных результатов
        /// </summary>
        /// <param name="results">Результаты симуляции</param>
        public void AddRecord(double time, Population population)
        {
            Data.Add(new CsvRow
            {
                Time = time,
                Susceptible = population.SusceptibleCount,
                Exposed = population.ExposedCount,
                Infected = population.InfectedCount,
                Recovered = population.RecoveredCount,
                Dead = population.DeadCount
            });
        }

        /// <summary>
        /// Сохранение статистики в файл формата csv
        /// </summary>
        /// <param name="filePath">Имя файла</param>
        public void SaveToFile(string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Time,Susceptible,Exposed,Infected,Recovered,Dead");
                foreach (var record in Data)
                {
                    writer.WriteLine($"{record.Time.ToString().Replace(",",".")},{record.Susceptible},{record.Exposed},{record.Infected},{record.Recovered},{record.Dead}");
                }
            }
        }
    }
}
