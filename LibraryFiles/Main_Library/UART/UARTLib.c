/**************************************************
Filename: UARTLib.c
---------------------------------------------------
This c-file contains the code of all the functions
in UARTLib.h.

Functions found here:
 * Initialize the UART
 * Write to the UART
 * Write Line to the UART
 * Read data from the UART
 * + extra help functions

USART nr 1 is used here for communication

***************************************************/
#include <avr/interrupt.h>
#include <util/delay.h>
#include <stdlib.h>
#include "UARTLib.h"


//Variable
volatile unsigned char uart_buffer[UART_BUFFER_SIZE];
volatile uint8_t uart_producer_count = 0;
volatile uint8_t uart_producer_pos = 0;
volatile uint8_t uart_consumer_pos = 0;
volatile uint8_t uart_status;

//Initialize UART
void InitUART(void) 
{
	//Set the baud rate
	UBRR1H = (unsigned char) (BAUD_RATE >> 8);
	UBRR1L = (unsigned char) BAUD_RATE;
	
	//Enable receive and transmit
	UCSR1B = (1 << RXEN1) | (1 << TXEN1);
	
	//Initialize receive interrupt (Interrupt execution when data is being received)
	UCSR1B |= (1 << RXCIE1); 
	
	//Set the receiver and transmit bits per frame (5-9bits)
	UCSR1C = (1 << UCSZ11) | (1 << UCSZ10); //Set TX, RX to 8 bit
	
	uart_status = UART_WAITING;
	
	sei();
	
}

//Write function
void Write(char* string)
{
	while (*string)
		WriteChar(*string++);
}


void WriteLine(char *string)
{
	Write(string);
	WriteChar('\n');
}

//Interrupt vector for handling the receive of bytes
//TODO: FIX THIS CODE SO THAT IT DOES NOT DISCARD OVERFLOWING
//BYTES
/* ----------------------------------------------------- */
ISR(USART1_RX_vect) 
{
	volatile uint8_t receivedByte = UDR1;
	//Check if data can be added to the buffer
	if(uart_producer_count < UART_BUFFER_SIZE)
	{
		uart_buffer[uart_producer_pos] = receivedByte;
		//Increase the count of produced values
		uart_producer_count++;
		//Calculate the next position
		uart_producer_pos = (uart_producer_pos + 1) % UART_BUFFER_SIZE;
		//Signal that data has been received
		uart_status = UART_PROCESSING;
	}
}
/* ----------------------------------------------------- */

//Read Function
unsigned char ReadNextByte(void)
{
	if(uart_status == UART_PROCESSING && uart_producer_count > 0)
	{
		uint8_t ret = uart_buffer[uart_consumer_pos];
		//Update consumer position
		uart_consumer_pos = (uart_consumer_pos + 1) % UART_BUFFER_SIZE;
		//Allow one more byte to be produced
		uart_producer_count--;
		
		//Check if last bit of data was produced
		if(uart_producer_count == 0)
			uart_status = UART_WAITING;
		
		return ret;
	}
	
	return 0;
}

void ReadToBuffer(char* buffer, int length)
{
	for(int i = 0; i < length || (uart_status == UART_PROCESSING && uart_producer_count > 0); i++) 
	{
		buffer[i] = ReadNextByte();
	}
}

//Help functions

//Write function
void WriteChar(char c) 
{
	//Polling until transmitter is ready
	while(!(UCSR1A & (1<<UDRE1)));
	//Assign UDR0 the data to be sent
	UDR1 = (uint8_t) c;
}

//void WaitUntilReady()
//{
//
//}

//Read function

void WaitForReceive(void)
{
	while(uart_status == UART_WAITING);
}

void WriteInt(uint16_t value)
{
	char buffer[17];
	itoa(value, buffer, 10);
	Write(buffer);
}

/* ----------------------------------------------------- */
// Holds the test function for the class (test the functionality)
/* ----------------------------------------------------- */
void TestUART(void)
{
	InitUART();

	//Try to write
	Write("Hello World!");
	WriteLine(" - This is a line");
	_delay_ms(1500);

	Write("Number: ");
	WriteInt(1);
	WriteLine("");

	//Try to recieve data from the chip
	//(perform a ECO)
	WaitForReceive();
	_delay_ms(100);
	unsigned char data;
	while ((data = ReadNextByte()) != 0) 
	{
		WriteChar(data);
	}
	_delay_ms(1000);
}
/* ----------------------------------------------------- */
