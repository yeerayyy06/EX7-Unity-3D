using UnityEngine;

public class ObjecteVida : MonoBehaviour
{
    float _vel = 2f;

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
        if (objecteTocat.CompareTag("NauJugador"))
        {
            GameObject textVides = GameObject.Find("TextVides");
            if (textVides != null)
                textVides.GetComponent<TextVidesJugador>().AfegirVida();

            Destroy(gameObject);
        }
    }
}
