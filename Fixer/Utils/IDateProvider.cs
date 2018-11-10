using System;

namespace Fixer.Utils
{
    public interface IDateProvider
    {
        DateTime Today { get; }
    }
}