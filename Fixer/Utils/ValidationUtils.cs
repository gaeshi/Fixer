namespace Fixer.Utils
{
    public static class ValidationUtils
    {
        public static int GetValueInBoundaries(int value, int min, int max)
        {
            return value < min ? min
                : value > max ? max
                : value;
        }
    }
}