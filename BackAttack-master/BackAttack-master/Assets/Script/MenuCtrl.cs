using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCtrl : MonoBehaviour {

    public GameObject Dim;
    public GameObject Menu;

    public GameObject Music;
    public GameObject Fx;
    public GameObject Blood;
    private bool isMusic;
    private bool isFx;
    private bool m_isBlood;
    public bool isBlood
    {
        get
        {
            return m_isBlood;
        }
        set
        {
            m_isBlood = value;
            DataManager.Instance.isBlood = m_isBlood;
            PlayerPrefs.SetInt("Blood", m_isBlood == true ? 1 : 2);
            PlayerPrefs.Save();
        }
    }

    private bool isOn = false;

    private void OnEnable()
    {
        isMusic = PlayerPrefs.GetInt("Music", 1) == 1 ? true : false;
        isFx = PlayerPrefs.GetInt("Fx", 1) == 1 ? true : false;
        isBlood = PlayerPrefs.GetInt("Blood", 1) == 1 ? true : false;

        if (isMusic)
        {
            Music.transform.GetChild(0).gameObject.SetActive(true);
            Music.transform.GetChild(1).gameObject.SetActive(false);
            SoundManager.Instance.SetMusic(true);
        }
        else
        {
            Music.transform.GetChild(0).gameObject.SetActive(false);
            Music.transform.GetChild(1).gameObject.SetActive(true);
            SoundManager.Instance.SetMusic(false);
        }

        if (isFx)
        {
            Fx.transform.GetChild(0).gameObject.SetActive(true);
            Fx.transform.GetChild(1).gameObject.SetActive(false);
            SoundManager.Instance.SetFX(true);
        }
        else
        {
            Fx.transform.GetChild(0).gameObject.SetActive(false);
            Fx.transform.GetChild(1).gameObject.SetActive(true);
            SoundManager.Instance.SetFX(false);
        }

        if (isBlood)
        {
            Blood.transform.GetChild(0).gameObject.SetActive(true);
            Blood.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            Blood.transform.GetChild(0).gameObject.SetActive(false);
            Blood.transform.GetChild(1).gameObject.SetActive(true);
        } 
    }
    private void OnDisable()
    {
        isOn = false;
    }

    public void MenuActive ()
    {
        if (DataManager.Instance.IsAds) return;

        isOn = !isOn;
        Dim.SetActive(isOn);
        Menu.SetActive(isOn);
    }
    public void MusicOnOff()
    {
        isMusic = !isMusic;
        if(isMusic)
        {
            Music.transform.GetChild(0).gameObject.SetActive(true);
            Music.transform.GetChild(1).gameObject.SetActive(false);
            SoundManager.Instance.SetMusic(true);
        }
        else
        {
            Music.transform.GetChild(0).gameObject.SetActive(false );
            Music.transform.GetChild(1).gameObject.SetActive(true);
            SoundManager.Instance.SetMusic(false);
        }
    }
    public void BloodOnOff()
    {
        isBlood = !isBlood;
        if (isBlood)
        {
            Blood.transform.GetChild(0).gameObject.SetActive(true);
            Blood.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            Blood.transform.GetChild(0).gameObject.SetActive(false);
            Blood.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void FxOnOff()
    {
        isFx = !isFx;
        if (isFx)
        {
            Fx.transform.GetChild(0).gameObject.SetActive(true);
            Fx.transform.GetChild(1).gameObject.SetActive(false);
            SoundManager.Instance.SetFX(true);
        }
        else
        {
            Fx.transform.GetChild(0).gameObject.SetActive(false );
            Fx.transform.GetChild(1).gameObject.SetActive(true);
            SoundManager.Instance.SetFX(false );
        }
    }
}
