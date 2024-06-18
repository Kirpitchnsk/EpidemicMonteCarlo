namespace MonteCarloCommon
{
    /// <summary>
    /// Класс, представляющий индивида
    /// </summary>
    public class Individual
    {
        /// <summary>
        /// Перечисление состояний индивидуума
        /// </summary>
        public enum State { Susceptible, Exposed, Infected, Recovered, Dead }

        /// <summary>
        /// Текущее состояние индивидуума
        /// </summary>
        public State CurrentState { get; private set; }

        /// <summary>
        /// Инициализация состояния индивидуума
        /// </summary>
        /// <param name="initialState">Начальное состояние.</param>
        public Individual(State initialState)
        {
            CurrentState = initialState;
        }

        /// <summary>
        /// Обновление состояния индивидуума
        /// </summary>
        /// <param name="newState">Новое состояние.</param>
        public void UpdateState(State newState)
        {
            CurrentState = newState;
        }
    }
}
