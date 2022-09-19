using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class BitSwapTest : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        int size = (sizeof(ulong) * 8) -1;
        ulong u = 1ul << size;
        Debug.Log(Convert.ToString((long)u, toBase: 2));

        u = swapBits(u) ;
        Debug.Log(Convert.ToString((long)u, toBase: 2));

        u = swapBits(u, true);
        Debug.Log(Convert.ToString((long)u, toBase: 2));
    }

    private ulong swapBits(ulong info, bool inverse = false)
    {
        ulong res = 0;
        int size = (sizeof(ulong) * 8) - 1;
        if (inverse)
        {
            res = (info >> 1) + (((info & 1ul) == 0 ? 0ul : 1ul) << size);
        }
        else
        {
            res = (info << 1) + ((info & (1ul << size)) == 0 ? 0ul : 1ul);
        }
        return res;
    }
}
