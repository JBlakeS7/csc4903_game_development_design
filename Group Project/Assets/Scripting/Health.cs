using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float _health = 30;
    public bool timer = true;
    public float health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
        }
    }
    float maxHealth = 60;
    public UnityEngine.UI.Text healthText;
    public UnityEngine.UI.Slider healthBar;
    public void TakeDamage(float damage)
    {
        _health -= damage;
    }
    public void AddHealth(float delta)
    {
        _health += delta;
    }

    private bool canSend = true;
    // Update is called once per frame
    void Update()
    {
        if (health > 0) canSend = true;
        if (timer && health > 0f)
            _health -= Time.fixedDeltaTime;
        if (_health > maxHealth) _health = maxHealth;
        {
            healthText.text = (Mathf.Ceil(healthBar.value = Mathf.RoundToInt(_health * 10) / 10f)).ToString();
        }
        if (_health <= 0 && canSend)
        {
            canSend = false;
            HordeMode.main.PlayerDied();
        }
    }
}
