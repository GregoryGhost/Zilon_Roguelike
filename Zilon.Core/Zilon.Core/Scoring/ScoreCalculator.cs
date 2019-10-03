﻿namespace Zilon.Core.Scoring
{
    /// <summary>
    /// Вспомогательный класс для объединения всех расчётов очков.
    /// </summary>
    public static class ScoreCalculator
    {
        public static DetailedLifetime ConvertTurnsToDetailed(int turns)
        {
            var minutesTotal = turns * 2;
            var hoursTotal = minutesTotal / 60f;
            var daysTotal = hoursTotal / 24f;

            var days = (int)daysTotal;

            // Замечание от lgtm
            // https://lgtm.com/rules/1506096756023/
            // Оба числа в скобках - float. Не видно причин, почему это плохо.
            // Зато анализатор не беспокоится.
            // Здесь не может быть переполнения, потому что все эти числа получены из turns,
            // который является int. И дни, и часы являются результатом деления целочисленного turns.

            var hours = (int)(hoursTotal - (float)days * 24);

            return new DetailedLifetime(days, hours);

        }
    }

    /// <summary>
    /// Структура для хранения разобранного значения времени жизни персонажа.
    /// </summary>
    public sealed class DetailedLifetime
    {
        /// <summary>
        /// Конструктор объекта.
        /// </summary>
        /// <param name="days"> Количество целых прожитых дней. </param>
        /// <param name="hours"> Количество целых пожитых часов. </param>
        public DetailedLifetime(int days, int hours)
        {
            Days = days;
            Hours = hours;
        }

        /// <summary>
        /// Количество целых прожитых дней.
        /// </summary>
        public int Days { get; }

        /// <summary>
        /// Количество целых пожитых часов.
        /// </summary>
        public int Hours { get; }
    }
}
