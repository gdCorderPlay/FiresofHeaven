using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainView
{
    public Button loginBtn;
    public Button startBtn;
    public Button exitBtn;
    public GameObject tips;
    public GameObject panel;
    public Text UID;
    public MainView()
    {
        GameObject uiRoot = GameObject.Find("Canvas");
        panel = uiRoot.transform.Find("MainView").gameObject;
        startBtn = panel.transform.Find("Begin").GetComponent<Button>();
        exitBtn = panel.transform.Find("Exit").GetComponent<Button>();
        loginBtn = panel.transform.Find("Login").GetComponent<Button>();
        tips = panel.transform.Find("Tips").gameObject;
        UID = panel.transform.Find("UID").GetComponent<Text>();
        Bind();
        Reset();
    }

    void Bind()
    {
        startBtn.onClick.AddListener(OnStartClick);
        exitBtn.onClick.AddListener(OnExitClick);
        loginBtn.onClick.AddListener(OnLoginClick);
    }
    private void OnLoginClick()
    {
        loginBtn.gameObject.SetActive(false);
    }
    public void OnLoginSuccess(int uid)
    {
        startBtn.gameObject.SetActive(true);
        UID.text = "UID:" + uid;
    }

    private void OnExitClick()
    {
        startBtn.gameObject.SetActive(true);
        tips.SetActive(false);
        exitBtn.gameObject.SetActive(false);
    }

    private void OnStartClick()
    {
        startBtn.gameObject.SetActive(false);
        tips.SetActive(true);
        exitBtn.gameObject.SetActive(true);
    }

    void Reset()
    {
        panel.SetActive(true);
        loginBtn.gameObject.SetActive(true);
        startBtn.gameObject.SetActive(false);
        tips.SetActive(false);
        exitBtn.gameObject.SetActive(false);
        UID.text = "";
    }

    public void Show()
    {
        panel.SetActive(true);
    }
    public void Hide()
    {

        panel.SetActive(false);
    }
}
