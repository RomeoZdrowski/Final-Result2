using UnityEngine;

public class BossController : MonoBehaviour
{
    private enum BossState
    {
        Flying,
        Landing,
        Grounded,
        Takeoff
    }

    [Header("References")]
    [SerializeField] private Health health;
    [SerializeField] private Animator anim;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Transform[] flyPoints;
    [SerializeField] private Transform groundPoint;

    [Header("Timers")]
    [SerializeField] private float flyDuration = 10f;
    [SerializeField] private float groundDuration = 5f;

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;

    private BossState state = BossState.Flying;

    private int currentPoint;
    private float stateTimer;
    private float attackTimer;
    private bool dead;

    private void Update()
    {
        if (dead)
            return;

        if (health.currentHealth <= 0)
        {
            Die();
            return;
        }

        switch (state)
        {
            case BossState.Flying:
                FlyingState();
                break;

            case BossState.Landing:
                LandingState();
                break;

            case BossState.Grounded:
                GroundedState();
                break;

            case BossState.Takeoff:
                TakeoffState();
                break;
        }
    }

    private void FlyingState()
    {
        stateTimer += Time.deltaTime;

        MoveBetweenPoints();

        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0;
            anim.SetTrigger("attack");
        }

        if (stateTimer >= flyDuration)
        {
            stateTimer = 0;
            state = BossState.Landing;
        }
    }

    private void LandingState()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            groundPoint.position,
            moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, groundPoint.position) < 0.2f)
        {
            state = BossState.Grounded;
        }
    }

    private void GroundedState()
    {
        stateTimer += Time.deltaTime;

        if (stateTimer >= groundDuration)
        {
            stateTimer = 0;
            state = BossState.Takeoff;
        }
    }

    private void TakeoffState()
    {
        Transform target = flyPoints[0];

        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.2f)
        {
            state = BossState.Flying;
        }
    }

    private void MoveBetweenPoints()
    {
        Transform target = flyPoints[currentPoint];

        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.2f)
        {
            currentPoint++;

            if (currentPoint >= flyPoints.Length)
                currentPoint = 0;
        }
    }

    // Вызывается Animation Event из Attack
    public void ShootRadial()
    {
        int count = 8;
        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            GameObject fireball = GetFireball();

            fireball.transform.position = firePoint.position;

            float angle = i * angleStep;

            Vector2 dir = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad));

            fireball.GetComponent<BossProjectile>()
                .ActivateProjectile(dir);
        }
    }

    private GameObject GetFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return fireballs[i];
        }

        return fireballs[0];
    }

    private void Die()
    {
        dead = true;

        anim.SetTrigger("die");

        GetComponent<Collider2D>().enabled = false;

        enabled = false;
    }
}