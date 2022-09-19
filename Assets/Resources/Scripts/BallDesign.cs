using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDesign : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 6; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                NormalBall n = new NormalBall(0, (i * 10) + j);
                n.BallObject.transform.Translate((j - 5)*1.5f, (i - 3)*1.5f, 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
