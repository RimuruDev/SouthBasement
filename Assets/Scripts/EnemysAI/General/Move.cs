using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace EnemysAI
{
    [RequireComponent(typeof(Pathfinder))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Move : MonoBehaviour
    {
        [Header("��������� ������������")]
        [SerializeField] private Movement movement = Movement.Wandering;
        [SerializeField] private float _speed = 3; // �������� ������������ 
        [SerializeField] private float searchRate = 1f; // ������� �������� 
        [SerializeField] private bool controlMovementFromHere = true;
        private float nextSearchTime = 0f;
        public float speed
        {
            get { return _speed; }
            set
            {
                if (value <= 0f)
                {
                    _speed = 1;
                    return;
                }
                if (value > 8f)
                {
                    _speed = 8;
                    return;
                }
                _speed = value;
            }
        }
        [HideInInspector] public bool isNowWalk; //���� �� ������ 
        private bool isSleep = true; //"����" �� ������
        private bool blockStop = false;
        private bool isStopped = false; //���������� ��
        private List<Vector2> path = new List<Vector2>(); //����
        private EnemyTarget target; //������

        [Header("�������")]
        public UnityEvent onFlip = new UnityEvent();
        public UnityEvent onArrive = new UnityEvent();
        public UnityEvent onWakeUp = new UnityEvent();
        public UnityEvent onSleep = new UnityEvent();

        //���������� ��� ��������
        private bool flippedOnRight;
        private Vector2 lastPos;

        //������ �� ������ �������
        [Header("������")]
        [SerializeField] private TriggerChecker stopCheker;
        [SerializeField] private TargetSelection targetSelection;
        private Pathfinder pathfinding;
        private Grid grid;
        private Rigidbody2D rb;

        //������ ���������� ��������
        public void ResetVelocity()
        {
            //����� velocity
            if (rb != null) rb.velocity = Vector2.zero;
        }
        public void Moving()
        {
            if (!isSleep && !isStopped && pathfinding.grid.isGridCreated)
            {
                //�������� 
                if (path.Count != 0)
                {
                    transform.position = Vector2.MoveTowards(transform.position, path[0], speed * Time.deltaTime);
                    isNowWalk = true;

                    if (transform.position == new Vector3(path[0].x, path[0].y, transform.position.z))
                    {
                        path.RemoveAt(0);
                        //������� ��������� ����
                        if (pathfinding.gridChanges.Count != 0)
                        {
                            grid.grid[pathfinding.gridChanges[0].x, pathfinding.gridChanges[0].y] = 0;
                            pathfinding.gridChanges.RemoveAt(0);
                        }
                    }
                }
                else if (path.Count == 0)
                {
                    if (target != null && target.targetMoveType == TargetType.Static)
                    {
                        ResetTarget();
                        onArrive.Invoke();
                        isNowWalk = false;
                    }
                }
            }

            isNowWalk = false;
        }
        public void DynamicPathfind()
        {
            //���������� ����� ����
            if (target != null && target.targetMoveType == TargetType.Movable && (Time.time >= nextSearchTime || path.Count == 0))
            {
                ResetTarget();
                FindNewPath(target);
                SetNextSearchTime();
            }
        }
        public void CheckRotation()
        {
            //�������
            if (new Vector3(lastPos.x, lastPos.y, transform.position.z) != transform.position && isNowWalk)
            {
                if (lastPos.x < transform.position.x && flippedOnRight)
                {
                    Debug.Log("[TestFlip]: 1");
                    FlipThisObject();
                    flippedOnRight = false;
                }
                else if (lastPos.x > transform.position.x && !flippedOnRight)
                {
                    Debug.Log("[TestFlip]: 2");
                    FlipThisObject();
                    flippedOnRight = true;
                }
            }
            lastPos = transform.position;
        }

        //������ ������ ����
        public void FindNewPath(EnemyTarget target)
        {
            if (!isSleep)
            {
                if (path.Count != 0) ResetTarget();
                this.target = target;

                bool cashinPath; //����� �� "������������" ����
                if (target.targetMoveType == TargetType.Static) cashinPath = true;
                else cashinPath = false;

                path = pathfinding.FindPath(
                   new Vector2(transform.position.x / grid.nodeSize, transform.position.y / grid.nodeSize),
                   new Vector2(target.transform.position.x / grid.nodeSize, target.transform.position.y / grid.nodeSize), cashinPath);
            }
        }
        public void ResetTarget(EnemyTarget target = null)
        {
            if (target != null && target == this.target) { this.target = null; }
            path.Clear();
            pathfinding.ResetGridChanges();
        }
        private void SetNextSearchTime() { nextSearchTime = Time.time + searchRate; }

        //���� ������� � �������
        public void WakeUp()
        {
            if (isSleep)
            {
                onWakeUp.Invoke();
                isSleep = false;
            }
        }
        public void GoSleep()
        {
            if (!isSleep)
            {
                onSleep.Invoke();
                isSleep = true;
            }
        }
        public void SetStop(bool stopActive) { if (!blockStop) isStopped = stopActive; }
        public void SetBlocking(bool blockActive) { blockStop = blockActive; }
        public bool GetStop() { return isStopped; }
        public bool GetBlocking() { return blockStop; }

        //����� ��������    
        private void FlipThisObject() { if (!isStopped) { transform.Rotate(0f, 180f, 0f); onFlip.Invoke(); } }
        public void FlipOther(Transform _transform) { _transform.Rotate(180f, 0f, 0f); }

        //���������� ������
        private void Start()
        {
            pathfinding = GetComponent<Pathfinder>();
            rb = GetComponent<Rigidbody2D>();
            grid = FindObjectOfType<Grid>();
            targetSelection.onTargetChange.AddListener(FindNewPath);
            targetSelection.onResetTarget.AddListener(ResetTarget);
            if (movement == Movement.Wandering) onArrive.AddListener(targetSelection.RefindTarget);
            isStopped = false;
        }
        private void FixedUpdate() //���������� ������
        {
            if(controlMovementFromHere)
            {
                ResetVelocity();
                DynamicPathfind();
                Moving();
                CheckRotation();
            }
        }
    }
}
public enum Movement
{
    Wandering,
    StaticPursuit
}