using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitstop : MonoBehaviour
{
    [SerializeField] public bool is_active = false;
    [SerializeField] public int active_frames = 0;
    [SerializeField] public int type = -1;
    // 0 = hit
    // 1 = reversal
    // 2 = counter
    // 3 = ult
}
