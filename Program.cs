using MonteCarloCommon;

namespace EpidemicMonteCarloConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите параметры модели:");

            var beta = GetInputFromConsole<double>("Вероятность заражения при контакте (beta): ", 0, 1);
            var sigma = GetInputFromConsole<double>("Вероятность перехода из инкубационного периода (sigma): ", 0, 1);
            var gamma = GetInputFromConsole<double>("Вероятность выздоровления (gamma): ", 0, 1);
            var N = GetInputFromConsole<int>("Общая численность популяции (N): ", 1, int.MaxValue);
            var stepNumber = GetInputFromConsole<int>("Количество шагов моделирования: ", 1, int.MaxValue);

            var simulation = new Simulation(beta, sigma, gamma, N, stepNumber);
            simulation.Run();
            simulation.PrintResults();

            Console.WriteLine("Введите название csv файла, расширение записывать не нужно:");
            var filePath = Console.ReadLine() + ".csv";
            simulation.SaveResults(filePath);

            Console.WriteLine($"Файл {filePath} успешно сохранен");
        }

        /// <summary>
        /// Ввод данных в конслоь и проверка ввода до тех поор пока данные не бдут введены корректно
        /// </summary>
        /// <typeparam name="T">Тип получаемых даннных</typeparam>
        /// <param name="prompt">Дополнительный текст для ввода</param>
        /// <param name="minValue">Минимальное допустимое число</param>
        /// <param name="maxValue">Максимальное допустимое число</param>
        /// <returns></returns>
        private static T GetInputFromConsole<T>(string prompt, double minValue, double maxValue)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();
                var result = InputValidator<T>.GetValidatedInput(input, minValue, maxValue, out string errorMessage);

                if (errorMessage == null)
                {
                    return result;
                }
                else
                {
                    Console.WriteLine(errorMessage);
                }
            }
        }
    }
}
