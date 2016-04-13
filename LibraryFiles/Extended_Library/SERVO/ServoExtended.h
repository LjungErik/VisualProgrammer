/**************************************************
Filename: ServoExtended.h
---------------------------------------------------
This header-file contains the definitions of 
extended functions for the servos.

Functions found here:
	* Start the servos (init, load and power on)
	* Stop the servos (power off)
	* Move a selected servo a given position in degrees
	* Test function for testing the library

***************************************************/

#ifndef SERVOEXTEND_H
#define SERVOEXTEND_H

#include <avr/io.h>
#include "ServoLib.h"

void StartServo(void);
void StopServo(void);

void MoveDegrees(uint8_t servo, uint8_t degrees);

#if TEST
void TestServoExtended(void);
#endif

//Degrees per increase unit ratio for each servo
#define SERVO1_INCREASE_RATIO 8
#define SERVO2_INCREASE_RATIO 9
#define SERVO3_INCREASE_RATIO 10
#define SERVO4_INCREASE_RATIO 10
#define SERVO5_INCREASE_RATIO 11
#define SERVO6_INCREASE_RATIO 13

//Degrees per decrease unit ratio for each servo
#define SERVO1_DECREASE_RATIO 12
#define SERVO2_DECREASE_RATIO 15
#define SERVO3_DECREASE_RATIO 12
#define SERVO4_DECREASE_RATIO 11
#define SERVO5_DECREASE_RATIO 11
#define SERVO6_DECREASE_RATIO 10

#define START_ANGLE 90

//CurrentPosition of the servos in degrees
uint8_t ServoPosDegrees[7];

//Define unsafe tag (unsafe deactivated)
#define UNSAFE 0

#if !UNSAFE
//The risk zone for the servos
#define RISK_ZONE_Y -3 //[cm]

//The hypotenuse for the given servos [cm] (Arm length )
#define SERVO1_HYPOTENUSE 0
#define SERVO2_HYPOTENUSE 0
#define SERVO3_HYPOTENUSE 21 //From Servo 3 to top of claw
#define SERVO4_HYPOTENUSE 12
#define SERVO5_HYPOTENUSE 10
#define SERVO6_HYPOTENUSE 0

#define START_SERVO 5

#define LAST_SERVO 3

#endif

#endif /* SERVOLIB_H */