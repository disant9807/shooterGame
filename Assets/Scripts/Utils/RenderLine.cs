using UnityEngine;

public static class RenderLine 
{
    public static void DrawSector(int angle, float radius, float lineWidth, LineRenderer lineRenderer)
    {
        Debug.Log(radius);
        var segments = angle;
        
        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = segments + 1;
        var pointCount = segments + 2;
        var points = new Vector3[pointCount];

        int pointHalf = pointCount / 2;

        for (int i = 0; i < pointHalf; i++)
        {
            if (i == pointHalf - 2)
            {
                points[i] = new Vector3(0, 0);
            }
            else
            {
                var rad = Mathf.Deg2Rad * ( i + 90);
                points[i] = new Vector3(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius, 0);
            }
        }
        for (int i = pointCount-1; i > pointHalf; i--)
        {
            if ( i == pointCount-2)
            {
                points[i] = new Vector3(0, 0);
            }
            else
            {
                var rad = Mathf.Deg2Rad * ( pointHalf - i + 90);
                points[i] = new Vector3(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius, 0);
            }
        }        
        
        lineRenderer.SetPositions(points);
    }

    public static void DrawLinePoints(Vector3[] points, float lineWidth, LineRenderer lineRenderer, bool worldSpace)
    {
        lineRenderer.useWorldSpace = worldSpace;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = points.Length;
        
        lineRenderer.SetPositions(points);
    }

    public static void DrawCircle(float radius, float lineWidth, LineRenderer lineRenderer)
    {

        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = 360;

        var pointCount = 360;
        var points = new Vector3[pointCount];

        int pointHalf = pointCount / 2;

        for (int i = 0; i < pointHalf; i++)
        {

            var rad = Mathf.Deg2Rad * ( i + 90);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius, 0);
            
        }
        
        
        lineRenderer.SetPositions(points);
    }

    public static void ClearPoints(LineRenderer lineRenderer)
    {
        lineRenderer.positionCount = 0;
    }
}
