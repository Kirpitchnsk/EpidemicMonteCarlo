using MonteCarloCommon;
using System;
using System.Linq;

namespace EpidemicMonteCarloConsole
{
    public class Simulation
    {
        private readonly double beta;
        private readonly double sigma;
        private readonly double gamma;
        private readonly int N;
        private readonly int modelSteps;
        private readonly Random random;
        private Population currentPopulation;
        private Statistics statistics;

        /// <summary>
        /// Основной конструктор класса симуляция
        /// </summary>
        /// <param name="beta">Вероятность заражения</param>
        /// <param name="sigma">Вероятность перехода из </param>
        /// <param name="gamma">Вероятность выздоровления</param>
        /// <param name="N">Численность популяции</param>
        /// <param name="modelSteps">Количество шагов моделирования</param>
        public Simulation(double beta, double sigma, double gamma, int N, int modelSteps)
        {
            this.beta = beta;
            this.sigma = sigma;
            this.gamma = gamma;
            this.N = N;
            this.modelSteps = modelSteps;

            random = new Random();
            statistics = new Statistics();
        }

        /// <summary>
        /// Запуск моделировнаия эпидемии
        /// </summary>
        public void Run()
        {
            currentPopulation = new Population(N); 

            var time = 0.0;
            var stepCounter = 0;

            while (stepCounter < modelSteps && (currentPopulation.InfectedCount > 0 || currentPopulation.ExposedCount > 0))
            {
                statistics.AddRecord(time, currentPopulation.Clone());

                // Общая вероятность события
                var lambdaTotal = beta * currentPopulation.SusceptibleCount * currentPopulation.InfectedCount +
                                     sigma * currentPopulation.ExposedCount +
                                     gamma * currentPopulation.InfectedCount + 
                                     (1 - gamma) * currentPopulation.InfectedCount;

                if (lambdaTotal == 0)
                {
                    break;
                }

                // Рассчет времени до следущего события
                var tau = -Math.Log(random.NextDouble()) / lambdaTotal;

                time += tau;

                if (stepCounter > modelSteps)
                {
                    break;
                }

                // Выбор типа события
                var eventProb = random.NextDouble() * lambdaTotal;
                if (eventProb < beta * currentPopulation.SusceptibleCount * currentPopulation.InfectedCount)
                {
                    // Инфецирование
                    currentPopulation.UpdateState(Individual.State.Susceptible, Individual.State.Exposed);
                }
                else if (eventProb < beta * currentPopulation.SusceptibleCount * currentPopulation.InfectedCount +
                                  sigma * currentPopulation.ExposedCount)
                {
                    // Переход из инкубационного периода в зараженные
                    currentPopulation.UpdateState(Individual.State.Exposed, Individual.State.Infected);
                }
                else if (eventProb < beta * currentPopulation.SusceptibleCount * currentPopulation.InfectedCount +
                                  sigma * currentPopulation.ExposedCount +
                                  gamma * currentPopulation.InfectedCount)
                {
                    // Выздоровление
                    currentPopulation.UpdateState(Individual.State.Infected, Individual.State.Recovered);
                }
                else if (eventProb < beta * currentPopulation.SusceptibleCount * currentPopulation.InfectedCount +
                                  sigma * currentPopulation.ExposedCount +
                                  gamma * currentPopulation.InfectedCount + (1-gamma) * currentPopulation.InfectedCount)
                {
                    // Смерть
                    currentPopulation.UpdateState(Individual.State.Infected, Individual.State.Dead);
                }

                stepCounter++;
            }

            statistics.AddRecord(time, currentPopulation.Clone());
        }

        /// <summary>
        /// Вывод результатов на экран консоли
        /// </summary>
        public void PrintResults()
        {
            Console.WriteLine("Time\t\tSusceptible\tExposed\t\tInfected\tRecovered\tDead");
            foreach (var record in statistics.Data)
            {
                Console.WriteLine($"{record.Time:N5}\t\t{record.Susceptible}\t\t{record.Exposed}\t\t{record.Infected}\t\t{record.Recovered}\t\t{record.Dead}");
            }

            Console.WriteLine("Вероятность распространения эпидемии: " + CalculateEpidemicSpreadProbability());
        }

        /// <summary>
        /// Сохранение результатов моделирование в файл CSV
        /// </summary>
        public void SaveResults(string filePath)
        {
            statistics.SaveToFile(filePath);
        }

        /// <summary>
        /// Функция  для расчета вероятности распространения эпидемии
        /// </summary>
        /// <returns>Искомая вероятность распространения</returns>
        public double CalculateEpidemicSpreadProbability()
        {
            if (statistics.Data.Count > 0)
            {
                int maxInfected = statistics.Data.Max(record => record.Infected);
                int totalPopulation = N;

                return (double)maxInfected / totalPopulation;
            }
            else
            {
                return 0;
            }
        }

    }
}
