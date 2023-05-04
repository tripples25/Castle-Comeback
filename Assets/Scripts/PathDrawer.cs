using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathDrawer : MonoBehaviour
{
    private Camera camera;
    private LineRenderer lineRenderer;
    private Path path = new();
    private EntityType entityType;
    public Action<Path> OnNewPathCreated;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        camera = Camera.main;
        entityType = GetComponent<Entity>().entityType;
    }

    private void OnMouseUp()
    {
        if (GetComponent<Player>().isPathCreated) return;
        
        var ray = camera.ScreenPointToRay(Input.mousePosition);
        var hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

        //TODO: собрать часть условия в отдельные методы для верификации
        if (hit.collider is not null && hit.collider.TryGetComponent<Target>(out var target) && !target.isLocked && (target.entityType == entityType || target.entityType == EntityType.Both))
        {
            GetComponent<Player>().isPathCreated = true;
            target.isLocked = true;
            OnNewPathCreated(path);
        }
        else
        {
            path.Length = 0;
            path.Points.Clear();
            lineRenderer.positionCount = path.Points.Count;
            lineRenderer.SetPositions(path.Points.ToArray());
        }
    }

    private void OnMouseDrag()
    {
        if (GetComponent<Player>().isPathCreated) return;
        
        var point = camera.ScreenToWorldPoint(Input.mousePosition);
        point = new Vector3(point.x, point.y, 0);
        var distance = DistanceToLastPoint(point);

        if (path.Points.Count == 0 || distance > 0.1f)
        {
            path.Points.Add(point);
            path.Length += distance;

            lineRenderer.positionCount = path.Points.Count;
            lineRenderer.SetPositions(path.Points.ToArray());
        }
    }

    private float DistanceToLastPoint(Vector3 point)
    {
        if (path.Points.Any())
            return Vector3.Distance(path.Points.Last(), point);
        return 0;
    }
}
