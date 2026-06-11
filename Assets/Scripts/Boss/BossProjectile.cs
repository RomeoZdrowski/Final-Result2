using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BossProjectile : EnemyDamage
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float resetTime = 5f;

    private float lifetime;
    private bool hit;
    private Vector2 direction;

    private Animator anim;
    private BoxCollider2D coll;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }

    public void ActivateProjectile(Vector2 dir)
    {
        Debug.Log("Projectile Activated");

        direction = dir.normalized;

        hit = false;
        lifetime = 0f;

        Debug.Log("Before SetActive: " + gameObject.activeSelf);

        gameObject.SetActive(true);

        Debug.Log("After SetActive: " + gameObject.activeSelf);

        if (coll == null)
            coll = GetComponent<BoxCollider2D>();

        if (coll != null)
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
        hit = true;

        base.OnTriggerEnter2D(collision);

        if (coll != null)
            coll.enabled = false;

        gameObject.SetActive(false);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}