
using UnityEngine;

/// <summary>
/// Player controller and behavior
/// </summary>
public class PlayerScript : MonoBehaviour
{
	/// <summary>
	/// 1 - The speed of the ship
	/// </summary>
	public Vector2 speed = new Vector2(50, 50);
	
	// 2 - Store the movement
	private Vector2 movement;

	private float timeLeft = 30;

	void Update()
	{
		// 3 - Retrieve axis information
		float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");

		float newX = speed.x * inputX;
		float newY = speed.y * inputY;

		Vector3 cameraPosition = Camera.main.WorldToViewportPoint (transform.position);
		if(cameraPosition.x + newX > Camera.main.pixelRect.xMax)
		{
			newX = 0;
		}
		if(cameraPosition.y + newY > Camera.main.pixelRect.yMax)
		{
			newY = 0;
		}

		// 4 - Movement per direction
		movement = new Vector2 (newX, newY);

		// 5 - Shooting
		bool shoot = Input.GetButtonDown("Fire1");
		shoot |= Input.GetButtonDown("Fire2");
		// Careful: For Mac users, ctrl + arrow is a bad idea
		
		if (shoot)
		{
			WeaponScript weapon = GetComponent<WeaponScript>();
			if (weapon != null)
			{
				// false because the player is not an enemy
				weapon.Attack(false);
			}
		}

		GameObject[] remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
		if(remainingEnemies == null || remainingEnemies.Length == 0) {
			GameObject scoreText = GameObject.Find("ScoreText");
			if (!scoreText.GetComponent<TextMesh>().text.StartsWith("Final"))
			{
				scoreText.GetComponent<TextMesh>().text = "Final: " + scoreText.GetComponent<TextMesh>().text;
				Vector3 currentPosition = scoreText.transform.position;

				currentPosition.x = currentPosition.x - 5;
				scoreText.transform.position = currentPosition;
			}
		}


		if (timeLeft > 0){
			timeLeft -= Time.deltaTime;
			GameObject.Find("TimerText").GetComponent<TextMesh>().text = timeLeft.ToString("F0");
		}
	}
	
	void FixedUpdate()
	{
		// 5 - Move the game object
		rigidbody2D.velocity = movement;
	}
}

