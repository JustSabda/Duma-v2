using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;
    public float speed;
    public float curSpeed;


    public Transform playerTransform;
    public bool isChasing;
    public float chaseDistance;

    // Start is called before the first frame update
    void Start()
    {
        isChasing = false;
        playerTransform = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;

        anim.SetBool("isRunning", true);
        curSpeed = speed;
        

        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 point = currentPoint.position - transform.position;

        if (isChasing)
        {
            if(transform.position.x < playerTransform.position.x)
            {
                if (curSpeed != 0)
                    rb.velocity = new Vector2(curSpeed + 1, 0);
            }
            else
            {
                if (curSpeed != 0)
                    rb.velocity = new Vector2(-curSpeed - 1 , 0);
            }

            if (Vector2.Distance(transform.position, playerTransform.position) > chaseDistance)
            {
                isChasing = false;
            }
        }
        else
        {

            if (currentPoint == pointB.transform)
            {
                rb.velocity = new Vector2(curSpeed, 0);
                if (Vector2.Distance(transform.position, playerTransform.position) < chaseDistance && transform.position.x < playerTransform.position.x)
                {
                    isChasing = true;
                }
            }
            else
            {
                rb.velocity = new Vector2(-curSpeed, 0);
                if (Vector2.Distance(transform.position, playerTransform.position) < chaseDistance && transform.position.x > playerTransform.position.x)
                {
                    isChasing = true;
                }
            }

            if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
            {
                StartCoroutine(Idle());
                currentPoint = pointA.transform;
            }
            else if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
            {
                StartCoroutine(Idle());
                currentPoint = pointB.transform;
            }
        }

    }

    private IEnumerator Idle()
    {
        curSpeed = 0;
        anim.SetBool("isRunning", false);
        yield return new WaitForSeconds(2f);

        flip();
        curSpeed = speed;
    }

    private void flip()
    {
        anim.SetBool("isRunning", true);
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }

    
}
