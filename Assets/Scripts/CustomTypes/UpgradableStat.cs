using System;
using UnityEngine;

namespace Assets.Scripts.CustomTypes
{
    public interface IUpgradableStat<T>
        where T : struct, IComparable<T>, IConvertible
    {
        public void UpgradeValueBasedOnUpdateRange();

        public void UpdateRangeOfPossibleValuesForUpgradeBasedOnScalar(T scalar);
    }

    public abstract class UpgradableStat<T> : IUpgradableStat<T>
        where T : struct, IComparable<T>, IConvertible
    {
        [field: SerializeField] public T Value { get; protected set; }
        [SerializeField] protected T _maxValue;
        [SerializeField] protected bool _willRangeOfPossibleValuesChange = true;
        [SerializeField] protected bool _alwaysUseMinValueForUpgrade;
        [SerializeField] protected bool _substractMode;
        protected ValueRange<T> _rangeOfPossibleValuesForUpgrade;

        public UpgradableStat(
            T value,
            T maxValue,
            bool willRangeOfPossibleValuesChange = true,
            bool alwaysUseMinValueForUpgrade = false)
        {
            Value = value;
            _maxValue = maxValue;
            _willRangeOfPossibleValuesChange = willRangeOfPossibleValuesChange;
            _alwaysUseMinValueForUpgrade = alwaysUseMinValueForUpgrade;
        }

        public UpgradableStat(
            T value,
            T maxValue,
            ValueRange<T> rangeOfPossibleValuesForUpgrade,
            bool willRangeOfPossibleValuesChange = true,
            bool alwaysUseMinValueForUpgrade = false) : this(value, maxValue, willRangeOfPossibleValuesChange, alwaysUseMinValueForUpgrade)
        {
            _rangeOfPossibleValuesForUpgrade = rangeOfPossibleValuesForUpgrade;
        }

        public virtual void UpgradeValueBasedOnUpdateRange()
        {
            float value = FromTypeToFloat(Value);
            float minValue = FromTypeToFloat(_rangeOfPossibleValuesForUpgrade.Min);
            float maxValue = FromTypeToFloat(_rangeOfPossibleValuesForUpgrade.Max);

            if (_substractMode)
            {
                minValue = -minValue;
                maxValue = -maxValue;

                if (value + minValue < 0)
                {
                    return;
                }
            }
            else if (value + minValue > maxValue)
            {
                return;
            }

            if (_alwaysUseMinValueForUpgrade)
            {
                Value = FromFloatToType(value + minValue);
            }
            else
            {
                float randomValueFromUpgradeRange = (float)(object)_rangeOfPossibleValuesForUpgrade.GetRandomValueInRange();
                if (value + randomValueFromUpgradeRange <= maxValue)
                {
                    Value = FromFloatToType(value + randomValueFromUpgradeRange);
                }
                else
                {
                    Value = FromFloatToType(value + minValue);
                }
            }
        }

        public virtual void UpdateRangeOfPossibleValuesForUpgradeBasedOnScalar(T scalar)
        {
            if (_willRangeOfPossibleValuesChange)
            {
                float min = FromTypeToFloat(_rangeOfPossibleValuesForUpgrade.Min);
                float max = FromTypeToFloat(_rangeOfPossibleValuesForUpgrade.Max);
                float convertedScalar = FromTypeToFloat(scalar);

                _rangeOfPossibleValuesForUpgrade.Min = FromFloatToType(min + convertedScalar);
                _rangeOfPossibleValuesForUpgrade.Max = FromFloatToType(max + convertedScalar);
            }
        }

        private float FromTypeToFloat(T value)
        {
            return Convert.ToSingle(value);
        }

        private T FromFloatToType(float value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }

    // For unity serialization we need to use nongeneric class.

    [Serializable]
    public class FloatUpgradableStat : UpgradableStat<float>
    {
        [SerializeField] private FloatValueRange _floatRangeOfPossibleValuesForUpgrade;

        public FloatUpgradableStat(float value, float maxValue, FloatValueRange rangeOfPossibleValuesForUpgrade, bool willRangeOfPossibleValuesChange = true, bool alwaysUseMinValueForUpgrade = false)
            : base(value, maxValue, rangeOfPossibleValuesForUpgrade, willRangeOfPossibleValuesChange, alwaysUseMinValueForUpgrade)
        {
        }

        public FloatUpgradableStat(float value, float maxValue, bool willRangeOfPossibleValuesChange = true, bool alwaysUseMinValueForUpgrade = false)
            : base(value, maxValue, willRangeOfPossibleValuesChange, alwaysUseMinValueForUpgrade)
        {
            _rangeOfPossibleValuesForUpgrade = _floatRangeOfPossibleValuesForUpgrade;
        }
    }

    [Serializable]
    public class ByteUpgradableStat : UpgradableStat<byte>
    {
        [SerializeField] private ByteValueRange _byteRangeOfPossibleValuesForUpgrade;

        public ByteUpgradableStat(byte value, byte maxValue, ByteValueRange rangeOfPossibleValuesForUpgrade, bool willRangeOfPossibleValuesChange = true, bool alwaysUseMinValueForUpgrade = false)
            : base(value, maxValue, rangeOfPossibleValuesForUpgrade, willRangeOfPossibleValuesChange, alwaysUseMinValueForUpgrade)
        {
        }

        public ByteUpgradableStat(byte value, byte maxValue, bool willRangeOfPossibleValuesChange = true, bool alwaysUseMinValueForUpgrade = false)
            : base(value, maxValue, willRangeOfPossibleValuesChange, alwaysUseMinValueForUpgrade)
        {
            _rangeOfPossibleValuesForUpgrade = _byteRangeOfPossibleValuesForUpgrade;
        }
    }
}
