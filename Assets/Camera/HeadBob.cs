//Author: Kyle Shepherd
//Headbobbing for first-person camera, attach to camera game object
using UnityEngine;

public class HeadBob : MonoBehaviour
{
	[SerializeField]
	float bobbingSpeed, bobbingAmount, midpoint, bobLerpTime;

	float timer, waveslice, horizontal, vertical, translateChange, totalAxes;

	void Start()
	{
		midpoint = transform.localPosition.y;
	}

	// Update is called once per frame
	void Update()
	{
		waveslice = 0f;
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");

		if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
		{
			timer = 0f;
		}
		else
		{
			waveslice = Mathf.Sin(timer);
			timer = timer + bobbingSpeed;

			if (timer > Mathf.PI * 2)
			{
				timer = timer - (Mathf.PI * 2);
			}
		}

		if (waveslice != 0)
		{
			translateChange = waveslice * bobbingAmount;
			totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
			totalAxes = Mathf.Clamp(totalAxes, 0, 1);
			translateChange = totalAxes * translateChange;
			Vector3 pos = transform.localPosition;
			pos.y = midpoint + translateChange;
			transform.localPosition = pos;
		}
		else
		{
			Vector3 pos = transform.localPosition;
			pos.y = Mathf.Lerp(pos.y, midpoint, bobLerpTime);
			transform.localPosition = pos;
		}
	}
}
