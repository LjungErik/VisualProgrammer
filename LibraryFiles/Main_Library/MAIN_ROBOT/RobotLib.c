/**************************************************
Filename: RobotLib.c
---------------------------------------------------
This c-file contains the implementation for all of
the functions that are needed and used when controlling
the Servos on the board

Functions found here:
 * Read from the EEPROM

***************************************************/

#include "RobotLib.h"
#include "UARTLib.h"
#include <avr/eeprom.h>

//functions

void ReadFromEEMemory(void) 
{
	ServoPos[1] = ReadEEValue(2)  | (ReadEEValue(3) << 8);
	ServoPos[2] = ReadEEValue(4)  | (ReadEEValue(5) << 8);
	ServoPos[3] = ReadEEValue(6)  | (ReadEEValue(7) << 8);
	ServoPos[4] = ReadEEValue(8)  | (ReadEEValue(9) << 8);
	ServoPos[5] = ReadEEValue(10) | (ReadEEValue(11) << 8);
	ServoPos[6] = ReadEEValue(12) | (ReadEEValue(13) << 8);
}

uint8_t ReadEEValue(uint8_t byte)
{
	eeprom_busy_wait();
	EEAR = byte;
	EECR |= (1 << EERE);
	return EEDR;
}

/* ----------------------------------------------------- */
// Holds the test function for the class (test the functionality)
/* ----------------------------------------------------- */
void TestRobot(void)
{
	ReadFromEEMemory();

	Write("Servo1: ");
	WriteInt(ServoPos[1]);
	WriteLine("");

	Write("Servo2: ");
	WriteInt(ServoPos[2]);
	WriteLine("");

	Write("Servo3: ");
	WriteInt(ServoPos[3]);
	WriteLine("");

	Write("Servo4: ");
	WriteInt(ServoPos[4]);
	WriteLine("");

	Write("Servo5: ");
	WriteInt(ServoPos[5]);
	WriteLine("");

	Write("Servo6: ");
	WriteInt(ServoPos[6]);
	WriteLine("");
}
/* ----------------------------------------------------- */