using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_System : MonoBehaviour
{
    [SerializeField] public bool actionAllowed;

    [SerializeField] private int whichPlayer;

    [SerializeField] private Animator animator;

    private List<action> lastFrameInputs;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public class action
    {
        public float s, a, r;

        public action(float s, float a, float r)
        {
            this.s = s;
            this.a = a;
            this.r = r;
        }
    }

    action punch = new action(7, 2, 5);
    action moveForward = new action(0, 0, 0);
    action moveBackward = new action(0, 0, 0);
    action sit = new action(0, 0, 0);
    action jump = new action(0, 0, 0);

    action nothing = new action(0, 0, 0);

    private action currentAction;
    private IEnumerator windupAction(float time)
    {
        actionAllowed = false;
        animator.SetTrigger("Punch");
        yield return new WaitForSeconds(time);
        StartCoroutine(activeAction(currentAction.s));
        
    }

    private IEnumerator activeAction(float time)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(recoveryAction(currentAction.r));
    }

    private IEnumerator recoveryAction(float time)
    {
        yield return new WaitForSeconds(time);
        actionAllowed = true;
        bam();
    }

    private void bam()
    {
        Debug.Log("kek");
    }

    private void FixedUpdate()
    {
        lastFrameInputs = frameInputs();

        if (actionAllowed == true)
        {
            if (lastFrameInputs.Count > 1) 
            {
                Debug.Log(lastFrameInputs.Count);
                currentAction = lastFrameInputs[1]; 
            }
        }
        else
        {
            currentAction = nothing;
        }

        if (currentAction == null) { currentAction = nothing; }

        if (currentAction.a > 0)
        {
            StartCoroutine(windupAction(currentAction.a));
        }
    }

    private List<action> frameInputs()
    {
        List<action> inputs = new List<action>();

        inputs.Add(nothing);
        
        if (whichPlayer == 1)
        {
            if (Input.GetKeyDown(KeyCode.J)) { inputs.Add(punch); }
        }

        if (whichPlayer == 2)
        {
            if (Input.GetKeyDown(KeyCode.Keypad1)) { inputs.Add(punch); }
        }
        
        return inputs;
    }
}
