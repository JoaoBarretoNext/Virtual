using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class InterfaceManager : MonoBehaviour
{
   
    [SerializeField]
    TextMeshProUGUI message_TEXT;
    [SerializeField]
    TextMeshProUGUI quit_TEXT;
    [SerializeField]
    Image background;
    bool gamePaused;
    // Start is called before the first frame update



    private static InterfaceManager _instance;

    public static InterfaceManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        gamePaused = true;    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PauseGameEscape(string message)
    {
        gamePaused = !gamePaused;
        if (!gamePaused)
        {
            message_TEXT.gameObject.SetActive(true);
            quit_TEXT.gameObject.SetActive(true);
            message_TEXT.text = message;
            //message. (1, 1
            background.DOFade(0.16f, 1).SetUpdate(true);
            Time.timeScale = 0;

        }
        else
        {
            Time.timeScale = 1;
            message_TEXT.gameObject.SetActive(false);
            quit_TEXT.gameObject.SetActive(false);
            //pauseButton.image.DOFade(0, 1);
            background.DOFade(0, 1);
        }
        
        
    }

    public void WinGame(string message)
    {
        gamePaused = !gamePaused;
        message_TEXT.gameObject.SetActive(true);
        quit_TEXT.gameObject.SetActive(true);
        message_TEXT.text = message;
        //message. (1, 1
        background.DOFade(0.16f, 1).SetUpdate(true);
        Time.timeScale = 0;
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
