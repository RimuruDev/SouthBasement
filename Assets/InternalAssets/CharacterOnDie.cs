﻿using NTC.GlobalStateMachine;
using SouthBasement.Characters;
using SouthBasement.Characters.Components;
using SouthBasement.Characters.Stats;
using SouthBasement.CameraHandl;
using Zenject;

namespace SouthBasement
{
    public sealed class CharacterOnDie : StateMachineUser
    {
        private Character _character;
        private CameraHandler _cameraHandler;

        [Inject]
        private void Construct(Character characterFactory, CameraHandler cameraHandler)
        {
            _character = characterFactory;
            _cameraHandler = cameraHandler;
        }
        
        protected override void OnDied()
        {
            _character.Components.Get<PlayerAnimator>().PlayDead();
            
            _character.Components
                .Remove<ICharacterAttacker>()
                .Remove<IDashable>()
                .Remove<IFlipper>()
                .Remove<ICharacterMovable>();
            
            //_character.Components.Get<ICharacterMovable>().CanMove = false;
        }
    }
}