/*
 *  ======= exercise2 ========
 *
 *  Created on: Sep. 29, 2020
 *  Author:     Frienddo
 */

#include <msp430.h>
#include <exercise.h>


int exercise2(void)
{
  // from exercise 1
  WDTCTL = WDTPW + WDTHOLD;                 // Stop WDT
  CSCTL0_H = 0xA5;                          // unlock registers
  CSCTL2 |= SELS0 + SELS1;                  // Explicitly set SMCLK on DCO (should already be on DCO by default)
  CSCTL1 |= DCOFSEL0 + DCOFSEL1;            // Set max. DCO setting =8MHz
  CSCTL3 |= DIVS0 + DIVS2;                  // Explicitly set SMCLK divider to 32 (should already be 32 by default)

  PJDIR |= PJ_ALLON;                        // Set to output
  PJOUT |= PJ_ALLON;                        // Set initial LED
  P3DIR |= P3_ALLON;
  P3OUT |= P3_ALLON;

  // Blink loop
  while(1)
    {
      PJOUT |= PJ_ALLON;                    // Set blink on
      P3OUT |= P3_ALLON;
      __delay_cycles(100000);
      PJOUT &= PJ_INIT;                     // Set blink off
      P3OUT &= P3_INIT;
      __delay_cycles(100000);
    }

  return 0;
}
