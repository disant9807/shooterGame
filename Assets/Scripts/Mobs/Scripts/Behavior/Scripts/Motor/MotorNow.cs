using UnityEngine;
using Pathfinding;

public class MotorNow : MonoBehaviour, IMotor
{
    //public переменные
    public float speed { get; set; } //Скорость хождения
    public float speedRotate { get; set; } //Скорость поворота

    //private переменные
    private Transform _thisTransform; // Кешируем трансформ объекта
    private Rigidbody2D _rigidBody;
    private float _thisRotate;
    private bool _rotateAction;
    
    private Vector3 _thisVectorRotate;
    private bool _rotateNow;
    private bool freeze = false;
    
    private Transform targetPosition;
    private Seeker seeker;
    public Path path;
    public float nextWaypointDistance = 0.5f;
    private int currentWaypoint = 0;
    public bool reachedEndOfPath;
    public float repathRate = 0.5f;
    private float lastRepath = float.NegativeInfinity;
    

    public MotorNow(Transform transform, Rigidbody2D rigidBody, float speed, float speedRotate, Transform targetPosition, Seeker seeker)
    {
        _thisTransform = transform;
        _rigidBody = rigidBody;
        _rotateNow = true;
        this.speed = speed;
        this.speedRotate = speedRotate;
        this.seeker = seeker;
        this.targetPosition = targetPosition;
    }

    public void GoToVector(Vector3 vector)
    {
        if (!freeze)
        {
            var position = _thisTransform.position;
            Vector3 direction = (vector - position);
            float angle = Vector2.Angle(Vector2.right, direction);

            // thisTransform.position += direction.normalized * speed * Time.deltaTime;

            _thisTransform.eulerAngles = new Vector3(0f, 0f, position.y < vector.y ? angle : -angle);
            _rigidBody.velocity = new Vector2(direction.x, direction.y) * (speed * Time.deltaTime);
        }
    }

    public void GoToForward(Vector3 forward)
    {
        if (!freeze)
        {
            _rigidBody.velocity = forward * (speed * Time.deltaTime);
            // _thisTransform.position += forward * (speed * Time.deltaTime);
        }
    }

    public void GoToTarget(Vector3 point)
    {
        if (!freeze)
        {

            if (Time.time > lastRepath + repathRate && seeker.IsDone())
            {
                lastRepath = Time.time;

                // Start a new path to the targetPosition, call the the OnPathComplete function
                // when the path has been calculated (which may take a few frames depending on the complexity)
                seeker.StartPath(_thisTransform.position, point, OnPathComplete);
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
                distanceToWaypoint = Vector2.Distance(_thisTransform.position, path.vectorPath[currentWaypoint]);
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
            Vector3 dir = (path.vectorPath[currentWaypoint] - _thisTransform.position).normalized;
            // Multiply the direction by our desired speed to get a velocity
            Vector2 velocity = dir * speed * speedFactor;

            // Move the agent using the CharacterController component
            // Note that SimpleMove takes a velocity in meters/second, so we should not multiply by Time.deltaTime
            _rigidBody.velocity += velocity * Time.deltaTime;
             Rotate(point);

            // If you are writing a 2D game you may want to remove the CharacterController and instead modify the position directly
            // transform.position += velocity * Time.deltaTime;
        }
    }
    
    public void GoToTarget()
    {
        if (!freeze)
        {

            if (Time.time > lastRepath + repathRate && seeker.IsDone())
            {
                lastRepath = Time.time;

                // Start a new path to the targetPosition, call the the OnPathComplete function
                // when the path has been calculated (which may take a few frames depending on the complexity)
                seeker.StartPath(_thisTransform.position, targetPosition.position, OnPathComplete);
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
                distanceToWaypoint = Vector2.Distance(_thisTransform.position, path.vectorPath[currentWaypoint]);
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
            Vector3 dir = (path.vectorPath[currentWaypoint] - _thisTransform.position).normalized;
            // Multiply the direction by our desired speed to get a velocity
            Vector2 velocity = dir * speed * speedFactor;

            // Move the agent using the CharacterController component
            // Note that SimpleMove takes a velocity in meters/second, so we should not multiply by Time.deltaTime
            _rigidBody.velocity += velocity * Time.deltaTime;
             Rotate(targetPosition.position);

            // If you are writing a 2D game you may want to remove the CharacterController and instead modify the position directly
            // transform.position += velocity * Time.deltaTime;
        }
    }

    public void OnPathComplete (Path p) {
        if (!freeze)
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
            }
            else
            {
                p.Release(this);
            }
        }
    }
    
    public void TpToPoint(Vector3 point)
    {
        if (!freeze)
        {
            _thisTransform.position = point;
        }
    }

    public void Rotate(Vector3 point)
    {
        if (!freeze)
        {
            var position = _thisTransform.position;
            float rotateDegree = Vector2.Angle(Vector2.right, point - position);
            _thisTransform.eulerAngles = new Vector3(0f, 0f, position.y < point.y ? rotateDegree : -rotateDegree);
        }
    }

    public void RotateAngle(float angle, bool forward)
    {
        if (!freeze)
        {
            // thisRigidbody.velocity = forward * speed * Time.deltaTime;
            _thisTransform.eulerAngles = new Vector3(0f, 0f, forward ? angle : -angle);
        }
    }
    public void RotateFlow(Vector3 point)
    {
        if (!freeze)
        {
            float rotateDegree = Vector2.Angle(Vector2.right, point - _thisTransform.position);

            if (rotateDegree > 1)
            {
                var rotation = _thisTransform.rotation;
                _thisTransform.eulerAngles = new Vector3(0f, 0f,
                    _thisTransform.position.y < point.y
                        ? rotation.z + speed * Time.deltaTime
                        : rotation.z + speed * Time.deltaTime * (-1));
            }
        }
    }

    public void RotateFlowAngle(float angle, bool forwardRight)
    {
        if (!freeze)
        {
            if (_rotateNow)
            {
                _thisVectorRotate = _thisTransform.right;
                _rotateNow = false;
            }
            else if (_rotateNow == false)
            {
                float thisAngle = Vector2.Angle(_thisTransform.right, _thisVectorRotate);

                if (thisAngle < angle)
                {
                    var eulerAngles = _thisTransform.eulerAngles;
                    eulerAngles = new Vector3(0f, 0f,
                        forwardRight == true
                            ? eulerAngles.z + speedRotate * Time.deltaTime
                            : eulerAngles.z + speedRotate * Time.deltaTime * (-1));
                    _thisTransform.eulerAngles = eulerAngles;
                }
                else
                {
                    _rotateNow = true;
                }
            }
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    
    public float GetSpeed()
    {
        return speed;
    }

    public void SetFreeze(bool value)
    {
        freeze = value;
    }

    public bool GetFreeze(bool value)
    {
        return freeze;
    }
}
