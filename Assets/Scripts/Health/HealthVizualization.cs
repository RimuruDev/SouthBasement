using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthVizualization : MonoBehaviour
{
    [SerializeField] private Health health;

    [Header("���������� �������")]
    [SerializeField] private Color damageColor; //���� ��� ��������� �����
    [SerializeField] private Color healColor; //���� ��� ����������� ��������
    [SerializeField] protected string destroySound; // ���� ������
    [SerializeField] protected string damageSound; // ���� ��������� �����
    [SerializeField] protected string healSound; // ���� ����������� ��������
    private SpriteRenderer spriteRenderer;

    public void TakeHitVizualization(int health, int maxHealth) => StartCoroutine(TakeHitVizualizationCoroutine());
    public void HealVizualization(int health, int maxHealth) => StartCoroutine(HealVizualizationCoroutine());
    private IEnumerator TakeHitVizualizationCoroutine()
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(0.6f);
        spriteRenderer.color = new Color(100, 100, 100, 100);
    }
    private IEnumerator HealVizualizationCoroutine()
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(0.6f);
        spriteRenderer.color = new Color(100, 100, 100, 100);
    }

    private void Start()
    {
        if(health != null)
        {
            health.onTakeHit.AddListener(TakeHitVizualization);
            health.onHeal.AddListener(HealVizualization);
        }
    }
}
