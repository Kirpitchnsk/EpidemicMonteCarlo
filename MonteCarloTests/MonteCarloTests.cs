using EpidemicMonteCarloConsole;
using MonteCarloCommon;

namespace MonteCarloTests
{
    [TestFixture]
    public class UnitTests
    {
        [Test]
        public void Individual_StateChange_Success()
        {
            var individual = new Individual(Individual.State.Susceptible);
            Assert.That(individual.CurrentState, Is.EqualTo(Individual.State.Susceptible));

            individual.UpdateState(Individual.State.Infected);
            Assert.That(individual.CurrentState, Is.EqualTo(Individual.State.Infected));
        }

        [Test]
        public void Population_Initialization_Success()
        {
            var population = new Population(100);
            Assert.Multiple(() =>
            {
                Assert.That(population.SusceptibleCount, Is.EqualTo(99));
                Assert.That(population.InfectedCount, Is.EqualTo(1));
                Assert.That(population.ExposedCount, Is.EqualTo(0));
                Assert.That(population.RecoveredCount, Is.EqualTo(0));
                Assert.That(population.DeadCount, Is.EqualTo(0));
            });
        }

        [Test]
        public void Population_UpdateState_Success()
        {
            var population = new Population(100);
            population.UpdateState(Individual.State.Susceptible, Individual.State.Exposed);
            Assert.Multiple(() =>
            {
                Assert.That(population.SusceptibleCount, Is.EqualTo(98));
                Assert.That(population.ExposedCount, Is.EqualTo(1));
            });
        }

        [Test]
        public void Statistics_AddRecord_Success()
        {
            var population = new Population(100);
            var statistics = new Statistics();
            statistics.AddRecord(0.0, population);

            Assert.That(statistics.Data.Count, Is.EqualTo(1));
            Assert.That(statistics.Data[0].Susceptible + statistics.Data[0].Exposed + statistics.Data[0].Infected + statistics.Data[0].Recovered + statistics.Data[0].Dead, Is.EqualTo(100));
        }

        [Test]
        public void Simulation_Run_Success()
        {
            var simulation = new Simulation(0.3, 0.2, 0.1, 1000, 160);
            Assert.DoesNotThrow(() => simulation.Run());
        }

        [Test]
        public void Simulation_PrintResults_Success()
        {
            var simulation = new Simulation(0.3, 0.2, 0.1, 1000, 160);
            simulation.Run();
            Assert.DoesNotThrow(() => simulation.PrintResults());
        }

        [Test]
        public void CsvReader_ReadStatistics_Success()
        {
            var csvContent = "Time,Susceptible,Exposed,Infected,Recovered,Dead\n0,999,0,1,0,0\n1,998,1,1,0,0";
            var filePath = Path.GetTempFileName();
            File.WriteAllText(filePath, csvContent);

            var csvReader = new CsvReader();
            var statistics = csvReader.ReadStatistics(filePath);

            Assert.That(statistics.Count, Is.EqualTo(2));
            Assert.Multiple(() =>
            {
                Assert.That(statistics[0].Susceptible, Is.EqualTo(999));
                Assert.That(statistics[1].Exposed, Is.EqualTo(1));
            });
        }

        [Test]
        public void InputValidator_GetValidatedInput_Success()
        {
            string errorMessage;

            var result = InputValidator<double>.GetValidatedInput("0,5", 0, 1, out errorMessage);
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(0.5));
                Assert.That(errorMessage, Is.Null);
            });

            result = InputValidator<double>.GetValidatedInput("1,5", 0, 1, out errorMessage);
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(0));
                Assert.That(errorMessage, Is.EqualTo("Значение должно быть в диапазоне от 0 до 1."));
            });
        }

        [Test]
        public void CsvReader_InvalidCsvStructure_ThrowsException()
        {
            var csvContent = "Time,Susceptible,Exposed,Infected,Recovered\n0,999,0,1,0";
            var filePath = Path.GetTempFileName();
            File.WriteAllText(filePath, csvContent);

            var csvReader = new CsvReader();
            Assert.Throws<InvalidDataException>(() => csvReader.ReadStatistics(filePath));
        }

        [Test]
        public void CsvReader_EmptyFile_ThrowsException()
        {
            var filePath = Path.GetTempFileName();

            var csvReader = new CsvReader();
            Assert.Throws<InvalidDataException>(() => csvReader.ReadStatistics(filePath));
        }
    }
}
