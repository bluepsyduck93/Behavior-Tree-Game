using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour
{
    private Animator animator;

    bool isRunning = false;
    bool isWalking = false;
    bool isIdle = true;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("player");
        animator = player.GetComponent<Animator>();
    }


    void Update()
    {
       
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        float speed = new Vector2(h, v).sqrMagnitude;
        animator.SetFloat("Speed", v);
        animator.SetFloat("Direction",h );

         if (Input.GetKeyDown(KeyCode.Space))
               {
              animator.SetTrigger("jump");
              StartCoroutine(HandleIt());
              transform.Translate(Vector3.up * 20* Time.deltaTime, Space.World);
              animator.SetBool("isIdle", false);
              animator.SetBool("isWalking", false);
              animator.SetBool("isRunning", false);

          }

        if (v > 0.05 && v < 0.8)
             {
                 animator.SetBool("isIdle", false);
                 animator.SetBool("isWalking", true);
                 animator.SetBool("isRunning", false);
             }
             else if (v >= 0.8)
             {
                 animator.SetBool("isIdle", false);
                 animator.SetBool("isWalking", false);
                 animator.SetBool("isRunning", true);
             }

             else if (v==0){
                 animator.SetBool("isIdle", true);
                 animator.SetBool("isWalking", false);
                 animator.SetBool("isRunning", false);
             }
          
         }
    
      private IEnumerator HandleIt()
    {
     
        yield return new WaitForSeconds(1.0f);
     
    }

}

