using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PathCreation;
using UnityEngine;
using UnityEngine.AI;

public class PathMover : MonoBehaviour
{
    private Queue<Vector3> pathPoints = new();
    private SpriteRenderer spriteRenderer;
    private Vector3? currentPoint;
    private float currentPos;
    private float speed = 5f;
    private Path path;
    public float timeToTarget = 10;
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        GetComponent<PathDrawer>().OnNewPathCreated += SetPath;
        GameManager.Instance.OnAllPathsCreated += StartMoveCoroutine;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnAllPathsCreated -= StartMoveCoroutine;
    }

    private void StartMoveCoroutine()
    {
        StartCoroutine(MoveAlongPath());
    }
    
    private void SetPath(Path path)
    {
        this.path = path;
        pathPoints = new Queue<Vector3>(path.Points);
    }

    private IEnumerator MoveAlongPath()
    {
        print(path.Length / timeToTarget);
        var animator = GetComponent<Animator>();
        animator.SetBool(IsWalking, true);
        speed = path.Length / timeToTarget;
        
        while (currentPos < path.Length)
        {
            currentPos += speed * Time.deltaTime;
            
            float distanceTraveled = 0.0f;
            Vector3 startPos = Vector3.zero;
            Vector3 endPos = Vector3.zero;

            for (int i = 0; i < path.Points.Count - 1; i++)
            {
                float segmentLength = Vector3.Distance(path.Points[i], path.Points[i+1]);
                if (distanceTraveled + segmentLength >= currentPos)
                {
                    startPos = path.Points[i];
                    endPos = path.Points[i+1];
                    break;
                }
                distanceTraveled += segmentLength;
            }
            
            float segmentPos = (currentPos - distanceTraveled) / Vector3.Distance(startPos, endPos);
            Vector3 newPos = Vector3.Lerp(startPos, endPos, segmentPos);
            
            spriteRenderer.flipX = endPos.x - startPos.x < 0;

            transform.position = newPos == Vector3.zero ? path.Points.Last() : newPos;

            yield return new WaitForEndOfFrame();
        }
        
        animator.SetBool(IsWalking, false);
        GetComponent<Player>().isPathCompleted = true;
        print(DateTime.Now.ToString("fff"));
    }

    private void UpdatePathing()
    {
        if (ShouldSetDestination())
            currentPoint = pathPoints.Dequeue();
    }

    private bool ShouldSetDestination()
    {
        if (pathPoints.Count == 0)
            return false;

        if (currentPoint == null || Vector3.Distance(transform.position, currentPoint.Value) <= 0.1)
            return true;

        return false;
    }
}
