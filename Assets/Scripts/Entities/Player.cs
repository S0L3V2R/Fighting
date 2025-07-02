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
    [SerializeField] private float sideOfSight;
    [SerializeField] private float collisionDelta;
    [SerializeField] private float whichAxisLocked;
    [SerializeField] private float leftArenaBorder, rightArenaBorder;

    [SerializeField] private GameObject enemySkybox;

    [SerializeField] private LayerMask movementLayer;

    [SerializeField] private bool isGrounded;

    [SerializeField] private Collider2D movementCollider;

    [SerializeField] private GameObject enemy;
    

    private Rigidbody2D rb;

    private float axisX, axisY;


    [SerializeField] bool actionAllowed = true;
    
    
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
        Debug.Log("kek");
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        whichAxisLocked = 0;
    }

    private void FixedUpdate()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y);
        inputs = GetInputs();

        if (whichAxisLocked == 1 && axisX > 0) { axisX = 0; }
        if (whichAxisLocked == -1 && axisX < 0) { axisX = 0; }

        rb.velocity = new Vector2(axisX * speed, rb.velocity.y);
        if (axisY > 0) { Jump(); }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftArenaBorder, rightArenaBorder), Mathf.Clamp(transform.position.y, 0, 10000), transform.position.z);

        if (transform.position.y > 0) { rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - fallSpeedDelta); }

        switchSideCheck();
        if (sideOfSight == 1)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
        }

        Debug.Log(inputs.Count+100);

        foreach (var input in inputs)
        {
            if (actionAllowed == true && input.a > 0)
            {
                Debug.Log("bang");
                currentAction = input;
                StartCoroutine(windupAction(input.a));
            }
        }
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


    private void flyTriggerCheck()
    {

    }

    private List<action> GetInputs()
    {
        axisX = Input.GetAxisRaw("Horizontal");
        axisY = Input.GetAxisRaw("Vertical");
        isGrounded = isGroundedCheck();

        List<action> inputs = new List<action>();


        if (Input.GetKeyDown(KeyCode.J))
        {
            inputs.Add(punch);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            inputs.Add(moveBackward);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            inputs.Add(punch);
        }
        

        Debug.Log(axisX + 1000);
        Debug.Log(axisY + 2000);

        return inputs;
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
        Debug.Log("kekw");
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
        if (sideOfSight == 1)
        {
            whichAxisLocked = 1;
        }
        else
        {
            whichAxisLocked = -1;
        }
        Debug.Log("workin");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        whichAxisLocked = 0;
    }

}
