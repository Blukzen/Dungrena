﻿using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
public abstract class AbstractAstarAI : MonoBehaviour
{
    public Vector2 targetPosition;
    public Path path;

    protected AbstractEnemy entity;
    private Seeker seeker;

    public float repathRate = 0.5f;
    private float lastRepath = float.NegativeInfinity;

    public float nextWaypointDistance = 1;
    private int currentWaypoint;

    public bool reachedEndOfPath;

    public void Awake() 
    {
        seeker = GetComponent<Seeker>();
        entity = GetComponent<AbstractEnemy>();
    }

    public void OnPathComplete(Path p) 
    {

        // Path pooling. To avoid unnecessary allocations paths are reference counted.
        // Calling Claim will increase the reference count by 1 and Release will reduce
        // it by one, when it reaches zero the path will be pooled and then it may be used
        // by other scripts. The ABPath.Construct and Seeker.StartPath methods will
        // take a path from the pool if possible. See also the documentation page about path pooling.
        p.Claim(this);
        if (!p.error) {
            if (path != null) path.Release(this);
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        } else {
            p.Release(this);
        }
    }

    public void AstarMoveToTarget() 
    {
        if (AstarPath.active == null)
            return;
        
        if (AstarPath.active.IsAnyGraphUpdateInProgress)
            return;

        if (Time.time > lastRepath + repathRate && seeker.IsDone()) 
        {
            lastRepath = Time.time;

            // Start a new path to the targetPosition, call the the OnPathComplete function
            // when the path has been calculated (which may take a few frames depending on the complexity)
            seeker.StartPath(transform.position, targetPosition, OnPathComplete);
        }

        if (path == null) 
        {
            // We have no path to follow yet, so don't do anything
            return;
        }

        // Check in a loop if we are close enough to the current waypoint to switch to the next one.
        // We do this in a loop because many waypoints might be close to each other and we may reach
        // several of them in the same frame.
        reachedEndOfPath = false;
        // The distance to the next waypoint in the path
        float distanceToWaypoint;
        while (true) {
            // If you want maximum performance you can check the squared distance instead to get rid of a
            // square root calculation. But that is outside the scope of this tutorial.
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance) {
                // Check if there is another waypoint or if we have reached the end of the path
                if (currentWaypoint + 1 < path.vectorPath.Count) {
                    currentWaypoint++;
                } else {
                    // Set a status variable to indicate that the agent has reached the end of the path.
                    // You can use this to trigger some special code if your game requires that.
                    reachedEndOfPath = true;
                    break;
                }
            } else {
                break;
            }
        }

        // Slow down smoothly upon approaching the end of the path
        // This value will smoothly go from 1 to 0 as the agent approaches the last waypoint in the path.
        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;

        // Direction to the next waypoint
        // Normalize it so that it has a length of 1 world unit
        Vector2 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        // Multiply the direction by our desired speed to get a velocity
        Vector3 velocity = dir * speedFactor;

        // Move the agent using the CharacterController component
        // Note that SimpleMove takes a velocity in meters/second, so we should not multiply by Time.deltaTime
        entity.SetMovement(velocity);
        //entity.AddVelocity(dir, entity.maxSpeed);
        // If you are writing a 2D game you may want to remove the CharacterController and instead modify the position directly
        //transform.position += velocity * Time.deltaTime;
    }
}
