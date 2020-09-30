/*
 *  ======= exercise1 ========
 *
 *  Created on: Sep. 29, 2020
 *  Author:     Frienddo
 */

#include <msp430.h>
#include <exercise.h>

int exercise1(void)
{
  WDTCTL = WDTPW + WDTHOLD;                 // Stop WDT
  CSCTL0_H = 0xA5;                          // unlock registers
  CSCTL2 |= SELS0 + SELS1;                  // Explicitly set SMCLK on DCO (should already be on DCO by default)
  CSCTL1 |= DCOFSEL0 + DCOFSEL1;            // Set DCO =8MHz
  CSCTL3 |= DIVS0 + DIVS2;                  // Set SMCLK divider to 32 (should already be 32 by default)

  P3DIR |= BIT4;                            // Set P3.4 to out
  TA0CTL |= TASSEL_2 + MC_1 + TAIE;         // SMCLK, count mode, interrupt on
  TA0CCTL0 = CCIE;                          // TACCR0 interrupt enabled
  TA0CCR0 = 1;                              // count up to 1 = send signal every tick
  TA0CTL &= ~TAIFG;                         // Clear IFG

  __bis_SR_register(LPM4_bits + GIE);       // enable interrupts
  while(1) {
    __no_operation();
  }

  return 0;
}

#pragma vector=TIMER0_A0_VECTOR
__interrupt void Timer_A(void)
{
  P3OUT ^= BIT4;                            // Toggle output
  TA0CTL &= ~TAIFG;                         // Clear IFG
}
