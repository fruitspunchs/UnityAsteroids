using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    Vector2 lookDirection = new Vector2();
    float speed = 5.0f;
    float bulletLifespan = 1.5f;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 position = transform.position;
        if (position.x > 6.7f)
        {
            position.x = -6.7f;
        }
        else if (position.x < -6.7f)
        {
            position.x = 6.7f;
        }
        else if (position.y > 5.0f)
        {
            position.y = -5.0f;
        }
        else if (position.y < -5.0f)
        {
            position.y = 5.0f;
        }
        transform.position = position;

        bulletLifespan -= Time.deltaTime;
        if (bulletLifespan < 0)
        {
            Destroy(gameObject);
        }
    }
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;

        position.x += lookDirection.x * speed * Time.deltaTime;
        position.y += lookDirection.y * speed * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void setDirection(Vector2 direction)
    {
        direction.Normalize();
        this.lookDirection = direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
