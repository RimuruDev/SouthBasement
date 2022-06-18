using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetSelection : MonoBehaviour
{
    public TriggerCheker areaCheker; //������� � ������� ���� ����� ���� �� �������
    [SerializeField] private EnemyTarget target;
    private TargetType targetMoveType = TargetType.Static; //��������� �� ������
    [SerializeField] private List<EnemyTarget> targets = new List<EnemyTarget>(); //����� ��� ������������
    [SerializeField] private List<string> blackTagList = new List<string>();

    //������
    public UnityEvent<EnemyTarget> SetTarget = new UnityEvent<EnemyTarget>();
    public UnityEvent<EnemyTarget> ResetTarget = new UnityEvent<EnemyTarget>();

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
    public void OnTriggerExit2D(Collider2D coll) //����� ������� ����� ���������� ��� ������ �� ������� ���� ������ �����
    {
        if (coll.TryGetComponent(typeof(EnemyTarget), out Component comp))
        {
            if (targets.Contains(coll.GetComponent<EnemyTarget>()))
            {
                //���� ������ ������� ����� �� ������� ���� ������ ����������� ������, �� �� ��������
                if (target == coll.GetComponent<EnemyTarget>()) ResetTarget.Invoke(target);

                targets.Remove(coll.GetComponent<EnemyTarget>());
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D coll) //����� ������� ����� ���������� ��� ����� � ���� ������
    {
        if (coll.TryGetComponent(typeof(EnemyTarget), out Component comp))
        {
            EnemyTarget newTarget = coll.GetComponent<EnemyTarget>();
            if (!targets.Contains(newTarget) && !blackTagList.Contains(coll.tag))
            {
                targets.Add(newTarget);
                QuickSort(targets, 0, targets.Count - 1);
                target = FindNewTarget();
                SetTarget.Invoke(target);
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
