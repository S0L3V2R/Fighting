using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Buffer : MonoBehaviour
{
    [SerializeField] Player player;
    // Start is called before the first frame update
    [SerializeField] private string cur;
    [SerializeField] private List<int> normal_penalty = new List<int>() { -1, -1, -1, -1, -1 };
    [SerializeField] private List<int> special_penalty = new List<int>() { -1, -1, -1, -1, -1 };
    [SerializeField] public int move_number;
    [SerializeField] private int idle_timer = 0;
    private List<string> moves = new List<string>() { 
        // moves should be read from the txt file
        "6246P", "626S", "24K", "26H", "6H", "P", "K", "S", "H", "G"
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
        else if (Input.GetKeyDown(KeyCode.S))
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
        int cnt_normals = 0;
        if (idle_timer > 30)
        {
            cur = "";
        }
        if (cur.Length > 5)
        {
            cur = cur.Substring(1);
        }
        if (cur.EndsWith('P') || cur.EndsWith('K') || cur.EndsWith('S') || cur.EndsWith('H') || cur.EndsWith('G'))
        {
            int i = 0;
            foreach (string s in moves)
            {
                if (cur.EndsWith(s))
                {
                    Debug.Log(s);
                    move_number = i;
                    //DO THE MOVE HERE


                    //PENALTY TRACKERS
                    if (s.Length < 3)
                    {
                        cnt_normals++;
                        for (int j = 0; j < 4; j++)
                        {
                            normal_penalty[j] = normal_penalty[j + 1];
                        }
                        normal_penalty[4] = i;
                    } else
                    {
                        while (cnt_normals > 3)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                special_penalty[j] = special_penalty[j + 1];
                            }
                            special_penalty[4] = -1;
                            cnt_normals -= 3;
                        }
                        cnt_normals = 0;
                        for (int j = 0; j < 4; j++)
                        {
                            special_penalty[j] = special_penalty[j + 1];
                        }
                        special_penalty[4] = i;
                        for (int k = 0; k < 3; k++)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                normal_penalty[j] = normal_penalty[j + 1];
                            }
                            normal_penalty[4] = -1;
                        }
                    }
                    cur = "";
                    break;
                }
                i++;
            }
        }

    }
}
