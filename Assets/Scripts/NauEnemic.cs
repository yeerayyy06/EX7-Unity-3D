using UnityEngine;

public class NauEnemic : MonoBehaviour
{
    float _vel = 3f;

    public GameObject _ExplosioPrefab;

    void Update()
    {
        transform.position += Vector3.down * _vel * Time.deltaTime;

        float dist = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 minP = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist));
        if (transform.position.y < minP.y)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider objecteTocat)
    {
        if (objecteTocat.CompareTag("ProjectilJugador") || objecteTocat.CompareTag("NauJugador"))
        {
            if (_ExplosioPrefab != null)
            {
                GameObject exp = Instantiate(_ExplosioPrefab);
                exp.transform.position = transform.position;
            }

            int punts = 200;
            GameObject textPunts = GameObject.Find("TextPunts");
            if (textPunts != null)
                textPunts.GetComponent<TextPuntsJugador>().setPuntsJugador(punts);

            Destroy(gameObject);
        }
    }
}
