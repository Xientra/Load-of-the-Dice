using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeUI : MonoBehaviour
{
    private PlayerLife player;
    public Image[] lifeImages;

    private void Start()
    {
        player = FindObjectOfType<PlayerLife>();
    }

    private void Update()
    {
        for (int i = 0; i < lifeImages.Length; i++)
            lifeImages[i].gameObject.SetActive(i <= player.life);
    }
}
