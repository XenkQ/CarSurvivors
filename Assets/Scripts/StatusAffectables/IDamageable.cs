﻿namespace Assets.Scripts.StatusAffectables
{
    public interface IDamageable
    {
        public void TakeDamage(float damage);

        public void TakeFullHpDamage();
    }
}
