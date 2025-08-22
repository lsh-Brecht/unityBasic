using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D coll;
    void Awake() {
        coll = GetComponent<Collider2D>();
    }
    void OnTriggerExit2D(Collider2D collision) {
        if (!collision.CompareTag("Area"))
            return;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        //Vector3 playerDir = GameManager.instance.player.inputVec;
        //inputVector�� ���ġ�ϴ� ������ ���� �߻�.

        switch (transform.tag) {
            case "Ground":
                //�Է¿� �������� �ʴ� ������� ������
                float diffy = playerPos.y - myPos.y;
                float diffx = playerPos.x - myPos.x;
                float dirx = diffx < 0 ? -1 : 1;
                float diry = diffy < 0 ? -1 : 1;
                diffx = Mathf.Abs(diffx);
                diffy = Mathf.Abs(diffy);
                if (diffx > diffy) {
                    transform.Translate(Vector3.right * dirx * 40);//(1,0,0) * dirx
                }
                else if (diffx < diffy) {
                    transform.Translate(Vector3.up * diry * 40);//(0,1,0) * dirx
                }
                break;
            case "Enemy":
                //�Է¿� �������� �ʴ� ������� ������
                if (coll.enabled) {
                    Vector3 dist = playerPos - myPos;
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    transform.Translate(ran + dist*2);
                    //transform.Translate(playerDir * 30 + new Vector3(Random.Range(-3, 3f), Random.Range(-3, 3f), 0f));
                }
                break;
        }
    }
}
