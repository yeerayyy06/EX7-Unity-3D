using UnityEngine;

public class GeneradorObjectesVida : MonoBehaviour
{
    public GameObject _ObjecteVidaPrefab;

    public void IniciGeneraObjectesVida()
    {
        InvokeRepeating("CreaObjecteVida", 5f, 10f);
    }

    public void AturaGenerarObjectesVida()
    {
        CancelInvoke("CreaObjecteVida");
    }

    private void CreaObjecteVida()
    {
        float dist = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 minP = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist));
        Vector3 maxP = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, dist));

        float posX = Random.Range(minP.x + 0.5f, maxP.x - 0.5f);
        Vector3 spawnPos = new Vector3(posX, maxP.y, 0f);

        Instantiate(_ObjecteVidaPrefab, spawnPos, Quaternion.identity);
    }
}
