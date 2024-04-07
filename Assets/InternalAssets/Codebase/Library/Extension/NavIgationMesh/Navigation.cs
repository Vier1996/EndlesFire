using System;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace Codebase.Library.Extension.NavIgationMesh
{
    public static class Navigation
    {
        /*public static Observable<bool> MoveTo(this NavMeshAgent agent, Vector3 targetPosition, float maxDistanceRadius = 2f, float remainingDistance = 0.1f, Action completeCallback = null)
        {
            return Observable.Create<bool>(observer =>
            {
                if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, maxDistanceRadius, 1) == false)
                    targetPosition = agent.transform.position;

                agent.SetDestination(targetPosition);
                agent.isStopped = false;

                void CheckRemainDistance()
                {
                    if (agent == null)
                    {
                        completeCallback = null;
                        observer.OnCompleted();
                        return;
                    }

                    if (agent.remainingDistance > remainingDistance)
                    {
                        observer.OnNext(false);
                    }
                    else
                    {
                        agent.isStopped = true;
                        completeCallback?.Invoke();

                        observer.OnNext(true);
                        observer.OnCompleted();
                    }
                }

                return new CompositeDisposable
                {
                    Observable.EveryUpdate().Subscribe(_ => CheckRemainDistance()),
                };
            });
        }*/
    }
}