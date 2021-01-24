using System.Collections.Generic;
using Assets.Core.Common;
using UnityEngine;
using Assets.Core.Logic;
using Pathfinding;
using System.Linq;

namespace Assets.Logic.Motor
{
    public class MotorAStarLogic : MonoBehaviour, IMotorLogic
    {
        //public переменные

        public float speed; //Скорость хождения

        public float speedRotate; //Скорость поворота

        public bool reachedEndOfPath;

        public float repathRate = 0.5f;

        //private переменные
        private Transform transformCache; // Кешируем трансформ объекта
        private Rigidbody2D rigibodyCache;
        private Seeker seeker;
        public Path path;
        public float nextWaypointDistance = 0.5f;
        private int currentWaypoint = 0;
        private float lastRepath = float.NegativeInfinity;
        public List<List<Vector3>> addedPath; // дополнительный фейковые пути

        public void Start()
        {
            transformCache = transform;
            rigibodyCache = GetComponent<Rigidbody2D>();
            seeker = GetComponent<Seeker>();
            addedPath = new List<List<Vector3>>();
        }


        public void GoToTarget(Vector3 point)
        {
            if (Time.time > lastRepath + repathRate && seeker.IsDone())
            {
                lastRepath = Time.time;

                // Start a new path to the targetPosition, call the the OnPathComplete function
                // when the path has been calculated (which may take a few frames depending on the complexity)
                seeker.StartPath(transformCache.position, point, OnPathComplete);
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
            while (true)
            {
                // If you want maximum performance you can check the squared distance instead to get rid of a
                // square root calculation. But that is outside the scope of this tutorial.
                distanceToWaypoint = Vector2.Distance(transformCache.position, path.vectorPath[currentWaypoint]);
                if (distanceToWaypoint < nextWaypointDistance)
                {
                    // Check if there is another waypoint or if we have reached the end of the path
                    if (currentWaypoint + 1 < path.vectorPath.Count)
                    {
                        currentWaypoint++;
                    }
                    else
                    {
                        // Set a status variable to indicate that the agent has reached the end of the path.
                        // You can use this to trigger some special code if your game requires that.
                        reachedEndOfPath = true;
                        break;
                    }
                }
                else
                {
                    break;
                }
            }


            // Slow down smoothly upon approaching the end of the path
            // This value will smoothly go from 1 to 0 as the agent approaches the last waypoint in the path.
            var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;

            // Direction to the next waypoint
            // Normalize it so that it has a length of 1 world unit
            Vector3 dir = (path.vectorPath[currentWaypoint] - transformCache.position).normalized;
            // Multiply the direction by our desired speed to get a velocity
            Vector2 velocity = dir * speed * speedFactor;

            // Move the agent using the CharacterController component
            // Note that SimpleMove takes a velocity in meters/second, so we should not multiply by Time.deltaTime
            rigibodyCache.velocity += velocity * Time.deltaTime;
            transform.right = Vector2.MoveTowards(transform.right, path.vectorPath[currentWaypoint] - transformCache.position, Time.deltaTime * speedRotate);
            //Rotate(point);

            // If you are writing a 2D game you may want to remove the CharacterController and instead modify the position directly
            // transform.position += velocity * Time.deltaTime;
        }

        public void OnPathComplete(Path p)
        {
            // Path pooling. To avoid unnecessary allocations paths are reference counted.
            // Calling Claim will increase the reference count by 1 and Release will reduce
            // it by one, when it reaches zero the path will be pooled and then it may be used
            // by other scripts. The ABPath.Construct and Seeker.StartPath methods will
            // take a path from the pool if possible. See also the documentation page about path pooling.
            p.Claim(this);
            if (!p.error)
            {
                if (path != null) path.Release(this);
                path = p;
                // Reset the waypoint counter so that we start to move towards the first point in the path
                currentWaypoint = 0;
                OnAddedPathComplete(path);
            }
            else
            {
                p.Release(this);
            }

        }

        public void OnAddedPathComplete(Path p)
        {

            addedPath = new List<List<Vector3>>();

            List<Vector3?> leftList = new List<Vector3?>();
            List<Vector3?> leftOneList = new List<Vector3?>();
            List<Vector3?> leftTwoList = new List<Vector3?>();

            List<Vector3?> rightList = new List<Vector3?>();
            List<Vector3?> rightOneList = new List<Vector3?>();
            List<Vector3?> rightTwoList = new List<Vector3?>();

            for (var i = 0; i < p.vectorPath.Count; i++)
            {
                var pointVectorPath = p.vectorPath[i];

                if (i + 1 < p.vectorPath.Count)
                {
                    var dir = (p.vectorPath[i + 1] - pointVectorPath).normalized;

                    var left = new Vector2(-dir.y, dir.x);
                    var leftOne = ProjectMath.Vector2Rotate(left, -10);
                    var leftTwo = ProjectMath.Vector2Rotate(left, 10);

                    var right = new Vector2(dir.y, -dir.x);
                    var rightOne = ProjectMath.Vector2Rotate(right, -10);
                    var rightTwo = ProjectMath.Vector2Rotate(right, 10);

                    leftList.Add(checkRayDistance(Physics2D.Raycast(pointVectorPath, left), pointVectorPath));
                    leftOneList.Add(checkRayDistance(Physics2D.Raycast(pointVectorPath, leftOne), pointVectorPath));
                    leftTwoList.Add(checkRayDistance(Physics2D.Raycast(pointVectorPath, leftTwo), pointVectorPath));

                    rightList.Add(checkRayDistance(Physics2D.Raycast(pointVectorPath, right), pointVectorPath));
                    rightOneList.Add(checkRayDistance(Physics2D.Raycast(pointVectorPath, rightOne), pointVectorPath));
                    rightTwoList.Add(checkRayDistance(Physics2D.Raycast(pointVectorPath, rightTwo), pointVectorPath));


                }
            }

            addedPath.Add(leftList.Where(e => e.HasValue).Select(e => e.Value).ToList());
            addedPath.Add(leftOneList.Where(e => e.HasValue).Select(e => e.Value).ToList());
            addedPath.Add(leftTwoList.Where(e => e.HasValue).Select(e => e.Value).ToList());

            addedPath.Add(rightList.Where(e => e.HasValue).Select(e => e.Value).ToList());
            addedPath.Add(rightOneList.Where(e => e.HasValue).Select(e => e.Value).ToList());
            addedPath.Add(rightTwoList.Where(e => e.HasValue).Select(e => e.Value).ToList());

            addedPath.ForEach(e => Debug.Log(e.Count));

        }

        private Vector3? checkRayDistance(RaycastHit2D hit, Vector3 point)
        {


            float maxDistance = 0.3f;
            float distanceToStop = 0.1f;

            var distance = Vector2.Distance(hit.transform.position, point);

            var speed = distance < maxDistance ?
                distance - distanceToStop :
                maxDistance;

            var newDir = (hit.transform.position - point).normalized;

            var vectorSpeed = speed * newDir;

            return point + vectorSpeed;

        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            if (addedPath != null)
            {
                var p = addedPath[2];
                for (var i = 0; i < p.Count; i++)
                {
                    var point = p[i];

                    if (i + 1 < p.Count) Gizmos.DrawLine(point, p[i + 1]);

                }

            }
        }
    }
}
