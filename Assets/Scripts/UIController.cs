using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController init;

    [Header("Анимации")]
    /// <summary>
    /// Компонент анимции
    /// </summary>
    public Animation UIAnimation;
    public AnimationClip startGame;
    public AnimationClip endGame;
    /// <summary>
    /// Анимцая настроек
    /// </summary>
    public AnimationClip animSettings;
    public AnimationClip animBackSettings;
    /// <summary>
    /// Анимация инструкции
    /// </summary>
    public AnimationClip animHelp;
    public AnimationClip animBackHelp;
    /// <summary>
    /// Анимации магазина
    /// </summary>
    public AnimationClip animShop;
    public AnimationClip animShopBack;

    [Header("Переменные")]
    public UIState uiState = UIState.Menu;

    [Header("Таймер")]
    public Text tTimer;
    public Image iTimer;
    public float MaxTimer = 60;
    public float Timer;
    private float OldTimer;

    [Header("Текстовые элементы")]
    public Text tMoney;
    public Text tScore;
    public Text tName;

    [Header("Компоненты для звука")]
    public Image SoundImage;
    public Sprite SoundOn;
    public Sprite SoundOff;

    [Header("Компоненты для музыки")]
    public Image  MusicImage;
    public Sprite MusicOn;
    public Sprite MusicOff;

    [Header("Кнопки для покупки скинов")]
    public List<Image> SkinImages;
    public List<Image> BthSkinImages;
    public Sprite IcOk;

    /// <summary>
    /// Состояния игры
    /// </summary>
    public enum UIState {
        Menu,
        StartGame,
        Settings,
        Help,
        Shop
    }
    public UIController() {
        init = this;
    }
    public void Start()
    {
        tScore.text = "Лучший: " + PlayerPrefs.GetInt("Score");
        tMoney.text = PlayerPrefs.GetInt("Money").ToString();
        Timer = MaxTimer;
        OldTimer = MaxTimer;
        SoundImage.sprite = SoundController.init.onSound ? SoundOn : SoundOff;
        MusicImage.sprite = SoundController.init.onMusic ? MusicOn : MusicOff;
    }
    void Update()
    {
        if (GameController.init.Game && Timer > 0) {
            Timer -= Time.deltaTime;
            tTimer.text = ((int)Timer).ToString();
            iTimer.fillAmount = Timer / MaxTimer;

            if (OldTimer != (int)Timer) {
                if (Level.init.CheckEndLevel())
                    GameController.init.CreateLevel();
                SoundController.init.PlaySound("timer");

                OldTimer = (int)Timer;
            }
        }
    }

    /// <summary>
    /// Старт игры
    /// </summary>
    public void OnStartGame() {
        if (uiState == UIState.Menu) {
            UIAnimation.Play(startGame.name);
            uiState = UIState.StartGame;
            GameController.init.CreateLevel();

            new Thread(() => { Thread.Sleep(100); GameController.init.Game = true; }).Start();
        }
    }
    public void AddTimer(int Second) {
        Timer += Second;
        if (Timer > MaxTimer)
            Timer = MaxTimer;
    }

    /// <summary>
    /// Открытие настроек
    /// </summary>
    public void OnSettings() {
        if (uiState != UIState.Settings)
        {
            UIAnimation.Play(animSettings.name);
            uiState = UIState.Settings;
        }
        else if (uiState == UIState.Settings) {
            UIAnimation.Play(animBackSettings.name);
            uiState = UIState.Menu;
        }
    }
    /// <summary>
    /// Открытие инструкции
    /// </summary>
    public void OnHelp() {
        if (uiState != UIState.Help)
        {
            UIAnimation.Play(animHelp.name);
            uiState = UIState.Help;
        } else if (uiState == UIState.Help) {
            UIAnimation.Play(animBackHelp.name);
            uiState = UIState.Settings;
        }
    }

    public void OnSound() =>
        SoundImage.sprite = SoundController.init.OnSound() ? SoundOn : SoundOff;

    public void OnMusic() =>
       MusicImage.sprite = SoundController.init.OnMusic() ? MusicOn : MusicOff;

    public void OnShop() {
        if (uiState != UIState.Shop)
        {
            UIAnimation.Play(animShop.name);
            uiState = UIState.Shop;
        }
        else {
            UIAnimation.Play(animShopBack.name);
            uiState = UIState.Menu;
        }
    }

    public void EndGame()
    {
        int MaxScore = PlayerPrefs.GetInt("Score");
        if(MaxScore < GameController.init.GetILevel())
            PlayerPrefs.SetInt("Score", GameController.init.GetILevel());

        tScore.text = "Очки: " + GameController.init.GetILevel();
        tMoney.text = PlayerPrefs.GetInt("Money").ToString();
        tName.text = "Конец!";
        UIAnimation.Play(endGame.name);

        Invoke("EndGameTimer", 5);
    }

    /// <summary>
    /// Метод покупки скина
    /// </summary>
    /// <param name="IdSkin">Код скина</param>
    public void OnBuy(int IdSkin) {
        tMoney.text = PlayerPrefs.GetInt("Money").ToString();
        SkinImages[IdSkin].sprite = IcOk;
    }
    /// <summary>
    /// Метод который графически активирует скин блока
    /// </summary>
    /// <param name="Id">ИД блока</param>
    public void OnSkinActive(int Id) {
        foreach (Image BthImage in BthSkinImages)
            BthImage.color = new Color(0.09f, 0.97f, 0);

        BthSkinImages[Id].color = new Color(0.97f, 0.75f, 0);
    }

    public void EndGameTimer() =>
        SceneManager.LoadScene(0);
}
