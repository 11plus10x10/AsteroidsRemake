using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    private Rigidbody2D _rb;
    public float acceleration;
    public float maxSpeed;
    public float inertia;
    public float angularSpeed;
    public float shootRate = 0.5f;
    public GameObject bulletPrefab;
    public Transform bulletSpawner;
    private InputMaster _inputMaster;

    private float _thrustInput;
    private float _rotationInput;
    private bool _shooting;
    private bool _canShoot = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D> ();
        _rb.drag = inertia;
        
        // Instantiating input actions.
        _inputMaster = new InputMaster();
        
        // Subscribing to Fire action.
        _inputMaster.Player.Fire.performed += _ =>
        {
            _shooting = true;
            Fire();
            _shooting = false;
        };
        // Subscribing to Hyperspace action.
        _inputMaster.Player.Hyperspace.performed += _ => Hyperspace();
    }

    private void OnEnable()
    {
        _inputMaster.Enable();
    }

    private void OnDisable()
    {
        _inputMaster.Disable();
    }
    
    private void Update() 
    {
        _thrustInput = _inputMaster.Player.Thrust.ReadValue<float>();
        _rotationInput = _inputMaster.Player.Rotate.ReadValue<float>();
    }

    /// <summary>
    ///     Shoot with specific fire rate.
    /// </summary>
    private void Fire() 
    {
        if (_shooting && _canShoot) 
        {
            StartCoroutine (FireRate ());
        }
    }

    /// <summary>
    ///     Rotating ship. Nuff said.
    /// </summary>
    private void Rotate() 
    {
        if (_rotationInput == 0) 
        {
            return;
        }
        transform.Rotate (0, 0, -angularSpeed * _rotationInput * Time.deltaTime);
    }

    private void FixedUpdate() 
    {
        // Moving forward and without reverse.
        var forwardMotion = Mathf.Clamp (_thrustInput, 0f, 1f);
        
        
        _rb.AddForce (transform.up * (acceleration * forwardMotion));
        
        // Speed limit.
        if (_rb.velocity.magnitude > maxSpeed) 
        {
            _rb.velocity = _rb.velocity.normalized * maxSpeed;
        }
        
        Rotate();
    }

    /// <summary>
    ///     Teleporting player to the center of the screen.
    /// </summary>
    public void Defeat() {
        _rb.velocity = Vector3.zero;
        transform.position = Vector3.zero;
    }

    /// <summary>
    ///     Spawning bullets and making a delay between shots.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FireRate() {
        _canShoot = false;
        var bullet = Instantiate (
            bulletPrefab,
            bulletSpawner.position,
            bulletSpawner.rotation
        );
        Destroy (bullet, 5);
        yield return new WaitForSeconds (shootRate);
        _canShoot = true;
    }

    /// <summary>
    ///     Ability to teleport to random location.
    /// </summary>
    private void Hyperspace()
    {
        // Turn off colliders and spriteRenderers.
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        
        // Moving to new random position.
        Vector2 newPosition = new Vector2(Random.Range(-10f, 10f), Random.Range(-5f, 5f));
        transform.position = newPosition;
        
        // Turn on colliders and spriteRenderers.
        GetComponent<Collider2D>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
    }
}