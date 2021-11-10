using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObstacles : MonoBehaviour {
    //Deletes any obstacle which spawned close to the Target and tries to avoid the case where target
    //can't be seen or is unreachable
    void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.2f);
        foreach (Collider collider in colliders) {
            if(collider.transform.tag == "obstacle") {
                Destroy(collider.transform.gameObject);
            }
        }

    }
}
