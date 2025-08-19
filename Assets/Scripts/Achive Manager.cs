using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    public GameObject uiNotice;

    enum Achive { UnlockFirstOne, UnlockSecondOne }
    Achive[] achives;
    WaitForSecondsRealtime wait; //게임이 중간에 멈춰도 흘러감

    void Awake() {
        achives = (Achive[])Enum.GetValues(typeof(Achive));
        wait = new WaitForSecondsRealtime(5);
        if (!PlayerPrefs.HasKey("MyData")) {
            Init();
        }
    }
    void Init() {
        //PlayerPrefs : 플레이어의 기본 설정을 저장하기 위해 제공되는 클래스
        PlayerPrefs.SetInt("MyData", 1);
        //PlayerPrefs.SetInt("UnlockFirstOne", 0);
        //PlayerPrefs.SetInt("UnlockSecondOne", 0);
        //for문으로 전부 0으로 초기화
        foreach (Achive achive in achives) {
            PlayerPrefs.SetInt(achive.ToString(), 0);
        }
    }
    void Start() {
        UnlockCharacter();
    }
    void UnlockCharacter() {
        for (int i = 0; i < lockCharacter.Length; ++i) {
            string achieveName = achives[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achieveName) == 1;
            lockCharacter[i].SetActive(!isUnlock);
            unlockCharacter[i].SetActive(isUnlock);
        }
    }
    void LateUpdate() {
        foreach (Achive ahive in achives) {
            CheckAchive(ahive);
        }
    }
    void CheckAchive(Achive achive) {
        bool isAchive = false;
        switch (achive) {
            case Achive.UnlockFirstOne:
                isAchive = GameManager.instance.kill >= 10;
                break;
            case Achive.UnlockSecondOne:
                isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                break;
        }
        if(isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0) {
            PlayerPrefs.SetInt(achive.ToString(), 1);
            for (int i = 0; i < uiNotice.transform.childCount; i++) {
                bool isActive = i == (int)achive;
                uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
            }
            StartCoroutine(NoticeRoutine());
        }
    }
    IEnumerator NoticeRoutine() {
        uiNotice.SetActive(true);
        yield return wait;
        uiNotice.SetActive(false);
    }
}
