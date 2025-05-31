using System;
using UnityEngine;

namespace Assets.Scripts.Exp
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
            return $"LevelData(Level: {Lvl}, Exp: {Exp}, MaxExp: {MaxExp})";
        }
    }

    public class ExpDataEventArgs : EventArgs
    {
        public LevelData ExpData { get; set; }
    }

    public class LevelSystem : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _expCurve;

        public LevelData ExpData { get; private set; } = new LevelData();

        public static LevelSystem Instance { get; private set; }

        public event EventHandler<ExpDataEventArgs> OnLvlUp;

        public event EventHandler<ExpDataEventArgs> OnExpChange;

        private void Awake()
        {
            ExpData = new LevelData(maxExp: CalculateMaxExp(ExpData.Lvl));
        }

        public void AddExp(float value)
        {
            if (ExpData.Lvl == byte.MaxValue)
            {
                return;
            }

            (byte lvl, float exp, float maxExp) = ExpData;
            byte prevLvl = lvl;
            exp += value;

            while (lvl < byte.MaxValue && exp >= maxExp)
            {
                lvl++;
                exp -= maxExp;
                maxExp = CalculateMaxExp(lvl);

                ExpData = new LevelData(lvl, exp, maxExp);
                OnLvlUp?.Invoke(this, new ExpDataEventArgs()
                {
                    ExpData = ExpData
                });
            }

            ExpData = new LevelData(lvl, exp, maxExp);
            OnExpChange?.Invoke(this, new ExpDataEventArgs()
            {
                ExpData = ExpData
            });
        }

        private float CalculateMaxExp(byte level)
            => _expCurve.Evaluate(level);
    }
}
