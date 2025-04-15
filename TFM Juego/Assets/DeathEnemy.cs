using UnityEngine;

public class DeathEnemy : MonoBehaviour
{
    public GameObject effectDeath;
    public AudioSource enemyDeath;
    public GameObject player;
    public float extraJumpForce = 5f;
    public float gravity = -9.8f;
    public CubeMovement cubeMovement;

    public float knockbackForce = 30f; // Fuerza del empujón
    private Rigidbody rb;
    private bool isDying = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.isKinematic = true;
        rb.useGravity = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDying) return;

        if (other.CompareTag("PiesPlayer") && !cubeMovement.isInvulnerable)
        {
            TriggerDeathEffect();
            cubeMovement.AddExtraJump(extraJumpForce);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player") && cubeMovement.isAttacking && !cubeMovement.isInvulnerable)
        {
            isDying = true;


            // Activar física
            rb.isKinematic = false;
            rb.useGravity = true;

            // Dirección contraria al jugador
            Vector3 directionAway = (transform.position - player.transform.position).normalized + Vector3.up * 0.5f;
            rb.AddForce(directionAway * knockbackForce, ForceMode.Impulse);

            // Destruir después de un corto tiempo para que el empujón se vea
            Invoke(nameof(DestroyEnemy), 0.3f);
        }
    }

    private void TriggerDeathEffect()
    {
        if (effectDeath != null)
        {
            GameObject deathEffect = Instantiate(effectDeath, transform.position, Quaternion.identity);
            deathEffect.transform.localScale *= 2f;
        }

        if (enemyDeath != null)
        {
            enemyDeath.Play();
        }
    }

    private void DestroyEnemy()
    {
        TriggerDeathEffect();
        Destroy(gameObject);
    }
}
