using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEnemyController : MonoBehaviour
{
    
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;

    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;
    Animator animator;

    // Start is called before the first frame update


void Start()
{
     rigidbody2D = GetComponent<Rigidbody2D>();
     timer = changeTime;
     animator = GetComponent<Animator>();
}

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2D.position;

        if (vertical)
        {
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", direction);
            position.y = position.y + Time.deltaTime * speed * direction;;
        }
        else
        {
            animator.SetFloat("MoveX", direction);
            animator.SetFloat("MoveY", 0);
            position.x = position.x + Time.deltaTime * speed * direction;;
        }

        rigidbody2D.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController controller = other.gameObject.GetComponent<RubyController >();

        if (controller != null)
        {
            controller.ChangeHealth(-1);
            controller.speedDown(1);
            Destroy(gameObject);

        }
    }

    
}

