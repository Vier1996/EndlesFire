using DG.Tweening;
using UnityEngine;

namespace CustomEffects.WarningArea
{
    public class WarningZone : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _radius;
        [SerializeField] private ParticleSystem _radiusEffect;

        private Transform _selfTransform;
        
        private void Awake() => _selfTransform = transform;
        
        public void SetupWarningZone(float radius, float time)
        {
            Vector3 localScale = _selfTransform.localScale;
            localScale = new Vector3(
                localScale.x * radius,
                localScale.y * radius,
                localScale.z * radius
                );
            
            _selfTransform.localScale = localScale;

            _radius.DOFade(1f, time).SetEase(Ease.Linear).OnComplete(Break);

            ParticleSystem.MainModule module = _radiusEffect.main;
            module.startLifetime = time;
            
            _radiusEffect.Play();
        }

        public void Break()
        {
            _radius.DOPause();
            _radius.DOKill();
            _radiusEffect.Stop();
            
            Destroy(gameObject);
        }
    }
}
