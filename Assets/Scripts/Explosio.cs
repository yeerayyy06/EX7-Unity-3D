using UnityEngine;

public class Explosio : MonoBehaviour
{
    void Start()
    {
        Invoke("DestrueixExplosio", 1f);
    }

    private void DestrueixExplosio()
    {
        Destroy(gameObject);
    }
}
