using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class DeckEdit : MonoBehaviour
{
    public UIButton BtnDeckEdit;

    void Awake()
    {
        EventDelegate.Add(BtnDeckEdit.onClick, onClickDeckEdit);
    }

    private void onClickDeckEdit()
    {
        SceneManager.LoadScene("Robby");
    }
}