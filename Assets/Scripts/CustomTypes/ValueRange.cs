using System;

namespace Assets.Scripts.CustomTypes
{
    public interface IValueRange<T>
        where T : struct, IComparable<T>, IConvertible
    {
        public T GetRandomValueInRange();
    }

    public class ValueRange<T> : IValueRange<T>
        where T : struct, IComparable<T>, IConvertible
    {
        public T Min;
        public T Max;

        public ValueRange(T min, T max)
        {
            Min = min;
            Max = max;
        }

        public T GetRandomValueInRange()
        {
            if (typeof(T) == typeof(float))
            {
                return (T)(object)UnityEngine.Random.Range((float)(object)Min, (float)(object)Max);
            }
            else if (typeof(T) == typeof(int)
                || typeof(T) == typeof(byte))
            {
                return (T)(object)UnityEngine.Random.Range((int)(object)Min, (int)(object)Max);
            }
            else
            {
                throw new InvalidOperationException("Unsupported type for random value generation.");
            }
        }
    }

    // For unity serialization we need to use nongeneric class.

    [Serializable]
    public class FloatValueRange : ValueRange<float>
    {
        public FloatValueRange(float min, float max) : base(min, max)
        {
        }
    }

    [Serializable]
    public class ByteValueRange : ValueRange<byte>
    {
        public ByteValueRange(byte min, byte max) : base(min, max)
        {
        }
    }
}
