using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;
    Player player;
    void Awake() {
        player = GameManager.instance.player;
    }
    void Update() {
        if (!GameManager.instance.isLive) return;
        switch (id) {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;
                if(timer > speed) {
                    timer = 0f;
                    Fire();
                }
                break;
        }
    }
    public void LevelUp(float damage, int count) {
        this.damage = damage * Character.Damage;
        this.count += count;
        if (id == 0)
            Locate();
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }
    public void Init(ItemData data) {
        //Basic
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        //Property
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;
        for(int i = 0; i < GameManager.instance.pool.prefabs.Length; i++) {
            if(data.projectile == GameManager.instance.pool.prefabs[i]) {
                prefabId = i;
                break;
            }
        }
        switch (id) {
            case 0:
                speed = 150 * Character.WeaponSpeed;
                Locate();
                break;
            default:
                speed = 0.7f * Character.WeaponRate; ; //발사 주기
                break;
        }
        //Hand Set
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }
    void Locate() {
        for (int i = 0; i < count; i++) {
            Transform bullet;
            if (i < transform.childCount)
                bullet = transform.GetChild(i);
            else {
                //기존 오브젝트 활용 후 필요하면 더 만들라고 요청
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);// -1 is Infinity Per(영구적임)
        }
    }
    //player가 부모
    void Fire() {
        if (!player.scanner.nearestTarget)
            return;
        Vector3 targetPosition = player.scanner.nearestTarget.position;
        Vector3 dir = targetPosition - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.GetComponent<Bullet>().Init(damage, count, dir); // count가 per(관통)로 사용됨
    
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
