using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float fallSpeedDelta;
    [SerializeField] private float flyTriggerDelta;
    [SerializeField] public float sideOfSight;
    [SerializeField] private float collisionDelta;
    [SerializeField] private float whichAxisLocked;
    [SerializeField] private float leftArenaBorder, rightArenaBorder;

    [SerializeField] private GameObject enemySkybox;

    [SerializeField] private Battle_System bs;
    [SerializeField] private Buffer buf;

    [SerializeField] private LayerMask movementLayer;

    [SerializeField] private bool isGrounded;

    [SerializeField] private Collider2D movementCollider;

    [SerializeField] private GameObject enemy;
    

    private Rigidbody2D rb;
    

    private float axisX, axisY;


    [SerializeField] bool actionAllowed = true;

    [SerializeField] public bool isStunned = false;

    [SerializeField] public List<string> controls;
    
    
    
    public struct action
    {
        public float s, a, r;

        public action(float s, float a, float r)
        {
            this.s = s;
            this.a = a;
            this.r = r;
        }
    }

    public List<action> inputs;

    action punch = new action(2, 1, 2);
    action moveForward = new action(0,0,0);
    action moveBackward = new action(0,0,0);
    action sit = new action(0,0,0);
    action jump = new action(0,0,0);

    private action currentAction;

    private IEnumerator windupAction(float time)
    {
        actionAllowed = false;
        yield return new WaitForSeconds(time/60);
        bam();
        StartCoroutine(activeAction(currentAction.s));
    }

    private IEnumerator activeAction(float time)
    {
        yield return new WaitForSeconds(time / 60);
        StartCoroutine(recoveryAction(currentAction.r));
    }

    private IEnumerator recoveryAction(float time)
    {
        yield return new WaitForSeconds(time / 60);
        actionAllowed = true;
    }

    private void bam() {
        //Debug.Log("kek");
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bs = GetComponent<Battle_System>();
        buf = GetComponent<Buffer>();
        whichAxisLocked = 0;
    }

    private void FixedUpdate()
    {
        if (bs.actionAllowed != null)
        {
            isStunned = !bs.actionAllowed;
        }

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y);

        if (isStunned == false)
        {
            inputs = GetInputs();
        }
        else
        {
            isGrounded = false;
        }

            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y);

        if (whichAxisLocked == 1 && axisX > 0) { axisX = 0; }
        if (whichAxisLocked == -1 && axisX < 0) { axisX = 0; }

        rb.velocity = new Vector2(axisX * speed, rb.velocity.y);
        if (axisY > 0) { Jump(); }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftArenaBorder, rightArenaBorder), Mathf.Clamp(transform.position.y, 0, 10000), transform.position.z);

        if (transform.position.y > 0) { rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - fallSpeedDelta); }

        switchSideCheck();
        if (sideOfSight == 1)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        //Debug.Log(inputs.Count+100);
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftArenaBorder, rightArenaBorder), Mathf.Clamp(transform.position.y, 0, 10000), transform.position.z);
    }

    private bool isGroundedCheck()
    {
        if (transform.position.y <= 0) { return true;  }
        else { return false; }
    }

    private void switchSideCheck()
    {
        if (transform.position.x < enemy.transform.position.x) { sideOfSight = 1; }
        else { sideOfSight = -1; }
    }


    private void Jump()
    {
        if (isGrounded == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("kekw");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == enemySkybox)
        {
            transform.position = new Vector3(transform.position.x - sideOfSight * flyTriggerDelta,
                transform.position.y,
                transform.position.z);
            enemy.transform.position = new Vector3(enemy.transform.position.x + sideOfSight * flyTriggerDelta,
                enemy.transform.position.y,
                enemy.transform.position.z);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        whichAxisLocked = sideOfSight;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        whichAxisLocked = 0;
    }

}
