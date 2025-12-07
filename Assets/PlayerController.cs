using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Vector3 playerPosition = new Vector3(0, -2, 0);
    private float laneOffset = 2.5f;
    private int currentLane = 1;
    private bool isJumping = false;
    private float jumpSpeed = 4f;
    private float verticalVelocity = 0f;

    public float moveSpeed = 10f;
    public float HP = 100f;
    public float regenRate = 1f;
    public Text hpText;

    public ItemSpawner spawner;

    private bool canTakeDamage = true;
    private float damageCooldown = 0.5f;

    private float targetX;

    void Start()
    {
        targetX = playerPosition.x;
        StartCoroutine(RegenerateHealth());
    }

    void Update()
    {
        HandleInput();
        HandleJump();
        SmoothMoveBetweenLanes();

        transform.position = new Vector2(playerPosition.x, playerPosition.y);

        CheckCollision();

        if (hpText != null)
            hpText.text = "HP: " + Mathf.RoundToInt(HP);
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
            MoveLeft();
        if (Input.GetKeyDown(KeyCode.D))
            MoveRight();
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
            StartJump();
    }

    void MoveLeft()
    {
        if (currentLane > 0)
        {
            currentLane--;
            targetX = (currentLane - 1) * laneOffset;
        }
    }

    void MoveRight()
    {
        if (currentLane < 2)
        {
            currentLane++;
            targetX = (currentLane - 1) * laneOffset;
        }
    }

    void SmoothMoveBetweenLanes()
    {
        
        playerPosition.x = Mathf.MoveTowards(playerPosition.x, targetX, moveSpeed * Time.deltaTime);
    }

    void StartJump()
    {
        isJumping = true;
        verticalVelocity = jumpSpeed;
    }

    void HandleJump()
    {
        if (isJumping)
        {
            playerPosition.y += verticalVelocity * Time.deltaTime;
            verticalVelocity -= 9.8f * Time.deltaTime;

            if (playerPosition.y <= -2)
            {
                playerPosition.y = -2;
                isJumping = false;
            }
        }
    }

    void CheckCollision()
    {
        if (spawner != null && spawner.transform.childCount > 0)
        {
            Item obstacle = spawner.GetComponent<ItemSpawner>().GetComponentInChildren<Item>();

            if (obstacle != null)
            {
               
                if (Mathf.Abs(obstacle.itemPosition.z) < 10f)
                {
                   
                    if (Mathf.Abs(obstacle.itemPosition.x - playerPosition.x) < 1f)
                    {
                        
                        if (!isJumping && canTakeDamage)
                        {
                            TakeDamage(5f);
                        }
                    }
                }
            }
        }
    }

    void TakeDamage(float amount)
    {
        HP -= amount;
        HP = Mathf.Max(0, HP);
        StartCoroutine(DamageCooldown());
    }

    IEnumerator DamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    IEnumerator RegenerateHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (HP < 100f)
            {
                HP += regenRate;
                HP = Mathf.Min(100f, HP);
            }
        }
    }
}
