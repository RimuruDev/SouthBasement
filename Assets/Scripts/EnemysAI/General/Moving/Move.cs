using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EnemysAI.Moving
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Move : MonoBehaviour
    {
        [Header("��������� ��������")]
        [SerializeField] private float speed = 3; // �������� ������������ 
        public UnityEvent onBeginingOfMoving = new UnityEvent(); //������ ������
        public UnityEvent onArrive = new UnityEvent(); //����� ������ � �������� ����� ����
        
        private List<Vector2> path = new List<Vector2>(); //���� �� �������� ���� ������
        private bool isNowWalk = false; //���� �� ������ ������
        private bool isStopped = false;
        private bool blockingStop = false;
        new private Rigidbody2D rigidbody;

        public float Speed
        {
            get => speed;
            set
            {
                if (value <= 0f) value = 1;
                if (value > 8f) value = 8;
                
                speed = value;
            }
        }
        public void SetPath(List<Vector2> path) => this.path = path;
        public void SetStop(bool stopped) { if (!blockingStop) isStopped = stopped; }
        public void BlockStop(bool blocked) => blockingStop = blocked; 
        public bool IsStopped => isStopped;
        public void Moving()
        {
            rigidbody.velocity = Vector2.zero;
            rigidbody.angularVelocity = 0f;

            if (path.Count != 0 && !isStopped)
            {
                if (!isNowWalk) onBeginingOfMoving.Invoke();
                transform.position = Vector2.MoveTowards(transform.position, path[0], Speed * Time.deltaTime);
                isNowWalk = true;

                if (transform.position == new Vector3(path[0].x, path[0].y, transform.position.z))
                    path.RemoveAt(0);
            }
            else
            {
                isNowWalk = false;
                onArrive.Invoke();
            }
        }

        private void Awake() => rigidbody = GetComponent<Rigidbody2D>();
    }
}