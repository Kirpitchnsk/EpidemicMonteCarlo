namespace MonteCarloCommon
{
    /// <summary>
    /// Класс, представляющий необходимую строку данных из CSV файла.
    /// </summary>
    public class CsvRow
    {
        public double Time { get; set; }
        public int Susceptible { get; set; }
        public int Exposed { get; set; }
        public int Infected { get; set; }
        public int Recovered { get; set; }
        public int Dead { get; set; }
    }
}
