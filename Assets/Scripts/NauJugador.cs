using UnityEngine;
using UnityEngine.SceneManagement;

public class NauJugador : MonoBehaviour
{
    private float _vel = 8f;

    public GameObject _ExplosioPrefab;
    public GameManager _gameManager;

    void Start() { }

    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        Vector3 direccio = new Vector3(inputX, inputY, 0f).normalized;
        MoureNau(direccio);
    }

    void MoureNau(Vector3 direccio)
    {
        Vector3 pos = transform.position;
        pos += direccio * _vel * Time.deltaTime;

        float dist = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 minP = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist));
        Vector3 maxP = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, dist));

        pos.x = Mathf.Clamp(pos.x, minP.x + 0.6f, maxP.x - 0.6f);
        pos.y = Mathf.Clamp(pos.y, minP.y + 0.8f, maxP.y - 0.8f);
        pos.z = 0f;

        transform.position = pos;
    }

    private void OnTriggerEnter(Collider objecteTocat)
    {
        if (objecteTocat.CompareTag("Enemic") || objecteTocat.CompareTag("ProjectilEnemic"))
        {
            if (_ExplosioPrefab != null)
            {
                GameObject exp = Instantiate(_ExplosioPrefab);
                exp.transform.position = transform.position;
            }

            ValorsGlobals.videsActuals--;

            GameObject textVides = GameObject.Find("TextVides");
            if (textVides != null)
                textVides.GetComponent<TextVidesJugador>().ActualitzarText();

            if (ValorsGlobals.videsActuals <= 0)
            {
                SceneManager.LoadScene("EscenaResultats");
            }
            else
            {
                float dist = Mathf.Abs(Camera.main.transform.position.z);
                Vector3 minP = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist));
                transform.position = new Vector3(0f, minP.y + 1.5f, 0f);
            }
        }
    }
}
