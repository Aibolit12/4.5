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
    /// �������� ���� ���������
    /// </summary>
    public int ActiveSkin = 0;

    public ShopController() =>
        init = this;
        
    private void Awake()
    {
        // ��������� ���������� � ��������� ������
        // ���������� �����, � ��������� � ��� ���������
        for(int iSkin = 1; iSkin < Skins.Count; iSkin++) {
            // ���������� ���������� � ��� ��� ���� ������
            Skins[iSkin].Buy = PlayerPrefs.GetInt($"Skin_{Skins[iSkin].Name}", 0) == 1;
            if(Skins[iSkin].Buy)
                UIController.init.OnBuy(iSkin);
        }
        // ��������� �������� ����
        ActiveSkin = PlayerPrefs.GetInt("Skin_Active", 0);
        UIController.init.OnSkinActive(ActiveSkin);
    }
    /// <summary>
    /// ����� �������
    /// </summary>
    /// <param name="IdSkin">��� �����</param>
    /// <returns></returns>
    public void OnBuy(int IdSkin) {
        // �������� ���-�� �����
        int Money = PlayerPrefs.GetInt("Money");
        if (Skins[IdSkin].Buy)
        {
            ActiveSkin = IdSkin;
            UIController.init.OnSkinActive(ActiveSkin);
            // ��������� �������� ����
            PlayerPrefs.SetInt($"Skin_Active", ActiveSkin);
        }
        else {
            // ���� ����� ������� �� �������
            if (Money >= Skins[IdSkin].Price)
            {
                // ��������� ���-�� �����
                Money -= Skins[IdSkin].Price;
                // ������� ��� ���� ������
                Skins[IdSkin].Buy = true;
                // ���������� ��� ���� ������
                PlayerPrefs.SetInt($"Skin_{Skins[IdSkin].Name}", 1);
                // ��������� ���-�� �����
                PlayerPrefs.SetInt("Money", Money);
                // ��������� UI ���������
                UIController.init.OnBuy(IdSkin);
            }
        }
    }
    [ContextMenu("�������� ���������� ������")]
    public void DeleteSave() {
        for (int iSkin = 1; iSkin < Skins.Count; iSkin++)
        {
            PlayerPrefs.DeleteKey($"Skin_{Skins[iSkin].Name}");
        }
    } 
}
