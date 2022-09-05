using UnityEngine;
using System.Collections;

public enum PointRotationTargetType
{
    Mouse,
    Other
}
public class PointRotation : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] private PointRotationTargetType targetType;
    [SerializeField] private bool usePlayerAsTarget;
    [SerializeField] private Transform target; 
    [SerializeField] private bool useLocalPosition; //����� �� �������������� ��������� ������� ��� ����������
    
    [Header("Info")]
    [SerializeField] private float offset = 0f; //��������
    [SerializeField] private float coefficient = 1f; //�����������

    //������
    private bool stopRotating = false; //��������� ��������
    private Vector3 lastTargetPosition;
    [SerializeField] private float angle; //��������� ���� �������� ������� ��� ��������

    private Camera mainCamera;

    //�������
    public bool UsePlayerAsTarget => usePlayerAsTarget;
    public Transform Target => target;
    public float Offset => offset;
    public float Coefficient => coefficient;
    public PointRotationTargetType TargetType => targetType;
    
    //������
    public void SetTarget(Transform newTarget) 
    {
        if (targetType == PointRotationTargetType.Other) target = newTarget;
        else Debug.LogWarning("Rotation type is - " + targetType);
    }
    public void MultiplyCoefficent(float x) { coefficient *= x; }
    public void StopRotating(bool active, float time) { StartCoroutine(StopRotatingCoroutine(active, time)); }
    
    private IEnumerator StopRotatingCoroutine(bool active, float time)
    {
        stopRotating = active;
        yield return new WaitForSeconds(time);
        stopRotating = !active;
    }
    
    private float CalculateAngle() 
    {
        if(!stopRotating)
        {
            Vector3 targetPosition = DefineTargetPosition();
            Vector2 direction = CalculateDirection(targetPosition);

            lastTargetPosition = targetPosition;

            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            angle += offset;
            return angle;
        }
        return angle;
    }
    private Vector3 DefineTargetPosition()
    {
        if (targetType == PointRotationTargetType.Other) return target.position;
        else return new Vector3(mainCamera.ScreenToWorldPoint(Input.mousePosition).x, mainCamera.ScreenToWorldPoint(Input.mousePosition).y, 0f);
    }
    private Vector2 CalculateDirection(Vector3 targetPosition) => transform.position - targetPosition;

    //���������� ������
    private void Awake() => mainCamera = Camera.main; 
    private void FixedUpdate()
    {
        //�������� 
        if (DefineTargetPosition() != lastTargetPosition)
            transform.localRotation = Quaternion.Euler(0f, 0f, coefficient * CalculateAngle());
    }

    //private void OnDrawGizmosSelected() => Gizmos.DrawWireSphere(DefineTargetPosition(), 1f);
}