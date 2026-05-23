using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public float fallbackWinRadius = 1.25f;

    private Transform player;
    private bool triggered;

    private void Start()
    {
        PlayerController playerController = Object.FindAnyObjectByType<PlayerController>();
        if (playerController != null)
        {
            player = playerController.transform;
        }
    }

    private void Update()
    {
        if (!triggered && player != null && Vector3.Distance(transform.position, player.position) <= fallbackWinRadius)
        {
            TriggerWin(player.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TriggerWin(other.gameObject);
    }

    private void TriggerWin(GameObject other)
    {
        if (triggered || other.GetComponentInParent<PlayerController>() == null)
        {
            return;
        }

        triggered = true;
        GameStateController.Instance?.PlayerWon();
    }
}
