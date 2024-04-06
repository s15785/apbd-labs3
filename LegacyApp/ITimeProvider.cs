using System;

namespace LegacyApp;

public interface ITimeProvider
{
    DateTime NowDateTime();
}

public class DefaultTimeProvider: ITimeProvider {
    public DateTime NowDateTime()
    {
        return DateTime.Now;
    }
}