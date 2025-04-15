using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class CubeMovement : MonoBehaviour
{
    public float moveSpeed = 6f;     // Ya existente
    public float speedMove = 10f; 
    public float jumpForce = 18f;
    public float gravity = 9.8f;

    private CharacterController characterController;
    private float verticalVelocity = 0f;

    public GameObject Cam3D;
    public GameObject Cam2D;
    public GameObject Cam2D_2;
    public Camera mainCamera;

    public bool is3DView = false;
    public bool canChangeView = true;


    public TextMeshProUGUI coinCounterText;
    public int coinCount = 0;
    public int totalCoins = 30;

    public TextMeshProUGUI gemCounterText;
    public int gemCount = 0;
    public int totalGems = 30;

    public GameObject coinCounterContainer;
    private Coroutine visibilityCoroutine;
    public GameObject gemCounterContainer;

    public GameObject[] monedasTransparentesEn3D;
    public GameObject[] monedasTransparentesEn2D;

    private float transparentAlpha = 0.3f;
    private float opaqueAlpha = 1.0f;

    private Vector3 savedPosition;
    public Transform spawnTransform;
    public Vector3 spawn;

    public MonoBehaviour movimiento3D;

    private bool isTransitioning = false;
    public Animator animator;
    public bool isMoving = false;

    private Quaternion rightRotation = Quaternion.Euler(0, 118, 0);
    private Quaternion leftRotation = Quaternion.Euler(0, 230, 0);

    public bool isAttacking = false;
    public float attackDuration;
    private bool canCombo = false;
    public float comboTimeWindow ; // Tiempo para realizar el segundo ataque

    // Sistema de vidas
    public int lives = 3;
    public bool isInvulnerable = false;
    public float invulnerabilityDuration = 2f;

    // Corazones UI
    public RawImage heartPrefab; // El prefab debe estar asignado en el Inspector.
    public Transform heartsContainer; // Contenedor en la UI.
    public List<RawImage> heartInstances = new List<RawImage>();

    public AudioSource jumpAudio;
    public AudioSource movementAudio;
    public AudioSource attackAudio;
    public AudioSource attackAudio2;
    public AudioSource defenseAudio;
    public AudioSource hitAudio;
    public AudioSource deathAudio;
    public AudioSource coinSound;
    public AudioSource gemSound;
    public AudioSource changeView;
    public AudioSource cheackpointSound;

    public float currentNormalSpeed;
    public float accelerationRate ;
    private bool hasDoubleJumped = false;
    public bool isDefending;
    public bool inChangeZone;
    public CameraFollow2D camerafollow2d;
    public FreeLookCam freeLookCam3D;
    public bool in2D1;    
    public bool in2D2;
    public bool in3D;
    public bool isChangingView;
    public GameObject loadingPanel;
    public AudioSource mainSong;
    public AudioSource musicDeath;
    public GameObject cuerpo;
    public SaveManager saveManager;
    public TriggerMapa triggerMapa;
    public bool isGrounded;
    public bool dead;
    public float flashInterval = 0.1f;
    public ParticleSystem dustDeath;
    public GameObject paneles;
    void Start()
    {
        loadingPanel.SetActive(false);
        Cam2D_2.SetActive(false);
        in2D1 = true;
        spawn = spawnTransform.position;
        characterController = GetComponent<CharacterController>();
        SetView2D();
        gemCounterContainer.SetActive(false);
        UpdateCoinCounter();
        UpdateGemCounter();
        UpdateCoinVisibility();

        savedPosition = transform.position;
        InitializeHearts();
        if (movimiento3D != null)
        {
            movimiento3D.enabled = false;
        }

        transform.position = spawn;
        transform.rotation = rightRotation;

        if (animator != null)
        {
            animator.SetBool("Move", false);
        }


        // Inicializa los corazones según el número de vidas.

    }
    void InitializeHearts()
    {
        // Limpia corazones previos.
        foreach (RawImage heart in heartInstances)
        {
            Destroy(heart.gameObject);
        }
        heartInstances.Clear();

        // Crea nuevos corazones.
        float offsetX = 80f; // Distancia reducida entre cada corazón.
        float initialPosX = -860f; // La posición X del primer corazón.

        for (int i = 0; i < lives; i++)
        {
            RawImage newHeart = Instantiate(heartPrefab, heartsContainer);
            newHeart.rectTransform.localScale = new Vector3(0.8f, 0.9f, 0.5f); // Escala de 0.5 en todos los ejes.

            // Ajusta la posición X con menos distancia entre corazones.
            float posX = initialPosX + (i * offsetX);
            newHeart.rectTransform.anchoredPosition = new Vector2(posX, newHeart.rectTransform.anchoredPosition.y);

            // Asegura que el objeto sea visible (activo).
            newHeart.gameObject.SetActive(true);

            heartInstances.Add(newHeart);
        }
    }



    private IEnumerator RotateCamera(Transform cam, Quaternion targetRotation, float duration)
    {
        Quaternion startRotation = cam.rotation;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, time / duration); // Suaviza la transición
            cam.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        cam.rotation = targetRotation; // Asegura que termine exactamente en la rotación deseada
    }

    void Update()
    {
         
        if (characterController.isGrounded)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        if (in2D1)
        {
            StartCoroutine(RotateCamera(Cam2D.transform, Quaternion.Euler(0, 0, 0), 0.4f));
            Cam2D_2.transform.rotation = Cam2D.transform.rotation;
            Cam2D_2.transform.position = Cam2D.transform.position;
        }
        else if (in2D2)
        {
            StartCoroutine(RotateCamera(Cam2D_2.transform, Quaternion.Euler(0, -90, 0), 0.4f));
            Cam2D.transform.rotation = Cam2D_2.transform.rotation;
            Cam2D.transform.position = Cam2D_2.transform.position;
        }
        if (characterController.isGrounded)
        {
            animator.SetBool("Ground", true);
        }else
        {
            animator.SetBool("Ground", false);

        }
        if (!is3DView  && in3D == true && in2D1)
        {
           
        }
       

        if (!isTransitioning)
        {
            if (!is3DView && isDefending == false)
            {
                Handle2DMovement();
                Update2DRotation();
            }
        }

       
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!isAttacking)
            {
                TriggerAttackAnimation();
            }
            else if (canCombo)
            {
                TriggerComboAnimation();
            }
        }
        void TriggerAttackAnimation()
        {
            if (animator != null)
            {
                isAttacking = true;
                canCombo = false;
                animator.SetTrigger("Attack");

                if (attackAudio != null)
                {
                    attackAudio.Play();
                }

                StartCoroutine(EnableComboWindow());
                StartCoroutine(AttackImpulse()); // ← Corutina para movimiento hacia delante
            }
        }

        IEnumerator AttackImpulse()
        {
            float duration = 0.5f;         // Duración del impulso
            float speed = 8f;              // 6 * 3 = 18 → Impulso tres veces más fuerte
            float timer = 0f;

            while (timer < duration)
            {
                Vector3 impulse = transform.forward * speed;
                characterController.Move(impulse * Time.deltaTime);

                timer += Time.deltaTime;
                yield return null;
            }
        }

        void TriggerComboAnimation()
        {
            if (animator != null && canCombo)
            {
                attackAudio2.Play();
                animator.SetTrigger("Attack2");
                canCombo = false; // Evita más ataques en cadena.
            }
        }

        // Detecta cuando se pulsa la tecla E para activar la defensa.
        if (Input.GetKeyDown(KeyCode.E))
        {
            TriggerDefenseAnimation();
            if (defenseAudio != null) defenseAudio.Play();
            isDefending = true;
        }  if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("Parry");
            if (defenseAudio != null) defenseAudio.Play();
          
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            StopDefenseAnimation();
            isDefending = false;

        }
        UpdateMovementState();
     

    }
    IEnumerator EnableComboWindow()
    {
        canCombo = true; // Permite el combo.
        yield return new WaitForSeconds(0.62f);
        canCombo = false; // Desactiva la ventana de combo.
        isAttacking = false; // Reinicia el estado de ataque.
    }

    void TriggerDefenseAnimation()
    {
        if (animator != null)
        {
            // Activa el parámetro "Defense" en el Animator.
            animator.SetBool("Defense", true);

            // Llama a la animación de defensa.
            animator.SetTrigger("Defense");
        }
    }
    // Variable fuera de cualquier método
    private float lastJumpTime = 0f;


    void Handle2DMovement()
    {
        if (isChangingView)
        {
            return;
        }

        float moveHorizontal = 0f;
        float moveVertical = 0f;

        if (in2D1)
        {
            moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");
        }
        else if (in2D2)
        {
            moveHorizontal = (Input.GetKey(KeyCode.W)) ? -1f : (Input.GetKey(KeyCode.S)) ? 1f : 0f;
            moveVertical = (Input.GetKey(KeyCode.A)) ? -1f : (Input.GetKey(KeyCode.D)) ? 1f : 0f;
        }

        float baseSpeed = isAttacking ? 3f : moveSpeed;
        if (!isAttacking)
        {
            if (Mathf.Abs(moveHorizontal) > 0 || Mathf.Abs(moveVertical) > 0)
            {
                currentNormalSpeed = Mathf.MoveTowards(currentNormalSpeed, baseSpeed, accelerationRate * Time.deltaTime);
            }
            else
            {
                currentNormalSpeed = Mathf.MoveTowards(currentNormalSpeed, 3f, accelerationRate * Time.deltaTime);
            }
        }
        else
        {
            currentNormalSpeed = moveSpeed;
        }

        float currentSpeed = (!isAttacking && Input.GetKey(KeyCode.LeftShift)) ? speedMove : currentNormalSpeed;

        animator.SetFloat("Speed", Mathf.Max(Mathf.Abs(moveHorizontal), Mathf.Abs(moveVertical)) * currentSpeed);

        Vector3 moveInput = new Vector3(moveHorizontal, 0, moveVertical).normalized;
        Vector3 moveDirection = moveInput * currentSpeed;
        moveDirection.y = verticalVelocity;

        characterController.Move(moveDirection * Time.deltaTime);

        if (moveHorizontal != 0 || moveVertical != 0)
        {
            if (in2D2)
            {
                if (moveVertical > 0) transform.rotation = Quaternion.Euler(0, 0, 0);
                else if (moveVertical < 0) transform.rotation = Quaternion.Euler(0, -180, 0);
                else if (moveHorizontal > 0) transform.rotation = Quaternion.Euler(0, 110, 0);
                else if (moveHorizontal < 0) transform.rotation = Quaternion.Euler(0, -110, 0);
            }
            else
            {
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveHorizontal, 0, moveVertical));
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 15f);
            }
        }

        bool jumpPressed = Input.GetKeyDown(KeyCode.Space);
        float jumpCooldown = 0.5f;

        if (characterController.isGrounded)
        {
            verticalVelocity = -gravity * Time.deltaTime;
            hasDoubleJumped = false;
            lastJumpTime = 0f;

            if (jumpPressed)
            {
                verticalVelocity = isAttacking ? jumpForce + 4 : jumpForce;
                TriggerJumpAnimation();
                lastJumpTime = Time.time;
                if (jumpAudio != null) jumpAudio.Play();
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;

            if (Mathf.Abs(moveHorizontal) == 0 && Mathf.Abs(moveVertical) == 0)
            {
                currentNormalSpeed = Mathf.Max(currentNormalSpeed - accelerationRate * Time.deltaTime, 3f);
            }

            if (!hasDoubleJumped && jumpPressed && Time.time - lastJumpTime > 0.01f && Time.time - lastJumpTime < jumpCooldown)
            {
                verticalVelocity = 16;
                hasDoubleJumped = true;
                TriggerJumpAnimation2();
                if (jumpAudio != null) jumpAudio.Play();
            }
        }

        if (characterController.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -gravity * Time.deltaTime;
        }
    }


    public void AddExtraJump(float extraJumpHeight)
    {
       
            // Aplicamos el salto adicional solo si está en el suelo
            verticalVelocity = extraJumpHeight;

            // Activamos la animación de salto
            TriggerJumpAnimation2();

            // Reproducimos el sonido de salto
            if (jumpAudio != null) jumpAudio.Play();
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Verifica si el objeto que colisiona tiene un tag específico
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.SetTrigger("Ground");

        }
    }
            void TriggerJumpAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Jump");
        }
    }  
    void TriggerJumpAnimation2()
    {
        if (animator != null)
        {
            animator.SetTrigger("Jump2");
        }
    }

    void Update2DRotation()
    {
       
            float moveHorizontal = Input.GetAxis("Horizontal");

            if (moveHorizontal > 0)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, rightRotation, Time.deltaTime * 10);
            }
            else if (moveHorizontal < 0)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, leftRotation, Time.deltaTime * 10);
            }
        
    }

    IEnumerator ChangeView()
    {
        if (changeView != null) changeView.Play();

        canChangeView = false;
        DisableCharacterMovement();

        if (is3DView)
        {
            SetView2D();

        }
        else
        {
            SetView3D();
        }

        is3DView = !is3DView;

        UpdateCoinVisibility();

        for (float t = 0; t <= 1; t += Time.deltaTime * 2)
        {
            yield return null;
        }

        canChangeView = true;

        EnableCharacterMovement();
        isTransitioning = false;
        UpdateMovementState();
    }

    void SetView2D()
    {
        Cam2D.SetActive(true);
        Cam3D.SetActive(false);


        transform.position = new Vector3(transform.position.x, transform.position.y, 1f);

        if (movimiento3D != null)
        {
            movimiento3D.enabled = false;
        }

        transform.rotation = rightRotation;
    }

    void SetView3D()
    {
        Cam2D.SetActive(false);
        Cam2D_2.SetActive(false);
        Cam3D.SetActive(true);
        mainCamera = Cam3D.GetComponent<Camera>();

        mainCamera.orthographic = false;

        if (movimiento3D != null)
        {
            movimiento3D.enabled = true;
        }
    }

    void UpdateCoinVisibility()
    {
        foreach (GameObject moneda in monedasTransparentesEn3D)
        {
            SetCoinTransparency(moneda, is3DView ? transparentAlpha : opaqueAlpha);
        }
        foreach (GameObject moneda in monedasTransparentesEn2D)
        {
            SetCoinTransparency(moneda, is3DView ? opaqueAlpha : transparentAlpha);
        }
    }

    void SetCoinTransparency(GameObject moneda, float alpha)
    {
        Renderer renderer = moneda.GetComponent<Renderer>();
        Color color = renderer.material.color;
        color.a = alpha;
        renderer.material.color = color;
    }

    void UpdateCoinCounter()
    {
        coinCounterText.text = $"{coinCount}";

        if (visibilityCoroutine != null)
        {
            StopCoroutine(visibilityCoroutine);
        }
        else if (!coinCounterContainer.activeSelf)
        {
            coinCounterContainer.SetActive(true);
            visibilityCoroutine = StartCoroutine(ShowAndHideCounter());
        }
        else
        {
            visibilityCoroutine = StartCoroutine(FadeOutIfActive());
        }
    }

    IEnumerator ShowAndHideCounter()
    {
        yield return new WaitForSeconds(3f);

        visibilityCoroutine = StartCoroutine(FadeOutIfActive());
    }

    IEnumerator FadeOutIfActive()
    {
        if (coinCounterContainer.activeSelf)
        {
            yield return FadeText(1f, 0f, 1f);
            coinCounterContainer.SetActive(false);
        }

        visibilityCoroutine = null;
    }

    IEnumerator FadeText(float startAlpha, float endAlpha, float duration)
    {
        CanvasGroup canvasGroup = coinCounterContainer.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = coinCounterContainer.AddComponent<CanvasGroup>();
        }

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t / duration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }

    IEnumerator ShowAndHideCounterGems()
    {
        gemCounterContainer.SetActive(true);
      //  yield return FadeTextGems(0f, 1f, 1f);
        yield return new WaitForSeconds(6f);
        //yield return FadeTextGems(1f, 0f, 1f);
        //gemCounterContainer.SetActive(false);
        gemCounterContainer.SetActive(false);

    }
    void UpdateGemCounter()
    {
        gemCounterText.text = $"{gemCount}";

        if (visibilityCoroutine != null)
        {
            StopCoroutine(visibilityCoroutine);
        }
        visibilityCoroutine = StartCoroutine(ShowAndHideCounterGems());
    }
 
    IEnumerator FadeTextGems(float startAlpha, float endAlpha, float duration)
    {
        CanvasGroup canvasGroup = gemCounterContainer.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gemCounterContainer.AddComponent<CanvasGroup>();
        }

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t / duration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }

    void Muerte()
    {
        // Si el personaje ha perdido todas las vidas, detener el movimiento y activar la animación de muerte.
        if (lives <= 0)
        {
            paneles.SetActive(false);
            // Instanciar y reproducir el efecto de partículas
            ParticleSystem dust = Instantiate(dustDeath, transform.position, Quaternion.identity);
            dust.Play();

            cuerpo.SetActive(false);
            DisableCharacterMovement(); // Detener movimiento
            animator.SetTrigger("Death"); // Activar animación de muerte
            StartCoroutine(ShowLoadingScreen()); // Mostrar pantalla de carga
            StartCoroutine(Respawn()); // Llamar a la función de respawn
        }
    }

    IEnumerator ShowLoadingScreen()
    {
        dead = true;
        yield return new WaitForSeconds(4f); // Esperar 3 segundos
        paneles.SetActive(false);

        triggerMapa.hud.SetActive(false);
        loadingPanel.SetActive(true); // Activar la pantalla de carga
        yield return new WaitForSeconds(2.5f); // Esperar 3 segundos
        triggerMapa.hud.SetActive(true);
        dead = false;
        loadingPanel.SetActive(false); // Ocultar la pantalla de carga
        paneles.SetActive(true);

    }

    IEnumerator Respawn()
    {
        triggerMapa.currentSoundtrack.Stop();
        musicDeath.Play();
        yield return new WaitForSeconds(5f); // Esperar unos segundos antes de respawn.
        saveManager.RestaurarMonedas();
        SetView2D();
        lives = 3; // Restaurar vidas
        InitializeHearts(); // Actualizar UI de vidas
        transform.position = spawn; // Reubicar al jugador en la posición de spawn.
        EnableCharacterMovement();
        triggerMapa.currentSoundtrack.Play();
        cuerpo.SetActive(true);

    }

    void RemoveHeart()
    {
        if (heartInstances.Count >= 0)
        {
            RawImage lastHeart = heartInstances[heartInstances.Count - 1];
            Destroy(lastHeart.gameObject);
            heartInstances.RemoveAt(heartInstances.Count - 1);
        }
    }

    
    public void sumarMoneda(int monedas)
    {
        coinCount += monedas;
        if (coinSound != null) coinSound.Play();
        UpdateCoinCounter();
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("ChangeView"))
        {
            inChangeZone = true;
        }
    }  void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ChangeView"))
        {
            inChangeZone = false;
        }
    }
        void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Moneda1"))
        {
            sumarMoneda(1);

        } 
        else if (other.CompareTag("Moneda2"))
        {
            sumarMoneda(2);
           
        }
        else if (other.CompareTag("Moneda5"))
        {

            sumarMoneda(5);
            

        }
        else if (other.CompareTag("Gema"))
        {
            if (gemSound != null) gemSound.Play();
            animator.SetBool("Gem", true);

            StartCoroutine(GemChangeBool());

            gemCount++;
            UpdateGemCounter();
        }
        else if (other.CompareTag("EnemyDamage") && !isInvulnerable && isAttacking == false)
        {
            PerderVidas();
        }

        if (other.CompareTag("Checkpoint"))
        {
            if (spawn != other.transform.position) // Verifica si es un nuevo checkpoint
            {
                spawn = other.transform.position;
                cheackpointSound.Play();
            }
        }

    
        if (other.CompareTag("Heart") && lives < 3)
        {
            RecoverLife();
            Destroy(other.gameObject); // Elimina el objeto del corazón tras ser recogido.
        } if (other.CompareTag("2DZone1") && !isChangingView)
        {
            if(is3DView == true)
            {
                
                in2D1 = true;
                in3D = false;
                in2D2 = false;

                StartCoroutine(ChangeView());
                StartCoroutine(changingViewRoutine());

            }
            if (in2D2 == true)
            {
                Cam2D_2.SetActive(false);
                Cam2D.SetActive(true);
                Cam3D.SetActive(false);
                in2D2 = false;
                in2D1 = true;
                StartCoroutine(changingViewRoutine());
            }
        }
        if (other.CompareTag("2DZone2") && !isChangingView)
        {
            if(in2D2 == false)
            {
                in2D2 = true;
                in2D1 = false;
                in3D = false;
                is3DView = false;
                Cam2D_2.SetActive(true);
                Cam3D.SetActive(false);
                Cam2D.SetActive(false);
                StartCoroutine(changingViewRoutine());

            }
        }
        if (other.CompareTag("3DZone"))
        {
            if(in3D == false)
            {
                in3D = true;
                in2D1 = false;
                in2D2 = false;

                isTransitioning = true;
                verticalVelocity = 0f;
                StartCoroutine(ChangeView());
                StartCoroutine(changingViewRoutine());

            }
        }
        
    }
    IEnumerator changingViewRoutine()
    {
        isChangingView = true;
        DisableCharacterMovement();

        yield return new WaitForSeconds(0f);

        EnableCharacterMovement();
        isChangingView = false;

    }IEnumerator GemChangeBool()
    {
      

        yield return new WaitForSeconds(6f);
        animator.SetBool("Gem", false);



    }
    void RecoverLife()
    {
        
            lives++;
            InitializeHearts();
        
    }
  

    void CreateCoinEffect(Vector3 position)
    {
        // Crear un nuevo GameObject para las partículas
        GameObject particleEffect = new GameObject("CoinEffect");

        // Añadir el componente ParticleSystem
        ParticleSystem ps = particleEffect.AddComponent<ParticleSystem>();

        // Configuración básica del sistema de partículas
        var main = ps.main;
        main.duration = 0.5f;
        main.startLifetime = 0.5f;
        main.startSize = 0.2f;
        main.startColor = Color.yellow;
        main.loop = false;

        var emission = ps.emission;
        emission.rateOverTime = 50;

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.1f;

        // Configurar el renderizador de partículas para evitar el color lila
        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.material = new Material(Shader.Find("Particles/Standard Unlit"));
        renderer.material.SetColor("_Color", Color.yellow);

        // Posicionar el efecto donde estaba la moneda
        particleEffect.transform.position = position;

        // Destruir el efecto después de que termine
        Destroy(particleEffect, main.duration);
    }
    public void PerderVidas()
    {
        if (hitAudio != null) hitAudio.Play();

        lives--;
        RemoveHeart();

        if (lives <= 0)
        {
            Muerte();
            if (deathAudio != null) deathAudio.Play();
        }
        else
        {
            StartCoroutine(Invulnerability());
            StartCoroutine(FlashWhileInvulnerable());
        }
    }

    IEnumerator Invulnerability()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
    }

    IEnumerator FlashWhileInvulnerable()
    {
        float elapsed = 0f;
        bool visible = false;

        while (elapsed < invulnerabilityDuration)
        {
            visible = !visible;
            cuerpo.SetActive(visible);

            elapsed += flashInterval;
            yield return new WaitForSeconds(flashInterval);
        }

        cuerpo.SetActive(true); // Asegura que esté activo al final
    }


    void TriggerAttackAnimation()
    {
        if (animator != null)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
            StartCoroutine(EnableComboWindow());
        }
    }
    IEnumerator AttackCooldown()
    {
        isAttacking = true;
        moveSpeed = 3;
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
        moveSpeed = 9;
    }

    void UpdateMovementState()
    {
        if (!isChangingView)
        {

            isMoving = Input.GetAxis("Horizontal") != 0;

            if (animator != null)
            {
                animator.SetBool("Move", isMoving);
            }
        }
    }
    void StopDefenseAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("Defense", false);
            animator.SetTrigger("StopDefense");
        }
    }

    void DisableCharacterMovement()
    {
        if (characterController != null)
        {
            characterController.enabled = false;
        }
    }

    void EnableCharacterMovement()
    {
        if (characterController != null)
        {
            characterController.enabled = true;
        }
    }
}