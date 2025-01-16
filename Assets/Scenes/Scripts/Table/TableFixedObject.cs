using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TableFixedObject : MonoBehaviour
{
    public GameObject table;
    



    // Start is called before the first frame update
    void Start()
    {
        if (table != null)
        {
            StartMovingTable();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    void StartMovingTable()
    {
        foreach (Transform child in table.transform)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }
        }
    }

    void StopMovingTable()
    {
        foreach (Transform child in table.transform)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }
        }
    }
}
