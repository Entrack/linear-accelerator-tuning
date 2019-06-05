using System;
﻿using UnityEngine;
using System.Collections;

public class AcceleratorLens : MonoBehaviour {
	private LineRenderer lr;

	public bool isX = false;
	public bool isSource = false;
	public bool isDestination = false;
	public Vector3 hitPosition;
	public bool isHit;
	public bool wasHit;

	private Quaternion intialRotation;
    private AcceleratorAcademy myAcademy;

	public void ResetHitStatus() {
		isHit = isSource;
	}

	public void ResetHitPosition() {
		 hitPosition = new Vector3(0f, 0f, 0f);
	}

	public void RotateLensEuler(float angle) {
		float angleDiff = GetAngleFromQuaternion(intialRotation * Quaternion.Inverse(transform.localRotation));

		bool moreThanZero = true;
		if (angleDiff >= myAcademy.maxLensAngle + 1f) {
			angleDiff = 360f - angleDiff;
			moreThanZero = false;
		}
		if (angle >= 0) {
			if (angleDiff + angle < myAcademy.maxLensAngle) {
				RotateEuler(angle);
			}
			if (moreThanZero) {
				RotateEuler(angle);	
			}
		}
		else {
			if (-(-angleDiff + angle) < myAcademy.maxLensAngle) {
				RotateEuler(angle);
			}
			if (!moreThanZero) {
				RotateEuler(angle);	
			}
		}
	}

	private void RotateEuler(float angle) {
		if (isX) {
			transform.Rotate(angle, 0f, 0f, Space.Self);
		}
		else {
			transform.Rotate(0f, angle, 0f, Space.Self);
		}
	}

	public float GetRotation() {
		float rotation;
		if (isX) {
			rotation = transform.localRotation.x;
		}
		else {
			rotation = transform.localRotation.y;
		}
		return rotation;
	}

	public void ResetRotation() {
		transform.localRotation = intialRotation;
		RotateLensEuler(UnityEngine.Random.Range((-myAcademy.maxLensAngle + 1f) / 2, (myAcademy.maxLensAngle - 1f) / 2));
	}

	public float GetHitAccuracy() {
		float max = (float)Math.Sqrt(Math.Pow(transform.localScale.x, 2) + Math.Pow(transform.localScale.y, 2));
		float curr = (float)Math.Sqrt(Math.Pow(hitPosition.x, 2) + Math.Pow(hitPosition.y, 2));
		return (max - curr * 2) / max;
	}

	private void SaveInitialRotation() {
		intialRotation = transform.localRotation;
	}

	private float GetAngleFromQuaternion(Quaternion quat) {
		float res;
		if (isX) {
			res = quat.eulerAngles.x;
		}
		else {
			res = quat.eulerAngles.y;
		}
		return res;
	}
	
	void Start() {
		lr = GetComponent<LineRenderer>();
		myAcademy = GameObject.Find("Academy").GetComponent<AcceleratorAcademy>();
		ResetHitStatus();
		ResetHitPosition();
		SaveInitialRotation();
	}

	void Update() {
		if (isHit & (!isDestination)) {
			ResetHitStatus();
			Vector3 rayStartPosition = transform.position + transform.forward * transform.localScale.z + hitPosition;

			lr.enabled = true;
			lr.SetPosition(0, rayStartPosition);

			Ray ray = new Ray(rayStartPosition, transform.forward);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				if (hit.collider)
				{
					lr.SetPosition(1, hit.point);

					GameObject hitObject = hit.collider.gameObject;
					if (hitObject.tag == "lens") {
						AcceleratorLens nextLens = hitObject.GetComponent<AcceleratorLens>();
						nextLens.isHit = true;
						nextLens.wasHit = true;
						if (!nextLens.isSource) {
							nextLens.hitPosition = hit.point - hitObject.transform.position;
						}
					}
				}
			}
			else lr.SetPosition(1, transform.forward * 100);
		}
		else {
			lr.enabled = false;
		}
	}
}