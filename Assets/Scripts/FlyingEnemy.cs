using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float maxSpeed;
    public bool chase = false;
    public Vector3 startingPoint;
    private float curSpeed;

   
    public GameObject player;

    float startingY;
    public float rangePatrol;
    int dir = 1;
    public bool isPatrol;

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        startingPoint = transform.position;
        startingY = startingPoint.y;

        isPatrol = true;
        //anim.SetBool("isRunning", true);
        curSpeed = maxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            return;
        if (chase == true)
            Chase();
        else
            ReturnStartPoint();

        Flip();

    }

    private void Chase()
    {
        startingPoint.y = transform.position.y;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, curSpeed * Time.deltaTime);
        isPatrol = false;
    }
    private void ReturnStartPoint()
    {
        if (!isPatrol)
        {
            
            transform.position = Vector2.MoveTowards(transform.position, startingPoint, curSpeed * Time.deltaTime);

   
        }

        if (transform.position == startingPoint)
        {
            isPatrol = true;
        }



    }

    private void FixedUpdate()
    {
        Patrol();
    }

    private void Patrol()
    {
        if (isPatrol)
        {
            transform.Translate(Vector2.up * curSpeed * Time.deltaTime * dir);
            if (transform.position.y < startingY - rangePatrol || transform.position.y > startingY + rangePatrol)
            {
                StartCoroutine(Idle());
                dir *= -1;
            }

        }
    }
    private IEnumerator Idle()
    {
        curSpeed = 0;
        //anim.SetBool("isRunning", false);
        yield return new WaitForSeconds(2f);

        curSpeed = maxSpeed;
        //anim.SetBool("isRunning", true);
    }


    

    private void Flip()
    {
        if (transform.position.x > player.transform.position.x)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}
