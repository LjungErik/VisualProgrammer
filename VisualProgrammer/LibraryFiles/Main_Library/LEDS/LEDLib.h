/**************************************************
Filename: LEDLib.h
---------------------------------------------------
This header-file contains the definitions of all of
the functions that are needed and used when controlling
the on board-LEDs

Functions found here:
 * Initialize the LEDs
 * Set the LEDs power High/Low (ON/OFF)
 * + extra help functions

***************************************************/
//This should probably be moved to a main class for the whole micro-controller

#ifndef LEDLIB_H
#define LEDLIB_H

#include <avr/io.h>
#include "RobotLib.h"

//Define the output channel for the LEDs
#define INIT_LED_DDR 0b00001111

//Define the LEDs main port
#define LEDSPORT PORTG

//Define names for the different led ports
#define LED1 (1<<PING0)
#define LED2 (1<<PING1)
#define LED3 (1<<PING2)
#define LED4 (1<<PING3)

//Define the states the LEDs can be in
#define HIGH 1
#define LOW  0
//Make the states clearer for clients
#define ON HIGH
#define OFF LOW

//Initialize function
void InitLEDs(void);

//Set LEDs Power
void SetLED1(uint8_t power);
void SetLED2(uint8_t power);
void SetLED3(uint8_t power);
void SetLED4(uint8_t power);

#if TEST
//Test function
void TestLED(void);
#endif

#endif /* LEDLIB_H */