using UnityEngine;

public class ProjectilJugador : MonoBehaviour
{
    float _vel = 10f;

    void Update()
    {
        transform.position += Vector3.up * _vel * Time.deltaTime;

        float dist = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 maxP = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, dist));
        if (transform.position.y > maxP.y)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider objecteTocat)
    {
        if (objecteTocat.CompareTag("Enemic"))
            Destroy(gameObject);
    }
}
