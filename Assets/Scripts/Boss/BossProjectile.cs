using UnityEngine;

public class BossProjectile : EnemyDamage
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float resetTime = 5f;

    private float lifetime;
    private bool hit;
    private Vector2 direction;

    private Animator anim;
    private Collider2D coll;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }

    public void ActivateProjectile(Vector2 dir)
    {
        if (coll == null)
            coll = GetComponent<Collider2D>();

        direction = dir.normalized;
        hit = false;
        lifetime = 0f;

        gameObject.SetActive(true);
        coll.enabled = true;
    }

    private void Update()
    {
        if (hit)
            return;

        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        lifetime += Time.deltaTime;

        if (lifetime >= resetTime)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Игнорируем самого босса и других врагов
        if (collision.CompareTag("Enemy"))
            return;

        // Игнорируем другие снаряды босса
        if (collision.GetComponent<BossProjectile>() != null)
            return;

        // Урон наносим только игроку
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Health>()?.TakeDamage(damage);
            DeactivateProjectile();
            return;
        }

        // Если хочешь, чтобы снаряд исчезал от стен/земли:
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            DeactivateProjectile();
            return;
        }
    }

    private void DeactivateProjectile()
    {
        hit = true;

        if (coll != null)
            coll.enabled = false;

        gameObject.SetActive(false);
    }
}