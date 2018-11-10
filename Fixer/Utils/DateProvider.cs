using System;

namespace Fixer.Utils
{
    public class DateProvider : IDateProvider
    {
        public DateTime Today => DateTime.Today;
    }
}