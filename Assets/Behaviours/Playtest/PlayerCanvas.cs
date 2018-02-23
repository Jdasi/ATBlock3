using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour
{
    [SerializeField] Image health_bar;
    [SerializeField] FadableGraphic health_bar_fade;
    [SerializeField] Color health_panel_color;


    void Start()
    {
        var life = GameManager.scene.player.GetComponent<LifeForce>();
        life.on_health_percentage_changed_event.AddListener(OnHealthPercentageChanged);
    }


    void OnHealthPercentageChanged(float _percentage)
    {
        health_bar_fade.FadeColor(Color.white, health_panel_color, 0.25f);
        health_bar.fillAmount = _percentage;
    }

}
