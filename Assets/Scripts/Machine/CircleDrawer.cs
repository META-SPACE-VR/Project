using UnityEngine;
using UnityEngine.EventSystems;

public class CircleDrawer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public LineRenderer lineRenderer;
    public Camera uiCamera;
    public Canvas canvas;
    public int segments = 100;
    public float radius = 50f;

    private bool isDrawing = false;
    private Vector3 centerPoint;

    void Start()
    {
        lineRenderer.positionCount = segments + 1;
        lineRenderer.useWorldSpace = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        centerPoint = eventData.pointerCurrentRaycast.worldPosition;
        isDrawing = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDrawing = false;
    }

    void Update()
    {
        if (isDrawing)
        {
            DrawCircle(centerPoint, radius);
        }
    }

    void DrawCircle(Vector3 center, float radius)
    {
        float angle = 20f;

        for (int i = 0; i < segments + 1; i++)
        {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            float y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            lineRenderer.SetPosition(i, new Vector3(x, y, 0) + center);

            angle += (360f / segments);
        }
    }
}
