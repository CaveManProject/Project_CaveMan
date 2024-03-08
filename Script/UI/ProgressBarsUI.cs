using Godot;
using System;

public partial class ProgressBarsUI : Control
{
    public TextureProgressBar HealthBar;
    public TextureProgressBar StaminaBar;
    private float maxHealth = 100f;
    private float currentHealth = 100f;
    private float maxStamina = 100f;
    private float currentStamina = 100f;
    private float healthRegenRate = 1f; // Health regenerated per second
    private float staminaRegenRate = 5f; // Stamina regenerated per second

    public override void _Ready()
    {
        HealthBar = GetNode<TextureProgressBar>("MarginContainer/VBoxContainer/HealthBar");
        StaminaBar = GetNode<TextureProgressBar>("MarginContainer/VBoxContainer/StaminaBar");
        HealthBar.MaxValue = maxHealth;
        HealthBar.Value = currentHealth;
        StaminaBar.MaxValue = maxStamina;
        StaminaBar.Value = currentStamina;
        SetProcess(true);
    }

    public void _Process(float delta)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += healthRegenRate * delta;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            HealthBar.Value = currentHealth;
        }

        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * delta;
            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
            StaminaBar.Value = currentStamina;
        }
    }
    public void ModifyStamina(float amount)
    {
        currentStamina += amount;
        if (currentStamina < 0)
        {
            currentStamina = 0;
        }

        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }

        StaminaBar.Value = currentStamina;
    }
}