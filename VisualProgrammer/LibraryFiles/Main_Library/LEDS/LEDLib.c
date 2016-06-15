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

#include "LEDLib.h"
#include <util/delay.h>

//Initialize LEDs
void InitLEDs(void)
{
	DDRG = INIT_LED_DDR;
}

//Set LEDs
void SetLED1(uint8_t power)
{
	LEDSPORT &= ~(LED1);
	LEDSPORT |= power > 0 ? LED1 : 0;
}

void SetLED2(uint8_t power)
{
	LEDSPORT &= ~(LED2);
	LEDSPORT |= power > 0 ? LED2 : 0;
}

void SetLED3(uint8_t power)
{
	LEDSPORT &= ~(LED3);
	LEDSPORT |= power > 0 ? LED3 : 0;
}

void SetLED4(uint8_t power)
{
	LEDSPORT &= ~(LED4);
	LEDSPORT |= power > 0 ? LED4 : 0;
}

#if TEST
/* ----------------------------------------------------- */
// Holds the test function for the class (test the functionality)
/* ----------------------------------------------------- */
void TestLED(void)
{
	InitLEDs();

	SetLED1(ON);
	_delay_ms(500);
		
	SetLED1(OFF);
	SetLED2(ON);
	_delay_ms(500);
		
	SetLED2(OFF);
	SetLED3(ON);
	_delay_ms(500);
		
	SetLED3(OFF);
	SetLED4(ON);
	_delay_ms(500);
		
	SetLED4(OFF);
}
/* ----------------------------------------------------- */
#endif