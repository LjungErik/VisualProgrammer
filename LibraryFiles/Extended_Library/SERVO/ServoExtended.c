/**************************************************
Filename: ServoExtended.c
---------------------------------------------------
This c-file contains the implementation for 
extended functionality of the servos

Functions found here:


***************************************************/

#include "ServoExtended.h"
#include "UARTLib.h"

/* Help functions */
uint8_t GetServoRatio(uint8_t servo)
{
	switch (servo)
	{
		case SERVO_1:
			return SERVO1_RATIO;
		case SERVO_2:
			return SERVO2_RATIO;
		case SERVO_3:
			return SERVO3_RATIO;
		case SERVO_4:
			return SERVO4_RATIO;
		case SERVO_5:
			return SERVO5_RATIO;
		case SERVO_6:
			return SERVO6_RATIO;
	}
	return 0;
}

uint16_t GetServoMax(uint8_t servo)
{
	switch (servo)
	{
	case SERVO_1:
		return SERVO1_MAX;
	case SERVO_2:
		return SERVO2_MAX;
	case SERVO_3:
		return SERVO3_MAX;
	case SERVO_4:
		return SERVO4_MAX;
	case SERVO_5:
		return SERVO5_MAX;
	case SERVO_6:
		return SERVO6_MAX;
	}
	return 0;
}

uint16_t GetServoMin(uint8_t servo)
{
	switch (servo)
	{
	case SERVO_1:
		return SERVO1_MIN;
	case SERVO_2:
		return SERVO2_MIN;
	case SERVO_3:
		return SERVO3_MIN;
	case SERVO_4:
		return SERVO4_MIN;
	case SERVO_5:
		return SERVO5_MIN;
	case SERVO_6:
		return SERVO6_MIN;
	}
	return 0;
}

/* Start and stop servos */
void StartServo(void)
{
	InitServos();
	LoadStartPosition();
	Power_On_Servos();
}

void StopServo(void)
{
	Power_Off_Servos();
}

/* Move the servos */
void MoveDegrees(uint8_t servo, uint8_t degrees)
{
	if (degrees > 180)
		degrees = 180;

	uint8_t degreeRatio = GetServoRatio(servo);

	Write("Degree Ratio: ");
	WriteInt(degreeRatio);
	WriteLine("");

	Write("MinServo: ");
	WriteInt(GetServoMin(servo));
	WriteLine("");

	//Convert the degrees to the raw position
	uint16_t position = GetServoMin(servo) + (uint16_t)(degrees * degreeRatio);

	Write("Degree: ");
	WriteInt(degrees);
	WriteLine("");

	position = (position < GetServoMax(servo) ? position : GetServoMax(servo));

	MoveServo(servo, position);
}