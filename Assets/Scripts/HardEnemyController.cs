using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardEnemyController : MonoBehaviour
{
    public float speed = 3.0f;
    public bool vertical; 
    public float changeTime = 3.0f;

    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;

    // Smoke Variable
    public ParticleSystem smokeEffect;

    bool broken = true;

    // Animation
    Animator animator;

    private RubyController rubyController;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;

        animator = GetComponent<Animator>();


        GameObject rubyControllerObject = GameObject.FindWithTag("RubyController");
        rubyController = rubyControllerObject.GetComponent<RubyController>();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }

        if(!broken)
        {
            return;
        }
    }

    
    void FixedUpdate()
    {
        Vector2 position = rigidbody2D.position;

        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;;
            
            
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        
        else 
        {
            position.x = position.x + Time.deltaTime * speed * direction;;

            
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }

        
        if(!broken)
        {
            return;
        }
        
        rigidbody2D.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
        {
            player.ChangeHealth(-2); 
        }
    }

    
    public void Fix()
    {
        broken = false;
        rigidbody2D.simulated = false;

        //optional if you added the fixed animation
        animator.SetTrigger("Fixed");

        // Particle effect set to false
        smokeEffect.Stop();

        if (rubyController != null)
        {
            rubyController.FixedRobots(1);
        }


        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController >();

        if (controller != null)
        {
            controller.ChangeHealth(-2); 
        }
    }
}
