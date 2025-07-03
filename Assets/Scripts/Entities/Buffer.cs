using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffer : MonoBehaviour
{
    [SerializeField] Player player;
    // Start is called before the first frame update
    [SerializeField] private string cur;
    [SerializeField] private int idle_timer = 0;
    private List<string> moves = new List<string>() { 
        // moves should be read from the txt file
        "6246P", "626S", "24K", "26H", "P", "K", "S", "H", "G"
    };
    private void Read()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            idle_timer = 0;
            cur += 'K';
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            idle_timer = 0;
            cur += 'S';
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            idle_timer = 0;
            cur += 'P';
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            idle_timer = 0;
            cur += 'H';
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            idle_timer = 0;
            cur += 'G';
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            idle_timer = 0;
            cur += '8';
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            idle_timer = 0;
            cur += '2';
        }
        else if (player.sideOfSight == 1)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                idle_timer = 0;
                cur += '6';
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                idle_timer = 0;
                cur += '4';
            }
            else
            {
                idle_timer += 1;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                idle_timer = 0;
                cur += '4';
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                idle_timer = 0;
                cur += '6';
            }
            else
            {
                idle_timer += 1;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Read();
        if (idle_timer > 15)
        {
            cur = "";
        }
        if (cur.EndsWith('P') || cur.EndsWith('K') || cur.EndsWith('S') || cur.EndsWith('H') || cur.EndsWith('G'))
        {
            foreach (string s in moves)
            {
                if (cur.EndsWith(s))
                {
                    Debug.Log(s);
                    cur = "";
                    break;
                }
            }
        }

    }
}
