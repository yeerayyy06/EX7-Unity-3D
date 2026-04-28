using UnityEngine;
using UnityEngine.SceneManagement;

public class PantallaResultats : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI puntsAconseguits;
    [SerializeField] private TMPro.TextMeshProUGUI videsAgafades;

    void Start()
    {
        puntsAconseguits.text = ValorsGlobals.puntsAconseguits;
        videsAgafades.text = "Vides agafades: " + ValorsGlobals.videsAgafades;
    }

    public void TornarAInici()
    {
        ValorsGlobals.Reiniciar();
        SceneManager.LoadScene("EscenaInici");
    }
}
