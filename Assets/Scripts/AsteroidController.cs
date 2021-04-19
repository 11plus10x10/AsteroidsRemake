using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour 
{
    private Rigidbody2D rb;
    public float speed;

    public GameObject[] subAsteroids;
    public int numberOfAsteroids;

    // Start is called before the first frame update
    private void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = 0;
        rb.angularDrag = 0;

        // Randomizing speed of asteroids.
        rb.velocity = new Vector3 (
            Random.Range (-1f, 1f),
            Random.Range (-1f, 1f),
            Random.Range (-1f, 1f)
        ).normalized * speed;

        // Randomizing rotation.
        rb.angularVelocity = Random.Range (-50f, 50f);
    }

    /// <summary>
    ///     Logic for colliding with player or bullet.
    /// </summary>
    /// <param name="col">Player or bullet collider</param>
    private void OnTriggerEnter2D(Collider2D col) 
    {
        if (col.CompareTag ("Bullet")) 
        {
            Destroy (gameObject);
            Destroy (col.gameObject);
            
            // Asteroid crumbles into smaller ones.
            for (var i = 0; i < numberOfAsteroids; i++) 
            {
                Instantiate (
                    subAsteroids[Random.Range (0, subAsteroids.Length)],
                    transform.position,
                    Quaternion.identity
                );
            }
        }
        // GGWP.
        if (!col.CompareTag("Player")) return;
        {
            var asteroidsCollection = FindObjectsOfType<AsteroidController>();
            foreach (var asteroid in asteroidsCollection)
            {
                Destroy(asteroid.gameObject);
            }
            col.GetComponent<PlayerController>().Defeat();
        }
    }
}