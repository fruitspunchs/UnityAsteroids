using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controller for UFOs
public class UFOController : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    Vector2 moveDirection = new Vector2(-1, 0);
    float speed = 2.0f;
    public GameObject ufoBullet;
    float shootInterval = 0.5f;
    float shootTimer = 0.5f;
    public float errorMargin = 45.0f;
    int points = 1000;

    GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        GameObject game = GameObject.Find("Game");
        gameController = game.GetComponent<GameController>();

        rigidbody2d = GetComponent<Rigidbody2D>();

        //Changes ufo movement every few seconds
        StartCoroutine(ChangeDirectionAfterDelay(Random.Range(1.0f, 3.0f)));
    }

    // Update is called once per frame
    void Update()
    {
        //Shoot bullets at player
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {
            shootTimer = shootInterval;

            GameObject playerShip = GameObject.FindGameObjectWithTag("Player");
            if (playerShip != null)
            {
                //Get player direction and add some aiming error
                Vector2 shipDirection = playerShip.transform.position - transform.position;
                float error = Random.Range(-errorMargin, errorMargin);
                shipDirection = Quaternion.Euler(0f, 0f, error) * shipDirection;

                GameObject firedBullet = Instantiate(ufoBullet, transform.position, Quaternion.identity);
                BulletController controller = firedBullet.GetComponent<BulletController>();

                controller.setDirection(shipDirection);
            }
        }
    }

    //Set if ufo goes left or right
    public void SetXDirection(float x)
    {
        moveDirection = new Vector2(x, 0);
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;

        //Move UFO
        position.x += moveDirection.x * speed * Time.deltaTime;
        position.y += moveDirection.y * speed * Time.deltaTime;

        rigidbody2d.MovePosition(position);

        //Destroy ufo if at opposite end of screen
        if (position.x > 6.7f || position.x < -6.7f)
        {
            Destroy(gameObject);
        }

        //Wrap ufo location if at top or bottom
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

    //Change ufo direction every few seconds
    IEnumerator ChangeDirectionAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);

        moveDirection.y = Random.Range(0, 2) == 0 ? -1 : 1;

        StartCoroutine(ChangeDirectionAfterDelay(Random.Range(1.0f, 3.0f)));
    }

    //Destroy ufo and give points on collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        gameController.AddScore(points);
        gameController.playExplosion(transform.position);
    }
}
