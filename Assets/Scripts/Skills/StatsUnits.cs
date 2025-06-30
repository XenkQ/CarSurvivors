using System;

namespace Assets.Scripts.Skills
{
    public static class StatsUnitsExtensions
    {
        public static string ToDisplayString(this StatsUnits unit)
        {
            return unit switch
            {
                StatsUnits.None => "",
                StatsUnits.Percentage => "%",
                StatsUnits.Seconds => "s",
                StatsUnits.Meters => "m",
                _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
            };
        }
    }

    public enum StatsUnits
    {
        None,
        Percentage,
        Seconds,
        Meters,
    }
}
