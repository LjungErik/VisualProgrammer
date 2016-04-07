/**************************************************
Filename: RobotLib.h
---------------------------------------------------
This header-file contains the definitions of all of
the base functions that are shared amoung the classes.

Functions found here:
 * Read from the EEPROM

***************************************************/

#ifndef ROBOTLIB_H
#define ROBOTLIB_H

#include <avr/io.h>

//This should probably be moved to a main class for the whole micro-controller
#ifndef F_CPU
#define F_CPU 16000000
#endif

//Functions
void ReadFromEEMemory(void);
uint8_t ReadEEValue(uint8_t byte);

//Test function
void TestRobot(void);

//StartPositions of servo that are stored in EE
//Variables for holding read data
uint16_t ServoPos[7];

#endif /* ROBOTLIB_H */