using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour
{
    [System.Serializable]
    public class BarGroup
    {
        public Image fill_image;
        public Color bg_color;
        public FadableGraphic fade;

        public float fill_amount { get { return fill_image.fillAmount; } set { fill_image.fillAmount = value; } }
    }

    [System.Serializable]
    public class TextGroup
    {
        public Text ui_text;
        public Color fade_from_color;
        public Color fade_to_color;
        public FadableGraphic fade;

        public string text { get { return ui_text.text; } set { ui_text.text = value; } }
    }

    [Header("Bars")]
    [SerializeField] BarGroup health_bar;
    [SerializeField] BarGroup mana_bar;

    [Header("Text")]
    [SerializeField] TextGroup health_potions_display;
    [SerializeField] TextGroup mana_potions_display;
    [SerializeField] TextGroup gold_display;

    [Header("Miscellaneous")]
    [SerializeField] Color menu_dead_color;
    [SerializeField] Color menu_paused_color;
    [SerializeField] GameObject playtest_menu_panel;


    public void BtnReturnToMenu()
    {
        GameManager.GoToMenu();
    }


    public void BtnReturnToEditor()
    {
        GameManager.GoToEditor();
    }


    public void BrightnessChanged(float _brightness)
    {
        GameManager.UpdateBrightness(_brightness);
    }


    public void ShowPlaytestMenu(bool _show)
    {
        playtest_menu_panel.GetComponent<Image>().color = GameManager.scene.player.life.IsAlive() ?
            menu_dead_color : menu_paused_color;

        playtest_menu_panel.SetActive(_show);
    }


    public void SetMenuText(string _str)
    {
        playtest_menu_panel.GetComponentInChildren<Text>().text = _str;
    }


    void Start()
    {
        var life = GameManager.scene.player.life;
        life.health_percent_changed_events.AddListener(HealthPercentChanged);
        GameManager.scene.player.mana_percent_changed_events.AddListener(ManaPercentChanged);
        GameManager.scene.player.mana_percent_changed_events_quiet.AddListener(ManaPercentChangedQuiet);

        var inventory = GameManager.scene.player.inventory;
        inventory.health_potions_changed_events.AddListener(HealthPotionsChanged);
        inventory.mana_potions_changed_events.AddListener(ManaPotionsChanged);
        inventory.gold_changed_events.AddListener(GoldChanged);
    }


    void HealthPercentChanged(float _percent)
    {
        health_bar.fade.FadeColor(Color.white, health_bar.bg_color, 0.25f);
        health_bar.fill_amount = _percent;
    }


    void ManaPercentChanged(float _percent)
    {
        mana_bar.fade.FadeColor(Color.white, mana_bar.bg_color, 0.25f);
        mana_bar.fill_amount = _percent;
    }


    void ManaPercentChangedQuiet(float _percent)
    {
        mana_bar.fill_amount = _percent;
    }


    void HealthPotionsChanged(int _potions)
    {
        health_potions_display.fade.FadeColor(health_potions_display.fade_from_color, health_potions_display.fade_to_color, 0.25f);
        health_potions_display.text = _potions.ToString();
    }


    void ManaPotionsChanged(int _potions)
    {
        mana_potions_display.fade.FadeColor(mana_potions_display.fade_from_color, mana_potions_display.fade_to_color, 0.25f);
        mana_potions_display.text = _potions.ToString();
    }


    void GoldChanged(int _gold)
    {
        gold_display.fade.FadeColor(gold_display.fade_from_color, gold_display.fade_to_color, 0.25f);
        gold_display.text = _gold.ToString();
    }

}
