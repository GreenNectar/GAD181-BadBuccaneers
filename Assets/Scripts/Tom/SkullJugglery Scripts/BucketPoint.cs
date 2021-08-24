using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketPoint : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Skull"))
        {
            Destroy(other.gameObject);
        }
    }
}
