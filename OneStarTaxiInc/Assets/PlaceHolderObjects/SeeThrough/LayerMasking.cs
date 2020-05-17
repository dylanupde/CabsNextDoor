using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class LayerMasking : MonoBehaviour
{
    public GameObject camera;
    public GameObject target;
    public LayerMask mylayermask;

    void Update()
    {
        RaycastHit hit;

        //Does the ray intersect with the sphere?
        if (Physics.Raycast(camera.transform.position,
            (target.transform.position - camera.transform.position).normalized,
            out hit, Mathf.Infinity, mylayermask))
        {
            //if it collides with the wall, scale it to 10 with Dotween
            if (hit.collider.gameObject.tag == "wall")
            {
                target.transform.DOScale(10, 2);
            }
        }
        //else if it doesn't, scale it to 0
        else
        {
            target.transform.DOScale(0, 2);
        }
    }
}
