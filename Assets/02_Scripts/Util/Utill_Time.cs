using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public partial class Utill
{
    static protected readonly DateTime UTC = new DateTime(1970, 1, 1, 0, 0, 0, 0);
    static public DateTime TickToUTCDateTime(long tick)
    {
        return UTC.AddSeconds(tick);
    }

    static public DateTime DateTimeToToDay(System.DateTime time)
    {
        return new DateTime(time.Year, time.Month, time.Day, 0, 0, 0, 0);
    }

    static public long TickToSecond(long tick)
    {
        return tick / TimeSpan.TicksPerSecond;
    }

    static public TimeSpan DateTimeToUTCNow(System.DateTime time)
    {
        return time - UTC;
    }
}