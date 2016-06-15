/**************************************************
Filename: UARTExtended.h
---------------------------------------------------
This header-file contains the definitions of 
the extended functions for the UART.

Functions found here:

***************************************************/
#ifndef UARTEXTEND_H
#define UARTEXTEND_H

#include <avr/io.h>	
#include "UARTLib.h"

//Define key inputs
#define ENTER 0x05
#define SPACE 0x06

//F - keys
#define F1	  0x11
#define F2	  0x12
#define F3    0x13
#define F4    0x14
#define F5    0x15
#define F6    0x16
#define F7    0x17
#define F8    0x18
#define F9    0x19
#define F10   0x1A
#define F11   0x1B
#define F12   0x1C

//Number
#define NUM_0 0x20
#define NUM_1 0x21
#define NUM_2 0x22
#define NUM_3 0x23
#define NUM_4 0x24
#define NUM_5 0x25
#define NUM_6 0x26
#define NUM_7 0x27
#define NUM_8 0x28
#define NUM_9 0x29


#endif