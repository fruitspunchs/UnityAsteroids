using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOController : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    Vector2 moveDirection = new Vector2(-1, 0);
    float speed = 2.0f;
    public GameObject ufoBullet;
    float shootInterval = 0.5f;
    float shootTimer = 0.5f;
    float errorMargin = 45.0f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        StartCoroutine(ChangeDirectionAfterDelay(Random.Range(1.0f, 3.0f)));
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {
            shootTimer = shootInterval;

            GameObject playerShip = GameObject.FindGameObjectWithTag("Player");
            if (playerShip != null)
            {
                Debug.Log("Shooting player");

                Vector2 shipDirection = playerShip.transform.position - transform.position;
                float error = Random.Range(-errorMargin, errorMargin);
                shipDirection = Quaternion.Euler(0f, 0f, error) * shipDirection;

                GameObject firedBullet = Instantiate(ufoBullet, transform.position, Quaternion.identity);
                BulletController controller = firedBullet.GetComponent<BulletController>();

                controller.setDirection(shipDirection);
            }
        }
    }

    public void SetXDirection(float x)
    {
        moveDirection = new Vector2(x, 0);
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;

        position.x += moveDirection.x * speed * Time.deltaTime;
        position.y += moveDirection.y * speed * Time.deltaTime;

        rigidbody2d.MovePosition(position);

        if (position.x > 6.7f || position.x < -6.7f)
        {
            Destroy(gameObject);
        }

        if (position.y > 5.0f)
        {
            position.y = -5.0f;
            rigidbody2d.MovePosition(position);
        }

        if (position.y < -5.0f)
        {
            position.y = 5.0f;
            rigidbody2d.MovePosition(position);
        }
    }

    IEnumerator ChangeDirectionAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);

        moveDirection.y = Random.Range(0, 2) == 0 ? -1 : 1;

        StartCoroutine(ChangeDirectionAfterDelay(Random.Range(1.0f, 3.0f)));
    }
}
