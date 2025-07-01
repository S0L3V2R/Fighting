using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
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
    action moveForward = new action(0, 0, 0);
    action moveBackward = new action(0, 0, 0);
    action sit = new action(0, 0, 0);
    action jump = new action(0, 0, 0);

    private action currentAction;

    private IEnumerator windupAction(float time)
    {
        actionAllowed = false;
        yield return new WaitForSeconds(time / 60);
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

    private void bam()
    {
        Debug.Log("kek");
    }


    private List<action> GetInputs()
    {
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

        return inputs;
    }

}
