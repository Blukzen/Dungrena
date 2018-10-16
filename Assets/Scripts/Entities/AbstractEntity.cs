using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class AbstractEntity : MonoBehaviour, IDamageable, IMoveable {

    [SerializeField]
    private int maxHealth;
    private int health;
    [SerializeField]
    private int moveSpeed;

    private Rigidbody2D rb2d;
    private Collider2D collider2d;

    private void Awake() {
        health = maxHealth;
        rb2d = GetComponent<Rigidbody2D>();
    }

    // TODO: Handle death event
    public void Damage(int amount) {
        health -= amount;
    }

    public void Move(Vector2 direction) {
        rb2d.velocity = (direction * moveSpeed);
    }

    public void AddForce(Vector2 direction, float strength) {
        rb2d.AddForce(direction.normalized * strength);
    }
}
