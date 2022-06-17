using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetSelection : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private TargetType targetMoveType = TargetType.Static; //��������� �� ������
    public TriggerCheker areaCheker; //������� � ������� ���� ����� ���� �� �������
    [SerializeField] private List<EnemyTarget> targets; //����� ��� ������������

    //������
    public UnityEvent SetTarget = new UnityEvent();
    public UnityEvent ResetTarget = new UnityEvent();

    private EnemyTarget FindNewTarget()
    {
        bool isSamePriority = true;
        EnemyTarget target = null;
        int priority = targets[0].priority;

        for (int i = 0; i < targets.Count; i++)
        //��������� ��� ������� �� ����������
        {
            if (targets[i].priority == priority) continue;
            else //���� ��������� �� ����������
            {
                isSamePriority = false;
                target = targets[targets.Count - 1];
                break;
            }
        }
        if (isSamePriority)
        //���� � ���� �������� ���������� ���������
        {
            int rand = Random.Range(0, targets.Count);
            target = targets[rand];
        }
        //������� ��������� � ������� ���� ������ �� �����
        else FindObjectOfType<RatConsole>().DisplayText("������ �� ��� ������", Color.red,
            RatConsole.Mode.ConsoleMessege, "<AngryRatAI.cs, line 132>");

        //�� ����� ��� ���� ��� ������ �������
        return target;
    }
    public void OnAreaExit(GameObject obj) //����� ������� ����� ���������� ��� ������ �� ������� ���� ������ �����
    {
        if (obj.TryGetComponent(typeof(EnemyTarget), out Component comp))
        {
            if (targets.Contains(obj.GetComponent<EnemyTarget>()))
            {
                //���� ������ ������� ����� �� ������� ���� ������ ����������� ������, �� �� ��������
                if (target == obj.GetComponent<EnemyTarget>()) ResetTarget.Invoke();
                
                targets.Remove(obj.GetComponent<EnemyTarget>());
            }
        }
    }
    public void OnAreaEnter(GameObject obj) //����� ������� ����� ���������� ��� ����� � ���� ������
    {
        if (obj.TryGetComponent(typeof(EnemyTarget), out Component comp))
        {
            EnemyTarget newTarget = obj.GetComponent<EnemyTarget>();
            if (!targets.Contains(newTarget))
            {
                targets.Add(newTarget);
                QuickSort(targets, 0, targets.Count - 1);
                target = FindNewTarget().transform;
            }
        }
    }

    //������ QuickSort-�
    private List<EnemyTarget> QuickSort(List<EnemyTarget> targets, int minIndex, int maxIndex)
    {
        if (minIndex >= maxIndex) return targets;

        int pivot = GetPivotInd(targets, minIndex, maxIndex);

        QuickSort(targets, minIndex, pivot - 1);
        QuickSort(targets, pivot + 1, maxIndex);

        return new List<EnemyTarget>();
    }
    private int GetPivotInd(List<EnemyTarget> targets, int minIndex, int maxIndex)
    {
        int pivot = minIndex - 1;

        for (int i = minIndex; i <= maxIndex; i++)
        {
            if (targets[i].priority < targets[maxIndex].priority)
            {
                pivot++;
                Swap(ref targets, pivot, i);
            }
        }

        pivot++;
        Swap(ref targets, pivot, maxIndex);

        return pivot;
    }
    private void Swap(ref List<EnemyTarget> targets, int firstInd, int secondInd)
    {
        EnemyTarget tmp = targets[firstInd];
        targets[firstInd] = targets[secondInd];
        targets[secondInd] = tmp;
    }
}
