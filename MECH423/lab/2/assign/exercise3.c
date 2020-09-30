/*
 *  ======= exercise3 ========
 *
 *  Created on: Sep. 29, 2020
 *  Author:     Frienddo
 */

#include <msp430.h>
#include <exercise.h>

int exercise3(void)
{
  WDTCTL = WDTPW + WDTHOLD;                 // Stop WDT
  CSCTL0_H = 0xA5;                          // unlock registers
  CSCTL2 |= SELS0 + SELS1;                  // Explicitly set SMCLK on DCO (should already be on DCO by default)
  CSCTL1 |= DCOFSEL0 + DCOFSEL1;            // Set max. DCO setting =8MHz
  CSCTL3 |= DIVS0 + DIVS2;                  // Set SMCLK divider to 32 (should already be 32 by default)

  P3DIR |= P3_ALLON;                        // Set to output
  P3OUT &= P3_ALLOFF;                       // Set initial LED

  P4OUT |= BIT0;                            // Configure pullup resistor
  P4DIR &= ~BIT0;                           // Direction = input
  P4REN |= BIT0;                            // Enable pullup resistor
  P4IES &= ~BIT0;                           // P4.0 Lo/Hi edge interrupt
  P4IE = BIT0;                              // P4.0 interrupt enabled
  P4IFG = 0;                                // P4 IFG cleared

  __bis_SR_register(LPM4_bits + GIE);       // Enter LPM4 w/interrupt

  while(1) {
    __no_operation();
  }
}

#pragma vector=PORT4_VECTOR
__interrupt void Port_4(void)
{
  P3OUT ^= LED8;

  P4IFG = 0;                                // P4 IFG cleared
}
