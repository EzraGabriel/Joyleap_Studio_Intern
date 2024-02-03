using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketShip : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] int maxHealth = 100;
    [SerializeField] float mainThrust = 8f;
    [SerializeField] float rotationThrust = 200f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip explosion;
    [SerializeField] AudioClip successSound;
    [SerializeField] ParticleSystem mainEngineParticle, explosionParticle;
    AudioSource myAudioSource;
    GameController gameController;
    HealthBar rocketHealthBar;

    bool isAlive = true;
    int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();
        gameController = FindObjectOfType<GameController>();
        rocketHealthBar = FindObjectOfType<HealthBar>();

        currentHealth = maxHealth;
        rocketHealthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if(isAlive)
        {
            RocketMovement();
        }
    }

    private void RocketMovement()
    {
        float rotationSpeed = Time.deltaTime * rotationThrust;

        Thrusting();
        Rotating(rotationSpeed);
    }

    private void Thrusting()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!myAudioSource.isPlaying)
            {
                myAudioSource.PlayOneShot(mainEngine);
            }
            mainEngineParticle.Play();
            rb.AddRelativeForce(Vector3.up * mainThrust);

        }
        else
        {
            myAudioSource.Stop();
            mainEngineParticle.Stop();
        }
    }

    private void Rotating(float rotationSpeed)
    {
        rb.freezeRotation = true;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }
        rb.freezeRotation = false;
    }
    private void LateUpdate()
    {
        transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z);
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        rocketHealthBar.SetHealth(currentHealth);

        FindObjectOfType<CanvasFade>().Fade();

        if(currentHealth == 0)
        {
            DeathRoutine();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!isAlive || !gameController.collisionEnabled)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Okay");
                break;

            case "Finish":
                SuccessRoutine();
                break;

            default:
                TakeDamage(20);
                break;
        }

    }

    private void DeathRoutine()
    {
        isAlive = false;
        AudioSource.PlayClipAtPoint(explosion, Camera.main.transform.position);
        gameController.ResetGame();

        FindObjectOfType<ShakeCam>().ShakeCamera();
        explosionParticle.Play();
    }

    private void SuccessRoutine()
    {
        rb.isKinematic = true;
        gameController.NextLevel();
        AudioSource.PlayClipAtPoint(successSound, Camera.main.transform.position);
    }
}
