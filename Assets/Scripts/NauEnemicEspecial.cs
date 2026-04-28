using UnityEngine;

// Enemic especial 3D: orbita al voltant del seu punt d'aparició i dispara en espiral cap al jugador.
// A diferència de l'enemic especial 2D (que es mou horitzontalment i cau), aquest:
// 1. Orbita al voltant de la seva posició inicial durant uns segons.
// 2. Dispara projectils cap al jugador des de tots angles de l'òrbita.
// 3. Finalment s'acosta directament cap al jugador.
public class NauEnemicEspecial : MonoBehaviour
{
    float _velOrbita = 120f;
    float _radioOrbita = 1.5f;
    float _velApropament = 4f;
    float _tempsOrbita = 4f;

    public GameObject _ExplosioPrefab;
    public GameObject _ProjectilEnemicPrefab;

    private Vector3 _centreOrbita;
    private float _anglActual = 0f;
    private bool _modeApropament = false;
    private float _tempsObitaActual = 0f;

    void Start()
    {
        _centreOrbita = transform.position;
        InvokeRepeating("DisparaProjectil", 0.5f, 0.6f);
    }

    void Update()
    {
        if (!_modeApropament)
        {
            _tempsObitaActual += Time.deltaTime;
            _anglActual += _velOrbita * Time.deltaTime;

            float rad = _anglActual * Mathf.Deg2Rad;
            Vector3 novaPos = _centreOrbita + new Vector3(
                Mathf.Cos(rad) * _radioOrbita,
                Mathf.Sin(rad) * _radioOrbita,
                0f
            );
            transform.position = novaPos;

            if (_tempsObitaActual >= _tempsOrbita)
                _modeApropament = true;
        }
        else
        {
            GameObject jugador = GameObject.FindWithTag("NauJugador");
            if (jugador != null)
            {
                Vector3 direccio = (jugador.transform.position - transform.position).normalized;
                transform.position += direccio * _velApropament * Time.deltaTime;
            }
            else
            {
                transform.position += Vector3.down * _velApropament * Time.deltaTime;
            }

            float dist = Mathf.Abs(Camera.main.transform.position.z);
            Vector3 minP = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist));
            if (transform.position.y < minP.y - 1f)
                Destroy(gameObject);
        }
    }

    private void DisparaProjectil()
    {
        if (_ProjectilEnemicPrefab == null) return;
        GameObject jugador = GameObject.FindWithTag("NauJugador");
        if (jugador == null) return;

        GameObject projectil = Instantiate(_ProjectilEnemicPrefab, transform.position, Quaternion.identity);
        Vector3 direccio = (jugador.transform.position - transform.position).normalized;

        ProjectilEnemicEspecial proj = projectil.GetComponent<ProjectilEnemicEspecial>();
        if (proj != null)
            proj.SetDireccio(new Vector2(direccio.x, direccio.y));
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

            GameObject textPunts = GameObject.Find("TextPunts");
            if (textPunts != null)
                textPunts.GetComponent<TextPuntsJugador>().setPuntsJugador(500);

            CancelInvoke("DisparaProjectil");
            Destroy(gameObject);
        }
    }
}
