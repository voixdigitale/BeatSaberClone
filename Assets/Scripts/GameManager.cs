using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public float health = 1.0f;
    public int hitBlockScore = 10;
    public float missBlockDamage = .1f;
    public float wrongBlockDamage = .08f;
    public float lifeRegenRate = .1f;
    public float songStartTime = 3.0f;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void HitBlock()
    {
        score += hitBlockScore;
        GameUI.instance.UpdateScoreText();
    }

    public void MissBlock()
    {
        health -= missBlockDamage;
    }

    public void WrongBlock()
    {
        health -= wrongBlockDamage;
    }

    void Update()
    {
        health = Mathf.MoveTowards(health, 1.0f, lifeRegenRate * Time.deltaTime);
        GameUI.instance.UpdateHealthBar();
    }
}
