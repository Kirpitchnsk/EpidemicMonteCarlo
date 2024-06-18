using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace MonteCarloCommon
{
    /// <summary>
    /// Предоставляет метод для чтения CSV-файлов и преобразования их в список объектов CsvRow
    /// </summary>
    public class CsvReader
    {
        /// <summary>
        /// Число строчек в csv файле
        /// </summary>
        private const int ExpectedColumnCount = 6;

        /// <summary>
        /// Считывает данные со статистикой из CSV-файла
        /// </summary>
        /// <param name="filePath">Путь к csv файлу со статистикой</param>
        /// <returns>Список объектов CsvRow, содержащих статистику</returns>
        /// <exception cref="InvalidDataException">Возникает, когда CSV-файл имеет некорректную структуру</exception>
        public List<CsvRow> ReadStatistics(string filePath)
        {
            var statisticsList = new List<CsvRow>();
            var lines = File.ReadAllLines(filePath);

            if (lines.Length == 0)
            {
                throw new InvalidDataException("CSV файл пустой.");
            }

            var headers = lines[0].Split(',');

            if (headers.Length != ExpectedColumnCount || !ValidateHeaders(headers))
            {
                throw new InvalidDataException("Неверная структура CSV файла.");
            }

            for (int i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(',');

                if (values.Length != ExpectedColumnCount)
                {
                    throw new InvalidDataException($"Неверное количество столбцов в строке {i + 1}. Ожидается {ExpectedColumnCount}.");
                }

                var dayStatistics = new CsvRow
                {
                    Time = Math.Round(double.Parse(values[0].Replace(".",",")),2),
                    Susceptible = int.Parse(values[1]),
                    Exposed = int.Parse(values[2]),
                    Infected = int.Parse(values[3]),
                    Recovered = int.Parse(values[4]),
                    Dead = int.Parse(values[5])
                };

                statisticsList.Add(dayStatistics);
            }

            return statisticsList;
        }

        /// <summary>
        /// Проверка на соответсвие заголовков CSV файла
        /// </summary>
        /// <param name="headers">Проверяемые заголовки</param>
        /// <returns>Истина или ложь в зависимости от совпадаемости заголовков</returns>
        private bool ValidateHeaders(string[] headers)
        {
            var expectedHeaders = new[]
            {
                "Time", "Susceptible", "Exposed", "Infected", "Recovered", "Dead"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                if (!string.Equals(headers[i], expectedHeaders[i], StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }
    }
}