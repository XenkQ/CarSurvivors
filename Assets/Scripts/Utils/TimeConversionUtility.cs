namespace Assets.Scripts.Utils
{
    public static class TimeConversionUtility
    {
        public static string FormatSecondsToTimeString(uint totalSeconds)
        {
            uint hours = totalSeconds / 3600;
            uint minutes = (totalSeconds % 3600) / 60;
            uint seconds = totalSeconds % 60;

            if (hours > 0)
                return $"{hours}h {minutes}m {seconds}s";
            if (minutes > 0)
                return $"{minutes}m {seconds}s";
            return $"{seconds}s";
        }
    }
}
