using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    RectTransform Transform;

    [SerializeField]
    [Tooltip("A larger number means a slower spin.")]
    private int SpinSpeed;         

    int SpinIncrement = 0;

    // Start is called before the first frame update
    void Start()
    {
        RectTransform Transform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SpinIncrement < SpinSpeed) SpinIncrement += 1;
        else
        {
            this.transform.Rotate(new Vector3(0, 0, -45));
            SpinIncrement = 0;
        }
    }
}
