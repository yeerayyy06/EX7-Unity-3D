using UnityEngine;

public class GeneradorProjectils : MonoBehaviour
{
    public GameObject _ProjectilPrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            GeneraProjectil();
    }

    private void GeneraProjectil()
    {
        GameObject projectil = Instantiate(_ProjectilPrefab);
        projectil.transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }
}
