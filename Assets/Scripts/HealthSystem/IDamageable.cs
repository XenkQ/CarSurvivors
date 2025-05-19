namespace Assets.Scripts.HealthSystem
{
    public interface IDamageable : IHealthy
    {
        public void TakeDamage(float damage);
    }
}
