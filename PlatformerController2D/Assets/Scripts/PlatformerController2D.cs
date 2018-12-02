using System;
using UnityEngine;

namespace PlatformerController
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class PlatformerController2D : MonoBehaviour
	{
		#region Member Variables

		[SerializeField] LayerMask collisionMask;

		[Range(0.01f, 1f)]
		[SerializeField] float skinWidth = 0.01f;

		[SerializeField] int horizontalRayCount = 3;
		[SerializeField] int verticalRayCount = 3;

		float horizontalRaySpacing;
		float verticalRaySpacing;

		BoxCollider2D boxCollider;
		RaycastBounds raycastBounds;
		public CollisionInfo collisionInfo;

		#endregion

		#region Unity Methods

		private void Start()
		{
			boxCollider = GetComponent<BoxCollider2D>();
			
			CalculateRaySpacing();
		}

		#endregion

		#region Helper Methods - Public

		public void Move(Vector3 velocity)
		{
			collisionInfo.Reset();
			UpdateRaycastBounds();

			if(velocity.x != 0)
				HorizontalCollisions(ref velocity);

			if(velocity.y != 0)
				VerticalCollisions(ref velocity);

			transform.Translate(velocity);
		}

		#endregion

		#region Helper Methods - Private

		void UpdateRaycastBounds()
		{
			Bounds bounds = boxCollider.bounds;
			// push the bounds inwards by skin width from all sides
			bounds.Expand(skinWidth * -2);

			raycastBounds.topLeft = new Vector2(bounds.min.x, bounds.max.y);
			raycastBounds.topRight = new Vector2(bounds.max.x, bounds.max.y);
			raycastBounds.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
			raycastBounds.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
		}

		void CalculateRaySpacing()
		{
			Bounds bounds = boxCollider.bounds;
			// push the bounds inwards by skin width from all sides
			bounds.Expand(skinWidth * -2);

			horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
			verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

			horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
			verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
		}

		/// <summary>
		/// Check for collisions on the left and right of the player
		/// </summary>
		/// <param name="velocity">player movement velocity</param>
		void HorizontalCollisions(ref Vector3 velocity)
		{
			float directionX = Mathf.Sign(velocity.x);
			float rayLength = Mathf.Abs(velocity.x) + skinWidth;

			for (int i = 0; i < horizontalRayCount; i++)
			{
				Vector2 rayOrigin = (directionX < 0) ? raycastBounds.bottomLeft : raycastBounds.bottomRight;
				rayOrigin += Vector2.up * (horizontalRaySpacing * i);

				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

				Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

				if (hit)
				{
					velocity.x = (hit.distance - skinWidth) * directionX;
					rayLength = hit.distance;

					collisionInfo.left = directionX < 0;
					collisionInfo.right = directionX >= 0;
				}
			}
		}

		/// <summary>
		/// Check for collisions above and below the player
		/// </summary>
		/// <param name="velocity">player movement velocity</param>
		void VerticalCollisions(ref Vector3 velocity)
		{
			float directionY = Mathf.Sign(velocity.y);
			float rayLength = Mathf.Abs(velocity.y) + skinWidth;

			for (int i = 0; i < verticalRayCount; i++)
			{
				Vector2 rayOrigin = (directionY < 0) ? raycastBounds.bottomLeft : raycastBounds.topLeft;
				rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);

				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

				Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

				if (hit)
				{
					velocity.y = (hit.distance - skinWidth) * directionY;
					rayLength = hit.distance;

					collisionInfo.below = directionY < 0;
					collisionInfo.above = directionY >= 0;
				}
			}
		}

		#endregion
		
	}

	public struct RaycastBounds
	{
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}

	public struct CollisionInfo
	{
		public bool above, below;
		public bool left, right;

		public void Reset()
		{
			above = below = left = right = false;
		}
	}
}
