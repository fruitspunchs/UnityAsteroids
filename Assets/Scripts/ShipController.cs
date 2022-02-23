using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    float horizontal;
    Vector2 lookDirection = new Vector2(1, 0);
    float acceleration = 1.0f;
    float rotationSpeed = 2.0f;
    Vector2 velocity = new Vector2();

    public GameObject bullet;
    GameController gameController;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        GameObject game = GameObject.Find("Game");
        gameController = game.GetComponent<GameController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");

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


        if (Input.GetKey(KeyCode.W))
        {
            velocity += lookDirection * acceleration * Time.deltaTime;
            animator.SetBool("Thrust", true);
        }
        else
        {
            animator.SetBool("Thrust", false);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject firedBullet = Instantiate(bullet, transform.position + transform.right * 0.5f, Quaternion.identity);
            BulletController controller = firedBullet.GetComponent<BulletController>();
            controller.setDirection(lookDirection);
        }
    }

    void FixedUpdate()
    {
        rigidbody2d.rotation += horizontal * -1 * rotationSpeed;
        lookDirection = transform.right;

        Vector2 position = rigidbody2d.position;

        position.x += velocity.x * Time.deltaTime;
        position.y += velocity.y * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        gameController.lives--;
    }
}
