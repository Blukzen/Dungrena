using UnityEngine;
using System.Collections;

public class PlayerSensor : MonoBehaviour 
{
    public LayerMask layerMask;
    public ISearcher searcher;
    public float viewDistance = 10;

    private GameObject player;

    private bool playerWasInRange = false; // Player was in range last raycast

    private void Start() 
    {
        searcher = GetComponentInParent<ISearcher>();
        player = GameManager.player == null ? null : GameManager.player.gameObject;
    }

    private void Update() 
    {
        if (player == null)
            return;

        if (Vector2.Distance(player.transform.position, transform.position) < viewDistance || playerWasInRange)
        {
            raycastToPlayer(player);
        }
    }

    private void raycastToPlayer(GameObject player)     
    {
        var raycastDir = player.transform.position - transform.position;
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, raycastDir, viewDistance, layerMask);

        Debug.DrawRay(transform.position, raycastDir);

        if (raycast.collider == null) {
            searcher.canSeeTarget(false);
            playerWasInRange = false;
            return;
        }

        if (raycast.collider.gameObject.tag == "Player")
        {
            searcher.canSeeTarget(true);
            playerWasInRange = true;
        }
        else
        {
            searcher.canSeeTarget(false);
            playerWasInRange = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }
}
