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
  CSCTL1 |= DCOFSEL0 + DCOFSEL1;            // Set max. DCO setting =8MHz
  CSCTL3 |= DIVS0 + DIVS2;                  // Set SMCLK divider to 32 (should already be 32 by default)

  return 0;
}
