using System;

namespace MonteCarloCommon
{
    /// <summary>
    /// Предоставляет метод для конвертирования данных. Для числовых данных можно задать границы для проверки
    /// </summary>
    /// <typeparam name="T">Тип данных в который нужно конвертировать</typeparam>
    public static class InputValidator<T>
    {
        /// <summary>
        /// Проверка пользовательского ввода
        /// </summary>
        /// <param name="input">Строка ввода для проверки</param>
        /// <param name="minValue">Минимально допустимое значение</param>
        /// <param name="maxValue">Максимально допустимое значение</param>
        /// <param name="errorMessage">The error message if validation fails.</param>
        /// <returns>Проверенное значение типа T</returns>
        public static T GetValidatedInput(string input, double minValue, double maxValue, out string errorMessage)
        {
            errorMessage = null;
            try
            {
                var value = (T)Convert.ChangeType(input, typeof(T));
                var doubleValue = Convert.ToDouble(value);

                if (doubleValue >= minValue && doubleValue <= maxValue)
                {
                    return value;
                }
                else
                {
                    errorMessage = $"Значение должно быть в диапазоне от {minValue} до {maxValue}.";
                    return default;
                }
            }
            catch
            {
                errorMessage = "Неверный ввод.";
                return default;
            }
        }
    }
}
