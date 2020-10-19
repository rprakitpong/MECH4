/*
 *  ======= exercise5 ========
 *
 *  Created on: Oct. 5, 2020
 *  Author:     Frienddo
 *
 *
 *  1. PWM Waveform Generation
Set up a timer to generate a 3 kHz square wave with a 33% duty cycle.
Output this waveform on one of the timer ports.
Measure waveform on the timer port using the AD2 oscilloscope. Record a video showing the waveform measurement, with the measured frequency clearly displayed, and upload to Canvas.
 */

#include <msp430.h>

/*
 * ======== main ========
 */
int main(void)
{
    WDTCTL = WDTPW + WDTHOLD;                 // Stop WDT
    CSCTL0_H = 0xA5;                          // unlock registers
    CSCTL2 |= SELS0 + SELS1; // Explicitly set SMCLK on DCO (should already be on DCO by default)
    CSCTL1 |= DCOFSEL0 + DCOFSEL1;            // Set DCO =8MHz
    CSCTL3 |= DIVS0 + DIVS2; // Set SMCLK divider to 32 (should already be 32 by default)

    P1DIR |= BIT0 + BIT1;                     // Set P1.0 & P1.1 to out
    TB0CTL |= TASSEL_2 + MC_1 + TBIE;         // SMCLK, count mode, interrupt on
    TB0CCTL0 = CCIE;                          // TACCR0 interrupt enabled
    TB0CCR0 = 28;
    TB0CTL &= ~TBIFG;                         // Clear IFG

    __bis_SR_register(LPM4_bits + GIE);       // enable interrupts
    while (1)
    {
        __no_operation();
    }

    return 0;
}
int counter = 0;

#pragma vector=TIMER0_B0_VECTOR
__interrupt void Timer_B_0(void)
{
    if (counter == 0)
    {
        P1OUT |= BIT0;
        P1OUT |= BIT1;
    }
    if (counter == 1)
    {
        P1OUT &= ~BIT0;
    }
    counter = counter + 1;
    if (counter == 3)
    {
        counter = 0;
    }

    TB0CTL &= ~TBIFG;                         // Clear IFG
}

