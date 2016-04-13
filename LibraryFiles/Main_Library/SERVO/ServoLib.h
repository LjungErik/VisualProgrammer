/**************************************************
Filename: ServoLib.h
---------------------------------------------------
This header-file contains the definitions of all of
the functions that are needed and used when controlling
the Servos on the board

Functions found here:
 * Initialize the Servos Timer (PWM)
 * Power on the servos
 * Power off the servos
 * Move a given servo to a given position
 * Load the servos start position into the servos
 * Load the default start position into the servos

***************************************************/

#ifndef SERVOLIB_H
#define SERVOLIB_H

#include <avr/io.h>
#include <avr/interrupt.h>
#include "RobotLib.h"

//Initialize the Servos (Timers for PWM)
void InitServos(void);

//PowerOn the servos
void Power_On_Servos(void);

//PowerOff the servos
void Power_Off_Servos(void);

//Control the servos
void MoveServo(uint8_t servo, uint16_t position);
void LoadStartPosition(void);
void LoadDefaultStartPosition(void);

//Help functions
void Move(uint8_t servo, uint16_t position);
uint16_t GetServoPosition(uint8_t servo);

#if TEST
//Test function (To test the underlying functionality)
void TestServo(void);
#endif

//Defining the numbers for the servos
#define SERVO_1 1
#define SERVO_2 2
#define SERVO_3 3
#define SERVO_4 4
#define SERVO_5 5
#define SERVO_6 6

//The ratio of how much to change
#define CHANGE_RATIO 20

//Defining the out ports for the servos
#define Servo1_Pos OCR1A
#define Servo2_Pos OCR1B
#define Servo3_Pos OCR1C
#define Servo4_Pos OCR3A
#define Servo5_Pos OCR3B
#define Servo6_Pos OCR3C

#define SERVO_POWER (1 << PING4)


#endif /* SERVOLIB_H */