using System.Collections.Generic;
using UnityEngine;

public class MomentumTracker : MonoBehaviour
{
    private List<GameObject> childs = new List<GameObject>();
    void Start()
    {
        foreach (Transform child in transform)
        {
            childs.Add(child.gameObject);
        }

        PlayerBattleStats.Instance.onMomentumChanged += RefreshMomentum;
    }

    private void RefreshMomentum()
    {
        int momentum = Mathf.Clamp(PlayerBattleStats.Instance.GetMomentum(), 0, childs.Count);

        for (int i = 0; i < childs.Count; i++)
        {
            childs[i].SetActive(i < momentum);
        }
    }
    
    void OnDestroy()
    {
        if (PlayerBattleStats.Instance != null)
        {
            PlayerBattleStats.Instance.onMomentumChanged -= RefreshMomentum;
        }
    }
}
