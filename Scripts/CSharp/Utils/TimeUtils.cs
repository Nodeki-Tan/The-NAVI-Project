using System;
using System.Diagnostics;

public class TimeUtils {
    
    private static readonly DateTime Jan1st1970 = new DateTime
    (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static long CurrentTimeMillis()
    {
        return (long) (DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
    }

    public static long GetNanoseconds()
	{
		double timestamp = Stopwatch.GetTimestamp();
		double nanoseconds = 1_000_000_000.0 * timestamp / Stopwatch.Frequency;

		return (long)nanoseconds;
	}

}

