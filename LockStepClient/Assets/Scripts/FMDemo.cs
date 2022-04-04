using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;
public class FMDemo : MonoBehaviour
{
    FP num1;
    public Transform cube;
    public float max;
    private TSVector vector1;
    private TSVector vector2;
    // Start is called before the first frame update
    void Start()
    {
        num1 = 0.5f;

        cube.position = new Vector3(num1.AsFloat(),num1.AsFloat(), num1.AsFloat());

        vector1 = new TSVector(0.1f,2,3);
        vector2 = new TSVector(20.2, 2, 3);
        Debug.Log(TSVector.Distance(vector1,vector2));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(num1 > max);
        }
    }
}
