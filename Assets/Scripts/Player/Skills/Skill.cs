using System;
using Assets.Scripts.Player;
using UnityEngine;

public abstract class Skill : MonoBehaviour, ISkill
{
    public abstract event EventHandler OnLevelUp;

    public virtual void Activate()
    {
        gameObject.SetActive(true);
    }

    public abstract void LevelUp();
}