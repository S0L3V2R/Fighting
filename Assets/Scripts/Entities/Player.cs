using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;

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

    action punch = new action(2, 1, 2);
    action moveForward = new action(0,0,0);
    action moveBackward = new action(0,0,0);
    action sit = new action(0,0,0);
    action jump = new action(0,0,0);

    private action currentAction;

    private IEnumerator doAction(float time)
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
    }

    private void FixedUpdate()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y);
        List<action> inputs = GetInputs();
        rb.velocity = new Vector2(axisX * speed, 0); 
        Debug.Log(inputs.Count+100);
        foreach (var input in inputs)
        {
            if (actionAllowed == true && input.a > 0)
            {
                Debug.Log("bang");
                currentAction = input;
                StartCoroutine(doAction(input.a));
            }
        }
        
    }

    
    private List<action> GetInputs()
    {
        axisX = Input.GetAxisRaw("Horizontal");
        axisY = Input.GetAxisRaw("Vertical");

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

        Debug.Log(axisX);

        return inputs;
    }


}
