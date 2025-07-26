using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;//prefabs을 보관할 변수
    List<GameObject>[] pools;//풀 담당을 하는 리스트들
    void Awake() {
        pools = new List<GameObject>[prefabs.Length];
        for (int i = 0; i < pools.Length; i++) {
            pools[i] = new List<GameObject>();
        }
    }
    public GameObject Get(int index) {
        GameObject select = null;
        foreach(GameObject item in pools[index]) {
            if(!item.activeSelf) {
                select = item; //풀에서 비활성화된 게임 오브젝트를 찾아 할당
                select.SetActive(true);
                break;
            }
        }
        if (!select) {
            //실제로 생성
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }
        return select;
    }
}
