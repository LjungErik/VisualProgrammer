/**************************************************
Filename: UARTLib.h
---------------------------------------------------
This header-file contains the definitions of all of
the functions that are needed and used by the UART
on the RobotArm.

Functions found here:
 * Initialize the UART
 * Write to the UART
 * Write Line to the UART
 * Read data from the UART
 * + extra help functions


***************************************************/
#ifndef UARTLIB_H
#define UARTLIB_H

#include <avr/io.h>	
#include "RobotLib.h"

//Init Functions
void InitUART(void);

//Write Function
void Write(char *string);
void WriteLine(char *string);

//Read data function
unsigned char ReadNextByte(void);
void ReadToBuffer(char* buffer, int length);

//Help functions

//Write functions
void WriteChar(char c);
//void WaitUntilReady();

//Read functions
void WaitForReceive(void);

void WriteInt(int16_t value);

#if TEST
//Test function
void TestUART(void);
#endif

//Defined keywords

//UART BUFFER SPECS 
#define UART_BUFFER_SIZE 16 //Size is in bytes

//UART Status
#define UART_PROCESSING 0
#define UART_WAITING 1

//Baud rate

#define BAUD		38400 //High speed - 500 kBaud
#define BAUD_RATE ((F_CPU / (16 * BAUD)) - 1) //Calculate the baud rate

#endif
