using System;
using UnityEngine;

namespace Assets.Scripts.CustomTypes
{
    public interface IUpgradeableStat
    {
        public void Upgrade(float upgradeValue);

        public float GetUpgradeValueBasedOnUpdateRange();
    }

    public abstract class UpgradeableStat<T> : IUpgradeableStat, ISerializationCallbackReceiver
        where T : struct, IComparable<T>, IConvertible
    {
        [field: SerializeField] public T Value { get; protected set; }
        [SerializeField] protected T _maxValue;
        [SerializeField] protected bool _alwaysUseMinValueForUpgrade;
        [SerializeField] protected bool _substractMode;
        protected ValueRange<T> _rangeOfPossibleValuesForUpgrade;

        public UpgradeableStat(
            T value,
            T maxValue,
            bool alwaysUseMinValueForUpgrade = false)
        {
            Value = value;
            _maxValue = maxValue;
            _alwaysUseMinValueForUpgrade = alwaysUseMinValueForUpgrade;
        }

        public UpgradeableStat(
            T value,
            T maxValue,
            ValueRange<T> rangeOfPossibleValuesForUpgrade,
            bool alwaysUseMinValueForUpgrade = false) : this(value, maxValue, alwaysUseMinValueForUpgrade)
        {
            _rangeOfPossibleValuesForUpgrade = rangeOfPossibleValuesForUpgrade;
        }

        public virtual void Upgrade(float upgradeValue)
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
                if (value + upgradeValue <= maxValue)
                {
                    Value = FromFloatToType(value + upgradeValue);
                }
                else
                {
                    Value = FromFloatToType(value + minValue);
                }
            }
        }

        public float GetUpgradeValueBasedOnUpdateRange()
        {
            return (float)(object)_rangeOfPossibleValuesForUpgrade.GetRandomValueInRange();
        }

        private float FromTypeToFloat(T value)
        {
            return Convert.ToSingle(value);
        }

        private T FromFloatToType(float value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public abstract void OnBeforeSerialize();

        public abstract void OnAfterDeserialize();
    }

    // For unity serialization we need to use nongeneric class.

    [Serializable]
    public class FloatUpgradeableStat : UpgradeableStat<float>
    {
        [SerializeField] private FloatValueRange _floatRangeOfPossibleValuesForUpgrade;

        public FloatUpgradeableStat(float value, float maxValue, FloatValueRange rangeOfPossibleValuesForUpgrade, bool alwaysUseMinValueForUpgrade = false)
            : base(value, maxValue, rangeOfPossibleValuesForUpgrade, alwaysUseMinValueForUpgrade)
        {
        }

        public FloatUpgradeableStat(float value, float maxValue, bool alwaysUseMinValueForUpgrade = false)
            : base(value, maxValue, alwaysUseMinValueForUpgrade)
        {
            _rangeOfPossibleValuesForUpgrade = _floatRangeOfPossibleValuesForUpgrade;
        }

        public override void OnAfterDeserialize()
        {
            _rangeOfPossibleValuesForUpgrade = _floatRangeOfPossibleValuesForUpgrade;
        }

        public override void OnBeforeSerialize()
        {
        }
    }

    [Serializable]
    public class ByteUpgradeableStat : UpgradeableStat<byte>
    {
        [SerializeField] private ByteValueRange _byteRangeOfPossibleValuesForUpgrade;

        public ByteUpgradeableStat(byte value, byte maxValue, ByteValueRange rangeOfPossibleValuesForUpgrade, bool alwaysUseMinValueForUpgrade = false)
            : base(value, maxValue, rangeOfPossibleValuesForUpgrade, alwaysUseMinValueForUpgrade)
        {
        }

        public ByteUpgradeableStat(byte value, byte maxValue, bool alwaysUseMinValueForUpgrade = false)
            : base(value, maxValue, alwaysUseMinValueForUpgrade)
        {
            _rangeOfPossibleValuesForUpgrade = _byteRangeOfPossibleValuesForUpgrade;
        }

        public override void OnAfterDeserialize()
        {
            _rangeOfPossibleValuesForUpgrade = _byteRangeOfPossibleValuesForUpgrade;
        }

        public override void OnBeforeSerialize()
        {
        }
    }
}
