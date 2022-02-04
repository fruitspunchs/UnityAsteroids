using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    Vector2 lookDirection;
    public float speed = 1.0f;
    Rigidbody2D rigidbody2d;

    // Start is called before the first frame update
    void Start()
    {
        lookDirection.x = Random.value;
        lookDirection.y = Random.value;
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        rigidbody2d.position += lookDirection * speed * Time.deltaTime;
    }
}
