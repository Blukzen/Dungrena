using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class AbstractEntity : MonoBehaviour, IDamageable
{

    [Header("Physics Values")]
    public int maxSpeed = 7;
    public float acceleration = 1;
    public float friction = 1.9f;

    protected Vector2 moveDirection;

    [Header("Player Stats")]
    [SerializeField]
    private int maxHealth;
    private int health;

    private float pickUpRange = 1.5f;
    private Vector3 pickUpOffset = new Vector2(0, 0.5f);

    protected Rigidbody2D rb2d;
    private Collider2D collider2d;

    public LayerMask layerMask;

    private void Awake()
    {
        health = maxHealth;
        rb2d = GetComponent<Rigidbody2D>();
    }

    public virtual void UpdatePhysics()
    {

        // TODO: friction and acceleration multipliers
        var _friction = friction;
        var _acceleration = acceleration;

        // Velocity updated
        Vector2 newVel = rb2d.velocity;

        // Movement

        // Left
        if (moveDirection.x < 0)
        {
            // Apply acceleration left
            if (newVel.x > 0)
                newVel.x = Approach(newVel.x, 0, _friction);
            newVel.x = Approach(newVel.x, -maxSpeed, _acceleration);
        }

        // Right
        if (moveDirection.x > 0)
        {
            // Apply acceleration right
            if (newVel.x < 0)
                newVel.x = Approach(newVel.x, 0, _friction);
            newVel.x = Approach(newVel.x, maxSpeed, _acceleration);
        }

        // Down
        if (moveDirection.y < 0)
        {
            // Apply acceleration down
            if (newVel.y > 0)
                newVel.y = Approach(newVel.y, 0, _friction);
            newVel.y = Approach(newVel.y, -maxSpeed, _acceleration);
        }

        // Up
        if (moveDirection.y > 0)
        {
            // Apply acceleration up
            if (newVel.y < 0)
                newVel.y = Approach(newVel.y, 0, _friction);
            newVel.y = Approach(newVel.y, maxSpeed, _friction);
        }

        // Friction when not moving
        if (moveDirection.x == 0)
            newVel.x = Approach(newVel.x, 0, _friction);
        if (moveDirection.y == 0)
            newVel.y = Approach(newVel.y, 0, _friction);

        rb2d.velocity = newVel;
    }

    // TODO: Handle death event.
    public void Damage(int amount)
    {
        health -= amount;
    }

    // Set movement input
    public void SetMovement(Vector2 direction)
    {
        moveDirection = direction;
    }

    // For adding extra velocity without moveSpeed bounds to rb2d
    public void AddVelocity(Vector2 direction, float speed)
    {
        rb2d.velocity += (direction.normalized * speed);
    }

    // For adding force to rb2d
    public void AddForce(Vector2 direction, float strength) {
        rb2d.AddForce(direction.normalized * strength);
    }

    // Approach end from start at an increment of shift
    public float Approach(float start, float end, float shift)
    {
        float val = 0;

        if (start < end)
        {
            val = Mathf.Min(start + shift, end);
        }
        else
        {
            val = Mathf.Max(start - shift, end);
        }

        return val;
    }

    // Pick up 
    public void pickupItem()
    {
        IPickupable itemToPickup = null;

        // Search for nearby items
        Collider2D[] items = Physics2D.OverlapPointAll(Input.mousePosition, layerMask);

        // No items
        if (items.Length == 0)
            return;

        // Find eligible item
        foreach (var item in items)
        {
            float distance = Vector2.Distance(transform.position, item.transform.position);

            if (distance > pickUpRange)
                continue;

            itemToPickup = item.GetComponent<IPickupable>();
        }

        // Pickup closes
        itemToPickup.pickup(this);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + pickUpOffset, pickUpRange);
    }
}
