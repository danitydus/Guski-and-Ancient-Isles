using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CannonLaunch : MonoBehaviour
{
    public KeyCode launchKey = KeyCode.L;
    public List<Transform> targetTransforms;
    public GameObject textCanon;
    public GameObject body;
    public GameObject limits;
    public CharacterController characterController;
    public float launchSpeed = 10f;
    public float arcHeight = 5f;
    public AudioSource launchSound;
    public Animator animator;
    public ParticleSystem launchEffect;

    private bool isLaunched = false;
    private bool canLaunch = false;
    private bool isVerticalLaunch = false; // Nuevo: para cañones hacia arriba
    private Vector3 startPosition;
    private float launchProgress = 0f;
    private Transform currentTarget;

    void Start()
    {
        if (textCanon != null)
        {
            textCanon.SetActive(false);
        }
    }

    void Update()
    {
        if (canLaunch && Input.GetKeyDown(launchKey) && !isLaunched)
        {
            StartCoroutine(PrepareLaunch());
        }

        if (isLaunched)
        {
            if (isVerticalLaunch)
            {
                MoveVertically();
            }
            else
            {
                MoveInArc();
            }
        }
    }

    IEnumerator PrepareLaunch()
    {
        if (body != null) body.SetActive(false);
        if (limits != null) limits.SetActive(false);
        if (textCanon != null) textCanon.SetActive(false);

        yield return new WaitForSeconds(2f);

        if (body != null) body.SetActive(true);
        if (launchSound != null) launchSound.Play();
        if (animator != null) animator.SetBool("InCanon", true);
        if (launchEffect != null)
        {
            launchEffect.transform.position = transform.position;
            launchEffect.Play();
        }

        startPosition = transform.position;
        launchProgress = 0f;
        isLaunched = true;
    }

    void MoveInArc()
    {
        if (currentTarget != null)
        {
            launchProgress += launchSpeed * Time.deltaTime;
            launchProgress = Mathf.Clamp01(launchProgress);

            Vector3 nextPosition = Vector3.Lerp(startPosition, currentTarget.position, launchProgress);
            nextPosition.y += Mathf.Sin(launchProgress * Mathf.PI) * arcHeight;

            transform.position = nextPosition;

            if (launchProgress >= 1f)
            {
                ArriveAtDestination();
            }
        }
    }

    void MoveVertically()
    {
        launchProgress += launchSpeed * Time.deltaTime;
        float verticalTime = Mathf.Clamp01(launchProgress);
        float height = Mathf.Sin(verticalTime * Mathf.PI) * arcHeight * 2f; // Ajuste de altura total

        Vector3 nextPosition = startPosition + new Vector3(0, height, 0);
        transform.position = nextPosition;

        if (verticalTime >= 1f)
        {
            ArriveAtDestination();
        }
    }

    void ArriveAtDestination()
    {
        isLaunched = false;

        if (characterController != null)
        {
            characterController.enabled = true;
        }

        if (limits != null)
        {
            limits.SetActive(true);
        }

        if (animator != null)
        {
            animator.SetBool("InCanon", false);
        }

        isVerticalLaunch = false; // Reset
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Canon"))
        {
            canLaunch = true;
            if (textCanon != null)
            {
                textCanon.SetActive(true);
            }

            string cannonName = other.gameObject.name;

            if (cannonName == "CanonUp")
            {
                isVerticalLaunch = true;
                currentTarget = null;
            }
            else
            {
                isVerticalLaunch = false;
                int cannonIndex = GetCanonIndex(cannonName);
                if (cannonIndex >= 0 && cannonIndex < targetTransforms.Count)
                {
                    currentTarget = targetTransforms[cannonIndex];
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Canon"))
        {
            canLaunch = false;
            if (textCanon != null)
            {
                textCanon.SetActive(false);
            }
        }
    }

    int GetCanonIndex(string cannonName)
    {
        if (string.IsNullOrEmpty(cannonName)) return -1;
        if (cannonName.StartsWith("Canon"))
        {
            string numberPart = cannonName.Substring(5);
            int canonIndex;
            if (int.TryParse(numberPart, out canonIndex))
            {
                return canonIndex - 1;
            }
        }
        return -1;
    }
}
