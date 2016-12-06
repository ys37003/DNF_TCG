using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public  static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(GameManager)) as GameManager;
            }

            if (instance == null)
            {
                GameObject obj = new GameObject("GameManager");
                instance = obj.AddComponent<GameManager>() as GameManager;
            }

            return instance;
        }
    }

    public enum Phase
    {
        Start,
        Cast,
        Activate,
        Finish,
        End,
    }

    private Phase phase;

    void Awake()
    {

    }

    public void NextPhase()
    {
        phase++;
    }

    IEnumerator Start()
    {
        yield return Cast();
    }

    IEnumerator Cast()
    {
        yield return Activate();
    }

    IEnumerator Activate()
    {
        yield return Finish();
    }

    IEnumerator Finish()
    {
        yield return End();
    }

    IEnumerator End()
    {
        yield return Start();
    }
}