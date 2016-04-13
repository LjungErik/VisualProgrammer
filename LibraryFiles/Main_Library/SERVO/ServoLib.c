/**************************************************
Filename: ServoLib.c
---------------------------------------------------
This c-file contains the implementation for all of
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

#include "ServoLib.h"
#include "RobotLib.h"
#include "UARTLib.h"
#include <util/delay.h>

/* Initialize settings */

void InitServos(void)
{
	//INIT DDR ports (Output ports for servos)
	DDRB |= (1 << PINB5) | (1 << PINB6) | (1 << PINB7);
	DDRE |= (1 << PINE3) | (1 << PINE4) | (1 << PINE5);
	
	//Set the Compare output mode for the Channels
	TCCR1A = (1 << COM1A1) | (1 << COM1B1) | (1 << COM1C1);
	TCCR3A = (1 << COM3A1) | (1 << COM3B1) | (1 << COM3C1);
	
	//Set the Value of TCCRnB
	//Set Waveform generation mode and the prescaler
	TCCR1B = (1 << WGM13) | (1 << CS11);
	TCCR3B = (1 << WGM33) | (1 << CS31);

	//Set this to the max frequency (20000hz to achieve 50hz) (20000)
	ICR1 = 20000;
	ICR3 = 20000;
	
	//Clear TCNTn
	TCNT1 = 0x0000;
	TCNT3 = 0x0000;
	
	//Start global interrupt
	sei();
}

/* Power settings */

void Power_On_Servos(void)
{
	PORTG |= SERVO_POWER;

	Servo1_Pos = ServoPos[1];
	_delay_ms(50);
	Servo2_Pos = ServoPos[2];
	_delay_ms(50);
	Servo3_Pos = ServoPos[3];
	_delay_ms(50);
	Servo4_Pos = ServoPos[4];
	_delay_ms(50);
	Servo5_Pos = ServoPos[5];
	_delay_ms(50);
	Servo6_Pos = ServoPos[6];
}

void Power_Off_Servos(void)
{
	PORTG &= ~(SERVO_POWER);
}

/* Servo controls */

void MoveServo(uint8_t servo, uint16_t position)
{
	//Perform convertion
	//Count up the current position until at the correct location
	uint16_t currentPos = GetServoPosition(servo);

	//Keep moving servo until correct position is reached
	while (currentPos != position) 
	{
		int change = 0;

#if DEBUG
		Write("CurrentPos: ");
		WriteInt(currentPos);
		WriteLine("");

		Write("Position: ");
		WriteInt(position);
		WriteLine("");

		Write("Diff: ");
		WriteInt((position - currentPos));
		WriteLine("");
#endif

		if ((int)(position - currentPos) < 0) 
		{
			//Need to decrease the currentPos
			change = -(CHANGE_RATIO < (currentPos - position) ? CHANGE_RATIO : (currentPos - position));
		}
		else if ((int)(position - currentPos) > 0)
		{
			//Need to increase the currentPos
			change = (CHANGE_RATIO < (position - currentPos) ? CHANGE_RATIO : (position - currentPos));
		}

#if DEBUG
		Write("Change: ");
		WriteInt(change);
		WriteLine("");
#endif

		currentPos += change;

		Move(servo, currentPos);
		_delay_ms(50);
	}
}

void LoadStartPosition(void)
{
	ReadFromEEMemory();
}

void LoadDefaultStartPosition(void)
{
	Servo1_Pos = 1000;
	_delay_ms(50);
	Servo2_Pos = 1500;
	_delay_ms(50);
	Servo3_Pos = 1500;
	_delay_ms(50);
	Servo4_Pos = 1500;
	_delay_ms(50);
	Servo5_Pos = 1500;
	_delay_ms(50);
	Servo6_Pos = 1500;
}

/* Help-functions */
void Move(uint8_t servo, uint16_t position)
{
	switch(servo)
	{
		case SERVO_1:
		Servo1_Pos = position;
		break;
		case SERVO_2:
		Servo2_Pos = position;
		break;
		case SERVO_3:
		Servo3_Pos = position;
		break;
		case SERVO_4:
		Servo4_Pos = position;
		break;
		case SERVO_5:
		Servo5_Pos = position;
		break;
		case SERVO_6:
		Servo6_Pos = position;
		break;
	}
}

uint16_t GetServoPosition(uint8_t servo) 
{
	switch(servo)
	{
		case SERVO_1:
			return (uint16_t)Servo1_Pos;
		case SERVO_2:
			return (uint16_t)Servo2_Pos;
		case SERVO_3:
			return (uint16_t)Servo3_Pos;
		case SERVO_4:
			return (uint16_t)Servo4_Pos;
		case SERVO_5:
			return (uint16_t)Servo5_Pos;
		case SERVO_6:
			return (uint16_t)Servo6_Pos;
	}
	return (uint16_t)0;
}

#if TEST
/* ----------------------------------------------------- */
// Holds the test function for the class (test the functionality)
/* ----------------------------------------------------- */
void TestServo(void)
{
	WriteLine("Initializing the servos...");
	InitServos();
	WriteLine("Powering on the servos...");
	Power_On_Servos();
	WriteLine("Loading the start positions...");
	LoadStartPosition();

	_delay_ms(2000);

	MoveServo(1, 700);
	_delay_ms(2000);
	MoveServo(1, 1500);
	_delay_ms(2000);
	MoveServo(2, 700);
	_delay_ms(2000);
	MoveServo(2, 1500);
	_delay_ms(2000);
	MoveServo(3, 700);
	_delay_ms(2000);
	MoveServo(3, 1500);
	_delay_ms(2000);
	MoveServo(4, 700);
	_delay_ms(2000);
	MoveServo(4, 1500);
	_delay_ms(2000);
	MoveServo(5, 700);
	_delay_ms(2000);
	MoveServo(5, 1500);
	_delay_ms(2000);
	MoveServo(6, 700);
	_delay_ms(2000);
	MoveServo(6, 1500);
	_delay_ms(2000);

	Power_Off_Servos();
}
/* ----------------------------------------------------- */
#endif