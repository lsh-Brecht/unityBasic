using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex; //16

    //hit�� melee�� 2�� �����Ƿ�
    public enum Sfx { Dead, Hit, LevelUp = 3, Lose, Melee, Range = 7, Select, Win };

    void Awake() {
        instance = this;
        Init();
    }
    void Init() {
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform; //�ڽ�����
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.clip = bgmClip;
        bgmPlayer.volume = bgmVolume;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        //ȿ����(sfx)
        GameObject sfxObject = new GameObject("sfxPlayer");
        sfxObject.transform.parent = transform; //�ڽ�����
        sfxPlayers = new AudioSource[channels];
        for (int i = 0; i < sfxPlayers.Length; i++) {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].bypassListenerEffects = true;
            sfxPlayers[i].volume = sfxVolume;
        }
    }

    public void PlayBGM(bool isPlay) {
        if (isPlay) {
            bgmPlayer.Play();
        }
        else {
            bgmPlayer.Stop();
        }
    }

    public void EffectBGM(bool isPlay) {
        bgmEffect.enabled = isPlay;
    }

    public void PlaySfx(Sfx sfx) {
        for (int i = 0; i < sfxPlayers.Length; i++) {
            int loopIndex = (i + channelIndex) % sfxPlayers.Length;
            if (sfxPlayers[loopIndex].isPlaying)
                continue;
            int ranIndex = 0;
            if(sfx == Sfx.Hit || sfx == Sfx.Melee) {
                ranIndex = Random.Range(0, 2);
            }
            channelIndex = loopIndex;
            //enum���� int�� , ȿ���� variation�� �ִ� �� �������� ����
            sfxPlayers[0].clip = sfxClips[(int)sfx + ranIndex];
            sfxPlayers[0].Play();
            break;
        }
    }
}
