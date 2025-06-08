using System;
using UnityEngine;

namespace Assets.Scripts.CustomTypes
{
    public interface IUpgradeableStat
    {
        public bool CanBeUpgraded { get; }

        public bool IsSubstractModeOn { get; }

        public void Upgrade(float upgradeValue);

        public float GetUpgradeValueBasedOnUpdateRange();

        public event EventHandler OnUpgrade;
    }

    [Serializable]
    public abstract class UpgradeableStat<T> : IUpgradeableStat, ISerializationCallbackReceiver
        where T : struct, IComparable<T>, IConvertible
    {
        [field: SerializeField] public bool IsSubstractModeOn { get; protected set; }
        [field: SerializeField] public T Value { get; protected set; }
        [SerializeField] protected bool _alwaysUseMinValueForUpgrade;
        public bool CanBeUpgraded { get; protected set; } = true;
        public ValueRange<T> MinMaxRange { get; protected set; }
        protected ValueRange<T> _rangeOfPossibleValuesForUpgrade;

        public event EventHandler OnUpgrade;

        public UpgradeableStat(
            T value,
            bool alwaysUseMinValueForUpgrade = false)
        {
            Value = value;
            _alwaysUseMinValueForUpgrade = alwaysUseMinValueForUpgrade;
        }

        public UpgradeableStat(
            T value,
            ValueRange<T> minMaxRange,
            ValueRange<T> rangeOfPossibleValuesForUpgrade,
            bool alwaysUseMinValueForUpgrade = false) : this(value, alwaysUseMinValueForUpgrade)
        {
            MinMaxRange = minMaxRange;
            _rangeOfPossibleValuesForUpgrade = rangeOfPossibleValuesForUpgrade;
        }

        public virtual void Upgrade(float upgradeValue)
        {
            if (!CanBeUpgraded)
            {
                return;
            }

            float value = Convert.ToSingle(Value);
            float minValue = Convert.ToSingle(MinMaxRange.Min);
            float maxValue = Convert.ToSingle(MinMaxRange.Max);

            float delta = _alwaysUseMinValueForUpgrade ? minValue : upgradeValue;
            delta = IsSubstractModeOn ? -delta : delta;

            float newValue = value + delta;

            Debug.Log("Value: " + value + "DELTA: " + delta + "NEW VAL " + newValue);

            if ((IsSubstractModeOn && newValue <= maxValue)
                || (!IsSubstractModeOn && newValue >= maxValue))
            {
                newValue = maxValue;
                CanBeUpgraded = false;
            }

            Value = FromFloatToType(newValue);

            OnUpgrade?.Invoke(this, EventArgs.Empty);
        }

        public float GetUpgradeValueBasedOnUpdateRange()
        {
            return Convert.ToSingle(_rangeOfPossibleValuesForUpgrade.GetRandomValueInRange());
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
        [SerializeField] private FloatValueRange _floatMinMaxRange;
        [SerializeField] private FloatValueRange _floatRangeOfPossibleValuesForUpgrade;

        public FloatUpgradeableStat(float value, float maxValue, FloatValueRange minMaxRange, FloatValueRange rangeOfPossibleValuesForUpgrade, bool alwaysUseMinValueForUpgrade = false)
            : base(value, minMaxRange, rangeOfPossibleValuesForUpgrade, alwaysUseMinValueForUpgrade)
        {
        }

        public FloatUpgradeableStat(float value, float maxValue, bool alwaysUseMinValueForUpgrade = false)
            : base(value, alwaysUseMinValueForUpgrade)
        {
            MinMaxRange = _floatMinMaxRange;
            _rangeOfPossibleValuesForUpgrade = _floatRangeOfPossibleValuesForUpgrade;
        }

        public override void OnAfterDeserialize()
        {
            MinMaxRange = _floatMinMaxRange;
            _rangeOfPossibleValuesForUpgrade = _floatRangeOfPossibleValuesForUpgrade;
            CanBeUpgraded = true;
        }

        public override void OnBeforeSerialize()
        {
        }
    }

    [Serializable]
    public class ByteUpgradeableStat : UpgradeableStat<byte>
    {
        [SerializeField] private ByteValueRange _byteMinMaxRange;
        [SerializeField] private ByteValueRange _byteRangeOfPossibleValuesForUpgrade;

        public ByteUpgradeableStat(byte value, byte maxValue, ByteValueRange minMaxRange, ByteValueRange rangeOfPossibleValuesForUpgrade, bool alwaysUseMinValueForUpgrade = false)
            : base(value, minMaxRange, rangeOfPossibleValuesForUpgrade, alwaysUseMinValueForUpgrade)
        {
        }

        public ByteUpgradeableStat(byte value, byte maxValue, bool alwaysUseMinValueForUpgrade = false)
            : base(value, alwaysUseMinValueForUpgrade)
        {
            MinMaxRange = _byteMinMaxRange;
            _rangeOfPossibleValuesForUpgrade = _byteRangeOfPossibleValuesForUpgrade;
        }

        public override void OnAfterDeserialize()
        {
            MinMaxRange = _byteMinMaxRange;
            _rangeOfPossibleValuesForUpgrade = _byteRangeOfPossibleValuesForUpgrade;
            CanBeUpgraded = true;
        }

        public override void OnBeforeSerialize()
        {
        }
    }
}
