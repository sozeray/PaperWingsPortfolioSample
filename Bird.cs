	void Animate (float rotAmount) {

		bendFactor = Mathf.Lerp (bendFactor, -holdTime * 3 * currentScale.x * currentScale.y, Time.deltaTime * 10);
		bendFactor = Mathf.Clamp (bendFactor, -1.2f, 1.2f);

		float tempFactor = bendFactor * 25;

		myHeadJointTransform.localEulerAngles = Vector3.forward * tempFactor;
		myTailJointTransform.localEulerAngles = -Vector3.forward * tempFactor * 1.5f;
		myLegJointTransform.localEulerAngles = -Vector3.forward * Mathf.Clamp (myVelocity.x * 4f + tempFactor * 2.5f, -45, 45);

		myHeadJointTransform.localPosition = new Vector3 (-0.25f - Mathf.Abs(bendFactor) * 0.05f, 0, 0);

		if (Mathf.Abs (Mathf.Abs (myTransform.position.x) - limitPosX) < 1)
		{
			float contraPosX = (myTransform.position.x > 0) ? myTransform.position.x - limitPosX * 2 : myTransform.position.x + limitPosX * 2;

			if (!myClone.activeSelf)
				myClone.SetActive (true);

			myClone.transform.position = new Vector3 (contraPosX, myTransform.position.y, myTransform.position.z);
			myClone.transform.rotation = myBodyTransform.rotation;
			myClone.transform.localScale = myBodyTransform.localScale;
		}

		else if (myClone.activeSelf)
			myClone.SetActive (false);

		if (Mathf.Abs(holdTime) <= 0.5f)
		{
			if (myVelocity.x >= 0.2f && currentScale.x != 1)
				currentScale.x = 1;

			else if (myVelocity.x < -0.2f && currentScale.x != -1)
				currentScale.x = -1;

			if (myTransform.eulerAngles.z > 100 && myTransform.eulerAngles.z < 260 && currentScale.y != -1)
				currentScale.y = -1;

			else if ((myTransform.eulerAngles.z <= 80 || myTransform.eulerAngles.z >= 280) && currentScale.y != 1)
				currentScale.y = 1;
		}

		else if (holdTime > 0.5f)
		{
			if (myVelocity.x > 0 && currentScale.y != -1)
				currentScale.y = -1;

			else if (myVelocity.x < 0 && currentScale.y != 1)
				currentScale.y = 1;
		}

		else if (holdTime < -0.5f)
		{
			if (myVelocity.x < 0 && currentScale.y != -1)
				currentScale.y = -1;

			else if (myVelocity.x > 0 && currentScale.y != 1)
				currentScale.y = 1;
		}

		Quaternion rot = Quaternion.identity;
		rot.eulerAngles = new Vector3 (90 * (1 - currentScale.y), 90 * (1 - currentScale.x), 0);
		myBodyTransform.localRotation = Quaternion.Slerp (myBodyTransform.localRotation, rot, Time.deltaTime * 5);

		float spd = Mathf.Abs(myVelocity.x);

    	if (spd >= 8 && flapping)
    	{
    		flapping = false;
    		myBodyAnimator.CrossFade("Dive", 0.25f, -1, 0);

    		StartCoroutine (Squint(false));
    	}

    	else if (spd < 8 && !flapping)
    	{
    		flapping = true;
    		myBodyAnimator.CrossFade("Flap", 0.25f, -1, 0);

    		StartCoroutine (Blink(false));
		}

		myBodyAnimator.speed = flapping ? Mathf.Min (2.75f, 7.5f/spd) : 1;
	}