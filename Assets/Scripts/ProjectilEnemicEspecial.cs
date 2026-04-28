using UnityEngine;

public class ProjectilEnemicEspecial : MonoBehaviour
{
    private float _vel = 6f;
    private Vector3 _direccio = Vector3.down;

    void Update()
    {
        transform.position += _direccio * _vel * Time.deltaTime;
        ComprovarDinsPantalla();
    }

    public void SetDireccio(Vector2 direccio)
    {
        _direccio = new Vector3(direccio.x, direccio.y, 0f).normalized;
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

    private void OnTriggerEnter(Collider objecteTocat)
    {
        if (objecteTocat.CompareTag("NauJugador"))
            Destroy(gameObject);
    }
}
