using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spotlight : MonoBehaviour {
    [SerializeField]
    internal Transform target;
    private void OnEnable()
    {
        if (target != null)
        {
            transform.position = target.position + Vector3.back * 2 + Vector3.up * 2 + Vector3.left;
        }
    }
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + Vector3.back * 1 + Vector3.up * 1 + Vector3.left * 0.25f,  Time.deltaTime / 0.1f);
        //transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z), Time.deltaTime / 0.3f);
    }
}
