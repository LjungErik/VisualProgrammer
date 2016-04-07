/**************************************************
Filename: ServoExtended.h
---------------------------------------------------
This header-file contains the definitions of 
extended functions for the servos.

Functions found here:

***************************************************/

#ifndef SERVOEXTEND_H
#define SERVOEXTEND_H

#include <avr/io.h>
#include "ServoLib.h"

void StartServo(void);
void StopServo(void);

void MoveDegrees(uint8_t servo, uint8_t degrees);

//Degrees per unit ratio for each servo
#define SERVO1_RATIO 8
#define SERVO2_RATIO 12
#define SERVO3_RATIO 10
#define SERVO4_RATIO 11
#define SERVO5_RATIO 11
#define SERVO6_RATIO 11

//Max value for the servos
#define SERVO1_MAX 2050
#define SERVO2_MAX 2120
#define SERVO3_MAX 2500
#define SERVO4_MAX 2430
#define SERVO5_MAX 2390
#define SERVO6_MAX 2400

//Min values for the servos
#define SERVO1_MIN 800
#define SERVO2_MIN 780
#define SERVO3_MIN 630
#define SERVO4_MIN 530
#define SERVO5_MIN 440
#define SERVO6_MIN 450

#define START_ANGLE 90

#endif /* SERVOLIB_H */