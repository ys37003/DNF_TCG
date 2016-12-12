using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    private static TitleManager instance;
    public  static TitleManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(TitleManager)) as TitleManager;
            }

            if (instance == null)
            {
                GameObject obj = new GameObject("TitleManager");
                instance = obj.AddComponent<TitleManager>() as TitleManager;
            }

            return instance;
        }
    }

    public UIInput InputId, InputPassword;
    public UIButton BtnLogin, BtnCreateAccount;

    void Awake()
    {
        EventDelegate.Add(BtnLogin.onClick, onClickLogin);
    }

    private void onClickLogin()
    {
        SceneManager.LoadScene("Robby");
    }
}