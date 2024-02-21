
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace SonicBloom.Koreo.Demos
{

	public class NoteObjectCustom : MonoBehaviour
	{


		[Tooltip("The visual to use for this Note Object.")]
		public SpriteRenderer visuals;

		// If active, the KoreographyEvent that this Note Object wraps.  Contains the relevant timing information.
		KoreographyEvent trackedEvent;

		// If active, the Lane Controller that this Note Object is contained by.
		//LaneController laneController;

		// If active, the Rhythm Game Controller that controls the game this Note Object is found within.
		RhythmController gameController;

		public SpriteRenderer visuals_outer;

		// Unclamped Lerp.  Same as Vector3.Lerp without the [0.0-1.0] clamping.
		static Vector3 Lerp(Vector3 from, Vector3 to, float t)
		{
			return new Vector3(from.x + (to.x - from.x) * t, from.y + (to.y - from.y) * t, from.z + (to.z - from.z) * t);
		}



		// Prepares the Note Object for use.
		public void Initialize(KoreographyEvent evt, Color color, RhythmController gameCont)
		{
			trackedEvent = evt;
			visuals.color = color;
			//laneController = laneCont;
			gameController = gameCont;

			UpdatePosition();
		}

		// Resets the Note Object to its default state.
		void Reset()
		{
			trackedEvent = null;
			//laneController = null;
			gameController = null;
		}

		void Update()
		{
			UpdateScale();
			IsNoteMissed();
			//UpdatePosition();

			//if (transform.position.y <= laneController.DespawnY)
			//{
			//gameController.ReturnNoteObjectToPool(this);
			//Reset();
			//}
		}

		// Updates the height of the Note Object.  This is relative to the speed at which the notes fall and 
		//  the specified Hit Window range.
		void UpdateScale()
		{
			//float baseUnitHeight = visuals.sprite.rect.height / visuals.sprite.pixelsPerUnit;
			//float targetTimeStamp = gameController.WindowSizeInUnits * 2f; // Double it for before/after.
			Vector3 scale = transform.localScale;
			scale = scale * Mathf.Lerp(1, 3, Mathf.Clamp((trackedEvent.StartSample - gameController.DelayedSampleTime) / gameController.ShowTimeOffSet, 0, 1));
			visuals_outer.transform.localScale = scale;

		}

		// Updates the position of the Note Object along the Lane based on current audio position.
		void UpdatePosition()
		{
			float minX = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
			float maxX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
			float minY = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
			float maxY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;

			// Generate random position
			float randomX = Random.Range(minX, maxX);
			float randomY = Random.Range(minY, maxY);

			// Set the position of the sprite
			transform.position = new Vector3(randomX, randomY, 10f);
		}

		// Checks to see if the Note Object is currently hittable or not based on current audio sample
		//  position and the configured hit window width in samples (this window used during checks for both
		//  before/after the specific sample time of the Note Object).
		public bool IsNoteHittable()
		{
			int noteTime = trackedEvent.StartSample;
			int curTime = gameController.DelayedSampleTime;
			int hitWindow = gameController.HitWindowSampleWidth;

			return (Mathf.Abs(noteTime - curTime) <= hitWindow);
		}

		// Checks to see if the note is no longer hittable based on the configured hit window width in
		//  samples.
		public bool IsNoteMissed()
		{
			bool bMissed = true;

			if (enabled)
			{
				int noteTime = trackedEvent.StartSample;
				int curTime = gameController.DelayedSampleTime;
				int hitWindow = gameController.HitWindowSampleWidth;

				bMissed = (curTime - noteTime > hitWindow);
			}

			if(bMissed)
				this.gameObject.SetActive(false);
		
			return bMissed;


		}

		// Returns this Note Object to the pool which is controlled by the Rhythm Game Controller.  This
		//  helps reduce runtime allocations.
		void ReturnToPool()
		{
			gameController.ReturnNoteObjectToPool(this);
			Reset();
		}

		// Performs actions when the Note Object is hit.
		public void OnHit()
		{
			ReturnToPool();
		}

		// Performs actions when the Note Object is cleared.
		public void OnClear()
		{
			ReturnToPool();
		}


	}
}
