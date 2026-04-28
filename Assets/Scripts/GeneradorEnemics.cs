using UnityEngine;

public class GeneradorEnemics : MonoBehaviour
{
    public GameObject _NauEnemicPrefab;
    public GameObject _NauEnemicEspecialPrefab;

    private int _comptadorEnemics = 0;

    public void IniciGeneraEnemics()
    {
        _comptadorEnemics = 0;
        CancelInvoke("CreaEnemic"); // evita doble InvokeRepeating si es crida més d'un cop
        InvokeRepeating("CreaEnemic", 2f, 1f);
    }

    public void AturaGenerarEnemics()
    {
        CancelInvoke("CreaEnemic");
    }

    private void CreaEnemic()
    {
        _comptadorEnemics++;

        float dist = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 minP = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist));
        Vector3 maxP = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, dist));

        float posX = Random.Range(minP.x + 0.5f, maxP.x - 0.5f);
        Vector3 spawnPos = new Vector3(posX, maxP.y, 0f);

        if (_comptadorEnemics % 7 == 0 && _NauEnemicEspecialPrefab != null)
            Instantiate(_NauEnemicEspecialPrefab, spawnPos, Quaternion.identity);
        else
            Instantiate(_NauEnemicPrefab, spawnPos, Quaternion.identity);
    }
}
