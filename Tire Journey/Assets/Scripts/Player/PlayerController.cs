using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    public float forwardSpeed = 10.0f;
    public float maxSpeed = 50f;
    private int desiredLane = 1; // 0 = left; 1 = middle; 2 = right
    public float laneDistance = 4; // the distance between two lanes

    public float jumpForce = 1.0f;
    public float Gravity = -20f;

    public Animator animator;
    private bool isSliding = false;


    private void Awake()
    {
        AdManager.instance.RequestInterstitial();
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerManager.isGameStarted)
            return;

        // Increase speed over time (not to exceed max)
        if (forwardSpeed < maxSpeed)
            forwardSpeed += 0.1f * Time.deltaTime;

        animator.SetBool("isGameStarted", true);

        direction.z = forwardSpeed;

        // Get distance for score and save for current score
        PlayerPrefs.SetInt("CurrentScore", Mathf.RoundToInt(transform.position.z));
        
        

        


        // Jumping + gravity
        if (controller.isGrounded)
        {
            animator.SetBool("isGrounded", true);
            if (SwipeManager.swipeUp)
            {
                Jump();
                animator.SetBool("isGrounded", false);
            }
        } else 
        {
            direction.y += Gravity * Time.deltaTime;
        }

        if (SwipeManager.swipeDown && !isSliding)
        {
            StartCoroutine(Slide());
        }

        

        // gather the inuts of which lane we should be in
        if (SwipeManager.swipeRight)
        {
            desiredLane++;
            if (desiredLane == 3)
                desiredLane = 2;
        }

        if (SwipeManager.swipeLeft)
        {
            desiredLane--;
            if (desiredLane == -1)
                desiredLane = 0;
        }

        // calculate where we should be in the future
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * laneDistance;
        } else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * laneDistance;
        }

        if (transform.position != targetPosition)
        {
            Vector3 diff = targetPosition - transform.position;
            Vector3 moveDir = diff.normalized * 30 * Time.deltaTime;
            if (moveDir.sqrMagnitude < diff.magnitude)
                controller.Move(moveDir);
            else
                controller.Move(diff);
        }

        // Move Player
        controller.Move(direction * Time.deltaTime);

    }

    private void Jump()
    {
        direction.y = jumpForce;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.tag == "Obstacle")
        {   
            Debug.Log("Score: " + PlayerPrefs.GetInt("CurrentScore", 0) + " Hi: " + PlayerPrefs.GetInt("HighScore", 0));

             // Update high score, if applicable
            if (PlayerPrefs.GetInt("CurrentScore", 0) > PlayerPrefs.GetInt("HighScore", 0)) {
                PlayerPrefs.SetInt("HighScore", Mathf.RoundToInt(transform.position.z));
                Debug.Log("NEW HIGH SCORE: " + PlayerPrefs.GetInt("HighScore", 0));
            }

            // Play game over sequence
            PlayerManager.gameOver = true;
            FindObjectOfType<AudioManager>().PlaySound("GameOver");
            

            // Show ad randomly (0-2 || 33% of time)
            int testNum = Random.Range(0, 3);

            if (testNum == 0)
                AdManager.instance.ShowInterstitial();
        }
    }

    private IEnumerator Slide()
    {
        isSliding = true;
        animator.SetBool("isSliding", true);
        

        // Change the collider box when sliding
        controller.center = new Vector3(0, -0.5f, 0);
        controller.height = 1;

        yield return new WaitForSeconds(1.3f);

        controller.center = new Vector3(0, 0, 0);
        controller.height = 2;

        animator.SetBool("isSliding", false);
        isSliding = false;

    }
}
