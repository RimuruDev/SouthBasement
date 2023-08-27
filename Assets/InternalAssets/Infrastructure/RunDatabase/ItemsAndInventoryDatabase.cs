﻿using SouthBasement.Characters.Stats;
using SouthBasement.Infrastucture;
using SouthBasement.InputServices;
using SouthBasement.InventorySystem;
using Zenject;

namespace SouthBasement
{
    public sealed class ItemsAndInventoryDatabase : IRunDatabase
    {
        private readonly DiContainer _container;
        
        private readonly CharacterCombatStats _combatStats;
        private readonly IInputService _inputService;
        private Inventory _inventory;
        private ICoroutineRunner _coroutineRunner;
        private InventoryDatabase _inventoryDatabase;
        private WeaponsUsage _weaponsUsage;

        public bool Created { get; private set; }

        public ItemsAndInventoryDatabase(DiContainer container, CharacterCombatStats combatStats, IInputService inputService)
        {
            _container = container;
            
            _combatStats = combatStats;
            _inputService = inputService;
        }

        public void Create()
        {
            if (Created) return;

            BindInventory();
            BindActiveItemUsager();
            BindPassiveItemUsager();
            BindWeaponsItemUsager();
            
            Created = true;
        }

        public void Remove()
        {
            Created = false;
            
            _container.Unbind<Inventory>();
            _container.Unbind<ActiveItemUsage>();
            _container.Unbind<WeaponsUsage>();
            _container.Unbind<PassiveItemsUsage>();
        }

        public void Reset()
        {
            if (!Created) Create();

            _coroutineRunner = _container.Resolve<ICoroutineRunner>();
            
            _inventoryDatabase.Reset();

            _container
                .Rebind<Inventory>()
                .FromInstance(_inventory)
                .AsCached();

            _container
                .Rebind<ActiveItemUsage>()
                .FromInstance(new ActiveItemUsage(_inputService, _inventory))
                .AsCached();

            _weaponsUsage = new WeaponsUsage(_inventory, _combatStats);
            
            _container
                .Rebind<WeaponsUsage>()
                .FromInstance(_weaponsUsage)
                .AsCached();

            _container
                .Rebind<PassiveItemsUsage>()
                .FromInstance(new PassiveItemsUsage(_inventory))
                .AsCached();
        }
        
        private void BindInventory()
        {
            _inventoryDatabase = new InventoryDatabase();
            
            _inventoryDatabase.Create();
            _inventory = _inventoryDatabase.Inventory;

            _container
                .BindInterfacesAndSelfTo<Inventory>()
                .FromInstance(_inventory)
                .AsCached();

        }
        private void BindActiveItemUsager()
        {
            _container
                .Bind<ActiveItemUsage>()
                .FromInstance(new ActiveItemUsage(_inputService, _inventory))
                .AsCached();
        }
        private void BindWeaponsItemUsager()
        {
            _weaponsUsage = new WeaponsUsage(_inventory, _combatStats);

            _container
                .Bind<WeaponsUsage>()
                .FromInstance(_weaponsUsage)
                .AsCached();
        }
        private void BindPassiveItemUsager()
        {
            _container
                .BindInterfacesTo<PassiveItemsUsage>()
                .FromInstance(new PassiveItemsUsage(_inventory))
                .AsCached();
        }

        public void OnCharacterSpawned()
        {
            if(_weaponsUsage.CurrentWeapon != null)
                _weaponsUsage.CurrentWeapon.OnEquip();
        }
    }
}