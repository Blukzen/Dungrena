using UnityEngine;
using System.Collections;

public class PlayerSensor : MonoBehaviour 
{
    public LayerMask layerMask;
    public ISearcher searcher;

    private GameObject player;

    private bool playerInRange = false;

    private void Start() 
    {
        searcher = GetComponentInParent<ISearcher>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") 
        {
            if (player == null)
                player = collision.gameObject;

            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) 
    {
        if (collision.gameObject.tag == "Player")
            playerInRange = false;

    }

    private void Update() 
    {
        if (playerInRange)
            raycastToPlayer(player);
    }

    private void raycastToPlayer(GameObject player)     
    {
        if (player == null)
            return;

        var raycastDir = player.transform.position - transform.position;
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, raycastDir, GetComponent<Collider2D>().bounds.size.magnitude, layerMask);

        Debug.DrawRay(transform.position, raycastDir);

        if (raycast.collider == null) {
            searcher.canSeeTarget(false);
            return;
        }

        if (raycast.collider.gameObject.tag == "Player")
            searcher.canSeeTarget(true);
        else
            searcher.canSeeTarget(false);
    }
}
