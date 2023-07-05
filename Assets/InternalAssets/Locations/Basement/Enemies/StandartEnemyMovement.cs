﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace SouthBasement.AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public sealed class StandartEnemyMovement : MonoBehaviour, IEnemyMovable
    {
        [SerializeField] private AudioSource walkSound;
        
        private NavMeshAgent agent;
        private IEnemyMovable _enemyMovable;
        private Coroutine _waitCoroutine;

        public bool Blocked
        {
            get => agent.isStopped;
            set => agent.isStopped = value;
        }

        public Vector2 CurrentMovement => agent.velocity;

        private void Awake() => agent = GetComponent<NavMeshAgent>();

        private void Update()
        {
            if(walkSound == null)
                return;

            if(CurrentMovement != Vector2.zero && !walkSound.isPlaying)
                walkSound.Play();
            else walkSound.Stop();
        }

        public void Move(Vector2 to, Action onCompleted = null)
        {
            if (!Blocked)
            {
                agent.SetDestination(to);
                
                if (onCompleted != null)
                {
                    if (_waitCoroutine != null)
                        StopCoroutine(_waitCoroutine);
                    
                    _waitCoroutine = StartCoroutine(WaitForComplete(onCompleted));
                }
            }
        }

        private IEnumerator WaitForComplete(Action onCompleted)
        {
            yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance);
            onCompleted?.Invoke();
        }
    }
}