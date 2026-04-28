using UnityEngine;

public class ProjectilEnemic : MonoBehaviour
{
    private float _vel = 5f;
    private bool _continuaUltimaDireccio = false;
    private Vector3 _direccioJugador = Vector3.down;

    void Start()
    {
        Invoke("ContinuaUltimaDireccio", 1.5f);
    }

    void Update()
    {
        GameObject jugador = GameObject.FindWithTag("NauJugador");

        if (jugador != null)
        {
            if (!_continuaUltimaDireccio)
                _direccioJugador = (jugador.transform.position - transform.position).normalized;

            transform.position += _direccioJugador * _vel * Time.deltaTime;
            ComprovarDinsPantalla();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void ComprovarDinsPantalla()
    {
        float dist = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 minP = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist));
        Vector3 maxP = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, dist));

        Vector3 pos = transform.position;
        if (pos.y < minP.y || pos.y > maxP.y || pos.x < minP.x || pos.x > maxP.x)
            Destroy(gameObject);
    }

    private void ContinuaUltimaDireccio()
    {
        _continuaUltimaDireccio = true;
    }

    private void OnTriggerEnter(Collider objecteTocat)
    {
        if (objecteTocat.CompareTag("NauJugador"))
            Destroy(gameObject);
    }
}
