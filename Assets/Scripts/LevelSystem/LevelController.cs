using System;
using UnityEngine;

namespace Assets.Scripts.LevelSystem
{
    public readonly struct LevelData
    {
        public readonly byte Lvl;
        public readonly float Exp;
        public readonly float MaxExp;

        public LevelData(byte lvl = 1, float exp = 0, float maxExp = float.MaxValue)
        {
            Lvl = lvl;
            Exp = exp;
            MaxExp = maxExp;
        }

        public void Deconstruct(out byte lvl, out float exp, out float maxExp)
        {
            lvl = Lvl;
            exp = Exp;
            maxExp = MaxExp;
        }

        public override string ToString()
        {
            return $"LevelData(DangerLevel: {Lvl}, Exp: {Exp}, MaxExp: {MaxExp})";
        }
    }

    public class LevelDataEventArgs : EventArgs
    {
        public LevelData ExpData { get; set; }
    }

    public interface ILevelController
    {
        public LevelData LevelData { get; }

        public event EventHandler<LevelDataEventArgs> OnExpChange;

        public event EventHandler<LevelDataEventArgs> OnLvlUp;

        public void AddExp(float value);
    }

    public class LevelController : MonoBehaviour, ILevelController
    {
        [SerializeField] private AnimationCurve _expCurve;

        public LevelData LevelData { get; private set; } = new LevelData();

        public event EventHandler<LevelDataEventArgs> OnLvlUp;

        public event EventHandler<LevelDataEventArgs> OnExpChange;

        private void Awake()
        {
            LevelData = new LevelData(maxExp: CalculateMaxExp(LevelData.Lvl));
        }

        public void AddExp(float value)
        {
            if (LevelData.Lvl == byte.MaxValue)
            {
                return;
            }

            (byte lvl, float exp, float maxExp) = LevelData;
            byte prevLvl = lvl;
            exp += value;

            while (lvl < byte.MaxValue && exp >= maxExp)
            {
                lvl++;
                exp -= maxExp;
                maxExp = CalculateMaxExp(lvl);

                LevelData = new LevelData(lvl, exp, maxExp);
                OnLvlUp?.Invoke(this, new LevelDataEventArgs()
                {
                    ExpData = LevelData
                });
            }

            LevelData = new LevelData(lvl, exp, maxExp);
            OnExpChange?.Invoke(this, new LevelDataEventArgs()
            {
                ExpData = LevelData
            });
        }

        private float CalculateMaxExp(byte level)
            => _expCurve.Evaluate(level);
    }
}
