using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindScript : MonoBehaviour
{
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.GetComponent<Rigidbody>() != null)
        {
            col.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * 20f);
        }
        Destroy(gameObject);
    }
}
