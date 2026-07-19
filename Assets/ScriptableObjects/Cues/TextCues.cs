using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Cues/TextCue")]
public class TextCues : GameCue
{
    public GameObject prefab;
    public string text;
    public bool autoDestroy;
    public float destroyDelay;
    public override GameObject Execute(Vector3 position)
    {
        if (prefab != null)
        {
            GameObject pooledObj = ObjectPool.Instance.GetPooledObject(prefab);
            if (pooledObj != null)
            {
                TextMeshProUGUI textMesh = pooledObj.GetComponentInChildren<TextMeshProUGUI>();
                textMesh.text = text;
                pooledObj.transform.position = position;
                ObjectPool.Instance.StartCoroutine(ObjectPool.ReturnToPoolAfterDelay(pooledObj, destroyDelay));
                return pooledObj;
            }
            else
            {
                Debug.Log("fEEEEEEEEEEEEEEEEEE");
            }
        }
        return null;
    }

    public override GameObject Execute(Vector3 position, Sprite png)
    {
        return Execute(position);
    }
}
