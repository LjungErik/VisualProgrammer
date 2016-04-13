/**************************************************
Filename: ServoExtended.c
---------------------------------------------------
This c-file contains the implementation for 
extended functionality of the servos

Functions found here:
	* Get the Hytotenuse for a given servo
	* Check if the new position is valid (not in risk zone)
	* Get the increase ratio per degrees
	* Get the decrease ratio per degrees 
	* Set default start position in degrees
	* Start the servos (init, load and power on)
	* Stop the servos (power off)
	* Move a selected servo a given position in degrees
	* Test function for testing the library

***************************************************/

#include "ServoExtended.h"
#include "UARTLib.h"
#include <util/delay.h>
#include <math.h>

/* Private Methods */

#if !UNSAFE

/* The hyptotenuse of the servo */
uint8_t GetHypotenuse(uint8_t servo)
{
	switch (servo)
	{
	case SERVO_1:
		return SERVO1_HYPOTENUSE;
	case SERVO_2:
		return SERVO2_HYPOTENUSE;
	case SERVO_3:
		return SERVO3_HYPOTENUSE;
	case SERVO_4:
		return SERVO4_HYPOTENUSE;
	case SERVO_5:
		return SERVO5_HYPOTENUSE;
	case SERVO_6:
		return SERVO6_HYPOTENUSE;
	}
	return 0;
}

//Checks that the servos have not entered a unsafe state
//The lowest point is below the risk zone
BOOL IsValid() {
	//Current start coordinates
	int x_current = 0;
	int y_current = 0;

	//Current angle in degrees
	int degrees_current = 0;

	//Go through all of the servos and
	//check if in risk zone (servo 6 is irrelavant (2,1 are special cases))
	for (uint8_t i = START_SERVO; i > (LAST_SERVO - 1); i--)
	{
		//Calculate the current angle (added with current angle)
		degrees_current = (ServoPosDegrees[i] - START_ANGLE) + (degrees_current);
		//Convert to radian for cos and sin functions
		double radian = degrees_current * (M_PI / 180);

		//Calculates the x and the y coordinates for the servo
		int xi = (int)(sin(radian) * GetHypotenuse(i));
		int yi = (int)(cos(radian) * GetHypotenuse(i));

#if DEBUG
		Write("Xi = ");
		WriteInt(xi);
		WriteLine("");
		Write("Yi = ");
		WriteInt(yi);
		WriteLine("");
#endif

		x_current = xi + x_current;
		y_current = yi + y_current;

		if (y_current < RISK_ZONE_Y)
		{
			return FALSE;
		}
	}
	return TRUE;
}

#endif

/* Help functions */
uint8_t GetServoIncreaseRatio(uint8_t servo)
{
	switch (servo)
	{
		case SERVO_1:
			return SERVO1_INCREASE_RATIO;
		case SERVO_2:
			return SERVO2_INCREASE_RATIO;
		case SERVO_3:
			return SERVO3_INCREASE_RATIO;
		case SERVO_4:
			return SERVO4_INCREASE_RATIO;
		case SERVO_5:
			return SERVO5_INCREASE_RATIO;
		case SERVO_6:
			return SERVO6_INCREASE_RATIO;
	}
	return 0;
}

uint8_t GetServoDecreaseRatio(uint8_t servo)
{
	switch (servo)
	{
	case SERVO_1:
		return SERVO1_DECREASE_RATIO;
	case SERVO_2:
		return SERVO2_DECREASE_RATIO;
	case SERVO_3:
		return SERVO3_DECREASE_RATIO;
	case SERVO_4:
		return SERVO4_DECREASE_RATIO;
	case SERVO_5:
		return SERVO5_DECREASE_RATIO;
	case SERVO_6:
		return SERVO6_DECREASE_RATIO;
	}
	return 0;
}

void SetDefaultDegrees(void)
{
	ServoPosDegrees[1] = START_ANGLE;
	ServoPosDegrees[2] = START_ANGLE;
	ServoPosDegrees[3] = START_ANGLE;
	ServoPosDegrees[4] = START_ANGLE;
	ServoPosDegrees[5] = START_ANGLE;
	ServoPosDegrees[6] = START_ANGLE;
}

/* Private Methods */

/* Start and stop servos */
void StartServo(void)
{
	InitServos();
	LoadStartPosition();
	SetDefaultDegrees();
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

	uint8_t degreeRatio = 0;

	//Get the degree ratio for convertion to actual posiition
	if ((degrees - START_ANGLE) > 0)
	{
		degreeRatio = GetServoIncreaseRatio(servo);
	}
	else if ((degrees - START_ANGLE) < 0)
	{
		degreeRatio = GetServoDecreaseRatio(servo);
	}

#if DEBUG
	Write("Degree Ratio: ");
	WriteInt(degreeRatio);
	WriteLine("");

	Write("Degree: ");
	WriteInt(degrees);
	WriteLine("");

	Write("Servo: ");
	WriteInt(servo);
	WriteLine("");
#endif

	//Convert the degrees to the raw position
	uint16_t position = ServoPos[servo] + ((degrees - START_ANGLE) * degreeRatio);
	// Servo 4 is a special case for calculating position
	if (servo == SERVO_4)
	{
		position = ServoPos[servo] + ((START_ANGLE - degrees) * degreeRatio);
	}

#if DEBUG
	Write("New Servo Postion: ");
	WriteInt(position);
	WriteLine("");
#endif

#if !UNSAFE
	//Temperary save the location (if validation fails)
	uint8_t temp = ServoPosDegrees[servo];
	//Update the current position in degrees
	ServoPosDegrees[servo] = degrees;

	//Make sure the movement can be made
	if (IsValid() == 1)
	{
		MoveServo(servo, position);
	}
	else
	{
		WriteLine("##WARNING#####WARNING#####WARNING#####WARNING#####WARNING##");
		WriteLine("# Could not move the servo, will put arm in the risk zone #");
		WriteLine("##WARNING#####WARNING#####WARNING#####WARNING#####WARNING##");
		ServoPosDegrees[servo] = temp;
	}
#else
	//Update the current position in degrees
	ServoPosDegrees[servo] = degrees;

	MoveServo(servo, position);
#endif
	
}

#if TEST

void TestServoExtended(void)
{	
	StartServo();

	_delay_ms(5000);
	MoveDegrees(1, 135);
	_delay_ms(5000);
	MoveDegrees(1, 45);
	_delay_ms(5000);
	MoveDegrees(1, 90);
	_delay_ms(5000);
	MoveDegrees(2, 135);
	_delay_ms(5000);
	MoveDegrees(2, 45);
	_delay_ms(5000);
	MoveDegrees(2, 90);
	_delay_ms(5000);
	MoveDegrees(3, 135);
	_delay_ms(5000);
	MoveDegrees(3, 45);
	_delay_ms(5000);
	MoveDegrees(3, 90);
	_delay_ms(5000);
	MoveDegrees(4, 135);
	_delay_ms(5000);
	MoveDegrees(4, 45);
	_delay_ms(5000);
	MoveDegrees(4, 90);
	_delay_ms(5000);
	MoveDegrees(5, 135);
	_delay_ms(5000);
	MoveDegrees(5, 45);
	_delay_ms(5000);
	MoveDegrees(5, 90);
	_delay_ms(5000);
	MoveDegrees(6, 135);
	_delay_ms(5000);
	MoveDegrees(6, 45);
	_delay_ms(5000);
	MoveDegrees(6, 90);
	_delay_ms(5000);

#if !UNSAFE
	/* WARNING WARNING WARNING */
	/* THIS CODE SHOULD ONLY BE RUN WHEN UNSAFE is 0 */
	/* -- [TEST 1] -- */
	_delay_ms(5000);
	MoveDegrees(3, 90);
	_delay_ms(5000);
	MoveDegrees(5, 45);
	_delay_ms(5000);
	MoveDegrees(4, 0);
	_delay_ms(5000);

	/* -- [TEST 2] -- */
	MoveDegrees(5, 135);
	_delay_ms(5000);
	MoveDegrees(3, 90);
	_delay_ms(5000);
	MoveDegrees(4, 180);
	_delay_ms(5000);

	/* -- [TEST 3] -- */
	MoveDegrees(5, 90);
	_delay_ms(5000);
	MoveDegrees(4, 180);
	_delay_ms(5000);
	MoveDegrees(3, 180);
	_delay_ms(5000);

	/* -- [TEST 4] -- */
	MoveDegrees(5, 90);
	_delay_ms(5000);
	MoveDegrees(4, 0);
	_delay_ms(5000);
	MoveDegrees(3, 0);
	_delay_ms(5000);

	/* -- [TEST 5] -- */
	MoveDegrees(4, 180);
	_delay_ms(5000);
	MoveDegrees(5, 0);
	_delay_ms(5000);
	MoveDegrees(3, 0);
	_delay_ms(5000);

	/* -- [TEST 6] -- */
	MoveDegrees(5, 90);
	_delay_ms(5000);
	MoveDegrees(4, 90);
	_delay_ms(5000);
	MoveDegrees(3, 0);
	_delay_ms(5000);
	
	MoveDegrees(5, 0);
	_delay_ms(5000);
	MoveDegrees(4, 0);
	_delay_ms(5000);

	/* -- [TEST 7] -- */
	MoveDegrees(5, 135);
	_delay_ms(5000);
	MoveDegrees(4, 180);
	_delay_ms(5000);
	MoveDegrees(3, 0);
	_delay_ms(5000);

	MoveDegrees(3, 45);
	_delay_ms(5000);
	MoveDegrees(3, 90);
	_delay_ms(5000);

#endif

	StopServo();
}
#endif