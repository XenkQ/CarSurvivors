using System;

namespace Assets.Scripts.Settings.Resolution
{
[Serializable]
public struct SerializableResolution
{
    public int Width;
    public int Height;
    public uint RefreshRateNumerator;
    public uint RefreshRateDenominator;

    public SerializableResolution(int width, int height, uint numerator, uint denominator)
    {
        Width = width;
        Height = height;
        RefreshRateNumerator = numerator;
        RefreshRateDenominator = denominator;
    }

    public static SerializableResolution FromUnityResolution(UnityEngine.Resolution resolution)
    {
        return new SerializableResolution(
            resolution.width,
            resolution.height,
            resolution.refreshRateRatio.numerator,
            resolution.refreshRateRatio.denominator
        );
    }
}
}