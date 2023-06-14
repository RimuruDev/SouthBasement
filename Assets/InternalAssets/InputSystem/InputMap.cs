//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.1
//     from Assets/InternalAssets/InputSystem/InputMap.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace TheRat
{
    public partial class @InputMap: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @InputMap()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMap"",
    ""maps"": [
        {
            ""name"": ""CharacterContoller"",
            ""id"": ""923f7c07-c26c-4a7a-90fc-50960fcf9b85"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""2ad15012-3315-4272-be08-8222b30c024a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""1bb2dbc9-40f1-4828-aeed-f2e4887e804f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interaction"",
                    ""type"": ""Button"",
                    ""id"": ""6788272a-a30c-432d-968f-d3f917911ca7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""746c88a4-5153-4ed0-8efd-60c0f3d42730"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Movement"",
                    ""id"": ""45a276c7-67e2-476f-a44a-edec97e82b3d"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""1dd74dc0-552a-4dce-abbb-060b6559aa35"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""6b6bfecb-e3bb-4760-b8fc-4892fed72ed0"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""303a04c2-30de-471b-a90f-5d60998c7cd9"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""7baeb80f-d32e-44c2-baf5-f9fd50f3e9b7"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""f79aad85-9565-4907-917c-a6d71d9980ae"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""783e6890-0239-478c-8552-3484e2b64e17"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b141a4d3-3e46-4598-a18e-ea3f547a5b7d"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // CharacterContoller
            m_CharacterContoller = asset.FindActionMap("CharacterContoller", throwIfNotFound: true);
            m_CharacterContoller_Move = m_CharacterContoller.FindAction("Move", throwIfNotFound: true);
            m_CharacterContoller_Attack = m_CharacterContoller.FindAction("Attack", throwIfNotFound: true);
            m_CharacterContoller_Interaction = m_CharacterContoller.FindAction("Interaction", throwIfNotFound: true);
            m_CharacterContoller_Dash = m_CharacterContoller.FindAction("Dash", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }

        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // CharacterContoller
        private readonly InputActionMap m_CharacterContoller;
        private List<ICharacterContollerActions> m_CharacterContollerActionsCallbackInterfaces = new List<ICharacterContollerActions>();
        private readonly InputAction m_CharacterContoller_Move;
        private readonly InputAction m_CharacterContoller_Attack;
        private readonly InputAction m_CharacterContoller_Interaction;
        private readonly InputAction m_CharacterContoller_Dash;
        public struct CharacterContollerActions
        {
            private @InputMap m_Wrapper;
            public CharacterContollerActions(@InputMap wrapper) { m_Wrapper = wrapper; }
            public InputAction @Move => m_Wrapper.m_CharacterContoller_Move;
            public InputAction @Attack => m_Wrapper.m_CharacterContoller_Attack;
            public InputAction @Interaction => m_Wrapper.m_CharacterContoller_Interaction;
            public InputAction @Dash => m_Wrapper.m_CharacterContoller_Dash;
            public InputActionMap Get() { return m_Wrapper.m_CharacterContoller; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(CharacterContollerActions set) { return set.Get(); }
            public void AddCallbacks(ICharacterContollerActions instance)
            {
                if (instance == null || m_Wrapper.m_CharacterContollerActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_CharacterContollerActionsCallbackInterfaces.Add(instance);
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @Interaction.started += instance.OnInteraction;
                @Interaction.performed += instance.OnInteraction;
                @Interaction.canceled += instance.OnInteraction;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
            }

            private void UnregisterCallbacks(ICharacterContollerActions instance)
            {
                @Move.started -= instance.OnMove;
                @Move.performed -= instance.OnMove;
                @Move.canceled -= instance.OnMove;
                @Attack.started -= instance.OnAttack;
                @Attack.performed -= instance.OnAttack;
                @Attack.canceled -= instance.OnAttack;
                @Interaction.started -= instance.OnInteraction;
                @Interaction.performed -= instance.OnInteraction;
                @Interaction.canceled -= instance.OnInteraction;
                @Dash.started -= instance.OnDash;
                @Dash.performed -= instance.OnDash;
                @Dash.canceled -= instance.OnDash;
            }

            public void RemoveCallbacks(ICharacterContollerActions instance)
            {
                if (m_Wrapper.m_CharacterContollerActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(ICharacterContollerActions instance)
            {
                foreach (var item in m_Wrapper.m_CharacterContollerActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_CharacterContollerActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public CharacterContollerActions @CharacterContoller => new CharacterContollerActions(this);
        public interface ICharacterContollerActions
        {
            void OnMove(InputAction.CallbackContext context);
            void OnAttack(InputAction.CallbackContext context);
            void OnInteraction(InputAction.CallbackContext context);
            void OnDash(InputAction.CallbackContext context);
        }
    }
}
