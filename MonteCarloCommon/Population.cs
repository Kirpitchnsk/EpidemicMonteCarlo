using System;
using System.Linq;

namespace MonteCarloCommon
{
    /// <summary>
    /// Представляет популяцию инидивидов в модели SEIRD
    /// </summary>
    public class Population
    {
        private readonly Individual[] individuals;

        private readonly Random random = new Random();

        /// <summary>
        /// Число здоровых индивидов
        /// </summary>
        public int SusceptibleCount
        {
            get
            {
                return Individuals.Count(i => i.CurrentState == Individual.State.Susceptible);
            }
        }

        /// <summary>
        /// Число индивидов в инкубационном периоде
        /// </summary>
        public int ExposedCount
        {
            get
            {
                return Individuals.Count(i => i.CurrentState == Individual.State.Exposed);
            }
        }

        /// <summary>
        /// Число инфицированных индивидов
        /// </summary>
        public int InfectedCount
        {
            get
            {
                return Individuals.Count(i => i.CurrentState == Individual.State.Infected);
            }
        }

        /// <summary>
        /// Число выздоровевшиих индивидов
        /// </summary>
        public int RecoveredCount
        {
            get
            {
                return Individuals.Count(i => i.CurrentState == Individual.State.Recovered);
            }
        }

        /// <summary>
        /// Число умерших индивиидов
        /// </summary>
        public int DeadCount
        {
            get
            {
                return Individuals.Count(i => i.CurrentState == Individual.State.Dead);
            }
        }

        /// <summary>
        /// Список имеющихся индивиидов
        /// </summary>
        public Individual[] Individuals
        {
            get
            {
                return (Individual[])individuals.Clone();
            }
        }

        /// <summary>
        /// Инициализирует новую популяцию с заданным размером. Один индивид инфицирован по умолчанию
        /// </summary>
        /// <param name="populationSize">Размер популяции</param>
        public Population(int populationSize)
        {
            individuals = new Individual[populationSize];

            for (int i = 0; i < populationSize; i++) individuals[i] = new Individual(Individual.State.Susceptible);

            individuals[random.Next(populationSize)].UpdateState(Individual.State.Infected);
        }

        /// <summary>
        /// Инициализирует новую популяцию с указанными индивидами
        /// </summary>
        /// <param name="individuals">Список индивидов</param>
        public Population(Individual[] individuals)
        {
            this.individuals = (Individual[])individuals.Clone();
        }

        /// <summary>
        /// Созданиие копии объекта представляющий популяцию
        /// </summary>
        /// <returns>Копия объекта</returns>
        public Population Clone()
        {
            var clonedIndividuals = individuals.Select(ind => new Individual(ind.CurrentState)).ToArray();
            return new Population(clonedIndividuals);
        }

        /// <summary>
        /// Смена состояния индивиида с oldState на newState
        /// </summary>
        /// <param name="oldState">Предыдущее состояние индивида</param>
        /// <param name="newState">Обновленное состояние инндивида</param>
        public void UpdateState(Individual.State oldState, Individual.State newState)
        {
            var candidates = individuals.Where(i => i.CurrentState == oldState).ToList();
            if (candidates.Count > 0)
            {
                candidates[random.Next(candidates.Count)].UpdateState(newState);
            }
        }
    }
}