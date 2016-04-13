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

//Define the test to signal that
//test code should be processed
#define TEST 0

//Functions
void ReadFromEEMemory(void);
uint8_t ReadEEValue(uint8_t byte);

#if TEST
//Test function
void TestRobot(void);
#endif

//StartPositions of servo that are stored in EE
//Variables for holding read data
uint16_t ServoPos[7];

#define BOOL uint8_t
#define TRUE 1
#define FALSE 0

//Define the debug to signal that
//debug code should be processed
#define DEBUG 0

#endif /* ROBOTLIB_H */