using Godot;
using System;

public partial class ProgressBarsUI : Control
{
	public TextureProgressBar healthBar;
	public TextureProgressBar staminaBar;
	private double _maxHealth = 100f;
	private double _currentHealth = 100f;
	private double _maxStamina = 100f;
	private double _currentStamina = 100f;
	private double _healthRegenRate = 1f; // Health regenerated per second
	private double _staminaRegenRate = 5f; // Stamina regenerated per second

	public override void _Ready()
	{
		healthBar = GetNode<TextureProgressBar>("MarginContainer/VBoxContainer/HealthBar");
		staminaBar = GetNode<TextureProgressBar>("MarginContainer/VBoxContainer/StaminaBar");
		healthBar.MaxValue = _maxHealth;
		healthBar.Value = _currentHealth;
		staminaBar.MaxValue = _maxStamina;
		staminaBar.Value = _currentStamina;
		SetProcess(true);
	}

	public override void _Process(double delta)
	{
		if (_currentHealth < _maxHealth)
		{
			_currentHealth += _healthRegenRate * delta;
			if (_currentHealth > _maxHealth)
			{
				_currentHealth = _maxHealth;
			}
			healthBar.Value = _currentHealth;
		}

		if (_currentStamina < _maxStamina)
		{
			_currentStamina += _staminaRegenRate * delta;
			if (_currentStamina > _maxStamina)
			{
				_currentStamina = _maxStamina;
			}
			staminaBar.Value = _currentStamina;
		}
	}
	public void ModifyStamina(double amount)
	{
		_currentStamina += amount;
		if (_currentStamina < 0)
		{
			_currentStamina = 0;
		}

		if (_currentStamina > _maxStamina)
		{
			_currentStamina = _maxStamina;
		}

		staminaBar.Value = _currentStamina;
	}
}
