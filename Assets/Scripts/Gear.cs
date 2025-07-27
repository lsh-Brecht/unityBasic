using System.Collections;
using UnityEngine;

//해결해야 하는 문제: 기어를 얻거나 레벨업 할 때마다 무기에 그 수치가 반영됨
//따라서 기어를 전부 업그레이드한 상태라면 무기에 아무런 영향을 줄 수 없게 됨.
//->Weapon.cs에서 player.BroadcastMessage("ApplyGear");을 추가해 해결
public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;
    public void Init(ItemData data) {
        //Basic
        name = "Gear " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        //Property
        type = data.itemType;
        rate = data.damages[0];
        ApplyGear(); //처음 장비를 착용할 때
    }
    public void LevelUp(float rate) {
        this.rate = rate;
        ApplyGear(); //레벨업할 때
    }
    void ApplyGear() {
        switch(type){
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
        }
    }

    void RateUp() {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();
        foreach (Weapon weapon in weapons) {
            switch (weapon.id) {
                case 0:
                    float speed = 150 * Character.WeaponSpeed;
                    weapon.speed = speed + (speed * rate);
                    break;
                default:
                    speed = 0.5f * Character.WeaponRate;
                    weapon.speed = speed * (1f - rate);
                    break;
            }
        }
    }

    void SpeedUp() {
        float speed = 3 * Character.Speed ;
        GameManager.instance.player.speed = speed + speed * rate;
    }

}
