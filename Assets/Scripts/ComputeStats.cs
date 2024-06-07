using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeStats : MonoBehaviour
{

    private RunCode runCode;
    public float meanIteration;
    public float maxIteration;
    public float minIteration;

    // Start is called before the first frame update
    void Start()
    {
        runCode = this.gameObject.GetComponent<RunCode>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ComputeIterationStats()
    {
        if (runCode.iterations.Count > 0)
        {
            meanIteration = 0f;
            maxIteration = 0f;
            minIteration = runCode.iterations[0];

            for (int i = 0; i < runCode.iterations.Count; i++)
            {
                if (runCode.iterations[i] > maxIteration)
                {
                    maxIteration = runCode.iterations[i];
                }
                if (runCode.iterations[i] < minIteration)
                {
                    minIteration = runCode.iterations[i];
                }

                meanIteration += runCode.iterations[i];
            }

            meanIteration /= runCode.iterations.Count;
        }
    }
}
