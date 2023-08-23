using UnityEngine;

namespace AFSInterview
{
	public class AttackVisualisation : MonoBehaviour
	{
		[SerializeField] private ParticleSystem particleSystem;

		public void SetParticleSystem(Vector3 target, Vector3 origin, float particleLifeTime)
		{
			particleSystem.transform.position = target;
			var shape = particleSystem.shape;
			shape.position = particleSystem.transform.InverseTransformPoint(origin) + Vector3.up;

			particleSystem.startLifetime = particleLifeTime;
			particleSystem.Play();
		}
	}
}