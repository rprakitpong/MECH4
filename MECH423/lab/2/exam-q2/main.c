/*
 *  ======= exercise3 ========
 *
 *  Created on: Sep. 29, 2020
 *  Author:     Frienddo
 *
 *
 *  2. Push Button LED Cycling
Configure P4.0 as a digital input
The switch S1 is connected to P4.0 on the EXP board. Enable the internal pull-up resistors for the switch.
Set P4.0 to interrupt on a falling edge (i.e. an interrupt occurs when the user presses the button). Enable local and global interrupts.
Write code to cycle through the LEDs connected to PJ.0, PJ.1, PJ.2, PJ.3, P3.4, P3.5, P3.6, and P3.7 each time the button is pressed. Specifically, each time the button is pressed, the current LED turns off and the next LED turns on. (Hint: use a state variable)
Record a video showing this behavior and upload to Canvas.
 */

#include <msp430.h>

#define LED1 BIT0
#define LED2 BIT1
#define LED3 BIT2
#define LED4 BIT3
#define LED5 BIT4
#define LED6 BIT5
#define LED7 BIT6
#define LED8 BIT7

#define PJ_INIT (LED1 + LED2)   // 0011
#define PJ_ALLON (LED1 + LED2 + LED3 + LED4) // bits 0-3
#define PJ_ALLOFF 0
#define P3_INIT (LED5 + LED8)   // 1001
#define P3_ALLON (LED5 + LED6 + LED7 + LED8) // bits 4-7
#define P3_ALLOFF 0

int main(void)
{
    WDTCTL = WDTPW + WDTHOLD;                 // Stop WDT
    CSCTL0_H = 0xA5;                          // unlock registers
    CSCTL2 |= SELS0 + SELS1; // Explicitly set SMCLK on DCO (should already be on DCO by default)
    CSCTL1 |= DCOFSEL0 + DCOFSEL1;            // Set max. DCO setting =8MHz
    CSCTL3 |= DIVS0 + DIVS2; // Set SMCLK divider to 32 (should already be 32 by default)

    PJDIR |= PJ_ALLON;                        // Set to output
    PJOUT &= ~PJ_ALLON;                        // Set initial LED
    P3DIR |= P3_ALLON;
    P3OUT &= ~P3_ALLON;

    P4OUT |= BIT0;                            // Configure pullup resistor
    P4DIR &= ~BIT0;                           // Direction = input
    P4REN |= BIT0;                            // Enable pullup resistor
    P4IES |= BIT0;                           // P4.0 hi/lo edge interrupt (falling edge)
    P4IE = BIT0;                              // P4.0 interrupt on
    P4IFG = 0;                                // P4 IFG cleared

    __bis_SR_register(LPM4_bits + GIE);       // Enter LPM4 w/interrupt

    while (1)
    {
        __no_operation();
    }
}

int temp = 0;

#pragma vector=PORT4_VECTOR
__interrupt void Port_4(void)
{
    int LED[8] = { BIT0, BIT1, BIT2, BIT3, BIT4, BIT5, BIT6, BIT7 };
    PJOUT &= ~PJ_ALLON;                        // Set initial LED
    P3OUT &= ~P3_ALLON;

    if (temp > 3)
    {
        P3OUT |= LED[temp];
    }
    else
    {
        PJOUT |= LED[temp];
    }

    temp = temp + 1;
    if (temp > 7)
    {
        temp = 0;
    }

    P4IFG = 0;                                // P4 IFG cleared
}
