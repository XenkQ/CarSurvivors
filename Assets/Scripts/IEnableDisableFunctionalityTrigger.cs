namespace Assets.Scripts
{
    public interface IEnableDisableFunctionalityTrigger<T>
        where T : IEnableDisableFunctionalityTrigger<T>
    {
        void EnableFunctionality();

        void DisableFunctionality();
    }
}
