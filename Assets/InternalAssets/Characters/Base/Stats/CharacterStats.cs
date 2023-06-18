﻿using System;
using SouthBasement.Weapons;

namespace SouthBasement
{
    [Serializable]
    public sealed class CharacterStats
    {
        public WeaponStats WeaponStats { get; set; } = new();
        public ObservableVariable<float> MoveSpeed { get; private set; } = new(5f);
        public int DashStaminaRequire { get; set; } = 10;

        public int MaximumStamina { get; set; } = 100;
        public ObservableVariable<int> Stamina { get; set; } = new(100);
        public float StaminaIncreaseRate { get; set; } = 0.1f;
        public int MaximumHealth { get; private set; } = 60;
        public int CurrentHealth { get; private set; } = 60;

        public Action<int> OnHealthChanged;

        public void SetHealth(int currentHealth, int maximumHealth = -10)
        {
            if(maximumHealth > 0)
                MaximumHealth = maximumHealth;

            CurrentHealth = currentHealth;

            if (CurrentHealth >= MaximumHealth)
                CurrentHealth = MaximumHealth;
            if(CurrentHealth <= 0)
                return;
                //делаем чтото
                
            OnHealthChanged?.Invoke(CurrentHealth);
        }
    }
}