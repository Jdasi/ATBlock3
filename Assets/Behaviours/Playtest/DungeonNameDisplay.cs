using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonNameDisplay : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float fade_in_time = 1;
    [SerializeField] float fade_out_time = 1;
    [SerializeField] float hang_time = 2;

    [Header("References")]
    [SerializeField] Text lbl_name;
    [SerializeField] FadableGraphic name_fade;
    [SerializeField] FadableGraphic underline_fade;


    public void ShowName()
    {
        StartCoroutine(DisplayRoutine());
    }


    IEnumerator DisplayRoutine()
    {
        string dungeon_name = GameManager.scene.playtest_dungeon.map_name;
        lbl_name.text = dungeon_name;

        name_fade.FadeIn(fade_in_time);
        underline_fade.FadeIn(fade_in_time);

        yield return new WaitForSeconds(fade_in_time + hang_time);

        name_fade.FadeOut(fade_out_time);
        underline_fade.FadeOut(fade_out_time);
    }

}
