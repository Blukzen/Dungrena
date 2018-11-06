using UnityEngine;
using System.Collections;

public class PlayerSensor : MonoBehaviour 
{
    public LayerMask sightBlock;
    public LayerMask attackBlock;

    public ISearcher searcher;
    public float viewDistance = 10;

    private GameObject player;

    private bool playerWasInRange = false; // Player was in range last raycast
    private bool canSeePlayer = false;
    private bool canAttackPlayer = false;

    private void Start() 
    {
        searcher = GetComponentInParent<ISearcher>();
        player = GameManager.player == null ? null : GameManager.player.gameObject;
    }

    private void Update() 
    {
        // If player is null keep checking the GameManager for the player
        if (player == null)
        {
            Debug.LogWarning("[PlayerSensor] " + "No player game object");
            player = GameManager.player == null ? null : GameManager.player.gameObject;
            return;
        }

        if (Vector2.Distance(player.transform.position, transform.position) < viewDistance || playerWasInRange)
        {
            raycastCanSeePlayer();
            raycastCanAttackPlayer();

            searcher.canSeeTarget(canSeePlayer);
            searcher.canAttackTarget(canAttackPlayer);
        }
    }

    private void raycastCanSeePlayer()     
    {
        var raycastDir = player.transform.position - transform.position;
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, raycastDir, viewDistance, sightBlock);

        if (raycast.collider == null) {
            canSeePlayer = false;
            playerWasInRange = false;
            return;
        }

        if (raycast.collider.gameObject.tag == "Player")
        {
            canSeePlayer = true;
            playerWasInRange = true;
        }
        else
        {
            canSeePlayer = false;
            playerWasInRange = false;
        }
    }

    private void raycastCanAttackPlayer()
    {
        var raycastDir = player.transform.position - transform.position;
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, raycastDir, viewDistance, attackBlock);

        if (raycast.collider == null)
        {
            canAttackPlayer = false;
            playerWasInRange = false;
            return;
        }

        if (raycast.collider.gameObject.tag == "Player")
        {
            Debug.DrawRay(transform.position, raycast.point);
            canAttackPlayer = true;
            playerWasInRange = true;
        }
        else
        {
            canAttackPlayer = false;
            playerWasInRange = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }
}
