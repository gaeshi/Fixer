using Fixer.Utils;

namespace Fixer.Services
{
    public class TmdbDataRefresherServiceSettings
    {
        private const int MinValidUpdateTaskDelayMinutes = 1;

        private int _updateTaskDelayMinutes = MinValidUpdateTaskDelayMinutes;

        public int UpdateTaskDelayMinutes
        {
            get => _updateTaskDelayMinutes;
            set => _updateTaskDelayMinutes = GetValidUpdateTaskDelayMinutes(value);
        }

        private static int GetValidUpdateTaskDelayMinutes(int value)
        {
            return ValidationUtils.GetValueInBoundaries(value, min: MinValidUpdateTaskDelayMinutes, max: value);
        }
    }
}