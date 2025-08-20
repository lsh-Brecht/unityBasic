using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;
    void Awake() {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }

    public void Show() {
        Next();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        AudioManager.instance.EffectBGM(true);
    }
    public void Hide() {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        AudioManager.instance.EffectBGM(false);
    }
    public void Select(int index) {
        items[index].OnClick();
    }
    void Next() {
        // 모든 아이템 버튼 비활성화
        foreach (Item item in items)
            item.gameObject.SetActive(false);
        // 이 중 3개 아이템 버튼만 활성화
        int[] ran = new int[3];
        while (true) {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);
            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                break;
        }
        for(int i=0; i < ran.Length; i++) {
            Item ranItem = items[ran[i]];
            // 업그레이드가 끝난 아이템은 소비아이템이 뜨도록
            //이미 소비아이템이 뽑힌 경우 2개만 뜸
            if(ranItem.level == ranItem.data.damages.Length)
                items[4].gameObject.SetActive(true);
            else
                ranItem.gameObject.SetActive(true);
        }
    }
}
