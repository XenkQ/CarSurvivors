using Assets.Scripts;
using UnityEngine;

public sealed class PlayerDeathPresenter : MonoBehaviour
{
    [SerializeField] private GameObject visual;
    public static PlayerDeathPresenter Instace { get; private set; }

    private PlayerDeathPresenter()
    {
    }

    private void Awake()
    {
        if (Instace == null)
        {
            Instace = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EnableDeathScreen()
    {
        visual.SetActive(true);
        GameTime.PauseTime();
    }
}
