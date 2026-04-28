using UnityEngine;

public class GeneradorProjectilEnemic : MonoBehaviour
{
    public GameObject _ProjectilEnemicPrefab;

    void Start()
    {
        Invoke("CreaProjectil", 0.75f);
    }

    private void CreaProjectil()
    {
        if (GameObject.FindWithTag("NauJugador") != null)
        {
            GameObject projectil = Instantiate(_ProjectilEnemicPrefab);
            projectil.transform.position = transform.position;
        }
    }
}
