using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Creature.Moving
{
    [RequireComponent(typeof(Rigidbody2D))]
    [AddComponentMenu("Creature/General/Moving/Move")]
    public class Move : MonoBehaviour
    {
        [Header("�������")]
        public UnityEvent onBeginingOfMoving = new UnityEvent(); //������ ������
        public UnityEvent onArrive = new UnityEvent(); //����� ������ � �������� ����� ����
        
        //��������� ����������
        private List<Vector2> path = new List<Vector2>(); //���� �� �������� ���� ������
        private bool isNowWalk = false; //���� �� ������ ������
        private List<string> stops = new List<string>();
        new private Rigidbody2D rigidbody;
        
        //�������� ����� ������
        public void Moving(float speed = 3f)
        {
            rigidbody.velocity = Vector2.zero;
            rigidbody.angularVelocity = 0f;

            if (path.Count != 0 && stops.Count == 0)
            {
                if (!isNowWalk) onBeginingOfMoving.Invoke();
                transform.position = Vector2.MoveTowards(transform.position, path[0], speed * Time.deltaTime);
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

        //�������, �������
        public void SetPath(List<Vector2> path) => this.path = path;
        public void AddStop(string key)
        {
            if (stops.Contains(key)) Debug.LogWarning("Stops already contain key - " + key);
            else stops.Add(key);
        }
        public void RemoveAStop(string key) 
        {
            if (stops.Contains(key)) stops.Remove(key);
            else Debug.LogWarning("Stops don't contain key - " + key );
        }
        
        //���������� ������
        private void Awake() => rigidbody = GetComponent<Rigidbody2D>();
    }
}