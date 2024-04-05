using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public static ShopController init;

    [System.Serializable]
    public class Skin {
        public string Name;
        public GameObject Prefab;
        public int Price;
        public bool Buy;
    }

    public List<Skin> Skins;
    /// <summary>
    /// Активный скин персонажа
    /// </summary>
    public int ActiveSkin = 0;

    public ShopController() =>
        init = this;
        
    private void Awake()
    {
        // Загружать информацию о купленных блоках
        // Перебираем скины, и загружаем о них информцию
        for(int iSkin = 1; iSkin < Skins.Count; iSkin++) {
            // Записываем информацию о том что скин куплен
            Skins[iSkin].Buy = PlayerPrefs.GetInt($"Skin_{Skins[iSkin].Name}", 0) == 1;
            if(Skins[iSkin].Buy)
                UIController.init.OnBuy(iSkin);
        }
        // Загружаем активный скин
        ActiveSkin = PlayerPrefs.GetInt("Skin_Active", 0);
        UIController.init.OnSkinActive(ActiveSkin);
    }
    /// <summary>
    /// Метод покупки
    /// </summary>
    /// <param name="IdSkin">Код скина</param>
    /// <returns></returns>
    public void OnBuy(int IdSkin) {
        // Получаем кол-во денег
        int Money = PlayerPrefs.GetInt("Money");
        if (Skins[IdSkin].Buy)
        {
            ActiveSkin = IdSkin;
            UIController.init.OnSkinActive(ActiveSkin);
            // Сохраняем активный скин
            PlayerPrefs.SetInt($"Skin_Active", ActiveSkin);
        }
        else {
            // Если денег хватает на покупку
            if (Money >= Skins[IdSkin].Price)
            {
                // Уменьшаем кол-во деняк
                Money -= Skins[IdSkin].Price;
                // Говорим что скин куплен
                Skins[IdSkin].Buy = true;
                // Запоминаем что скин куплен
                PlayerPrefs.SetInt($"Skin_{Skins[IdSkin].Name}", 1);
                // Сохраняем кол-во деняк
                PlayerPrefs.SetInt("Money", Money);
                // Обновляем UI интерфейс
                UIController.init.OnBuy(IdSkin);
            }
        }
    }
    [ContextMenu("Очистить сохраннёные данные")]
    public void DeleteSave() {
        for (int iSkin = 1; iSkin < Skins.Count; iSkin++)
        {
            PlayerPrefs.DeleteKey($"Skin_{Skins[iSkin].Name}");
        }
    } 
}
