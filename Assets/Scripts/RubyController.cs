using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;
    public int maxHealth = 5;
    public int health { get { return currentHealth; }}
    int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer; 

    //Cog Stuff
    public GameObject projectilePrefab;
    public int ammo { get { return currentAmmo; }}
    public int currentAmmo;
    public TextMeshProUGUI ammoText;

    public float timeBoosting = 4.0f;
    float speedBoostTimer;
    bool isBoosting;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);

    AudioSource audioSource;
    public AudioSource backgroundManager;
    public AudioClip throwSound;
    public AudioClip hitSound;
    public AudioClip WinnerTune;
    public AudioClip LoserTune;

    public ParticleSystem hitEffect;

    //fixed robot text
    public TextMeshProUGUI fixedText;
    private int scoreFixed = 0;

    public GameObject WinTextObject;
    public GameObject LoseTextObject;
    bool gameOver;
    public static int level;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        audioSource= GetComponent<AudioSource>();

        rigidbody2d = GetComponent<Rigidbody2D>();
        AmmoText();

        fixedText.text = "Fixed Robots: " + scoreFixed.ToString() + "/4";

        WinTextObject.SetActive(false);
        LoseTextObject.SetActive(false);
        gameOver = false;
        level = 1;


    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        
        Vector2 move = new Vector2(horizontal, vertical);
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        if (isBoosting == true)
        {
            speedBoostTimer -= Time.deltaTime; 
            speed = 5;
        
            if (speedBoostTimer < 0)
            {
                isBoosting = false;
                speed = 3; 
            }
        }
                
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();
            
            if (currentAmmo > 0)
            {
                ChangeAmmo(-1);
                AmmoText();
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
               NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
               if (character != null)
                {
                    character.DisplayDialog();
                }  
            }
            if (scoreFixed >= 4)
            {
                SceneManager.LoadScene("Level 2");
                level = 2;
            }

        }

        if (Input.GetKeyDown(KeyCode.R))
            {
                if (gameOver == true)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
    
    }
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }
        
    public void ChangeHealth(int amount)
    {
        if (amount < 0 )
        {
            if (isInvincible)
                return;
            isInvincible = true;
            invincibleTimer = timeInvincible;

            hitEffect.Play();

            PlaySound(hitSound);
        }


        //Ruby lose
        if (currentHealth <=1 )
        {
            LoseTextObject.SetActive(true);
            gameOver = true;

            speed = 0;

            Destroy(gameObject.GetComponent<SpriteRenderer>());

            backgroundManager.Stop();
            audioSource.clip = LoserTune;
            audioSource.Play();
            
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }
    public void ChangeAmmo(int amount)
    {
        currentAmmo = Mathf.Abs(currentAmmo + amount);
        Debug.Log("Ammo: " + currentAmmo);
    }
    public void AmmoText()
    {
        ammoText.text = "Ammo: " + currentAmmo.ToString();

    }
    
    void Launch()
    {
        if (currentAmmo > 0)
        {
            GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(lookDirection, 300);

            animator.SetTrigger("Launch");

            PlaySound(throwSound);
        }
    }
    public void FixedRobots(int amount)
    {
        scoreFixed += amount;
        fixedText.text = "Fixed Robots: " + scoreFixed.ToString() + "/4";
        Debug.Log("Fixed Robots: " + scoreFixed);
        //ruby win
        if (scoreFixed >=4)
        {
            WinTextObject.SetActive(true);
            gameOver = true;

            backgroundManager.Stop();
            audioSource.clip = WinnerTune;
            audioSource.Play();



        }

    }
    public void SpeedBoost(int amount)
    {
        if (amount > 0)
        {
            speedBoostTimer = timeBoosting;
            isBoosting = true;
        }
    }
    public void speedDown(int amount)
    {
        if(amount > 0)
        {
            speed = speed * 0.5f;
        }
    }
}

