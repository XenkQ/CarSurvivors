using System;
using UnityEngine;

public readonly struct ExpData
{
    public readonly byte Lvl;
    public readonly float Exp;
    public readonly float MaxExp;

    public ExpData(byte lvl = 1, float exp = 0, float maxExp = float.MaxValue)
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
}

public class ExpDataEventArgs : EventArgs
{
    public ExpData ExpData { get; set; }
}

public sealed class ExpManager : MonoBehaviour
{
    [SerializeField] private AnimationCurve _expCurve;

    public ExpData ExpData { get; private set; } = new ExpData();

    public static ExpManager Instance { get; private set; }

    public event EventHandler<ExpDataEventArgs> OnLvlUp;

    public event EventHandler<ExpDataEventArgs> OnExpChange;

    private ExpManager()
    { }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        ExpData = new ExpData(maxExp: CalculateMaxExp(ExpData.Lvl));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            AddExp(10f);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            AddExp(115f);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            AddExp(10000f);
        }
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

            ExpData = new ExpData(lvl, exp, maxExp);
            OnLvlUp?.Invoke(this, new ExpDataEventArgs()
            {
                ExpData = ExpData
            });
        }

        ExpData = new ExpData(lvl, exp + value, maxExp);
        OnExpChange?.Invoke(this, new ExpDataEventArgs()
        {
            ExpData = ExpData
        });
    }

    private float CalculateMaxExp(byte level)
        => _expCurve.Evaluate(level);
}
