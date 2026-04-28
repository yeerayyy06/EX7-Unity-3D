using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject _botoJugar;
    public GameObject _nauJugador;
    public GameObject _generadorEnemics;
    public GameObject _textPuntsJugador;
    public GameObject _textVidesJugador;
    public GameObject _generadorObjectesVida;

    [Header("Music")]
    public AudioClip _musicaFons;
    private AudioSource _audioSource;

    public enum EstatsGameManager { Inici, Jugant, GameOver }

    private EstatsGameManager _estatGameManager;

    void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.loop = true;
        _audioSource.volume = 0.5f;

        _estatGameManager = EstatsGameManager.Inici;
        ActualitzarEstatGameManager();
    }

    void Update() { }

    private void ActualitzarEstatGameManager()
    {
        switch (_estatGameManager)
        {
            case EstatsGameManager.Inici:
                _botoJugar.SetActive(true);
                _nauJugador.SetActive(false);
                _textPuntsJugador.GetComponent<TextPuntsJugador>().InicialitzarPunts();
                _textPuntsJugador.SetActive(false);

                if (_textVidesJugador != null)
                {
                    _textVidesJugador.GetComponent<TextVidesJugador>().InicialitzarVides();
                    _textVidesJugador.SetActive(false);
                }
                break;

            case EstatsGameManager.Jugant:
                _botoJugar.SetActive(false);
                _nauJugador.SetActive(true);
                _textPuntsJugador.SetActive(true);
                _generadorEnemics.GetComponent<GeneradorEnemics>().IniciGeneraEnemics();

                if (_textVidesJugador != null)
                {
                    _textVidesJugador.SetActive(true);
                    _textVidesJugador.GetComponent<TextVidesJugador>().ActualitzarText();
                }

                if (_generadorObjectesVida != null)
                    _generadorObjectesVida.GetComponent<GeneradorObjectesVida>().IniciGeneraObjectesVida();

                if (_musicaFons != null)
                {
                    _audioSource.clip = _musicaFons;
                    _audioSource.Play();
                }
                break;

            case EstatsGameManager.GameOver:
                _botoJugar.SetActive(false);
                _nauJugador.SetActive(false);
                _textPuntsJugador.SetActive(true);
                _generadorEnemics.GetComponent<GeneradorEnemics>().AturaGenerarEnemics();

                if (_textVidesJugador != null)
                    _textVidesJugador.SetActive(true);

                if (_generadorObjectesVida != null)
                    _generadorObjectesVida.GetComponent<GeneradorObjectesVida>().AturaGenerarObjectesVida();

                _audioSource.Stop();

                Invoke("PassarAEstatInici", 3f);
                break;
        }
    }

    public void setEstatGameManager(EstatsGameManager estatGameManager)
    {
        _estatGameManager = estatGameManager;
        ActualitzarEstatGameManager();
    }

    public void PassarAEstatJugant()
    {
        _estatGameManager = EstatsGameManager.Jugant;
        ActualitzarEstatGameManager();
    }

    public void PassarAGameOver()
    {
        _estatGameManager = EstatsGameManager.GameOver;
        ActualitzarEstatGameManager();
    }

    public void PassarAEstatInici()
    {
        _estatGameManager = EstatsGameManager.Inici;
        ActualitzarEstatGameManager();
    }
}
