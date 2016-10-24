using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public enum Phase
    {
        Start,
        Cast,
        Activate,
        Finish,
        End,
    }

    private Phase phase;

    //private PlayerInfo[] = new 

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