using UnityEngine;

public class Follow : MonoBehaviour
{
    RectTransform rect;
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        //���� ��ǥ�� ��ũ�� ��ǥ�� �ٸ�. �ٷ� ����x
        rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
    }
}
