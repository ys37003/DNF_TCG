using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sort : MonoBehaviour
{
    public List<UITexture> TexList = new List<UITexture>();
    public string forder = "Act";
    public string naming = "etr";

    [ContextMenu("Init")]
    void Init()
    {
        for (int i = 0; i < TexList.Count; ++i)
        {
            TexList[i].mainTexture = (Texture)Resources.Load(string.Format("CardList/{0}/{1}_{2}", forder, naming, (i+1).ToString("000")));
        }
    }
}
