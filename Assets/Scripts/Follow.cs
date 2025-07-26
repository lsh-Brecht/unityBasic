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
        //월드 좌표와 스크린 좌표가 다름. 바로 대입x
        rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
    }
}
