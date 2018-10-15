using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
class GenericAI : MonoBehaviour
{
    public Transform targetPosition;

    public void Start() {
        // Get a reference to the Seeker component we added earlier
        Seeker seeker = GetComponent<Seeker>();

        // Start to calculate a new path to the targetPosition object, return the result to the OnPathComplete method.
        // Path requests are asynchronous, so when the OnPathComplete method is called depends on how long it
        // takes to calculate the path. Usually it is called the next frame.
        seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
    }

    public void OnPathComplete(Path p) {
        Debug.Log("Yay, we got a path back. Did it have an error? " + p.error);
    }
}
