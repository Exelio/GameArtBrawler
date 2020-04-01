using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Range(1, 2)] [SerializeField] private int _playerAmount = 2;
    public int PlayerAmount { get => _playerAmount; set { _playerAmount = value; } }

    private int _characterAmount;
    public int CharacterAmount { get => _characterAmount; set { _characterAmount = value; } }

    private static Dictionary<int, GameObject> _playerCharacter;
    public static Dictionary<int, GameObject> PlayerCharacter { get => _playerCharacter; set { _playerCharacter = value; } }

    private static bool _isGamePlaying = true;
    public static bool IsGamePlaying { get => _isGamePlaying; set { _isGamePlaying = value; } }

    private static AudioSource _audioSource;
    [SerializeField] private AudioClip _startScreenSoundTrack;
    private static AudioClip CHARSELECTSCREENSOUNDTRACK;
    [SerializeField] private AudioClip _charSelectScreenSoundtrack;


    private static AudioClip[] ARENASOUNDTRACKS;
    [SerializeField] private AudioClip[] _arenaSoundTracks;

    private void Awake()
    {
        CheckDontDestroyOnLoad();

        Setup();

        _audioSource = GetComponent<AudioSource>();

        ARENASOUNDTRACKS = _arenaSoundTracks;
        CHARSELECTSCREENSOUNDTRACK = _charSelectScreenSoundtrack;

        _audioSource.Stop();

        _audioSource.clip = CHARSELECTSCREENSOUNDTRACK;

        _audioSource.Play();
        _audioSource.loop = true;
    }

    private void CheckDontDestroyOnLoad()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("GameController");
        if (gos.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Setup()
    {
        _playerCharacter = new Dictionary<int, GameObject>();

        for (int i = 1; i <= _playerAmount; i++)
        {
            _playerCharacter.Add(i, null);
        }
    }     
    
    public static void ChangeGameState(bool gamePlaying)
    {
        if (gamePlaying) SceneManager.LoadScene("Arena_Scene");
        _isGamePlaying = gamePlaying;
        PlayRandomSoundtrack();
    }

    public static IEnumerator GoToCharacterSelectScreen()
    {
        yield return new WaitForSeconds(3f);
        _audioSource.clip = CHARSELECTSCREENSOUNDTRACK;

        _audioSource.Play();
        _audioSource.loop = true;
        SceneManager.LoadScene("CharacterSelection_Scene");
    }

    private static void PlayRandomSoundtrack()
    {
        int index = Random.Range(0, ARENASOUNDTRACKS.Length - 1);
        
        _audioSource.Stop();

        _audioSource.clip = ARENASOUNDTRACKS[index];

        _audioSource.Play();

        _audioSource.loop = true;
    }

    public static void ChangeLockedInCharacter(int playerNumber, GameObject character)
    {
        _playerCharacter[playerNumber] = character;
    }
}
